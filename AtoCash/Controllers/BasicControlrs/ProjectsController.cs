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
    [Authorize(Roles = "AtominosAdmin, Finmgr, Admin, Manager, User")]
    public class ProjectsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public ProjectsController(AtoCashDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [ActionName("ProjectsForDropdown")]
        public async Task<ActionResult<IEnumerable<ProjectVM>>> GetProjectsForDropDown()
        {
            List<ProjectVM> ListProjectVM = new List<ProjectVM>();

            var projects = await _context.Projects.ToListAsync();
            foreach (Project project in projects)
            {
                ProjectVM projectVM = new ProjectVM
                {
                    Id = project.Id,
                    ProjectName = project.ProjectName,
                    ProjectDesc = project.ProjectDesc
                };

                ListProjectVM.Add(projectVM);
            }

            return ListProjectVM;

        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetProjects()
        {
            List<ProjectDTO> ListProjectDTO = new List<ProjectDTO>();

            var projects = await _context.Projects.ToListAsync();

            foreach (Project proj in projects)
            {
                ProjectDTO projectDTO = new ProjectDTO
                {
                    Id = proj.Id,
                    ProjectName = proj.ProjectName,
                    CostCentreId = proj.CostCentreId,
                    ProjectDesc = proj.ProjectDesc
                };

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
                return NoContent();
            }

            projectDTO.Id = proj.Id;
            projectDTO.ProjectName = proj.ProjectName;
            projectDTO.CostCentreId = proj.CostCentreId;
            projectDTO.ProjectDesc = proj.ProjectDesc;

            return projectDTO;

        }

        // GET: api/ProjectManagement/5
        [HttpGet("{id}")]
        [ActionName("GetEmployeeAssignedProjects")]
        public ActionResult<ProjectVM> GetEmployeeAssignedProjects(int id)
        {
            var listOfProjmgts = _context.ProjectManagements.Where(p => p.EmployeeId == id).ToList();

            List<ProjectVM> ListprojectVM = new List<ProjectVM>();

            if (listOfProjmgts != null)
            {
                foreach (var item in listOfProjmgts)
                {
                    ProjectVM project = new ProjectVM()
                    {
                        Id = item.ProjectId,
                        ProjectName = _context.Projects.Find(item.ProjectId).ProjectName
                    };
                    ListprojectVM.Add(project);

                }
                return Ok(ListprojectVM);
            }
            return Ok(new RespStatus { Status = "Success", Message = "No Projects Assigned to Employee" });
        }

        // PUT: api/Projects/5
        [HttpPut("{id}")]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<IActionResult> PutProject(int id, ProjectDTO projectDto)
        {
            if (id != projectDto.Id)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Id is invalid" });
            }

            var proj = await _context.Projects.FindAsync(id);
                
            proj.Id = projectDto.Id;
            proj.ProjectName = projectDto.ProjectName;
            proj.CostCentreId = projectDto.CostCentreId  ;
            proj.ProjectManagerId = projectDto.ProjectManagerId;
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
                    return NoContent();
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
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<ActionResult<Project>> PostProject(ProjectDTO projectDto)
        {
            var project = _context.Projects.Where(c => c.ProjectName == projectDto.ProjectName).FirstOrDefault();
            if (project != null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "ProjectName Already Exists" });
            }

            Project proj = new Project
            {
                ProjectName = projectDto.ProjectName,
                CostCentreId = projectDto.CostCentreId,
                ProjectManagerId = projectDto.ProjectManagerId,
                ProjectDesc = projectDto.ProjectDesc
            };

            _context.Projects.Add(proj);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = proj.Id }, proj);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var subProj = _context.SubProjects.Where(s => s.ProjectId == id).FirstOrDefault();
            if (subProj != null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Cant Delete the Project in Use" });
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NoContent();
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
