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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Admin")]
    public class ApprovalLevelsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public ApprovalLevelsController(AtoCashDbContext context)
        {
            _context = context;
        }

        // GET: api/ApprovalLevels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApprovalLevelDTO>>> GetApprovalLevels()
        {

            List<ApprovalLevelDTO> ListApprovalLevelDTO = new List<ApprovalLevelDTO>();

            var approvalLevels = await _context.ApprovalLevels.ToListAsync();

            foreach (ApprovalLevel approvalLevel in approvalLevels)
            {
                ApprovalLevelDTO approvalLevelDTO = new ApprovalLevelDTO
                {
                    Id = approvalLevel.Id,
                    Level = approvalLevel.Level,
                    LevelDesc = approvalLevel.LevelDesc
                };

                ListApprovalLevelDTO.Add(approvalLevelDTO);
            }

            return ListApprovalLevelDTO;

        }

        // GET: api/ApprovalLevels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApprovalLevelDTO>> GetApprovalLevel(int id)
        {

            ApprovalLevelDTO approvalLevelDTO = new ApprovalLevelDTO();

            var approvalLevel = await _context.ApprovalLevels.FindAsync(id);

            if (approvalLevel == null)
            {
                return NotFound();
            }

            approvalLevelDTO.Id = approvalLevel.Id;
            approvalLevelDTO.Level = approvalLevel.Level;
            approvalLevelDTO.LevelDesc = approvalLevel.LevelDesc;

            return approvalLevelDTO;
        }

        // PUT: api/ApprovalLevels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApprovalLevel(int id, ApprovalLevelDTO approvalLevelDTO)
        {
            if (id != approvalLevelDTO.Id)
            {
                return BadRequest();
            }

            var approvalLevel = await _context.ApprovalLevels.FindAsync(id);

            approvalLevel.Id = approvalLevelDTO.Id;
            approvalLevel.Level = approvalLevelDTO.Level;
            approvalLevel.LevelDesc = approvalLevelDTO.LevelDesc;

            _context.ApprovalLevels.Update(approvalLevel);
            //_context.Entry(expenseReimburseRequestDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApprovalLevelExists(id))
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

        // POST: api/ApprovalLevels
        [HttpPost]
        public async Task<ActionResult<ApprovalLevel>> PostApprovalLevel(ApprovalLevelDTO approvalLevelDto)
        {
            ApprovalLevel approvalLevel = new ApprovalLevel
            {
                Level = approvalLevelDto.Level,
                LevelDesc = approvalLevelDto.LevelDesc
            };


            _context.ApprovalLevels.Add(approvalLevel);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetApprovalLevel", new { id = approvalLevel.Id }, approvalLevel);
        }

        // DELETE: api/ApprovalLevels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApprovalLevel(int id)
        {
            var approvalLevel = await _context.ApprovalLevels.FindAsync(id);
            if (approvalLevel == null)
            {
                return NotFound();
            }

            _context.ApprovalLevels.Remove(approvalLevel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApprovalLevelExists(int id)
        {
            return _context.ApprovalLevels.Any(e => e.Id == id);
        }
    }
}