//using Bonded.Models;
namespace Bonded.Models
{
    public interface IFollowRepository
    {
        Task<bool> FollowUserAsync(string followerId, string followingId);
        Task<bool> UnfollowUserAsync(string followerId, string followingId);
        Task<bool> ToggleFollowAsync(string followerId, string followingId);
        Task<bool> IsFollowingAsync(string followerId, string followingId);
        Task<List<string>> GetFollowingIdsAsync(string userId);
        Task<List<string>> GetFollowersIdsAsync(string userId);
        Task<int> GetFollowersCountAsync(string userId);
        Task<int> GetFollowingCountAsync(string userId);
    }
}





//using System.Collections.Generic;
//using System.Threading.Tasks;


//public stringerface IFollowRepository
//{
//    Task<bool> FollowUser(string followerId, string followingId);
//    Task<bool> UnfollowUser(string followerId, string followingId);
//    List <string> GetFollowingIds(string userId);
//    List<string> GetFollowersIds(string userId);
//    Task<bool> IsFollowingAsync(string followerId, string followingId);
//    Task<bool> ToggleFollow(string followerId, string followingId);
//    string GetFollowersCount(string userId);
//    string GetFollowingCount(string userId);
//    //Task<IEnumerable<User>> GetFollowers(string userId); // Get all followers of a user
//    //Task<IEnumerable<User>> GetFollowing(string userId); // Get all users a person is following
//}
