using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtoCash.Models
{
    public class EmployeeDTO
    {
        public int Id { get; set; }


        public string FirstName { get; set; }

        public string MiddleName { get; set; }


        public string LastName { get; set; }

        public string EmpCode { get; set; }

        public string BankAccount { get; set; }


        public string BankCardNo { get; set; }

        public string NationalID { get; set; }


        public string PassportNo { get; set; }

        public string TaxNumber { get; set; }


        public string Nationality { get; set; }

        public DateTime DOB { get; set; }

        public DateTime DOJ { get; set; }


        public string Gender { get; set; }


        public string Email { get; set; }


        public string MobileNumber { get; set; }

        //Navigation Properties
        //------------------------------------------------

        public int EmploymentTypeId { get; set; }


        public int DepartmentId { get; set; }


        public int RoleId { get; set; }



        public int ApprovalGroupId { get; set; }

    }
}
