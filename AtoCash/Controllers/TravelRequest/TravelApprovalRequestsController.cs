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
using AtoCash.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace AtoCash.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr, User")]


    public class TravelApprovalRequestsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;
        private readonly IEmailSender _emailSender;

        public TravelApprovalRequestsController(AtoCashDbContext context, IEmailSender emailSender)
        {
            this._context = context;
            this._emailSender = emailSender;
        }


        // GET: api/TravelApprovalRequests
        [HttpGet]
        [ActionName("GetTravelApprovalRequests")]
        public async Task<ActionResult<IEnumerable<TravelApprovalRequestDTO>>> GetTravelApprovalRequests()
        {
            List<TravelApprovalRequestDTO> ListTravelApprovalRequestDTO = new();

            //var claimApprovalStatusTracker = await _context.TravelApprovalRequests.FindAsync(1);

            var TravelApprovalRequests = await _context.TravelApprovalRequests.ToListAsync();

            foreach (TravelApprovalRequest travelApprovalRequest in TravelApprovalRequests)
            {
                TravelApprovalRequestDTO travelApprovalRequestDTO = new();

                travelApprovalRequestDTO.Id = travelApprovalRequest.Id;
                travelApprovalRequestDTO.EmployeeId = travelApprovalRequest.EmployeeId;
                travelApprovalRequestDTO.EmployeeName = _context.Employees.Find(travelApprovalRequest.EmployeeId).GetFullName();
                travelApprovalRequestDTO.TravelStartDate = travelApprovalRequest.TravelStartDate;
                travelApprovalRequestDTO.TravelEndDate = travelApprovalRequest.TravelEndDate;
                travelApprovalRequestDTO.TravelPurpose = travelApprovalRequest.TravelPurpose;
                travelApprovalRequestDTO.ReqRaisedDate = travelApprovalRequest.ReqRaisedDate;
                travelApprovalRequestDTO.DepartmentId = travelApprovalRequest.DepartmentId;
                travelApprovalRequestDTO.Department = travelApprovalRequest.DepartmentId != null ? _context.Departments.Find(travelApprovalRequest.DepartmentId).DeptCode : null;
                travelApprovalRequestDTO.ProjectId = travelApprovalRequest.ProjectId;
                travelApprovalRequestDTO.Project = travelApprovalRequest.ProjectId != null ? _context.Projects.Find(travelApprovalRequest.ProjectId).ProjectName : null;
                travelApprovalRequestDTO.SubProjectId = travelApprovalRequest.SubProjectId;
                travelApprovalRequestDTO.SubProject = travelApprovalRequest.SubProjectId != null ? _context.SubProjects.Find(travelApprovalRequest.SubProjectId).SubProjectName : null;
                travelApprovalRequestDTO.WorkTaskId = travelApprovalRequest.WorkTaskId;
                travelApprovalRequestDTO.WorkTask = travelApprovalRequest.WorkTaskId != null ? _context.WorkTasks.Find(travelApprovalRequest.WorkTaskId).TaskName : null;
                travelApprovalRequestDTO.ApprovalStatusTypeId = travelApprovalRequest.ApprovalStatusTypeId;
                travelApprovalRequestDTO.ApprovalStatusType = _context.ApprovalStatusTypes.Find(travelApprovalRequest.ApprovalStatusTypeId).Status;
                travelApprovalRequestDTO.ApprovedDate = travelApprovalRequest.ApprovedDate;


                ListTravelApprovalRequestDTO.Add(travelApprovalRequestDTO);
            }

            return ListTravelApprovalRequestDTO.OrderByDescending(o => o.ReqRaisedDate).ToList();
        }



        // GET: api/TravelApprovalRequests/5
        [HttpGet("{id}")]
        [ActionName("GetTravelApprovalRequest")]
        public async Task<ActionResult<TravelApprovalRequestDTO>> GetTravelApprovalRequest(int id)
        {


            var travelApprovalRequest = await _context.TravelApprovalRequests.FindAsync(id);

            if (travelApprovalRequest == null)
            {
                return NoContent();
            }
            TravelApprovalRequestDTO travelApprovalRequestDTO = new();


            travelApprovalRequestDTO.Id = travelApprovalRequest.Id;
            travelApprovalRequestDTO.EmployeeId = travelApprovalRequest.EmployeeId;
            travelApprovalRequestDTO.EmployeeName = _context.Employees.Find(travelApprovalRequest.EmployeeId).GetFullName();
            travelApprovalRequestDTO.TravelStartDate = travelApprovalRequest.TravelStartDate;
            travelApprovalRequestDTO.TravelEndDate = travelApprovalRequest.TravelEndDate;
            travelApprovalRequestDTO.TravelPurpose = travelApprovalRequest.TravelPurpose;
            travelApprovalRequestDTO.ReqRaisedDate = travelApprovalRequest.ReqRaisedDate;
            travelApprovalRequestDTO.DepartmentId = travelApprovalRequest.DepartmentId;
            travelApprovalRequestDTO.Department = travelApprovalRequest.DepartmentId != null ? _context.Departments.Find(travelApprovalRequest.DepartmentId).DeptCode : null;
            travelApprovalRequestDTO.ProjectId = travelApprovalRequest.ProjectId;
            travelApprovalRequestDTO.Project = travelApprovalRequest.ProjectId != null ? _context.Projects.Find(travelApprovalRequest.ProjectId).ProjectName : null;
            travelApprovalRequestDTO.SubProjectId = travelApprovalRequest.SubProjectId;
            travelApprovalRequestDTO.SubProject = travelApprovalRequest.SubProjectId != null ? _context.SubProjects.Find(travelApprovalRequest.SubProjectId).SubProjectName : null;
            travelApprovalRequestDTO.WorkTaskId = travelApprovalRequest.WorkTaskId;
            travelApprovalRequestDTO.WorkTask = travelApprovalRequest.WorkTaskId != null ? _context.WorkTasks.Find(travelApprovalRequest.WorkTaskId).TaskName : null;
            travelApprovalRequestDTO.ApprovalStatusTypeId = travelApprovalRequest.ApprovalStatusTypeId;
            travelApprovalRequestDTO.ApprovalStatusType = _context.ApprovalStatusTypes.Find(travelApprovalRequest.ApprovalStatusTypeId).Status;
            travelApprovalRequestDTO.ApprovedDate = travelApprovalRequest.ApprovedDate;

            return travelApprovalRequestDTO;
        }





        [HttpGet("{id}")]
        [ActionName("GetTravelApprovalRequestRaisedForEmployee")]
        public async Task<ActionResult<IEnumerable<TravelApprovalRequestDTO>>> GetTravelApprovalRequestRaisedForEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NoContent();
            }

            //get the employee's approval level for comparison with approver level  to decide "ShowEditDelete" bool
            int reqEmpApprLevelId = _context.ApprovalRoleMaps.Where(a => a.RoleId == _context.Employees.Find(id).RoleId).FirstOrDefault().ApprovalLevelId;
            int reqEmpApprLevel = _context.ApprovalLevels.Find(reqEmpApprLevelId).Level;


            var TravelApprovalRequests = await _context.TravelApprovalRequests.Where(p => p.EmployeeId == id).ToListAsync();

            if (TravelApprovalRequests == null)
            {
                return NoContent();
            }

            List<TravelApprovalRequestDTO> TravelApprovalRequestDTOs = new();

            foreach (var travelApprovalRequest in TravelApprovalRequests)
            {
                TravelApprovalRequestDTO travelApprovalRequestDTO = new();

                travelApprovalRequestDTO.Id = travelApprovalRequest.Id;
                travelApprovalRequestDTO.EmployeeId = travelApprovalRequest.EmployeeId;
                travelApprovalRequestDTO.EmployeeName = _context.Employees.Find(travelApprovalRequest.EmployeeId).GetFullName();
                travelApprovalRequestDTO.TravelStartDate = travelApprovalRequest.TravelStartDate;
                travelApprovalRequestDTO.TravelEndDate = travelApprovalRequest.TravelEndDate;
                travelApprovalRequestDTO.TravelPurpose = travelApprovalRequest.TravelPurpose;
                travelApprovalRequestDTO.ReqRaisedDate = travelApprovalRequest.ReqRaisedDate;
                travelApprovalRequestDTO.DepartmentId = travelApprovalRequest.DepartmentId;
                travelApprovalRequestDTO.Department = travelApprovalRequest.DepartmentId != null ? _context.Departments.Find(travelApprovalRequest.DepartmentId).DeptCode : null;
                travelApprovalRequestDTO.ProjectId = travelApprovalRequest.ProjectId;
                travelApprovalRequestDTO.Project = travelApprovalRequest.ProjectId != null ? _context.Projects.Find(travelApprovalRequest.ProjectId).ProjectName : null;
                travelApprovalRequestDTO.SubProjectId = travelApprovalRequest.SubProjectId;
                travelApprovalRequestDTO.SubProject = travelApprovalRequest.SubProjectId != null ? _context.SubProjects.Find(travelApprovalRequest.SubProjectId).SubProjectName : null;
                travelApprovalRequestDTO.WorkTaskId = travelApprovalRequest.WorkTaskId;
                travelApprovalRequestDTO.WorkTask = travelApprovalRequest.WorkTaskId != null ? _context.WorkTasks.Find(travelApprovalRequest.WorkTaskId).TaskName : null;
                travelApprovalRequestDTO.ApprovalStatusTypeId = travelApprovalRequest.ApprovalStatusTypeId;
                travelApprovalRequestDTO.ApprovalStatusType = _context.ApprovalStatusTypes.Find(travelApprovalRequest.ApprovalStatusTypeId).Status;
                travelApprovalRequestDTO.ApprovedDate = travelApprovalRequest.ApprovedDate;

                int NextApproverInPending = _context.TravelApprovalStatusTrackers.Where(t =>
                        t.ApprovalStatusTypeId == (int)EApprovalStatus.Pending &&
                        t.TravelApprovalRequestId == travelApprovalRequest.Id).Select( s=> s.ApprovalLevel.Level).FirstOrDefault();

                //set the bookean flat to TRUE if No approver has yet approved the Request else FALSE
                    travelApprovalRequestDTO.ShowEditDelete = reqEmpApprLevel + 1 == NextApproverInPending ? true : false;

                //;

                TravelApprovalRequestDTOs.Add(travelApprovalRequestDTO);
            }


            return Ok(TravelApprovalRequestDTOs.OrderByDescending(o => o.ReqRaisedDate).ToList());
        }


        [HttpGet("{id}")]
        [ActionName("CountAllTravelRequestRaisedByEmployee")]
        public async Task<ActionResult> CountAllTravelRequestRaisedByEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return Ok(0);
            }

            var travelApprovalRequests = await _context.TravelApprovalRequests.Where(p => p.EmployeeId == id).ToListAsync();

            if (travelApprovalRequests == null)
            {
                return Ok(0);
            }

            int TotalCount = _context.TravelApprovalRequests.Where(c => c.EmployeeId == id).Count();
            int PendingCount = _context.TravelApprovalRequests.Where(c => c.EmployeeId == id && c.ApprovalStatusTypeId == (int)EApprovalStatus.Pending).Count();
            int RejectedCount = _context.TravelApprovalRequests.Where(c => c.EmployeeId == id && c.ApprovalStatusTypeId == (int)EApprovalStatus.Rejected).Count();
            int ApprovedCount = _context.TravelApprovalRequests.Where(c => c.EmployeeId == id && c.ApprovalStatusTypeId == (int)EApprovalStatus.Approved).Count();

            return Ok(new { TotalCount, PendingCount, RejectedCount, ApprovedCount });
        }


        [HttpGet]
        [ActionName("GetTravelReqInPendingForAll")]
        public async Task<ActionResult<int>> GetTravelReqInPendingForAll()
        {
            //debug
            var TravelApprovalRequests = await _context.TravelApprovalRequests.Include("TravelApprovalStatusTrackers").ToListAsync();


            //var TravelApprovalRequests = await _context.TravelApprovalRequests.Where(c => c.ApprovalStatusTypeId == ApprovalStatus.Pending).select( );

            if (TravelApprovalRequests == null)
            {
                return NoContent();
            }

            return Ok(TravelApprovalRequests.Count);
        }










        // PUT: api/TravelApprovalRequests/5
        [HttpPut("{id}")]
        [ActionName("PutTravelApprovalRequest")]

        public async Task<IActionResult> PutTravelApprovalRequest(int id, TravelApprovalRequestDTO travelApprovalRequestDTO)
        {
            if (id != travelApprovalRequestDTO.Id)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Id is invalid" });
            }

            var travelApprovalRequest = await _context.TravelApprovalRequests.FindAsync(id);
            travelApprovalRequest.Id = travelApprovalRequestDTO.Id;
            travelApprovalRequest.EmployeeId = travelApprovalRequestDTO.EmployeeId;
            travelApprovalRequest.TravelStartDate = travelApprovalRequestDTO.TravelStartDate;
            travelApprovalRequest.TravelEndDate = travelApprovalRequestDTO.TravelEndDate;
            travelApprovalRequest.TravelPurpose = travelApprovalRequestDTO.TravelPurpose;
            travelApprovalRequest.ReqRaisedDate = travelApprovalRequestDTO.ReqRaisedDate;
            travelApprovalRequest.DepartmentId = travelApprovalRequestDTO.DepartmentId;
            travelApprovalRequest.ProjectId = travelApprovalRequestDTO.ProjectId;
            travelApprovalRequest.SubProjectId = travelApprovalRequestDTO.SubProjectId;
            travelApprovalRequest.WorkTaskId = travelApprovalRequestDTO.WorkTaskId;
            travelApprovalRequest.ApprovalStatusTypeId = travelApprovalRequestDTO.ApprovalStatusTypeId;

            _context.TravelApprovalRequests.Update(travelApprovalRequest);
            //_context.Entry(travelApprovalRequest).State = EntityState.Modified;

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

        // POST: api/TravelApprovalRequests
        [HttpPost]
        [ActionName("PostTravelApprovalRequest")]
        public async Task<ActionResult<TravelApprovalRequest>> PostTravelApprovalRequest(TravelApprovalRequestDTO travelApprovalRequestDTO)
        {
            //Step ##1

            var dupReq = _context.TravelApprovalRequests.Where(
                t => t.TravelStartDate.Date == travelApprovalRequestDTO.TravelStartDate.Date &&
                t.TravelEndDate.Date == travelApprovalRequestDTO.TravelEndDate.Date &&
                t.TravelPurpose == travelApprovalRequestDTO.TravelPurpose).Count();

            if (dupReq != 0)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Duplicate request cannot be created" });
            }




            //##Step 2

            if (travelApprovalRequestDTO.ProjectId != null)
            {
                //Goes to Option 1 (Project)
                await Task.Run(() => ProjectTravelRequest(travelApprovalRequestDTO));
            }
            else
            {
                //Goes to Option 2 (Department)
                await Task.Run(() => DepartmentTravelRequest(travelApprovalRequestDTO));
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(new RespStatus { Status = "Success", Message = "Travel Request Created!" });


        }

        // DELETE: api/TravelApprovalRequests/5
        [HttpDelete("{id}")]
        [ActionName("DeleteTravelApprovalRequest")]
        [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr")]
        public async Task<IActionResult> DeleteTravelApprovalRequest(int id)
        {
            var travelApprovalRequest = await _context.TravelApprovalRequests.FindAsync(id);
            if (travelApprovalRequest == null)
            {
                return NoContent();
            }

            _context.TravelApprovalRequests.Remove(travelApprovalRequest);
            await _context.SaveChangesAsync();

            return Ok(new RespStatus { Status = "Success", Message = "Travel Approval Request Deleted!" });
        }

        private bool TravelApprovalRequestExists(int id)
        {
            return _context.TravelApprovalRequests.Any(e => e.Id == id);
        }





        /// <summary>
        /// This is the option 1 : : PROJECT BASED TRAVEL REQUEST
        /// </summary>
        /// <param name="travelApprovalRequestDto"></param>
        /// <param name="travelApprovalRequestDto"></param>
        private async Task<IActionResult> ProjectTravelRequest(TravelApprovalRequestDTO travelApprovalRequestDTO)
        {


            #region
            int costCentreId = _context.Projects.Find(travelApprovalRequestDTO.ProjectId).CostCentreId;
            int projManagerid = _context.Projects.Find(travelApprovalRequestDTO.ProjectId).ProjectManagerId;
            var approver = _context.Employees.Find(projManagerid);
            int reqEmpid = travelApprovalRequestDTO.EmployeeId;
            int maxApprLevel = _context.ApprovalRoleMaps.Max(a => a.ApprovalLevelId);
            int empApprLevel = _context.ApprovalRoleMaps.Where(a => a.RoleId == _context.Employees.Find(reqEmpid).RoleId).FirstOrDefault().Id;
            bool isSelfApprovedRequest = false;
            #endregion

            //### 1. If Employee Travel Request enter a record in TravelApprovalRequestTracker
            #region
            TravelApprovalRequest travelApprovalRequest = new();

            travelApprovalRequest.Id = travelApprovalRequestDTO.Id;
            travelApprovalRequest.EmployeeId = travelApprovalRequestDTO.EmployeeId;
            travelApprovalRequest.TravelStartDate = travelApprovalRequestDTO.TravelStartDate;
            travelApprovalRequest.TravelEndDate = travelApprovalRequestDTO.TravelEndDate;
            travelApprovalRequest.TravelPurpose = travelApprovalRequestDTO.TravelPurpose;
            travelApprovalRequest.ReqRaisedDate = DateTime.Now;
            travelApprovalRequest.DepartmentId = travelApprovalRequestDTO.DepartmentId;
            travelApprovalRequest.ProjectId = travelApprovalRequestDTO.ProjectId;
            travelApprovalRequest.SubProjectId = travelApprovalRequestDTO.SubProjectId;
            travelApprovalRequest.WorkTaskId = travelApprovalRequestDTO.WorkTaskId;
            travelApprovalRequest.ApprovalStatusTypeId = (int)EApprovalStatus.Initiating;


            _context.TravelApprovalRequests.Add(travelApprovalRequest);

            await _context.SaveChangesAsync();
            //get the saved record Id
            travelApprovalRequestDTO.Id = travelApprovalRequest.Id;
            #endregion

            //##### 3. Add an entry to ClaimApproval Status tracker
            //get costcentreID based on project
            #region

            ///////////////////////////// Check if self Approved Request /////////////////////////////

            //if highest approver is requesting Petty cash request himself
            if (maxApprLevel == empApprLevel)
            {
                isSelfApprovedRequest = true;
            }
            //////////////////////////////////////////////////////////////////////////////////////////
            TravelApprovalStatusTracker travelApprovalStatusTracker = new();
            if (isSelfApprovedRequest)
            {
                travelApprovalStatusTracker.EmployeeId = travelApprovalRequestDTO.EmployeeId;
                travelApprovalStatusTracker.TravelApprovalRequestId = travelApprovalRequestDTO.Id;
                travelApprovalStatusTracker.TravelStartDate = travelApprovalRequestDTO.TravelStartDate;
                travelApprovalStatusTracker.TravelEndDate = travelApprovalRequestDTO.TravelEndDate;
                travelApprovalStatusTracker.DepartmentId = null;
                travelApprovalStatusTracker.ProjectId = travelApprovalRequestDTO.ProjectId;
                travelApprovalStatusTracker.RoleId = approver.RoleId;
                travelApprovalStatusTracker.ApprovalGroupId = _context.Employees.Find(travelApprovalRequestDTO.EmployeeId).ApprovalGroupId;
                travelApprovalStatusTracker.ApprovalLevelId = empApprLevel; // default approval level is 2 for Project based request
                travelApprovalStatusTracker.ReqDate = DateTime.Now;
                travelApprovalStatusTracker.FinalApprovedDate = DateTime.Now;
                travelApprovalStatusTracker.ApprovalStatusTypeId = (int)EApprovalStatus.Approved; //1-Initiating; 2-Pending; 3-InReview; 4-Approved; 5-Rejected
                travelApprovalStatusTracker.Comments = "Travel Request in Proceess";
            }
            else
            {
                travelApprovalStatusTracker.EmployeeId = travelApprovalRequestDTO.EmployeeId;
                travelApprovalStatusTracker.TravelApprovalRequestId = travelApprovalRequestDTO.Id;
                travelApprovalStatusTracker.TravelStartDate = travelApprovalRequestDTO.TravelStartDate;
                travelApprovalStatusTracker.TravelEndDate = travelApprovalRequestDTO.TravelEndDate;
                travelApprovalStatusTracker.DepartmentId = null;
                travelApprovalStatusTracker.ProjectId = travelApprovalRequestDTO.ProjectId;
                travelApprovalStatusTracker.RoleId = approver.RoleId;
                // get the next ProjectManager approval.
                travelApprovalStatusTracker.ApprovalGroupId = _context.Employees.Find(travelApprovalRequestDTO.EmployeeId).ApprovalGroupId;
                travelApprovalStatusTracker.ApprovalLevelId = 2; // default approval level is 2 for Project based request
                travelApprovalStatusTracker.ReqDate = DateTime.Now;
                travelApprovalStatusTracker.FinalApprovedDate = null;
                travelApprovalStatusTracker.ApprovalStatusTypeId = (int)EApprovalStatus.Pending; //1-Initiating, 2-Pending, 3-InReview, 4-Approved, 5-Rejected
                travelApprovalStatusTracker.Comments = "Travel Request in Proceess";
            }



            _context.TravelApprovalStatusTrackers.Add(travelApprovalStatusTracker);
            await _context.SaveChangesAsync();
            #endregion


            //##### 4. Send email to the user
            //####################################
            #region
            if (isSelfApprovedRequest)
            {
                return null;
            }
            var approverMailAddress = approver.Email;
            string subject = "Travel Approval Request No# " + travelApprovalRequestDTO.Id.ToString();
            Employee emp = await _context.Employees.FindAsync(travelApprovalRequestDTO.EmployeeId);
            var pettycashreq = _context.TravelApprovalRequests.Find(travelApprovalRequestDTO.Id);
            string content = "Travel Approval Request sought by " + emp.FirstName + "<br/>Travel Request Details <br/>Start Date: " + travelApprovalRequestDTO.TravelStartDate + "<br/>End Date: " + travelApprovalRequestDTO.TravelEndDate + "<br/>Travel Purpose: " + travelApprovalRequestDTO.TravelPurpose;
            var messagemail = new Message(new string[] { approverMailAddress }, subject, content);

            await _emailSender.SendEmailAsync(messagemail);
            #endregion



            await _context.SaveChangesAsync();

            return Ok(new RespStatus { Status = "Success", Message = "Travel Request Created!" });
        }

        /// <summary>
        /// This is option 2 : DEPARTMENT BASED CASH ADVANCE REQUEST
        /// </summary>
        /// <param name="travelApprovalRequestDto"></param>

        private async Task DepartmentTravelRequest(TravelApprovalRequestDTO travelApprovalRequestDto)
        {
            //### 1. If Employee Eligible for Cash Claim enter a record and reduce the available amount for next claim
            #region

            int reqEmpid = travelApprovalRequestDto.EmployeeId;
            Employee reqEmp = _context.Employees.Find(reqEmpid);
            int reqApprGroupId = reqEmp.ApprovalGroupId;
            int reqRoleId = reqEmp.RoleId;
            int maxApprLevel = _context.ApprovalRoleMaps.Include("ApprovalLevel").Where(a => a.ApprovalGroupId == reqApprGroupId).ToList().Select(x => x.ApprovalLevel).Max(a => a.Level);
            int reqApprLevel = _context.ApprovalRoleMaps.Include("ApprovalLevel").Where(a => a.ApprovalGroupId == reqApprGroupId && a.RoleId == reqRoleId).Select(x => x.ApprovalLevel).FirstOrDefault().Level;
            bool isSelfApprovedRequest = false;

            var travelApprovalRequest = new TravelApprovalRequest()
            {
                EmployeeId = reqEmpid,
                TravelStartDate = travelApprovalRequestDto.TravelStartDate,
                TravelEndDate = travelApprovalRequestDto.TravelEndDate,
                TravelPurpose = travelApprovalRequestDto.TravelPurpose,
                ReqRaisedDate = DateTime.Now,
                DepartmentId = _context.Employees.Find(reqEmpid).DepartmentId,
                ProjectId = travelApprovalRequestDto.ProjectId,
                SubProjectId = travelApprovalRequestDto.SubProjectId,
                WorkTaskId = travelApprovalRequestDto.WorkTaskId,
                ApprovalStatusTypeId = (int)EApprovalStatus.Pending


            };
            _context.TravelApprovalRequests.Add(travelApprovalRequest);
            await _context.SaveChangesAsync();

            //get the saved record Id
            travelApprovalRequestDto.Id = travelApprovalRequest.Id;

            #endregion
            
            
            #region
            //##### STEP 3. ClaimsApprovalTracker to be updated for all the allowed Approvers

            ///////////////////////////// Check if self Approved Request /////////////////////////////
           

            //if highest approver is requesting Petty cash request himself
            if (maxApprLevel == reqApprLevel)
            {
                isSelfApprovedRequest = true;
            }
            //////////////////////////////////////////////////////////////////////////////////////////


            var getEmpClaimApproversAllLevels = _context.ApprovalRoleMaps
                                .Include(a => a.ApprovalLevel)
                                .Where(a => a.ApprovalGroupId == reqApprGroupId)
                                .OrderBy(o => o.ApprovalLevel.Level).ToList();
            bool isFirstApprover = true;

            if(isSelfApprovedRequest)
            {
                TravelApprovalStatusTracker travelApprovalStatusTracker = new()
                {
                    EmployeeId = travelApprovalRequestDto.EmployeeId,
                    TravelApprovalRequestId = travelApprovalRequestDto.Id,
                    TravelStartDate = travelApprovalRequestDto.TravelStartDate,
                    TravelEndDate = travelApprovalRequestDto.TravelEndDate,
                    DepartmentId = _context.Employees.Find(travelApprovalRequestDto.EmployeeId).DepartmentId,
                    ProjectId = null,
                    RoleId = _context.Employees.Find(travelApprovalRequestDto.EmployeeId).RoleId,
                    ApprovalLevelId = reqApprLevel,
                    ApprovalGroupId = reqApprGroupId,
                    ReqDate = DateTime.Now,
                    FinalApprovedDate = DateTime.Now,
                    ApprovalStatusTypeId = (int)EApprovalStatus.Approved,
                    Comments = "Travel Request in Proceess"
                };
            }
            else
            {
                foreach (ApprovalRoleMap ApprMap in getEmpClaimApproversAllLevels)
                {

                    int role_id = ApprMap.RoleId;
                    var approver = _context.Employees.Where(e => e.RoleId == role_id && e.ApprovalGroupId == reqApprGroupId).FirstOrDefault();
                    if (approver == null)
                    {
                        continue;
                    }

                    int approverLevelid = _context.ApprovalRoleMaps.Where(x => x.RoleId == approver.RoleId && x.ApprovalGroupId == reqApprGroupId).FirstOrDefault().ApprovalLevelId;
                    int approverLevel = _context.ApprovalLevels.Find(approverLevelid).Level;

                    if (reqApprLevel >= approverLevel)
                    {
                        continue;
                    }


                    TravelApprovalStatusTracker travelApprovalStatusTracker = new()
                    {
                        EmployeeId = travelApprovalRequestDto.EmployeeId,
                        TravelApprovalRequestId = travelApprovalRequestDto.Id,
                        TravelStartDate = travelApprovalRequestDto.TravelStartDate,
                        TravelEndDate = travelApprovalRequestDto.TravelEndDate,
                        DepartmentId = approver.DepartmentId,
                        ProjectId = null,
                        RoleId = approver.RoleId,
                        ApprovalLevelId = ApprMap.ApprovalLevelId,
                        ApprovalGroupId = reqApprGroupId,
                        ReqDate = DateTime.Now,
                        FinalApprovedDate = null,
                        ApprovalStatusTypeId = isFirstApprover ? (int)EApprovalStatus.Pending : (int)EApprovalStatus.Initiating,
                        Comments = "Travel Request in Proceess"
                        //1-Initiating, 2-Pending, 3-InReview, 4-Approved, 5-Rejected
                    };


                    _context.TravelApprovalStatusTrackers.Add(travelApprovalStatusTracker);
                    await _context.SaveChangesAsync();


                    if (isFirstApprover)
                    {
                        //##### 4. Send email to the Approver
                        //####################################
                        var approverMailAddress = approver.Email;
                        string subject = "Travel Approval Request No# " + travelApprovalRequestDto.Id.ToString();
                        Employee emp = await _context.Employees.FindAsync(travelApprovalRequestDto.EmployeeId);
                        var pettycashreq = _context.TravelApprovalRequests.Find(travelApprovalRequestDto.Id);
                        string content = "Travel Approval Request sought by " + emp.FirstName + "<br/>Travel Request Deetails <br/>Start Date: " + travelApprovalRequestDto.TravelStartDate + "<br/>End Date: " + travelApprovalRequestDto.TravelEndDate + "<br/>Travel Purpose: " + travelApprovalRequestDto.TravelPurpose;
                        var messagemail = new Message(new string[] { approverMailAddress }, subject, content);

                        await _emailSender.SendEmailAsync(messagemail);
                    }

                    //first approver will be added as Pending, other approvers will be with In Approval Queue
                    isFirstApprover = false;

                }
            }

          
            #endregion


            await _context.SaveChangesAsync();
        }

       
      

    }
}
