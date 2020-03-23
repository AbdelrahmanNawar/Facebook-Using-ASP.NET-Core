using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCProject.Models
{
    public class User: IdentityUser
    {
        public User()
            :base()
        {
            Posts = new HashSet<Post>();
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
            FriendRequestSenders = new HashSet<FriendRequest>();
            FriendRequestReceivers = new HashSet<FriendRequest>();
        }

        [Required]
        public String UserFirstName { get; set; }
        public String UserLastName { get; set; }
        [Required]
        public DateTime UserBirthday { get; set; }
        [Required]
        public int UserGender { get; set; }
        public String UserBio { get; set; }
        public String UserPicture { get; set; }
        public String UserRole { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsBlocked { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<FriendRequest> FriendRequestSenders { get; set;}
        public virtual ICollection<FriendRequest> FriendRequestReceivers { get; set; }
    }
}
