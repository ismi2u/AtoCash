using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    public class PettyCashRequest
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        public int EmployeeId { get; set; }

        [Required]
        [Column(TypeName = "decimal(7,2)")]
        public decimal PettyClaimAmount { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string PettyClaimRequestDesc{ get; set; }

        [Required]
        public DateTime CashReqDate { get; set; }

        
        /// <summary>
        /// //////////////////
        /// </summary>
        //Foreign Key Relationsions

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
        public int? ProjectId { get; set; }

        [ForeignKey("SubProjectId")]
        public virtual SubProject SubProject { get; set; }
        public int? SubProjectId { get; set; }

        [ForeignKey("WorkTaskId")]
        public virtual WorkTask WorkTask { get; set; }
        public int? WorkTaskId { get; set; }

    }
}
