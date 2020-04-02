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
    public class SettingsController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IDataRepository<User, string> userRepository;
        private readonly IDataRepository<FriendRequest, string> friendRequestRepository;
        private User currentUser;
        private List<User> friendList;

        public SettingsController(SignInManager<User> signInManager,
                                  UserManager<User> userManager,
                                  IDataRepository<User, string> userRepository,
                                  IDataRepository<FriendRequest, string> friendRequestRepository)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.friendRequestRepository = friendRequestRepository;
        }

        private User GetCurrentUser()
        {
            if (currentUser == null)
            {
                currentUser = userManager.FindByIdAsync(userManager.GetUserId(User)).Result;
                currentUser.FriendRequestReceivers = friendRequestRepository.GetFriendRequestReceivers(currentUser.Id);
                currentUser.FriendRequestSenders = friendRequestRepository.GetFriendRequestSenders(currentUser.Id);
            }
            return currentUser;
        }

        private List<User> GetFriends()
        {
            if (friendList == null)
            {
                friendList = new List<User>();
                foreach (var friendRequest in GetCurrentUser().FriendRequestReceivers)
                {
                    if (friendRequest.State == FriendRequestState.Accepted)
                        friendList.Add(friendRequest.Sender);
                }
                foreach (var friendRequest in GetCurrentUser().FriendRequestSenders)
                {
                    if (friendRequest.State == FriendRequestState.Accepted)
                        friendList.Add(friendRequest.Receiver);
                }
            }
            return friendList;
        }

        public IActionResult Index()
        {
            ViewBag.UserFriends = GetFriends();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ChangePasswordViewModel model)
        {
            ViewBag.UserFriends = GetFriends();
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
