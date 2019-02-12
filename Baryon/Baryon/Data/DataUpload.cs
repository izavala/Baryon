using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Baryon.Models;
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
                                                                     WHERE UID = @{nameof(user)}", user);

            foreach (var permission in permissions)
            {
                if (permission.ForumID == forum)
                    return false;
            }
            _connection.Execute("INSERT INTO FAcces (ForumID, UID) Values (@{forum}, @{user})", new { forum, user } );
            return true;
        }

        public int SetComment(Comment comment)
        {
            //Sets comment and returns the unique comment Id 
            _connection.Query<int>($@"INSERT INTO Comments 
                               (Thread, Text, Likes, UID, Date)
                                Values (@{nameof(comment.Thread)}, @{nameof(comment.Text)}, @{nameof(comment.Likes)}, @{nameof(comment.UID)},@{nameof(comment.Date)});
                                SELECT CAST(SCOPE_IDENTITY() as int)", new {  comment.Thread,  comment.Text, comment.Likes,  comment.UID, Date = DateTime.Now.Date }).SingleOrDefault();

            return comment.Thread;
        }

        public void SetModRequest(string request, string _user)
        {
            _connection.Execute($@"INSERT INTO Request(ForumID, UID, Pending , Status)
                                Values( @{request}, @{_user}, @{true}, @{false})", new { request, _user });
        }

        public void SetPost(Post post)
        {
            _connection.Execute($@"INSERT INTO Post (UID, Likes, Date, PostText,PostTitle,ForumId) Values (@{nameof(post.UID)}, @{nameof(post.Likes)}, @Date, @{nameof(post.PostText)},@{nameof(post.PostTitle)},@{nameof(post.ForumId)})", new {  post.UID,  post.Likes, Date = DateTime.Now.Date, post.PostText,  post.PostTitle,  post.ForumId });
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
