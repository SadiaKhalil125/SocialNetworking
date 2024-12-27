namespace Bonded.Models.ViewModels
{
    public class CommentsDetailViewModel
    {
        public User User {  get; set; }
        public Comment Comment { get; set; }
        public bool CanUserDelete { get; set; }

    }
}
