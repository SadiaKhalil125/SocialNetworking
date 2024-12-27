using Microsoft.AspNetCore.Identity;

namespace Bonded.Models
{
    public class User:IdentityUser
    {
        public string ProfilePicture { get; set; }
        public string Bio { get; set; }
        public bool IsAdmin { get; set; } = false;
        //public ICollection<Post> Posts { get; set; }
        public bool isFollowing { get; set; }
        public int FollowerCount { get; set; }
        public int FollowingCount { get; set; }
        public int IdForOtherUse { get; set; }
        public List<Post> Posts { get; set; }

    }
}
