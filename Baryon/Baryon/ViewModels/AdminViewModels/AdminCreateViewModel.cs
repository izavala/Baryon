using Baryon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baryon.ViewModels.AdminViewModels
{
    public class AdminCreateViewModel
    {
        public Forum newForum { get; set; }
        public Post initPost { get; set; }
    }
}
