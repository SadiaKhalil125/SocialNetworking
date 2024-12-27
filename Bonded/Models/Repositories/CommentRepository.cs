using Bonded.Data;
using Bonded.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Bonded.Models
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add a new comment to the database asynchronously
        public async Task<bool> AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);  // Add the comment to the DbSet
            int result = await _context.SaveChangesAsync();  // Save changes to the database
            return result > 0;
        }

        // Retrieve all comments for a specific post asynchronously
        public async Task<List<Comment>> ViewCommentsAsync(int postId)
        {
            var comments = await _context.Comments
                                          .Where(c => c.PostId == postId)
                                          .ToListAsync();  // Get all comments for the post
            return comments;
        }

        // Retrieve comments with user details asynchronously
        public async Task<List<CommentsDetailViewModel>> ViewCommentsWithUserDetailsAsync(int postId)
        {
            var commentsWithUserDetails = await _context.Comments
                                                        .Where(c => c.PostId == postId)
                                                        .Select(c => new CommentsDetailViewModel
                                                        {
                                                            Comment = c,
                                                            User = _context.Users.FirstOrDefault(u => u.Id == c.UserId) // Include user details
                                                        })
                                                        .ToListAsync();  // Get comments with user details
            return commentsWithUserDetails;
        }
        public async Task DeleteComment(int CommentId)
        {
            var Comment = await _context.Comments.Where(c => c.Id == CommentId).FirstOrDefaultAsync();
            if (Comment != null)
            {
                _context.Comments.Remove(Comment);
            }
            _context.SaveChanges();
        }
        public void DeleteCommentsOfAPost(int postId)
        {
            var comments = _context.Comments.Where(c => c.PostId == postId).ToList();
            foreach (var comment in comments)
            {
                _context.Remove(comment);
            }
            _context.SaveChanges();
        }

    }
}

















//using Bonded.Models.Interfaces;
//using Microsoft.Data.SqlClient;
//using Bonded.Models.ViewModels;
//namespace Bonded.Models
//{
//    public class CommentRepository:ICommentRepository
//    {
//        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PassionConnect;Integrated Security=True";

//        // Get all notifications for a specific user
//        public bool AddComment(Comment comment)
//        {
//            int r = 0;
//            using (var connection = new SqlConnection(connectionString))
//            {
//                connection.Open();
//                var query = "INSERT INTO Comments (UserId,Content,PostId) VALUES (@userid,@content,@postid)";
//                using (var command = new SqlCommand(query, connection))
//                {
//                    command.Parameters.AddWithValue("@userid", comment.UserId);
//                    command.Parameters.AddWithValue("@content", comment.Content);
//                    command.Parameters.AddWithValue("@postid", comment.PostId);
//                    r = command.ExecuteNonQuery();
//                }
//            }
//            return r>0;
//        }
//        public List<CommentsDetailViewModel> ViewComments(int postId)
//        {
//            UserRepository userRepository = new UserRepository();   
//            List<CommentsDetailViewModel> comments = new List<CommentsDetailViewModel>(); 
//            using (var connection = new SqlConnection(connectionString))
//            {
//                connection.Open();
//                var query = "SELECT * FROM Comments WHERE PostId = @PostId";
//                using (var command = new SqlCommand(query, connection))
//                {

//                    command.Parameters.AddWithValue("@Postid", postId);
//                    SqlDataReader reader = command.ExecuteReader();
//                    while (reader.Read())
//                    {
//                        CommentsDetailViewModel comment = new CommentsDetailViewModel();
//                        Comment comm = new Comment(){ Id = reader.GetInt32(0),UserId = reader.GetInt32(1),Content=reader.GetString(2),PostId=reader.GetInt32(3) };
//                        User user = userRepository.GetUserById(reader.GetInt32(1));
//                        comment.Comment = comm;
//                        comment.User = user;
//                        comments.Add(comment);
//                    }

//                }
//            }


//            return comments;
//        }

//    }
//}

