using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtoCash.Models
{
    public class DepartmentDTO
    {

        public int Id { get; set; }

        public string DeptCode { get; set; }

        public string DeptName { get; set; }

        public int CostCentreId { get; set; }

    }
}
