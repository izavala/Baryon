using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Baryon.Models
{
    public class Forum
    {
        public int FSubscribers { get; set; }
        [Display(Name = "Name of Forum")]
        public string FName { get; set; }
        public int FId { get; set; }
        public int FPosts { get; set; }
        [Display(Name = "Is this forum private?")]
        public bool HasLock { get; set; }
    }
}
