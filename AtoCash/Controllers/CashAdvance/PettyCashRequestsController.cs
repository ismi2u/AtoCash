using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtoCash.Data;
using AtoCash.Models;
using EmailService;
using AtoCash.Authentication;

namespace AtoCash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PettyCashRequestsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;
        private readonly IEmailSender _emailSender;

        public PettyCashRequestsController(AtoCashDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: api/PettyCashRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PettyCashRequestDTO>>> GetPettyCashRequests()
        {
            List<PettyCashRequestDTO> ListPettyCashRequestDTO = new List<PettyCashRequestDTO>();

            var pettyCashRequests = await _context.PettyCashRequests.ToListAsync();

            foreach (PettyCashRequest pettyCashRequest in pettyCashRequests)
            {
                PettyCashRequestDTO pettyCashRequestsDTO = new PettyCashRequestDTO
                {
                    Id = pettyCashRequest.Id,
                    EmployeeId = pettyCashRequest.EmployeeId,
                    PettyClaimAmount = pettyCashRequest.PettyClaimAmount,
                    PettyClaimRequestDesc = pettyCashRequest.PettyClaimRequestDesc,
                    CashReqDate = pettyCashRequest.CashReqDate,
                    ProjectId = pettyCashRequest.ProjectId,
                    SubProjectId = pettyCashRequest.SubProjectId,
                    WorkTaskId = pettyCashRequest.WorkTaskId
                };

                ListPettyCashRequestDTO.Add(pettyCashRequestsDTO);

            }

            return ListPettyCashRequestDTO;
        }

        // GET: api/PettyCashRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PettyCashRequestDTO>> GetPettyCashRequest(int id)
        {
            PettyCashRequestDTO pettyCashRequestDTO = new PettyCashRequestDTO();

            var pettyCashRequest = await _context.PettyCashRequests.FindAsync(id);

            if (pettyCashRequest == null)
            {
                return NotFound();
            }

            pettyCashRequestDTO.Id = pettyCashRequest.Id;
            pettyCashRequestDTO.EmployeeId = pettyCashRequest.EmployeeId;
            pettyCashRequestDTO.PettyClaimAmount = pettyCashRequest.PettyClaimAmount;
            pettyCashRequestDTO.PettyClaimRequestDesc = pettyCashRequest.PettyClaimRequestDesc;
            pettyCashRequestDTO.CashReqDate = pettyCashRequest.CashReqDate;
            pettyCashRequestDTO.ProjectId = pettyCashRequest.ProjectId;
            pettyCashRequestDTO.SubProjectId = pettyCashRequest.SubProjectId;
            pettyCashRequestDTO.WorkTaskId = pettyCashRequest.WorkTaskId;

            return pettyCashRequestDTO;
        }

        // PUT: api/PettyCashRequests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPettyCashRequest(int id, PettyCashRequestDTO pettyCashRequestDto)
        {
            if (id != pettyCashRequestDto.Id)
            {
                return BadRequest();
            }

            var pettyCashRequest = await _context.PettyCashRequests.FindAsync(id);

            pettyCashRequest.Id = pettyCashRequestDto.Id;

            pettyCashRequest.Id = pettyCashRequestDto.Id;
            pettyCashRequest.EmployeeId = pettyCashRequestDto.EmployeeId;
            pettyCashRequest.PettyClaimAmount = pettyCashRequestDto.PettyClaimAmount;
            pettyCashRequest.PettyClaimRequestDesc = pettyCashRequestDto.PettyClaimRequestDesc;
            pettyCashRequest.CashReqDate = pettyCashRequestDto.CashReqDate;
            pettyCashRequest.ProjectId = pettyCashRequestDto.ProjectId;
            pettyCashRequest.SubProjectId = pettyCashRequestDto.SubProjectId;
            pettyCashRequest.WorkTaskId = pettyCashRequestDto.WorkTaskId;


            _context.PettyCashRequests.Update(pettyCashRequest);
            //_context.Entry(pettyCashRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PettyCashRequestExists(id))
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

        // POST: api/PettyCashRequests
        [HttpPost]
        public async Task<ActionResult<PettyCashRequest>> PostPettyCashRequest(PettyCashRequestDTO pettyCashRequestDto)
        {

            /*!!=========================================
               Check Eligibility for Cash Disbursement
             .==========================================*/

            decimal empCurAvailBal = GetEmpCurrentAvailablePettyCashBalance(pettyCashRequestDto);

            if (pettyCashRequestDto.PettyClaimAmount <= empCurAvailBal && pettyCashRequestDto.PettyClaimAmount > 0)
            {
                await Task.Run(() => ProcessPettyCashRequestClaim(pettyCashRequestDto, empCurAvailBal));

                return Ok(pettyCashRequestDto);

            }
            else
            {
                return BadRequest(new RespStatus() { Status = "Failure", Message = "Invalid Cash Request Amount Or Limit Exceeded" });
            }


        }

        // DELETE: api/PettyCashRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePettyCashRequest(int id)
        {
            var pettyCashRequest = await _context.PettyCashRequests.FindAsync(id);
            if (pettyCashRequest == null)
            {
                return NotFound();
            }

            _context.PettyCashRequests.Remove(pettyCashRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PettyCashRequestExists(int id)
        {
            return _context.PettyCashRequests.Any(e => e.Id == id);
        }



        private enum ApprovalStatus
        {
            Pending = 1,
            Approved,
            Rejected

        }

        private enum ClaimType
        {
            CashAdvance = 1,
            ExpenseReim

        }

        private decimal GetEmpCurrentAvailablePettyCashBalance(PettyCashRequestDTO pettyCashRequest)
        {
            //If Employee has no previous record of requesting the Cash so add a new record with full balance to amount to "EmpCurrentPettyCashBalance"
            //<<<-----------
            decimal empPettyCashAmounteligible = _context.JobRoles.Find(_context.Employees.Find(pettyCashRequest.EmployeeId).RoleId).MaxPettyCashAllowed;

            //Check if employee cash balance is availabel in the EmpCurrentPettyCashBalance table, if NOT then ADD
            if (!_context.EmpCurrentPettyCashBalances.Where(e => e.EmployeeId == pettyCashRequest.EmployeeId).Any())
            {
                _context.EmpCurrentPettyCashBalances.Add(new EmpCurrentPettyCashBalance()
                {
                    EmployeeId = pettyCashRequest.EmployeeId,
                    CurBalance = empPettyCashAmounteligible
                });

                _context.SaveChangesAsync();

                return empPettyCashAmounteligible;
            }

            return _context.EmpCurrentPettyCashBalances.Find(pettyCashRequest.EmployeeId).CurBalance;


        }



        //NO HTTPACTION HERE. Void method just to add data to database table
        private async Task ProcessPettyCashRequestClaim(PettyCashRequestDTO pettyCashRequestDto, decimal empCurAvailBal)
        {

            if (pettyCashRequestDto.ProjectId == null)
            {
                //Goes to Option 1 (Project)
                await Task.Run(() => ProjectCashRequest(pettyCashRequestDto, empCurAvailBal));
            }
            else
            {
                //Goes to Option 2 (Department)
                await Task.Run(() => DepartmentCashRequest(pettyCashRequestDto, empCurAvailBal));
            }

        }


        /// <summary>
        /// This is the option 1 : : PROJECT BASED CASH ADVANCE REQUEST
        /// </summary>
        /// <param name="pettyCashRequestDto"></param>
        /// <param name="empCurAvailBal"></param>
        private async Task ProjectCashRequest(PettyCashRequestDTO pettyCashRequestDto, decimal empCurAvailBal)
        {

            //### 1. If Employee Eligible for Cash Claim enter a record and reduce the available amount for next claim
            #region
            int costCentre = _context.Projects.Find(pettyCashRequestDto.ProjectId).CostCentreId;

            int projManagerid = _context.ProjectManagements.Find(pettyCashRequestDto.ProjectId).EmployeeId;

            var approver = _context.Employees.Find(projManagerid);
            ////
            int empid = pettyCashRequestDto.EmployeeId;
            decimal empReqAmount = pettyCashRequestDto.PettyClaimAmount;
            int empApprGroupId = _context.Employees.Find(empid).ApprovalGroupId;



            var curPettyCashBal = _context.EmpCurrentPettyCashBalances.Where(x => x.EmployeeId == empid).FirstOrDefault();
            curPettyCashBal.Id = curPettyCashBal.Id;
            curPettyCashBal.CurBalance = empCurAvailBal - empReqAmount;
            curPettyCashBal.EmployeeId = empid;
            _context.Update(curPettyCashBal);
            await _context.SaveChangesAsync();
            #endregion

            //##### 2. Adding entry to PettyCashRequest table for record
            #region
            var pcrq = new PettyCashRequest()
            {
                EmployeeId = empid,
                PettyClaimAmount = empReqAmount,
                CashReqDate = DateTime.Now,
                DepartmentId = pettyCashRequestDto.DepartmentId,
                ProjectId = pettyCashRequestDto.ProjectId,
                SubProjectId = pettyCashRequestDto.SubProjectId,
                WorkTaskId = pettyCashRequestDto.WorkTaskId
            };
            _context.PettyCashRequests.Add(pcrq);
            await _context.SaveChangesAsync();
            #endregion

            //##### 3. Add an entry to ClaimApproval Status tracker
            //get costcentreID based on project
            #region

            _context.ClaimApprovalStatusTrackers.Add(new ClaimApprovalStatusTracker
            {

                EmployeeId = pettyCashRequestDto.EmployeeId,
                PettyCashRequestId = pettyCashRequestDto.Id,
                DepartmentId = null,
                ProjectId = pettyCashRequestDto.ProjectId.Value,
                RoleId = approver.RoleId,
                ReqDate = DateTime.Now,
                FinalApprovedDate = null,
                ApprovalStatusTypeId = (int)ApprovalStatus.Pending //1-Pending, 2-Approved, 3-Rejected

            });

            await _context.SaveChangesAsync();
            #endregion


            //##### 4. Send email to the user
            //####################################
            #region
            var approverMailAddress = approver.Email;
            string subject = "Pettycash Request Approval " + pettyCashRequestDto.Id.ToString();
            Employee emp = await _context.Employees.FindAsync(pettyCashRequestDto.EmployeeId);
            var pettycashreq = _context.PettyCashRequests.Find(pettyCashRequestDto.Id);
            string content = "Petty Cash Approval sought by " + emp.FirstName + "/nCash Request for the amount of " + pettycashreq.PettyClaimAmount + "/ntowards " + pettycashreq.PettyClaimRequestDesc;
            var messagemail = new Message(new string[] { approverMailAddress }, subject, content);

            await _emailSender.SendEmailAsync(messagemail);
            #endregion


            //##### 5. Adding a entry in DisbursementsAndClaimsMaster table for records
            #region
            _context.DisbursementsAndClaimsMasters.Add(new DisbursementsAndClaimsMaster()
            {
                EmployeeId = pettyCashRequestDto.EmployeeId,
                PettyCashRequestId = pettyCashRequestDto.Id,
                ExpenseReimburseReqId = null,
                RequestTypeId = (int)ClaimType.CashAdvance,
                DepartmentId = _context.Employees.Find(pettyCashRequestDto.EmployeeId).DepartmentId,
                ProjectId = pettyCashRequestDto.ProjectId,
                SubProjectId = pettyCashRequestDto.SubProjectId,
                WorkTaskId = pettyCashRequestDto.WorkTaskId,
                RecordDate = DateTime.Now,
                Amount = pettyCashRequestDto.PettyClaimAmount,
                CostCentreId = _context.Departments.Find(_context.Employees.Find(pettyCashRequestDto.EmployeeId).DepartmentId).CostCentreId,
                ApprovalStatusId = (int)ApprovalStatus.Pending
            });
            await _context.SaveChangesAsync();
            #endregion
        }

        /// <summary>
        /// This is option 2 : DEPARTMENT BASED CASH ADVANCE REQUEST
        /// </summary>
        /// <param name="pettyCashRequestDto"></param>
        /// <param name="empCurAvailBal"></param>
        private async Task DepartmentCashRequest(PettyCashRequestDTO pettyCashRequestDto, decimal empCurAvailBal)
        {
            //### 1. If Employee Eligible for Cash Claim enter a record and reduce the available amount for next claim
            #region
            int empid = pettyCashRequestDto.EmployeeId;
            decimal empReqAmount = pettyCashRequestDto.PettyClaimAmount;
            int empApprGroupId = _context.Employees.Find(empid).ApprovalGroupId;

           

            var curPettyCashBal = _context.EmpCurrentPettyCashBalances.Where(x => x.EmployeeId == empid).FirstOrDefault();
            curPettyCashBal.Id = curPettyCashBal.Id;
            curPettyCashBal.CurBalance = empCurAvailBal - empReqAmount;
            curPettyCashBal.EmployeeId = empid;
            _context.Update(curPettyCashBal);
            await _context.SaveChangesAsync();

            #endregion

            //##### 2. Adding entry to PettyCashRequest table for record
            #region
            var pcrq = new PettyCashRequest()
            {
                EmployeeId = empid,
                PettyClaimAmount = empReqAmount,
                CashReqDate = DateTime.Now,
                ProjectId = pettyCashRequestDto.ProjectId,
                SubProjectId = pettyCashRequestDto.SubProjectId,
                WorkTaskId = pettyCashRequestDto.WorkTaskId


            };
            _context.PettyCashRequests.Add(pcrq);
            await _context.SaveChangesAsync();
            #endregion

            //##### STEP 3. ClaimsApprovalTracker to be updated for all the allowed Approvers
            //##### STEP 4. Send email to all the allowed Approvers
            #region
            var getEmpClaimApproversAllLevels = _context.ApprovalRoleMaps.Where(a => a.ApprovalGroupId == empApprGroupId).ToList().OrderBy(a => a.ApprovalLevel);

            foreach (ApprovalRoleMap ApprMap in getEmpClaimApproversAllLevels)
            {

                int role_id = ApprMap.RoleId;
                var approver = _context.Employees.Where(e => e.RoleId == role_id).FirstOrDefault();

                _context.ClaimApprovalStatusTrackers.Add(new ClaimApprovalStatusTracker
                {
                    EmployeeId = pettyCashRequestDto.EmployeeId,
                    PettyCashRequestId = pettyCashRequestDto.Id,
                    DepartmentId = approver.DepartmentId,
                    ProjectId = null,
                    RoleId = approver.RoleId,
                    ReqDate = DateTime.Now,
                    FinalApprovedDate = null,
                    ApprovalStatusTypeId = (int)ApprovalStatus.Pending //1-Pending, 2-Approved, 3-Rejected
                });
                await _context.SaveChangesAsync();


                //##### 4. Send email to the Approver
                //####################################
                var approverMailAddress = approver.Email;
                string subject = "Pettycash Request Approval " + pettyCashRequestDto.Id.ToString();
                Employee emp = await _context.Employees.FindAsync(pettyCashRequestDto.EmployeeId);
                var pettycashreq = _context.PettyCashRequests.Find(pettyCashRequestDto.Id);
                string content = "Petty Cash Approval sought by " + emp.FirstName + "/nCash Request for the amount of " + pettycashreq.PettyClaimAmount + "/ntowards " + pettycashreq.PettyClaimRequestDesc;
                var messagemail = new Message(new string[] { approverMailAddress }, subject, content);

                await _emailSender.SendEmailAsync(messagemail);

            }
            #endregion

            //##### STEP 5. Adding a SINGLE entry in DisbursementsAndClaimsMaster table for records
            #region
            _context.DisbursementsAndClaimsMasters.Add(new DisbursementsAndClaimsMaster()
            {
                EmployeeId = empid,
                PettyCashRequestId = pcrq.Id,
                ExpenseReimburseReqId = null,
                RequestTypeId = (int)ClaimType.CashAdvance,
                DepartmentId = _context.Employees.Find(empid).DepartmentId,
                ProjectId = pettyCashRequestDto.ProjectId,
                SubProjectId = pettyCashRequestDto.SubProjectId,
                WorkTaskId = pettyCashRequestDto.WorkTaskId,
                RecordDate = DateTime.Now,
                Amount = empReqAmount,
                CostCentreId = _context.Departments.Find(_context.Employees.Find(empid).DepartmentId).CostCentreId,
                ApprovalStatusId = (int)ApprovalStatus.Pending
            });
            await _context.SaveChangesAsync();
            #endregion

        }


    }
}
