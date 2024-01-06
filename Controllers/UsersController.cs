using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity_V2.Models;
using Identity_V2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity_V2.Controllers
{
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
            return View();
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View();
        }
        public async Task<IActionResult> List()
        {
            var userList = await _userManager.Users.ToListAsync();
            return View(userList);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {

            if (ModelState.IsValid)
            {

                var user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    IsActive = true
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {

                    if (model.SelectedRoles != null)
                    {
                        await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                    }


                    return RedirectToAction("Create");
                }

                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }

            }
            return RedirectToAction("Create");
        }


        public async Task<IActionResult> Delete(string id)
        {

            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("Index");
        }
    }
}