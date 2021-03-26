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
        public string EmployeeName { get; set; }

        public int? PettyCashRequestId { get; set; }
        public int? ExpenseReimburseRequestId { get; set; }

        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }

        public int? SubProjectId { get; set; }
        public string SubProjectName { get; set; }

        public int? WorkTaskId { get; set; }
        public string WorkTask { get; set; }

        public int RoleId { get; set; }
        public string JobRole{ get; set; }

        public int ApprovalLevelId { get; set; }

        public DateTime ReqDate { get; set; }

        public DateTime? FinalApprovedDate { get; set; }

        public int ApprovalStatusTypeId { get; set; }
        public string ApprovalStatusType { get; set; }

    }
}
