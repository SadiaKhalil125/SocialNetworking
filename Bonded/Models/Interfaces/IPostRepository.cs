using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bonded.Models.Interfaces
{
    public interface IPostRepository
    {
        // Retrieve all posts by a specific user
        //List<Post> ShowPosts(string userId);
        Task<List<Post>> ShowPostsAsync(string userId);
        // Retrieve a single post by its ID
        Post GetPostById(int postId);

        // Async version of retrieving a single post by its ID
        Task<Post> GetPostByIdAsync(int postId);

        // Add a new post
        void StorePost(Post newPost);

        // Async version of adding a new post
        Task StorePostAsync(Post newPost);

        // Get the user ID associated with a specific post ID
        string GetUserIdByPostId(int postId);
        Task DeletePostAsync(int postId);
        public void EditPost(int postId, Post updatedPost);
        Task EditPostAsync(int postId, Post updatedPost);
        // Async version of retrieving user ID by post ID
        Task<string> GetUserIdByPostIdAsync(int postId);
    }
}



//namespace Bonded.Models.Interfaces
//{
//    public interface IPostRepository
//    {
//        public List<Post> showPosts(int userId);
//        public Post GetPostById(int postId);
//        public void StorePost(Post newPost);
//        Task<Post> GetPostByIdAsync(int postId);
//        int GetUserIdByPostId(int postId);
//    }
//}
