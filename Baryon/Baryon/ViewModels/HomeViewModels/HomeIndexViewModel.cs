using Baryon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baryon.ViewModels.HomeViewModels
{
    public class HomeIndexViewModel
    {

        public IEnumerable<Forum> Forums { get; set; }
        public Forum current { get; set; }

    }
}
