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
using EmailService;
using AtoCash.Authentication;

namespace AtoCash.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Finmgr, Admin, User, Manager")]
    public class TravelApprovalStatusTrackersController : ControllerBase
    {
        private readonly AtoCashDbContext _context;
        private readonly IEmailSender _emailSender;


        public TravelApprovalStatusTrackersController(AtoCashDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }




        // GET: api/TravelApprovalStatusTrackers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelApprovalStatusTrackerDTO>>> GetTravelApprovalStatusTrackers()
        {
            List<TravelApprovalStatusTrackerDTO> ListTravelApprovalStatusTrackerDTO = new List<TravelApprovalStatusTrackerDTO>();

            var TravelApprovalStatusTrackers = await _context.TravelApprovalStatusTrackers.ToListAsync();

            foreach (TravelApprovalStatusTracker travelApprovalStatusTracker in TravelApprovalStatusTrackers)
            {
                TravelApprovalStatusTrackerDTO TravelApprovalStatusTrackerDTO = new TravelApprovalStatusTrackerDTO();

                TravelApprovalStatusTrackerDTO.Id = travelApprovalStatusTracker.Id;
                TravelApprovalStatusTrackerDTO.EmployeeId = travelApprovalStatusTracker.EmployeeId;
                TravelApprovalStatusTrackerDTO.EmployeeName = _context.Employees.Find(travelApprovalStatusTracker.EmployeeId).GetFullName();
                TravelApprovalStatusTrackerDTO.TravelApprovalRequestId = travelApprovalStatusTracker.TravelApprovalRequestId;
                TravelApprovalStatusTrackerDTO.DepartmentId = travelApprovalStatusTracker.DepartmentId;
                TravelApprovalStatusTrackerDTO.DepartmentName = travelApprovalStatusTracker.DepartmentId != null ? _context.Departments.Find(travelApprovalStatusTracker.DepartmentId).DeptName : null;
                TravelApprovalStatusTrackerDTO.ProjectId = travelApprovalStatusTracker.ProjectId;
                TravelApprovalStatusTrackerDTO.ProjectName = travelApprovalStatusTracker.ProjectId != null ? _context.Projects.Find(travelApprovalStatusTracker.ProjectId).ProjectName : null;
                TravelApprovalStatusTrackerDTO.RoleId = travelApprovalStatusTracker.RoleId;
                TravelApprovalStatusTrackerDTO.JobRole = _context.JobRoles.Find(travelApprovalStatusTracker.RoleId).RoleName;
                TravelApprovalStatusTrackerDTO.ApprovalLevelId = travelApprovalStatusTracker.ApprovalLevelId;
                TravelApprovalStatusTrackerDTO.ReqDate = travelApprovalStatusTracker.ReqDate;
                TravelApprovalStatusTrackerDTO.FinalApprovedDate = travelApprovalStatusTracker.FinalApprovedDate;
                TravelApprovalStatusTrackerDTO.ApprovalStatusTypeId = travelApprovalStatusTracker.ApprovalStatusTypeId;
                TravelApprovalStatusTrackerDTO.ApprovalStatusType = _context.ApprovalStatusTypes.Find(travelApprovalStatusTracker.ApprovalStatusTypeId).Status;


                ListTravelApprovalStatusTrackerDTO.Add(TravelApprovalStatusTrackerDTO);

            }

            return ListTravelApprovalStatusTrackerDTO;
        }

        // GET: api/TravelApprovalStatusTrackers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TravelApprovalStatusTracker>> GetTravelApprovalStatusTracker(int id)
        {
            var travelApprovalStatusTracker = await _context.TravelApprovalStatusTrackers.FindAsync(id);

            if (travelApprovalStatusTracker == null)
            {
                return NotFound();
            }

            TravelApprovalStatusTrackerDTO TravelApprovalStatusTrackerDTO = new TravelApprovalStatusTrackerDTO();

            TravelApprovalStatusTrackerDTO.Id = travelApprovalStatusTracker.Id;
            TravelApprovalStatusTrackerDTO.EmployeeId = travelApprovalStatusTracker.EmployeeId;
            TravelApprovalStatusTrackerDTO.EmployeeName = _context.Employees.Find(travelApprovalStatusTracker.EmployeeId).GetFullName();
            TravelApprovalStatusTrackerDTO.TravelApprovalRequestId = travelApprovalStatusTracker.TravelApprovalRequestId;
            TravelApprovalStatusTrackerDTO.DepartmentId = travelApprovalStatusTracker.DepartmentId;
            TravelApprovalStatusTrackerDTO.DepartmentName = travelApprovalStatusTracker.DepartmentId != null ? _context.Departments.Find(travelApprovalStatusTracker.DepartmentId).DeptName : null;
            TravelApprovalStatusTrackerDTO.ProjectId = travelApprovalStatusTracker.ProjectId;
            TravelApprovalStatusTrackerDTO.ProjectName = travelApprovalStatusTracker.ProjectId != null ? _context.Projects.Find(travelApprovalStatusTracker.ProjectId).ProjectName : null;
            TravelApprovalStatusTrackerDTO.RoleId = travelApprovalStatusTracker.RoleId;
            TravelApprovalStatusTrackerDTO.JobRole = _context.JobRoles.Find(travelApprovalStatusTracker.RoleId).RoleName;
            TravelApprovalStatusTrackerDTO.ApprovalLevelId = travelApprovalStatusTracker.ApprovalLevelId;
            TravelApprovalStatusTrackerDTO.ReqDate = travelApprovalStatusTracker.ReqDate;
            TravelApprovalStatusTrackerDTO.FinalApprovedDate = travelApprovalStatusTracker.FinalApprovedDate;
            TravelApprovalStatusTrackerDTO.ApprovalStatusTypeId = travelApprovalStatusTracker.ApprovalStatusTypeId;
            TravelApprovalStatusTrackerDTO.ApprovalStatusType = _context.ApprovalStatusTypes.Find(travelApprovalStatusTracker.ApprovalStatusTypeId).Status;


            return travelApprovalStatusTracker;
        }

        // PUT: api/TravelApprovalStatusTrackers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTravelApprovalStatusTracker(int id, TravelApprovalStatusTracker travelApprovalStatusTracker)
        {
            if (id != travelApprovalStatusTracker.Id)
            {
                return BadRequest();
            }

            _context.Entry(travelApprovalStatusTracker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TravelApprovalStatusTrackerExists(id))
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

        // POST: api/TravelApprovalStatusTrackers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TravelApprovalStatusTracker>> PostTravelApprovalStatusTracker(TravelApprovalStatusTracker travelApprovalStatusTracker)
        {
            _context.TravelApprovalStatusTrackers.Add(travelApprovalStatusTracker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTravelApprovalStatusTracker", new { id = travelApprovalStatusTracker.Id }, travelApprovalStatusTracker);
        }

        // DELETE: api/TravelApprovalStatusTrackers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTravelApprovalStatusTracker(int id)
        {
            var travelApprovalStatusTracker = await _context.TravelApprovalStatusTrackers.FindAsync(id);
            if (travelApprovalStatusTracker == null)
            {
                return NotFound();
            }

            _context.TravelApprovalStatusTrackers.Remove(travelApprovalStatusTracker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TravelApprovalStatusTrackerExists(int id)
        {
            return _context.TravelApprovalStatusTrackers.Any(e => e.Id == id);
        }

        [HttpGet("{id}")]
        [ActionName("ApprovalsPendingForApprover")]
        public ActionResult<IEnumerable<TravelApprovalStatusTrackerDTO>> GetPendingApprovalRequestForApprover(int id)
        {

            if (id == 0)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Employee Id is Invalid" });
            }


            //get the RoleID of the Employee (Approver)
            int roleid = _context.Employees.Find(id).RoleId;

            if (roleid == 0)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Role Id is Invalid" });
            }

            var TravelApprovalStatusTrackers = _context.TravelApprovalStatusTrackers.Where(r => r.RoleId == roleid && r.ApprovalStatusTypeId == (int)ApprovalStatus.Pending);
            List<TravelApprovalStatusTrackerDTO> ListTravelApprovalStatusTrackerDTO = new List<TravelApprovalStatusTrackerDTO>();

            foreach (TravelApprovalStatusTracker travelApprovalStatusTracker in TravelApprovalStatusTrackers)
            {
                TravelApprovalStatusTrackerDTO TravelApprovalStatusTrackerDTO = new TravelApprovalStatusTrackerDTO();

                TravelApprovalStatusTrackerDTO.Id = travelApprovalStatusTracker.Id;
                TravelApprovalStatusTrackerDTO.EmployeeId = travelApprovalStatusTracker.EmployeeId;
                TravelApprovalStatusTrackerDTO.EmployeeName = _context.Employees.Find(travelApprovalStatusTracker.EmployeeId).GetFullName();
                TravelApprovalStatusTrackerDTO.TravelApprovalRequestId = travelApprovalStatusTracker.TravelApprovalRequestId;
                TravelApprovalStatusTrackerDTO.DepartmentId = travelApprovalStatusTracker.DepartmentId;
                TravelApprovalStatusTrackerDTO.DepartmentName = travelApprovalStatusTracker.DepartmentId != null ? _context.Departments.Find(travelApprovalStatusTracker.DepartmentId).DeptName : null;
                TravelApprovalStatusTrackerDTO.ProjectId = travelApprovalStatusTracker.ProjectId;
                TravelApprovalStatusTrackerDTO.ProjectName = travelApprovalStatusTracker.ProjectId != null ? _context.Projects.Find(travelApprovalStatusTracker.ProjectId).ProjectName : null;
                TravelApprovalStatusTrackerDTO.RoleId = travelApprovalStatusTracker.RoleId;
                TravelApprovalStatusTrackerDTO.JobRole = _context.JobRoles.Find(travelApprovalStatusTracker.RoleId).RoleName;
                TravelApprovalStatusTrackerDTO.ApprovalLevelId = travelApprovalStatusTracker.ApprovalLevelId;
                TravelApprovalStatusTrackerDTO.ReqDate = travelApprovalStatusTracker.ReqDate;
                TravelApprovalStatusTrackerDTO.FinalApprovedDate = travelApprovalStatusTracker.FinalApprovedDate;
                TravelApprovalStatusTrackerDTO.ApprovalStatusTypeId = travelApprovalStatusTracker.ApprovalStatusTypeId;
                TravelApprovalStatusTrackerDTO.ApprovalStatusType = _context.ApprovalStatusTypes.Find(travelApprovalStatusTracker.ApprovalStatusTypeId).Status;


                ListTravelApprovalStatusTrackerDTO.Add(TravelApprovalStatusTrackerDTO);

            }


            return Ok(ListTravelApprovalStatusTrackerDTO);

        }


        [HttpGet("{id}")]
        [ActionName("CountOfApprovalsPendingForApprover")]
        public ActionResult<int> GetCountOfApprovalsPendingForApprover(int id)
        {

            if (id == 0)
            {
                return NotFound(new RespStatus { Status = "Failure", Message = "Employee Id is Invalid" });
            }
            //get the RoleID of the Employee (Approver)
            int roleid = _context.Employees.Find(id).RoleId;

            if (roleid == 0)
            {
                return NotFound(new RespStatus { Status = "Failure", Message = "Role Id is Invalid" });
            }

            return Ok(_context.TravelApprovalStatusTrackers.Where(r => r.RoleId == roleid && r.ApprovalStatusTypeId == (int)ApprovalStatus.Pending).Count());

        }


        [HttpGet("{id}")]
        [ActionName("ApprovalFlowForTravelRequest")]
        public ActionResult<IEnumerable<ApprovalStatusFlowVM>> GetApprovalFlowForTravelRequest(int id)
        {

            if (id == 0)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Approval Flow Id is Invalid" });
            }



            var travelRequestTracks = _context.TravelApprovalStatusTrackers.Where(c => c.TravelApprovalRequestId == id).ToList();

            if (travelRequestTracks == null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Approval Flow Id is Not Found" });
            }

            List<ApprovalStatusFlowVM> ListApprovalStatusFlow = new List<ApprovalStatusFlowVM>();

            foreach (TravelApprovalStatusTracker travel in travelRequestTracks)
            {
                ApprovalStatusFlowVM approvalStatusFlow = new ApprovalStatusFlowVM()
                {
                    ApprovalLevel = travel.ApprovalLevelId,
                    ApproverRole = _context.JobRoles.Find(travel.RoleId).RoleName,
                    ApproverName = _context.Employees.Where(x => x.RoleId == travel.RoleId).Select(s => s.FirstName + " " + s.MiddleName + " " + s.LastName).FirstOrDefault(),
                    ApprovedDate = travel.FinalApprovedDate,
                    ApprovalStatusType = _context.ApprovalStatusTypes.Find(travel.ApprovalStatusTypeId).Status

                };
                ListApprovalStatusFlow.Add(approvalStatusFlow);
            }

            return Ok(ListApprovalStatusFlow);

        }



        private enum RequestType
        {
            CashAdvance = 1,
            ExpenseReim

        }

        private enum ApprovalStatus
        {
            Initiating = 1,
            Pending,
            InReview,
            Approved,
            Rejected

        }
    }


}
