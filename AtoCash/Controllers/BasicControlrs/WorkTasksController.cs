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
    [Authorize(Roles = "AtominosAdmin, Admin, Manager, User")]
    public class WorkTasksController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public WorkTasksController(AtoCashDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [ActionName("WorkTasksForDropdown")]
        public async Task<ActionResult<IEnumerable<WorkTaskVM>>> GetWorkTasksForDropDown()
        {
            List<WorkTaskVM> ListWorkTaskVM = new List<WorkTaskVM>();

            var workTasks = await _context.WorkTasks.ToListAsync();
            foreach (WorkTask workTask in workTasks)
            {
                WorkTaskVM workTaskVM = new WorkTaskVM
                {
                    Id = workTask.Id,
                    TaskName = workTask.TaskName
                };

                ListWorkTaskVM.Add(workTaskVM);
            }

            return ListWorkTaskVM;

        }


        [HttpGet("{id}")]
        [ActionName("GetTasksForSubProjects")]
        public async Task<ActionResult<IEnumerable<SubProjectVM>>> GetTasksForSubProjects(int id)
        {
           
            var listOfTasks = _context.WorkTasks.Where(t => t.SubProjectId == id).ToList();

            List<WorkTaskVM> ListWorkTaskVM = new List<WorkTaskVM>();

            if (listOfTasks != null)
            {
                foreach (var item in listOfTasks)
                {
                    WorkTaskVM workTaskVM = new WorkTaskVM()
                    {
                        Id = item.Id,
                        TaskName = item.TaskName
                    };
                    ListWorkTaskVM.Add(workTaskVM);

                }
                return Ok(ListWorkTaskVM);
            }
            return Ok(new RespStatus { Status = "Success", Message = "No WorkTask Assigned to Employee" });

        }

        [HttpGet]
        [ActionName("WorkTasksBySubProjectForDropdown")]
        public async Task<ActionResult<IEnumerable<WorkTaskVM>>> GetWorkTasksBySubProjectForDropdown(int Id)
        {
            List<WorkTaskVM> ListWorkTaskVM = new List<WorkTaskVM>();

            var workTasks = await _context.WorkTasks.Where(w => w.SubProjectId == Id).ToListAsync();
            foreach (WorkTask workTask in workTasks)
            {
                WorkTaskVM workTaskVM = new WorkTaskVM
                {
                    Id = workTask.Id,
                    TaskName = workTask.TaskName
                };

                ListWorkTaskVM.Add(workTaskVM);
            }

            return ListWorkTaskVM;


        }
        // GET: api/WorkTasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkTaskDTO>>> GetWorkTasks()
        {
            List<WorkTaskDTO> ListWorkTaskDto = new List<WorkTaskDTO>();

            var WorkTasks = await _context.WorkTasks.ToListAsync();

            foreach (WorkTask worktask in WorkTasks)
            {
                WorkTaskDTO workTaskDto = new WorkTaskDTO
                {
                    Id = worktask.Id,
                    SubProjectId = worktask.SubProjectId,
                    TaskName = worktask.TaskName,
                    TaskDesc = worktask.TaskDesc
                };

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
        [Authorize(Roles = "AtominosAdmin, Admin")]
        public async Task<IActionResult> PutWorkTask(int id, WorkTaskDTO workTaskDto)
        {
            if (id != workTaskDto.Id)
            {
                return BadRequest(new RespStatus { Status = "Failure", Message = "Id is invalid" });
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
        [Authorize(Roles = "AtominosAdmin, Admin")]
        public async Task<ActionResult<WorkTask>> PostWorkTask(WorkTaskDTO workTaskDto)
        {
            WorkTask workTask = new WorkTask
            {
                SubProjectId = workTaskDto.SubProjectId,
                TaskName = workTaskDto.TaskName,
                TaskDesc = workTaskDto.TaskDesc
            };

            _context.WorkTasks.Add(workTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkTask", new { id = workTask.Id }, workTask);


        }

        // DELETE: api/WorkTasks/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "AtominosAdmin, Admin")]
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
