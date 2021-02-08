using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    public class ClaimApprovalStatusTrackerDTO
    {
       public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int? PettyCashRequestId { get; set; }
        public int? ExpenseReimburseRequestId { get; set; }

        public int? DepartmentId { get; set; }

        public int? ProjectId { get; set; }

        public int RoleId { get; set; }

        public DateTime ReqDate { get; set; }

        public DateTime? FinalApprovedDate { get; set; }

        public int ApprovalStatusTypeId { get; set; }

    }
}
