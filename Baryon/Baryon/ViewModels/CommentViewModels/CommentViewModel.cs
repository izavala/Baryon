using Baryon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baryon.ViewModels.CommentViewModels
{
    public class CommentViewModel
    {
        public IEnumerable<Comment> Comments { get; set; }

        public Post PostInf { get; set; }
        public Comment CommentId { get; set; }
    }
}
