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

namespace AtoCash.Controllers.ExpenseReimburse
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr, User")]
    public class ExpenseReimburseStatusTrackersController : ControllerBase
    {
        private readonly AtoCashDbContext _context;
        private readonly IEmailSender _emailSender;

        public ExpenseReimburseStatusTrackersController(AtoCashDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: api/ExpenseReimburseStatusTrackers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseReimburseStatusTracker>>> GetExpenseReimburseStatusTrackers()
        {
            return await _context.ExpenseReimburseStatusTrackers.ToListAsync();
        }

        // GET: api/ExpenseReimburseStatusTrackers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseReimburseStatusTracker>> GetExpenseReimburseStatusTracker(int id)
        {
            var expenseReimburseStatusTracker = await _context.ExpenseReimburseStatusTrackers.FindAsync(id);

            if (expenseReimburseStatusTracker == null)
            {
                return NotFound();
            }

            return expenseReimburseStatusTracker;
        }


        [HttpGet("{id}")]
        [ActionName("ApprovalFlowForRequest")]
        public ActionResult<IEnumerable<ApprovalStatusFlowVM>> GetApprovalFlowForRequest(int id)
        {

            if (id == 0)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Expense-Reimburse Request Id is Invalid" });
            }



            var expenseReimburseStatusTrackers = _context.ExpenseReimburseStatusTrackers.Where(e => e.ExpenseReimburseRequestId == id).ToList();

            if (expenseReimburseStatusTrackers == null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Expense-Reimburse Request Id is Not Found" });
            }

            List<ApprovalStatusFlowVM> ListApprovalStatusFlow = new();

            foreach (ExpenseReimburseStatusTracker claim in expenseReimburseStatusTrackers)
            {
                string claimApproverName = null;

                if (claim.ProjectId > 0)
                {
                    claimApproverName = _context.Employees.Where(e => e.Id == _context.Projects.Find(claim.ProjectId).ProjectManagerId)
                        .Select(s => s.FirstName + " " + s.MiddleName + " " + s.LastName).FirstOrDefault();
                }
                else
                {
                    claimApproverName = _context.Employees.Where(x => x.RoleId == claim.JobRoleId && x.ApprovalGroupId == claim.ApprovalGroupId)
                        .Select(s => s.FirstName + " " + s.MiddleName + " " + s.LastName).FirstOrDefault();
                }

                ApprovalStatusFlowVM approvalStatusFlow = new()
                {
                    ApprovalLevel = claim.ApprovalLevelId,
                    ApproverRole = _context.JobRoles.Find(claim.JobRoleId).RoleName,
                    ApproverName = claimApproverName,
                    ApprovedDate = claim.ApprovedDate,
                    ApprovalStatusType = _context.ApprovalStatusTypes.Find(claim.ApprovalStatusTypeId).Status

                };
                ListApprovalStatusFlow.Add(approvalStatusFlow);
            }

            return Ok(ListApprovalStatusFlow);

        }


        [HttpGet("{id}")]
        [ActionName("ApprovalsPendingForApprover")]
        public ActionResult<IEnumerable<ClaimApprovalStatusTrackerDTO>> GetPendingApprovalRequestForApprover(int id)
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

            var expenseReimburseStatusTrackers = _context.ExpenseReimburseStatusTrackers
                                .Where(r =>
                                    r.JobRoleId == jobRoleid &&
                                    r.ApprovalGroupId == apprGroupId &&
                                    r.ApprovalStatusTypeId == (int)EApprovalStatus.Pending);

            List<ExpenseReimburseStatusTrackerDTO> ListExpenseReimburseStatusTrackerDTO = new();

            foreach (ExpenseReimburseStatusTracker expenseReimburseStatusTracker in expenseReimburseStatusTrackers)
            {
                ExpenseReimburseStatusTrackerDTO expenseReimburseStatusTrackerDTO = new()
                {
                    Id = expenseReimburseStatusTracker.Id,
                    EmployeeId = expenseReimburseStatusTracker.EmployeeId,
                    EmployeeName = _context.Employees.Find(expenseReimburseStatusTracker.EmployeeId).GetFullName(),
                    ExpenseReimburseRequestId = expenseReimburseStatusTracker.ExpenseReimburseRequestId,
                    DepartmentId = expenseReimburseStatusTracker.DepartmentId,
                    Department = expenseReimburseStatusTracker.DepartmentId != null ? _context.Departments.Find(expenseReimburseStatusTracker.DepartmentId).DeptName : null,
                    ProjectId = expenseReimburseStatusTracker.ProjectId,
                    Project = expenseReimburseStatusTracker.ProjectId != null ? _context.Projects.Find(expenseReimburseStatusTracker.ProjectId).ProjectName : null,
                    JobRoleId = expenseReimburseStatusTracker.JobRoleId,
                    JobRole = _context.JobRoles.Find(expenseReimburseStatusTracker.JobRoleId).RoleName,
                    ApprovalLevelId = expenseReimburseStatusTracker.ApprovalLevelId,
                    ExpReimReqDate = expenseReimburseStatusTracker.ExpReimReqDate,
                    ApprovedDate = expenseReimburseStatusTracker.ApprovedDate,
                    ApprovalStatusTypeId = expenseReimburseStatusTracker.ApprovalStatusTypeId,
                    ApprovalStatusType = _context.ApprovalStatusTypes.Find(expenseReimburseStatusTracker.ApprovalStatusTypeId).Status,
                    Comments = expenseReimburseStatusTracker.Comments
                };


                ListExpenseReimburseStatusTrackerDTO.Add(expenseReimburseStatusTrackerDTO);

            }


            return Ok(ListExpenseReimburseStatusTrackerDTO);

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
            int Jobroleid = _context.Employees.Find(id).RoleId;

            if (Jobroleid == 0)
            {
                return NotFound(new RespStatus { Status = "Failure", Message = "JobRole Id is Invalid" });
            }

            return Ok(_context.ExpenseReimburseStatusTrackers.Where(r => r.JobRoleId == Jobroleid && r.ApprovalStatusTypeId == (int)EApprovalStatus.Pending).Count());

        }

        // PUT: api/ExpenseReimburseStatusTrackers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutExpenseReimburseStatusTracker(List<ExpenseReimburseStatusTrackerDTO> ListExpenseReimburseStatusTrackerDto)
        {


            if (ListExpenseReimburseStatusTrackerDto.Count == 0)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "No Request to Approve!" });
            }


            bool isNextApproverAvailable = true;
            foreach (ExpenseReimburseStatusTrackerDTO expenseReimburseStatusTrackerDto in ListExpenseReimburseStatusTrackerDto)
            {
                var expenseReimburseStatusTracker = await _context.ExpenseReimburseStatusTrackers.FindAsync(expenseReimburseStatusTrackerDto.Id);

                //if same status continue to next loop, otherwise process
                if (expenseReimburseStatusTracker.ApprovalStatusTypeId == expenseReimburseStatusTrackerDto.ApprovalStatusTypeId)
                {
                    continue;
                }

                expenseReimburseStatusTracker.Id = expenseReimburseStatusTrackerDto.Id;
                expenseReimburseStatusTracker.EmployeeId = expenseReimburseStatusTrackerDto.EmployeeId;
                expenseReimburseStatusTracker.ExpenseReimburseRequestId = expenseReimburseStatusTrackerDto.ExpenseReimburseRequestId;
                expenseReimburseStatusTracker.DepartmentId = expenseReimburseStatusTrackerDto.DepartmentId;
                expenseReimburseStatusTracker.ProjectId = expenseReimburseStatusTrackerDto.ProjectId;
                expenseReimburseStatusTracker.JobRoleId = expenseReimburseStatusTrackerDto.JobRoleId;
                expenseReimburseStatusTracker.ApprovalLevelId = expenseReimburseStatusTrackerDto.ApprovalLevelId;
                expenseReimburseStatusTracker.ExpReimReqDate = expenseReimburseStatusTrackerDto.ExpReimReqDate;
                expenseReimburseStatusTracker.ApprovedDate = expenseReimburseStatusTrackerDto.ApprovedDate;
                expenseReimburseStatusTracker.ApprovalStatusTypeId = expenseReimburseStatusTrackerDto.ApprovalStatusTypeId;
                expenseReimburseStatusTracker.Comments = expenseReimburseStatusTrackerDto.Comments;



                ExpenseReimburseStatusTracker claimitem;
                if (expenseReimburseStatusTrackerDto.DepartmentId != null)
                {
                    int empApprGroupId = _context.Employees.Find(expenseReimburseStatusTracker.EmployeeId).ApprovalGroupId;

                    //Check if the record is already approved
                    //if it is not approved then trigger next approver level email & Change the status to approved
                    if (expenseReimburseStatusTrackerDto.ApprovalStatusTypeId == (int)EApprovalStatus.Approved)
                    {
                        //Get the next approval level (get its ID)
                        //int qExpReimRequestId = expenseReimburseStatusTrackerDto.ExpenseReimburseRequestId ?? 0;
                        int qExpReimRequestId = expenseReimburseStatusTrackerDto.ExpenseReimburseRequestId;

                        isNextApproverAvailable = true;

                        int CurClaimApprovalLevel = _context.ApprovalLevels.Find(expenseReimburseStatusTrackerDto.ApprovalLevelId).Level;
                        int nextClaimApprovalLevel = CurClaimApprovalLevel + 1;
                        int qApprovalLevelId;
                        int apprGroupId = _context.ExpenseReimburseStatusTrackers.Find(expenseReimburseStatusTrackerDto.Id).ApprovalGroupId;

                        if (_context.ApprovalRoleMaps.Where(a => a.ApprovalGroupId == apprGroupId && a.ApprovalLevelId == nextClaimApprovalLevel).FirstOrDefault() != null)
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
                            claimitem = _context.ExpenseReimburseStatusTrackers.Where(c => c.ExpenseReimburseRequestId == qExpReimRequestId &&
                                c.ApprovalStatusTypeId == qApprovalStatusTypeId &&
                                 c.ApprovalGroupId == empApprGroupId &&
                                c.ApprovalLevelId == qApprovalLevelId).FirstOrDefault();
                            claimitem.ApprovalStatusTypeId = (int)EApprovalStatus.Pending;

                        }
                        else
                        {
                            //final approver hence update PettyCashRequest
                            claimitem = _context.ExpenseReimburseStatusTrackers.Where(c => c.ExpenseReimburseRequestId == qExpReimRequestId &&
                               c.ApprovalStatusTypeId == qApprovalStatusTypeId &&
                                c.ApprovalGroupId == empApprGroupId &&
                               c.ApprovalLevelId == qApprovalLevelId).FirstOrDefault();
                            //claimitem.ApprovalStatusTypeId = (int)EApprovalStatus.Approved;
                            claimitem.ApprovedDate = DateTime.Now;

                            //DisbursementAndClaimsMaster update the record to Approved (ApprovalStatusId
                            int disbAndClaimItemId = _context.DisbursementsAndClaimsMasters.Where(d => d.ExpenseReimburseReqId == claimitem.ExpenseReimburseRequestId).FirstOrDefault().Id;
                            var disbAndClaimItem = await _context.DisbursementsAndClaimsMasters.FindAsync(disbAndClaimItemId);

                            disbAndClaimItem.ApprovalStatusId = (int)EApprovalStatus.Approved;
                            _context.Update(disbAndClaimItem);
                        }

                        //Save to database
                        _context.Update(claimitem);
                        await _context.SaveChangesAsync();
                        int reqApprGroupId = _context.Employees.Find(expenseReimburseStatusTrackerDto.EmployeeId).ApprovalGroupId;
                        var getEmpClaimApproversAllLevels = _context.ApprovalRoleMaps.Include(a => a.ApprovalLevel).Where(a => a.ApprovalGroupId == reqApprGroupId).OrderBy(o => o.ApprovalLevel.Level).ToList();

                        foreach (var ApprMap in getEmpClaimApproversAllLevels)
                        {

                            //only next level (level + 1) approver is considered here
                            if (ApprMap.ApprovalLevelId == expenseReimburseStatusTracker.ApprovalLevelId + 1)
                            {
                                int role_id = ApprMap.RoleId;
                                var approver = _context.Employees.Where(e => e.RoleId == role_id && e.ApprovalGroupId == reqApprGroupId).FirstOrDefault();

                                //##### 4. Send email to the Approver
                                //####################################
                                var approverMailAddress = approver.Email;
                                string subject = "Expense Reimburse Request " + expenseReimburseStatusTracker.ExpenseReimburseRequestId.ToString();
                                Employee emp = await _context.Employees.FindAsync(expenseReimburseStatusTracker.EmployeeId);
                                var expReimReqt = _context.ExpenseReimburseRequests.Find(expenseReimburseStatusTracker.ExpenseReimburseRequestId);
                                string content = "Expense Reimburse Request by " + emp.FirstName + "<br/>Total Claim for the amount of " + expReimReqt.TotalClaimAmount + "<br/>towards " + expReimReqt.ExpenseReportTitle;
                                var messagemail = new Message(new string[] { approverMailAddress }, subject, content);

                                await _emailSender.SendEmailAsync(messagemail);

                                break;

                            }
                        }
                    }

                    //if nothing else then just update the approval status
                    expenseReimburseStatusTracker.ApprovalStatusTypeId = expenseReimburseStatusTrackerDto.ApprovalStatusTypeId;

                }
                else
                {
                    //final approver hence update PettyCashRequest
                    claimitem = _context.ExpenseReimburseStatusTrackers.Where(c => c.ExpenseReimburseRequestId == expenseReimburseStatusTracker.ExpenseReimburseRequestId &&
                                c.ApprovalStatusTypeId == (int)EApprovalStatus.Pending).FirstOrDefault();
                    expenseReimburseStatusTracker.ApprovalStatusTypeId = expenseReimburseStatusTrackerDto.ApprovalStatusTypeId;
                    //DisbursementAndClaimsMaster update the record to Approved (ApprovalStatusId
                    int disbAndClaimItemId = _context.DisbursementsAndClaimsMasters.Where(d => d.ExpenseReimburseReqId == claimitem.ExpenseReimburseRequestId).FirstOrDefault().Id;
                    var disbAndClaimItem = await _context.DisbursementsAndClaimsMasters.FindAsync(disbAndClaimItemId);

                    disbAndClaimItem.ApprovalStatusId = (int)EApprovalStatus.Approved;
                    _context.Update(disbAndClaimItem);

                    //Update Pettycashrequest table to update the record to Approved as the final approver has approved it.
                    int pettyCashReqId = _context.PettyCashRequests.Where(d => d.Id == claimitem.ExpenseReimburseRequestId).FirstOrDefault().Id;
                    var pettyCashReq = await _context.PettyCashRequests.FindAsync(pettyCashReqId);

                    pettyCashReq.ApprovalStatusTypeId = (int)EApprovalStatus.Approved;
                    pettyCashReq.ApprovedDate = DateTime.Now;
                    _context.Update(pettyCashReq);

                }

                _context.ExpenseReimburseStatusTrackers.Update(expenseReimburseStatusTracker);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(new RespStatus { Status = "Success", Message = "Expense-Reimburse Requests is/are Approved!" });

            //if (id != expenseReimburseStatusTracker.Id)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(expenseReimburseStatusTracker).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!ExpenseReimburseStatusTrackerExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
        }

        // POST: api/ExpenseReimburseStatusTrackers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExpenseReimburseStatusTracker>> PostExpenseReimburseStatusTracker(ExpenseReimburseStatusTracker expenseReimburseStatusTracker)
        {
            _context.ExpenseReimburseStatusTrackers.Add(expenseReimburseStatusTracker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpenseReimburseStatusTracker", new { id = expenseReimburseStatusTracker.Id }, expenseReimburseStatusTracker);
        }

        // DELETE: api/ExpenseReimburseStatusTrackers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenseReimburseStatusTracker(int id)
        {
            var expenseReimburseStatusTracker = await _context.ExpenseReimburseStatusTrackers.FindAsync(id);
            if (expenseReimburseStatusTracker == null)
            {
                return NotFound();
            }

            _context.ExpenseReimburseStatusTrackers.Remove(expenseReimburseStatusTracker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpenseReimburseStatusTrackerExists(int id)
        {
            return _context.ExpenseReimburseStatusTrackers.Any(e => e.Id == id);
        }
    }
}
