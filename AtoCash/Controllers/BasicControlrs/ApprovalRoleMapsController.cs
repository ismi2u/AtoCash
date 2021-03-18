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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Admin")]
    public class ApprovalRoleMapsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public ApprovalRoleMapsController(AtoCashDbContext context)
        {
            _context = context;
        }

        // GET: api/ApprovalRoleMaps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApprovalRoleMapDTO>>> GetApprovalRoleMaps()
        {
            List<ApprovalRoleMapDTO> ListApprovalRoleMapDTO = new List<ApprovalRoleMapDTO>();

            var approvalRoleMaps = await _context.ApprovalRoleMaps.ToListAsync();

            foreach (ApprovalRoleMap approvalRoleMap in approvalRoleMaps)
            {
                ApprovalRoleMapDTO approvalRoleMapDTO = new ApprovalRoleMapDTO
                {
                    Id = approvalRoleMap.Id,
                    ApprovalGroupId = approvalRoleMap.ApprovalGroupId,
                    ApprovalGroup = _context.ApprovalGroups.Find(approvalRoleMap.ApprovalGroupId).ApprovalGroupCode,
                    RoleId = approvalRoleMap.RoleId,
                    Role = _context.JobRoles.Find(approvalRoleMap.RoleId).RoleCode,
                    ApprovalLevelId = approvalRoleMap.ApprovalLevelId,
                    ApprovalLevel = _context.ApprovalLevels.Find(approvalRoleMap.ApprovalLevelId).Level

                };

                ListApprovalRoleMapDTO.Add(approvalRoleMapDTO);

            }

            return ListApprovalRoleMapDTO;
        }

        // GET: api/ApprovalRoleMaps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApprovalRoleMapDTO>> GetApprovalRoleMap(int id)
        {
            ApprovalRoleMapDTO approvalRoleMapDTO = new ApprovalRoleMapDTO();

            var approvalRoleMap = await _context.ApprovalRoleMaps.FindAsync(id);

            if (approvalRoleMap == null)
            {
                return NotFound();
            }

            approvalRoleMapDTO.Id = approvalRoleMap.Id;
            approvalRoleMapDTO.ApprovalGroupId = approvalRoleMap.ApprovalGroupId;
            approvalRoleMapDTO.RoleId = approvalRoleMap.RoleId;
            approvalRoleMapDTO.ApprovalLevelId = approvalRoleMap.ApprovalLevelId;

            return approvalRoleMapDTO;
        }

        // PUT: api/ApprovalRoleMaps/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApprovalRoleMap(int id, ApprovalRoleMapDTO approvalRoleMapDto)
        {
            if (id != approvalRoleMapDto.Id)
            {
                return BadRequest(new RespStatus { Status = "Failure", Message = "Id is invalid" });
            }

            var approvalRoleMap = await _context.ApprovalRoleMaps.FindAsync(id);



            approvalRoleMap.Id = approvalRoleMapDto.Id;
            approvalRoleMap.ApprovalGroupId = approvalRoleMapDto.ApprovalGroupId;
            approvalRoleMap.RoleId = approvalRoleMapDto.RoleId;
            approvalRoleMap.ApprovalLevelId = approvalRoleMapDto.ApprovalLevelId;

            _context.ApprovalRoleMaps.Update(approvalRoleMap);
            _context.Entry(approvalRoleMap).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApprovalRoleMapExists(id))
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

        // POST: api/ApprovalRoleMaps
        [HttpPost]
        public async Task<ActionResult<ApprovalRoleMap>> PostApprovalRoleMap(ApprovalRoleMapDTO approvalRoleMapDto)
        {
            ApprovalRoleMap approvalRoleMap = new ApprovalRoleMap
            {
                Id = approvalRoleMapDto.Id,
                ApprovalGroupId = approvalRoleMapDto.ApprovalGroupId,
                RoleId = approvalRoleMapDto.RoleId,
                ApprovalLevelId = approvalRoleMapDto.ApprovalLevelId
            };

            _context.ApprovalRoleMaps.Add(approvalRoleMap);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApprovalRoleMap", new { id = approvalRoleMap.Id }, approvalRoleMap);
        }

        // DELETE: api/ApprovalRoleMaps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApprovalRoleMap(int id)
        {
            var approvalRoleMap = await _context.ApprovalRoleMaps.FindAsync(id);
            if (approvalRoleMap == null)
            {
                return NotFound();
            }

            _context.ApprovalRoleMaps.Remove(approvalRoleMap);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApprovalRoleMapExists(int id)
        {
            return _context.ApprovalRoleMaps.Any(e => e.Id == id);
        }
    }
}
