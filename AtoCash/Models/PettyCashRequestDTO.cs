using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    public class PettyCashRequestDTO
    {

        public int Id { get; set; }
        public string EmployeeName { get; set; }

        public int EmployeeId { get; set; }
        public Double PettyClaimAmount { get; set; }

        public string PettyClaimRequestDesc { get; set; }

        public DateTime CashReqDate { get; set; }

        public string Department { get; set; }
        public int? DepartmentId { get; set; }
        public string Project { get; set; }
        public int? ProjectId { get; set; }

        public string SubProject { get; set; }
        public int? SubProjectId { get; set; }

        public string WorkTask { get; set; }
        public int? WorkTaskId { get; set; }

    }
}
