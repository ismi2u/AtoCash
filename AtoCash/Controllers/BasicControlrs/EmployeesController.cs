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
using AtoCash.Authentication;
using System.Net.Mail;

namespace AtoCash.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr, Manager, User")]
    public class EmployeesController : ControllerBase
    {
        private readonly AtoCashDbContext _context;

        public EmployeesController(AtoCashDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ActionName("EmployeesForDropdown")]
        public async Task<ActionResult<IEnumerable<EmployeeVM>>> GetEmployeesForDropDown()
        {
            List<EmployeeVM> ListEmployeeVM = new List<EmployeeVM>();

            var employees = await _context.Employees.ToListAsync();
            foreach (Employee employee in employees)
            {
                EmployeeVM employeeVM = new EmployeeVM
                {
                    Id = employee.Id,
                    FullName = employee.EmpCode + " " + employee.FirstName + " " + employee.MiddleName + " " + employee.LastName
                };

                ListEmployeeVM.Add(employeeVM);
            }

            return ListEmployeeVM;

        }
        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees()
        {
            List<EmployeeDTO> ListEmployeeDTO = new List<EmployeeDTO>();

            var employees = await _context.Employees.ToListAsync();

            foreach (Employee employee in employees)
            {
                EmployeeDTO employeeDTO = new();

                employeeDTO.Id = employee.Id;
                employeeDTO.FirstName = employee.FirstName;
                employeeDTO.MiddleName = employee.MiddleName;
                employeeDTO.LastName = employee.LastName;
                employeeDTO.EmpCode = employee.EmpCode;
                employeeDTO.BankAccount = employee.BankAccount;
                employeeDTO.BankCardNo = employee.BankCardNo;
                employeeDTO.NationalID = employee.NationalID;
                employeeDTO.PassportNo = employee.PassportNo;
                employeeDTO.TaxNumber = employee.TaxNumber;
                employeeDTO.Nationality = employee.Nationality;
                employeeDTO.DOB = employee.DOB;
                employeeDTO.DOJ = employee.DOJ;
                employeeDTO.Gender = employee.Gender;
                employeeDTO.Email = employee.Email;
                employeeDTO.MobileNumber = employee.MobileNumber;
                employeeDTO.EmploymentTypeId = employee.EmploymentTypeId;
                employeeDTO.DepartmentId = employee.DepartmentId;
                employeeDTO.RoleId = employee.RoleId;
                employeeDTO.CurrencyTypeId = employee.CurrencyTypeId;
                employeeDTO.ApprovalGroupId = employee.ApprovalGroupId;
                employeeDTO.StatusTypeId = employee.StatusTypeId;


                ListEmployeeDTO.Add(employeeDTO);

            }

            return ListEmployeeDTO;
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            EmployeeDTO employeeDTO = new EmployeeDTO();

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NoContent();
            }

            employeeDTO.Id = employee.Id;
            employeeDTO.FirstName = employee.FirstName;
            employeeDTO.MiddleName = employee.MiddleName;
            employeeDTO.LastName = employee.LastName;
            employeeDTO.EmpCode = employee.EmpCode;
            employeeDTO.BankAccount = employee.BankAccount;
            employeeDTO.BankCardNo = employee.BankCardNo;
            employeeDTO.NationalID = employee.NationalID;
            employeeDTO.PassportNo = employee.PassportNo;
            employeeDTO.TaxNumber = employee.TaxNumber;
            employeeDTO.Nationality = employee.Nationality;
            employeeDTO.DOB = employee.DOB;
            employeeDTO.DOJ = employee.DOJ;
            employeeDTO.Gender = employee.Gender;
            employeeDTO.Email = employee.Email;
            employeeDTO.MobileNumber = employee.MobileNumber;
            employeeDTO.EmploymentTypeId = employee.EmploymentTypeId;
            employeeDTO.DepartmentId = employee.DepartmentId;
            employeeDTO.RoleId = employee.RoleId;
            employeeDTO.CurrencyTypeId = employee.CurrencyTypeId;
            employeeDTO.ApprovalGroupId = employee.ApprovalGroupId;
            employeeDTO.StatusTypeId = employee.StatusTypeId;


            return employeeDTO;
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeDTO employeeDto)
        {
            if (id != employeeDto.Id)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Id is invalid" });
            }

            var emp = _context.Employees.Find(employeeDto.Id);

            int _testempId = _context.Employees.Where(e => e.MobileNumber == employeeDto.MobileNumber || e.EmpCode == employeeDto.EmpCode || e.Email == employeeDto.Email).Select(x => x.Id).FirstOrDefault();

            if (employeeDto.Id != _testempId)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Unique EmpCode/Mobile/Email required" });
            }
            


            //var emplye = _context.Employees.Where(e => e.FirstName == employeeDto.FirstName && e.MiddleName == employeeDto.MiddleName && e.LastName == employeeDto.LastName).FirstOrDefault();
            //if (emplye != null)
            //{
            //    return Conflict(new RespStatus { Status = "Failure", Message = "Employee Already Exists" });
            //}

            var employee = await _context.Employees.FindAsync(id);

            employee.Id = employeeDto.Id;
            employee.FirstName = employeeDto.FirstName;
            employee.MiddleName = employeeDto.MiddleName;
            employee.LastName = employeeDto.LastName;
            employee.EmpCode = employeeDto.EmpCode;
            employee.BankAccount = employeeDto.BankAccount;
            employee.BankCardNo = employeeDto.BankCardNo;
            employee.NationalID = employeeDto.NationalID;
            employee.PassportNo = employeeDto.PassportNo;
            employee.TaxNumber = employeeDto.TaxNumber;
            employee.Nationality = employeeDto.Nationality;
            employee.DOB = employeeDto.DOB;
            employee.DOJ = employeeDto.DOJ;
            employee.Gender = employeeDto.Gender;

            MailAddress mailAdd = new MailAddress(employeeDto.Email);
            if ((mailAdd.Host.ToUpper() != "MAILINATOR.COM") && mailAdd.Host.ToUpper() != "GMAIL.COM")
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Use company mail address!" });
            }
            else
            {
                employee.Email = employeeDto.Email;
            }

            employee.Email = employeeDto.Email;
            employee.MobileNumber = employeeDto.MobileNumber;
            employee.EmploymentTypeId = employeeDto.EmploymentTypeId;
            employee.DepartmentId = employeeDto.DepartmentId;
            employee.RoleId = employeeDto.RoleId;
            employee.CurrencyTypeId = employeeDto.CurrencyTypeId;
            employee.ApprovalGroupId = employeeDto.ApprovalGroupId;
            employee.StatusTypeId = employeeDto.StatusTypeId;

            _context.Employees.Update(employee);
            //_context.Entry(employeeDto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return Conflict(new RespStatus { Status = "Failure", Message = "Employee Doesnt Exist!" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new RespStatus { Status = "Success", Message = "Employee Records Updated!" });
        }

        // POST: api/Employees
        [HttpPost]
        [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr")]
        public async Task<ActionResult<Employee>> PostEmployee(EmployeeDTO employeeDto)
        {

            //var emplye = _context.Employees.Where(e => e.FirstName == employeeDto.FirstName && e.MiddleName == employeeDto.MiddleName && e.LastName == employeeDto.LastName).FirstOrDefault();

            var emplye = _context.Employees.Where(e => e.Email == employeeDto.Email || e.EmpCode == employeeDto.EmpCode || e.MobileNumber == employeeDto.MobileNumber).FirstOrDefault();

            if (emplye != null)
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Unique EmpCode/Mobile/Email required" });
            }


            Employee employee = new Employee
            {
                FirstName = employeeDto.FirstName,
                MiddleName = employeeDto.MiddleName,
                LastName = employeeDto.LastName,
                EmpCode = employeeDto.EmpCode,
                BankAccount = employeeDto.BankAccount,
                BankCardNo = employeeDto.BankCardNo,
                NationalID = employeeDto.NationalID,
                PassportNo = employeeDto.PassportNo,
                TaxNumber = employeeDto.TaxNumber,
                Nationality = employeeDto.Nationality,
                DOB = employeeDto.DOB,
                DOJ = employeeDto.DOJ,
                Gender = employeeDto.Gender,
                Email = employeeDto.Email,
                MobileNumber = employeeDto.MobileNumber,
                EmploymentTypeId = employeeDto.EmploymentTypeId,
                DepartmentId = employeeDto.DepartmentId,
                RoleId = employeeDto.RoleId,
                ApprovalGroupId = employeeDto.ApprovalGroupId,
                CurrencyTypeId = employeeDto.CurrencyTypeId,
                StatusTypeId = employeeDto.StatusTypeId
            };

            MailAddress mailAdd = new MailAddress(employeeDto.Email);
            if ((mailAdd.Host.ToUpper() != "MAILINATOR.COM") && mailAdd.Host.ToUpper() != "GMAIL.COM")
            {
                return Conflict(new RespStatus { Status = "Failure", Message = "Use company mail address!" });
            }
            else
            {
                employee.Email = employeeDto.Email;
            }
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "AtominosAdmin, Admin, Manager, Finmgr")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NoContent();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
