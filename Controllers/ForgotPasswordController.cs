using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Identity_V2.Controllers
{
    public class ForgotPasswordController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult UpdatePassword(string userId, string token, string password, string passwordconfirm)
        {
            // return "Basarili";
            return RedirectToAction("Index", "Home");
        }
    }
}