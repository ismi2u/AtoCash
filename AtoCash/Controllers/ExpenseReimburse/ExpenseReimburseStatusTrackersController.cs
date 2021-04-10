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

namespace AtoCash.Controllers.ExpenseReimburse
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Finmgr, Admin, User")]
    public class ExpenseReimburseStatusTrackersController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public ExpenseReimburseStatusTrackersController(AtoCashDbContext context)
        {
            _context = context;
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
                ApprovalStatusFlowVM approvalStatusFlow = new()
                {
                    ApprovalLevel = claim.ApprovalLevelId,
                    ApproverRole = _context.JobRoles.Find(claim.JobRoleId).RoleName,
                    ApproverName = _context.Employees.Where(x => x.RoleId == claim.JobRoleId).Select(s => s.FirstName + " " + s.MiddleName + " " + s.LastName).FirstOrDefault(),
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
            int Jobroleid = _context.Employees.Find(id).RoleId;

            if (Jobroleid == 0)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Role Id is Invalid" });
            }

            var expenseReimburseStatusTrackers = _context.ExpenseReimburseStatusTrackers.Where(r => r.JobRoleId == Jobroleid && r.ApprovalStatusTypeId == (int)EApprovalStatus.Pending);
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
                    Department= expenseReimburseStatusTracker.DepartmentId != null ? _context.Departments.Find(expenseReimburseStatusTracker.DepartmentId).DeptName : null,
                    ProjectId = expenseReimburseStatusTracker.ProjectId,
                    Project= expenseReimburseStatusTracker.ProjectId != null ? _context.Projects.Find(expenseReimburseStatusTracker.ProjectId).ProjectName : null,
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpenseReimburseStatusTracker(int id, ExpenseReimburseStatusTracker expenseReimburseStatusTracker)
        {
            if (id != expenseReimburseStatusTracker.Id)
            {
                return BadRequest();
            }

            _context.Entry(expenseReimburseStatusTracker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseReimburseStatusTrackerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
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
