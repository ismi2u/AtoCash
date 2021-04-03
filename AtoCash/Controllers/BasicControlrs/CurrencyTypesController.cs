using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtoCash.Data;
using AtoCash.Models;
using AtoCash.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace AtoCash.Controllers.BasicControlrs
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Finmgr, Admin, User")]
    public class CurrencyTypesController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public CurrencyTypesController(AtoCashDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ActionName("CurrencyTypesForDropdown")]
        public async Task<ActionResult<IEnumerable<CurrencyTypeVM>>> GetCurrencyTypesForDropdown()
        {
            List<CurrencyTypeVM> ListCurrencyTypeVM = new List<CurrencyTypeVM>();

            var currencyTypes = await _context.CurrencyTypes.Where(c => c.StatusTypeId == (int)StatusType.Active).ToListAsync();
            foreach (CurrencyType currencyType in currencyTypes)
            {
                CurrencyTypeVM currencyTypeVM = new CurrencyTypeVM
                {
                    Id = currencyType.Id,
                    CurrencyCode = currencyType.CurrencyCode,
                };

                ListCurrencyTypeVM.Add(currencyTypeVM);
            }

            return ListCurrencyTypeVM;

        }


        // GET: api/CurrencyTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyTypeDTO>>> GetCurrencyTypes()
        {

            List<CurrencyTypeDTO> ListCurrencyTypeDTO = new List<CurrencyTypeDTO>();

            var currencyTypes = await _context.CurrencyTypes.ToListAsync();

            foreach (CurrencyType currencyType in currencyTypes)
            {
                CurrencyTypeDTO currencyTypeDTO = new CurrencyTypeDTO
                {
                    Id = currencyType.Id,
                    CurrencyCode = currencyType.CurrencyCode,
                    CurrencyName = currencyType.CurrencyName,
                    Country = currencyType.Country,
                    StatusTypeId = currencyType.StatusTypeId
                };

                ListCurrencyTypeDTO.Add(currencyTypeDTO);

            }
            return Ok(ListCurrencyTypeDTO);
        }

        // GET: api/CurrencyTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CurrencyTypeDTO>> GetCurrencyType(int id)
        {
            var currencyType = await _context.CurrencyTypes.FindAsync(id);

            if (currencyType == null)
            {
                return NotFound();
            }

            CurrencyTypeDTO currencyTypeDTO = new CurrencyTypeDTO
            {
                Id = currencyType.Id,
                CurrencyCode = currencyType.CurrencyCode,
                CurrencyName = currencyType.CurrencyName,
                Country = currencyType.Country,
                StatusTypeId = currencyType.StatusTypeId
            };


            return currencyTypeDTO;
        }

        // PUT: api/CurrencyTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<IActionResult> PutCurrencyType(int id, CurrencyTypeDTO currencyTypeDTO)
        {
            if (id != currencyTypeDTO.Id)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Id is invalid" });
            }

            var currencyType = await _context.CurrencyTypes.FindAsync(id);
            currencyType.CurrencyCode = currencyTypeDTO.CurrencyCode;
            currencyType.CurrencyName = currencyTypeDTO.CurrencyName;
            currencyType.Country = currencyTypeDTO.Country;
            currencyType.StatusTypeId = currencyTypeDTO.StatusTypeId;

            _context.CurrencyTypes.Update(currencyType);

            //_context.Entry(currencyType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurrencyTypeExists(id))
                {
                    return Conflict(new RespStatus { Status = "Failure", Message = "CurrencyType is invalid" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new RespStatus { Status = "Success", Message = "CurrencyType Details Updated!" });
        }

        // POST: api/CurrencyTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<ActionResult<CurrencyType>> PostCurrencyType(CurrencyType currencyType)
        {

            var curncyType = _context.CurrencyTypes.Where(c => c.CurrencyCode == currencyType.CurrencyCode).FirstOrDefault();
            if (curncyType != null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Currency Already Exists" });
            }

            CurrencyType currencyTyp = new();

            currencyTyp.CurrencyCode = currencyType.CurrencyCode;
            currencyTyp.CurrencyName = currencyType.CurrencyName;
            currencyTyp.Country = currencyType.Country;
            currencyTyp.StatusTypeId = currencyType.StatusTypeId;

            _context.CurrencyTypes.Add(currencyTyp);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCurrencyType", new { id = currencyType.Id }, currencyType);
        }

        // DELETE: api/CurrencyTypes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<IActionResult> DeleteCurrencyType(int id)
        {
            var emp = _context.Employees.Where(e => e.CurrencyTypeId == id).FirstOrDefault();
            var pettyReq = _context.PettyCashRequests.Where(p => p.CurrencyTypeId == id).FirstOrDefault();


          
            if (emp != null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Currency in use for Employee" });
            }
            if (pettyReq != null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Currency used in Cash Advance" });
            }

            var currencyType = await _context.CurrencyTypes.FindAsync(id);
            _context.CurrencyTypes.Remove(currencyType);
            await _context.SaveChangesAsync();

            return Ok(new RespStatus { Status = "Success", Message = "Currency Deleted!" });
        }

        private bool CurrencyTypeExists(int id)
        {
            return _context.CurrencyTypes.Any(e => e.Id == id);
        }

        private enum StatusType
        {
            Active = 1,
            Inactive

        }
        //
    }
}
