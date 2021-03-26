using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    public class EmploymentTypeDTO
    {

        public int Id { get; set; }
        public string EmpJobTypeCode { get; set; }
        public string EmpJobTypeDesc { get; set; }

    }
}
