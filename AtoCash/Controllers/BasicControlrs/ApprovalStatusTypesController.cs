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

namespace AtoCash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Admin, User")]
    public class ApprovalStatusTypesController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public ApprovalStatusTypesController(AtoCashDbContext context)
        {
            _context = context;
        }

        // GET: api/ApprovalStatusTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApprovalStatusType>>> GetApprovalStatusTypes()
        {
            return await _context.ApprovalStatusTypes.ToListAsync();
        }

        // GET: api/ApprovalStatusTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApprovalStatusType>> GetApprovalStatusType(int id)
        {
            var approvalStatusType = await _context.ApprovalStatusTypes.FindAsync(id);

            if (approvalStatusType == null)
            {
                return NoContent();
            }

            return approvalStatusType;
        }

        // PUT: api/ApprovalStatusTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "AtominosAdmin, Admin")]
        public async Task<IActionResult> PutApprovalStatusType(int id, ApprovalStatusType approvalStatusType)
        {
            if (id != approvalStatusType.Id)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Id is invalid" });
            }

            _context.Entry(approvalStatusType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApprovalStatusTypeExists(id))
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

        // POST: api/ApprovalStatusTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "AtominosAdmin, Admin")]
        public async Task<ActionResult<ApprovalStatusType>> PostApprovalStatusType(ApprovalStatusType approvalStatusType)
        {
            _context.ApprovalStatusTypes.Add(approvalStatusType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApprovalStatusType", new { id = approvalStatusType.Id }, approvalStatusType);
        }

        // DELETE: api/ApprovalStatusTypes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "AtominosAdmin, Admin")]
        public async Task<IActionResult> DeleteApprovalStatusType(int id)
        {
            var approvalStatusType = await _context.ApprovalStatusTypes.FindAsync(id);
            if (approvalStatusType == null)
            {
                return NoContent();
            }

            _context.ApprovalStatusTypes.Remove(approvalStatusType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApprovalStatusTypeExists(int id)
        {
            return _context.ApprovalStatusTypes.Any(e => e.Id == id);
        }
    }
}
