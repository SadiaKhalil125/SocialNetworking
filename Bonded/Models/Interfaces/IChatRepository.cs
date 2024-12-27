
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bonded.Models
{
    public interface IChatRepository
    {
       
        Task SendMessageAsync(int chatId, string senderId, string receiverId, string text);
        Task<Chat> GetChatsForUserAsync(string userId);
        Chat GetOrCreateChat(string senderId, string receiverId);
        List<Convo> GetConvoForAChat(int chatId);
        Task<List<Chat>> GetChatAsync(string userIdValue);
       
        void DeleteMessage(int msgId);
        Convo GetMessageById(int msgId);
        void DeleteChat(int chatId);
        void DeleteMessagesOfAChat(int chatId);


    }
}
