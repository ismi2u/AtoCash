using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtoCash.Models
{
    public class CostCentre
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string CostCentreCode { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string CostCentreDesc{ get; set; }

    }
}
