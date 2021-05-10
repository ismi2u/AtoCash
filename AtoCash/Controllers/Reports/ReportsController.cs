using AtoCash.Authentication;
using AtoCash.Data;
using AtoCash.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;

namespace AtoCash.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr, User")]
    public class ReportsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        private readonly IWebHostEnvironment hostingEnvironment;

        public ReportsController(AtoCashDbContext context, IWebHostEnvironment hostEnv)
        {
            //var user = System.Threading.Thread.CurrentPrincipal;
            //var TheUser = User.Identity.IsAuthenticated ? UserRepository.GetUser(user.Identity.Name) : null;
            _context = context;
            hostingEnvironment = hostEnv;
            //Get Logged in User's EmpId.
            //var   LoggedInEmpid = User.Identities.First().Claims.ToList().Where(x => x.Type == "EmployeeId").Select(c => c.Value);
        }



        //       TravelReqReportsForEmployee based on EmpId
        //   TravelReqReportsForAdmin No filters
        //TravelReqReportsForManager  Based on department of the manager
        //   TravelReqReportsForProjectManager Based on Projects

        /*get current logged -in user details.

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var test = currentUser.Identities.First().Claims.ToList();
            //var id = userManager.GetUserId(User);
            var tets = User.Identities.First().Claims.ToList().Select(x => x.Type == "EmployeeId");

            var skj = User.Identities.First().Claims.ToList().Where(x => x.Type == "EmployeeId");

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims; */

        //int empid = (int) User.Identities.First().Claims.ToList().Where(x => x.Type == "EmployeeId").Select(c => c.Value);

        [HttpPost]
        [ActionName("GetAdvanceAndReimburseReportsEmployeeJson")]

        public async Task<IActionResult> GetAdvanceAndReimburseReportsEmployeeJson(CashAdvanceSearchModel searchModel)
        {
            //if (!LoggedInEmpid == searchModel.EmpId)
            // {
            //     return Ok(new RespStatus() { Status = "Failure", Message = "Employee reports only!" });
            // }

            int? empid = searchModel.EmpId;

            if (empid != null)
            {
                string empFullName = _context.Employees.Find(empid).GetFullName(); //employee object

                //restrict employees to generate only their content not of other employees
                var result = _context.DisbursementsAndClaimsMasters.Where(x => x.EmployeeId == searchModel.EmpId).AsQueryable();

                if (searchModel != null)
                {
                    //For  string use the below
                    //if (!string.IsNullOrEmpty(searchModel.Name))
                    //    result = result.Where(x => x.Name.Contains(searchModel.Name));

                    if (searchModel.RequestTypeId.HasValue)
                        result = result.Where(x => x.RequestTypeId == searchModel.RequestTypeId);
                    if (searchModel.DepartmentId.HasValue)
                        result = result.Where(x => x.DepartmentId == searchModel.DepartmentId);
                    if (searchModel.ProjectId.HasValue)
                        result = result.Where(x => x.ProjectId == searchModel.ProjectId);
                    if (searchModel.SubProjectId.HasValue)
                        result = result.Where(x => x.SubProjectId == searchModel.SubProjectId);
                    if (searchModel.RecordDateFrom.HasValue)
                        result = result.Where(x => x.RecordDate >= searchModel.RecordDateFrom);
                    if (searchModel.RecordDateTo.HasValue)
                        result = result.Where(x => x.RecordDate <= searchModel.RecordDateTo);
                    if (searchModel.AmountFrom > 0)
                        result = result.Where(x => x.ClaimAmount >= searchModel.AmountFrom);
                    if (searchModel.AmountTo > 0)
                        result = result.Where(x => x.ClaimAmount <= searchModel.AmountTo);
                    if (searchModel.CostCenterId.HasValue)
                        result = result.Where(x => x.CostCenterId == searchModel.CostCenterId);
                    if (searchModel.ApprovalStatusId.HasValue)
                        result = result.Where(x => x.ApprovalStatusId == searchModel.ApprovalStatusId);

                    List<DisbursementsAndClaimsMasterDTO> ListDisbItemsDTO = new();

                    foreach (DisbursementsAndClaimsMaster disb in result)
                    {
                        DisbursementsAndClaimsMasterDTO disbursementsAndClaimsMasterDTO = new();
                        disbursementsAndClaimsMasterDTO.Id = disb.Id;
                        disbursementsAndClaimsMasterDTO.EmployeeId = disb.EmployeeId;
                        disbursementsAndClaimsMasterDTO.EmployeeName = _context.Employees.Find(disb.EmployeeId).GetFullName();
                        disbursementsAndClaimsMasterDTO.PettyCashRequestId = disb.PettyCashRequestId;
                        disbursementsAndClaimsMasterDTO.ExpenseReimburseReqId = disb.ExpenseReimburseReqId;
                        disbursementsAndClaimsMasterDTO.RequestTypeId = disb.RequestTypeId;
                        disbursementsAndClaimsMasterDTO.RequestType = _context.RequestTypes.Find(disb.RequestTypeId).RequestName;
                        disbursementsAndClaimsMasterDTO.DepartmentId = disb.DepartmentId;
                        disbursementsAndClaimsMasterDTO.Department = disb.DepartmentId != null ? _context.Departments.Find(disb.DepartmentId).DeptCode : null;
                        disbursementsAndClaimsMasterDTO.ProjectId = disb.ProjectId;
                        disbursementsAndClaimsMasterDTO.Project = disb.ProjectId != null ? _context.Projects.Find(disb.ProjectId).ProjectName : null;
                        disbursementsAndClaimsMasterDTO.SubProjectId = disb.SubProjectId;
                        disbursementsAndClaimsMasterDTO.SubProject = disb.SubProjectId != null ? _context.SubProjects.Find(disb.SubProjectId).SubProjectName : null;
                        disbursementsAndClaimsMasterDTO.WorkTaskId = disb.WorkTaskId;
                        disbursementsAndClaimsMasterDTO.WorkTask = disb.WorkTaskId != null ? _context.WorkTasks.Find(disb.WorkTaskId).TaskName : null;
                        disbursementsAndClaimsMasterDTO.CurrencyTypeId = disb.CurrencyTypeId;
                        disbursementsAndClaimsMasterDTO.CurrencyType = disb.CurrencyTypeId != null ? _context.CurrencyTypes.Find(disb.CurrencyTypeId).CurrencyCode : null;
                        disbursementsAndClaimsMasterDTO.ClaimAmount = disb.ClaimAmount;
                        disbursementsAndClaimsMasterDTO.AmountToWallet = disb.AmountToWallet ?? 0;
                        disbursementsAndClaimsMasterDTO.AmountToCredit = disb.AmountToCredit ?? 0;
                        disbursementsAndClaimsMasterDTO.CostCenterId = disb.CostCenterId;
                        disbursementsAndClaimsMasterDTO.CostCenter = disb.CostCenterId != null ? _context.CostCenters.Find(disb.CostCenterId).CostCenterCode : null;
                        disbursementsAndClaimsMasterDTO.ApprovalStatusId = disb.ApprovalStatusId;
                        disbursementsAndClaimsMasterDTO.ApprovalStatusType = disb.ApprovalStatusId != null ? _context.ApprovalStatusTypes.Find(disb.ApprovalStatusId).Status : null;
                        disbursementsAndClaimsMasterDTO.RecordDate = disb.RecordDate;
                        ListDisbItemsDTO.Add(disbursementsAndClaimsMasterDTO);
                    }

                    return Ok(ListDisbItemsDTO);
                }
            }
            return Conflict(new RespStatus() { Status = "Failure", Message = "User Id not valid" });
        }


        [HttpPost]
        [ActionName("GetAdvanceAndReimburseReportsEmployeeExcel")]

        public async Task<IActionResult> GetAdvanceAndReimburseReportsEmployeeExcel(CashAdvanceSearchModel searchModel)
        {
            //if (!LoggedInEmpid == searchModel.EmpId)
            // {
            //     return Ok(new RespStatus() { Status = "Failure", Message = "Employee reports only!" });
            // }

            int? empid = searchModel.EmpId;

            if (empid != null)
            {
                string empFullName = _context.Employees.Find(empid).GetFullName(); //employee object

                //restrict employees to generate only their content not of other employees
                var result = _context.DisbursementsAndClaimsMasters.Where(x => x.EmployeeId == searchModel.EmpId).AsQueryable();

                if (searchModel != null)
                {
                    //For  string use the below
                    //if (!string.IsNullOrEmpty(searchModel.Name))
                    //    result = result.Where(x => x.Name.Contains(searchModel.Name));

                    if (searchModel.RequestTypeId.HasValue)
                        result = result.Where(x => x.RequestTypeId == searchModel.RequestTypeId);
                    if (searchModel.DepartmentId.HasValue)
                        result = result.Where(x => x.DepartmentId == searchModel.DepartmentId);
                    if (searchModel.ProjectId.HasValue)
                        result = result.Where(x => x.ProjectId == searchModel.ProjectId);
                    if (searchModel.SubProjectId.HasValue)
                        result = result.Where(x => x.SubProjectId == searchModel.SubProjectId);
                    if (searchModel.RecordDateFrom.HasValue)
                        result = result.Where(x => x.RecordDate >= searchModel.RecordDateFrom);
                    if (searchModel.RecordDateTo.HasValue)
                        result = result.Where(x => x.RecordDate <= searchModel.RecordDateTo);
                    if (searchModel.AmountFrom > 0)
                        result = result.Where(x => x.ClaimAmount >= searchModel.AmountFrom);
                    if (searchModel.AmountTo > 0)
                        result = result.Where(x => x.ClaimAmount <= searchModel.AmountTo);
                    if (searchModel.CostCenterId.HasValue)
                        result = result.Where(x => x.CostCenterId == searchModel.CostCenterId);
                    if (searchModel.ApprovalStatusId.HasValue)
                        result = result.Where(x => x.ApprovalStatusId == searchModel.ApprovalStatusId);


                    List<DisbursementsAndClaimsMasterDTO> ListDisbItemsDTO = new();

                    foreach (DisbursementsAndClaimsMaster disb in result)
                    {
                        DisbursementsAndClaimsMasterDTO disbursementsAndClaimsMasterDTO = new();
                        disbursementsAndClaimsMasterDTO.Id = disb.Id;
                        disbursementsAndClaimsMasterDTO.EmployeeId = disb.EmployeeId;
                        disbursementsAndClaimsMasterDTO.EmployeeName = _context.Employees.Find(disb.EmployeeId).GetFullName();
                        disbursementsAndClaimsMasterDTO.PettyCashRequestId = disb.PettyCashRequestId;
                        disbursementsAndClaimsMasterDTO.ExpenseReimburseReqId = disb.ExpenseReimburseReqId;
                        disbursementsAndClaimsMasterDTO.RequestTypeId = disb.RequestTypeId;
                        disbursementsAndClaimsMasterDTO.RequestType = _context.RequestTypes.Find(disb.RequestTypeId).RequestName;
                        disbursementsAndClaimsMasterDTO.DepartmentId = disb.DepartmentId;
                        disbursementsAndClaimsMasterDTO.Department = disb.DepartmentId != null ? _context.Departments.Find(disb.DepartmentId).DeptCode : null;
                        disbursementsAndClaimsMasterDTO.ProjectId = disb.ProjectId;
                        disbursementsAndClaimsMasterDTO.Project = disb.ProjectId != null ? _context.Projects.Find(disb.ProjectId).ProjectName : null;
                        disbursementsAndClaimsMasterDTO.SubProjectId = disb.SubProjectId;
                        disbursementsAndClaimsMasterDTO.SubProject = disb.SubProjectId != null ? _context.SubProjects.Find(disb.SubProjectId).SubProjectName : null;
                        disbursementsAndClaimsMasterDTO.WorkTaskId = disb.WorkTaskId;
                        disbursementsAndClaimsMasterDTO.WorkTask = disb.WorkTaskId != null ? _context.WorkTasks.Find(disb.WorkTaskId).TaskName : null;
                        disbursementsAndClaimsMasterDTO.CurrencyTypeId = disb.CurrencyTypeId;
                        disbursementsAndClaimsMasterDTO.CurrencyType = disb.CurrencyTypeId != null ? _context.CurrencyTypes.Find(disb.CurrencyTypeId).CurrencyCode : null;
                        disbursementsAndClaimsMasterDTO.ClaimAmount = disb.ClaimAmount;
                        disbursementsAndClaimsMasterDTO.AmountToWallet = disb.AmountToWallet ?? 0;
                        disbursementsAndClaimsMasterDTO.AmountToCredit = disb.AmountToCredit ?? 0;
                        disbursementsAndClaimsMasterDTO.CostCenterId = disb.CostCenterId;
                        disbursementsAndClaimsMasterDTO.CostCenter = disb.CostCenterId != null ? _context.CostCenters.Find(disb.CostCenterId).CostCenterCode : null;
                        disbursementsAndClaimsMasterDTO.ApprovalStatusId = disb.ApprovalStatusId;
                        disbursementsAndClaimsMasterDTO.ApprovalStatusType = disb.ApprovalStatusId != null ? _context.ApprovalStatusTypes.Find(disb.ApprovalStatusId).Status : null;
                        disbursementsAndClaimsMasterDTO.RecordDate = disb.RecordDate;
                        ListDisbItemsDTO.Add(disbursementsAndClaimsMasterDTO);
                    }

                    //return Ok(ListDisbItemsDTO);


                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[15]
                        {
                    //new DataColumn("Id", typeof(int)),
                    new DataColumn("EmployeeName", typeof(string)),
                    new DataColumn("PettyCashRequestId", typeof(int)),
                    new DataColumn("ExpenseReimburseReqId", typeof(int)),
                    new DataColumn("RequestType",typeof(string)),
                    new DataColumn("Department",typeof(string)),
                    new DataColumn("Project",typeof(string)),
                    new DataColumn("SubProject", typeof(string)),
                    new DataColumn("WorkTask",typeof(string)),
                    new DataColumn("RecordDate",typeof(DateTime)),
                    new DataColumn("CurrencyType",typeof(string)),
                    new DataColumn("ClaimAmount", typeof(Double)),
                    new DataColumn("AmountToWallet", typeof(Double)),
                    new DataColumn("AmountToCredit", typeof(Double)),
                    new DataColumn("CostCenter", typeof(string)),
                    new DataColumn("ApprovalStatus", typeof(string))
                        });

                    foreach (var disbItem in ListDisbItemsDTO)
                    {
                        dt.Rows.Add(
                            disbItem.EmployeeName,
                            disbItem.PettyCashRequestId,
                            disbItem.ExpenseReimburseReqId,
                            disbItem.RequestType,
                            disbItem.Department,
                            disbItem.Project,
                            disbItem.SubProject,
                            disbItem.WorkTask,
                            disbItem.RecordDate,
                            disbItem.CurrencyType,
                            disbItem.ClaimAmount,
                            disbItem.AmountToWallet,
                            disbItem.AmountToCredit,
                            disbItem.CostCenter,
                            disbItem.ApprovalStatusType
                            );
                    }
                    // Creating the Excel workbook 
                    // Add the datatable to the Excel workbook

                    List<string> docUrls = new();
                    var docUrl = GetExcel("CashReimburseReportByEmployee", dt);

                    docUrls.Add(docUrl);

                    return Ok(docUrls);


                }
            }
            return Conflict(new RespStatus() { Status = "Failure", Message = "User Id not valid" });
        }



        [HttpPost]
        [ActionName("GetTravelRequestReportForEmployeeJson")]


        public async Task<IActionResult> GetTravelRequestReportForEmployeeJson(TravelRequestSearchModel searchModel)
        {
            //if (!LoggedInEmpid == searchModel.EmpId)
            //{
            //    return Ok(new RespStatus() { Status = "Failure", Message = "Employee reports only!" });
            //}

            int? empid = searchModel.EmployeeId;

            if (empid != null)
            {
                var emp = await _context.Employees.FindAsync(empid); //employee object
                string empFullName = emp.FirstName + emp.MiddleName + emp.LastName;

                //restrict employees to generate only their content not of other employees
                var result = _context.TravelApprovalRequests.Where(x => x.EmployeeId == empid).AsQueryable();

                if (searchModel.TravelApprovalRequestId.HasValue)
                    result = result.Where(x => x.Id == searchModel.TravelApprovalRequestId);
                if (searchModel.TravelStartDate.HasValue)
                    result = result.Where(x => x.TravelStartDate >= searchModel.TravelStartDate);
                if (searchModel.TravelEndDate.HasValue)
                    result = result.Where(x => x.TravelEndDate <= searchModel.TravelEndDate);
                if (!string.IsNullOrEmpty(searchModel.TravelPurpose))
                    result = result.Where(x => x.TravelPurpose.Contains(searchModel.TravelPurpose));
                if (searchModel.DepartmentId.HasValue)
                    result = result.Where(x => x.DepartmentId == searchModel.DepartmentId);
                if (searchModel.ProjectId.HasValue)
                    result = result.Where(x => x.ProjectId == searchModel.ProjectId);
                if (searchModel.TravelApprovalRequestId.HasValue)
                    result = result.Where(x => x.Id == searchModel.TravelApprovalRequestId);
                if (searchModel.ReqRaisedDate.HasValue)
                    result = result.Where(x => x.ReqRaisedDate >= searchModel.ReqRaisedDate);
                if (searchModel.ReqRaisedDate.HasValue)
                    result = result.Where(x => x.ReqRaisedDate <= searchModel.ReqRaisedDate);
                if (searchModel.ApprovalStatusTypeId.HasValue)
                    result = result.Where(x => x.ApprovalStatusTypeId == searchModel.ApprovalStatusTypeId);

                List<TravelApprovalRequestDTO> ListTravelItemsDTO = new();

                foreach (TravelApprovalRequest travel in result)
                {
                    TravelApprovalRequestDTO travelItemDTO = new();
                    travelItemDTO.Id = travel.Id;
                    travelItemDTO.EmployeeId = travel.EmployeeId;
                    travelItemDTO.EmployeeName = _context.Employees.Find(travel.EmployeeId).GetFullName();

                    travelItemDTO.DepartmentId = travel.DepartmentId;
                    travelItemDTO.Department = travel.DepartmentId != null ? _context.Departments.Find(travel.DepartmentId).DeptName : null;
                    travelItemDTO.ProjectId = travel.ProjectId;
                    travelItemDTO.Project = travel.ProjectId != null ? _context.Projects.Find(travel.ProjectId).ProjectName : null;
                    travelItemDTO.SubProjectId = travel.SubProjectId;
                    travelItemDTO.SubProject = travel.SubProjectId != null ? _context.SubProjects.Find(travel.SubProjectId).SubProjectName : null;
                    travelItemDTO.WorkTaskId = travel.WorkTaskId;
                    travelItemDTO.WorkTask = travel.WorkTaskId != null ? _context.WorkTasks.Find(travel.WorkTaskId).TaskName : null;
                    travelItemDTO.CostCenterId = travel.CostCenterId;
                    travelItemDTO.CostCenter = travel.CostCenterId != null ? _context.CostCenters.Find(travel.CostCenterId).CostCenterCode : null;
                    travelItemDTO.ApprovalStatusTypeId = travel.ApprovalStatusTypeId;
                    travelItemDTO.ApprovalStatusType = travel.ApprovalStatusTypeId != null ? _context.ApprovalStatusTypes.Find(travel.ApprovalStatusTypeId).Status : null;
                    travelItemDTO.ReqRaisedDate = travel.ReqRaisedDate;

                    ListTravelItemsDTO.Add(travelItemDTO);
                }


                return Ok(ListTravelItemsDTO);
            }

            return Conflict(new RespStatus() { Status = "Failure", Message = "Invalid Filter criteria" });

        }



        [HttpPost]
        [ActionName("GetTravelRequestReportForEmployeeExcel")]


        public async Task<IActionResult> GetTravelRequestReportForEmployeeExcel(TravelRequestSearchModel searchModel)
        {
            //if (!LoggedInEmpid == searchModel.EmpId)
            //{
            //    return Ok(new RespStatus() { Status = "Failure", Message = "Employee reports only!" });
            //}

            int? empid = searchModel.EmployeeId;

            if (empid != null)
            {
                var emp = await _context.Employees.FindAsync(empid); //employee object
                string empFullName = emp.FirstName + emp.MiddleName + emp.LastName;

                //restrict employees to generate only their content not of other employees
                var result = _context.TravelApprovalRequests.Where(x => x.EmployeeId == empid).AsQueryable();

                if (searchModel.TravelApprovalRequestId.HasValue)
                    result = result.Where(x => x.Id == searchModel.TravelApprovalRequestId);
                if (searchModel.TravelStartDate.HasValue)
                    result = result.Where(x => x.TravelStartDate >= searchModel.TravelStartDate);
                if (searchModel.TravelEndDate.HasValue)
                    result = result.Where(x => x.TravelEndDate <= searchModel.TravelEndDate);
                if (!string.IsNullOrEmpty(searchModel.TravelPurpose))
                    result = result.Where(x => x.TravelPurpose.Contains(searchModel.TravelPurpose));
                if (searchModel.DepartmentId.HasValue)
                    result = result.Where(x => x.DepartmentId == searchModel.DepartmentId);
                if (searchModel.ProjectId.HasValue)
                    result = result.Where(x => x.ProjectId == searchModel.ProjectId);
                if (searchModel.TravelApprovalRequestId.HasValue)
                    result = result.Where(x => x.Id == searchModel.TravelApprovalRequestId);
                if (searchModel.ReqRaisedDate.HasValue)
                    result = result.Where(x => x.ReqRaisedDate >= searchModel.ReqRaisedDate);
                if (searchModel.ReqRaisedDate.HasValue)
                    result = result.Where(x => x.ReqRaisedDate <= searchModel.ReqRaisedDate);
                if (searchModel.ApprovalStatusTypeId.HasValue)
                    result = result.Where(x => x.ApprovalStatusTypeId == searchModel.ApprovalStatusTypeId);

                List<TravelApprovalRequestDTO> ListTravelItemsDTO = new();

                foreach (TravelApprovalRequest travel in result)
                {
                    TravelApprovalRequestDTO travelItemDTO = new();
                    travelItemDTO.Id = travel.Id;
                    travelItemDTO.EmployeeId = travel.EmployeeId;
                    travelItemDTO.EmployeeName = _context.Employees.Find(travel.EmployeeId).GetFullName();

                    travelItemDTO.DepartmentId = travel.DepartmentId;
                    travelItemDTO.Department = travel.DepartmentId != null ? _context.Departments.Find(travel.DepartmentId).DeptName : null;
                    travelItemDTO.ProjectId = travel.ProjectId;
                    travelItemDTO.Project = travel.ProjectId != null ? _context.Projects.Find(travel.ProjectId).ProjectName : null;
                    travelItemDTO.SubProjectId = travel.SubProjectId;
                    travelItemDTO.SubProject = travel.SubProjectId != null ? _context.SubProjects.Find(travel.SubProjectId).SubProjectName : null;
                    travelItemDTO.WorkTaskId = travel.WorkTaskId;
                    travelItemDTO.WorkTask = travel.WorkTaskId != null ? _context.WorkTasks.Find(travel.WorkTaskId).TaskName : null;
                    travelItemDTO.CostCenterId = travel.CostCenterId;
                    travelItemDTO.CostCenter = travel.CostCenterId != null ? _context.CostCenters.Find(travel.CostCenterId).CostCenterCode : null;
                    travelItemDTO.ApprovalStatusTypeId = travel.ApprovalStatusTypeId;
                    travelItemDTO.ApprovalStatusType = travel.ApprovalStatusTypeId != null ? _context.ApprovalStatusTypes.Find(travel.ApprovalStatusTypeId).Status : null;
                    travelItemDTO.ReqRaisedDate = travel.ReqRaisedDate;
                    ListTravelItemsDTO.Add(travelItemDTO);
                }




                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[12]
                    {
                    new DataColumn("TravelRequestId", typeof(int)),
                    new DataColumn("EmployeeName", typeof(string)),
                    new DataColumn("TravelStartDate",typeof(string)),
                    new DataColumn("TravelEndDate",typeof(string)),
                    new DataColumn("TravelPurpose",typeof(string)),
                    new DataColumn("Department",typeof(string)),
                    new DataColumn("Project",typeof(string)),
                    new DataColumn("SubProject", typeof(string)),
                    new DataColumn("WorkTask",typeof(string)),
                    new DataColumn("ReqRaisedDate",typeof(DateTime)),
                    new DataColumn("CostCenter", typeof(string)),
                    new DataColumn("ApprovalStatus", typeof(string))
                    });

                foreach (var travelItem in ListTravelItemsDTO)
                {
                    dt.Rows.Add(
                    travelItem.Id,
                    travelItem.EmployeeName,
                    travelItem.TravelStartDate,
                    travelItem.TravelEndDate,
                    travelItem.TravelPurpose,
                    travelItem.Department,
                    travelItem.Project,
                    travelItem.SubProject,
                    travelItem.WorkTask,
                    travelItem.ReqRaisedDate,
                    travelItem.CostCenter,
                    travelItem.ApprovalStatusType
                        );
                }


                // Creating the Excel workbook 
                // Add the datatable to the Excel workbook

                List<string> docUrls = new();
                var docUrl = GetExcel("TravelRequestReportForEmployee", dt);

                docUrls.Add(docUrl);

                return Ok(docUrl);
            }

            return Conflict(new RespStatus() { Status = "Failure", Message = "Invalid Filter criteria" });

        }





        private string GetExcel(string reporttype, DataTable dt)
        {
            // Creating the Excel workbook 
            // Add the datatable to the Excel workbook
            using XLWorkbook wb = new XLWorkbook();
            wb.Worksheets.Add(dt, reporttype);
            // string xlfileName = reporttype + "_" + DateTime.Now.ToShortDateString().Replace("/", string.Empty) + ".xlsx";
            string xlfileName = reporttype + ".xlsx";

            using MemoryStream stream = new MemoryStream();

            wb.SaveAs(stream);

            string uploadsfolder = Path.Combine(hostingEnvironment.ContentRootPath, "images");

            string filepath = Path.Combine(uploadsfolder, xlfileName);

            if (System.IO.File.Exists(filepath))
                System.IO.File.Delete(filepath);


            using var outputtream = new FileStream(filepath, FileMode.Create);

            using (FileStream outputfilestream = new FileStream(filepath, FileMode.Create))
            {
                stream.CopyTo(outputtream);
            }
            string docurl = Directory.EnumerateFiles(uploadsfolder).Select(f => filepath).FirstOrDefault().ToString();

            return docurl;
            // return File(stream.ToArray(), "Application/Ms-Excel", xlfileName);
        }


        ///End of methods
    }
}
