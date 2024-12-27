namespace Bonded.Models
{
    public interface ILikeRepository
    {
        // Asynchronous method to check if a user has liked a specific post
        Task<bool> IsPostLikedByUserAsync(string userId, int postId);

        // Asynchronous method to get the count of likes for a specific post
        Task<int> GetLikeCountAsync(int postId);

        // Asynchronous method to add a like to a post
        Task LikePostAsync(int postId, string userId);

        // Asynchronous method to remove a like from a post
        Task UnlikePostAsync(int postId, string userId);

        // Synchronous method to check if a user has liked a specific post
        bool IsPostLikedByUser(int postId, string userId);

        // Asynchronous method to get the list of post IDs liked by the user
        Task<List<int>> GetLikedPostIdsAsync(string userId);
        void DeleteLikesOfAPost(int postId);
    }
}


















//using Bonded.Models;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//public interface ILikeRepository
//{

//    void LikePost(int userId, int postId);
//    void UnlikePost(int userId, int postId);
//    Task<bool> IsPostLikedByUserAsync(int userId, int postId);
//    bool IsPostLikedByUser(int userId, int postId);
//    Task<int> GetLikeCountAsync(int postId);

//    //Task<Post> GetPostDetails(int postId);
//}

