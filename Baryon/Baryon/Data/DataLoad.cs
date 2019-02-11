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
                                                                            WHERE FName = @{forumId}", forumId);
            return forum.HasLock;
        }

        public IEnumerable<Forum> GetAll()
        {
            return _connection.Query<Forum>("SELECT * FROM Forum WHERE FName IS NOT null");
        }

        public Comment GetComment(int cId)
        {
            return _connection.QueryFirstOrDefault<Comment>($@"SELECT * FROM Comments 
                                                                        WHERE CommentId = @{cId}", cId);
        }

        public IEnumerable<Comment> GetComments(int threadId)
        {
            return _connection.Query<Comment>($@"SELECT * FROM Comments 
                                                          WHERE Thread = @{threadId} ",  threadId);
        }

        public FAccess GetFA(string fID, string user)
        {
            //Looks through Formum Access database to check and see if the user permission is stored for the forum being accessed 
            var permissions = _connection.Query<FAccess>("SELECT * FROM FAcces WHERE ForumID = @Fid", new { Fid = fID });
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
                                                                      WHERE FName = @{fname} ", fname);
        }

        public Post GetPost(int postId)
        {
            return _connection.QueryFirstOrDefault<Post>($@"SELECT * FROM Post 
                                                                     WHERE PostId = @{postId}", postId);
        }

        public IEnumerable<Post> GetPosts(string forumId)
        {
            return _connection.Query<Post>($@"SELECT * FROM Post 
                                                       WHERE ForumId = @{forumId} ", forumId);
        }

        public string GetUser(string id)
        {
            return _connection.QueryFirstOrDefault<string>($@"SELECT UserName FROM ApplicationUser 
                                                                              WHERE NormalizedEmail = @{id}", id);
        }

        public bool HasForumAccess(string fID, string user)
        {
            var permissions = _connection.Query<FAccess>($@"SELECT * FROM FAcces 
                                                           WHERE ForumID = @{fID}",  fID);
            foreach (var permission in permissions)
            {
                if (permission.UID == user)
                { return true; }
            }
            return false;
        }
    }
}
