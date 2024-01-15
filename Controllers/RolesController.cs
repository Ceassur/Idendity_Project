using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity_V2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity_V2.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RolesController(RoleManager<AppRole> roleManager)
        {

            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Login", "Account");
            }
            return View(_roleManager.Roles);

        }
        [HttpPost]
        public async Task<IActionResult> Create(AppRole model)
        {

            if (ModelState.IsValid)
            {

                var result = await _roleManager.CreateAsync(model);

                if (result.Succeeded)
                {

                    return RedirectToAction("Index");
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }


    }


}