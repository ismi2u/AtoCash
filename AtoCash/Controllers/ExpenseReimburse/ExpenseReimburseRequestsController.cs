using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtoCash.Data;
using AtoCash.Models;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using EmailService;

namespace AtoCash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseReimburseRequestsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IEmailSender _emailSender;

        public ExpenseReimburseRequestsController(AtoCashDbContext context, IWebHostEnvironment hostEnv, IEmailSender emailSender)
        {
            _context = context;
            hostingEnvironment = hostEnv;
            _emailSender = emailSender;
        }

        // GET: api/ExpenseReimburseRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseReimburseRequestDTO>>> GetExpenseReimburseRequests()
        {
            List<ExpenseReimburseRequestDTO> ListExpenseReimburseRequestDTO = new List<ExpenseReimburseRequestDTO>();

            var expenseReimburseRequests = await _context.ExpenseReimburseRequests.ToListAsync();

            foreach (ExpenseReimburseRequest expenseReimburseRequest in expenseReimburseRequests)
            {
                ExpenseReimburseRequestDTO expenseReimburseRequestDTO = new ExpenseReimburseRequestDTO();

                expenseReimburseRequestDTO.Id = expenseReimburseRequest.Id;
                expenseReimburseRequestDTO.EmployeeId = expenseReimburseRequest.EmployeeId;
                expenseReimburseRequestDTO.ExpenseReimbClaimAmount = expenseReimburseRequest.ExpenseReimbClaimAmount;

                //collect the saved images and conver to IformFile and send to User.
                ///


                //expenseReimburseRequestDTO.Documents = expenseReimburseRequest.Documents;


                ////
                expenseReimburseRequestDTO.ExpReimReqDate = expenseReimburseRequest.ExpReimReqDate;
                expenseReimburseRequestDTO.ExpenseTypeId = expenseReimburseRequest.ExpenseTypeId;
                expenseReimburseRequestDTO.ProjectId = expenseReimburseRequest.ProjectId;
                expenseReimburseRequestDTO.SubProjectId = expenseReimburseRequest.SubProjectId;
                expenseReimburseRequestDTO.WorkTaskId = expenseReimburseRequest.WorkTaskId;
    

                ListExpenseReimburseRequestDTO.Add(expenseReimburseRequestDTO);

            }

            return ListExpenseReimburseRequestDTO;
        }

        // GET: api/ExpenseReimburseRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseReimburseRequestDTO>> GetExpenseReimburseRequest(int id)
        {
            ExpenseReimburseRequestDTO expenseReimburseRequestDTO = new ExpenseReimburseRequestDTO();

            var expenseReimburseRequest = await _context.ExpenseReimburseRequests.FindAsync(id);

            if (expenseReimburseRequest == null)
            {
                return NotFound();
            }

            expenseReimburseRequestDTO.Id = expenseReimburseRequest.Id;
            expenseReimburseRequestDTO.EmployeeId = expenseReimburseRequest.EmployeeId;
            expenseReimburseRequestDTO.ExpenseReimbClaimAmount = expenseReimburseRequest.ExpenseReimbClaimAmount;

            //collect the saved images and conver to IformFile and send to User.

            //Pending
            //expenseReimburseRequestDTO.Documents = expenseReimburseRequest.Documents;



            expenseReimburseRequestDTO.ExpReimReqDate = expenseReimburseRequest.ExpReimReqDate;
            expenseReimburseRequestDTO.ExpenseTypeId = expenseReimburseRequest.ExpenseTypeId;
            expenseReimburseRequestDTO.ProjectId = expenseReimburseRequest.ProjectId;
            expenseReimburseRequestDTO.SubProjectId = expenseReimburseRequest.SubProjectId;
            expenseReimburseRequestDTO.WorkTaskId = expenseReimburseRequest.WorkTaskId;

            return expenseReimburseRequestDTO;
        }

        // PUT: api/ExpenseReimburseRequests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpenseReimburseRequest(int id, ExpenseReimburseRequestDTO expenseReimburseRequestDto)
        {
            if (id != expenseReimburseRequestDto.Id)
            {
                return BadRequest();
            }

            var expenseReimburseRequest = await _context.ExpenseReimburseRequests.FindAsync(id);

            expenseReimburseRequest.Id = expenseReimburseRequestDto.Id;
            expenseReimburseRequest.EmployeeId = expenseReimburseRequestDto.EmployeeId;
            expenseReimburseRequest.ExpenseReimbClaimAmount = expenseReimburseRequestDto.ExpenseReimbClaimAmount;

            //receiving Iformfile so process it and save to folder =>
            expenseReimburseRequest.Documents = await SaveFileToFolderAndGetName(expenseReimburseRequestDto);
            expenseReimburseRequest.ExpReimReqDate = expenseReimburseRequestDto.ExpReimReqDate;
            expenseReimburseRequest.ExpenseTypeId = expenseReimburseRequestDto.ExpenseTypeId;
            expenseReimburseRequest.ProjectId = expenseReimburseRequestDto.ProjectId;
            expenseReimburseRequest.SubProjectId = expenseReimburseRequestDto.SubProjectId;
            expenseReimburseRequest.WorkTaskId = expenseReimburseRequestDto.WorkTaskId;

            _context.ExpenseReimburseRequests.Update(expenseReimburseRequest);
            //_context.Entry(expenseReimburseRequestDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseReimburseRequestExists(id))
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

        // POST: api/ExpenseReimburseRequests
        [HttpPost]
        public async Task<ActionResult<ExpenseReimburseRequest>> PostExpenseReimburseRequest(List<ExpenseReimburseRequestDTO> listExpenseReimburseRequestDto)
        {
            

            if (listExpenseReimburseRequestDto.Count == 1)
            {
                ExpenseReimburseRequest expenseReimburseRequest = null;

                ExpenseReimburseRequestDTO expenseReimburseRequestDto = listExpenseReimburseRequestDto[0];

                StringBuilder StrBuilderUploadedDocuments = new StringBuilder();

                if (ModelState.IsValid)
                {
                    string uniqueFileName = null;

                    uniqueFileName = await SaveFileToFolderAndGetName(expenseReimburseRequestDto);

                    expenseReimburseRequest = new ExpenseReimburseRequest()
                    {
                        EmployeeId = expenseReimburseRequestDto.EmployeeId,
                        ExpenseReimbClaimAmount = expenseReimburseRequestDto.ExpenseReimbClaimAmount,
                        Documents = StrBuilderUploadedDocuments.ToString(),
                        ExpReimReqDate = expenseReimburseRequestDto.ExpReimReqDate
                        //ExpenseTypeId = expenseReimburseRequestDto.ExpenseTypeId
                    };

                    _context.ExpenseReimburseRequests.Add(expenseReimburseRequest);
                    await _context.SaveChangesAsync();

                }

                //##### 3. Adding a entry in DisbursementsAndClaimsMaster table for records
                //If it is department based Expense reimbursement claim
                if (expenseReimburseRequestDto.ProjectId == null)
                {

                    _context.DisbursementsAndClaimsMasters.Add(new DisbursementsAndClaimsMaster()
                    {
                        EmployeeId = expenseReimburseRequestDto.EmployeeId,
                        PettyCashRequestId = null,
                        ExpenseReimburseReqId = expenseReimburseRequest.Id,

                        AdvanceOrReimburseId = (int)AdvanceOrReimburse.ExpenseReim,
                        ProjectId = null,
                        SubProjectId = null,
                        WorkTaskId = null,

                        RecordDate = DateTime.Now,
                        Amount = expenseReimburseRequest.ExpenseReimbClaimAmount,
                        CostCentreId = _context.Departments.Find(_context.Employees.Find(expenseReimburseRequestDto.EmployeeId).DepartmentId).CostCentreId,
                        ApprovalStatusId = (int)ApprovalStatus.Pending
                    });
                    await _context.SaveChangesAsync();
                }
                else //If it is Project based Expense reimbursement claim
                {
                    _context.DisbursementsAndClaimsMasters.Add(new DisbursementsAndClaimsMaster()
                    {
                        EmployeeId = expenseReimburseRequestDto.EmployeeId,
                        PettyCashRequestId = null,
                        ExpenseReimburseReqId = expenseReimburseRequest.Id,
                        AdvanceOrReimburseId = (int)AdvanceOrReimburse.ExpenseReim,
                        ProjectId = expenseReimburseRequest.ProjectId,
                        SubProjectId = expenseReimburseRequest.SubProjectId,
                        WorkTaskId = expenseReimburseRequest.WorkTaskId,
                        RecordDate = DateTime.Now,
                        Amount = expenseReimburseRequest.ExpenseReimbClaimAmount,
                        CostCentreId = _context.Projects.Find(expenseReimburseRequestDto.ProjectId).CostCentreId,
                        ApprovalStatusId = (int)ApprovalStatus.Pending
                    });
                    await _context.SaveChangesAsync();
                }
                //##### 4. ClaimsApprovalTracker to be updated for all the allowed Approvers

                if (expenseReimburseRequestDto.ProjectId == null)
                {
                    await Task.Run(() => ProjectBasedExpReimRequest(expenseReimburseRequestDto, expenseReimburseRequest.Id));
                }
                else
                {
                    await Task.Run(() => DepartmentBasedExpReimRequest(expenseReimburseRequestDto, expenseReimburseRequest.Id));
                }


            }
            else
            {
                /// for multiple Expenseclaims at the same time
                /// 
                ///TODO lis 
            }



            return Ok();
        }

        private async Task<string> SaveFileToFolderAndGetName(ExpenseReimburseRequestDTO expenseReimburseRequestDto)
        {

            string uniqueFileName = string.Empty;
            StringBuilder StrBuilderUploadedDocuments = new StringBuilder();

            if (expenseReimburseRequestDto.Documents != null && expenseReimburseRequestDto.Documents.Count > 0)
            {
                foreach (IFormFile document in expenseReimburseRequestDto.Documents)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Images");

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + document.FileName;

                    StrBuilderUploadedDocuments.Append(uniqueFileName + "^");

                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);


                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await document.CopyToAsync(stream);
                        stream.Flush();
                    }
                }

            }

            return uniqueFileName;
        }

        // DELETE: api/ExpenseReimburseRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenseReimburseRequest(int id)
        {
            var expenseReimburseRequest = await _context.ExpenseReimburseRequests.FindAsync(id);
            if (expenseReimburseRequest == null)
            {
                return NotFound();
            }

            _context.ExpenseReimburseRequests.Remove(expenseReimburseRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpenseReimburseRequestExists(int id)
        {
            return _context.ExpenseReimburseRequests.Any(e => e.Id == id);
        }







        private enum AdvanceOrReimburse
        {
            CashAdvance = 1,
            ExpenseReim

        }

        private enum ApprovalStatus
        {
            Pending = 1,
            Approved,
            Rejected

        }




        private async Task DepartmentBasedExpReimRequest(ExpenseReimburseRequestDTO expenseReimburseRequestDto, int ExpReimReqId)
        {

            var getEmpClaimApproversAllLevels = _context.ApprovalRoleMaps.Where(a => a.ApprovalGroupId == expenseReimburseRequestDto.EmployeeId).ToList().OrderBy(a => a.ApprovalLevel);

            foreach (ApprovalRoleMap ApprMap in getEmpClaimApproversAllLevels)
            {

                int role_id = ApprMap.RoleId;
                var approver = _context.Employees.Where(e => e.RoleId == role_id).FirstOrDefault();

                _context.ClaimApprovalStatusTrackers.Add(new ClaimApprovalStatusTracker
                {
                    EmployeeId = expenseReimburseRequestDto.EmployeeId,
                    PettyCashRequestId = null,
                    ExpenseReimburseRequestId = ExpReimReqId,
                    DepartmentId = approver.DepartmentId,
                    ProjectId = null, //Approver Project Id
                    RoleId = approver.RoleId,
                    ReqDate = DateTime.Now,
                    FinalApprovedDate = null,
                    ApprovalStatusTypeId = (int)ApprovalStatus.Pending //1-Pending, 2-Approved, 3-Rejected
                });

                await _context.SaveChangesAsync();

                //##### 5. Send email to the Approver
                //####################################

                var approverMailAddress = approver.Email;
                string subject = "Expense Claim Approval Request " + expenseReimburseRequestDto.Id.ToString();
                Employee emp = _context.Employees.Find(expenseReimburseRequestDto.EmployeeId);
                var expenseReimClaimReq = _context.ExpenseReimburseRequests.Find(expenseReimburseRequestDto.Id);
                string content = "Expense Reimbursement request Approval sought by " + emp.FirstName + "/nfor the amount of " + expenseReimClaimReq.ExpenseReimbClaimAmount + "/ntowards " + expenseReimClaimReq.ExpenseType;
                var messagemail = new Message(new string[] { approverMailAddress }, subject, content);

               await _emailSender.SendEmailAsync(messagemail);

                //repeat for each approver
            }

           
        }


        private async Task ProjectBasedExpReimRequest(ExpenseReimburseRequestDTO expenseReimburseRequestDto, int ExpReimReqId)
        {

            int projManagerid = _context.ProjectManagements.Find(expenseReimburseRequestDto.ProjectId).EmployeeId;
            var approver = _context.Employees.Find(projManagerid);

            _context.ClaimApprovalStatusTrackers.Add(new ClaimApprovalStatusTracker
            {
                EmployeeId = expenseReimburseRequestDto.EmployeeId, //Employee Requester Id
                PettyCashRequestId = null,
                ExpenseReimburseRequestId = ExpReimReqId,
                DepartmentId = null, //Department of approver
                ProjectId = expenseReimburseRequestDto.ProjectId.Value, //Approver Project Id
                RoleId = approver.RoleId, // Approver Role Id
                ReqDate = DateTime.Now,
                FinalApprovedDate = null,
                ApprovalStatusTypeId = (int)ApprovalStatus.Pending //1-Pending, 2-Approved, 3-Rejected
            });

            //##### 5. Send email to the Approver
            //Single instance for Project
            //####################################

            var approverMailAddress = approver.Email;
            string subject = "Expense Claim Approval Request " + expenseReimburseRequestDto.Id.ToString();
            Employee emp = _context.Employees.Find(expenseReimburseRequestDto.EmployeeId);
            var expenseReimClaimReq = _context.ExpenseReimburseRequests.Find(expenseReimburseRequestDto.Id);
            string content = "Expense Reimbursement request Approval sought by " + emp.FirstName + "/nfor the amount of " + expenseReimClaimReq.ExpenseReimbClaimAmount + "/ntowards " + expenseReimClaimReq.ExpenseType;
            var messagemail = new Message(new string[] { approverMailAddress }, subject, content);

           await _emailSender.SendEmailAsync(messagemail);
        }



    }
}
