using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCProject.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            UserFriends = new List<User>();
            Posts = new List<Post>();
        }
        public User User { get; set; }

        public List<User> UserFriends { get; set; }

        public List<Post> Posts { get; set; }
    }
}
