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

namespace AtoCash.Controllers.ExpenseReimburse
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Finmgr, Admin, User")]
    public class ExpenseSubClaimsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public ExpenseSubClaimsController(AtoCashDbContext context)
        {
            _context = context;
        }

        // GET: api/ExpenseSubClaims
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseSubClaim>>> GetExpenseSubClaims()
        {
            return await _context.ExpenseSubClaims.ToListAsync();
        }

        // GET: api/ExpenseSubClaims/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseSubClaim>> GetExpenseSubClaim(int id)
        {
            var expenseSubClaim = await _context.ExpenseSubClaims.FindAsync(id);

            if (expenseSubClaim == null)
            {
                return NotFound();
            }

            return expenseSubClaim;
        }


        // GET: api/ExpenseSubClaims/5
        [HttpGet("{id}")]
        [ActionName("GetExpenseSubClaimsByExpenseId")]
        public async Task<ActionResult<ExpenseSubClaim>> GetExpenseSubClaimsByExpenseId(int id)
        {
            var expenseSubClaims = await _context.ExpenseSubClaims.Where(e => e.ExpenseReimburseRequestId == id).ToListAsync();

            if (expenseSubClaims.Count == 0)
            {
                return NotFound();
            }

            return Ok(expenseSubClaims);
        }
        // PUT: api/ExpenseSubClaims/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpenseSubClaim(int id, ExpenseSubClaim expenseSubClaim)
        {
            if (id != expenseSubClaim.Id)
            {
                return BadRequest();
            }

            _context.Entry(expenseSubClaim).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseSubClaimExists(id))
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

        // POST: api/ExpenseSubClaims
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExpenseSubClaim>> PostExpenseSubClaim(ExpenseSubClaim expenseSubClaim)
        {
            _context.ExpenseSubClaims.Add(expenseSubClaim);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpenseSubClaim", new { id = expenseSubClaim.Id }, expenseSubClaim);
        }

        // DELETE: api/ExpenseSubClaims/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenseSubClaim(int id)
        {
            var expenseSubClaim = await _context.ExpenseSubClaims.FindAsync(id);
            if (expenseSubClaim == null)
            {
                return NotFound();
            }

            _context.ExpenseSubClaims.Remove(expenseSubClaim);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpenseSubClaimExists(int id)
        {
            return _context.ExpenseSubClaims.Any(e => e.Id == id);
        }
    }
}
