using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Baryon.Models;
using Dapper;

namespace Baryon.Data
{
    public class DataEdit : IDataEd
    {
        private IDbConnection _connection;

        public DataEdit(IDbConnection connection)
        {
            _connection = connection;
        }

        public void LikeComment(int id) 
        {
            //Updates likes by one in specified comment
            _connection.Execute($@"UPDATE Comments SET Likes = (Likes +1)
                                           WHERE CommentId = @{nameof(id)}", new { id });
        }

        public void LikePost(int id)
        {   //Updates likes by one in specified post
            _connection.Execute($@"UPDATE Post SET Likes = (Likes +1) 
                                            WHERE PostId = @{nameof(id)}", new { id });
        }

        public void RemoveAllPost(string forumId)
        {
            //Loads all the post that match the forum ID to remove all the comments from each post
            var posts = _connection.Query<Post>($@"SELECT * FROM Post 
                                                            WHERE ForumId = @{nameof(forumId)} ", new { forumId });
            foreach (var post in posts)
            {
                _connection.Execute($@"DELETE FROM Comments 
                                              WHERE Thread = @{nameof(post.PostId)}", new { post });
            }
            //Removes all posts from the database
            _connection.Execute($@"DELETE FROM Post 
                                          WHERE ForumId = @{nameof(forumId)}", new { forumId });
        }

        public void RemoveComment(Comment comment)
        { 
            _connection.Execute($@"DELETE FROM Comments 
                                           WHERE CommentId = @{nameof(comment.CommentId)}",  new { comment });
        }

        public void RemoveForum(Forum forum)
        {
            //Removes all post related to forum using RemoveAllPost method before removing forum from database
            RemoveAllPost(forum.FName);
            _connection.Execute($@"DELETE FROM Forum 
                                          WHERE FId = @{forum.FId}", forum);
        }

        public void RemovePost(Post post)
        {
            //Removes selected post from database, will not allow for the initial descriptive 
            //post to be removed
            if (post.PostTitle != "Forum Description")
            {
                var found = _connection.QueryFirstOrDefault<Post>($@"SELECT * FROM Post WHERE PostId = @{post.PostId} ", post);
                if (found.PostTitle != "Forum Description")
                {
                    _connection.Execute($@"DELETE FROM Comments 
                                                  WHERE Thread = @{post.PostId}", post);
                    _connection.Execute($@"DELETE FROM Post WHERE PostId = @{post.PostId}", post);
                }
            }
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
        // ~DataEdit() {
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
