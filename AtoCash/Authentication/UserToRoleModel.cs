using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Authentication
{
    public class UserToRoleModel
    {
        public string UserId { get; set; }
        public List<string> RoleIds { get; set; }
       

    }
}
