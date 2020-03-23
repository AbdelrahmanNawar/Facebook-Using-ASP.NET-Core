﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCProject.Models;
using MVCProject.ViewModels;

namespace MVCProject.Controllers
{
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
                var user = new User()
                {
                    UserFirstName = model.FirstName,
                    UserLastName = model.LastName,
                    UserName = model.Email,
                    Email = model.Email,
                    UserBirthday = new DateTime(model.Year ?? 2000, model.Month ?? 1, model.Day ?? 1),
                    UserGender = model.Gender ?? 0
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    /// second parameter in SignInAsync() is a bool 
                    /// to specify weather we want to a session cookie or permenant cookie
                    /// session cookie is lost after closing the browsing window unlike the permenant cookie
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.LoginEmail, model.LoginPassword, isPersistent: false, lockoutOnFailure: false);
                if(result.Succeeded)
                    return RedirectToAction("Index", "Home");
                    ModelState.AddModelError("","Invalid Login");
            }
            return View("Index");
        }
    }
}