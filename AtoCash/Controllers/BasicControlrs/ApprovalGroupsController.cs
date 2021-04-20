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
    [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr, User")]
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
            List<ApprovalGroupVM> ListApprovalGroupVM = new();

            var approvalGroups = await _context.ApprovalGroups.ToListAsync();
            foreach (ApprovalGroup approvalGroup in approvalGroups)
            {
                ApprovalGroupVM approvalGroupVM = new()
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
                return NoContent();
            }

            return approvalGroup;
        }

        // PUT: api/ApprovalGroups/5
        [HttpPut("{id}")]
        [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr")]
        public async Task<IActionResult> PutApprovalGroup(int id, ApprovalGroup approvalGroup)
        {
            if (id != approvalGroup.Id)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Id is Invalid" });
            }

            var agroup = await _context.ApprovalGroups.FindAsync(id);
            agroup.ApprovalGroupDesc = approvalGroup.ApprovalGroupDesc;

            _context.ApprovalGroups.Update(agroup);

            _context.Entry(approvalGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApprovalGroupExists(id))
                {
                    return Conflict(new RespStatus { Status = "Failure", Message = "ApprovalGroup is invalid" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new RespStatus { Status = "Success", Message = "ApprovalGroup Details Updated!" });
        }

        // POST: api/ApprovalGroups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr")]
        public async Task<ActionResult<ApprovalGroup>> PostApprovalGroup(ApprovalGroup approvalGroup)
        {
            var aprGrpCode = _context.ApprovalGroups.Where(a => a.ApprovalGroupCode == approvalGroup.ApprovalGroupCode).FirstOrDefault();
            if (aprGrpCode != null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Approval Group Code Already Exists" });
            }

            _context.ApprovalGroups.Add(approvalGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApprovalGroup", new { id = approvalGroup.Id }, approvalGroup);
        }

        // DELETE: api/ApprovalGroups/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr")]
        public async Task<IActionResult> DeleteApprovalGroup(int id)
        {
            var approvalGroup = await _context.ApprovalGroups.FindAsync(id);
            if (approvalGroup == null)
            {
                return NoContent();
            }

            _context.ApprovalGroups.Remove(approvalGroup);
            await _context.SaveChangesAsync();

            return Ok(new RespStatus { Status = "Success", Message = "Approval Group Deleted!" });
        }

        private bool ApprovalGroupExists(int id)
        {
            return _context.ApprovalGroups.Any(e => e.Id == id);
        }
    }
}
