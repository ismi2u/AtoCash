using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    public class ExpenseReimburseRequestDTO
    {

        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public decimal ExpenseReimbClaimAmount { get; set; }

        public List<IFormFile> Documents { get; set; }

        public DateTime ExpReimReqDate { get; set; }

        public int? ExpenseTypeId { get; set; }

        public int? ProjectId { get; set; }

        public int? SubProjectId { get; set; }

        public int? WorkTaskId { get; set; }


    }
}
