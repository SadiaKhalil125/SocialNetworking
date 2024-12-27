using Microsoft.EntityFrameworkCore;

namespace Bonded.Models
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetAllNotificationsAsync(string userId);
        Task<List<Notification>> GetTodayNotificationsAsync(string userId);
        Task AddNotificationAsync(string userId, string message, int relatedId);
        Task DeleteNotificationForUnfollowAsync(string userId, int unfollowedUserId);
        Task DeleteNotificationForUnlikePostAsync(string userId, int postId);
        void DeleteNotification(int id);
        Task<List<Notification>> GetLatest10Notifications(string userId);
      

    }
}






//using Bonded.Models;
//using System.Collections.Generic;

//namespace Bonded.Models.Interfaces
//{
//    public interface INotificationRepository
//    {
//        public List<Notification> GetAllNotifications(int userId);
//        public List<Notification> GetTodayNotifications(int userId);
//        public void AddNotification(int userId, string message, int relatedId);
//    }
//}
