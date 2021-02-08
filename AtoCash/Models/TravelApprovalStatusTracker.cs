using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    public class TravelApprovalStatusTracker
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        public int EmployeeId { get; set; }


        [ForeignKey("TravelApprovalRequestId")]
        public virtual TravelApprovalRequest TravelApprovalRequest { get; set; }

        public int TravelApprovalRequestId { get; set; }


        //Approver Department
        [Required]
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        public int? DepartmentId { get; set; }

        //Approver Project (either Department or Project => Can't be both)
        [Required]
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
        public int? ProjectId { get; set; }


        //Approver Role => Multiple Approvers, hence their roles
        [Required]
        [ForeignKey("RoleId")]
        public virtual JobRole Role { get; set; }
        public int RoleId { get; set; }

        [Required]
        public DateTime ReqDate { get; set; }

        public DateTime? FinalApprovedDate { get; set; }

        [Required]
        [ForeignKey("ApprovalStatusTypeId")]
        public virtual ApprovalStatusType ApprovalStatusType { get; set; }
        public int ApprovalStatusTypeId { get; set; }



    }
}
