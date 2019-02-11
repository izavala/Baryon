using Baryon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baryon.Data
{
    public interface IDataEd
    {
        void RemoveForum(Forum model);
        void RemoveAllPost(string model);
        void RemovePost(Post model);
        void RemoveComment(Comment model);
        void LikePost(int id);
        void LikeComment(int id);
       // void UpdateModRequest(ModRequestViewModel update);
    }
}
