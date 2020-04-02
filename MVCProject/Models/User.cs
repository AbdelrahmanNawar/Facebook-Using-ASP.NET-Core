using Microsoft.AspNetCore.Identity;
using MVCProject.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVCProject.Models
{
    public class User : IdentityUser
    {
        public User()
            : base()
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
        public bool IsDeleted { get; set; }
        public bool IsBlocked { get; set; }

        [NotMapped]
        public AboutViewModel AboutViewModel
        {
            get
            {
                return new AboutViewModel()
                {
                    FirstName = UserFirstName,
                    LastName = UserLastName,
                    Gender = SetGender(UserGender),
                    Bio = UserBio,
                    ProfilePicture = UserPicture,
                    Birthdate = UserBirthday
                };
            }
            set
            {
                UserFirstName = value.FirstName ?? UserFirstName;
                UserLastName = value.LastName ?? UserLastName;
                UserGender = value.GetGender();
                UserBio = value.Bio ?? UserBio;
                UserPicture = value.ProfilePicture ?? UserPicture;
                UserBirthday = value.Birthdate;
            }
        }

        private string SetGender(int gender)
        {
            switch (gender)
            {
                case 0:
                    return "Female";
                case 1:
                    return "Male";
                default:
                    return "";
            }
        }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<FriendRequest> FriendRequestSenders { get; set; }
        public virtual ICollection<FriendRequest> FriendRequestReceivers { get; set; }
    }
}
