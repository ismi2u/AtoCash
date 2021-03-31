using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    public class TravelApprovalStatusTrackerDTO
    {

        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int TravelApprovalRequestId { get; set; }


        public DateTime TravelStartDate { get; set; }

        public DateTime TravelEndDate { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }

        public int RoleId { get; set; }
        public int ApprovalLevelId { get; set; }
        public DateTime ReqDate { get; set; }

        public DateTime? FinalApprovedDate { get; set; }
        public int ApprovalStatusTypeId { get; set; }



    }
}
