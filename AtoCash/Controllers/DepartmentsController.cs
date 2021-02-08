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
    public class DepartmentsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public DepartmentsController(AtoCashDbContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetDepartments()
        {
            List<DepartmentDTO> ListDepartmentDTO = new List<DepartmentDTO>();

            var departments = await _context.Departments.ToListAsync();

            foreach (Department department in departments)
            {
                DepartmentDTO departmentDTO = new DepartmentDTO();

                departmentDTO.Id = department.Id;
                departmentDTO.DeptCode = department.DeptCode;
                departmentDTO.DeptName = department.DeptName;
                departmentDTO.CostCentreId = department.CostCentreId;

                ListDepartmentDTO.Add(departmentDTO);

            }

            return ListDepartmentDTO;
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDTO>> GetDepartment(int id)
        {
            DepartmentDTO departmentDTO = new DepartmentDTO();

            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            departmentDTO.Id = department.Id;
            departmentDTO.DeptCode = department.DeptCode;
            departmentDTO.DeptName = department.DeptName;
            departmentDTO.CostCentreId = department.CostCentreId;

            return departmentDTO;
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, DepartmentDTO departmentDto)
        {
            if (id != departmentDto.Id)
            {
                return BadRequest();
            }

            var department = await _context.Departments.FindAsync(id);

            department.Id = departmentDto.Id;
            department.DeptCode = departmentDto.DeptCode;
            department.DeptName = departmentDto.DeptName;
            department.CostCentreId = departmentDto.CostCentreId;

            _context.Departments.Update(department);
            //_context.Entry(projectDto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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

        // POST: api/Departments
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(DepartmentDTO departmentDto)
        {
            Department department = new Department();

            department.DeptCode = departmentDto.DeptCode;
            department.DeptName = departmentDto.DeptName;
            department.CostCentreId = departmentDto.CostCentreId;

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepartment", new { id = department.Id }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
