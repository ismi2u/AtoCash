﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AtoCash.Models
{
    public class StatusType
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(8)")]
        public string Status { get; set; }
    }

    public class StatusTypeVM
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }
}