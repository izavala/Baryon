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
                                           WHERE CommentId = @{id}", id);
        }

        public void LikePost(int id)
        {   //Updates likes by one in specified post
            _connection.Execute($@"UPDATE Post SET Likes = (Likes +1) 
                                            WHERE PostId = @{id}", id);
        }

        public void RemoveAllPost(string forumId)
        {
            //Loads all the post that match the forum ID to remove all the comments from each post
            var posts = _connection.Query<Post>($@"SELECT * FROM Post 
                                                            WHERE ForumId = @{forumId} ",  forumId);
            foreach (var post in posts)
            {
                _connection.Execute($@"DELETE FROM Comments 
                                              WHERE Thread = @{post.PostId}", post);
            }
            //Removes all posts from the database
            _connection.Execute($@"DELETE FROM Post 
                                          WHERE ForumId = @{forumId}", forumId);
        }

        public void RemoveComment(Comment comment)
        { 
            _connection.Execute($@"DELETE FROM Comments 
                                           WHERE CommentId = @{comment.CommentId}",  comment);
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
    }
}
