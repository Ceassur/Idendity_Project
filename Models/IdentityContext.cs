using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity_V2.Models
{
    public class IdentityContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {

        }

    }
}