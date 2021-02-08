using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    [ComplexType]
    public class TravelApprovalRequest
    {


        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        public int EmployeeId { get; set; }

        [Required]
        public DateTime TravelStartDate { get; set; }

        [Required]
        public DateTime TravelEndDate { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string TravelPurpose { get; set; }


        /// <summary>
        /// foreignkeys
        /// </summary>

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
