using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCProject.DataRepositories;
using MVCProject.Models;
using MVCProject.ViewModels;

namespace MVCProject.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IDataRepository<User, string> userRepository;
        private User currentUser;

        public SettingsController(SignInManager<User> signInManager,
                                  UserManager<User> userManager,
                                  IDataRepository<User, string> userRepository)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userRepository = userRepository;
        }

        private User GetCurrentUser()
        {
            if (currentUser == null)
            {
                currentUser = userManager.FindByIdAsync(userManager.GetUserId(User)).Result;
            }
            return currentUser;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await userManager.ChangePasswordAsync(GetCurrentUser(), model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile", "Profile");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View("Index");
        }

    }
}
