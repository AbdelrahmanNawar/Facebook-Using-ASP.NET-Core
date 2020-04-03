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
    [Authorize(Roles = "Ultimate Admin")]
    //[Authorize(Roles = "Admino")]
    //[Authorize(Roles = "Admina")]
    //[AllowAnonymous]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;
        private readonly IDataRepository<Post, int> postRepository;
        private readonly IDataRepository<FriendRequest, string> friendRequestRepository;
        private readonly IDataRepository<User, string> userRepository;
        private User currentUser;

        public AdministrationController(RoleManager<Role> roleManager,
                                        UserManager<User> userManager,
                                        IDataRepository<Post, int> postRepository,
                                        IDataRepository<FriendRequest, string> friendRequestRepository,
                                        IDataRepository<User, string> userRepository)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.postRepository = postRepository;
            this.friendRequestRepository = friendRequestRepository;
            this.userRepository = userRepository;
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

        [HttpGet]
        public IActionResult AllUsers()
        {
            var usersList = userManager.Users.ToList();
            usersList.Remove(GetCurrentUser());
            ViewBag.User = GetCurrentUser();
            return View(usersList);
        }

        [HttpGet]
        public IActionResult AllRoles()
        {
            var roles = roleManager.Roles;
            ViewBag.User = GetCurrentUser();
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            ViewBag.User = GetCurrentUser();
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role With Id = {id} Can not be found";
                return View("NotFound");
            }
            var model = new EditRoleViewModel()
            {
                Id = role.Id,
                RoleName = role.Name,
                Description = role.Description
            };
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add($"{user.UserFirstName} {user.UserLastName}");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            ViewBag.User = GetCurrentUser();
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(model.Id);
                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Role With Id = {model.Id} Can not be found";
                    return View("NotFound");
                }
                else
                {
                    role.Name = model.RoleName;
                    role.Description = model.Description;
                    var result = await roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("AllRoles");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string Id)
        {
            ViewBag.User = GetCurrentUser();
            ViewBag.RoleId = Id;
            var role = await roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id:{Id} can not be found";
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users)
            {
                var userRoleModel = new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleModel.IsSelected = true;
                }
                else
                {
                    userRoleModel.IsSelected = false;
                }
                model.Add(userRoleModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> models, string Id)
        {
            ViewBag.User = GetCurrentUser();
            var role = await roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id:{Id} can not be found";
                return View("NotFound");
            }
            for (int i = 0; i < models.Count; i++)
            {
                var user = await userManager.FindByIdAsync(models[i].UserId);
                IdentityResult result = null;
                if (models[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!models[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < models.Count - 1)
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = Id });
                }
            }
            return RedirectToAction("EditRole", new { Id = Id });
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            ViewBag.User = GetCurrentUser();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            ViewBag.User = GetCurrentUser();
            if (ModelState.IsValid)
            {
                //IdentityRole identityRole = new IdentityRole() { Name = model.RoleName };
                Role role = new Role() { Name = model.RoleName, Description = model.Description };
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("AllRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AdminUserSearch(string searchText)
        {
            ViewBag.User = GetCurrentUser();
            if (searchText == null || searchText == "")
            {
                var searchList = userManager.Users.ToList();
                searchList.Remove(GetCurrentUser());
                return View(searchList);
            }
            string[] fullname = searchText.Split(" ");
            List<User> users = null;
            if (fullname.Length == 1)
            {
                users = userManager.Users.Where(u => u.UserFirstName.Contains(searchText) || u.UserLastName.Contains(searchText)).ToList();
            }
            else if (fullname.Length > 1)
            {
                users = userManager.Users.Where(u => u.UserFirstName.Contains(fullname[0]) && u.UserLastName.Contains(fullname[1])).ToList();
            }

            if (users == null)
            {
                return NotFound();
            }
            users.Remove(GetCurrentUser());
            return PartialView(users);
        }

        [HttpGet]
        public void Block(string id)
        {
            if (id != null)
            {
                var user = userRepository.SelectById(id);
                user.IsBlocked = true;
                userRepository.Update(id, user);
            }
        }

        [HttpGet]
        public void Unblock(string id)
        {
            if (id != null)
            {
                var user = userRepository.SelectById(id);
                user.IsBlocked = false;
                userRepository.Update(id, user);
            }
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            ViewBag.User = GetCurrentUser();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterViewModel model)
        {
            ViewBag.User = GetCurrentUser();
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
                    IdentityResult identityResult = await userManager.AddToRoleAsync(user, model.RoleName);
                    if (identityResult.Succeeded)
                        return RedirectToAction("AllUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);
        }


    }
}