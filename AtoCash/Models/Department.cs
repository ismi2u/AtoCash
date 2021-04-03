using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtoCash.Models
{
    public class Department
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string DeptCode { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string DeptName { get; set; }

        [Required]
        [ForeignKey("CostCentreId")]
        public virtual CostCentre CostCentre { get; set; }
        public int CostCentreId { get; set; }


        [Required]
        [ForeignKey("StatusTypeId")]
        public virtual StatusType StatusType { get; set; }
        public int StatusTypeId { get; set; }
    }

    public class DepartmentDTO
    {

        public int Id { get; set; }

        public string DeptCode { get; set; }

        public string DeptName { get; set; }

        public int CostCentreId { get; set; }

        public string CostCentre { get; set; }

        public string StatusType { get; set; }

        public int StatusTypeId { get; set; }

    }

    public class DepartmentVM
    {

        public int Id { get; set; }

        public string DeptName { get; set; }
        public string DeptDesc { get; set; }

    }
}
