using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Identity_V2.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsActive { get; set; }
    }
}