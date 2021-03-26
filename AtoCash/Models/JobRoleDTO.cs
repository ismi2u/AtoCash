using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    public class JobRoleDTO
    {

        public int Id { get; set; }

        public string RoleCode { get; set; }

        public string RoleName { get; set; }
        public Double MaxPettyCashAllowed { get; set; }

    }
}
