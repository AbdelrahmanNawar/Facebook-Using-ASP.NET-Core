using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVCProject.Models
{
    public class Like
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public int PostId { get; set; }
        public bool IsLiked { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
