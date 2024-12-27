using Bonded.Data;
using Bonded.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bonded.Models
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PostRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Get all posts by a specific user
        public async Task<List<Post>> ShowPostsAsync(string userId)
        {
            return await _dbContext.Posts// Eagerly load the related User entity
                .Where(p => p.PosterId == userId)
                .ToListAsync();
        }

        // Get a post by its ID
        public Post GetPostById(int postId)
        {
            var post = _dbContext.Posts.Where(p => p.Id == postId).FirstOrDefault();
            if (post != null)
            {
                return post;
            }
            else
            {
                return null;
            }
        }

        // Async version of GetPostById
        public async Task<Post> GetPostByIdAsync(int postId)
        {
            var post =  await _dbContext.Posts.Where(p => p.Id == postId).FirstOrDefaultAsync();
            if (post != null)
            {
                return post;
            }
            else
            {
                return null;
            }
        }

        // Add a new post to the database
        public void StorePost(Post newPost)
        {
            _dbContext.Posts.Add(newPost);
            _dbContext.SaveChanges();
        }

        // Async version of StorePost
        public async Task StorePostAsync(Post newPost)
        {
            await _dbContext.Posts.AddAsync(newPost);
            await _dbContext.SaveChangesAsync();
        }

        // Get the UserId associated with a specific PostId
        public string GetUserIdByPostId(int postId)
        {
            var post = _dbContext.Posts
                .AsNoTracking() // Disable tracking for better performance
                .FirstOrDefault(p => p.Id == postId);

            if (post == null)
            {
                throw new Exception("Post not found");
            }

            return post.PosterId;
        }

        // Async version of GetUserIdByPostId
        public async Task<string> GetUserIdByPostIdAsync(int postId)
        {
            var post = await _dbContext.Posts
                .AsNoTracking() // Disable tracking for better performance
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                throw new Exception("Post not found");
            }

            return post.PosterId;
        }
        public async Task EditPostAsync(int postId, Post updatedPost)
        {
            var existingPost = await _dbContext.Posts
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (existingPost != null)
            {
                existingPost.Content = updatedPost.Content;
                existingPost.ImagePath = updatedPost.ImagePath;


                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Post not found");
            }
        }
        public async Task DeletePostAsync(int postId)
        {
            var post = await _dbContext.Posts
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post != null)
            {
                _dbContext.Posts.Remove(post);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Post not found");
            }
        }
        public void EditPost(int postId, Post updatedPost)
        {
            var post = _dbContext.Posts.FirstOrDefault(p => p.Id == postId);
            if (post != null)
            {
                // Update the content (caption)
                post.Content = updatedPost.Content;

                // Save the changes to the database
                _dbContext.SaveChanges();
            }
        }
    }
}






//using Bonded.Models.Interfaces;
//using Microsoft.AspNetCore.Mvc.ActionConstraints;
//using Microsoft.Data.SqlClient;
//namespace Bonded.Models
//{
//    public class PostRepository : IPostRepository
//    {
//        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PassionConnect;Integrated Security=True";
//        public List<Post> showPosts(int userId)
//        {

//            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
//            {
//                UserRepository uR = new UserRepository();
//                List<Post> posts = new List<Post>();
//                sqlConnection.Open();
//                string query = "SELECT * FROM [Posts] WHERE UserId = @Id";
//                SqlCommand cmd = new SqlCommand(query, sqlConnection);
//                cmd.Parameters.AddWithValue("@Id", userId);

//                SqlDataReader reader = cmd.ExecuteReader();
//                while (reader.Read())
//                {
//                    Post post = new Post
//                    {
//                        Id = reader.GetInt32(0),
//                        UserId = reader.GetInt32(1),
//                        Poster = uR.GetUserById(userId),
//                        Content = reader.GetString(2),
//                        ImagePath = reader.GetString(3),
//                        CreatedAt = reader.GetDateTime(4),
//                        IsPrivate = reader.GetBoolean(5)
//                    };
//                    posts.Add(post);

//                }
//                return posts;
//            }

//        }
//        public Post GetPostById(int postId)
//        {
//            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
//            {

//                UserRepository uR = new UserRepository();

//                sqlConnection.Open();

//                string query = "SELECT * FROM [Posts] WHERE Id = @Id";
//                SqlCommand cmd = new SqlCommand(query, sqlConnection);
//                cmd.Parameters.AddWithValue("@Id", postId);

//                SqlDataReader reader = cmd.ExecuteReader();

//                if (reader.Read())
//                {

//                    Post post = new Post
//                    {
//                        Id = reader.GetInt32(0),
//                        UserId = reader.GetInt32(1),
//                        Poster = uR.GetUserById(reader.GetInt32(1)),
//                        Content = reader.GetString(2),
//                        ImagePath = reader.GetString(3),
//                        CreatedAt = reader.GetDateTime(4),
//                        IsPrivate = reader.GetBoolean(5)
//                    };

//                    return post;
//                }
//            }

//            return null;
//        }
//        public async Task<Post> GetPostByIdAsync(int postId)
//        {
//            try
//            {
//                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
//                {
//                    UserRepository uR = new UserRepository();

//                    await sqlConnection.OpenAsync();  // Asynchronously open the connection

//                    string query = "SELECT * FROM [Posts] WHERE Id = @Id";
//                    SqlCommand cmd = new SqlCommand(query, sqlConnection);
//                    cmd.Parameters.AddWithValue("@Id", postId);

//                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())  // Asynchronously execute the query
//                    {
//                        if (await reader.ReadAsync())  // Asynchronously read the data
//                        {
//                            Post post = new Post
//                            {
//                                Id = reader.GetInt32(0),
//                                UserId = reader.GetInt32(1),
//                                Poster = await uR.GetUserByIdAsync(reader.GetInt32(1)),  // Assuming GetUserByIdAsync is asynchronous
//                                Content = reader.GetString(2),
//                                ImagePath = reader.GetString(3),
//                                CreatedAt = reader.GetDateTime(4),
//                                IsPrivate = reader.GetBoolean(5)
//                            };

//                            return post;  // Return the post object if found
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Log the exception(you can use a logging framework here)
//                Console.WriteLine($"Error fetching post: {ex.Message}");
//            }

//            return null;  // Return null if no post is found or an error occurred
//        }

//        public void StorePost(Post newPost)
//        {
//            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
//            {
//                sqlConnection.Open();

//                string query = "INSERT INTO [Posts] (UserId, Content, ImagePath, CreatedAt, IsPrivate) " +
//                               "VALUES (@UserId, @Content, @ImagePath, @CreatedAt, @IsPrivate)";

//                SqlCommand cmd = new SqlCommand(query, sqlConnection);
//                cmd.Parameters.AddWithValue("@UserId", newPost.UserId);
//                cmd.Parameters.AddWithValue("@Content", newPost.Content);
//                cmd.Parameters.AddWithValue("@ImagePath", newPost.ImagePath);
//                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
//                cmd.Parameters.AddWithValue("@IsPrivate", newPost.IsPrivate);

//                cmd.ExecuteNonQuery();
//            }
//        }
//        public int GetUserIdByPostId(int postId)
//        {
//            using (var connection = new SqlConnection(connectionString))
//            {
//                connection.Open();
//                var query = "SELECT UserId FROM Posts WHERE Id = @PostId";
//                using (var command = new SqlCommand(query, connection))
//                {
//                    command.Parameters.AddWithValue("@PostId", postId);
//                    var result = command.ExecuteScalar();

//                    if (result == null)
//                    {
//                        throw new Exception("Post not found");
//                    }

//                    return Convert.ToInt32(result);
//                }
//            }
//        }


//    }
//}
