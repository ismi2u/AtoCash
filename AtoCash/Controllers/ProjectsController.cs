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
    public class ProjectsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public ProjectsController(AtoCashDbContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetProjects()
        {
            List<ProjectDTO> ListProjectDTO = new List<ProjectDTO>();

            var projects = await _context.Projects.ToListAsync();

            foreach (Project proj in projects)
            {
                ProjectDTO projectDTO = new ProjectDTO();
                projectDTO.Id = proj.Id;
                projectDTO.ProjectName = proj.ProjectName;
                projectDTO.CostCentreId = proj.CostCentreId;
                projectDTO.ProjectDesc = proj.ProjectDesc;

                ListProjectDTO.Add(projectDTO);

            }

            return ListProjectDTO;
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDTO>> GetProject(int id)
        {
           
            ProjectDTO projectDTO = new ProjectDTO();

            var proj = await _context.Projects.FindAsync(id);

            if (proj == null)
            {
                return NotFound();
            }

            projectDTO.Id = proj.Id;
            projectDTO.ProjectName = proj.ProjectName;
            projectDTO.CostCentreId = proj.CostCentreId;
            projectDTO.ProjectDesc = proj.ProjectDesc;

            return projectDTO;

        }

        // PUT: api/Projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, ProjectDTO projectDto)
        {
            if (id != projectDto.Id)
            {
                return BadRequest();
            }

           var proj = await  _context.Projects.FindAsync(id);

            proj.Id = projectDto.Id;
            proj.ProjectName = projectDto.ProjectName;
            proj.CostCentreId = projectDto.CostCentreId;
            proj.ProjectDesc = projectDto.ProjectDesc;

            _context.Projects.Update(proj);
            //_context.Entry(projectDto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(ProjectDTO projectDto)
        {

            Project proj = new Project();

            proj.ProjectName = projectDto.ProjectName;
            proj.CostCentreId = projectDto.CostCentreId;
            proj.ProjectDesc = projectDto.ProjectDesc;

            _context.Projects.Add(proj);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = proj.Id }, proj);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
