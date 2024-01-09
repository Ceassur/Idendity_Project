using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity_V2.Models;
using Identity_V2.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Identity_V2.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private SignInManager<AppUser> _signInManager;
        private IEmailSender _emailSender;

        public object FormsAuthentication { get; private set; }

        public AccountController(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IEmailSender emailSender
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
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
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError("", "Hesabınızı Onaylayınız");
                        return View(model);
                    }

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
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View();
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
                var url = Url;

                if (result.Succeeded)
                {

                    if (model.SelectedRoles != null)
                    {
                        await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                    }

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    Url.Action("ConfirmEmail", "Account", new { user.Id, token });

                    await _emailSender.SendEmailAsync(user.Email, "Hesap Onayı", "http://localhost:5225" + url);

                    TempData["message"] = "Hesabınızı Onaylamak İçin Mail Adresinizi Kontrol Ediniz !! ";

                    return RedirectToAction("Login", "Account");


                    return RedirectToAction("Create");
                }

                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }

            }
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> ConfirmEmail(string Id, string token)
        {

            if (Id == null || token == null)
            {
                TempData["message"] = "Geçersiz Token Bilgisi";
                return View();
            }

            var user = await _userManager.FindByIdAsync(Id);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    TempData["message"] = "Hesabınız Onaylanmıştır";
                    return View();
                }

            }
            else
            {
                TempData["message"] = "Kullanıcı bulunamadı";
            }
            return View();
        }

        public async Task<IActionResult> Logout() //Login işlemi için yazılan method.
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }




    }
}