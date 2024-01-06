using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity_V2.Models
{
    public class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Admin_145";

        public static async void IdentityTestUser(IApplicationBuilder app)
        {


            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IdentityContext>();
            if (context.Database.GetAppliedMigrations().Any())
            {
                context.Database.Migrate();
            }
            var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            var user = await userManager.FindByNameAsync(adminUser);
            if (user == null)
            {
                user = new AppUser();
                user.Email = "Admin@admin.com";
                user.UserName = adminUser;
                user.PhoneNumber = "5347231566";
                user.FullName ="Berat Cesur";
                user.IsActive = true;
                await userManager.CreateAsync(user, adminPassword);

            }


        }

    }
}