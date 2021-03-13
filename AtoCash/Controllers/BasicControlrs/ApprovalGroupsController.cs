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
    public class ApprovalGroupsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public ApprovalGroupsController(AtoCashDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [ActionName("ApprovalGroupsForDropdown")]
        public async Task<ActionResult<IEnumerable<ApprovalGroupVM>>> GetApprovalGroupsForDropDown()
        {
            List<ApprovalGroupVM> ListApprovalGroupVM = new List<ApprovalGroupVM>();

            var approvalGroups = await _context.ApprovalGroups.ToListAsync();
            foreach (ApprovalGroup approvalGroup in approvalGroups)
            {
                ApprovalGroupVM approvalGroupVM = new ApprovalGroupVM
                {
                    Id = approvalGroup.Id,
                    ApprovalGroupCode = approvalGroup.ApprovalGroupCode
                };

                ListApprovalGroupVM.Add(approvalGroupVM);
            }

            return ListApprovalGroupVM;

        }
        // GET: api/ApprovalGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApprovalGroup>>> GetApprovalGroups()
        {
            return await _context.ApprovalGroups.ToListAsync();
        }

        // GET: api/ApprovalGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApprovalGroup>> GetApprovalGroup(int id)
        {
            var approvalGroup = await _context.ApprovalGroups.FindAsync(id);

            if (approvalGroup == null)
            {
                return NotFound();
            }

            return approvalGroup;
        }

        // PUT: api/ApprovalGroups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApprovalGroup(int id, ApprovalGroup approvalGroup)
        {
            if (id != approvalGroup.Id)
            {
                return BadRequest();
            }

            _context.Entry(approvalGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApprovalGroupExists(id))
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

        // POST: api/ApprovalGroups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApprovalGroup>> PostApprovalGroup(ApprovalGroup approvalGroup)
        {
            _context.ApprovalGroups.Add(approvalGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApprovalGroup", new { id = approvalGroup.Id }, approvalGroup);
        }

        // DELETE: api/ApprovalGroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApprovalGroup(int id)
        {
            var approvalGroup = await _context.ApprovalGroups.FindAsync(id);
            if (approvalGroup == null)
            {
                return NotFound();
            }

            _context.ApprovalGroups.Remove(approvalGroup);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApprovalGroupExists(int id)
        {
            return _context.ApprovalGroups.Any(e => e.Id == id);
        }
    }
}