using Baryon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baryon.Data
{
    public interface IDataUp
    {
        int SetComment(Comment comment);
        void SetPost(Post post);
       // int SetForum(AdminCreateViewModel model);
        bool SetAccess(string forum, string user);
        void SetModRequest(string request, string _user);
    }
}
