using Bonded.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bonded.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _repository;
        public NotificationController(INotificationRepository repository)
        {
            _repository = repository;
        }

        // Show all notifications for a user
        [HttpGet]
        public async Task<IActionResult> AllNotificationsAsync()
        {
            string? userId = HttpContext.Session.GetString("UserId");
            string userIdValue = userId ?? "";
            if (userIdValue == "")
            {
                TempData["ErrorMessage"] = "Signin to view more!";
                return RedirectToAction("Login", "User");
            }

            var notifications = await _repository.GetAllNotificationsAsync(userIdValue);
            return View(notifications);
        }

        [HttpGet]
        public async Task<IActionResult> TodayAsync()
        {
            string? userId = HttpContext.Session.GetString("UserId");
            string userIdValue = userId ?? "";
            if (userId == "")
            {
                TempData["ErrorMessage"] = "Signin to view more!";
                return RedirectToAction("Login", "User");
            }

            var notifications = await _repository.GetTodayNotificationsAsync(userIdValue);
            return View(notifications);
        }
        [HttpGet]
        public IActionResult DeleteNotification(int id)
        {
            _repository.DeleteNotification(id);
            return RedirectToAction("AllNotifications", "Notification");
        }




    }
}
