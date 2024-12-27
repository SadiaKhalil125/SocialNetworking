using Bonded.Data;
using Microsoft.EntityFrameworkCore;
namespace Bonded.Models
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationDbContext _context;

        public LikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Check if a user has liked a specific post
        public async Task<bool> IsPostLikedByUserAsync(string userId, int postId)
        {
            return await _context.Likes
                .AnyAsync(l => l.UserId == userId && l.PostId == postId);
        }

        // Get the count of likes for a post
        public async Task<int> GetLikeCountAsync(int postId)
        {
            return await _context.Likes
                .Where(l => l.PostId == postId)
                .CountAsync();
        }

        // Add a like to a post
        public async Task LikePostAsync(int postId, string userId)
        {
            var like = new Like
            {
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.Now
            };

            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();
        }

        // Remove a like from a post
        public async Task UnlikePostAsync(int postId, string userId)
        {
            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

            if (like != null)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
            }
        }

        // Check if a user has liked a specific post (sync version)
        public bool IsPostLikedByUser(int postId, string userId)
        {
            return _context.Likes
                .Any(l => l.PostId == postId && l.UserId == userId);
        }

        // Get the list of post IDs liked by the user
        public async Task<List<int>> GetLikedPostIdsAsync(string userId)
        {
            return await _context.Likes
                .Where(l => l.UserId == userId)
                .Select(l => l.PostId)
                .ToListAsync();
        }
        public void DeleteLikesOfAPost(int postId)
        {
            var likes = _context.Likes.Where(l => l.PostId == postId).ToList();
            foreach (var like in likes)
            {
                _context.Remove(like);
            }
            _context.SaveChanges();
        }
    }
}




























//using Bonded.Models;
//using System.Collections.Generic;
//using Microsoft.Data.SqlClient;
//using System.Threading.Tasks;


//public class LikeRepository : ILikeRepository
//{
//    private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PassionConnect;Integrated Security=True";
//    public async Task<bool> IsPostLikedByUserAsync(int userId, int postId)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();

//            var query = "SELECT COUNT(*) FROM Likes WHERE UserId = @UserId AND PostId = @PostId";
//            using (var command = new SqlCommand(query, connection))
//            {
//                command.Parameters.AddWithValue("@UserId", userId);
//                command.Parameters.AddWithValue("@PostId", postId);

//                var count = (int)await command.ExecuteScalarAsync();
//                return count > 0; // Return true if the count is greater than 0
//            }
//        }
//    }

//    public async Task<int> GetLikeCountAsync(int postId)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();

//            var query = "SELECT COUNT(*) FROM Likes WHERE PostId = @PostId";
//            using (var command = new SqlCommand(query, connection))
//            {
//                command.Parameters.AddWithValue("@PostId", postId);

//                return (int)await command.ExecuteScalarAsync(); // Return the count directly
//            }
//        }
//    }
//    public bool IsPostLikedByUser(int postId, int userId)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            connection.Open();
//            string query = "SELECT COUNT(*) FROM Likes WHERE PostId = @PostId AND UserId = @UserId";
//            SqlCommand cmd = new SqlCommand(query, connection);
//            cmd.Parameters.AddWithValue("@PostId", postId);
//            cmd.Parameters.AddWithValue("@UserId", userId);
//            int likeCount = (int)cmd.ExecuteScalar();
//            return likeCount > 0;  // If there's a like, return true, otherwise false
//        }
//    }

//    public void LikePost(int postId, int userId)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            connection.Open();
//            string query = "INSERT INTO Likes (PostId, UserId, CreatedAt) VALUES (@PostId, @UserId, GETDATE())";
//            SqlCommand cmd = new SqlCommand(query, connection);
//            cmd.Parameters.AddWithValue("@PostId", postId);
//            cmd.Parameters.AddWithValue("@UserId", userId);
//            cmd.ExecuteNonQuery();  // Insert the like into the database
//        }
//    }

//    public void UnlikePost(int postId, int userId)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            connection.Open();
//            string query = "DELETE FROM Likes WHERE PostId = @PostId AND UserId = @UserId";
//            SqlCommand cmd = new SqlCommand(query, connection);
//            cmd.Parameters.AddWithValue("@PostId", postId);
//            cmd.Parameters.AddWithValue("@UserId", userId);
//            cmd.ExecuteNonQuery();  // Delete the like from the database
//        }
//    }

//}

