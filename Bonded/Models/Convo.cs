namespace Bonded.Models
{
    public class Convo
    {
        public int Id { get; set; }

        // Foreign key for the chat
        public int ChatId { get; set; }

        public User Sender { get; set; }
        public User Receiver { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }


        public string Text { get; set; }
        public DateTime Timestamp { get; set; }



    }
}
