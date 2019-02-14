using Baryon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baryon.ViewModels.AdminViewModels
{
    public class ModRequestViewModel
    {
        public IEnumerable<Request> _Requests { get; set; }
        public Request response { get; set; }
    }
}
