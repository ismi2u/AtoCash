using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    public class DisbursementsAndClaimsMasterDTO
    {

        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int? PettyCashRequestId { get; set; }
        public int? ExpenseReimburseReqId { get; set; }
        public int AdvanceOrReimburseId { get; set; }
        public int? ProjectId { get; set; }
        public int? SubProjectId { get; set; }
        public int? WorkTaskId { get; set; }
        public DateTime RecordDate { get; set; }
        public decimal Amount { get; set; }
        public int CostCentreId { get; set; }
        public int ApprovalStatusId { get; set; }



    }
}
