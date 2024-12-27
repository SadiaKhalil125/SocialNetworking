using Bonded.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bonded.Models
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all notifications for a specific user
        public async Task<List<Notification>> GetAllNotificationsAsync(string userId)
        {
            return await _context.Notifications
                                 .Where(n => n.UserId == userId)
                                 .OrderByDescending(n => n.CreatedAt)
                                 .ToListAsync(); // Get all notifications ordered by CreatedAt
        }

        // Get notifications for the current day for a specific user
        public async Task<List<Notification>> GetTodayNotificationsAsync(string userId)
        {
            var today = DateTime.UtcNow.Date;
            return await _context.Notifications
                                 .Where(n => n.UserId == userId && n.CreatedAt.Date == today)
                                 .OrderByDescending(n => n.CreatedAt)
                                 .ToListAsync(); // Get notifications for today
        }

        // Add a new notification
        public async Task AddNotificationAsync(string userId, string message, int relatedId)
        {
            // Create the Notification object with provided parameters
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                CreatedAt = DateTime.UtcNow,  // Set the current time
                RelatedId = relatedId
            };

            await _context.Notifications.AddAsync(notification);  // Add the notification to the DbSet
            await _context.SaveChangesAsync();  // Save changes to the database
        }
        public async Task DeleteNotificationForUnfollowAsync(string userId, int unfollowedUserId)
        {
            var notifications = await _context.Notifications
                                              .Where(n => n.UserId == userId && n.RelatedId == unfollowedUserId)
                                              .ToListAsync();

            if (notifications.Any())
            {
                _context.Notifications.RemoveRange(notifications);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteNotificationForUnlikePostAsync(string userId, int postId)
        {
            var notifications = await _context.Notifications
                                              .Where(n => n.UserId == userId && n.RelatedId == postId)
                                              .ToListAsync();

            if (notifications.Any())
            {
                _context.Notifications.RemoveRange(notifications);
                await _context.SaveChangesAsync();
            }
        }

        public void DeleteNotification(int id)
        {
            var n = _context.Notifications.Where(n => n.Id == id).FirstOrDefault();
            if (n != null)
            {
                _context.Remove(n);
            }
            _context.SaveChanges();
        }
        public async Task<List<Notification>> GetLatest10Notifications(string userId)
        {
            var notifications = await _context.Notifications
              .Where(n => n.UserId == userId)
              .OrderByDescending(n => n.CreatedAt)
              .Take(10) // Latest 10 notifications
              .ToListAsync();
            return notifications;
        }
    }
}
























//using System;
//using System.Collections.Generic;
//using Microsoft.Data.SqlClient;
//using System.Linq;
//using Bonded.Models.Interfaces;

//namespace Bonded.Models
//{
//    public class NotificationRepository:INotificationRepository
//    {
//        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PassionConnect;Integrated Security=True";

//        // Get all notifications for a specific user
//        public List<Notification> GetAllNotifications(int userId)
//        {
//            var notifications = new List<Notification>();
//            using (var connection = new SqlConnection(connectionString))
//            {
//                connection.Open();
//                var query = "SELECT * FROM Notifications WHERE UserId = @UserId ORDER BY CreatedAt DESC";
//                using (var command = new SqlCommand(query, connection))
//                {
//                    command.Parameters.AddWithValue("@UserId", userId);
//                    using (var reader = command.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            notifications.Add(new Notification
//                            {
//                                Id = reader.GetInt32(0),
//                                UserId = reader.GetInt32(1),
//                                Message = reader.GetString(2),
//                                CreatedAt = reader.GetDateTime(3),
//                                RelatedId = reader.GetInt32(4)
//                            });
//                        }
//                    }
//                }
//            }
//            return notifications;
//        }

//        // Get notifications for the current day for a specific user
//        public List<Notification> GetTodayNotifications(int userId)
//        {
//            var notifications = new List<Notification>();
//            using (var connection = new SqlConnection(connectionString))
//            {
//                connection.Open();
//                var query = @"
//                    SELECT * FROM Notifications 
//                    WHERE UserId = @UserId AND CAST(CreatedAt AS DATE) = CAST(GETDATE() AS DATE)
//                    ORDER BY CreatedAt DESC";
//                using (var command = new SqlCommand(query, connection))
//                {
//                    command.Parameters.AddWithValue("@UserId", userId);
//                    using (var reader = command.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            notifications.Add(new Notification
//                            {
//                                Id = reader.GetInt32(0),
//                                UserId = reader.GetInt32(1),
//                                Message = reader.GetString(2),
//                                CreatedAt = reader.GetDateTime(3),
//                                RelatedId = reader.GetInt32(4)
//                            });
//                        }
//                    }
//                }
//            }
//            return notifications;
//        }

//        // Add a new notification
//        public void AddNotification(int userId, string message , int relatedId)
//        {

//            using (var connection = new SqlConnection(connectionString))
//            {
//                connection.Open();
//                var query = "INSERT INTO Notifications (UserId, Message,RelatedId) VALUES (@UserId, @Message,@Relatedid)";
//                using (var command = new SqlCommand(query, connection))
//                {
//                    command.Parameters.AddWithValue("@UserId", userId);
//                    command.Parameters.AddWithValue("@Message", message);
//                    command.Parameters.AddWithValue("@Relatedid", relatedId);
//                    command.ExecuteNonQuery();
//                }
//            }
//        }
//    }
//}
