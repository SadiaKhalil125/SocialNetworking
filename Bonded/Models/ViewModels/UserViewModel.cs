namespace Bonded.Models.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string ProfilePicture { get; set; }
        public string Bio { get; set; }
        public bool IsAdmin { get; set; } = false;
        //public ICollection<Post> Posts { get; set; }
        public bool IsFollowing { get; set; }
        public int FollowingCount {  get; set; }
        public int FollowerCount {  get; set; }
        public List<Notification> Notifications { get; set; }

    }
}
