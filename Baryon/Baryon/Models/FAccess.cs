using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baryon.Models
{
    public class FAccess
    {
        public int ID { get; set; }
        public string ForumID { get; set; }
        public string UID { get; set; }
        public int CanEdit { get; set; }
        public int Requested { get; set; }
    }
}
