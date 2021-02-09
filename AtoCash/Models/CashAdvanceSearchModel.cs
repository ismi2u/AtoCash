using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtoCash.Models
{
    public class CashAdvanceSearchModel
    {
        public int? EmpId { get; set; }
        public int? AdvanceOrReimburseId { get; set; }
        public int? DepartmentId { get; set; }
        public int? ProjectId{ get; set; }
        public int? SubProjectId { get; set; }
        public int? WorkTaskId { get; set; }
        public DateTime? RecordDateFrom { get; set; }
        public DateTime? RecordDateTo { get; set; }
        public decimal AmountFrom { get; set; }
        public decimal AmountTo { get; set; }
        public int? CostCentreId { get; set; }
        public int? ApprovalStatusId { get; set; }


    }
}
