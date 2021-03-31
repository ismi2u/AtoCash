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
    public class SubProjectsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public SubProjectsController(AtoCashDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ActionName("SubProjectsForDropdown")]
        public async Task<ActionResult<IEnumerable<SubProjectVM>>> GetSubProjectsForDropDown()
        {
            List<SubProjectVM> ListSubProjectVM = new List<SubProjectVM>();

            var subProjects = await _context.SubProjects.ToListAsync();
            foreach (SubProject subProject in subProjects)
            {
                SubProjectVM subProjectVM = new SubProjectVM
                {
                    Id = subProject.Id,
                    SubProjectName = subProject.SubProjectName,
                    SubProjectDesc = subProject.SubProjectDesc
                };

                ListSubProjectVM.Add(subProjectVM);
            }

            return ListSubProjectVM;

        }


        [HttpGet("{id}")]
        [ActionName("GetSubProjectsForProjects")]
        public async Task<ActionResult<IEnumerable<SubProjectVM>>> GetSubProjectsForProjects(int id)
        {
            var listOfSubProject = await _context.SubProjects.Where(s => s.ProjectId == id).ToListAsync();

            List<SubProjectVM> ListSubProjectVM = new List<SubProjectVM>();

            if (listOfSubProject != null)
            {
                foreach (var item in listOfSubProject)
                {
                    SubProjectVM subproject = new SubProjectVM()
                    {
                        Id = item.Id,
                        SubProjectName = item.SubProjectName
                    };
                    ListSubProjectVM.Add(subproject);

                }
                return Ok(listOfSubProject);
            }
            return Ok(new RespStatus { Status = "Success", Message = "No SubProjects Assigned to Employee" });

        }

        [HttpGet]
        [ActionName("GetSubProjectsForDropdown")]
        public async Task<ActionResult<IEnumerable<SubProjectVM>>> GetSubProjectsForDropdown()
        {
            List<SubProjectVM> ListSubProjectVM = new List<SubProjectVM>();

            var subProjects = await _context.SubProjects.ToListAsync();
            foreach (SubProject subProject in subProjects)
            {
                SubProjectVM subProjectVM = new SubProjectVM
                {
                    Id = subProject.Id,
                    SubProjectName = subProject.SubProjectName
                };

                ListSubProjectVM.Add(subProjectVM);
            }

            return ListSubProjectVM;

        }


        // GET: api/SubProjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubProjectDTO>>> GetSubProjects()
        {
            List<SubProjectDTO> ListSubProjectDTO = new List<SubProjectDTO>();

            var SubProjects = await _context.SubProjects.ToListAsync();

            foreach (SubProject SubProj in SubProjects)
            {
                SubProjectDTO SubProjectDTO = new SubProjectDTO
                {
                    Id = SubProj.Id,
                    ProjectId = SubProj.ProjectId,
                    ProjectName = _context.Projects.Find(SubProj.ProjectId).ProjectName,
                    SubProjectName = SubProj.SubProjectName,
                    SubProjectDesc = SubProj.SubProjectDesc
                };

                ListSubProjectDTO.Add(SubProjectDTO);

            }

            return ListSubProjectDTO;
        }

        // GET: api/SubProjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubProjectDTO>> GetSubProject(int id)
        {
            SubProjectDTO subProjectDTO = new SubProjectDTO();

            var SubProj = await _context.SubProjects.FindAsync(id);

            if (SubProj == null)
            {
                return NoContent();
            }

            subProjectDTO.Id = SubProj.Id;
            subProjectDTO.SubProjectName = SubProj.SubProjectName;
            subProjectDTO.ProjectId = SubProj.ProjectId;
            subProjectDTO.SubProjectDesc = SubProj.SubProjectDesc;

            return subProjectDTO;
        }

        // PUT: api/SubProjects/5
        [HttpPut("{id}")]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<IActionResult> PutSubProject(int id, SubProjectDTO subProjectDto)
        {
            if (id != subProjectDto.Id)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Id is invalid" });
            }

            var subProj = await _context.SubProjects.FindAsync(id);

            subProj.Id = subProjectDto.Id;
            subProj.SubProjectName = subProjectDto.SubProjectName;
            subProj.SubProjectDesc = subProjectDto.SubProjectDesc;

            _context.SubProjects.Update(subProj);
            //_context.Entry(SubProjects).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubProjectExists(id))
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

        // POST: api/SubProjects
        [HttpPost]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<ActionResult<SubProject>> PostSubProject(SubProjectDTO subProjectDto)
        {
            var subproject = _context.SubProjects.Where(c => c.SubProjectName == subProjectDto.SubProjectName).FirstOrDefault();
            if (subproject != null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Sub Project Already Exists" });
            }

            SubProject SubProj = new SubProject
            {
                ProjectId = subProjectDto.ProjectId,
                SubProjectName = subProjectDto.SubProjectName,
                SubProjectDesc = subProjectDto.SubProjectDesc
            };

            _context.SubProjects.Add(SubProj);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubProject", new { id = SubProj.Id }, SubProj);
        }

        // DELETE: api/SubProjects/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<IActionResult> DeleteSubProject(int id)
        {
            var wrktask = _context.WorkTasks.Where(w => w.SubProjectId == id).FirstOrDefault();
            if (wrktask != null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Cant Delete the SubProject in Use" });
            }

            var subProject = await _context.SubProjects.FindAsync(id);
            if (subProject == null)
            {
                return NoContent();
            }

            _context.SubProjects.Remove(subProject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubProjectExists(int id)
        {
            return _context.SubProjects.Any(e => e.Id == id);
        }
    }
}
