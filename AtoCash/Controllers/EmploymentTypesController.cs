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

namespace AtoCash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class EmploymentTypesController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public EmploymentTypesController(AtoCashDbContext context)
        {
            _context = context;
        }

        // GET: api/EmploymentTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmploymentType>>> GetEmploymentTypes()
        {
            return await _context.EmploymentTypes.ToListAsync();
        }

        // GET: api/EmploymentTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmploymentType>> GetEmploymentType(int id)
        {
            var employmentType = await _context.EmploymentTypes.FindAsync(id);

            if (employmentType == null)
            {
                return NotFound();
            }

            return employmentType;
        }

        // PUT: api/EmploymentTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmploymentType(int id, EmploymentType employmentType)
        {
            if (id != employmentType.Id)
            {
                return BadRequest();
            }

            _context.Entry(employmentType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmploymentTypeExists(id))
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

        // POST: api/EmploymentTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmploymentType>> PostEmploymentType(EmploymentType employmentType)
        {
            _context.EmploymentTypes.Add(employmentType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmploymentType", new { id = employmentType.Id }, employmentType);
        }

        // DELETE: api/EmploymentTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmploymentType(int id)
        {
            var employmentType = await _context.EmploymentTypes.FindAsync(id);
            if (employmentType == null)
            {
                return NotFound();
            }

            _context.EmploymentTypes.Remove(employmentType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmploymentTypeExists(int id)
        {
            return _context.EmploymentTypes.Any(e => e.Id == id);
        }
    }
}
