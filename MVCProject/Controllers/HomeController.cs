using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCProject.DataRepositories;
using MVCProject.Models;
using MVCProject.ViewModels;

namespace MVCProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataRepository<FriendRequest, string> friendRequestRepository;
        private readonly IDataRepository<User, string> userRepository;
        private readonly IDataRepository<Post, int> postRepository;
        private readonly IDataRepository<Like, string> likeRepository;
        private readonly IDataRepository<Comment, int> commentRepository;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private User currentUser;

        public HomeController(IDataRepository<FriendRequest, string> friendRequestRepository,
                              IDataRepository<User, string> userRepository,
                              IDataRepository<Post, int> postRepository,
                              IDataRepository<Like, string> likeRepository,
                              IDataRepository<Comment, int> commentRepository,
                              SignInManager<User> signInManager,
                              UserManager<User> userManager)
        {
            this.friendRequestRepository = friendRequestRepository;
            this.userRepository = userRepository;
            this.postRepository = postRepository;
            this.likeRepository = likeRepository;
            this.commentRepository = commentRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
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

        [HttpGet]
        public IActionResult Index()
        {
            List<Post> posts = new List<Post>();
            foreach (var item in GetCurrentUser().FriendRequestReceivers)
            {
                posts.AddRange(postRepository.SelectAll().ToList().Where(p => p.UserId == item.SenderId));
            }
            foreach (var item in GetCurrentUser().FriendRequestSenders)
            {
                posts.AddRange(postRepository.SelectAll().ToList().Where(p => p.UserId == item.ReceiverId));
            }
            posts.AddRange(GetCurrentUser().Posts);

            foreach (var post in posts)
            {
                foreach (var like in post.Likes)
                {
                    like.User = userRepository.SelectById(like.UserId);
                }
            }

            posts.OrderBy(p => p.PostDateTime);

            List<User> friendList = new List<User>();
            foreach (var friendRequest in GetCurrentUser().FriendRequestReceivers)
            {
                if(friendRequest.State == FriendRequestState.Accepted)
                    friendList.Add(friendRequest.Sender);
            }
            foreach (var friendRequest in GetCurrentUser().FriendRequestSenders)
            {
                if(friendRequest.State == FriendRequestState.Accepted)
                    friendList.Add(friendRequest.Receiver);
            }

            HomeViewModel model = new HomeViewModel()
            {
                UserFriends = friendList,
                Posts = posts,
                User = GetCurrentUser()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Post p)
        {
            p.PostDateTime = DateTime.Now;
            postRepository.Insert(p);
            return Json(p.PostId);
        }

        [HttpPost]
        public ActionResult AddComment(Comment c)
        {
            commentRepository.Insert(c);
            return Json(c.PostId);
        }

        [HttpPost]
        public void Like(Like l)
        {
            likeRepository.Like(l);
        }

        [HttpPost]
        public ActionResult RemovePost(int PostId)
        {
            postRepository.Delete(PostId);
            return Json(PostId);
        }

        [HttpPost]
        public ActionResult RemoveComment(int commentId)
        {
            commentRepository.Delete(commentId);
            return Json(commentId);
        }
    }
}
