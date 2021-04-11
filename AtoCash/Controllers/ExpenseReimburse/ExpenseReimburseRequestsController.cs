﻿using System;
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
using Microsoft.AspNetCore.Authorization;
using AtoCash.Authentication;
using System.Net.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace AtoCash.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Finmgr, Admin, User")]
    public class ExpenseReimburseRequestsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;
        //private readonly IWebHostEnvironment hostingEnvironment;
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

            var expenseReimburseRequests = await _context.ExpenseReimburseRequests.ToListAsync();


            List<ExpenseReimburseRequestDTO> ListExpenseReimburseRequestDTO = new();
            foreach (ExpenseReimburseRequest expenseReimbRequest in expenseReimburseRequests)
            {
                ExpenseReimburseRequestDTO expenseReimburseRequestDTO = new()
                {
                    Id = expenseReimbRequest.Id,
                    EmployeeId = expenseReimbRequest.EmployeeId,
                    EmployeeName = _context.Employees.Find(expenseReimbRequest.EmployeeId).GetFullName(),
                    ExpenseReportTitle = expenseReimbRequest.ExpenseReportTitle,
                    CurrencyTypeId = expenseReimbRequest.CurrencyTypeId,
                    TotalClaimAmount = expenseReimbRequest.TotalClaimAmount,

                    DepartmentId = expenseReimbRequest.DepartmentId,
                    Department = expenseReimbRequest.DepartmentId != null ? _context.Departments.Find(expenseReimbRequest.DepartmentId).DeptName : null,

                    ProjectId = expenseReimbRequest.ProjectId,
                    Project = expenseReimbRequest.ProjectId != null ? _context.Projects.Find(expenseReimbRequest.ProjectId).ProjectName : null,

                    SubProjectId = expenseReimbRequest.SubProjectId,
                    SubProject = expenseReimbRequest.SubProjectId != null ? _context.SubProjects.Find(expenseReimbRequest.SubProjectId).SubProjectName : null,

                    WorkTaskId = expenseReimbRequest.WorkTaskId,
                    WorkTask = expenseReimbRequest.WorkTaskId != null ? _context.WorkTasks.Find(expenseReimbRequest.WorkTaskId).TaskName : null,

                    ExpReimReqDate = expenseReimbRequest.ExpReimReqDate,
                    ApprovedDate = expenseReimbRequest.ApprovedDate,
                    ApprovalStatusTypeId = expenseReimbRequest.ApprovalStatusTypeId,
                    ApprovalStatusType = _context.ApprovalStatusTypes.Find(expenseReimbRequest.ApprovalStatusTypeId).Status,
                };
                ListExpenseReimburseRequestDTO.Add(expenseReimburseRequestDTO);

            }

            return ListExpenseReimburseRequestDTO;
        }

        //GET: api/ExpenseReimburseRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseReimburseRequestDTO>> GetExpenseReimburseRequest(int id)
        {


            var expenseReimbRequest = await _context.ExpenseReimburseRequests.FindAsync(id);

            if (expenseReimbRequest == null)
            {
                return NoContent();
            }

            ExpenseReimburseRequestDTO expenseReimburseRequestDTO = new()
            {
                Id = expenseReimbRequest.Id,
                EmployeeId = expenseReimbRequest.EmployeeId,
                EmployeeName = _context.Employees.Find(expenseReimbRequest.EmployeeId).GetFullName(),
                ExpenseReportTitle = expenseReimbRequest.ExpenseReportTitle,
                CurrencyTypeId = expenseReimbRequest.CurrencyTypeId,
                TotalClaimAmount = expenseReimbRequest.TotalClaimAmount,

                DepartmentId = expenseReimbRequest.DepartmentId,
                Department = expenseReimbRequest.DepartmentId != null ? _context.Departments.Find(expenseReimbRequest.DepartmentId).DeptName : null,

                ProjectId = expenseReimbRequest.ProjectId,
                Project = expenseReimbRequest.ProjectId != null ? _context.Projects.Find(expenseReimbRequest.ProjectId).ProjectName : null,

                SubProjectId = expenseReimbRequest.SubProjectId,
                SubProject = expenseReimbRequest.SubProjectId != null ? _context.SubProjects.Find(expenseReimbRequest.SubProjectId).SubProjectName : null,

                WorkTaskId = expenseReimbRequest.WorkTaskId,
                WorkTask = expenseReimbRequest.WorkTaskId != null ? _context.WorkTasks.Find(expenseReimbRequest.WorkTaskId).TaskName : null,

                ExpReimReqDate = expenseReimbRequest.ExpReimReqDate,
                ApprovedDate = expenseReimbRequest.ApprovedDate,
                ApprovalStatusTypeId = expenseReimbRequest.ApprovalStatusTypeId,
                ApprovalStatusType = _context.ApprovalStatusTypes.Find(expenseReimbRequest.ApprovalStatusTypeId).Status,
            };
            return expenseReimburseRequestDTO;
        }



        [HttpGet("{id}")]
        [ActionName("GetExpenseReimburseRequestRaisedForEmployee")]
        public async Task<ActionResult<IEnumerable<ExpenseReimburseRequestDTO>>> GetExpenseReimburseRequestRaisedForEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NoContent();
            }

            //get the employee's approval level for comparison with approver level  to decide "ShowEditDelete" bool
            int reqEmpApprLevelId = _context.ApprovalRoleMaps.Where(a => a.RoleId == _context.Employees.Find(id).RoleId).FirstOrDefault().ApprovalLevelId;
            int reqEmpApprLevel = _context.ApprovalLevels.Find(reqEmpApprLevelId).Level;

            var expenseReimbRequests = await _context.ExpenseReimburseRequests.Where(p => p.EmployeeId == id).ToListAsync();

            if (expenseReimbRequests == null)
            {
                return NoContent();
            }

            List<ExpenseReimburseRequestDTO> ListExpenseReimburseRequestDTO = new();
            foreach (ExpenseReimburseRequest expenseReimbRequest in expenseReimbRequests)
            {
                ExpenseReimburseRequestDTO expenseReimburseRequestDTO = new();

                expenseReimburseRequestDTO.Id = expenseReimbRequest.Id;
                expenseReimburseRequestDTO.EmployeeId = expenseReimbRequest.EmployeeId;
                expenseReimburseRequestDTO.EmployeeName = _context.Employees.Find(expenseReimbRequest.EmployeeId).GetFullName();
                expenseReimburseRequestDTO.ExpenseReportTitle = expenseReimbRequest.ExpenseReportTitle;
                expenseReimburseRequestDTO.CurrencyTypeId = expenseReimbRequest.CurrencyTypeId;
                expenseReimburseRequestDTO.TotalClaimAmount = expenseReimbRequest.TotalClaimAmount;

                expenseReimburseRequestDTO.DepartmentId = expenseReimbRequest.DepartmentId;
                expenseReimburseRequestDTO.Department = expenseReimbRequest.DepartmentId != null ? _context.Departments.Find(expenseReimbRequest.DepartmentId).DeptName : null;

                expenseReimburseRequestDTO.ProjectId = expenseReimbRequest.ProjectId;
                expenseReimburseRequestDTO.Project = expenseReimbRequest.ProjectId != null ? _context.Projects.Find(expenseReimbRequest.ProjectId).ProjectName : null;

                expenseReimburseRequestDTO.SubProjectId = expenseReimbRequest.SubProjectId;
                expenseReimburseRequestDTO.SubProject = expenseReimbRequest.SubProjectId != null ? _context.SubProjects.Find(expenseReimbRequest.SubProjectId).SubProjectName : null;

                expenseReimburseRequestDTO.WorkTaskId = expenseReimbRequest.WorkTaskId;
                expenseReimburseRequestDTO.WorkTask = expenseReimbRequest.WorkTaskId != null ? _context.WorkTasks.Find(expenseReimbRequest.WorkTaskId).TaskName : null;

                expenseReimburseRequestDTO.ExpReimReqDate = expenseReimbRequest.ExpReimReqDate;
                expenseReimburseRequestDTO.ApprovedDate = expenseReimbRequest.ApprovedDate;
                expenseReimburseRequestDTO.ApprovalStatusTypeId = expenseReimbRequest.ApprovalStatusTypeId;
                expenseReimburseRequestDTO.ApprovalStatusType = _context.ApprovalStatusTypes.Find(expenseReimbRequest.ApprovalStatusTypeId).Status;


                int NextApproverInPending = _context.ExpenseReimburseStatusTrackers.Where(t =>
                      t.ApprovalStatusTypeId == (int)EApprovalStatus.Pending &&
                      t.ExpenseReimburseRequestId == expenseReimbRequest.Id).Select(s => s.ApprovalLevel.Level).FirstOrDefault();

                //set the bookean flat to TRUE if No approver has yet approved the Request else FALSE
                expenseReimburseRequestDTO.ShowEditDelete = reqEmpApprLevel + 1 == NextApproverInPending ? true : false;

                ListExpenseReimburseRequestDTO.Add(expenseReimburseRequestDTO);

            }


            return Ok(ListExpenseReimburseRequestDTO);
        }



        [HttpGet("{id}")]
        [ActionName("CountExpsenseReiburseRequestRaisedByEmployee")]
        public async Task<ActionResult<int>> CountExpsenseReiburseRequestRaisedByEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NoContent();
            }

            var expenseReimbRequests = await _context.ExpenseReimburseRequests.Where(p => p.EmployeeId == id).ToListAsync();

            if (expenseReimbRequests == null)
            {
                return NoContent();
            }

            return Ok(expenseReimbRequests.Count);

        }



        // PUT: api/ExpenseReimburseRequests/5
        [HttpPut]
        //[Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<IActionResult> PutExpenseReimburseRequest(List<ExpenseReimburseRequestDTO> ListExpenseReimbRequestDTO)
        {
            if (ListExpenseReimbRequestDTO.Count == 0)
            {
                return Ok(new RespStatus { Status = "Failure", Message = "No" });
            }

            foreach (ExpenseReimburseRequestDTO expenseReimbRequestDTO in ListExpenseReimbRequestDTO)
            {
                var expenseReimbRequest = await _context.ExpenseReimburseRequests.FindAsync(expenseReimbRequestDTO.Id);

                expenseReimbRequest.Id = expenseReimbRequestDTO.Id;
                expenseReimbRequest.EmployeeId = expenseReimbRequestDTO.EmployeeId;
                expenseReimbRequest.ExpenseReportTitle = expenseReimbRequestDTO.ExpenseReportTitle;
                expenseReimbRequest.CurrencyTypeId = expenseReimbRequestDTO.CurrencyTypeId;
                expenseReimbRequest.TotalClaimAmount = expenseReimbRequestDTO.TotalClaimAmount;

                expenseReimbRequest.DepartmentId = expenseReimbRequestDTO.DepartmentId;
                expenseReimbRequest.ProjectId = expenseReimbRequestDTO.ProjectId;

                expenseReimbRequest.SubProjectId = expenseReimbRequestDTO.SubProjectId;

                expenseReimbRequest.WorkTaskId = expenseReimbRequestDTO.WorkTaskId;

                expenseReimbRequest.ExpReimReqDate = expenseReimbRequestDTO.ExpReimReqDate;
                expenseReimbRequest.ApprovedDate = expenseReimbRequestDTO.ApprovedDate;
                expenseReimbRequest.ApprovalStatusTypeId = expenseReimbRequestDTO.ApprovalStatusTypeId;

                await Task.Run(() => _context.ExpenseReimburseRequests.Update(expenseReimbRequest));

            }
            //_context.Entry(expenseReimburseRequestDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/ExpenseReimburseRequests

        [HttpPost]
        [ActionName("PostDocuments")]
        public async Task<ActionResult<List<FileDocumentDTO>>> PostFiles([FromForm] IFormFileCollection Documents)
        {
            string uniqueFileName = string.Empty;
            //StringBuilder StrBuilderUploadedDocuments = new();

            List<FileDocumentDTO> fileDocumentDTOs = new();

            foreach (IFormFile document in Documents)
            {
                //Store the file to the contentrootpath/images =>
                //for docker it is /app/Images configured with volume mount in docker-compose

                string uploadsFolder = Path.Combine(hostingEnvironment.ContentRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + document.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await document.CopyToAsync(stream);
                stream.Flush();


                // Save it to the acutal FileDocuments table
                FileDocument fileDocument = new();
                fileDocument.ActualFileName = document.FileName;
                fileDocument.UniqueFileName = uniqueFileName;
                _context.FileDocuments.Add(fileDocument);
                await _context.SaveChangesAsync();
                //

                // Populating the List of Document Id for FrontEnd consumption
                FileDocumentDTO fileDocumentDTO = new();
                fileDocumentDTO.Id = fileDocument.Id;
                fileDocumentDTO.ActualFileName = document.FileName;
                fileDocumentDTOs.Add(fileDocumentDTO);
                //

                //StrBuilderUploadedDocuments.Append(uniqueFileName + "^");


            }

            return Ok(fileDocumentDTOs);
        }


        [HttpGet("{id}")]
        [ActionName("GetDocumentsBySubClaimsId")]
        //<List<FileContentResult>
        public async Task<ActionResult> GetDocumentsBySubClaimsId(int id)
        {
            List<string> documentIds = _context.ExpenseSubClaims.Find(id).DocumentIDs.Split(",").ToList();
            string documentsFolder = Path.Combine(hostingEnvironment.ContentRootPath, "Images");
            //var content = new MultipartContent();

            List<FileContentResult> ListOfDocuments = new();
            var provider = new FileExtensionContentTypeProvider();

            foreach (string doc in documentIds)
            {
                var fd = _context.FileDocuments.Find(id);
                string uniqueFileName = fd.UniqueFileName;
                string actualFileName = fd.ActualFileName;

                string filePath = Path.Combine(documentsFolder, uniqueFileName);
                var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
                if (!provider.TryGetContentType(filePath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                FileContentResult thisfile = File(bytes, contentType, Path.GetFileName(filePath));

                ListOfDocuments.Add(thisfile);
            }
            return Ok(ListOfDocuments);
        }


        [HttpGet("{id}")]
        [ActionName("GetDocumentByDocId")]
        public async Task<ActionResult> GetDocumentByDocId(int id)
        {
            string documentsFolder = Path.Combine(hostingEnvironment.ContentRootPath, "Images");
            //var content = new MultipartContent();

            var provider = new FileExtensionContentTypeProvider();

            var fd = _context.FileDocuments.Find(id);
            string uniqueFileName = fd.UniqueFileName;
            string actualFileName = fd.ActualFileName;

            string filePath = Path.Combine(documentsFolder, uniqueFileName);
            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            FileContentResult thisfile = File(bytes, contentType, Path.GetFileName(filePath));

            return Ok(thisfile);
        }





        [HttpPost]
        public async Task<ActionResult> PostExpenseReimburseRequest(ExpenseReimburseRequestDTO expenseReimburseRequestDto)
        {

            if (expenseReimburseRequestDto == null)
            {
                return NoContent();
            }

            if (expenseReimburseRequestDto.ProjectId != null)
            {
                //Goes to Option 1 (Project)
                await Task.Run(() => ProjectBasedExpReimRequest(expenseReimburseRequestDto));
            }
            else
            {
                //Goes to Option 2 (Department)
                await Task.Run(() => DepartmentBasedExpReimRequest(expenseReimburseRequestDto));
            }

            return Ok(new RespStatus { Status = "Success", Message = "Expense Reimburse Request Created!" });
        }


        // DELETE: api/ExpenseReimburseRequests/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "AtominosAdmin, Finmgr, Admin")]
        public async Task<IActionResult> DeleteExpenseReimburseRequest(int id)
        {
            var expenseReimburseRequest = await _context.ExpenseReimburseRequests.FindAsync(id);
            if (expenseReimburseRequest == null)
            {
                return NoContent();
            }

            _context.ExpenseReimburseRequests.Remove(expenseReimburseRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        /// <summary>
        /// Department based Expreimburse request
        /// </summary>
        /// <param name="expenseReimburseRequestDto"></param>
        /// <returns></returns>

        private async Task<IActionResult> DepartmentBasedExpReimRequest(ExpenseReimburseRequestDTO expenseReimburseRequestDto)
        {

            #region
            int reqEmpid = expenseReimburseRequestDto.EmployeeId;
            int empApprGroupId = _context.Employees.Find(reqEmpid).ApprovalGroupId;
            ////

            ExpenseReimburseRequest expenseReimburseRequest = new();
            double dblTotalClaimAmount = 0;

            expenseReimburseRequest.ExpenseReportTitle = expenseReimburseRequestDto.ExpenseReportTitle;
            expenseReimburseRequest.EmployeeId = expenseReimburseRequestDto.EmployeeId;
            expenseReimburseRequest.CurrencyTypeId = expenseReimburseRequestDto.CurrencyTypeId;
            expenseReimburseRequest.TotalClaimAmount = dblTotalClaimAmount; //Currently Zero but added as per the request
            expenseReimburseRequest.ExpReimReqDate = DateTime.Now;
            expenseReimburseRequest.DepartmentId = _context.Employees.Find(expenseReimburseRequestDto.EmployeeId).DepartmentId;
            //expenseReimburseRequest.ProjectId = expenseReimburseRequestDto.ProjectId;
            //expenseReimburseRequest.SubProjectId = expenseReimburseRequestDto.SubProjectId;
            //expenseReimburseRequest.WorkTaskId = expenseReimburseRequestDto.WorkTaskId;
            expenseReimburseRequest.ApprovalStatusTypeId = (int)EApprovalStatus.Pending;
            //expenseReimburseRequest.ApprovedDate = expenseReimburseRequestDto.ApprovedDate;

            _context.ExpenseReimburseRequests.Add(expenseReimburseRequest); //  <= this generated the Id
            await _context.SaveChangesAsync();

            //
            foreach (ExpenseSubClaimDTO expenseSubClaimDto in expenseReimburseRequestDto.ExpenseSubClaims)
            {
                ExpenseSubClaim expenseSubClaim = new();

                //get expensereimburserequestId from the saved record and then use here for sub-claims
                expenseSubClaim.ExpenseReimburseRequestId = expenseReimburseRequest.Id;
                expenseSubClaim.ExpenseTypeId = expenseSubClaimDto.ExpenseTypeId;
                expenseSubClaim.ExpenseReimbClaimAmount = expenseSubClaimDto.ExpenseReimbClaimAmount;
                expenseSubClaim.DocumentIDs = expenseSubClaimDto.DocumentIDs;
                expenseSubClaim.InvoiceNo = expenseSubClaimDto.InvoiceNo;
                expenseSubClaim.InvoiceDate = expenseSubClaimDto.InvoiceDate;
                expenseSubClaim.Tax = expenseSubClaimDto.Tax;
                expenseSubClaim.TaxAmount = expenseSubClaimDto.TaxAmount;
                expenseSubClaim.Vendor = expenseSubClaimDto.Vendor;
                expenseSubClaim.Location = expenseSubClaimDto.Location;
                expenseSubClaim.Description = expenseSubClaimDto.Description;

                _context.ExpenseSubClaims.Add(expenseSubClaim);
                await _context.SaveChangesAsync();
                dblTotalClaimAmount = dblTotalClaimAmount + expenseSubClaimDto.ExpenseReimbClaimAmount;

            }

            ExpenseReimburseRequest exp = _context.ExpenseReimburseRequests.Find(expenseReimburseRequest.Id);

            exp.TotalClaimAmount = dblTotalClaimAmount;
            _context.ExpenseReimburseRequests.Update(exp);
            await _context.SaveChangesAsync();




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
            var getEmpClaimApproversAllLevels = _context.ApprovalRoleMaps.Where(a => a.ApprovalGroupId == expenseReimburseRequestDto.EmployeeId).ToList().OrderBy(a => a.ApprovalLevel);
            var ReqEmpRoleId = _context.Employees.Where(e => e.Id == reqEmpid).FirstOrDefault().RoleId;
            var ReqEmpHisOwnApprLevel = _context.ApprovalRoleMaps.Where(a => a.RoleId == ReqEmpRoleId);
            bool isFirstApprover = true;

            if (isSelfApprovedRequest)
            {
                ExpenseReimburseStatusTracker expenseReimburseStatusTracker = new()
                {
                    EmployeeId = expenseReimburseRequestDto.EmployeeId,
                    ExpenseReimburseRequestId = expenseReimburseRequest.Id,
                    CurrencyTypeId = expenseReimburseRequestDto.CurrencyTypeId,
                    TotalClaimAmount = expenseReimburseRequestDto.TotalClaimAmount,
                    ExpReimReqDate = DateTime.Now,
                    DepartmentId = _context.Employees.Find(expenseReimburseRequestDto.EmployeeId).DepartmentId,
                    ProjectId = null, //Approver Project Id
                    JobRoleId = _context.Employees.Find(expenseReimburseRequestDto.EmployeeId).RoleId,
                    ApprovalLevelId = empApprLevel,
                    ApprovedDate = null,
                    ApprovalStatusTypeId = (int)EApprovalStatus.Approved, //1-Pending, 2-Approved, 3-Rejected
                    Comments = "Self Approved Request"
                };
                _context.ExpenseReimburseStatusTrackers.Add(expenseReimburseStatusTracker);
                await _context.SaveChangesAsync();
            }
            else
            {
                foreach (ApprovalRoleMap ApprMap in getEmpClaimApproversAllLevels)
                {
                    int role_id = ApprMap.RoleId;
                    var approver = _context.Employees.Where(e => e.RoleId == role_id).FirstOrDefault();

                    if (ReqEmpRoleId >= approver.RoleId)
                    {
                        continue;
                    }


                    ExpenseReimburseStatusTracker expenseReimburseStatusTracker = new()
                    {
                        EmployeeId = expenseReimburseRequestDto.EmployeeId,
                        ExpenseReimburseRequestId = expenseReimburseRequest.Id,
                        CurrencyTypeId = expenseReimburseRequestDto.CurrencyTypeId,
                        TotalClaimAmount = expenseReimburseRequestDto.TotalClaimAmount,
                        ExpReimReqDate = DateTime.Now,
                        DepartmentId = _context.Employees.Find(expenseReimburseRequestDto.EmployeeId).DepartmentId,
                        ProjectId = null, //Approver Project Id
                        JobRoleId = approver.RoleId,
                        ApprovalLevelId = ApprMap.ApprovalLevelId,
                        ApprovedDate = null,
                        ApprovalStatusTypeId = isFirstApprover ? (int)EApprovalStatus.Pending : (int)EApprovalStatus.Initiating,
                        Comments = "Awaiting Approver Action"
                    };
                    _context.ExpenseReimburseStatusTrackers.Add(expenseReimburseStatusTracker);
                    await _context.SaveChangesAsync();

                    //##### 5. Send email to the Approver
                    //####################################

                    if (isFirstApprover)
                    {
                        var approverMailAddress = approver.Email;
                        string subject = "Expense Reimburse Request " + expenseReimburseRequest.ExpenseReportTitle + " - #" + expenseReimburseRequest.Id.ToString();
                        Employee emp = _context.Employees.Find(expenseReimburseRequestDto.EmployeeId);
                        string content = "Expense Reimbursement request Approval sought by " + emp.FirstName + "<br/>for the amount of " + expenseReimburseRequest.TotalClaimAmount + "<br/>towards " + expenseReimburseRequest.ExpenseReportTitle;
                        var messagemail = new Message(new string[] { approverMailAddress }, subject, content);

                        await _emailSender.SendEmailAsync(messagemail);
                    }
                    isFirstApprover = false;

                    //repeat for each approver
                }

            }

            //##### 5. Adding a entry in DisbursementsAndClaimsMaster table for records
            #region

            DisbursementsAndClaimsMaster disbursementsAndClaimsMaster = new();
            disbursementsAndClaimsMaster.EmployeeId = expenseReimburseRequestDto.EmployeeId;
            disbursementsAndClaimsMaster.ExpenseReimburseReqId = expenseReimburseRequest.Id;
            disbursementsAndClaimsMaster.RequestTypeId = (int)ERequestType.ExpenseReim;
            disbursementsAndClaimsMaster.DepartmentId = _context.Employees.Find(expenseReimburseRequestDto.EmployeeId).DepartmentId;
            disbursementsAndClaimsMaster.ProjectId = expenseReimburseRequestDto.ProjectId;
            disbursementsAndClaimsMaster.SubProjectId = expenseReimburseRequestDto.SubProjectId;
            disbursementsAndClaimsMaster.WorkTaskId = expenseReimburseRequestDto.WorkTaskId;
            disbursementsAndClaimsMaster.RecordDate = DateTime.Now;
            disbursementsAndClaimsMaster.CurrencyTypeId = expenseReimburseRequestDto.CurrencyTypeId;
            disbursementsAndClaimsMaster.Amount = dblTotalClaimAmount;
            disbursementsAndClaimsMaster.CostCentreId = _context.Departments.Find(_context.Employees.Find(expenseReimburseRequestDto.EmployeeId).DepartmentId).CostCentreId;
            disbursementsAndClaimsMaster.ApprovalStatusId = (int)EApprovalStatus.Pending; //1-Initiating; 2-Pending; 3-InReview; 4-Approved; 5-Rejected
            await _context.DisbursementsAndClaimsMasters.AddAsync(disbursementsAndClaimsMaster);
            await _context.SaveChangesAsync();
            #endregion

            return Ok(new RespStatus { Status = "Success", Message = "Expense Claim Submitted Successfully!" });


        }


        private async Task<IActionResult> ProjectBasedExpReimRequest(ExpenseReimburseRequestDTO expenseReimburseRequestDto)
        {

            //### 1. If Employee Eligible for Cash Claim enter a record and reduce the available amount for next claim
            #region
            int costCentre = _context.Projects.Find(expenseReimburseRequestDto.ProjectId).CostCentreId;

            int projManagerid = _context.Projects.Find(expenseReimburseRequestDto.ProjectId).ProjectManagerId;
            var approver = _context.Employees.Find(projManagerid);
            int empid = expenseReimburseRequestDto.EmployeeId;


            ExpenseReimburseRequest expenseReimburseRequest = new();
            double dblTotalClaimAmount = 0;

            expenseReimburseRequest.ExpenseReportTitle = expenseReimburseRequestDto.ExpenseReportTitle;
            expenseReimburseRequest.EmployeeId = expenseReimburseRequestDto.EmployeeId;
            expenseReimburseRequest.CurrencyTypeId = expenseReimburseRequestDto.CurrencyTypeId;
            expenseReimburseRequest.TotalClaimAmount = dblTotalClaimAmount; //Currently Zero but added as per the request
            expenseReimburseRequest.ExpReimReqDate = DateTime.Now;
            //expenseReimburseRequest.DepartmentId = _context.Employees.Find(expenseReimburseRequest.EmployeeId).DepartmentId;
            expenseReimburseRequest.ProjectId = expenseReimburseRequestDto.ProjectId;
            expenseReimburseRequest.SubProjectId = expenseReimburseRequestDto.SubProjectId;
            expenseReimburseRequest.WorkTaskId = expenseReimburseRequestDto.WorkTaskId;
            expenseReimburseRequest.ApprovalStatusTypeId = (int)EApprovalStatus.Pending;
            //expenseReimburseRequest.ApprovedDate = expenseReimburseRequestDto.ApprovedDate;

            _context.ExpenseReimburseRequests.Add(expenseReimburseRequest); //  <= this generated the Id
            await _context.SaveChangesAsync();

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
            ///
            var getEmpClaimApproversAllLevels = _context.ApprovalRoleMaps.Where(a => a.ApprovalGroupId == expenseReimburseRequestDto.EmployeeId).ToList().OrderBy(a => a.ApprovalLevel);

            if (isSelfApprovedRequest)
            {
                ExpenseReimburseStatusTracker expenseReimburseStatusTracker = new()
                {
                    EmployeeId = expenseReimburseRequestDto.EmployeeId,
                    ExpenseReimburseRequestId = expenseReimburseRequest.Id,
                    CurrencyTypeId = expenseReimburseRequestDto.CurrencyTypeId,
                    TotalClaimAmount = expenseReimburseRequestDto.TotalClaimAmount,
                    ExpReimReqDate = DateTime.Now,
                    DepartmentId = approver.DepartmentId,
                    ProjectId = null, //Approver Project Id
                    JobRoleId = approver.RoleId,
                    ApprovalLevelId = empApprLevel,
                    ApprovedDate = null,
                    ApprovalStatusTypeId = (int)EApprovalStatus.Approved, //1-Pending, 2-Approved, 3-Rejected
                    Comments = "Self Approved Request"
                };
                _context.ExpenseReimburseStatusTrackers.Add(expenseReimburseStatusTracker);
                await _context.SaveChangesAsync();
            }
            else
            {


                ExpenseReimburseStatusTracker expenseReimburseStatusTracker = new()
                {
                    EmployeeId = expenseReimburseRequestDto.EmployeeId,
                    ExpenseReimburseRequestId = expenseReimburseRequest.Id,
                    CurrencyTypeId = expenseReimburseRequestDto.CurrencyTypeId,
                    TotalClaimAmount = expenseReimburseRequestDto.TotalClaimAmount,
                    ExpReimReqDate = DateTime.Now,
                    DepartmentId = approver.DepartmentId,
                    ProjectId = null, //Approver Project Id
                    JobRoleId = approver.RoleId,
                    ApprovalLevelId = empApprLevel,
                    ApprovedDate = null,
                    ApprovalStatusTypeId = (int)EApprovalStatus.Pending, //1-Pending, 2-Approved, 3-Rejected
                    Comments = "Awaiting Approver Action"
                };

                await _context.SaveChangesAsync();

                //##### 5. Send email to the Approver
                //Single instance for Project
                //####################################

                var approverMailAddress = approver.Email;
                string subject = "Expense Reimburse Request " + expenseReimburseRequest.ExpenseReportTitle + " - #" + expenseReimburseRequest.Id.ToString();
                Employee emp = _context.Employees.Find(expenseReimburseRequestDto.EmployeeId);
                string content = "Expense Reimbursement request Approval sought by " + emp.FirstName + "<br/>for the amount of " + expenseReimburseRequest.TotalClaimAmount + "<br/>towards " + expenseReimburseRequest.ExpenseReportTitle;
                var messagemail = new Message(new string[] { approverMailAddress }, subject, content);

                await _emailSender.SendEmailAsync(messagemail);

                //repeat for each approver


            }
            #endregion

            //##### 5. Adding a entry in DisbursementsAndClaimsMaster table for records
            #region
            DisbursementsAndClaimsMaster disbursementsAndClaimsMaster = new();
            disbursementsAndClaimsMaster.EmployeeId = expenseReimburseRequestDto.EmployeeId;
            disbursementsAndClaimsMaster.ExpenseReimburseReqId = expenseReimburseRequestDto.Id;
            disbursementsAndClaimsMaster.RequestTypeId = (int)ERequestType.ExpenseReim;
            disbursementsAndClaimsMaster.DepartmentId = null;
            disbursementsAndClaimsMaster.ProjectId = expenseReimburseRequestDto.ProjectId;
            disbursementsAndClaimsMaster.SubProjectId = expenseReimburseRequestDto.SubProjectId;
            disbursementsAndClaimsMaster.WorkTaskId = expenseReimburseRequestDto.WorkTaskId;
            disbursementsAndClaimsMaster.RecordDate = DateTime.Now;
            disbursementsAndClaimsMaster.CurrencyTypeId = expenseReimburseRequestDto.CurrencyTypeId;
            disbursementsAndClaimsMaster.Amount = expenseReimburseRequestDto.TotalClaimAmount;
            disbursementsAndClaimsMaster.CostCentreId = _context.Departments.Find(_context.Employees.Find(expenseReimburseRequestDto.EmployeeId).DepartmentId).CostCentreId;
            disbursementsAndClaimsMaster.ApprovalStatusId = (int)EApprovalStatus.Pending; //1-Initiating; 2-Pending; 3-InReview; 4-Approved; 5-Rejected
            await _context.DisbursementsAndClaimsMasters.AddAsync(disbursementsAndClaimsMaster);
            await _context.SaveChangesAsync();
            #endregion

            return Ok(new RespStatus { Status = "Success", Message = "Expense Claim Submitted Successfully!" });

        }

        #endregion

        //
    }
}
