using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtoCash.Data;
using AtoCash.Models;

namespace AtoCash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimApprovalStatusTrackersController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public ClaimApprovalStatusTrackersController(AtoCashDbContext context)
        {
            _context = context;
        }

        // GET: api/ClaimApprovalStatusTrackers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClaimApprovalStatusTrackerDTO>>> GetClaimApprovalStatusTrackers()
        {
            List<ClaimApprovalStatusTrackerDTO> ListClaimApprovalStatusTrackerDTO = new List<ClaimApprovalStatusTrackerDTO>();

            var claimApprovalStatusTrackers = await _context.ClaimApprovalStatusTrackers.ToListAsync();

            foreach (ClaimApprovalStatusTracker claimApprovalStatusTracker in claimApprovalStatusTrackers)
            {
                ClaimApprovalStatusTrackerDTO claimApprovalStatusTrackerDTO = new ClaimApprovalStatusTrackerDTO();

                claimApprovalStatusTrackerDTO.Id = claimApprovalStatusTracker.Id;
                claimApprovalStatusTrackerDTO.EmployeeId = claimApprovalStatusTracker.EmployeeId;
                claimApprovalStatusTrackerDTO.PettyCashRequestId = claimApprovalStatusTracker.PettyCashRequestId;
                claimApprovalStatusTrackerDTO.ExpenseReimburseRequestId = claimApprovalStatusTracker.ExpenseReimburseRequestId;
                claimApprovalStatusTrackerDTO.DepartmentId = claimApprovalStatusTracker.DepartmentId;
                claimApprovalStatusTrackerDTO.ProjectId = claimApprovalStatusTracker.ProjectId;
                claimApprovalStatusTrackerDTO.ReqDate = claimApprovalStatusTracker.ReqDate;
                claimApprovalStatusTrackerDTO.FinalApprovedDate = claimApprovalStatusTracker.FinalApprovedDate;
                claimApprovalStatusTrackerDTO.ApprovalStatusTypeId = claimApprovalStatusTracker.ApprovalStatusTypeId;

                ListClaimApprovalStatusTrackerDTO.Add(claimApprovalStatusTrackerDTO);

            }

            return ListClaimApprovalStatusTrackerDTO;
        }

        // GET: api/ClaimApprovalStatusTrackers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClaimApprovalStatusTrackerDTO>> GetClaimApprovalStatusTracker(int id)
        {
            ClaimApprovalStatusTrackerDTO claimApprovalStatusTrackerDTO = new ClaimApprovalStatusTrackerDTO();

            var claimApprovalStatusTracker = await _context.ClaimApprovalStatusTrackers.FindAsync(id);

            if (claimApprovalStatusTracker == null)
            {
                return NotFound();
            }

            claimApprovalStatusTrackerDTO.Id = claimApprovalStatusTracker.Id;
            claimApprovalStatusTrackerDTO.EmployeeId = claimApprovalStatusTracker.EmployeeId;
            claimApprovalStatusTrackerDTO.PettyCashRequestId = claimApprovalStatusTracker.PettyCashRequestId;
            claimApprovalStatusTrackerDTO.ExpenseReimburseRequestId = claimApprovalStatusTracker.ExpenseReimburseRequestId;
            claimApprovalStatusTrackerDTO.DepartmentId = claimApprovalStatusTracker.DepartmentId;
            claimApprovalStatusTrackerDTO.ProjectId = claimApprovalStatusTracker.ProjectId;
            claimApprovalStatusTrackerDTO.ReqDate = claimApprovalStatusTracker.ReqDate;
            claimApprovalStatusTrackerDTO.FinalApprovedDate = claimApprovalStatusTracker.FinalApprovedDate;
            claimApprovalStatusTrackerDTO.ApprovalStatusTypeId = claimApprovalStatusTracker.ApprovalStatusTypeId;

            return claimApprovalStatusTrackerDTO;
        }

        // PUT: api/ClaimApprovalStatusTrackers/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutClaimApprovalStatusTracker(int id, ClaimApprovalStatusTrackerDTO claimApprovalStatusTrackerDto)
        {
            if (id != claimApprovalStatusTrackerDto.Id)
            {
                return BadRequest();
            }

            var claimApprovalStatusTracker = await _context.ClaimApprovalStatusTrackers.FindAsync(id);

            claimApprovalStatusTracker.Id = claimApprovalStatusTrackerDto.Id;
            claimApprovalStatusTracker.EmployeeId = claimApprovalStatusTrackerDto.EmployeeId;
            claimApprovalStatusTracker.PettyCashRequestId = claimApprovalStatusTrackerDto.PettyCashRequestId;
            claimApprovalStatusTracker.ExpenseReimburseRequestId = claimApprovalStatusTrackerDto.ExpenseReimburseRequestId;
            claimApprovalStatusTracker.DepartmentId = claimApprovalStatusTrackerDto.DepartmentId;
            claimApprovalStatusTracker.ProjectId = claimApprovalStatusTrackerDto.ProjectId;
            claimApprovalStatusTracker.ReqDate = claimApprovalStatusTrackerDto.ReqDate;
            claimApprovalStatusTracker.FinalApprovedDate = claimApprovalStatusTrackerDto.FinalApprovedDate;
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
                    return NotFound();
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
        public async Task<ActionResult<ClaimApprovalStatusTracker>> PostClaimApprovalStatusTracker(ClaimApprovalStatusTracker claimApprovalStatusTrackerDto)
        {
            ClaimApprovalStatusTracker claimApprovalStatusTracker = new ClaimApprovalStatusTracker();

            claimApprovalStatusTracker.Id = claimApprovalStatusTrackerDto.Id;
            claimApprovalStatusTracker.EmployeeId = claimApprovalStatusTrackerDto.EmployeeId;
            claimApprovalStatusTracker.PettyCashRequestId = claimApprovalStatusTrackerDto.PettyCashRequestId;
            claimApprovalStatusTracker.ExpenseReimburseRequestId = claimApprovalStatusTrackerDto.ExpenseReimburseRequestId;
            claimApprovalStatusTracker.DepartmentId = claimApprovalStatusTrackerDto.DepartmentId;
            claimApprovalStatusTracker.ProjectId = claimApprovalStatusTrackerDto.ProjectId;
            claimApprovalStatusTracker.ReqDate = claimApprovalStatusTrackerDto.ReqDate;
            claimApprovalStatusTracker.FinalApprovedDate = claimApprovalStatusTrackerDto.FinalApprovedDate;
            claimApprovalStatusTracker.ApprovalStatusTypeId = claimApprovalStatusTrackerDto.ApprovalStatusTypeId;

            _context.ClaimApprovalStatusTrackers.Add(claimApprovalStatusTracker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClaimApprovalStatusTracker", new { id = claimApprovalStatusTracker.Id }, claimApprovalStatusTracker);
        }

        // DELETE: api/ClaimApprovalStatusTrackers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClaimApprovalStatusTracker(int id)
        {
            var claimApprovalStatusTracker = await _context.ClaimApprovalStatusTrackers.FindAsync(id);
            if (claimApprovalStatusTracker == null)
            {
                return NotFound();
            }

            _context.ClaimApprovalStatusTrackers.Remove(claimApprovalStatusTracker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClaimApprovalStatusTrackerExists(int id)
        {
            return _context.ClaimApprovalStatusTrackers.Any(e => e.Id == id);
        }
    }
}
