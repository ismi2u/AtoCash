﻿using System;
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
    [Route("api/[controller]/[Action]")]
    [ApiController]
      [Authorize(Roles = "AtominosAdmin, Admin")]
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
                    SubProjectName = subProject.SubProjectName
                };

                ListSubProjectVM.Add(subProjectVM);
            }

            return ListSubProjectVM;

        }

        [HttpGet]
        [ActionName("SubProjectsByProjectForDropdown")]
        public async Task<ActionResult<IEnumerable<SubProjectVM>>> GetSubProjectsByProjectForDropdown(int Id)
        {
            List<SubProjectVM> ListSubProjectVM = new List<SubProjectVM>();

            var subProjects = await _context.SubProjects.Where(s => s.ProjectId==Id).ToListAsync();
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
            SubProject SubProj = new SubProject
            {
                SubProjectName = subProjectDto.SubProjectName,
                SubProjectDesc = subProjectDto.SubProjectDesc
            };

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