using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AtoCash.Models
{
    public class WorkTask
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("SubProjectId")]
        public virtual SubProject SubProject { get; set; }
        public int SubProjectId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(25)")]
        public string TaskName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string TaskDesc { get; set; }

    }
}
