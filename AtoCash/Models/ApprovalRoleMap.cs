using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtoCash.Models
{
    public class ApprovalRoleMap
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ApprovalGroupId")]
        public virtual ApprovalGroup ApprovalGroup { get; set; }
        public int ApprovalGroupId { get; set; }

        [Required]
        [ForeignKey("RoleId")]
        public virtual JobRole Role { get; set; }
        public int RoleId { get; set; }

        [Required]
        public int ApprovalLevel { get; set; }
    }
}
