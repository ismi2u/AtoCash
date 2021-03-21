using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AtoCash.Models
{
    public class WorkTaskVM
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string TaskDesc { get; set; }
    }
}
