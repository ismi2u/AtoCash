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
using AtoCash.Authentication;

namespace AtoCash.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
      [Authorize(Roles = "AtominosAdmin, Admin")]
    public class JobRolesController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public JobRolesController(AtoCashDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ActionName("JobRolesForDropdown")]
        public async Task<ActionResult<IEnumerable<JobRoleVM>>> GetJobRolesForDropDown()
        {
            List<JobRoleVM> ListJobRoleVM = new List<JobRoleVM>();

            var jobRoles = await _context.JobRoles.ToListAsync();
            foreach (JobRole jobRole in jobRoles)
            {
                JobRoleVM jobRoleVM = new JobRoleVM
                {
                    Id = jobRole.Id,
                    RoleCode = jobRole.RoleCode
                };

                ListJobRoleVM.Add(jobRoleVM);
            }

            return ListJobRoleVM;

        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobRole>>> GetRoles()
        {
            return await _context.JobRoles.ToListAsync();
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobRole>> GetRole(int id)
        {
            var role = await _context.JobRoles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, JobRole role)
        {
            if (id != role.Id)
            {
                return BadRequest(new RespStatus { Status = "Failure", Message = "Id is invalid" });
            }

            var jRole = _context.JobRoles.Where(c => c.RoleCode == role.RoleCode).FirstOrDefault();
            if (jRole != null)
            {
                return BadRequest(new RespStatus { Status = "Failure", Message = "JobRole Already Exists" });
            }


            _context.Entry(role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
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

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<JobRole>> PostRole(JobRole role)
        {
            var jRole = _context.JobRoles.Where(c => c.RoleCode == role.RoleCode).FirstOrDefault();
            if (jRole != null)
            {
                return BadRequest(new RespStatus { Status = "Failure", Message = "JobRole Already Exists" });
            }
            _context.JobRoles.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRole", new { id = role.Id }, role);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.JobRoles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.JobRoles.Remove(role);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoleExists(int id)
        {
            return _context.JobRoles.Any(e => e.Id == id);
        }
    }
}
