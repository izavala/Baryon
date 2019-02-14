using Baryon.Models;
using Baryon.ViewModels;
using Baryon.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baryon.Data
{
    public interface IDataEd : IDisposable
    {
        void RemoveForum(Forum model);
        void RemoveAllPost(string model);
        void RemovePost(Post model);
        void RemoveComment(int comment);
        void LikePost(int id);
        void LikeComment(int id);
        void UpdateModRequest(ModRequestViewModel update);
    }
}
