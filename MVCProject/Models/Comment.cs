using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCProject.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public DateTime CommentDateTime { get; set; }
        public String CommentContent { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual Post Post { get; set; }

    }
}
