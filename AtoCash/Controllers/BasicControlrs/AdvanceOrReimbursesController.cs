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
    public class AdvanceOrReimbursesController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public AdvanceOrReimbursesController(AtoCashDbContext context)
        {
            _context = context;
        }

        // GET: api/AdvanceOrReimburses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdvanceOrReimburse>>> GetAdvanceOrReimburseTypes()
        {
            return await _context.AdvanceOrReimburseTypes.ToListAsync();
        }

        // GET: api/AdvanceOrReimburses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdvanceOrReimburse>> GetAdvanceOrReimburse(int id)
        {
            var advanceOrReimburse = await _context.AdvanceOrReimburseTypes.FindAsync(id);

            if (advanceOrReimburse == null)
            {
                return NotFound();
            }

            return advanceOrReimburse;
        }

        // PUT: api/AdvanceOrReimburses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdvanceOrReimburse(int id, AdvanceOrReimburse advanceOrReimburse)
        {
            if (id != advanceOrReimburse.Id)
            {
                return BadRequest();
            }

            _context.Entry(advanceOrReimburse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdvanceOrReimburseExists(id))
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

        // POST: api/AdvanceOrReimburses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdvanceOrReimburse>> PostAdvanceOrReimburse(AdvanceOrReimburse advanceOrReimburse)
        {
            _context.AdvanceOrReimburseTypes.Add(advanceOrReimburse);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdvanceOrReimburse", new { id = advanceOrReimburse.Id }, advanceOrReimburse);
        }

        // DELETE: api/AdvanceOrReimburses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdvanceOrReimburse(int id)
        {
            var advanceOrReimburse = await _context.AdvanceOrReimburseTypes.FindAsync(id);
            if (advanceOrReimburse == null)
            {
                return NotFound();
            }

            _context.AdvanceOrReimburseTypes.Remove(advanceOrReimburse);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdvanceOrReimburseExists(int id)
        {
            return _context.AdvanceOrReimburseTypes.Any(e => e.Id == id);
        }
    }
}
