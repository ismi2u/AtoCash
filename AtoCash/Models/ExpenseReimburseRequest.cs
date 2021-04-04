using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    public class ExpenseReimburseRequest
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        public int EmployeeId { get; set; }

        [Required]
        public Double ExpenseReimbClaimAmount { get; set; }

        public string Documents { get; set; }

        [Required]
        public DateTime ExpReimReqDate { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string InvoiceNo { get; set; } 
        
        [Required]
        public DateTime InvoiceDate { get; set; }  
        
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Vendor { get; set; } 
        
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string Location { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string Description { get; set; }

        //Foreign Key Relationsions
        [Required]
        [ForeignKey("CurrencyTypeId")]
        public virtual CurrencyType CurrencyType { get; set; }
        public int CurrencyTypeId { get; set; }

        [Required]
        [ForeignKey("ExpenseTypeId")]
        public virtual ExpenseType ExpenseType { get; set; }
        public int ExpenseTypeId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        public int? DepartmentId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
        public int? ProjectId { get; set; }

        [ForeignKey("SubProjectId")]
        public virtual SubProject SubProject { get; set; }
        public int? SubProjectId { get; set; }

        [ForeignKey("WorkTaskId")]
        public virtual WorkTask WorkTask { get; set; }
        public int? WorkTaskId { get; set; }



        [Required]
        [ForeignKey("ApprovalStatusTypeId")]
        public virtual ApprovalStatusType ApprovalStatusType { get; set; }
        public int ApprovalStatusTypeId { get; set; }

        public DateTime? ApprovedDate { get; set; }


    }

    public class ExpenseReimburseRequestDTO
    {

        public int Id { get; set; }

        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }

        public Double ExpenseReimbClaimAmount { get; set; }

        public string Documents { get; set; }

        public DateTime ExpReimReqDate { get; set; }

        public string InvoiceNo { get; set; }

        public DateTime InvoiceDate { get; set; }

        public string Vendor { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }
        //Foreign Key Relationsions

        public int CurrencyTypeId { get; set; }
        public string CurrencyType { get; set; }
        public int ExpenseTypeId { get; set; }
        public string ExpenseType{ get; set; }

        public string Department { get; set; }
        public int? DepartmentId { get; set; }
        public string Project { get; set; }
        public int? ProjectId { get; set; }

        public string SubProject { get; set; }
        public int? SubProjectId { get; set; }

        public string WorkTask { get; set; }
        public int? WorkTaskId { get; set; }


        public string ApprovalStatusType { get; set; }
        public int ApprovalStatusTypeId { get; set; }


        public DateTime? ApprovedDate { get; set; }

    }
}
