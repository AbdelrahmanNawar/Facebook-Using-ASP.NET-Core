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
    public class ProfileController : Controller
    {
        private readonly IDataRepository<FriendRequest, string> friendRequestRepository;
        private readonly IDataRepository<User, string> userRepository;
        private readonly IDataRepository<Post, int> postRepository;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IDataRepository<Like, string> likeRepository;
        private readonly IDataRepository<Comment, int> commentRepository;
        private User currentUser;
        private List<User> friendList;

        public ProfileController(IDataRepository<FriendRequest, string> friendRequestRepository,
                                 IDataRepository<User, string> userRepository,
                                 IDataRepository<Post, int> postRepository,
                                 SignInManager<User> signInManager,
                                 UserManager<User> userManager,
                                 IDataRepository<Like, string> likeRepository,
                                 IDataRepository<Comment, int> commentRepository)
        {
            this.friendRequestRepository = friendRequestRepository;
            this.userRepository = userRepository;
            this.postRepository = postRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.likeRepository = likeRepository;
            this.commentRepository = commentRepository;
        }

        private User GetCurrentUser()
        {
            if (currentUser == null)
            {
                currentUser = userManager.FindByIdAsync(userManager.GetUserId(User)).Result;
                currentUser.Posts = postRepository.SelectByUserId(currentUser.Id);
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

        [AllowAnonymous]
        public IActionResult Search(string searchText)
        {
            ViewBag.CurrentUser = GetCurrentUser();
            ViewBag.UserFriends = GetFriends();
            if (searchText == null || searchText == "")
            {
                var searchList = userRepository.SelectAll();
                searchList.Remove(GetCurrentUser());
                return View(searchList);
            }
            string[] fullname = searchText.Split(" ");
            List<User> users = null;
            if (fullname.Length == 1)
            {
                users = userRepository.SelectAll().Where(u => u.UserFirstName.Contains(searchText) || u.UserLastName.Contains(searchText)).ToList();
            }
            else if (fullname.Length > 1)
            {
                users = userRepository.SelectAll().Where(u => u.UserFirstName.Contains(fullname[0]) || u.UserLastName.Contains(fullname[1]) || u.UserFirstName.Contains(fullname[1]) || u.UserLastName.Contains(fullname[0])).ToList();
            }

            if (users == null)
            {
                return NotFound();
            }
            users.Remove(GetCurrentUser());
            return View(users);
        }

        [HttpGet]
        public IActionResult Profile()
        {
            if (GetCurrentUser().IsBlocked)
                return View("Blocked");
            ViewBag.UserFriends = GetFriends();
            User u = GetCurrentUser();
            u.Posts = u.Posts.OrderByDescending(p => p.PostDateTime).ToList();
            foreach (var p in u.Posts)
            {
                p.Likes = likeRepository.SelectAll().Where(x => x.PostId == p.PostId).ToList();
                p.Comments = commentRepository.SelectAll().Where(x => x.PostId == p.PostId).ToList();
            }
            return View(GetCurrentUser());
        }

        [HttpPost]
        public IActionResult Profile(string newPost, string ImageFile)
        {
            ViewBag.UserFriends = GetFriends();
            if (newPost != null || ImageFile != null)
            {
                Post p = new Post()
                {
                    PostContent = newPost,
                    PostDateTime = DateTime.Now,
                    UserId = GetCurrentUser().Id,
                    PostImage = ImageFile,
                    IsDeleted = false
                };
                postRepository.Insert(p);
            }
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public ActionResult ProfilePost(string newPost, string ImageFile)
        {
            ViewBag.UserFriends = GetFriends();
            Post post = new Post();
            if (newPost != null || ImageFile != null)
            {
                Post p = new Post()
                {
                    PostContent = newPost,
                    PostDateTime = DateTime.Now,
                    UserId = GetCurrentUser().Id,
                    PostImage = ImageFile,
                    IsDeleted = false
                };
                GetCurrentUser().Posts.Add(p);
                postRepository.Insert(p);
                post = p;
            }
            return Json(post.PostId);
            //return RedirectToAction("Profile");
        }

        [HttpPost, ActionName("ProfilePic")]
        public IActionResult ProfilePic(string ImageFile)
        {
            ViewBag.UserFriends = GetFriends();
            if (ImageFile != null)
            {
                GetCurrentUser().UserPicture = ImageFile;
                userRepository.Update(GetCurrentUser().Id, GetCurrentUser());
            }
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public IActionResult Edit(AboutViewModel model)
        {
            ViewBag.UserFriends = GetFriends();
            if (ModelState.IsValid)
            {
                GetCurrentUser().AboutViewModel = model;
                userRepository.Update(GetCurrentUser().Id, GetCurrentUser());
            }

            return View("Profile", GetCurrentUser());
        }

        public IActionResult Friend_Profile(string id)
        {
            ViewBag.UserFriends = GetFriends();
            ViewBag.CurrentUser = GetCurrentUser();
            if (!userRepository.IsExist(id))
            {
                return NotFound();
            }

            var user = userRepository.SelectById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        public void AddFriend(string id)
        {
            if (id != null)
            {
                var friendRequest = new FriendRequest() { SenderId = GetCurrentUser().Id, ReceiverId = id, State = FriendRequestState.Pending };
                //_context.FriendRequests.Add(friendRequest);
                friendRequestRepository.Insert(friendRequest);
                //_context.SaveChanges();
            }
        }

        public void RemoveFriend(string id)
        {

            if (id != null)
            {
                var friendRequest = friendRequestRepository.SelectAll().SingleOrDefault(m => (m.SenderId == GetCurrentUser().Id && m.ReceiverId == id) || (m.SenderId == id && m.ReceiverId == GetCurrentUser().Id));
                if (friendRequest != null)
                {
                    friendRequestRepository.Delete(friendRequest);

                }
            }
        }
        public void AcceptFriend(string id)
        {
            if (id != null)
            {
                var friendRequest = friendRequestRepository.SelectAll().SingleOrDefault(m => m.SenderId == id && m.ReceiverId == GetCurrentUser().Id);

                if (friendRequest != null)
                {
                    friendRequest.State = FriendRequestState.Accepted;
                    friendRequestRepository.Update("", friendRequest);
                }
            }
        }

        [HttpPost]
        public void Like(string UserId, int PostId)
        {
            Like like = new Like() { UserId = UserId, PostId = PostId, IsLiked = true };
            likeRepository.Like(like);
        }

        [HttpPost]
        public ActionResult RemovePost(int PostId)
        {
            ViewBag.UserFriends = GetFriends();
            var result = postRepository.SelectById(PostId);
            if (result != null)
            {
                result.IsDeleted = true;
                postRepository.Update(0, result);
            }

            return Json(result.PostId);
        }
        [HttpPost]
        public ActionResult AddComment(Comment c)
        {

            commentRepository.Insert(c);

            return Json(c.CommentId);
            //return PartialView("Comments");
        }
        [HttpPost]
        public ActionResult RemoveComment(int CommentId)
        {
            ViewBag.UserFriends = GetFriends();
            //var result = db.Comments.SingleOrDefault(Comment => Comment.CommentId == c.CommentId);
            var result = commentRepository.SelectById(CommentId);
            if (result != null)
            {
                result.IsDeleted = true;
                commentRepository.Update(0, result);
            }
            return Json(result.PostId);
        }

    }
}

