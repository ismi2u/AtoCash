using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtoCash.Models
{
    [ComplexType]
    public class Department
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string DeptCode { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string DeptName { get; set; }

        [Required]
        [ForeignKey("CostCentreId")]
        public virtual CostCentre CostCentre { get; set; }
        public int CostCentreId { get; set; }

    }
}
