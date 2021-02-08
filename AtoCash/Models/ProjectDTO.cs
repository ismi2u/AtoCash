using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AtoCash.Models
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public int CostCentreId { get; set; }
        public string ProjectDesc{ get; set; }

    }
}
