﻿using System;
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
        [Column(TypeName = "nvarchar(20)")]
        public string CostCentreCode { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string CostCentreDesc{ get; set; }

        [Required]
        [ForeignKey("StatusTypeId")]
        public virtual StatusType StatusType { get; set; }
        public int StatusTypeId { get; set; }

    }

    public class CostCentreDTO
    {

        public int Id { get; set; }
        public string CostCentreCode { get; set; }
        public string CostCentreDesc { get; set; }

        public int StatusTypeId { get; set; }
        public string StatusType { get; set; }

    }

    public class CostCentreVM
    {
        public int Id { get; set; }
        public string CostCentreCode { get; set; }


    }
}
