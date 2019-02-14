using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baryon.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string ForumID { get; set; }
        public string UID { get; set; }
        public bool Pending { get; set; }
        public int Status { get; set; }
    }
}
