using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    public class TravelRequestSearchModel
    {
        public int? EmpId { get; set; }
        public int? TravelApprovalRequestId { get; set; }

        public DateTime? TravelStartDate { get; set; }
        public DateTime? TravelEndDate { get; set; }
        
        public string TravelPurpose { get; set; }
        public int? DepartmentId { get; set; }
        public int? ProjectId { get; set; }
        //public int RoleId { get; set; }
        public DateTime? ReqRaisedDateFrom { get; set; }

        public DateTime? ReqRaisedDateTo{ get; set; }

        public int CurrentStatus { get; set; }
    }
}
