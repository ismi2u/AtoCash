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
                ApprovalRoleMapDTO approvalRoleMapDTO = new ApprovalRoleMapDTO();

                approvalRoleMapDTO.Id = approvalRoleMap.Id;
                approvalRoleMapDTO.ApprovalGroupId = approvalRoleMap.ApprovalGroupId;
                approvalRoleMapDTO.RoleId = approvalRoleMap.RoleId;
                approvalRoleMapDTO.ApprovalLevel = approvalRoleMap.ApprovalLevel;

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
            approvalRoleMapDTO.ApprovalLevel = approvalRoleMap.ApprovalLevel;

            return approvalRoleMapDTO;
        }

        // PUT: api/ApprovalRoleMaps/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApprovalRoleMap(int id, ApprovalRoleMapDTO approvalRoleMapDto)
        {
            if (id != approvalRoleMapDto.Id)
            {
                return BadRequest();
            }

            var approvalRoleMap = await _context.ApprovalRoleMaps.FindAsync(id);



            approvalRoleMap.Id = approvalRoleMapDto.Id;
            approvalRoleMap.ApprovalGroupId = approvalRoleMapDto.ApprovalGroupId;
            approvalRoleMap.RoleId = approvalRoleMapDto.RoleId;
            approvalRoleMap.ApprovalLevel = approvalRoleMapDto.ApprovalLevel;

            _context.ApprovalRoleMaps.Update(approvalRoleMap);
            //_context.Entry(projectDto).State = EntityState.Modified;


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
            ApprovalRoleMap approvalRoleMap = new ApprovalRoleMap();

            approvalRoleMap.Id = approvalRoleMapDto.Id;
            approvalRoleMap.ApprovalGroupId = approvalRoleMapDto.ApprovalGroupId;
            approvalRoleMap.RoleId = approvalRoleMapDto.RoleId;
            approvalRoleMap.ApprovalLevel = approvalRoleMapDto.ApprovalLevel;

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
