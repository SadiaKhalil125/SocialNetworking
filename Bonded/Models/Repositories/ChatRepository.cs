using Bonded.Data;
using Bonded.Models;
using Microsoft.EntityFrameworkCore;
namespace Bonded.Models
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all chats for a specific user
        public async Task<Chat> GetChatsForUserAsync(string userId)
        {
            return await _context.Chats
                .Where(c => c.UserOneId == userId || c.UserTwoId == userId).FirstOrDefaultAsync();
        }


        //Get a specific chat with messages
        public async Task<List<Chat>> GetChatAsync(string userIdValue)
        {
            return await _context.Chats
                .Where(c => (c.UserOneId == userIdValue) || (c.UserTwoId == userIdValue))
                .ToListAsync();
        }
        
        public List<Convo> GetConvoForAChat(int chatId)
        {
            return _context.Convos.Where(c => c.ChatId == chatId).ToList();
        }
        public Chat GetOrCreateChat(string senderId, string receiverId)
        {
            var chat = _context.Chats.Where(c => ((c.UserOneId == senderId) && (c.UserTwoId == receiverId)) || ((c.UserTwoId == senderId) && (c.UserOneId == receiverId))).FirstOrDefault();
            if (chat == null)
            {
                chat = new Chat { UserOneId = senderId, UserTwoId = receiverId };
                _context.Chats.Add(chat);
                _context.SaveChanges();
                chat.Id = _context.Chats.Where(c => ((c.UserOneId == senderId) && (c.UserTwoId == receiverId)) || ((c.UserTwoId == senderId) && (c.UserOneId == receiverId))).FirstOrDefaultAsync().Id;
            }
            return chat;
        }

        // Send a new message
        public async Task SendMessageAsync(int chatId, string senderId, string receiverId, string text)
        {
            Convo message = new Convo();

            message.ChatId = chatId;
            message.SenderId = senderId;
            message.ReceiverId = receiverId;
            message.Text = text;
            message.Timestamp = DateTime.UtcNow;


            await _context.Convos.AddAsync(message);
            await _context.SaveChangesAsync();
        }
        public void DeleteMessage(int msgId)
        {
            var Convo = _context.Convos.Where(c => c.Id == msgId).FirstOrDefault();

            if (Convo != null)
            {

                _context.Convos.Remove(Convo);
                _context.SaveChanges();

            }

        }
        public Convo GetMessageById(int msgId)
        {
            var message = _context.Convos.Where(c => c.Id == msgId).FirstOrDefault();
            if (message != null)
            {
                return message;
            }
            else
            {
                message = new Convo();
                return message;
            }
        }
        public void DeleteChat(int chatId)
        {
            var chat = _context.Chats.Where(c=>c.Id == chatId).FirstOrDefault();
            if (chat != null)
            {
                _context.Chats.Remove(chat);
                _context.SaveChanges();
            }
        }
        public void DeleteMessagesOfAChat(int chatId)
        {
            var chats = _context.Convos.Where(c => c.ChatId == chatId).ToList();
            foreach (var chat in chats)
            {
                _context.Convos.Remove(chat);
                _context.SaveChanges();
            }
            
        }

    }
}
