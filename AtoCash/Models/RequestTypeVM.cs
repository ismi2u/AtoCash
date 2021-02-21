using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AtoCash.Models
{
    public class RequestTypeVM
    {
        public int Id { get; set; }
        public string RequestName { get; set; }

    }
}
