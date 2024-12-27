namespace Bonded.Models.ViewModels
{
    public class ChatViewModel
    {
        public int ChatId { get; set; }
        public User UserONE { get; set; }
        public User UserTWO { get; set; }
        public List<ConvoViewModel> Messages { get; set; }
    }
    public class ChatViewModel1
    {
        public int ChatId { get; set; }
        public User UserONE { get; set; }
        public User UserTWO { get; set; }
        public List<Convo> Messages { get; set; }
    }

    public class ConvoViewModel
    {
        public Convo Message { get; set; }
        public string SenderId { get; set; }    
        public string ReceiverId { get; set; }  
        public User Sender { get; set; }
        public User Receiver { get; set; }
    }

}

