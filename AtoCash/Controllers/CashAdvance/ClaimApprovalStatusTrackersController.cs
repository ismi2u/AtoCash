using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtoCash.Data;
using AtoCash.Models;
using Microsoft.AspNetCore.Authorization;
using AtoCash.Authentication;
using EmailService;

namespace AtoCash.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Finmgr, Admin, User, Manager")]
    public class ClaimApprovalStatusTrackersController : ControllerBase
    {
        private readonly AtoCashDbContext _context;
        private readonly IEmailSender _emailSender;

        public ClaimApprovalStatusTrackersController(AtoCashDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: api/ClaimApprovalStatusTrackers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClaimApprovalStatusTrackerDTO>>> GetClaimApprovalStatusTrackers()
        {
            List<ClaimApprovalStatusTrackerDTO> ListClaimApprovalStatusTrackerDTO = new();

            var claimApprovalStatusTrackers = await _context.ClaimApprovalStatusTrackers.ToListAsync();

            foreach (ClaimApprovalStatusTracker claimApprovalStatusTracker in claimApprovalStatusTrackers)
            {
                ClaimApprovalStatusTrackerDTO claimApprovalStatusTrackerDTO = new()
                {
                    Id = claimApprovalStatusTracker.Id,
                    EmployeeId = claimApprovalStatusTracker.EmployeeId,
                    EmployeeName = _context.Employees.Find(claimApprovalStatusTracker.EmployeeId).GetFullName(),
                    PettyCashRequestId = claimApprovalStatusTracker.PettyCashRequestId,
                    ExpenseReimburseRequestId = claimApprovalStatusTracker.ExpenseReimburseRequestId,
                    DepartmentId = claimApprovalStatusTracker.DepartmentId,
                    DepartmentName = claimApprovalStatusTracker.DepartmentId != null ? _context.Departments.Find(claimApprovalStatusTracker.DepartmentId).DeptName : null,
                    ProjectId = claimApprovalStatusTracker.ProjectId,
                    ProjectName = claimApprovalStatusTracker.ProjectId != null ? _context.Projects.Find(claimApprovalStatusTracker.ProjectId).ProjectName : null,
                    RoleId = claimApprovalStatusTracker.RoleId,
                    JobRole = _context.JobRoles.Find(claimApprovalStatusTracker.RoleId).RoleName,
                    ApprovalLevelId = claimApprovalStatusTracker.ApprovalLevelId,
                    ReqDate = claimApprovalStatusTracker.ReqDate,
                    FinalApprovedDate = claimApprovalStatusTracker.FinalApprovedDate,
                    ApprovalStatusTypeId = claimApprovalStatusTracker.ApprovalStatusTypeId,
                    ApprovalStatusType = _context.ApprovalStatusTypes.Find(claimApprovalStatusTracker.ApprovalStatusTypeId).Status,
                    Comments = claimApprovalStatusTracker.Comments
                };

                ListClaimApprovalStatusTrackerDTO.Add(claimApprovalStatusTrackerDTO);

            }

            return ListClaimApprovalStatusTrackerDTO;
        }

        // GET: api/ClaimApprovalStatusTrackers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClaimApprovalStatusTrackerDTO>> GetClaimApprovalStatusTracker(int id)
        {


            var claimApprovalStatusTracker = await _context.ClaimApprovalStatusTrackers.FindAsync(id);

            if (claimApprovalStatusTracker == null)
            {
                return NoContent();
            }

            ClaimApprovalStatusTrackerDTO claimApprovalStatusTrackerDTO = new()
            {
                Id = claimApprovalStatusTracker.Id,
                EmployeeId = claimApprovalStatusTracker.EmployeeId,
                EmployeeName = _context.Employees.Find(claimApprovalStatusTracker.EmployeeId).GetFullName(),
                PettyCashRequestId = claimApprovalStatusTracker.PettyCashRequestId,
                ExpenseReimburseRequestId = claimApprovalStatusTracker.ExpenseReimburseRequestId,
                DepartmentId = claimApprovalStatusTracker.DepartmentId,
                DepartmentName = claimApprovalStatusTracker.DepartmentId != null ? _context.Departments.Find(claimApprovalStatusTracker.DepartmentId).DeptName : null,
                ProjectId = claimApprovalStatusTracker.ProjectId,
                ProjectName = claimApprovalStatusTracker.ProjectId != null ? _context.Projects.Find(claimApprovalStatusTracker.ProjectId).ProjectName : null,
                RoleId = claimApprovalStatusTracker.RoleId,
                JobRole = _context.JobRoles.Find(claimApprovalStatusTracker.RoleId).RoleName,
                ApprovalLevelId = claimApprovalStatusTracker.ApprovalLevelId,
                ReqDate = claimApprovalStatusTracker.ReqDate,
                FinalApprovedDate = claimApprovalStatusTracker.FinalApprovedDate,
                ApprovalStatusTypeId = claimApprovalStatusTracker.ApprovalStatusTypeId,
                ApprovalStatusType = _context.ApprovalStatusTypes.Find(claimApprovalStatusTracker.ApprovalStatusTypeId).Status,
                Comments = claimApprovalStatusTracker.Comments
            };


            return claimApprovalStatusTrackerDTO;
        }

        /// <summary>
        /// Approver Approving the claim
        /// </summary>
        /// <param name="id"></param>
        /// <param name="claimApprovalStatusTrackerDto"></param>
        /// <returns></returns>

        // PUT: api/ClaimApprovalStatusTrackers/5

        [HttpPut]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin, Manager")]
        public async Task<IActionResult> PutClaimApprovalStatusTracker(List<ClaimApprovalStatusTrackerDTO> ListClaimApprovalStatusTrackerDto)
        {

            if (ListClaimApprovalStatusTrackerDto.Count == 0)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "No Request to Approve!" });
            }


            bool isNextApproverAvailable = true;
            foreach (ClaimApprovalStatusTrackerDTO claimApprovalStatusTrackerDto in ListClaimApprovalStatusTrackerDto)
            {
                var claimApprovalStatusTracker = await _context.ClaimApprovalStatusTrackers.FindAsync(claimApprovalStatusTrackerDto.Id);

                //if same status continue to next loop, otherwise process
                if (claimApprovalStatusTracker.ApprovalStatusTypeId == claimApprovalStatusTrackerDto.ApprovalStatusTypeId)
                {
                    continue;
                }

                claimApprovalStatusTracker.Id = claimApprovalStatusTrackerDto.Id;
                claimApprovalStatusTracker.EmployeeId = claimApprovalStatusTrackerDto.EmployeeId;
                claimApprovalStatusTracker.PettyCashRequestId = claimApprovalStatusTrackerDto.PettyCashRequestId;
                claimApprovalStatusTracker.ExpenseReimburseRequestId = claimApprovalStatusTrackerDto.ExpenseReimburseRequestId;
                claimApprovalStatusTracker.DepartmentId = claimApprovalStatusTrackerDto.DepartmentId;
                claimApprovalStatusTracker.ProjectId = claimApprovalStatusTrackerDto.ProjectId;
                claimApprovalStatusTracker.RoleId = claimApprovalStatusTrackerDto.RoleId;
                claimApprovalStatusTracker.ApprovalLevelId = claimApprovalStatusTrackerDto.ApprovalLevelId;
                claimApprovalStatusTracker.ReqDate = claimApprovalStatusTrackerDto.ReqDate;
                claimApprovalStatusTracker.FinalApprovedDate = DateTime.Now;
                claimApprovalStatusTracker.Comments = claimApprovalStatusTrackerDto.Comments;

                ClaimApprovalStatusTracker claimitem;
                if (claimApprovalStatusTrackerDto.DepartmentId != null)
                {
                    int empApprGroupId = _context.Employees.Find(claimApprovalStatusTracker.EmployeeId).ApprovalGroupId;

                    //Check if the record is already approved
                    //if it is not approved then trigger next approver level email & Change the status to approved
                    if (claimApprovalStatusTrackerDto.ApprovalStatusTypeId == (int)EApprovalStatus.Approved)
                    {
                        //Get the next approval level (get its ID)
                        int qPettyCashRequestId = claimApprovalStatusTrackerDto.PettyCashRequestId ?? 0;

                        isNextApproverAvailable = true;

                        int CurClaimApprovalLevel = _context.ApprovalLevels.Find(claimApprovalStatusTrackerDto.ApprovalLevelId).Level;
                        int nextClaimApprovalLevel = CurClaimApprovalLevel + 1;
                        int qApprovalLevelId;
                        if (_context.ApprovalLevels.Where(x => x.Level == nextClaimApprovalLevel).FirstOrDefault() != null)
                        {
                            qApprovalLevelId = _context.ApprovalLevels.Where(x => x.Level == nextClaimApprovalLevel).FirstOrDefault().Id;
                        }
                        else
                        {
                            qApprovalLevelId = _context.ApprovalLevels.Where(x => x.Level == CurClaimApprovalLevel).FirstOrDefault().Id;
                            isNextApproverAvailable = false;
                        }

                        int qApprovalStatusTypeId = isNextApproverAvailable ? (int)EApprovalStatus.Initiating : (int)EApprovalStatus.Pending;

                        //update the next level approver Track request to PENDING (from Initiating) 
                        //if claimitem is not null change the status
                        if (isNextApproverAvailable)
                        {
                            claimitem = _context.ClaimApprovalStatusTrackers.Where(c => c.PettyCashRequestId == qPettyCashRequestId &&
                                c.ApprovalStatusTypeId == qApprovalStatusTypeId &&
                                c.ApprovalLevelId == qApprovalLevelId).FirstOrDefault();
                            claimitem.ApprovalStatusTypeId = (int)EApprovalStatus.Pending;

                        }
                        else
                        {
                            //final approver hence update PettyCashRequest
                            claimitem = _context.ClaimApprovalStatusTrackers.Where(c => c.PettyCashRequestId == qPettyCashRequestId &&
                               c.ApprovalStatusTypeId == qApprovalStatusTypeId &&
                               c.ApprovalLevelId == qApprovalLevelId).FirstOrDefault();
                            //claimitem.ApprovalStatusTypeId = (int)EApprovalStatus.Approved;
                            claimitem.FinalApprovedDate = DateTime.Now;

                            //DisbursementAndClaimsMaster update the record to Approved (ApprovalStatusId
                            int disbAndClaimItemId = _context.DisbursementsAndClaimsMasters.Where(d => d.PettyCashRequestId == claimitem.PettyCashRequestId).FirstOrDefault().Id;
                            var disbAndClaimItem = await _context.DisbursementsAndClaimsMasters.FindAsync(disbAndClaimItemId);

                            disbAndClaimItem.ApprovalStatusId = (int)EApprovalStatus.Approved;
                            _context.Update(disbAndClaimItem);
                        }

                        //Save to database
                        _context.Update(claimitem);
                        await _context.SaveChangesAsync();
                        var getEmpClaimApproversAllLevels = _context.ApprovalRoleMaps.Where(a => a.ApprovalGroupId == empApprGroupId).OrderBy(a => a.ApprovalLevel).ToList();

                        foreach (var ApprMap in getEmpClaimApproversAllLevels)
                        {

                            //only next level (level + 1) approver is considered here
                            if (ApprMap.ApprovalLevelId == claimApprovalStatusTracker.ApprovalLevelId + 1)
                            {
                                int role_id = ApprMap.RoleId;
                                var approver = _context.Employees.Where(e => e.RoleId == role_id).FirstOrDefault();

                                //##### 4. Send email to the Approver
                                //####################################
                                var approverMailAddress = approver.Email;
                                string subject = "Pettycash Request Approval " + claimApprovalStatusTracker.PettyCashRequestId.ToString();
                                Employee emp = await _context.Employees.FindAsync(claimApprovalStatusTracker.EmployeeId);
                                var pettycashreq = _context.PettyCashRequests.Find(claimApprovalStatusTracker.PettyCashRequestId);
                                string content = "Petty Cash Approval sought by " + emp.FirstName + "<br/>Cash Request for the amount of " + pettycashreq.PettyClaimAmount + "<br/>towards " + pettycashreq.PettyClaimRequestDesc;
                                var messagemail = new Message(new string[] { approverMailAddress }, subject, content);

                                await _emailSender.SendEmailAsync(messagemail);

                                break;

                            }
                        }
                    }

                    //if nothing else then just update the approval status
                    claimApprovalStatusTracker.ApprovalStatusTypeId = claimApprovalStatusTrackerDto.ApprovalStatusTypeId;

                }
                else
                {
                    //final approver hence update PettyCashRequest
                    claimitem = _context.ClaimApprovalStatusTrackers.Where(c => c.PettyCashRequestId == claimApprovalStatusTracker.PettyCashRequestId &&
                                c.ApprovalStatusTypeId == (int)EApprovalStatus.Pending).FirstOrDefault();
                    claimApprovalStatusTracker.ApprovalStatusTypeId = claimApprovalStatusTrackerDto.ApprovalStatusTypeId;
                    //DisbursementAndClaimsMaster update the record to Approved (ApprovalStatusId
                    int disbAndClaimItemId = _context.DisbursementsAndClaimsMasters.Where(d => d.PettyCashRequestId == claimitem.PettyCashRequestId).FirstOrDefault().Id;
                    var disbAndClaimItem = await _context.DisbursementsAndClaimsMasters.FindAsync(disbAndClaimItemId);

                    disbAndClaimItem.ApprovalStatusId = (int)EApprovalStatus.Approved;
                    _context.Update(disbAndClaimItem);

                    //Update Pettycashrequest table to update the record to Approved as the final approver has approved it.
                    int pettyCashReqId = _context.PettyCashRequests.Where(d => d.Id == claimitem.PettyCashRequestId).FirstOrDefault().Id;
                    var pettyCashReq = await _context.PettyCashRequests.FindAsync(pettyCashReqId);

                    pettyCashReq.ApprovalStatusTypeId = (int)EApprovalStatus.Approved;
                    pettyCashReq.ApprovedDate = DateTime.Now;
                    _context.Update(pettyCashReq);

                }

                _context.ClaimApprovalStatusTrackers.Update(claimApprovalStatusTracker);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(new RespStatus { Status = "Success", Message = "Cash Advances is/are Approved!" });
        }



        // POST: api/ClaimApprovalStatusTrackers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin, Manager, User")]
        public async Task<ActionResult<ClaimApprovalStatusTracker>> PostClaimApprovalStatusTracker(ClaimApprovalStatusTrackerDTO claimApprovalStatusTrackerDto)
        {
            ClaimApprovalStatusTracker claimApprovalStatusTracker = new()
            {
                Id = claimApprovalStatusTrackerDto.Id,
                EmployeeId = claimApprovalStatusTrackerDto.EmployeeId,
                PettyCashRequestId = claimApprovalStatusTrackerDto.PettyCashRequestId,
                ExpenseReimburseRequestId = claimApprovalStatusTrackerDto.ExpenseReimburseRequestId,
                DepartmentId = claimApprovalStatusTrackerDto.DepartmentId,
                ProjectId = claimApprovalStatusTrackerDto.ProjectId,
                RoleId = claimApprovalStatusTrackerDto.RoleId,
                ApprovalLevelId = claimApprovalStatusTrackerDto.ApprovalLevelId,
                ReqDate = claimApprovalStatusTrackerDto.ReqDate,
                FinalApprovedDate = claimApprovalStatusTrackerDto.FinalApprovedDate,
                ApprovalStatusTypeId = (int)EApprovalStatus.Pending,
                Comments = claimApprovalStatusTrackerDto.Comments
        };

            _context.ClaimApprovalStatusTrackers.Add(claimApprovalStatusTracker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClaimApprovalStatusTracker", new { id = claimApprovalStatusTracker.Id }, claimApprovalStatusTracker);
        }

        // DELETE: api/ClaimApprovalStatusTrackers/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<IActionResult> DeleteClaimApprovalStatusTracker(int id)
        {
            var claimApprovalStatusTracker = await _context.ClaimApprovalStatusTrackers.FindAsync(id);
            if (claimApprovalStatusTracker == null)
            {
                return NoContent();
            }

            _context.ClaimApprovalStatusTrackers.Remove(claimApprovalStatusTracker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //private bool ClaimApprovalStatusTrackerExists(int id)
        //{
        //    return _context.ClaimApprovalStatusTrackers.Any(e => e.Id == id);
        //}


        /// <summary>
        /// List of Pending approvals for the given Approver
        /// </summary>
        /// <param EmployeeId="id"></param>
        /// <returns>List of Claim</returns>

        [HttpGet("{id}")]
        [ActionName("ApprovalsPendingForApprover")]
        public ActionResult<IEnumerable<ClaimApprovalStatusTrackerDTO>> GetPendingApprovalRequestForApprover(int id)
        {

            if (id == 0)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Employee Id is Invalid" });
            }


            //get the RoleID of the Employee (Approver)
            int roleid = _context.Employees.Find(id).RoleId;

            if (roleid == 0)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Role Id is Invalid" });
            }

            var claimApprovalStatusTrackers = _context.ClaimApprovalStatusTrackers.Where(r => r.RoleId == roleid && r.ApprovalStatusTypeId == (int)EApprovalStatus.Pending);
            List<ClaimApprovalStatusTrackerDTO> ListClaimApprovalStatusTrackerDTO = new();

            foreach (ClaimApprovalStatusTracker claimApprovalStatusTracker in claimApprovalStatusTrackers)
            {
                ClaimApprovalStatusTrackerDTO claimApprovalStatusTrackerDTO = new()
                {
                    Id = claimApprovalStatusTracker.Id,
                    EmployeeId = claimApprovalStatusTracker.EmployeeId,
                    EmployeeName = _context.Employees.Find(claimApprovalStatusTracker.EmployeeId).GetFullName(),
                    PettyCashRequestId = claimApprovalStatusTracker.PettyCashRequestId,
                    ExpenseReimburseRequestId = claimApprovalStatusTracker.ExpenseReimburseRequestId,
                    DepartmentId = claimApprovalStatusTracker.DepartmentId,
                    DepartmentName = claimApprovalStatusTracker.DepartmentId != null ? _context.Departments.Find(claimApprovalStatusTracker.DepartmentId).DeptName : null,
                    ProjectId = claimApprovalStatusTracker.ProjectId,
                    ProjectName = claimApprovalStatusTracker.ProjectId != null ? _context.Projects.Find(claimApprovalStatusTracker.ProjectId).ProjectName : null,
                    RoleId = claimApprovalStatusTracker.RoleId,
                    JobRole = _context.JobRoles.Find(claimApprovalStatusTracker.RoleId).RoleName,
                    ApprovalLevelId = claimApprovalStatusTracker.ApprovalLevelId,
                    ReqDate = claimApprovalStatusTracker.ReqDate,
                    FinalApprovedDate = claimApprovalStatusTracker.FinalApprovedDate,
                    ApprovalStatusTypeId = claimApprovalStatusTracker.ApprovalStatusTypeId,
                    ApprovalStatusType = _context.ApprovalStatusTypes.Find(claimApprovalStatusTracker.ApprovalStatusTypeId).Status,
                    Comments = claimApprovalStatusTracker.Comments
                };


                ListClaimApprovalStatusTrackerDTO.Add(claimApprovalStatusTrackerDTO);

            }


            return Ok(ListClaimApprovalStatusTrackerDTO);

        }



        //To get the counts of pending approvals

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

            return Ok(_context.ClaimApprovalStatusTrackers.Where(r => r.RoleId == roleid && r.ApprovalStatusTypeId == (int)EApprovalStatus.Pending).Count());

        }

        /// <summary>
        /// GetApprovalFlowForRequest
        /// </summary>
        /// <param PettycashRequestId="id"></param>
        /// <returns></returns>

        [HttpGet("{id}")]
        [ActionName("ApprovalFlowForRequest")]
        public ActionResult<IEnumerable<ApprovalStatusFlowVM>> GetApprovalFlowForRequest(int id)
        {

            if (id == 0)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "PettycashRequest Id is Invalid" });
            }



            var claimRequestTracks = _context.ClaimApprovalStatusTrackers.Where(c => c.PettyCashRequestId == id).ToList();

            if (claimRequestTracks == null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "PettycashRequest Id is Not Found" });
            }

            List<ApprovalStatusFlowVM> ListApprovalStatusFlow = new();

            foreach (ClaimApprovalStatusTracker claim in claimRequestTracks)
            {
                ApprovalStatusFlowVM approvalStatusFlow = new()
                {
                    ApprovalLevel = claim.ApprovalLevelId,
                    ApproverRole = _context.JobRoles.Find(claim.RoleId).RoleName,
                    ApproverName = _context.Employees.Where(x => x.RoleId == claim.RoleId).Select(s => s.FirstName + " " + s.MiddleName + " " + s.LastName).FirstOrDefault(),
                    ApprovedDate = claim.FinalApprovedDate,
                    ApprovalStatusType = _context.ApprovalStatusTypes.Find(claim.ApprovalStatusTypeId).Status

                };
                ListApprovalStatusFlow.Add(approvalStatusFlow);
            }

            return Ok(ListApprovalStatusFlow);

        }

        private enum RequestType
        {
            CashAdvance = 1,
            ExpenseReim

        }

        private enum ApprovalStatus
        {
            Initiating = 1,
            Pending,
            InReview,
            Approved,
            Rejected

        }


        ////
    }
}
