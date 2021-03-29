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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Finmgr, Admin, User")]
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

            List<ApprovalLevelDTO> ListApprovalLevelDTO = new();

            var approvalLevels = await _context.ApprovalLevels.ToListAsync();

            foreach (ApprovalLevel approvalLevel in approvalLevels)
            {
                ApprovalLevelDTO approvalLevelDTO = new()
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

            ApprovalLevelDTO approvalLevelDTO = new();

            var approvalLevel = await _context.ApprovalLevels.FindAsync(id);

            if (approvalLevel == null)
            {
                return NoContent();
            }

            approvalLevelDTO.Id = approvalLevel.Id;
            approvalLevelDTO.Level = approvalLevel.Level;
            approvalLevelDTO.LevelDesc = approvalLevel.LevelDesc;

            return approvalLevelDTO;
        }

        // PUT: api/ApprovalLevels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<IActionResult> PutApprovalLevel(int id, ApprovalLevelDTO approvalLevelDTO)
        {
            if (id != approvalLevelDTO.Id)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Id state is invalid" });
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
                    return NoContent();
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
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<ActionResult<ApprovalLevel>> PostApprovalLevel(ApprovalLevelDTO approvalLevelDto)
        {

            var aprlevel = _context.ApprovalLevels.Where(a => a.Level == approvalLevelDto.Level).FirstOrDefault();
            if (aprlevel != null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Approval Level Already Exists" });
            }

            
            if (approvalLevelDto.Level !=  _context.ApprovalLevels.Select(x => x.Level).Max() + 1)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Approval Level need to be Linear integer" });
            }

            ApprovalLevel approvalLevel = new()
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
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<IActionResult> DeleteApprovalLevel(int id)
        {
            var approvalLevel = await _context.ApprovalLevels.FindAsync(id);
            if (approvalLevel == null)
            {
                return NoContent();
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
