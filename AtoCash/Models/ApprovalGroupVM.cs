﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtoCash.Models
{
    public class ApprovalGroupVM
    {
        public int Id { get; set; }
        public string ApprovalGroupCode { get; set; }
    }
}