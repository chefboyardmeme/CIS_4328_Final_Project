using System;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
//
using UNFBusShuttle.Models;
using UNFBusShuttle.Data;

namespace UNFBusShuttle.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext db = new ApplicationDbContext();

        public AccountController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Login()
        { 
            return View(); 
        }

        //[Authorize]
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Login(string userId, string password)
        {
            var user = (from u in db.Users
                        .Where(x => x.UserId.ToLower() == userId.ToLower() && x.Password == password && x.IsActive == true)
                        select u).FirstOrDefault();

            if (user != null && user.Id > 0)
            {
                var claims = new[] { new Claim(ClaimTypes.Name, userId) };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                //
                // Set Logged-In User Information                
                HttpContext.Session.SetInt32("UserPk", user.Id);
                //HttpContext.Session.SetString("UserId",user.UserId);
                //HttpContext.Session.SetString("FirstName", user.FirstName);
                //HttpContext.Session.SetString("LastName", user.LastName);
                HttpContext.Session.SetString("UserName", (user.FirstName + " " + user.LastName));
                //HttpContext.Session.SetString("UserType", user.UserType);

                if (user.UserType.ToLower() == "driver")
                    return RedirectToAction("Index", "Home");
                else if (user.UserType.ToLower() == "admin")
                    return RedirectToAction("Index", "Admin");
                else
                    return RedirectToAction("Home", "Home");
            }
            //
            ViewBag.Error = "Not a valid User. Please try again.";
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            // Forms Authentication
            HttpContext.SignOutAsync();
            //
            return RedirectToAction("Login", "Account");
        }

    }

}

