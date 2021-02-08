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
    public class WorkTasksController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public WorkTasksController(AtoCashDbContext context)
        {
            _context = context;
        }

        // GET: api/WorkTasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkTaskDTO>>> GetWorkTasks()
        {
            List<WorkTaskDTO> ListWorkTaskDto = new List<WorkTaskDTO>();

            var WorkTasks = await _context.WorkTasks.ToListAsync();

            foreach (WorkTask worktask in WorkTasks)
            {
                WorkTaskDTO workTaskDto = new WorkTaskDTO();
                workTaskDto.Id = worktask.Id;
                workTaskDto.SubProjectId = worktask.SubProjectId;
                workTaskDto.TaskName = worktask.TaskName;
                workTaskDto.TaskDesc = worktask.TaskDesc;

                ListWorkTaskDto.Add(workTaskDto);

            }

            return ListWorkTaskDto;
        }

        // GET: api/WorkTasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkTaskDTO>> GetWorkTask(int id)
        {
            WorkTaskDTO workTaskDto = new WorkTaskDTO();

            var worktask = await _context.WorkTasks.FindAsync(id);

            if (worktask == null)
            {
                return NotFound();
            }

            workTaskDto.Id = worktask.Id;
            workTaskDto.SubProjectId = worktask.SubProjectId;
            workTaskDto.TaskName = worktask.TaskName;
            workTaskDto.TaskDesc = worktask.TaskDesc;

            return workTaskDto;
        }

        // PUT: api/WorkTasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkTask(int id, WorkTaskDTO workTaskDto)
        {
            if (id != workTaskDto.Id)
            {
                return BadRequest();
            }

            var workTask = await _context.WorkTasks.FindAsync(id);

            workTask.Id = workTaskDto.Id;
            workTask.SubProjectId = workTaskDto.SubProjectId;
            workTask.TaskName = workTaskDto.TaskName;
            workTask.TaskDesc = workTaskDto.TaskDesc;

            _context.WorkTasks.Update(workTask);
            //_context.Entry(workTaskDto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkTaskExists(id))
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

        // POST: api/WorkTasks
        [HttpPost]
        public async Task<ActionResult<WorkTask>> PostWorkTask(WorkTaskDTO workTaskDto)
        {
            WorkTask workTask = new WorkTask();

            workTask.SubProjectId = workTaskDto.SubProjectId;
            workTask.TaskName = workTaskDto.TaskName;
            workTask.TaskDesc = workTaskDto.TaskDesc;

            _context.WorkTasks.Add(workTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkTask", new { id = workTask.Id }, workTask);

            
        }

        // DELETE: api/WorkTasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkTask(int id)
        {
            var workTask = await _context.WorkTasks.FindAsync(id);
            if (workTask == null)
            {
                return NotFound();
            }

            _context.WorkTasks.Remove(workTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkTaskExists(int id)
        {
            return _context.WorkTasks.Any(e => e.Id == id);
        }
    }
}
