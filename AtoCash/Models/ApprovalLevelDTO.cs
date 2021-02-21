using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    public class ApprovalLevelDTO
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public string LevelDesc { get; set; }
    }
}
