using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class IndexUser : DateUser
    {
        public string Name { get; set; }

        public string Email  { get; set; }

        public bool IsBlocked  { get; set; }
    }
}
