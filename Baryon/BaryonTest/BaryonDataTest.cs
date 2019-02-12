using Baryon.Data;
using Baryon.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace BaryonTest
{
    public class BaryonDataTest : IDisposable
    {
        private readonly DataUpload  dataUpload = new DataUpload(new SqlConnection(TestConstants.ConnectionString));
        private readonly DataLoad dataLoad = new DataLoad(new SqlConnection(TestConstants.ConnectionString));
        private readonly DataEdit dataEdit = new DataEdit(new SqlConnection(TestConstants.ConnectionString));
        [Fact (DisplayName = "Test upload post")]
        public void TestPostUpload()
        {
            var testPost = new Post{ PostId = 1, UID = "me@me.com",PostText = "Test text",Likes = 0 ,PostTitle = "Test Title",ForumId = "Test Forum" , Date = DateTime.Now.Date};
            var resultPost = dataLoad.GetPost(1);
            Assert.Equal(testPost.UID, resultPost.UID);
            Assert.Equal(testPost.PostText, resultPost.PostText);
            Assert.Equal(testPost.PostTitle, resultPost.PostTitle);
            Assert.Equal(testPost.Likes, resultPost.Likes);
        }                    

        [Fact]
        public void TestForumUpload()
        {
            var testComment = new Comment {CommentId = 4006, Likes = 0 , Text = "Comment Text", Thread = 1 };
            dataUpload.SetComment(testComment);
            var result = dataLoad.GetComment(4006);
            Assert.Equal(testComment.CommentId , result.CommentId);
            Assert.Equal(testComment.Likes, result.Likes);
            Assert.Equal(testComment.Text, result.Text);
            Assert.Equal(testComment.Thread, result.Thread);
            
            

        }

       

        public void Dispose()
        {
            dataUpload.Dispose();
            dataLoad.Dispose();
        }
    }


}
