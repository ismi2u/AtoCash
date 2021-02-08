using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtoCash.Models
{
    [ComplexType]
    public class Employee
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string MiddleName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string EmpCode { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string BankAccount { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string BankCardNo { get; set; }

        public string NationalID { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string PassportNo { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string TaxNumber { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Nationality { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Required]
        public DateTime DOJ { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string MobileNumber { get; set; }

        //Navigation Properties
        //------------------------------------------------
        [Required]
        [ForeignKey("EmploymentTypeId")]
        public virtual EmploymentType EmploymentType { get; set; }
        public int EmploymentTypeId { get; set; }

        [Required]
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        public int DepartmentId { get; set; }


        [Required]
        [ForeignKey("RoleId")]
        public virtual JobRole Role { get; set; }
        public int RoleId { get; set; }

        [Required]
        [ForeignKey("ApprovalGroupId")]
        public virtual ApprovalGroup ApprovalGroup { get; set; }
        public int ApprovalGroupId { get; set; }

    }
}
