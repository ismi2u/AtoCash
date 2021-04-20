﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtoCash.Data;
using AtoCash.Models;
using Microsoft.AspNetCore.Authorization;
using EmailService;
using AtoCash.Authentication;

namespace AtoCash.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr, User, Manager")]
    public class TravelApprovalStatusTrackersController : ControllerBase
    {
        private readonly AtoCashDbContext _context;
        private readonly IEmailSender _emailSender;


        public TravelApprovalStatusTrackersController(AtoCashDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }




        // GET: api/TravelApprovalStatusTrackers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelApprovalStatusTrackerDTO>>> GetTravelApprovalStatusTrackers()
        {
            List<TravelApprovalStatusTrackerDTO> ListTravelApprovalStatusTrackerDTO = new();

            var TravelApprovalStatusTrackers = await _context.TravelApprovalStatusTrackers.ToListAsync();

            foreach (TravelApprovalStatusTracker travelApprovalStatusTracker in TravelApprovalStatusTrackers)
            {
                TravelApprovalStatusTrackerDTO TravelApprovalStatusTrackerDTO = new()
                {
                    Id = travelApprovalStatusTracker.Id,
                    EmployeeId = travelApprovalStatusTracker.EmployeeId,
                    TravelStartDate = travelApprovalStatusTracker.TravelStartDate,
                    TravelEndDate = travelApprovalStatusTracker.TravelEndDate,
                    EmployeeName = _context.Employees.Find(travelApprovalStatusTracker.EmployeeId).GetFullName(),
                    TravelApprovalRequestId = travelApprovalStatusTracker.TravelApprovalRequestId,
                    DepartmentId = travelApprovalStatusTracker.DepartmentId,
                    DepartmentName = travelApprovalStatusTracker.DepartmentId != null ? _context.Departments.Find(travelApprovalStatusTracker.DepartmentId).DeptName : null,
                    ProjectId = travelApprovalStatusTracker.ProjectId,
                    ProjectName = travelApprovalStatusTracker.ProjectId != null ? _context.Projects.Find(travelApprovalStatusTracker.ProjectId).ProjectName : null,
                    RoleId = travelApprovalStatusTracker.RoleId,
                    JobRole = _context.JobRoles.Find(travelApprovalStatusTracker.RoleId).RoleName,
                    ApprovalLevelId = travelApprovalStatusTracker.ApprovalLevelId,
                    ReqDate = travelApprovalStatusTracker.ReqDate,
                    FinalApprovedDate = travelApprovalStatusTracker.FinalApprovedDate,
                    ApprovalStatusTypeId = travelApprovalStatusTracker.ApprovalStatusTypeId,
                    ApprovalStatusType = _context.ApprovalStatusTypes.Find(travelApprovalStatusTracker.ApprovalStatusTypeId).Status,
                    Comments = travelApprovalStatusTracker.Comments
                };


                ListTravelApprovalStatusTrackerDTO.Add(TravelApprovalStatusTrackerDTO);

            }

            return ListTravelApprovalStatusTrackerDTO.OrderByDescending(o => o.ReqDate).ToList();
        }



        // GET: api/TravelApprovalStatusTrackers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TravelApprovalStatusTrackerDTO>> GetTravelApprovalStatusTracker(int id)
        {
            var travelApprovalStatusTracker = _context.TravelApprovalStatusTrackers.Where(t => t.TravelApprovalRequestId == id).FirstOrDefault();

            if (travelApprovalStatusTracker == null)
            {
                return NotFound();
            }

            TravelApprovalStatusTrackerDTO travelApprovalStatusTrackerDTO = new()
            {
                Id = travelApprovalStatusTracker.Id,
                EmployeeId = travelApprovalStatusTracker.EmployeeId,
                TravelStartDate = travelApprovalStatusTracker.TravelStartDate,
                TravelEndDate = travelApprovalStatusTracker.TravelEndDate,
                EmployeeName = _context.Employees.Find(travelApprovalStatusTracker.EmployeeId).GetFullName(),
                TravelApprovalRequestId = travelApprovalStatusTracker.TravelApprovalRequestId,
                DepartmentId = travelApprovalStatusTracker.DepartmentId,
                DepartmentName = travelApprovalStatusTracker.DepartmentId != null ? _context.Departments.Find(travelApprovalStatusTracker.DepartmentId).DeptName : null,
                ProjectId = travelApprovalStatusTracker.ProjectId,
                ProjectName = travelApprovalStatusTracker.ProjectId != null ? _context.Projects.Find(travelApprovalStatusTracker.ProjectId).ProjectName : null,
                RoleId = travelApprovalStatusTracker.RoleId,
                JobRole = _context.JobRoles.Find(travelApprovalStatusTracker.RoleId).RoleName,
                ApprovalLevelId = travelApprovalStatusTracker.ApprovalLevelId,
                ReqDate = travelApprovalStatusTracker.ReqDate,
                FinalApprovedDate = travelApprovalStatusTracker.FinalApprovedDate,
                ApprovalStatusTypeId = travelApprovalStatusTracker.ApprovalStatusTypeId,
                ApprovalStatusType = _context.ApprovalStatusTypes.Find(travelApprovalStatusTracker.ApprovalStatusTypeId).Status,
                Comments = travelApprovalStatusTracker.Comments
            };


            return travelApprovalStatusTrackerDTO;
        }

        // PUT: api/TravelApprovalStatusTrackers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutTravelApprovalStatusTracker(List<TravelApprovalStatusTrackerDTO> ListTravelApprovalStatusTrackerDTO)
        {
            if (ListTravelApprovalStatusTrackerDTO.Count == 0)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "No Request to Approve!" });
            }

            bool isNextApproverAvailable = true;
            bool bRejectMessage = false;
            foreach (TravelApprovalStatusTrackerDTO travelApprovalStatusTrackerDTO in ListTravelApprovalStatusTrackerDTO)
            {
                var travelApprovalStatusTracker = await _context.TravelApprovalStatusTrackers.FindAsync(travelApprovalStatusTrackerDTO.Id);

                //if same status continue to next loop, otherwise process
                if (travelApprovalStatusTracker.ApprovalStatusTypeId == travelApprovalStatusTrackerDTO.ApprovalStatusTypeId)
                {
                    continue;
                }
                if (travelApprovalStatusTrackerDTO.ApprovalStatusTypeId == (int)EApprovalStatus.Rejected)
                {
                    bRejectMessage = true;
                }
                travelApprovalStatusTracker.Id = travelApprovalStatusTrackerDTO.Id;
                travelApprovalStatusTracker.EmployeeId = travelApprovalStatusTrackerDTO.EmployeeId;

                travelApprovalStatusTracker.TravelApprovalRequestId = travelApprovalStatusTrackerDTO.TravelApprovalRequestId ?? 0;

                travelApprovalStatusTracker.DepartmentId = travelApprovalStatusTrackerDTO.DepartmentId;
                travelApprovalStatusTracker.ProjectId = travelApprovalStatusTrackerDTO.ProjectId;
                travelApprovalStatusTracker.RoleId = travelApprovalStatusTrackerDTO.RoleId;
                travelApprovalStatusTracker.ApprovalLevelId = travelApprovalStatusTrackerDTO.ApprovalLevelId;
                travelApprovalStatusTracker.ReqDate = travelApprovalStatusTrackerDTO.ReqDate;
                travelApprovalStatusTracker.FinalApprovedDate = DateTime.Now;
                travelApprovalStatusTracker.Comments = travelApprovalStatusTrackerDTO.Comments ?? "In Process";

                TravelApprovalStatusTracker travelItem;

                //department based request
                if (travelApprovalStatusTrackerDTO.DepartmentId != null)
                {
                    int empApprGroupId = _context.Employees.Find(travelApprovalStatusTrackerDTO.EmployeeId).ApprovalGroupId;

                    //Check if the record is already approved
                    //if it is not approved then trigger next approver level email & Change the status to approved
                    if (travelApprovalStatusTrackerDTO.ApprovalStatusTypeId == (int)EApprovalStatus.Approved)
                    {
                        //Get the next approval level (get its ID)
                        int qTravelApprovalRequestId = travelApprovalStatusTrackerDTO.TravelApprovalRequestId ?? 0;
                        int CurTravelApprovalLevel = _context.ApprovalLevels.Find(travelApprovalStatusTrackerDTO.ApprovalLevelId).Level;


                       //int MaxApprLevelForApprGroupId =  _context.ApprovalRoleMaps.Where(a => a.ApprovalGroupId == travelApprovalStatusTracker.ApprovalGroupId).Select(s => s.ApprovalLevel).Max(x => x.Level);

                       // if(CurTravelApprovalLevel == MaxApprLevelForApprGroupId)
                       // {
                       //     isNextApproverAvailable = false;
                       // }


                       
                        int nextClaimApprovalLevel = CurTravelApprovalLevel + 1;
                        int qApprovalLevelId;
                        int apprGroupId = _context.TravelApprovalStatusTrackers.Find(travelApprovalStatusTrackerDTO.Id).ApprovalGroupId;

                        if (_context.ApprovalRoleMaps.Where(a => a.ApprovalGroupId == apprGroupId && a.ApprovalLevelId == nextClaimApprovalLevel).FirstOrDefault() != null)
                        {
                            qApprovalLevelId = _context.ApprovalLevels.Where(x => x.Level == nextClaimApprovalLevel).FirstOrDefault().Id;
                        }
                        else
                        {
                            qApprovalLevelId = _context.ApprovalLevels.Where(x => x.Level == CurTravelApprovalLevel).FirstOrDefault().Id;
                            isNextApproverAvailable = false;
                        }

                        int qApprovalStatusTypeId = isNextApproverAvailable ? (int)EApprovalStatus.Initiating : (int)EApprovalStatus.Pending;

                        //update the next level approver Track request to PENDING (from Initiating) 
                        //if claimitem is not null change the status
                        if (isNextApproverAvailable)
                        {
                            travelItem = _context.TravelApprovalStatusTrackers
                                        .Where(c =>
                                        c.TravelApprovalRequestId == qTravelApprovalRequestId &&
                                        c.ApprovalStatusTypeId == qApprovalStatusTypeId &&
                                        c.ApprovalGroupId == empApprGroupId &&
                                        c.ApprovalLevelId == qApprovalLevelId).FirstOrDefault();
                            if (travelItem != null)
                            {
                                travelItem.ApprovalStatusTypeId = (int)EApprovalStatus.Pending;
                            }

                        }
                        else
                        {
                            //final approver hence update TravelApprovalRequest
                            travelItem = _context.TravelApprovalStatusTrackers.Where(c => c.TravelApprovalRequestId == qTravelApprovalRequestId &&
                               c.ApprovalStatusTypeId == qApprovalStatusTypeId &&
                               c.ApprovalGroupId == empApprGroupId &&
                               c.ApprovalLevelId == qApprovalLevelId).FirstOrDefault();
                            //travelItem.ApprovalStatusTypeId = (int)EApprovalStatus.Approved;
                            travelItem.FinalApprovedDate = DateTime.Now;

                            //final Approver hence updating TravelApprovalRequest
                            var travelApprovalRequest = _context.TravelApprovalRequests.Find(qTravelApprovalRequestId);
                            travelApprovalRequest.ApprovalStatusTypeId = (int)EApprovalStatus.Approved;
                            travelApprovalRequest.ApprovedDate = DateTime.Now;
                            _context.Update(travelApprovalRequest);
                        }

                        if (travelItem != null) { _context.Update(travelItem); };
                        await _context.SaveChangesAsync();
                        int reqApprGroupId = _context.Employees.Find(travelApprovalStatusTrackerDTO.EmployeeId).ApprovalGroupId;

                        //Save to database
                        var getEmpClaimApproversAllLevels = _context.ApprovalRoleMaps.Include(a => a.ApprovalLevel).Where(a => a.ApprovalGroupId == empApprGroupId).OrderBy(o => o.ApprovalLevel.Level).ToList();

                        foreach (var ApprMap in getEmpClaimApproversAllLevels)
                        {
                            //only next level (level + 1) approver is considered here
                            if (ApprMap.ApprovalLevelId == travelApprovalStatusTracker.ApprovalLevelId + 1)
                            {
                                int role_id = ApprMap.RoleId;
                                var approver = _context.Employees.Where(e => e.RoleId == role_id && e.ApprovalGroupId == reqApprGroupId).FirstOrDefault();

                                //##### 4. Send email to the Approver
                                //####################################
                                var approverMailAddress = approver.Email;
                                string subject = "Travel Approval Request " + travelApprovalStatusTracker.TravelApprovalRequestId.ToString();
                                Employee emp = await _context.Employees.FindAsync(travelApprovalStatusTracker.EmployeeId);
                                var travelApprReq = _context.TravelApprovalRequests.Find(travelApprovalStatusTracker.TravelApprovalRequestId);
                                string content = "Travel Request Approval sought by " + emp.FirstName + "<br/>for the purpose of " + travelApprReq.TravelPurpose;
                                var messagemail = new Message(new string[] { approverMailAddress }, subject, content);


                                await _emailSender.SendEmailAsync(messagemail);

                                break;

                            }
                        }
                    }

                    //if nothing else then just update the approval status
                    travelApprovalStatusTracker.ApprovalStatusTypeId = travelApprovalStatusTrackerDTO.ApprovalStatusTypeId;


                }
                else //if the approver is the final approver
                {

                    //final approver hence update TravelApprovalRequest
                    travelItem = _context.TravelApprovalStatusTrackers.Where(c => c.TravelApprovalRequestId == travelApprovalStatusTracker.TravelApprovalRequestId &&
                                c.ApprovalStatusTypeId == (int)EApprovalStatus.Pending).FirstOrDefault();
                    travelApprovalStatusTracker.ApprovalStatusTypeId = travelApprovalStatusTrackerDTO.ApprovalStatusTypeId;
                    //Update the TravelApprovalRequst table -> update the record to Approved (ApprovalStatusId

                    int travelApprovalrequestId = _context.TravelApprovalRequests.Where(d => d.Id == travelItem.TravelApprovalRequestId).FirstOrDefault().Id;
                    var travelApprovalrequest = await _context.TravelApprovalRequests.FindAsync(travelApprovalrequestId);

                    travelApprovalrequest.ApprovalStatusTypeId = (int)EApprovalStatus.Approved;
                    travelApprovalrequest.ApprovedDate = DateTime.Now;
                    _context.Update(travelApprovalrequest);

                }

                _context.TravelApprovalStatusTrackers.Update(travelApprovalStatusTracker);
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            RespStatus respStatus = new();

            if (bRejectMessage)
            {
                respStatus.Status = "Success";
                respStatus.Message = "Travel Approval Request(s) Rejected!";
            }
            else
            {
                respStatus.Status = "Success";
                respStatus.Message = "Travel Approval Request(s) Approved!";
            }

            return Ok(respStatus);
        }

        // POST: api/TravelApprovalStatusTrackers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TravelApprovalStatusTracker>> PostTravelApprovalStatusTracker(TravelApprovalStatusTracker travelApprovalStatusTracker)
        {
            _context.TravelApprovalStatusTrackers.Add(travelApprovalStatusTracker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTravelApprovalStatusTracker", new { id = travelApprovalStatusTracker.Id }, travelApprovalStatusTracker);
        }

        // DELETE: api/TravelApprovalStatusTrackers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTravelApprovalStatusTracker(int id)
        {
            var travelApprovalStatusTracker = await _context.TravelApprovalStatusTrackers.FindAsync(id);
            if (travelApprovalStatusTracker == null)
            {
                return NotFound();
            }

            _context.TravelApprovalStatusTrackers.Remove(travelApprovalStatusTracker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //private bool TravelApprovalStatusTrackerExists(int id)
        //{
        //    return _context.TravelApprovalStatusTrackers.Any(e => e.Id == id);
        //}

        [HttpGet("{id}")]
        [ActionName("ApprovalsPendingForApprover")]
        public ActionResult<IEnumerable<TravelApprovalStatusTrackerDTO>> GetPendingApprovalRequestForApprover(int id)
        {

            if (id == 0)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Employee Id is Invalid" });
            }


            //get the RoleID of the Employee (Approver)
            Employee apprEmp = _context.Employees.Find(id);
            int jobRoleid = apprEmp.RoleId;
            int apprGroupId = apprEmp.ApprovalGroupId;

            if (jobRoleid == 0)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Role Id is Invalid" });
            }

            var TravelApprovalStatusTrackers = _context.TravelApprovalStatusTrackers
                                .Where(r =>
                                    (r.RoleId == jobRoleid &&
                                    r.ApprovalGroupId == apprGroupId &&
                                    r.ApprovalStatusTypeId == (int)EApprovalStatus.Pending) ||
                                   
                                    (r.RoleId == jobRoleid &&
                                    r.ProjectId != null &&
                                    r.ApprovalStatusTypeId == (int)EApprovalStatus.Pending)

                                    );


            List<TravelApprovalStatusTrackerDTO> ListTravelApprovalStatusTrackerDTO = new();

            foreach (TravelApprovalStatusTracker travelApprovalStatusTracker in TravelApprovalStatusTrackers)
            {
                TravelApprovalStatusTrackerDTO TravelApprovalStatusTrackerDTO = new()
                {
                    Id = travelApprovalStatusTracker.Id,
                    EmployeeId = travelApprovalStatusTracker.EmployeeId,
                    EmployeeName = _context.Employees.Find(travelApprovalStatusTracker.EmployeeId).GetFullName(),
                    TravelApprovalRequestId = travelApprovalStatusTracker.TravelApprovalRequestId,
                    DepartmentId = travelApprovalStatusTracker.DepartmentId,
                    DepartmentName = travelApprovalStatusTracker.DepartmentId != null ? _context.Departments.Find(travelApprovalStatusTracker.DepartmentId).DeptName : null,
                    ProjectId = travelApprovalStatusTracker.ProjectId,
                    ProjectName = travelApprovalStatusTracker.ProjectId != null ? _context.Projects.Find(travelApprovalStatusTracker.ProjectId).ProjectName : null,
                    RoleId = travelApprovalStatusTracker.RoleId,
                    JobRole = _context.JobRoles.Find(travelApprovalStatusTracker.RoleId).RoleName,
                    ApprovalLevelId = travelApprovalStatusTracker.ApprovalLevelId,
                    ReqDate = travelApprovalStatusTracker.ReqDate,
                    FinalApprovedDate = travelApprovalStatusTracker.FinalApprovedDate,
                    ApprovalStatusTypeId = travelApprovalStatusTracker.ApprovalStatusTypeId,
                    ApprovalStatusType = _context.ApprovalStatusTypes.Find(travelApprovalStatusTracker.ApprovalStatusTypeId).Status
                };


                ListTravelApprovalStatusTrackerDTO.Add(TravelApprovalStatusTrackerDTO);

            }


            return Ok(ListTravelApprovalStatusTrackerDTO.OrderByDescending(o => o.ReqDate).ToList());

        }


        [HttpGet("{id}")]
        [ActionName("CountOfApprovalsPendingForApprover")]
        public ActionResult<int> GetCountOfApprovalsPendingForApprover(int id)
        {

            if (id == 0)
            {
                return NotFound(new RespStatus { Status = "Failure", Message = "Employee Id is Invalid" });
            }
            //get the RoleID of the Employee (Approver)
            int roleid = _context.Employees.Find(id).RoleId;

            if (roleid == 0)
            {
                return NotFound(new RespStatus { Status = "Failure", Message = "Role Id is Invalid" });
            }

            return Ok(_context.TravelApprovalStatusTrackers.Where(r => r.RoleId == roleid && r.ApprovalStatusTypeId == (int)EApprovalStatus.Pending).Count());

        }


        [HttpGet("{id}")]
        [ActionName("ApprovalFlowForTravelRequest")]
        public ActionResult<IEnumerable<ApprovalStatusFlowVM>> GetApprovalFlowForTravelRequest(int id)
        {

            if (id == 0)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Approval Flow Id is Invalid" });
            }



            var travelRequestTracks = _context.TravelApprovalStatusTrackers.Where(c => c.TravelApprovalRequestId == id).ToList();

            if (travelRequestTracks == null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Approval Flow Id is Not Found" });
            }

            List<ApprovalStatusFlowVM> ListApprovalStatusFlow = new();

            foreach (TravelApprovalStatusTracker travel in travelRequestTracks)
            {
                string claimApproverName = null;

                if (travel.ProjectId > 0)
                {
                    claimApproverName = _context.Employees.Where(e => e.Id == _context.Projects.Find(travel.ProjectId).ProjectManagerId)
                        .Select(s => s.FirstName + " " + s.MiddleName + " " + s.LastName).FirstOrDefault();
                }
                else
                {
                    claimApproverName = _context.Employees.Where(x => x.RoleId == travel.RoleId && x.ApprovalGroupId == travel.ApprovalGroupId)
                        .Select(s => s.FirstName + " " + s.MiddleName + " " + s.LastName).FirstOrDefault();
                }

                ApprovalStatusFlowVM approvalStatusFlow = new();

                approvalStatusFlow.ApprovalLevel = travel.ApprovalLevelId;
                approvalStatusFlow.ApproverRole = travel.ProjectId == null ? _context.JobRoles.Find(travel.RoleId).RoleName : "Project Manager" ;
                approvalStatusFlow.ApproverName = claimApproverName;
                approvalStatusFlow.ApprovedDate = travel.FinalApprovedDate;
                approvalStatusFlow.ApprovalStatusType = _context.ApprovalStatusTypes.Find(travel.ApprovalStatusTypeId).Status;
                ListApprovalStatusFlow.Add(approvalStatusFlow);
            }

            return Ok(ListApprovalStatusFlow);

        }


        ///
    }


}
