﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtoCash.Data;
using AtoCash.Models;
using EmailService;
using Microsoft.AspNetCore.Authorization;
using AtoCash.Authentication;

namespace AtoCash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Finmgr, Admin, User")]
    public class TravelApprovalRequestsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;
        private readonly IEmailSender _emailSender;

        public TravelApprovalRequestsController(AtoCashDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: api/TravelApprovalRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelApprovalRequestDTO>>> GetTravelApprovalRequest()
        {
            List<TravelApprovalRequestDTO> ListTravelRequestDTO = new List<TravelApprovalRequestDTO>();

            var travelrequests = await _context.TravelApprovalRequests.ToListAsync();

            foreach (TravelApprovalRequest travelrequest in travelrequests)
            {
                TravelApprovalRequestDTO travelApprovalRequestDTO = new TravelApprovalRequestDTO
                {
                    Id = travelrequest.Id,
                    EmployeeId = travelrequest.EmployeeId,
                    TravelStartDate = travelrequest.TravelStartDate,
                    TravelEndDate = travelrequest.TravelEndDate,
                    TravelPurpose = travelrequest.TravelPurpose,
                    CurrentStatus = travelrequest.CurrentStatus
                };

                ListTravelRequestDTO.Add(travelApprovalRequestDTO);

            }

            return ListTravelRequestDTO;
        }

        // GET: api/TravelApprovalRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TravelApprovalRequestDTO>> GetTravelApprovalRequest(int id)
        {
            var travelrequest = await _context.TravelApprovalRequests.FindAsync(id);

            TravelApprovalRequestDTO travelApprovalRequestDTO = new TravelApprovalRequestDTO
            {
                Id = travelrequest.Id,
                EmployeeId = travelrequest.EmployeeId,
                TravelStartDate = travelrequest.TravelStartDate,
                TravelEndDate = travelrequest.TravelEndDate,
                TravelPurpose = travelrequest.TravelPurpose,
                ReqRaisedDate = travelrequest.ReqRaisedDate,
                CurrentStatus = travelrequest.CurrentStatus
            };


            if (travelrequest == null)
            {
                return NoContent();
            }
            return travelApprovalRequestDTO;
        }

        // PUT: api/TravelApprovalRequests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<IActionResult> PutTravelApprovalRequest(int id, TravelApprovalRequestDTO travelApprovalRequestDto)
        {
            if (id != travelApprovalRequestDto.Id)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Id state is invalid" });
            }

            var travelApprovalRequest = await _context.TravelApprovalRequests.FindAsync(id);

            travelApprovalRequest.Id = travelApprovalRequestDto.Id;
            travelApprovalRequest.EmployeeId = travelApprovalRequestDto.EmployeeId;
            travelApprovalRequest.TravelStartDate = travelApprovalRequestDto.TravelStartDate;
            travelApprovalRequest.TravelEndDate = travelApprovalRequestDto.TravelEndDate;
            travelApprovalRequest.TravelPurpose = travelApprovalRequestDto.TravelPurpose;
            travelApprovalRequest.ReqRaisedDate = travelApprovalRequestDto.ReqRaisedDate;
            travelApprovalRequest.CurrentStatus = travelApprovalRequestDto.CurrentStatus;

            _context.TravelApprovalRequests.Update(travelApprovalRequest);
            //_context.Entry(projectDto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TravelApprovalRequestExists(id))
                {
                    return NoContent();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        /// <summary>
        /// Step 1 : enter a record in TravelApprovalRequest table
        /// Step 2: Enter a Record in Travel Approval Request Tracker (table)
        /// Step 3: Send email to approvers after adding them in TravelApprovalStatusTracker
        /// </summary>
        /// <param name="travelApprovalRequestDto"></param>
        /// <returns></returns>


        // POST: api/TravelApprovalRequests
        [HttpPost]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<ActionResult<TravelApprovalRequest>> PostTravelApprovalRequest(TravelApprovalRequestDTO travelApprovalRequestDto)
        {
            /// Step 1 : enter a record in TravelApprovalRequest table

            TravelApprovalRequest travelApprovalRequest = new TravelApprovalRequest
            {
                EmployeeId = travelApprovalRequestDto.EmployeeId,
                TravelStartDate = travelApprovalRequestDto.TravelStartDate,
                TravelEndDate = travelApprovalRequestDto.TravelEndDate,
                TravelPurpose = travelApprovalRequestDto.TravelPurpose,
                ReqRaisedDate = travelApprovalRequestDto.ReqRaisedDate,
                CurrentStatus = travelApprovalRequestDto.CurrentStatus,
                DepartmentId = _context.Employees.Find(travelApprovalRequestDto.EmployeeId).DepartmentId,
                ProjectId = travelApprovalRequestDto.ProjectId,
                SubProjectId = travelApprovalRequestDto.SubProjectId,
                WorkTaskId = travelApprovalRequestDto.WorkTaskId
            };

            _context.TravelApprovalRequests.Add(travelApprovalRequest);
            await _context.SaveChangesAsync();

            //Check if the Requestor is raising Travel reques on behalf of Project (Or) department
            //Process the travel request appropriately
            if (travelApprovalRequestDto.ProjectId == null)
            {
                await Task.Run(() => ProjectTravelRequest(travelApprovalRequestDto));
            }
            else
            {
                await Task.Run(() => DepartmentTravelRequest(travelApprovalRequestDto));
            }

           

            return CreatedAtAction("GetGetTravelApprovalRequest", new { id = travelApprovalRequest.Id }, travelApprovalRequest);
        }

        private async Task DepartmentTravelRequest(TravelApprovalRequestDTO travelApprovalRequestDto)
        {


            //Add oned entry to the TravelApprovalRequestTracker for Department based Request
            //Here project ID will be null

            int empid = travelApprovalRequestDto.EmployeeId;
            int empApprGroupId = _context.Employees.Find(empid).ApprovalGroupId;
            int costCentre = _context.Projects.Find(travelApprovalRequestDto.ProjectId).CostCentreId;
            int projManagerid = _context.ProjectManagements.Find(travelApprovalRequestDto.ProjectId).EmployeeId;
            var getEmpApproversAllLevels = _context.ApprovalRoleMaps.Where(a => a.ApprovalGroupId == empApprGroupId).ToList().OrderBy(a => a.ApprovalLevel);

            foreach (ApprovalRoleMap ApprMap in getEmpApproversAllLevels)
            {

                int role_id = ApprMap.RoleId;
                var approver = _context.Employees.Where(e => e.RoleId == role_id).FirstOrDefault();

                /// Step 2: Enter a Record in Travel Approval Request Tracker (table)

                _context.TravelApprovalStatusTrackers.Add(new TravelApprovalStatusTracker
                {
                    EmployeeId = travelApprovalRequestDto.EmployeeId,
                    TravelApprovalRequestId = travelApprovalRequestDto.Id,
                    DepartmentId = approver.DepartmentId,
                    ProjectId = null,
                    RoleId = approver.RoleId,
                    ReqDate = DateTime.Now,
                    FinalApprovedDate = null,
                    ApprovalStatusTypeId = (int)ApprovalStatus.Pending //1-Pending, 2-Approved, 3-Rejected
                });

                //##### 3. Send email to the Approver
                //Multiple instance for Department
                //####################################

                var approverMailAddress = approver.Email;
                string subject = "Expense Claim Approval Request " + travelApprovalRequestDto.Id.ToString();
                Employee emp = _context.Employees.Find(travelApprovalRequestDto.EmployeeId);
                var expenseReimClaimReq = _context.ExpenseReimburseRequests.Find(travelApprovalRequestDto.Id);
                string content = "Travel Request Approval sought by " + emp.FirstName + "/nfor the purpose of " + travelApprovalRequestDto.TravelPurpose;
                var messagemail = new Message(new string[] { approverMailAddress }, subject, content);

               await _emailSender.SendEmailAsync(messagemail);

                //repeat for each approver


            }

        }


        private async Task ProjectTravelRequest(TravelApprovalRequestDTO travelApprovalRequestDto)
        {
            //Add oned entry to the TravelApprovalRequestTracker for PROJECT based Request
            //Here Department ID will be null
            //###### 2. add entry in TravelApprovalStatus tracker
            //int costCentre = _context.Projects.Find(travelApprovalRequestDto.ProjectId).CostCentreId;

            int projManagerid = _context.ProjectManagements.Find(travelApprovalRequestDto.ProjectId).EmployeeId;

            var approver = _context.Employees.Find(projManagerid);

            _context.TravelApprovalStatusTrackers.Add(new TravelApprovalStatusTracker()
            {
                EmployeeId = travelApprovalRequestDto.EmployeeId,
                TravelApprovalRequestId = travelApprovalRequestDto.Id,
                DepartmentId = null,
                ProjectId = travelApprovalRequestDto.ProjectId.Value,
                RoleId = approver.RoleId,
                ReqDate = DateTime.Now,
                FinalApprovedDate = null,
                ApprovalStatusTypeId = (int)ApprovalStatus.Pending //1-Pending, 2-Approved, 3-Rejected
            });

            //##### 3. Send email to the Approver
            //Single instance for Project
            //####################################

            var approverMailAddress = approver.Email;
            string subject = "Expense Claim Approval Request " + travelApprovalRequestDto.Id.ToString();
            Employee emp = _context.Employees.Find(travelApprovalRequestDto.EmployeeId);
            //var expenseReimClaimReq = _context.ExpenseReimburseRequests.Find(travelApprovalRequestDto.Id);
            string content = "Travel Request Approval sought by " + emp.FirstName + "/nfor the purpose of " + travelApprovalRequestDto.TravelPurpose;
            var messagemail = new Message(new string[] { approverMailAddress }, subject, content);

           await _emailSender.SendEmailAsync(messagemail);

        }

        // DELETE: api/TravelApprovalRequests/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<IActionResult> DeleteTravelApprovalRequest(int id)
        {
            var travelApprovalRequest = await _context.TravelApprovalRequests.FindAsync(id);
            if (travelApprovalRequest == null)
            {
                return NoContent();
            }

            _context.TravelApprovalRequests.Remove(travelApprovalRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TravelApprovalRequestExists(int id)
        {
            return _context.TravelApprovalRequests.Any(e => e.Id == id);
        }



        private enum ApprovalStatus
        {
            Pending = 1,
            Approved,
            Rejected

        }

    }
}
