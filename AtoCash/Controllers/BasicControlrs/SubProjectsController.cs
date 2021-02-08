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
    public class SubProjectsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public SubProjectsController(AtoCashDbContext context)
        {
            _context = context;
        }

        // GET: api/SubProjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubProjectDTO>>> GetSubProjects()
        {
            List<SubProjectDTO> ListSubProjectDTO = new List<SubProjectDTO>();

            var SubProjects = await _context.SubProjects.ToListAsync();

            foreach (SubProject SubProj in SubProjects)
            {
                SubProjectDTO SubProjectDTO = new SubProjectDTO();
                SubProjectDTO.Id = SubProj.Id;
                SubProjectDTO.SubProjectName = SubProj.SubProjectName;
                SubProjectDTO.SubProjectDesc = SubProj.SubProjectDesc;

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
                return NotFound();
            }

            subProjectDTO.Id = SubProj.Id;
            subProjectDTO.SubProjectName = SubProj.SubProjectName;
            subProjectDTO.SubProjectDesc = SubProj.SubProjectDesc;

            return subProjectDTO;
        }

        // PUT: api/SubProjects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubProject(int id, SubProjectDTO subProjectDto)
        {
            if (id != subProjectDto.Id)
            {
                return BadRequest();
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
                    return NotFound();
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
        public async Task<ActionResult<SubProject>> PostSubProject(SubProjectDTO subProjectDto)
        {
            SubProject SubProj = new SubProject();

            SubProj.SubProjectName = subProjectDto.SubProjectName;
            SubProj.SubProjectDesc = subProjectDto.SubProjectDesc;

            _context.SubProjects.Add(SubProj);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubProject", new { id = SubProj.Id }, SubProj);
        }

        // DELETE: api/SubProjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubProject(int id)
        {
            var subProject = await _context.SubProjects.FindAsync(id);
            if (subProject == null)
            {
                return NotFound();
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
