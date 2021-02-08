using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtoCash.Models
{
    public class SubProjectDTO
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string SubProjectName { get; set; }
        public string SubProjectDesc { get; set; }
    }
}
