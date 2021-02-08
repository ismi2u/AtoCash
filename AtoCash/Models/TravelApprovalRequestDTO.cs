using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    public class TravelApprovalRequestDTO
    {

        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public DateTime TravelStartDate { get; set; }
        public DateTime TravelEndDate { get; set; }
        public string TravelPurpose { get; set; }

        public int? ProjectId { get; set; }
        public int? SubProjectId { get; set; }
        public int? WorkTaskId { get; set; }


    }
}
