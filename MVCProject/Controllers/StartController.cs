using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCProject.Models;
using MVCProject.ViewModels;

namespace MVCProject.Controllers
{
    [AllowAnonymous]
    public class StartController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public StartController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                int gender;
                switch (model.Gender)
                {
                    case "Female":
                        gender = 0;
                        break;
                    case "Male":
                        gender = 1;
                        break;
                    default:
                        gender = 2;
                        break;
                }
                var user = new User()
                {
                    UserFirstName = model.FirstName,
                    UserLastName = model.LastName,
                    UserName = model.Email,
                    Email = model.Email,
                    UserBirthday = model.Birthday,
                    UserGender = gender
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    /// second parameter in SignInAsync() is a bool 
                    /// to specify weather we want to a session cookie or permenant cookie
                    /// session cookie is lost after closing the browsing window unlike the permenant cookie
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Profile", "Profile");
                }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                
            }
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.LoginEmail, model.LoginPassword, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    User.IsInRole("LoggedIn");
                    return RedirectToAction("Profile", "Profile");
                }
                    ModelState.AddModelError("","Invalid Login");
            }
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return View("Index");
        }

        [HttpGet]
        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}