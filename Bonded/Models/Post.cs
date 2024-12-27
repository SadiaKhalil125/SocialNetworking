namespace Bonded.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PosterId { get; set; }
        public User Poster { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsPrivate { get; set; }
      

        
    }
}
