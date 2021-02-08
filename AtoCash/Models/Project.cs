using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AtoCash.Models
{
    public class Project
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(25)")]
        public string ProjectName { get; set; }

        [Required]
        [ForeignKey("CostCentreId")]
        public virtual CostCentre CostCentre { get; set; }
        public int CostCentreId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string ProjectDesc{ get; set; }

    }
}
