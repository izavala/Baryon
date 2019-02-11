using Baryon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baryon.Data
{
    public interface IDataLd
    {
        IEnumerable<Forum> GetAll();
        Forum GetForum(string fname);
        IEnumerable<Post> GetPosts(string forumId);
        Post GetPost(int postId);
        IEnumerable<Comment> GetComments(int threadId);
        Comment GetComment(int cId);
       // ModRequestViewModel GetModRequest();
        FAccess GetFA(string fID, string user);
        string GetUser(string id);
        bool CheckLock(string id);
        bool HasForumAccess(string fID, string user);
    }
}
