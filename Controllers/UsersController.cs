using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity_V2.Models;
using Identity_V2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity_V2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UsersController(UserManager<AppUser> UserManager, RoleManager<AppRole> RoleManager)
        {
            _userManager = UserManager;
            _roleManager = RoleManager;
        }
        public IActionResult Index()
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Login", "Account");
            }
            return View(_userManager.Users);
        }

        public async Task<IActionResult> List()
        {
            //BU ŞEKİLDE İF İLE DE KULLANALIABİLİ.. BELİRLİ BİR ROLE İÇİN AUTHORİZE İŞLEMİ YAPILIR..
            // if (!User.IsInRole("Admin")) 
            // {
            //     return RedirectToAction("Login", "Account");
            // }
            return View(_userManager.Users);
            var userList = await _userManager.Users.ToListAsync();
            return View(userList);
        }




        public async Task<IActionResult> Delete(string id)
        {

            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("List", "Users");
        }
    }
}