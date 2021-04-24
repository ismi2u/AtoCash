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
    [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr, User")]
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
                DisbursementsAndClaimsMasterDTO disbursementsAndClaimsMasterDTO = new DisbursementsAndClaimsMasterDTO
                {
                    Id = disbursementsAndClaimsMaster.Id,
                    EmployeeId = disbursementsAndClaimsMaster.EmployeeId,
                    PettyCashRequestId = disbursementsAndClaimsMaster.PettyCashRequestId,
                    ExpenseReimburseReqId = disbursementsAndClaimsMaster.ExpenseReimburseReqId,
                    RequestTypeId = disbursementsAndClaimsMaster.RequestTypeId,
                    ProjectId = disbursementsAndClaimsMaster.ProjectId,
                    SubProjectId = disbursementsAndClaimsMaster.SubProjectId,
                    WorkTaskId = disbursementsAndClaimsMaster.WorkTaskId,
                    RecordDate = disbursementsAndClaimsMaster.RecordDate,
                    ClaimAmount = disbursementsAndClaimsMaster.ClaimAmount,
                    CostCenterId = disbursementsAndClaimsMaster.CostCenterId,
                    ApprovalStatusId = disbursementsAndClaimsMaster.ApprovalStatusId
                };

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
                return Conflict(new RespStatus { Status = "Failure", Message = "Disburese Id invalid!" });
            }

            disbursementsAndClaimsMasterDTO.Id = disbursementsAndClaimsMaster.Id;
            disbursementsAndClaimsMasterDTO.EmployeeId = disbursementsAndClaimsMaster.EmployeeId;
            disbursementsAndClaimsMasterDTO.PettyCashRequestId = disbursementsAndClaimsMaster.PettyCashRequestId;
            disbursementsAndClaimsMasterDTO.ExpenseReimburseReqId = disbursementsAndClaimsMaster.ExpenseReimburseReqId;
            disbursementsAndClaimsMasterDTO.RequestTypeId = disbursementsAndClaimsMaster.RequestTypeId;
            disbursementsAndClaimsMasterDTO.ProjectId = disbursementsAndClaimsMaster.ProjectId;
            disbursementsAndClaimsMasterDTO.SubProjectId = disbursementsAndClaimsMaster.SubProjectId;
            disbursementsAndClaimsMasterDTO.WorkTaskId = disbursementsAndClaimsMaster.WorkTaskId;
            disbursementsAndClaimsMasterDTO.RecordDate = disbursementsAndClaimsMaster.RecordDate;
            disbursementsAndClaimsMasterDTO.ClaimAmount = disbursementsAndClaimsMaster.ClaimAmount;
            disbursementsAndClaimsMasterDTO.CostCenterId = disbursementsAndClaimsMaster.CostCenterId;
            disbursementsAndClaimsMasterDTO.ApprovalStatusId = disbursementsAndClaimsMaster.ApprovalStatusId;

            return disbursementsAndClaimsMasterDTO;
        }

        // PUT: api/DisbursementsAndClaimsMasters/5
        [HttpPut("{id}")]
        [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr")]
        public async Task<IActionResult> PutDisbursementsAndClaimsMaster(int id, DisbursementsAndClaimsMasterDTO disbursementsAndClaimsMasterDto)
        {
            if (id != disbursementsAndClaimsMasterDto.Id)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Id state is invalid" });
            }

            var disbursementsAndClaimsMaster = await _context.DisbursementsAndClaimsMasters.FindAsync(id);

            disbursementsAndClaimsMaster.Id = disbursementsAndClaimsMasterDto.Id;
            disbursementsAndClaimsMaster.Id = disbursementsAndClaimsMasterDto.Id;
            disbursementsAndClaimsMaster.EmployeeId = disbursementsAndClaimsMasterDto.EmployeeId;
            disbursementsAndClaimsMaster.PettyCashRequestId = disbursementsAndClaimsMasterDto.PettyCashRequestId;
            disbursementsAndClaimsMaster.ExpenseReimburseReqId = disbursementsAndClaimsMasterDto.ExpenseReimburseReqId;
            disbursementsAndClaimsMaster.RequestTypeId = disbursementsAndClaimsMasterDto.RequestTypeId;
            disbursementsAndClaimsMaster.ProjectId = disbursementsAndClaimsMasterDto.ProjectId;
            disbursementsAndClaimsMaster.SubProjectId = disbursementsAndClaimsMasterDto.SubProjectId;
            disbursementsAndClaimsMaster.WorkTaskId = disbursementsAndClaimsMasterDto.WorkTaskId;
            disbursementsAndClaimsMaster.RecordDate = disbursementsAndClaimsMasterDto.RecordDate;
            disbursementsAndClaimsMaster.ClaimAmount = disbursementsAndClaimsMasterDto.ClaimAmount;
            disbursementsAndClaimsMaster.CostCenterId = disbursementsAndClaimsMasterDto.CostCenterId;
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
                    return Conflict(new RespStatus { Status = "Failure", Message = "Disburse Id invalid!" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new RespStatus { Status = "Success", Message = "Disburse Details Updated!" });
        }

        // POST: api/DisbursementsAndClaimsMasters
        [HttpPost]
        [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr")]
        public async Task<ActionResult<DisbursementsAndClaimsMaster>> PostDisbursementsAndClaimsMaster(DisbursementsAndClaimsMasterDTO disbursementsAndClaimsMasterDto)
        {
            DisbursementsAndClaimsMaster disbursementsAndClaimsMaster = new DisbursementsAndClaimsMaster
            {
                EmployeeId = disbursementsAndClaimsMasterDto.EmployeeId,
                PettyCashRequestId = disbursementsAndClaimsMasterDto.PettyCashRequestId,
                ExpenseReimburseReqId = disbursementsAndClaimsMasterDto.ExpenseReimburseReqId,
                RequestTypeId = disbursementsAndClaimsMasterDto.RequestTypeId,
                ProjectId = disbursementsAndClaimsMasterDto.ProjectId,
                SubProjectId = disbursementsAndClaimsMasterDto.SubProjectId,
                WorkTaskId = disbursementsAndClaimsMasterDto.WorkTaskId,
                RecordDate = disbursementsAndClaimsMasterDto.RecordDate,
                ClaimAmount = disbursementsAndClaimsMasterDto.ClaimAmount,
                CostCenterId = disbursementsAndClaimsMasterDto.CostCenterId,
                ApprovalStatusId = disbursementsAndClaimsMasterDto.ApprovalStatusId
            };


            _context.DisbursementsAndClaimsMasters.Add(disbursementsAndClaimsMaster);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetDisbursementsAndClaimsMaster", new { id = disbursementsAndClaimsMaster.Id }, disbursementsAndClaimsMaster);
        }

        // DELETE: api/DisbursementsAndClaimsMasters/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr")]
        public async Task<IActionResult> DeleteDisbursementsAndClaimsMaster(int id)
        {
            var disbursementsAndClaimsMaster = await _context.DisbursementsAndClaimsMasters.FindAsync(id);
            if (disbursementsAndClaimsMaster == null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Disburse Id invalid!" });
            }

            _context.DisbursementsAndClaimsMasters.Remove(disbursementsAndClaimsMaster);
            await _context.SaveChangesAsync();

            return Ok(new RespStatus { Status = "Success", Message = "Disburse Details Updated!" });
        }

        private bool DisbursementsAndClaimsMasterExists(int id)
        {
            return _context.DisbursementsAndClaimsMasters.Any(e => e.Id == id);
        }
    }
}
