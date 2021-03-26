using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtoCash.Data;
using AtoCash.Models;
using AtoCash.Authentication;

namespace AtoCash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeStatusController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public EmployeeStatusController(AtoCashDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeStatus>>> GetEmployeeStatus()
        {
            return await _context.EmployeeStatus.ToListAsync();
        }

        // GET: api/EmployeeStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeStatus>> GetEmployeeStatus(int id)
        {
            var employeeStatus = await _context.EmployeeStatus.FindAsync(id);

            if (employeeStatus == null)
            {
                return NotFound();
            }

            return employeeStatus;
        }

        // PUT: api/EmployeeStatus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeStatus(int id, EmployeeStatus employeeStatus)
        {
            if (id != employeeStatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(employeeStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeStatusExists(id))
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

        // POST: api/EmployeeStatus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeStatus>> PostEmployeeStatus(EmployeeStatus employeeStatus)
        {
            var eStatus = _context.EmployeeStatus.Where(e => e.EmpStatus == employeeStatus.EmpStatus).FirstOrDefault();
            if (eStatus != null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Status Already Exists" });
            }

            _context.EmployeeStatus.Add(employeeStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeStatus", new { id = employeeStatus.Id }, employeeStatus);
        }

        // DELETE: api/EmployeeStatus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeStatus(int id)
        {
            var employeeStatus = await _context.EmployeeStatus.FindAsync(id);
            if (employeeStatus == null)
            {
                return NotFound();
            }

            _context.EmployeeStatus.Remove(employeeStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeStatusExists(int id)
        {
            return _context.EmployeeStatus.Any(e => e.Id == id);
        }
    }
}
