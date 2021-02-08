using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    public class JobRole
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string RoleCode { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string RoleName { get; set; }

        [Required]
        [Column(TypeName = "decimal(7,2)")]
        public decimal MaxPettyCashAllowed { get; set; }

    }
}
