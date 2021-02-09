﻿using AtoCash.Authentication;
using AtoCash.Data;
using AtoCash.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
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

namespace AtoCash.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public ReportsController(AtoCashDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ActionName("CashReimburseReportByEmployee")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetCashAdvanceReportRequestByEmployee(CashAdvanceSearchModel searchModel)
        {
            int? empid = searchModel.EmpId;

            if (empid != null)
            {
                var emp = await _context.Employees.FindAsync(empid); //employee object
                string empFullName = emp.FirstName + emp.MiddleName + emp.LastName;

                //restrict employees to generate only their content not of other employees
                var result = _context.DisbursementsAndClaimsMasters.Where(x => x.EmployeeId == searchModel.EmpId).AsQueryable();

                if (searchModel != null)
                {
                    //For  string use the below
                    //if (!string.IsNullOrEmpty(searchModel.Name))
                    //    result = result.Where(x => x.Name.Contains(searchModel.Name));

                    if (searchModel.AdvanceOrReimburseId.HasValue)
                        result = result.Where(x => x.AdvanceOrReimburseId == searchModel.AdvanceOrReimburseId);
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
                        result = result.Where(x => x.Amount == searchModel.AmountFrom);
                    if (searchModel.AmountTo > 0)
                        result = result.Where(x => x.Amount == searchModel.AmountTo);
                    if (searchModel.CostCentreId.HasValue)
                        result = result.Where(x => x.CostCentreId == searchModel.CostCentreId);
                    if (searchModel.ApprovalStatusId.HasValue)
                        result = result.Where(x => x.ApprovalStatusId == searchModel.ApprovalStatusId);


                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[10]
                        {
                    new DataColumn("EmployeeName", typeof(string)),
                    new DataColumn("PettyCashRequestId", typeof(int)),
                    new DataColumn("AdvanceOrReimburse",typeof(string)),
                    new DataColumn("ProjectName",typeof(string)),
                    new DataColumn("SubProject", typeof(string)),
                    new DataColumn("WorkTask",typeof(string)),
                    new DataColumn("RecordDate",typeof(DateTime)),
                    new DataColumn("Amount", typeof(decimal)),
                    new DataColumn("CostCentre", typeof(string)),
                    new DataColumn("ApprovalStatus", typeof(string))
                        });

                    foreach (var empCashAdvance in result)
                    {
                        dt.Rows.Add(
                            empFullName,
                            empCashAdvance.PettyCashRequestId,
                            empCashAdvance.AdvanceOrReimburse.ClaimType,
                            empCashAdvance.Project.ProjectName,
                            empCashAdvance.SubProject.SubProjectName,
                            empCashAdvance.WorkTask.TaskName,
                            empCashAdvance.RecordDate,
                            empCashAdvance.Amount,
                            empCashAdvance.CostCentre.CostCentreCode,
                            empCashAdvance.ApprovalStatusType.Status
                            );
                    }
                    // Creating the Excel workbook 
                    // Add the datatable to the Excel workbook

                    return GetExcel("CashReimburseReportByEmployee", dt);
                }
            }
            return BadRequest(new RespStatus() { Status = "Failure", Message = "User Id not valid" });
        }


        [HttpPost]
        [ActionName("CashReimburseReportByAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCashAdvanceReportReqestByAdmin(CashAdvanceSearchModel searchModel)
        {
            int? empId = searchModel.EmpId;
            var emp = await _context.Employees.FindAsync(empId); //employee object
            string empFullName = emp.FirstName + emp.MiddleName + emp.LastName;

            //ADMIN MODE - Access all employee details
            var result = _context.DisbursementsAndClaimsMasters.AsQueryable();

            if (searchModel != null)
            {
                //For  string use the below
                //if (!string.IsNullOrEmpty(searchModel.Name))
                //    result = result.Where(x => x.Name.Contains(searchModel.Name));
                if (searchModel.EmpId.HasValue)
                    result = result.Where(x => x.EmployeeId == empId);
                if (searchModel.AdvanceOrReimburseId.HasValue)
                    result = result.Where(x => x.AdvanceOrReimburseId == searchModel.AdvanceOrReimburseId);
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
                    result = result.Where(x => x.Amount == searchModel.AmountFrom);
                if (searchModel.AmountTo > 0)
                    result = result.Where(x => x.Amount == searchModel.AmountTo);
                if (searchModel.CostCentreId.HasValue)
                    result = result.Where(x => x.CostCentreId == searchModel.CostCentreId);
                if (searchModel.ApprovalStatusId.HasValue)
                    result = result.Where(x => x.ApprovalStatusId == searchModel.ApprovalStatusId);

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[10]
                    {
                    new DataColumn("EmployeeName", typeof(string)),
                    new DataColumn("PettyCashRequestId", typeof(int)),
                    new DataColumn("AdvanceOrReimburse",typeof(string)),
                    new DataColumn("ProjectName",typeof(string)),
                    new DataColumn("SubProject", typeof(string)),
                    new DataColumn("WorkTask",typeof(string)),
                    new DataColumn("RecordDate",typeof(DateTime)),
                    new DataColumn("Amount", typeof(decimal)),
                    new DataColumn("CostCentre", typeof(string)),
                    new DataColumn("ApprovalStatus", typeof(string))
                    });

                foreach (var empCashAdvance in result)
                {
                    dt.Rows.Add(
                        empFullName,
                        empCashAdvance.PettyCashRequestId,
                        empCashAdvance.AdvanceOrReimburse.ClaimType,
                        empCashAdvance.Project.ProjectName,
                        empCashAdvance.SubProject.SubProjectName,
                        empCashAdvance.WorkTask.TaskName,
                        empCashAdvance.RecordDate,
                        empCashAdvance.Amount,
                        empCashAdvance.CostCentre.CostCentreCode,
                        empCashAdvance.ApprovalStatusType.Status
                        );
                }
                // Creating the Excel workbook 
                // Add the datatable to the Excel workbook

                return GetExcel("CashReimburseReportByAdmin", dt);
            }

            return BadRequest(new RespStatus() { Status = "Failure", Message = "Invalid Filter criteria" });

        }


        [HttpPost]
        [ActionName("TravelRequestReportByEmployee")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetTravelRequestReportByEmployee(TravelRequestSearchModel searchModel)
        {
            int? empid = searchModel.EmpId;

            if (empid != null)
            {
                var emp = await _context.Employees.FindAsync(empid); //employee object
                string empFullName = emp.FirstName + emp.MiddleName + emp.LastName;

                //restrict employees to generate only their content not of other employees
                var result = _context.TravelApprovalRequests.Where(x => x.EmployeeId == searchModel.EmpId).AsQueryable();

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
                if (searchModel.ReqRaisedDateFrom.HasValue)
                    result = result.Where(x => x.ReqRaisedDate >= searchModel.ReqRaisedDateFrom);
                if (searchModel.ReqRaisedDateTo.HasValue)
                    result = result.Where(x => x.ReqRaisedDate <= searchModel.ReqRaisedDateTo);
                if (!string.IsNullOrEmpty(searchModel.CurrentStatus))
                    result = result.Where(x => x.CurrentStatus == searchModel.CurrentStatus);

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[9]
                    {
                    new DataColumn("EmployeeName", typeof(string)),
                    new DataColumn("TravelApprovalRequestId", typeof(int)),
                    new DataColumn("TravelStartDate",typeof(DateTime)),
                    new DataColumn("TravelEndDate",typeof(DateTime)),
                    new DataColumn("TravelPurpose", typeof(string)),
                    new DataColumn("Department",typeof(string)),
                    new DataColumn("Project",typeof(string)),
                    new DataColumn("TravelRequestDate", typeof(DateTime)),
                    new DataColumn("CurrentStatus", typeof(string))

                    });

                foreach (var travelreq in result)
                {
                    dt.Rows.Add(
                        empFullName,
                        travelreq.Id,
                       travelreq.TravelStartDate,
                       travelreq.TravelEndDate,
                       travelreq.TravelPurpose,
                       travelreq.Department.DeptName,
                       travelreq.Project.ProjectName,
                       travelreq.ReqRaisedDate,
                       travelreq.CurrentStatus
                        );
                }
                // Creating the Excel workbook 
                // Add the datatable to the Excel workbook

                return GetExcel("TravelRequestReportByEmployee", dt);
            }

            return BadRequest(new RespStatus() { Status = "Failure", Message = "Invalid Filter criteria" });

        }


        [HttpPost]
        [ActionName("TravelRequestReportByAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTravelRequestReportByAdmin(TravelRequestSearchModel searchModel)
        {
            int? empId = searchModel.EmpId;
            var emp = await _context.Employees.FindAsync(empId); //employee object
            string empFullName = emp.FirstName + emp.MiddleName + emp.LastName;

            //ADMIN MODE - Access all employee details
            var result = _context.TravelApprovalRequests.AsQueryable();

            if (searchModel != null)
            {
                //For  string use the below
                //if (!string.IsNullOrEmpty(searchModel.Name))
                //    result = result.Where(x => x.Name.Contains(searchModel.Name));
                if (searchModel.EmpId.HasValue)
                    result = result.Where(x => x.EmployeeId == empId);
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
                if (searchModel.ReqRaisedDateFrom.HasValue)
                    result = result.Where(x => x.ReqRaisedDate >= searchModel.ReqRaisedDateFrom);
                if (searchModel.ReqRaisedDateTo.HasValue)
                    result = result.Where(x => x.ReqRaisedDate <= searchModel.ReqRaisedDateTo);
                if (!string.IsNullOrEmpty(searchModel.CurrentStatus))
                    result = result.Where(x => x.CurrentStatus == searchModel.CurrentStatus);

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[9]
                    {
                    new DataColumn("EmployeeName", typeof(string)),
                    new DataColumn("TravelApprovalRequestId", typeof(int)),
                    new DataColumn("TravelStartDate",typeof(DateTime)),
                    new DataColumn("TravelEndDate",typeof(DateTime)),
                    new DataColumn("TravelPurpose", typeof(string)),
                    new DataColumn("Department",typeof(string)),
                    new DataColumn("Project",typeof(string)),
                    new DataColumn("TravelRequestDate", typeof(DateTime)),
                    new DataColumn("CurrentStatus", typeof(string))

                    });

                foreach (var travelreq in result)
                {
                    dt.Rows.Add(
                        empFullName,
                        travelreq.Id,
                       travelreq.TravelStartDate,
                       travelreq.TravelEndDate,
                       travelreq.TravelPurpose,
                       travelreq.Department.DeptName,
                       travelreq.Project.ProjectName,
                       travelreq.ReqRaisedDate,
                       travelreq.CurrentStatus
                        );
                }
                // Creating the Excel workbook 
                // Add the datatable to the Excel workbook

                return GetExcel("TravelRequestReportByAdmin", dt);
            }

            return BadRequest(new RespStatus() { Status = "Failure", Message = "Invalid Filter criteria" });
        }



        private FileContentResult GetExcel(string reporttype, DataTable dt)
        {
            // Creating the Excel workbook 
            // Add the datatable to the Excel workbook
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, reporttype);
                string xlfileName = reporttype + "_" + DateTime.Now.ToShortDateString().Replace("/", string.Empty) + ".xlsx";

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "Application/Ms-Excel", xlfileName);
                }
            }
        }


        ///End of methods
    }
}
