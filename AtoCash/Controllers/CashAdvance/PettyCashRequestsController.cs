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
using Microsoft.AspNetCore.Authorization;

namespace AtoCash.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Roles = "AtominosAdmin, Finmgr, Admin, User")]
    [Authorize(Roles = "AtominosAdmin, Finmgr, Admin, User")]

    public class PettyCashRequestsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;
        private readonly IEmailSender _emailSender;

        public PettyCashRequestsController(AtoCashDbContext context, IEmailSender emailSender)
        {
            this._context = context;
            this._emailSender = emailSender;
        }


        // GET: api/PettyCashRequests
        [HttpGet]
        [ActionName("GetPettyCashRequests")]
        public async Task<ActionResult<IEnumerable<PettyCashRequestDTO>>> GetPettyCashRequests()
        {
            List<PettyCashRequestDTO> ListPettyCashRequestDTO = new();

            //var claimApprovalStatusTracker = await _context.ClaimApprovalStatusTrackers.FindAsync(1);

            var pettyCashRequests = await _context.PettyCashRequests.ToListAsync();

            foreach (PettyCashRequest pettyCashRequest in pettyCashRequests)
            {
                PettyCashRequestDTO pettyCashRequestDTO = new();

                pettyCashRequestDTO.Id = pettyCashRequest.Id;
                pettyCashRequestDTO.EmployeeName = _context.Employees.Find(pettyCashRequest.EmployeeId).GetFullName();
                pettyCashRequestDTO.CurrencyTypeId = pettyCashRequest.CurrencyTypeId;
                pettyCashRequestDTO.CurrencyType = pettyCashRequest.CurrencyType != null ? _context.CurrencyTypes.Find(pettyCashRequest.CurrencyType).CurrencyName : null;
                pettyCashRequestDTO.PettyClaimAmount = pettyCashRequest.PettyClaimAmount;
                pettyCashRequestDTO.PettyClaimRequestDesc = pettyCashRequest.PettyClaimRequestDesc;
                pettyCashRequestDTO.Department = pettyCashRequest.DepartmentId != null ? _context.Departments.Find(pettyCashRequest.DepartmentId).DeptCode : null;
                pettyCashRequestDTO.ProjectId = pettyCashRequest.ProjectId;
                pettyCashRequestDTO.Project = pettyCashRequest.ProjectId != null ? _context.Projects.Find(pettyCashRequest.ProjectId).ProjectName : null;
                pettyCashRequestDTO.SubProjectId = pettyCashRequest.SubProjectId;
                pettyCashRequestDTO.SubProject = pettyCashRequest.SubProjectId != null ? _context.SubProjects.Find(pettyCashRequest.SubProjectId).SubProjectName : null;
                pettyCashRequestDTO.WorkTaskId = pettyCashRequest.WorkTaskId;
                pettyCashRequestDTO.WorkTask = pettyCashRequest.WorkTaskId != null ? _context.WorkTasks.Find(pettyCashRequest.WorkTaskId).TaskName : null;
                pettyCashRequestDTO.ApprovalStatusType = pettyCashRequest.ApprovalStatusTypeId != 0 ? _context.ApprovalStatusTypes.Find(pettyCashRequest.ApprovalStatusTypeId).Status : null;
                pettyCashRequestDTO.ApprovalStatusTypeId = pettyCashRequest.ApprovalStatusTypeId;
                pettyCashRequestDTO.ApprovedDate = pettyCashRequest.ApprovedDate;
                ListPettyCashRequestDTO.Add(pettyCashRequestDTO);
            }

            return ListPettyCashRequestDTO;
        }



        // GET: api/PettyCashRequests/5
        [HttpGet("{id}")]
        [ActionName("GetPettyCashRequest")]
        public async Task<ActionResult<PettyCashRequestDTO>> GetPettyCashRequest(int id)
        {


            var pettyCashRequest = await _context.PettyCashRequests.FindAsync(id);

            if (pettyCashRequest == null)
            {
                return NoContent();
            }
            PettyCashRequestDTO pettyCashRequestDTO = new();

            pettyCashRequestDTO.Id = pettyCashRequest.Id;
            pettyCashRequestDTO.EmployeeName = _context.Employees.Find(pettyCashRequest.EmployeeId).GetFullName();
            pettyCashRequestDTO.CurrencyTypeId = pettyCashRequest.CurrencyTypeId;
            pettyCashRequestDTO.CurrencyType = pettyCashRequest.CurrencyType != null ? _context.CurrencyTypes.Find(pettyCashRequest.CurrencyType).CurrencyName : null;
            pettyCashRequestDTO.PettyClaimAmount = pettyCashRequest.PettyClaimAmount;
            pettyCashRequestDTO.PettyClaimRequestDesc = pettyCashRequest.PettyClaimRequestDesc;
            pettyCashRequestDTO.Department = pettyCashRequest.DepartmentId != null ? _context.Departments.Find(pettyCashRequest.DepartmentId).DeptCode : null;
            pettyCashRequestDTO.ProjectId = pettyCashRequest.ProjectId;
            pettyCashRequestDTO.Project = pettyCashRequest.ProjectId != null ? _context.Projects.Find(pettyCashRequest.ProjectId).ProjectName : null;
            pettyCashRequestDTO.SubProjectId = pettyCashRequest.SubProjectId;
            pettyCashRequestDTO.SubProject = pettyCashRequest.SubProjectId != null ? _context.SubProjects.Find(pettyCashRequest.SubProjectId).SubProjectName : null;
            pettyCashRequestDTO.WorkTaskId = pettyCashRequest.WorkTaskId;
            pettyCashRequestDTO.WorkTask = pettyCashRequest.WorkTaskId != null ? _context.WorkTasks.Find(pettyCashRequest.WorkTaskId).TaskName : null;


            return pettyCashRequestDTO;
        }





        [HttpGet("{id}")]
        [ActionName("GetPettyCashRequestRaisedForEmployee")]
        public async Task<ActionResult<IEnumerable<PettyCashRequestDTO>>> GetPettyCashRequestRaisedForEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NoContent();
            }

            var pettyCashRequests = await _context.PettyCashRequests.Where(p => p.EmployeeId == id).ToListAsync();

            if (pettyCashRequests == null)
            {
                return NoContent();
            }

            List<PettyCashRequestDTO> PettyCashRequestDTOs = new();

            foreach (var pettyCashRequest in pettyCashRequests)
            {
                PettyCashRequestDTO pettyCashRequestDTO = new()
                {
                    Id = pettyCashRequest.Id,
                    EmployeeId = pettyCashRequest.EmployeeId,
                    EmployeeName = _context.Employees.Find(pettyCashRequest.EmployeeId).GetFullName(),
                     CurrencyTypeId = pettyCashRequest.CurrencyTypeId,
                      CurrencyType = pettyCashRequest.CurrencyType != null ? _context.CurrencyTypes.Find(pettyCashRequest.CurrencyType).CurrencyName : null,
                PettyClaimAmount = pettyCashRequest.PettyClaimAmount,
                    PettyClaimRequestDesc = pettyCashRequest.PettyClaimRequestDesc,
                    CashReqDate = pettyCashRequest.CashReqDate,
                    DepartmentId = pettyCashRequest.DepartmentId,
                    Department = pettyCashRequest.DepartmentId != null ? _context.Departments.Find(pettyCashRequest.DepartmentId).DeptCode + _context.Departments.Find(pettyCashRequest.DepartmentId).DeptName : null,
                    ProjectId = pettyCashRequest.ProjectId,
                    Project = pettyCashRequest.ProjectId != null ? _context.Projects.Find(pettyCashRequest.ProjectId).ProjectName : null,
                    SubProjectId = pettyCashRequest.SubProjectId,
                    SubProject = pettyCashRequest.SubProjectId != null ? _context.SubProjects.Find(pettyCashRequest.SubProjectId).SubProjectName : null,
                    WorkTaskId = pettyCashRequest.WorkTaskId,
                    WorkTask = pettyCashRequest.WorkTaskId != null ? _context.WorkTasks.Find(pettyCashRequest.WorkTaskId).TaskName : null
                };
                PettyCashRequestDTOs.Add(pettyCashRequestDTO);
            }


            return Ok(PettyCashRequestDTOs);
        }



        [HttpGet("{id}")]
        [ActionName("CountPettyCashRequestRaisedByEmployee")]
        public async Task<ActionResult<int>> CountPettyCashRequestRaisedByEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NoContent();
            }

            var pettyCashRequests = await _context.PettyCashRequests.Where(p => p.EmployeeId == id).ToListAsync();

            if (pettyCashRequests == null)
            {
                return NoContent();
            }

            List<PettyCashRequestDTO> PettyCashRequestDTOs = new();

            foreach (var pettyCashRequest in pettyCashRequests)
            {
                PettyCashRequestDTO pettyCashRequestDTO = new()
                {
                    Id = pettyCashRequest.Id,
                    EmployeeName = _context.Employees.Find(pettyCashRequest.EmployeeId).GetFullName(),
                    CurrencyTypeId = pettyCashRequest.CurrencyTypeId,
                    CurrencyType = pettyCashRequest.CurrencyType != null ? _context.CurrencyTypes.Find(pettyCashRequest.CurrencyType).CurrencyName : null,
                    PettyClaimAmount = pettyCashRequest.PettyClaimAmount,
                    PettyClaimRequestDesc = pettyCashRequest.PettyClaimRequestDesc,
                    CashReqDate = pettyCashRequest.CashReqDate,
                    ProjectId = pettyCashRequest.ProjectId,
                    Project = pettyCashRequest.ProjectId != null ? _context.Projects.Find(pettyCashRequest.ProjectId).ProjectName : null,
                    SubProjectId = pettyCashRequest.SubProjectId,
                    SubProject = pettyCashRequest.SubProjectId != null ? _context.SubProjects.Find(pettyCashRequest.SubProjectId).SubProjectName : null,
                    WorkTaskId = pettyCashRequest.WorkTaskId,
                    WorkTask = pettyCashRequest.WorkTaskId != null ? _context.WorkTasks.Find(pettyCashRequest.WorkTaskId).TaskName : null
                };
                PettyCashRequestDTOs.Add(pettyCashRequestDTO);
            }


            return Ok(PettyCashRequestDTOs.Count);
        }


        [HttpGet("{id}")]
        [ActionName("CountPettyCashReqInPendingRaisedByEmployee")]
        public async Task<ActionResult<int>> CountPettyCashReqInPendingRaisedByEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NoContent();
            }

            //debug
            //(int)ApprovalStatus.Pending
            List<ClaimApprovalStatusTracker> lists = _context.ClaimApprovalStatusTrackers.Where(c => c.EmployeeId == id).ToList();

            var claimApprovalsOfEmployee = await _context.ClaimApprovalStatusTrackers.Where(c => c.EmployeeId == id).Select(p => p.PettyCashRequestId).Distinct().ToListAsync();

            foreach (var item in claimApprovalsOfEmployee)
            {
                string test = item.ToString();

            }

            if (claimApprovalsOfEmployee == null)
            {
                return NoContent();
            }



            return Ok(claimApprovalsOfEmployee.Count);

        }



        [HttpGet]
        [ActionName("GetPettyCashReqInPendingForAll")]
        public async Task<ActionResult<int>> GetPettyCashReqInPendingForAll()
        {
            //debug
            var pettyCashRequests = await _context.PettyCashRequests.Include("ClaimApprovalStatusTrackers").ToListAsync();


            //var pettyCashRequests = await _context.ClaimApprovalStatusTrackers.Where(c => c.ApprovalStatusTypeId == ApprovalStatus.Pending).select( );

            if (pettyCashRequests == null)
            {
                return NoContent();
            }

            return Ok(pettyCashRequests.Count);
        }










        // PUT: api/PettyCashRequests/5
        [HttpPut("{id}")]
        [ActionName("PutPettyCashRequest")]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<IActionResult> PutPettyCashRequest(int id, PettyCashRequestDTO pettyCashRequestDto)
        {
            if (id != pettyCashRequestDto.Id)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Id is invalid" });
            }

            var pettyCashRequest = await _context.PettyCashRequests.FindAsync(id);

            pettyCashRequest.Id = pettyCashRequestDto.Id;

            pettyCashRequest.Id = pettyCashRequestDto.Id;
            pettyCashRequest.CurrencyTypeId = pettyCashRequestDto.CurrencyTypeId;
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
                    return NoContent();
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
        [ActionName("PostPettyCashRequest")]
        public async Task<ActionResult<PettyCashRequest>> PostPettyCashRequest(PettyCashRequestDTO pettyCashRequestDto)
        {

            /*!!=========================================
               Check Eligibility for Cash Disbursement
             .==========================================*/

            Double empCurAvailBal = GetEmpCurrentAvailablePettyCashBalance(pettyCashRequestDto);

            if (pettyCashRequestDto.PettyClaimAmount <= empCurAvailBal && pettyCashRequestDto.PettyClaimAmount > 0)
            {
                await Task.Run(() => ProcessPettyCashRequestClaim(pettyCashRequestDto, empCurAvailBal));

                return Created("PostPettyCashRequest", new RespStatus() { Status = "Success", Message = "Cash Advance Request Created" });

            }
            else
            {
                return Conflict(new RespStatus() { Status = "Failure", Message = "Invalid Cash Request Amount Or Limit Exceeded" });
            }


        }

        // DELETE: api/PettyCashRequests/5
        [HttpDelete("{id}")]
        [ActionName("DeletePettyCashRequest")]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<IActionResult> DeletePettyCashRequest(int id)
        {
            var pettyCashRequest = await _context.PettyCashRequests.FindAsync(id);
            if (pettyCashRequest == null)
            {
                return NoContent();
            }

            _context.PettyCashRequests.Remove(pettyCashRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PettyCashRequestExists(int id)
        {
            return _context.PettyCashRequests.Any(e => e.Id == id);
        }




        private Double GetEmpCurrentAvailablePettyCashBalance(PettyCashRequestDTO pettyCashRequest)
        {
            //If Employee has no previous record of requesting the Cash so add a new record with full balance to amount to "EmpCurrentPettyCashBalance"
            //<<<-----------


            Double empPettyCashAmountEligible = _context.JobRoles.Find(_context.Employees.Find(pettyCashRequest.EmployeeId).RoleId).MaxPettyCashAllowed;


            //Check if employee cash balance is availabel in the EmpCurrentPettyCashBalance table, if NOT then ADD
            if (!_context.EmpCurrentPettyCashBalances.Where(e => e.EmployeeId == pettyCashRequest.EmployeeId).Any())
            {
                _context.EmpCurrentPettyCashBalances.Add(new EmpCurrentPettyCashBalance()
                {
                    EmployeeId = pettyCashRequest.EmployeeId,
                    CurBalance = empPettyCashAmountEligible,
                    UpdatedOn = DateTime.Now
                }); ;

                _context.SaveChangesAsync();

                return empPettyCashAmountEligible;
            }

            return _context.EmpCurrentPettyCashBalances.Where(e => e.EmployeeId == pettyCashRequest.EmployeeId).Select(b => b.CurBalance).FirstOrDefault();


        }



        //NO HTTPACTION HERE. Void method just to add data to database table
        private async Task ProcessPettyCashRequestClaim(PettyCashRequestDTO pettyCashRequestDto, Double empCurAvailBal)
        {

            if (pettyCashRequestDto.ProjectId != null)
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
        private async Task<IActionResult> ProjectCashRequest(PettyCashRequestDTO pettyCashRequestDto, Double empCurAvailBal)
        {

            //### 1. If Employee Eligible for Cash Claim enter a record and reduce the available amount for next claim
            #region
            int costCentre = _context.Projects.Find(pettyCashRequestDto.ProjectId).CostCentreId;

            int projManagerid = _context.Projects.Find(pettyCashRequestDto.ProjectId).ProjectManagerId;

            var approver = _context.Employees.Find(projManagerid);
            ////
            int empid = pettyCashRequestDto.EmployeeId;
            Double empReqAmount = pettyCashRequestDto.PettyClaimAmount;
            //int empApprGroupId = _context.Employees.Find(empid).ApprovalGroupId;
            double maxCashAllowedForRole = (_context.JobRoles.Find(_context.Employees.Find(pettyCashRequestDto.EmployeeId).RoleId).MaxPettyCashAllowed);

            if (pettyCashRequestDto.PettyClaimAmount > maxCashAllowedForRole)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Advance Amount is not eligibile" });
            }

            var curPettyCashBal = _context.EmpCurrentPettyCashBalances.Where(x => x.EmployeeId == empid).FirstOrDefault();
            curPettyCashBal.Id = curPettyCashBal.Id;
            curPettyCashBal.CurBalance = empCurAvailBal - empReqAmount <= maxCashAllowedForRole ? empCurAvailBal - empReqAmount : maxCashAllowedForRole;
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
                DepartmentId = null,
                ProjectId = pettyCashRequestDto.ProjectId,
                SubProjectId = pettyCashRequestDto.SubProjectId,
                WorkTaskId = pettyCashRequestDto.WorkTaskId,
                PettyClaimRequestDesc = pettyCashRequestDto.PettyClaimRequestDesc,
                CurrencyTypeId = pettyCashRequestDto.CurrencyTypeId,
                ApprovalStatusTypeId = (int)EApprovalStatus.Initiating
                
            };
            _context.PettyCashRequests.Add(pcrq);
            await _context.SaveChangesAsync(); 
            
            pettyCashRequestDto.Id = pcrq.Id;
            #endregion

            //##### 3. Add an entry to ClaimApproval Status tracker
            //get costcentreID based on project
            #region

            ///////////////////////////// Check if self Approved Request /////////////////////////////
            int maxApprLevel = _context.ApprovalRoleMaps.Max(a => a.ApprovalLevelId);
            int empApprLevel = _context.ApprovalRoleMaps.Where(a => a.RoleId == _context.Employees.Find(empid).RoleId).FirstOrDefault().Id;
            bool isSelfApprovedRequest = false;
            //if highest approver is requesting Petty cash request himself
            if (maxApprLevel == empApprLevel)
            {
                isSelfApprovedRequest = true;
            }
            //////////////////////////////////////////////////////////////////////////////////////////
            if (isSelfApprovedRequest)
            {
                ClaimApprovalStatusTracker claimAppStatusTrack = new()
                {
                    EmployeeId = pettyCashRequestDto.EmployeeId,
                    PettyCashRequestId = pettyCashRequestDto.Id,
                    ExpenseReimburseRequestId = null,
                    DepartmentId = null,
                    ProjectId = pettyCashRequestDto.ProjectId,
                    RoleId = approver.RoleId,
                    // get the next ProjectManager approval.
                    ApprovalLevelId = empApprLevel, // default approval level is 2 for Project based request
                    ReqDate = DateTime.Now,
                    FinalApprovedDate = DateTime.Now,
                    ApprovalStatusTypeId = (int)ApprovalStatus.Approved, //1-Initiating, 2-Pending, 3-InReview, 4-Approved, 5-Rejected
                    Comments = "Awaiting Approver Action"
                };


                _context.ClaimApprovalStatusTrackers.Add(claimAppStatusTrack);
                await _context.SaveChangesAsync();
            }
            else
            {
                ClaimApprovalStatusTracker claimAppStatusTrack = new()
                {
                    EmployeeId = pettyCashRequestDto.EmployeeId,
                    PettyCashRequestId = pettyCashRequestDto.Id,
                    ExpenseReimburseRequestId = null,
                    DepartmentId = null,
                    ProjectId = pettyCashRequestDto.ProjectId,
                    RoleId = approver.RoleId,
                    // get the next ProjectManager approval.
                    ApprovalLevelId = 2, // default approval level is 2 for Project based request
                    ReqDate = DateTime.Now,
                    FinalApprovedDate = null,
                    ApprovalStatusTypeId = (int)ApprovalStatus.Pending, //1-Initiating, 2-Pending, 3-InReview, 4-Approved, 5-Rejected
                    Comments = "Awaiting Approver Action"
                };


                _context.ClaimApprovalStatusTrackers.Add(claimAppStatusTrack);
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
            }



            //##### 5. Adding a entry in DisbursementsAndClaimsMaster table for records
            #region
            _context.DisbursementsAndClaimsMasters.Add(new DisbursementsAndClaimsMaster()
            {
                EmployeeId = pettyCashRequestDto.EmployeeId,
                PettyCashRequestId = pettyCashRequestDto.Id,
                ExpenseReimburseReqId = null,
                RequestTypeId = (int)ClaimType.CashAdvance,
                DepartmentId = null,
                ProjectId = pettyCashRequestDto.ProjectId,
                SubProjectId = pettyCashRequestDto.SubProjectId,
                WorkTaskId = pettyCashRequestDto.WorkTaskId,
                RecordDate = DateTime.Now,
                CurrencyTypeId = pettyCashRequestDto.CurrencyTypeId,
                Amount = pettyCashRequestDto.PettyClaimAmount,
                CostCentreId = _context.Departments.Find(_context.Employees.Find(pettyCashRequestDto.EmployeeId).DepartmentId).CostCentreId,
                ApprovalStatusId = (int)ApprovalStatus.Pending //1-Initiating, 2-Pending, 3-InReview, 4-Approved, 5-Rejected
            });
            await _context.SaveChangesAsync();
            #endregion

            return Ok(new RespStatus { Status = "Success", Message = "Advance Request Created!" });
        }

        /// <summary>
        /// This is option 2 : DEPARTMENT BASED CASH ADVANCE REQUEST
        /// </summary>
        /// <param name="pettyCashRequestDto"></param>
        /// <param name="empCurAvailBal"></param>
        private async Task DepartmentCashRequest(PettyCashRequestDTO pettyCashRequestDto, Double empCurAvailBal)
        {
            //### 1. If Employee Eligible for Cash Claim enter a record and reduce the available amount for next claim
            #region
            int reqEmpid = pettyCashRequestDto.EmployeeId;
            Double empReqAmount = pettyCashRequestDto.PettyClaimAmount;
            int empApprGroupId = _context.Employees.Find(reqEmpid).ApprovalGroupId;



            var curPettyCashBal = _context.EmpCurrentPettyCashBalances.Where(x => x.EmployeeId == reqEmpid).FirstOrDefault();
            curPettyCashBal.Id = curPettyCashBal.Id;
            if (_context.JobRoles.Find(_context.Employees.Find(pettyCashRequestDto.EmployeeId).RoleId).MaxPettyCashAllowed >= empCurAvailBal - empReqAmount)
            {
                curPettyCashBal.CurBalance = empCurAvailBal - empReqAmount;
            }

            curPettyCashBal.EmployeeId = reqEmpid;
            curPettyCashBal.UpdatedOn = DateTime.Now;
            _context.Update(curPettyCashBal);
            await _context.SaveChangesAsync();

            #endregion

            //##### 2. Adding entry to PettyCashRequest table for record
            #region
            var pcrq = new PettyCashRequest()
            {
                EmployeeId = reqEmpid,
                PettyClaimAmount = empReqAmount,
                CashReqDate = DateTime.Now,
                PettyClaimRequestDesc = pettyCashRequestDto.PettyClaimRequestDesc,
                ProjectId = pettyCashRequestDto.ProjectId,
                SubProjectId = pettyCashRequestDto.SubProjectId,
                WorkTaskId = pettyCashRequestDto.WorkTaskId,
                DepartmentId = _context.Employees.Find(reqEmpid).DepartmentId,
                CurrencyTypeId = pettyCashRequestDto.CurrencyTypeId,
                ApprovalStatusTypeId = (int)EApprovalStatus.Initiating

            };
            _context.PettyCashRequests.Add(pcrq);
            await _context.SaveChangesAsync();

            //get the saved record Id
            pettyCashRequestDto.Id = pcrq.Id;

            #endregion

            //##### STEP 3. ClaimsApprovalTracker to be updated for all the allowed Approvers


            ///////////////////////////// Check if self Approved Request /////////////////////////////
            int maxApprLevel = _context.ApprovalRoleMaps.Max(a => a.ApprovalLevelId);
            int empApprLevel = _context.ApprovalRoleMaps.Where(a => a.RoleId == _context.Employees.Find(reqEmpid).RoleId).FirstOrDefault().Id;
            bool isSelfApprovedRequest = false;
            //if highest approver is requesting Petty cash request himself
            if (maxApprLevel == empApprLevel)
            {
                isSelfApprovedRequest = true;
            }
            //////////////////////////////////////////////////////////////////////////////////////////

            if (isSelfApprovedRequest)
            {

                ClaimApprovalStatusTracker claimAppStatusTrack = new()
                {
                    EmployeeId = pettyCashRequestDto.EmployeeId,
                    PettyCashRequestId = pettyCashRequestDto.Id,
                    ExpenseReimburseRequestId = null,
                    DepartmentId = _context.Employees.Find(pettyCashRequestDto.EmployeeId).DepartmentId,
                    ProjectId = null,
                    RoleId = _context.Employees.Find(pettyCashRequestDto.EmployeeId).RoleId,
                    ApprovalLevelId = empApprLevel,
                    ReqDate = DateTime.Now,
                    FinalApprovedDate = DateTime.Now,
                    ApprovalStatusTypeId =  (int)ApprovalStatus.Approved,
                    Comments = "Awaiting Approver Action"
                    //1-Initiating, 2-Pending, 3-InReview, 4-Approved, 5-Rejected
                };
                _context.ClaimApprovalStatusTrackers.Add(claimAppStatusTrack);
                await _context.SaveChangesAsync();
            }
            else
            {
                var getEmpClaimApproversAllLevels = _context.ApprovalRoleMaps.Include(a => a.ApprovalLevel).ToList().OrderBy(o => o.ApprovalLevel.Level);

                var ReqEmpRoleId = _context.Employees.Where(e => e.Id == reqEmpid).FirstOrDefault().RoleId;
                var ReqEmpHisOwnApprLevel = _context.ApprovalRoleMaps.Where(a => a.RoleId == ReqEmpRoleId);
                bool isFirstApprover = true;
                foreach (ApprovalRoleMap ApprMap in getEmpClaimApproversAllLevels)
                {

                    int role_id = ApprMap.RoleId;
                    var approver = _context.Employees.Where(e => e.RoleId == role_id).FirstOrDefault();

                    if (ReqEmpRoleId >= approver.RoleId)
                    {
                        continue;
                    }



                    ClaimApprovalStatusTracker claimAppStatusTrack = new()
                    {
                        EmployeeId = pettyCashRequestDto.EmployeeId,
                        PettyCashRequestId = pettyCashRequestDto.Id,
                        ExpenseReimburseRequestId = null,
                        DepartmentId = approver.DepartmentId,
                        ProjectId = null,
                        RoleId = approver.RoleId,
                        ApprovalLevelId = ApprMap.ApprovalLevelId,
                        ReqDate = DateTime.Now,
                        FinalApprovedDate = null,
                        ApprovalStatusTypeId = isFirstApprover ? (int)ApprovalStatus.Pending : (int)ApprovalStatus.Initiating,
                        Comments = "Awaiting Approver Action"
                        //1-Initiating, 2-Pending, 3-InReview, 4-Approved, 5-Rejected
                    };


                    _context.ClaimApprovalStatusTrackers.Add(claimAppStatusTrack);
                    await _context.SaveChangesAsync();


                    if (isFirstApprover)
                    {
                        //##### 4. Send email to the Approver
                        //####################################
                        var approverMailAddress = approver.Email;
                        string subject = "Pettycash Request Approval " + pettyCashRequestDto.Id.ToString();
                        Employee emp = await _context.Employees.FindAsync(pettyCashRequestDto.EmployeeId);
                        var pettycashreq = _context.PettyCashRequests.Find(pettyCashRequestDto.Id);
                        string content = "Petty Cash Approval sought by " + emp.FirstName + "@<br/>Cash Request for the amount of " + pettycashreq.PettyClaimAmount + "@<br/>towards " + pettycashreq.PettyClaimRequestDesc;
                        var messagemail = new Message(new string[] { approverMailAddress }, subject, content);

                        await _emailSender.SendEmailAsync(messagemail);
                    }

                    //first approver will be added as Pending, other approvers will be with In Approval Queue
                    isFirstApprover = false;

                }

            }

            //##### STEP 5. Adding a SINGLE entry in DisbursementsAndClaimsMaster table for records
            #region
            DisbursementsAndClaimsMaster disbursementsAndClaimsMaster = new();

            disbursementsAndClaimsMaster.EmployeeId = reqEmpid;
            disbursementsAndClaimsMaster.PettyCashRequestId = pcrq.Id;
            disbursementsAndClaimsMaster.ExpenseReimburseReqId = null;
            disbursementsAndClaimsMaster.RequestTypeId = (int)ClaimType.CashAdvance;
            disbursementsAndClaimsMaster.DepartmentId = _context.Employees.Find(reqEmpid).DepartmentId;
            disbursementsAndClaimsMaster.ProjectId = null;
            disbursementsAndClaimsMaster.SubProjectId = null;
            disbursementsAndClaimsMaster.WorkTaskId = null;
            disbursementsAndClaimsMaster.RecordDate = DateTime.Now;
            disbursementsAndClaimsMaster.CurrencyTypeId = pettyCashRequestDto.CurrencyTypeId;
            disbursementsAndClaimsMaster.Amount = empReqAmount;
            disbursementsAndClaimsMaster.CostCentreId = _context.Departments.Find(_context.Employees.Find(reqEmpid).DepartmentId).CostCentreId;
            disbursementsAndClaimsMaster.ApprovalStatusId = isSelfApprovedRequest ? (int)ApprovalStatus.Approved: (int)ApprovalStatus.Pending;

            _context.DisbursementsAndClaimsMasters.Add(disbursementsAndClaimsMaster);
            await _context.SaveChangesAsync();
            #endregion

        }


        private enum ApprovalStatus
        {
            Initiating = 1,
            Pending,
            InReview,
            Approved,
            Rejected

        }

        private enum ClaimType
        {
            CashAdvance = 1,
            ExpenseReim

        }

    }
}
