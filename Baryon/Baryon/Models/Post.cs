using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baryon.Models
{
    public class Post
    {
        public int Likes { get; set; }
        public string PostText { get; set; }
        public string PostTitle { get; set; }
        public string ForumId { get; set; }
        public int PostId { get; set; }
        public string UID { get; set; }
        public DateTime Date { get; set; }
    }
}
