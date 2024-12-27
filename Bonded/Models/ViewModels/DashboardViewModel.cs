namespace Bonded.Models.ViewModels
{
    public class DashboardViewModel
    {
        public User User { get; set; }
        public List<Notification> Notifications { get; set; }
        public List<User> Followings { get; set; }
        public List<User> Followers { get; set; }

    }
}
