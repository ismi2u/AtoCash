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
        [Column(TypeName = "nvarchar(150)")]
        public string Description { get; set; }
        //Foreign Key Relationsions

        [ForeignKey("ExpenseTypeId")]
        public virtual ExpenseType ExpenseType { get; set; }
        public int ExpenseTypeId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
        public int? ProjectId { get; set; }

        [ForeignKey("SubProjectId")]
        public virtual SubProject SubProject { get; set; }
        public int? SubProjectId { get; set; }

        [ForeignKey("WorkTaskId")]
        public virtual WorkTask WorkTask { get; set; }
        public int? WorkTaskId { get; set; }


    }

    public class ExpenseReimburseRequestDTO
    {

        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Double ExpenseReimbClaimAmount { get; set; }

        public ICollection<IFormFile> Documents { get; set; }

        public DateTime ExpReimReqDate { get; set; }

        public int ExpenseTypeId { get; set; }

        public int? ProjectId { get; set; }

        public int? SubProjectId { get; set; }

        public int? WorkTaskId { get; set; }


    }
}
