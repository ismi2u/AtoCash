using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    public class EmpCurrentPettyCashBalanceDTO
    {
        public int Id { get; set; }
       public int EmployeeId { get; set; }
        public decimal CurBalance { get; set; }

        public DateTime UpdatedOn { get; set; }

    }
}
