using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Baryon.Models;
using Baryon.ViewModels;
using Baryon.ViewModels.AdminViewModels;
using Dapper;

namespace Baryon.Data
{
    public class DataUpload : IDataUp
    {
        private IDbConnection _connection;

        public DataUpload(IDbConnection connection)
        {
            _connection = connection;
        }

        public bool SetAccess(string forum, string user)
        {
            var permissions = _connection.Query<FAccess>($@"SELECT * FROM FAcces 
                                                                     WHERE UID = @{nameof(user)}", new { user });

            foreach (var permission in permissions)
            {
                if (permission.ForumID == forum)
                    return false;
            }
            _connection.Execute($@"INSERT INTO FAcces (ForumID, UID) Values (@{nameof(forum)}, @{nameof(user)})", new { forum, user } );
            return true;
        }

        public int SetComment(Comment comment)
        {
            //Sets comment and returns the unique comment Id 
            _connection.Query<int>($@"INSERT INTO Comments 
                               (Thread, Text, Likes, UID, Date)
                                Values (@{nameof(comment.Thread)}, @{nameof(comment.Text)}, @{nameof(comment.Likes)}, @{nameof(comment.UID)},@Date);
                                SELECT CAST(SCOPE_IDENTITY() as int)", new {  comment.Thread,  comment.Text, comment.Likes,  comment.UID, Date = DateTime.Now.Date }).SingleOrDefault();

            return comment.Thread;
        }

        public void SetModRequest(string request, string _user)
        {
            _connection.Execute($@"INSERT INTO Request(ForumID, UID, Pending , Status)
                                Values( @{nameof(request)}, @{nameof(_user)}, @pending, @status)", new { request, _user,pending = true,status = 0 });
        }

        public void SetPost(Post post)
        {
            _connection.Execute($@"INSERT INTO Post (UID, Likes, Date, PostText,PostTitle,ForumId) Values (@{nameof(post.UID)}, @{nameof(post.Likes)}, @Date, @{nameof(post.PostText)},@{nameof(post.PostTitle)},@{nameof(post.ForumId)})", new {  post.UID,  post.Likes, Date = DateTime.Now.Date, post.PostText,  post.PostTitle,  post.ForumId });
        }

        public int SetForum(AdminCreateViewModel model)
        {
            var aa = _connection.Query<Forum>($@"SELECT * FROM Forum WHERE FName = @{nameof(model.newForum.FName)} ", new {  model.newForum.FName });
            if (aa.Count() == 0)
            {
                var ee = _connection.Query<int>($@"INSERT INTO Forum (FName, FSubscribers, FPosts,HasLock) Values (@{nameof(model.newForum.FName)}, @FSubscribers, @FPosts,@{nameof(model.newForum.HasLock)});SELECT CAST(SCOPE_IDENTITY() as int)", new { model.newForum.FName,  FSubscribers = 0, FPosts = 1 , model.newForum.HasLock }).SingleOrDefault();
                _connection.Execute($@"INSERT INTO Post (UID, Likes, Date, PostText,PostTitle,ForumId) Values (@{nameof(model.initPost.UID)}, @{nameof(model.initPost.Likes)}, @Date, @{nameof(model.initPost.PostText)},@PostTitle,@{nameof(model.newForum.FName)})", new { model.newForum.FName, model.initPost.UID, Date = DateTime.Now,  PostTitle = "Forum Description", model.initPost.PostText, model.initPost.Likes });
                return 1;
            }
            return 0;
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _connection.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DataUpload() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        
        #endregion
    }
}
