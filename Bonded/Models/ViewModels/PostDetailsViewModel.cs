namespace Bonded.Models.ViewModels
{
    public class PostDetailsViewModel
    {
       
            public Post Post { get; set; }
            public int LikeCount { get; set; }
            public bool LikedByUser { get; set; }
            public bool LoginnedUser{  get; set; }
    

    }
}
