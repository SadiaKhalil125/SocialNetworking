namespace Bonded.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RelatedId { get; set; }
        public User User { get; set; }
    }
}
