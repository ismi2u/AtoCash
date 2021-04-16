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
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr, User")]
    public class EmpCurrentPettyCashBalancesController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public EmpCurrentPettyCashBalancesController(AtoCashDbContext context)
        {
            _context = context;
        }

        // GET: api/EmpCurrentPettyCashBalances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpCurrentPettyCashBalanceDTO>>> GetEmpCurrentPettyCashBalances()
        {
            List<EmpCurrentPettyCashBalanceDTO> ListEmpCurrentPettyCashBalanceDTO = new List<EmpCurrentPettyCashBalanceDTO>();

            var empCurrentPettyCashBalances = await _context.EmpCurrentPettyCashBalances.ToListAsync();

            foreach (EmpCurrentPettyCashBalance empCurrentPettyCashBalance in empCurrentPettyCashBalances)
            {
                EmpCurrentPettyCashBalanceDTO empCurrentPettyCashBalanceDTO = new EmpCurrentPettyCashBalanceDTO
                {
                    Id = empCurrentPettyCashBalance.Id,
                    EmployeeId = empCurrentPettyCashBalance.EmployeeId,
                    CurBalance = empCurrentPettyCashBalance.CurBalance,
                    UpdatedOn = empCurrentPettyCashBalance.UpdatedOn
                };
                ListEmpCurrentPettyCashBalanceDTO.Add(empCurrentPettyCashBalanceDTO);

            }

            return ListEmpCurrentPettyCashBalanceDTO;
        }

        // GET: api/EmpCurrentPettyCashBalances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpCurrentPettyCashBalanceDTO>> GetEmpCurrentPettyCashBalance(int id)
        {
            EmpCurrentPettyCashBalanceDTO empCurrentPettyCashBalanceDTO = new EmpCurrentPettyCashBalanceDTO();

            var empCurrentPettyCashBalance = await _context.EmpCurrentPettyCashBalances.FindAsync(id);

            if (empCurrentPettyCashBalance == null)
            {
                return NoContent();
            }

            empCurrentPettyCashBalanceDTO.Id = empCurrentPettyCashBalance.Id;
            empCurrentPettyCashBalanceDTO.EmployeeId = empCurrentPettyCashBalance.EmployeeId;
            empCurrentPettyCashBalanceDTO.CurBalance = empCurrentPettyCashBalance.CurBalance;
            empCurrentPettyCashBalanceDTO.UpdatedOn = empCurrentPettyCashBalance.UpdatedOn;


            return empCurrentPettyCashBalanceDTO;
        }

        // PUT: api/EmpCurrentPettyCashBalances/5
        [HttpPut("{id}")]
        [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr")]
        public async Task<IActionResult> PutEmpCurrentPettyCashBalance(int id, EmpCurrentPettyCashBalanceDTO empCurrentPettyCashBalanceDto)
        {
            if (id != empCurrentPettyCashBalanceDto.Id)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Id is invalid" });
            }

            var empCurrentPettyCashBalance = await _context.EmpCurrentPettyCashBalances.FindAsync(id);

            empCurrentPettyCashBalance.Id = empCurrentPettyCashBalanceDto.Id;
            empCurrentPettyCashBalance.EmployeeId = empCurrentPettyCashBalanceDto.EmployeeId;
            empCurrentPettyCashBalance.CurBalance = empCurrentPettyCashBalanceDto.CurBalance;
            empCurrentPettyCashBalance.UpdatedOn = empCurrentPettyCashBalanceDto.UpdatedOn;

            _context.EmpCurrentPettyCashBalances.Update(empCurrentPettyCashBalance);
            //_context.Entry(projectDto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpCurrentPettyCashBalanceExists(id))
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

        // POST: api/EmpCurrentPettyCashBalances
        [HttpPost]
        [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr")]
        public async Task<ActionResult<EmpCurrentPettyCashBalance>> PostEmpCurrentPettyCashBalance(EmpCurrentPettyCashBalanceDTO empCurrentPettyCashBalanceDto)
        {
            EmpCurrentPettyCashBalance empCurrentPettyCashBalance = new EmpCurrentPettyCashBalance
            {
                Id = empCurrentPettyCashBalanceDto.Id,
                EmployeeId = empCurrentPettyCashBalanceDto.EmployeeId,
                CurBalance = empCurrentPettyCashBalanceDto.CurBalance,
                UpdatedOn = empCurrentPettyCashBalanceDto.UpdatedOn
            };

            _context.EmpCurrentPettyCashBalances.Add(empCurrentPettyCashBalance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmpCurrentPettyCashBalance", new { id = empCurrentPettyCashBalance.Id }, empCurrentPettyCashBalance);
        }

        // GET: api/EmpCurrentPettyCashBalances/GetEmpCashBalanceVsAdvanced
        [HttpGet("{id}")]
        [ActionName("GetEmpCashBalanceVsAdvanced")]
        public ActionResult<CashbalVsAdvancedVM> GetEmpCashBalanceVsAdvanced(int id)
        {
            CashbalVsAdvancedVM cashbalVsAdvancedVM = new CashbalVsAdvancedVM()
            {
                CurCashBal = _context.EmpCurrentPettyCashBalances.Find(id).CurBalance,
                MaxCashAllowed = _context.JobRoles.Find(_context.Employees.Find(id).RoleId).MaxPettyCashAllowed

            };
            return Ok(cashbalVsAdvancedVM);
        }

        

        // DELETE: api/EmpCurrentPettyCashBalances/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr")]
        public async Task<IActionResult> DeleteEmpCurrentPettyCashBalance(int id)
        {
            var empCurrentPettyCashBalance = await _context.EmpCurrentPettyCashBalances.FindAsync(id);
            if (empCurrentPettyCashBalance == null)
            {
                return NoContent();
            }

            _context.EmpCurrentPettyCashBalances.Remove(empCurrentPettyCashBalance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmpCurrentPettyCashBalanceExists(int id)
        {
            return _context.EmpCurrentPettyCashBalances.Any(e => e.Id == id);
        }
    }
}
