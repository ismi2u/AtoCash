using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtoCash.Data;
using AtoCash.Models;

namespace AtoCash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisbursementsAndClaimsMastersController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public DisbursementsAndClaimsMastersController(AtoCashDbContext context)
        {
            _context = context;
        }

        // GET: api/DisbursementsAndClaimsMasters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisbursementsAndClaimsMasterDTO>>> GetDisbursementsAndClaimsMasters()
        {
            List<DisbursementsAndClaimsMasterDTO> ListDisbursementsAndClaimsMasterDTO = new List<DisbursementsAndClaimsMasterDTO>();

            var disbursementsAndClaimsMasters = await _context.DisbursementsAndClaimsMasters.ToListAsync();

            foreach (DisbursementsAndClaimsMaster disbursementsAndClaimsMaster in disbursementsAndClaimsMasters)
            {
                DisbursementsAndClaimsMasterDTO disbursementsAndClaimsMasterDTO = new DisbursementsAndClaimsMasterDTO();

                disbursementsAndClaimsMasterDTO.Id = disbursementsAndClaimsMaster.Id;
                disbursementsAndClaimsMasterDTO.EmployeeId = disbursementsAndClaimsMaster.EmployeeId;
                disbursementsAndClaimsMasterDTO.PettyCashRequestId = disbursementsAndClaimsMaster.PettyCashRequestId;
                disbursementsAndClaimsMasterDTO.ExpenseReimburseReqId = disbursementsAndClaimsMaster.ExpenseReimburseReqId;
                disbursementsAndClaimsMasterDTO.AdvanceOrReimburseId = disbursementsAndClaimsMaster.AdvanceOrReimburseId;
                disbursementsAndClaimsMasterDTO.ProjectId = disbursementsAndClaimsMaster.ProjectId;
                disbursementsAndClaimsMasterDTO.SubProjectId = disbursementsAndClaimsMaster.SubProjectId;
                disbursementsAndClaimsMasterDTO.WorkTaskId = disbursementsAndClaimsMaster.WorkTaskId;
                disbursementsAndClaimsMasterDTO.RecordDate = disbursementsAndClaimsMaster.RecordDate;
                disbursementsAndClaimsMasterDTO.Amount = disbursementsAndClaimsMaster.Amount;
                disbursementsAndClaimsMasterDTO.CostCentreId = disbursementsAndClaimsMaster.CostCentreId;
                disbursementsAndClaimsMasterDTO.ApprovalStatusId = disbursementsAndClaimsMaster.ApprovalStatusId;

                ListDisbursementsAndClaimsMasterDTO.Add(disbursementsAndClaimsMasterDTO);

            }

            return ListDisbursementsAndClaimsMasterDTO;
        }

        // GET: api/DisbursementsAndClaimsMasters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DisbursementsAndClaimsMasterDTO>> GetDisbursementsAndClaimsMaster(int id)
        {

            DisbursementsAndClaimsMasterDTO disbursementsAndClaimsMasterDTO = new DisbursementsAndClaimsMasterDTO();

            var disbursementsAndClaimsMaster = await _context.DisbursementsAndClaimsMasters.FindAsync(id);

            if (disbursementsAndClaimsMaster == null)
            {
                return NotFound();
            }

            disbursementsAndClaimsMasterDTO.Id = disbursementsAndClaimsMaster.Id;
            disbursementsAndClaimsMasterDTO.EmployeeId = disbursementsAndClaimsMaster.EmployeeId;
            disbursementsAndClaimsMasterDTO.PettyCashRequestId = disbursementsAndClaimsMaster.PettyCashRequestId;
            disbursementsAndClaimsMasterDTO.ExpenseReimburseReqId = disbursementsAndClaimsMaster.ExpenseReimburseReqId;
            disbursementsAndClaimsMasterDTO.AdvanceOrReimburseId = disbursementsAndClaimsMaster.AdvanceOrReimburseId;
            disbursementsAndClaimsMasterDTO.ProjectId = disbursementsAndClaimsMaster.ProjectId;
            disbursementsAndClaimsMasterDTO.SubProjectId = disbursementsAndClaimsMaster.SubProjectId;
            disbursementsAndClaimsMasterDTO.WorkTaskId = disbursementsAndClaimsMaster.WorkTaskId;
            disbursementsAndClaimsMasterDTO.RecordDate = disbursementsAndClaimsMaster.RecordDate;
            disbursementsAndClaimsMasterDTO.Amount = disbursementsAndClaimsMaster.Amount;
            disbursementsAndClaimsMasterDTO.CostCentreId = disbursementsAndClaimsMaster.CostCentreId;
            disbursementsAndClaimsMasterDTO.ApprovalStatusId = disbursementsAndClaimsMaster.ApprovalStatusId;

            return disbursementsAndClaimsMasterDTO;
        }

        // PUT: api/DisbursementsAndClaimsMasters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDisbursementsAndClaimsMaster(int id, DisbursementsAndClaimsMasterDTO disbursementsAndClaimsMasterDto)
        {
            if (id != disbursementsAndClaimsMasterDto.Id)
            {
                return BadRequest();
            }

            var disbursementsAndClaimsMaster = await _context.DisbursementsAndClaimsMasters.FindAsync(id);

            disbursementsAndClaimsMaster.Id = disbursementsAndClaimsMasterDto.Id;
            disbursementsAndClaimsMaster.Id = disbursementsAndClaimsMasterDto.Id;
            disbursementsAndClaimsMaster.EmployeeId = disbursementsAndClaimsMasterDto.EmployeeId;
            disbursementsAndClaimsMaster.PettyCashRequestId = disbursementsAndClaimsMasterDto.PettyCashRequestId;
            disbursementsAndClaimsMaster.ExpenseReimburseReqId = disbursementsAndClaimsMasterDto.ExpenseReimburseReqId;
            disbursementsAndClaimsMaster.AdvanceOrReimburseId = disbursementsAndClaimsMasterDto.AdvanceOrReimburseId;
            disbursementsAndClaimsMaster.ProjectId = disbursementsAndClaimsMasterDto.ProjectId;
            disbursementsAndClaimsMaster.SubProjectId = disbursementsAndClaimsMasterDto.SubProjectId;
            disbursementsAndClaimsMaster.WorkTaskId = disbursementsAndClaimsMasterDto.WorkTaskId;
            disbursementsAndClaimsMaster.RecordDate = disbursementsAndClaimsMasterDto.RecordDate;
            disbursementsAndClaimsMaster.Amount = disbursementsAndClaimsMasterDto.Amount;
            disbursementsAndClaimsMaster.CostCentreId = disbursementsAndClaimsMasterDto.CostCentreId;
            disbursementsAndClaimsMaster.ApprovalStatusId = disbursementsAndClaimsMasterDto.ApprovalStatusId;


            _context.DisbursementsAndClaimsMasters.Update(disbursementsAndClaimsMaster);
            //_context.Entry(disbursementsAndClaimsMasterDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DisbursementsAndClaimsMasterExists(id))
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

        // POST: api/DisbursementsAndClaimsMasters
        [HttpPost]
        public async Task<ActionResult<DisbursementsAndClaimsMaster>> PostDisbursementsAndClaimsMaster(DisbursementsAndClaimsMasterDTO disbursementsAndClaimsMasterDto)
        {
            DisbursementsAndClaimsMaster disbursementsAndClaimsMaster = new DisbursementsAndClaimsMaster();

            disbursementsAndClaimsMaster.EmployeeId = disbursementsAndClaimsMasterDto.EmployeeId;
            disbursementsAndClaimsMaster.PettyCashRequestId = disbursementsAndClaimsMasterDto.PettyCashRequestId;
            disbursementsAndClaimsMaster.ExpenseReimburseReqId = disbursementsAndClaimsMasterDto.ExpenseReimburseReqId;
            disbursementsAndClaimsMaster.AdvanceOrReimburseId = disbursementsAndClaimsMasterDto.AdvanceOrReimburseId;
            disbursementsAndClaimsMaster.ProjectId = disbursementsAndClaimsMasterDto.ProjectId;
            disbursementsAndClaimsMaster.SubProjectId = disbursementsAndClaimsMasterDto.SubProjectId;
            disbursementsAndClaimsMaster.WorkTaskId = disbursementsAndClaimsMasterDto.WorkTaskId;
            disbursementsAndClaimsMaster.RecordDate = disbursementsAndClaimsMasterDto.RecordDate;
            disbursementsAndClaimsMaster.Amount = disbursementsAndClaimsMasterDto.Amount;
            disbursementsAndClaimsMaster.CostCentreId = disbursementsAndClaimsMasterDto.CostCentreId;
            disbursementsAndClaimsMaster.ApprovalStatusId = disbursementsAndClaimsMasterDto.ApprovalStatusId;


            _context.DisbursementsAndClaimsMasters.Add(disbursementsAndClaimsMaster);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetDisbursementsAndClaimsMaster", new { id = disbursementsAndClaimsMaster.Id }, disbursementsAndClaimsMaster);
        }

        // DELETE: api/DisbursementsAndClaimsMasters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisbursementsAndClaimsMaster(int id)
        {
            var disbursementsAndClaimsMaster = await _context.DisbursementsAndClaimsMasters.FindAsync(id);
            if (disbursementsAndClaimsMaster == null)
            {
                return NotFound();
            }

            _context.DisbursementsAndClaimsMasters.Remove(disbursementsAndClaimsMaster);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DisbursementsAndClaimsMasterExists(int id)
        {
            return _context.DisbursementsAndClaimsMasters.Any(e => e.Id == id);
        }
    }
}
