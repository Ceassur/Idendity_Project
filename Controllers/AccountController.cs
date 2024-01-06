using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity_V2.Models;
using Identity_V2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Identity_V2.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private SignInManager<AppUser> _signInManager;

        public AccountController(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);

                        return RedirectToAction("Index", "Home");

                    }
                    else if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("", "Hesabınız kilitlenmiştir Lütfen daha sonra tekrar deneyin.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Login işlemi hatalı.");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Mail ve şifreniz htalı kontrol edin.");
                }

            }
            return View();
        }
    }
}