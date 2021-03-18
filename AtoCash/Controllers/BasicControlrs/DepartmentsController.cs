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
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Admin, Manager, User")]
    public class DepartmentsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public DepartmentsController(AtoCashDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [ActionName("DepartmentsForDropdown")]
        public async Task<ActionResult<IEnumerable<DepartmentVM>>> GetDepartmentsForDropdown()
        {
            List<DepartmentVM> ListDepartmentVM = new List<DepartmentVM>();

            var departments = await _context.Departments.ToListAsync();
            foreach (Department department in departments)
            {
                DepartmentVM departmentVM = new DepartmentVM
                {
                    Id = department.Id,
                     DeptName = department.DeptName
                };

                ListDepartmentVM.Add(departmentVM);
            }
            return ListDepartmentVM;

        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetDepartments()
        {
            List<DepartmentDTO> ListDepartmentDTO = new List<DepartmentDTO>();

            var departments = await _context.Departments.ToListAsync();

            foreach (Department department in departments)
            {
                DepartmentDTO departmentDTO = new DepartmentDTO
                {
                    Id = department.Id,
                    DeptCode = department.DeptCode,
                    DeptName = department.DeptName,
                    CostCentreId = department.CostCentreId
                };

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
        [Authorize(Roles = "AtominosAdmin, Admin")]
        public async Task<IActionResult> PutDepartment(int id, DepartmentDTO departmentDto)
        {
            if (id != departmentDto.Id)
            {
                return BadRequest(new Authentication.RespStatus { Status = "Failure", Message = "Id not Valid for Department" });
            }

            var dept = _context.Departments.Where(c => c.DeptCode == departmentDto.DeptCode).FirstOrDefault();
            if (dept != null)
            {
                return BadRequest(new RespStatus { Status = "Failure", Message = "Department Already Exists" });
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
        [Authorize(Roles = "AtominosAdmin, Admin")]
        public async Task<ActionResult<Department>> PostDepartment(DepartmentDTO departmentDto)
        {
            var dept = _context.Departments.Where(c => c.DeptCode == departmentDto.DeptCode).FirstOrDefault();
            if (dept != null)
            {
                return BadRequest(new RespStatus { Status = "Failure", Message = "Department Already Exists" });
            }

            Department department = new Department
            {
                DeptCode = departmentDto.DeptCode,
                DeptName = departmentDto.DeptName,
                CostCentreId = departmentDto.CostCentreId
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepartment", new { id = department.Id }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "AtominosAdmin, Admin")]
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
