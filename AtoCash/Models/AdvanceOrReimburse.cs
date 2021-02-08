using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    public class AdvanceOrReimburse
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string ClaimType { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string ClaimTypeDesc { get; set; }
    }
}
