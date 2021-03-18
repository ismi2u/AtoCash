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
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Admin, User")]
    public class ProjectManagementController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public ProjectManagementController(AtoCashDbContext context)
        {
            _context = context;
        }

        // GET: api/ProjectManagement
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectManagementDTO>>> GetProjectManagement()
        {
            List<ProjectManagementDTO> ListProjectManagementDTO = new List<ProjectManagementDTO>();

            var projManagements = await _context.ProjectManagements.ToListAsync();

            foreach (ProjectManagement projMgt in projManagements)
            {
                ProjectManagementDTO projectmgmtDTO = new ProjectManagementDTO
                {
                    Id = projMgt.Id,
                    ProjectId = projMgt.ProjectId,
                    EmployeeId = projMgt.EmployeeId
                };

                ListProjectManagementDTO.Add(projectmgmtDTO);

            }

            return ListProjectManagementDTO;
        }

        // GET: api/ProjectManagement/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectManagementDTO>> GetProjectManagement(int id)
        {
            ProjectManagementDTO projManagementDTO = new ProjectManagementDTO();

            var projManagement = await _context.ProjectManagements.FindAsync(id);

            if (projManagement == null)
            {
                return NotFound();
            }

            projManagementDTO.Id = projManagement.Id;
            projManagementDTO.ProjectId = projManagement.ProjectId;
            projManagementDTO.EmployeeId = projManagement.EmployeeId;

            return projManagementDTO;
        }

       

            // PUT: api/ProjectManagement/5
        [HttpPut("{id}")]
        [Authorize(Roles = "AtominosAdmin, Admin")]
        public async Task<IActionResult> PutProjectManagement(int id, ProjectManagementDTO projectManagementDto)
        {
            if (id != projectManagementDto.Id)
            {
                return BadRequest(new RespStatus { Status = "Failure", Message = "Id is invalid" });
            }


            var projMgmt = await _context.ProjectManagements.FindAsync(id);

            projMgmt.Id = projectManagementDto.Id;
            projMgmt.ProjectId = projectManagementDto.ProjectId;
            projMgmt.EmployeeId = projectManagementDto.EmployeeId;

            _context.ProjectManagements.Update(projMgmt);
            //_context.Entry(projectDto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectManagementExists(id))
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

        // POST: api/ProjectManagement
        [HttpPost]
        [Authorize(Roles = "AtominosAdmin, Admin")]
        public async Task<ActionResult<ProjectManagement>> PostProjectManagement(ProjectManagementDTO projectManagementDTO)
        {
            ProjectManagement projectManagement = new ProjectManagement
            {
                ProjectId = projectManagementDTO.ProjectId,
                EmployeeId = projectManagementDTO.EmployeeId
            };


            _context.ProjectManagements.Add(projectManagement);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetProjectManagement", new { id = projectManagement.Id }, projectManagement);
        }

        // DELETE: api/ProjectManagement/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "AtominosAdmin, Admin")]
        public async Task<IActionResult> DeleteProjectManagement(int id)
        {
            var projectManagement = await _context.ProjectManagements.FindAsync(id);
            if (projectManagement == null)
            {
                return NotFound();
            }

            _context.ProjectManagements.Remove(projectManagement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectManagementExists(int id)
        {
            return _context.ProjectManagements.Any(e => e.Id == id);
        }
    }
}
