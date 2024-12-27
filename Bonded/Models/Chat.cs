namespace Bonded.Models
{
    public class Chat
    {
      

        public int Id { get; set; }
        public User UserOne { get; set; }
        public User UserTwo { get; set; }
        public string UserOneId { get; set; }
        public string UserTwoId { get; set; }
        public List<Convo> Messages {get; set;}
    }
}
