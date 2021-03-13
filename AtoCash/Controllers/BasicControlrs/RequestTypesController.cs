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
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Admin")]
    public class RequestsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public RequestsController(AtoCashDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [ActionName("RequestTypesForDropdown")]
        public async Task<ActionResult<IEnumerable<RequestTypeVM>>> GetRequestTypesForDropDown()
        {
            List<RequestTypeVM> ListRequestTypeVM = new List<RequestTypeVM>();

            var requestTypes = await _context.RequestTypes.ToListAsync();
            foreach (RequestType requestType in requestTypes)
            {
                RequestTypeVM requestTypeVM = new RequestTypeVM
                {
                    Id = requestType.Id,
                     RequestName = requestType.RequestName
                };

                ListRequestTypeVM.Add(requestTypeVM);
            }

            return ListRequestTypeVM;

        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestType>>> GetRequestTypes()
        {
            return await _context.RequestTypes.ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestType>> GetRequestType(int id)
        {
            var requestType = await _context.RequestTypes.FindAsync(id);

            if (requestType == null)
            {
                return NotFound();
            }

            return requestType;
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequestType(int id, RequestType requestType)
        {
            if (id != requestType.Id)
            {
                return BadRequest();
            }

            _context.Entry(requestType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestTypeExists(id))
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

        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RequestType>> PostRequestType(RequestType requestType)
        {
            _context.RequestTypes.Add(requestType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequestType", new { id = requestType.Id }, requestType);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequestType(int id)
        {
            var requestType = await _context.RequestTypes.FindAsync(id);
            if (requestType == null)
            {
                return NotFound();
            }

            _context.RequestTypes.Remove(requestType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestTypeExists(int id)
        {
            return _context.RequestTypes.Any(e => e.Id == id);
        }
    }
}