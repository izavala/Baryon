using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Baryon.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        [Display(Name = "Comment")]
        public string Text { get; set; }
        public int Likes { get; set; }
        public string UID { get; set; }
        public DateTime Date { get; set; }
        public int Thread { get; set; }
    }
}
