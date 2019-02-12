using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Baryon.Models;
using Dapper;

namespace Baryon.Data
{
    public class DataLoad : IDataLd
    {
        private IDbConnection _connection;

        public DataLoad(IDbConnection connection)
        {
            _connection = connection; 
        }
        public bool CheckLock(string forumId)
        {
            //Boolean check to see if Forum is accessible without loggin 
            var forum = _connection.QueryFirstOrDefault<Forum>($@"SELECT * FROM Forum 
                                                                            WHERE FName = @{nameof(forumId)}", new { forumId });
            return forum.HasLock;
        }

        public IEnumerable<Forum> GetAll()
        {
            return _connection.Query<Forum>("SELECT * FROM Forum WHERE FName IS NOT null");
        }

        public Comment GetComment(int cId)
        {
            return _connection.QueryFirstOrDefault<Comment>($@"SELECT * FROM Comments 
                                                                        WHERE CommentId = @{nameof(cId)}", new { cId });
        }

        public IEnumerable<Comment> GetComments(int threadId)
        {
            return _connection.Query<Comment>($@"SELECT * FROM Comments 
                                                          WHERE Thread = @{nameof(threadId)} ", new { threadId });
        }

        public FAccess GetFA(string fID, string user)
        {
            //Looks through Formum Access database to check and see if the user permission is stored for the forum being accessed 
            var permissions = _connection.Query<FAccess>($@"SELECT * FROM FAcces WHERE ForumID = @{nameof(fID)}", new { fID });
            foreach (var permission in permissions)
            {
                if (permission.UID == user)
                { return permission; }
            }
            return new FAccess { UID = "Out of Luck" };
        }

        public Forum GetForum(string fname)
        {
            return _connection.QueryFirstOrDefault<Forum>($@"SELECT * FROM Forum 
                                                                      WHERE FName = @{nameof(fname)} ", new { fname });
        }

        public Post GetPost(int postId)
        {
            return _connection.QueryFirstOrDefault<Post>($@"SELECT * FROM Post 
                                                                     WHERE PostId = @{nameof(postId)}", new { postId });
        }

        public IEnumerable<Post> GetPosts(string forumId)
        {
            return _connection.Query<Post>($@"SELECT * FROM Post 
                                                       WHERE ForumId = @{nameof(forumId)} ", new { forumId });
        }

        public string GetUser(string id)
        {
            return _connection.QueryFirstOrDefault<string>($@"SELECT UserName FROM ApplicationUser 
                                                                              WHERE NormalizedEmail = @{nameof(id)}", new { id });
        }

        public bool HasForumAccess(string fID, string user)
        {
            var permissions = _connection.Query<FAccess>($@"SELECT * FROM FAcces 
                                                           WHERE ForumID = @{nameof(fID)}",  new { fID });
            foreach (var permission in permissions)
            {
                if (permission.UID == user)
                { return true; }
            }
            return false;
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
        // ~DataLoad() {
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
