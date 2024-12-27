using Bonded.Models;
using Bonded.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Bonded.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager<User> _userManager;

        public HomeController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Search(string searchTerm)
        {

            if (string.IsNullOrEmpty(searchTerm))
            {
                return View(new List<User>());
            }
            searchTerm = searchTerm.ToLower();
            var users = await _userManager.Users
            .Where(u => u.UserName.ToLower().Contains(searchTerm) ||
                   u.Email.ToLower().Contains(searchTerm))
              .ToListAsync();
           // var users = await _userManager.FindByNameAsync(searchTerm);
            return View(users);
        }
        public IActionResult Index()
        {
            string? userId = HttpContext.Session.GetString("UserId");
            string userIdValue = userId ?? "";
            //if (userIdValue == "")
            //{
            //    TempData["ErrorMessage"] = "Signin to view more!";
            //    return RedirectToAction("Login", "User");
            //}
            TempData["Id"] = userIdValue;
            return View();
        }
        public IActionResult GlobalConnections()
        {
            return View();
        }

        public IActionResult ShareIdeas()
        {
            return View();
        }

        public IActionResult GrowTogether()
        {
            return View();
        }


    }
}