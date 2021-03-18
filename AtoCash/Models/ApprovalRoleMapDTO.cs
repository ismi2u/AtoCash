﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtoCash.Models
{
    public class ApprovalRoleMapDTO
    {
        public int Id { get; set; }

        public string ApprovalGroup { get; set; }
        public int ApprovalGroupId { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }

        public string ApprovalLevel { get; set; }
        public int ApprovalLevelId { get; set; }
    }
}
