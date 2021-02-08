using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtoCash.Models
{
    public class ExpenseType
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string ExpenseTypeName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string ExpenseTypeDesc { get; set; }
    }
}
