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
    [Authorize(Roles = "AtominosAdmin, Admin, User")]
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
            List<ClaimApprovalStatusTrackerDTO> ListClaimApprovalStatusTrackerDTO = new List<ClaimApprovalStatusTrackerDTO>();

            var claimApprovalStatusTrackers = await _context.ClaimApprovalStatusTrackers.ToListAsync();

            foreach (ClaimApprovalStatusTracker claimApprovalStatusTracker in claimApprovalStatusTrackers)
            {
                ClaimApprovalStatusTrackerDTO claimApprovalStatusTrackerDTO = new ClaimApprovalStatusTrackerDTO
                {
                    Id = claimApprovalStatusTracker.Id,
                    EmployeeId = claimApprovalStatusTracker.EmployeeId,
                    EmployeeName = _context.Employees.Find(claimApprovalStatusTracker.EmployeeId).GetFullName(),
                    PettyCashRequestId = claimApprovalStatusTracker.PettyCashRequestId,
                    ExpenseReimburseRequestId = claimApprovalStatusTracker.ExpenseReimburseRequestId,
                    DepartmentId = claimApprovalStatusTracker.DepartmentId,
                    DepartmentName = _context.Departments.Find(claimApprovalStatusTracker.DepartmentId).DeptName,
                    ProjectId = claimApprovalStatusTracker.ProjectId,
                    ProjectName = _context.Projects.Find(claimApprovalStatusTracker.ProjectId).ProjectName,
                    RoleId = claimApprovalStatusTracker.RoleId,
                    JobRole = _context.JobRoles.Find(claimApprovalStatusTracker.RoleId).RoleName,
                    ApprovalLevelId = claimApprovalStatusTracker.ApprovalLevelId,
                    ReqDate = claimApprovalStatusTracker.ReqDate,
                    FinalApprovedDate = claimApprovalStatusTracker.FinalApprovedDate,
                    ApprovalStatusTypeId = claimApprovalStatusTracker.ApprovalStatusTypeId
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
            ClaimApprovalStatusTrackerDTO claimApprovalStatusTrackerDTO = new ClaimApprovalStatusTrackerDTO
            {
                Id = claimApprovalStatusTracker.Id,
                EmployeeId = claimApprovalStatusTracker.EmployeeId,
                EmployeeName = _context.Employees.Find(claimApprovalStatusTracker.EmployeeId).GetFullName(),
                PettyCashRequestId = claimApprovalStatusTracker.PettyCashRequestId,
                ExpenseReimburseRequestId = claimApprovalStatusTracker.ExpenseReimburseRequestId,
                DepartmentId = claimApprovalStatusTracker.DepartmentId,
                DepartmentName = _context.Departments.Find(claimApprovalStatusTracker.DepartmentId).DeptName,
                ProjectId = claimApprovalStatusTracker.ProjectId,
                ProjectName = _context.Projects.Find(claimApprovalStatusTracker.ProjectId).ProjectName,
                RoleId = claimApprovalStatusTracker.RoleId,
                JobRole = _context.JobRoles.Find(claimApprovalStatusTracker.RoleId).RoleName,
                ApprovalLevelId = claimApprovalStatusTracker.ApprovalLevelId,
                ReqDate = claimApprovalStatusTracker.ReqDate,
                FinalApprovedDate = claimApprovalStatusTracker.FinalApprovedDate,
                ApprovalStatusTypeId = claimApprovalStatusTracker.ApprovalStatusTypeId
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

        [HttpPut("{id}")]
        [Authorize(Roles = "AtominosAdmin, Admin")]
        public async Task<IActionResult> PutClaimApprovalStatusTracker(int id, ClaimApprovalStatusTrackerDTO claimApprovalStatusTrackerDto)
        {
            if (id != claimApprovalStatusTrackerDto.Id)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Id is invalid" });
            }

            var claimApprovalStatusTracker = await _context.ClaimApprovalStatusTrackers.FindAsync(id);

            claimApprovalStatusTracker.Id = claimApprovalStatusTrackerDto.Id;
            claimApprovalStatusTracker.EmployeeId = claimApprovalStatusTrackerDto.EmployeeId;
            claimApprovalStatusTracker.PettyCashRequestId = claimApprovalStatusTrackerDto.PettyCashRequestId;
            claimApprovalStatusTracker.ExpenseReimburseRequestId = claimApprovalStatusTrackerDto.ExpenseReimburseRequestId;
            claimApprovalStatusTracker.DepartmentId = claimApprovalStatusTrackerDto.DepartmentId;
            claimApprovalStatusTracker.ProjectId = claimApprovalStatusTrackerDto.ProjectId;
            claimApprovalStatusTracker.RoleId = claimApprovalStatusTrackerDto.RoleId;
            claimApprovalStatusTracker.ApprovalLevelId = claimApprovalStatusTrackerDto.ApprovalLevelId;
            claimApprovalStatusTracker.ReqDate = claimApprovalStatusTrackerDto.ReqDate;
            claimApprovalStatusTracker.FinalApprovedDate = claimApprovalStatusTrackerDto.FinalApprovedDate;

            int empApprGroupId = _context.Employees.Find(claimApprovalStatusTracker.EmployeeId).ApprovalGroupId;

            //if it is approved then trigger next approver level email
            if (claimApprovalStatusTracker.ApprovalStatusTypeId == (int)ApprovalStatus.Pending &&
                claimApprovalStatusTrackerDto.ApprovalStatusTypeId == (int)ApprovalStatus.Approved)
            {
                var getEmpClaimApproversAllLevels = _context.ApprovalRoleMaps.Where(a => a.ApprovalGroupId == empApprGroupId).ToList().OrderBy(a => a.ApprovalLevel);

                foreach (ApprovalRoleMap ApprMap in getEmpClaimApproversAllLevels)
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
                        string content = "Petty Cash Approval sought by " + emp.FirstName + "/nCash Request for the amount of " + pettycashreq.PettyClaimAmount + "/ntowards " + pettycashreq.PettyClaimRequestDesc;
                        var messagemail = new Message(new string[] { approverMailAddress }, subject, content);

                        await _emailSender.SendEmailAsync(messagemail);

                        break;

                    }
                }
            }


            //if not then just update the approval status
            claimApprovalStatusTracker.ApprovalStatusTypeId = claimApprovalStatusTrackerDto.ApprovalStatusTypeId;


            _context.ClaimApprovalStatusTrackers.Update(claimApprovalStatusTracker);
            //_context.Entry(claimApprovalStatusTracker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClaimApprovalStatusTrackerExists(id))
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

        // POST: api/ClaimApprovalStatusTrackers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "AtominosAdmin, Admin")]
        public async Task<ActionResult<ClaimApprovalStatusTracker>> PostClaimApprovalStatusTracker(ClaimApprovalStatusTracker claimApprovalStatusTrackerDto)
        {
            ClaimApprovalStatusTracker claimApprovalStatusTracker = new ClaimApprovalStatusTracker
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
                ApprovalStatusTypeId = claimApprovalStatusTrackerDto.ApprovalStatusTypeId
            };

            _context.ClaimApprovalStatusTrackers.Add(claimApprovalStatusTracker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClaimApprovalStatusTracker", new { id = claimApprovalStatusTracker.Id }, claimApprovalStatusTracker);
        }

        // DELETE: api/ClaimApprovalStatusTrackers/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "AtominosAdmin, Admin")]
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

        private bool ClaimApprovalStatusTrackerExists(int id)
        {
            return _context.ClaimApprovalStatusTrackers.Any(e => e.Id == id);
        }


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

            var claimApprovalStatusTrackers = _context.ClaimApprovalStatusTrackers.Where(r => r.RoleId == roleid && r.ApprovalStatusTypeId == 1);
            List<ClaimApprovalStatusTrackerDTO> ListClaimApprovalStatusTrackerDTO = new List<ClaimApprovalStatusTrackerDTO>();

            foreach (ClaimApprovalStatusTracker claimApprovalStatusTracker in claimApprovalStatusTrackers)
            {
                ClaimApprovalStatusTrackerDTO claimApprovalStatusTrackerDTO = new ClaimApprovalStatusTrackerDTO
                {
                    Id = claimApprovalStatusTracker.Id,
                    EmployeeId = claimApprovalStatusTracker.EmployeeId,
                    EmployeeName = _context.Employees.Find(claimApprovalStatusTracker.EmployeeId).GetFullName(),
                    PettyCashRequestId = claimApprovalStatusTracker.PettyCashRequestId,
                    ExpenseReimburseRequestId = claimApprovalStatusTracker.ExpenseReimburseRequestId,
                    DepartmentId = claimApprovalStatusTracker.DepartmentId,
                    DepartmentName = _context.Departments.Find(claimApprovalStatusTracker.DepartmentId).DeptName,
                    ProjectId = claimApprovalStatusTracker.ProjectId,
                    ProjectName = _context.Projects.Find(claimApprovalStatusTracker.ProjectId).ProjectName,
                    RoleId = claimApprovalStatusTracker.RoleId,
                    JobRole = _context.JobRoles.Find(claimApprovalStatusTracker.RoleId).RoleName,
                    ApprovalLevelId = claimApprovalStatusTracker.ApprovalLevelId,
                    ReqDate = claimApprovalStatusTracker.ReqDate,
                    FinalApprovedDate = claimApprovalStatusTracker.FinalApprovedDate,
                    ApprovalStatusTypeId = claimApprovalStatusTracker.ApprovalStatusTypeId

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

            return Ok(_context.ClaimApprovalStatusTrackers.Where(r => r.RoleId == roleid && r.ApprovalStatusTypeId == 1).Count());

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

            List<ApprovalStatusFlowVM> ListApprovalStatusFlow = new List<ApprovalStatusFlowVM>();

            foreach (ClaimApprovalStatusTracker claim in claimRequestTracks)
            {
                ApprovalStatusFlowVM approvalStatusFlow = new ApprovalStatusFlowVM()
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
            Pending = 1,
            Approved,
            Rejected

        }



        ////
    }
}
