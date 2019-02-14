using Baryon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baryon.ViewModels.HomeViewModels
{
    public class HomeSubViewModel
    {
        public IEnumerable<Post> Posts { get; set; }

        public Post FId { get; set; }
    }
}
