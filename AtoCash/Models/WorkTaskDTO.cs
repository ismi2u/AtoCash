using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AtoCash.Models
{
    public class WorkTaskDTO
    {
        public int Id { get; set; }
        public int SubProjectId { get; set; }
        public string TaskName { get; set; }
        public string TaskDesc { get; set; }

    }
}
