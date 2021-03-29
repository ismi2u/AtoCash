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
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Finmgr, Admin, User")]
    public class CostCentresController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public CostCentresController(AtoCashDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ActionName("CostCentresForDropdown")]
        public async Task<ActionResult<IEnumerable<CostCentreVM>>> GetCostCentresForDropDown()
        {
            List<CostCentreVM> ListCostCentreVM = new List<CostCentreVM>();

            var costCentres = await _context.CostCentres.ToListAsync();
            foreach (CostCentre costCentre in costCentres)
            {
                CostCentreVM costCentreVM = new CostCentreVM
                {
                    Id = costCentre.Id,
                    CostCentreCode = costCentre.CostCentreCode
                };

                ListCostCentreVM.Add(costCentreVM);
            }

            return ListCostCentreVM;

        }

        // GET: api/CostCentres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CostCentre>>> GetCostCentres()
        {
            return await _context.CostCentres.ToListAsync();
        }

        // GET: api/CostCentres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CostCentre>> GetCostCentre(int id)
        {
            var costCentre = await _context.CostCentres.FindAsync(id);

            if (costCentre == null)
            {
                return NoContent();
            }

            return costCentre;
        }

        // PUT: api/CostCentres/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<IActionResult> PutCostCentre(int id, CostCentreDTO costCentre)
        {
            if (id != costCentre.Id)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Id is invalid" });
            }

            var ccentre = await _context.CostCentres.FindAsync(id);
            ccentre.CostCentreDesc = costCentre.CostCentreDesc;
            _context.CostCentres.Update(ccentre);

            //_context.Entry(costCentre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CostCentreExists(id))
                {
                    return Conflict(new RespStatus { Status = "Failure", Message = "Costcentre is invalid" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new RespStatus { Status = "Success", Message = "CostCentre Details Updated!" });
        }

        // POST: api/CostCentres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<ActionResult<CostCentre>> PostCostCentre(CostCentre costCentre)
        {
            var ccentre = _context.CostCentres.Where(c => c.CostCentreCode == costCentre.CostCentreCode).FirstOrDefault();
            if (ccentre != null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "CostCentre Already Exists" });
            }
            _context.CostCentres.Add(costCentre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCostCentre", new { id = costCentre.Id }, costCentre);
        }

        // DELETE: api/CostCentres/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<IActionResult> DeleteCostCentre(int id)
        {
            var dept = _context.Departments.Where(d => d.CostCentreId == id).FirstOrDefault();
            var proj = _context.Projects.Where(p => p.CostCentreId == id).FirstOrDefault();

            if (dept != null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Cost-Centre in use for Department" });
            }
            if (dept != null || proj != null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Cost-Centre in use for Project" });
            }

            var costCentre = await _context.CostCentres.FindAsync(id);
            if (costCentre == null)
            {
                return NoContent();
            }

            _context.CostCentres.Remove(costCentre);
            await _context.SaveChangesAsync();

            return Ok(new RespStatus { Status = "Success", Message = "Cost-Centre Deleted!" });
        }

        private bool CostCentreExists(int id)
        {
            return _context.CostCentres.Any(e => e.Id == id);
        }
    }
}
