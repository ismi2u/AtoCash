using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    public class DisbursementsAndClaimsMaster
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        public int EmployeeId { get; set; }


        [ForeignKey("PettyCashRequestId")]
        public virtual PettyCashRequest PettyCashRequest { get; set; }

        public int? PettyCashRequestId { get; set; }


        [ForeignKey("ExpenseReimburseReqId")]
        public virtual ExpenseReimburseRequest ExpenseReimburseRequest { get; set; }
        public int? ExpenseReimburseReqId { get; set; }

        [Required]
        [ForeignKey("AdvanceOrReimburseId")]
        public virtual AdvanceOrReimburse AdvanceOrReimburse { get; set; }
        public int AdvanceOrReimburseId { get; set; }

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
        public DateTime RecordDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(7,2)")]
        public decimal Amount { get; set; }


        [Required]
        [ForeignKey("CostCentreId")]
        public virtual CostCentre CostCentre { get; set; }
        public int CostCentreId { get; set; }

        [Required]
        [ForeignKey("ApprovalStatusId")]
        public ApprovalStatusType ApprovalStatusType { get; set; }
        public int ApprovalStatusId { get; set; }



    }
}
