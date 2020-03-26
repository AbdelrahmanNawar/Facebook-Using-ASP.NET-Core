using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCProject.Models
{
    public class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
        }
        public int PostId { get; set; }
        [Required]
        public DateTime PostDateTime { get; set; }
        public String PostContent { get; set; }
        public String PostImage { get; set; }
        public bool IsDeleted { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
    }
}