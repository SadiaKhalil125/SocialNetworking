using Bonded.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Bonded.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
namespace Bonded.Controllers

{
    public class UserController : Controller
    {
        

        private readonly IFollowRepository _followRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly UserManager <User> _userManager;
        private readonly SignInManager <User> _signInManager;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IFollowRepository followRepository,INotificationRepository notificationRepository)//,IUserRepository userRepository, IFollowRepository followRepository)
        {

            _followRepository = followRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _notificationRepository = notificationRepository;
            
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PasswordHash = model.Password,
                    Bio = "Default Bio",
                    ProfilePicture = "data:image/png;base64,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",
                    FollowerCount = 0,
                    FollowingCount = 0,
                    IsAdmin = false

                };
                
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "User");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, "Password must be strong.");
                }
            }
       
            return View(model);
        }
      
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string? userId = HttpContext.Session.GetString("UserId");
                string userIdValue = userId ?? "";
                if(userIdValue !="")
                {
                    ModelState.AddModelError(string.Empty, "Logout current user first!");
                    return View();
                }
                // Attempt to find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);

                // Check if the user exists
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt. User not found.");
                    return View(model);
                }

                // Check if the password is correct
                bool passwordMatch = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!passwordMatch)
                {
                    ModelState.AddModelError(string.Empty, "Password is incorrect.");
                    return View(model);
                }

                // Sign in the user and set the session
                HttpContext.Session.SetString("UserId", user.Id);
                await _signInManager.SignInAsync(user, model.RememberMe);

                // Redirect to the user's profile
                return RedirectToAction("DetailedProfile", "User");
            }

            // If the model state is invalid, return the login view with an error
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }




        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

        //        if (result.Succeeded)
        //        {
        //            var user = await _userManager.FindByEmailAsync(model.Email);

        //            if (user == null)
        //            {
        //                ModelState.AddModelError(string.Empty, "User not found.");
        //                return View(model);
        //            }

        //            HttpContext.Session.SetString("UserId", user.Id); // Ensure Session is configured
        //            return RedirectToAction("Index", "Home");
        //        }

        //        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //    }

        //    return View(model);
        //}


        public async Task<IActionResult> ProfileAsync()
        {
            string? userId = HttpContext.Session.GetString("UserId");
            string userIdValue = userId ?? "";
            
            var user = await _userManager.FindByIdAsync(userIdValue);

            if (user == null)
            {
                return NotFound();
            }
            
            return View("Profile", user);
        }
        [HttpGet]
        public IActionResult Updateprofile()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Updateprofile(string Bio, IFormFile ProfilePicture)
        {

            string ? userId = HttpContext.Session.GetString("UserId");
            string userIdValue = userId ?? "";

            var user = await _userManager.FindByIdAsync(userIdValue);
            
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found! Login again!";
                return RedirectToAction("Login", "User");
            }
            if (!string.IsNullOrEmpty(Bio))
            {

                user.Bio = Bio;
            }
            if (ProfilePicture != null && ProfilePicture.Length > 0)
            {
                try
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePicture.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(imagesDirectory))
                    {
                        Directory.CreateDirectory(imagesDirectory);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfilePicture.CopyToAsync(stream);
                    }

                    user.ProfilePicture = "/images/" + fileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error uploading the profile picture: " + ex.Message);
                    return View(user);
                }
            }
            await _userManager.UpdateAsync(user);
           // _userRepository.UpdateUserAsync(user);

            return RedirectToAction("Profile", "User");
        }
        [HttpGet]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Login", "User");
        }
        [HttpGet]
        public IActionResult DeletedAccount()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DeleteAccount()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(string reason)
        {

            string? userId = HttpContext.Session.GetString("UserId");
            string userIdValue = userId ?? "";
            if (userIdValue == "")
            {
                TempData["ErrorMessage"] = "Signin to view more!";
                return RedirectToAction("Login", "User");
           
            }
            var user = await _userManager.FindByIdAsync(userIdValue);
            if (user != null)
            {
               await _userManager.DeleteAsync(user);
            }
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("DeletedAccount", "User");
        }



        [HttpGet]
        public async Task<IActionResult> DetailedProfileAsync()
        {
            string? userId = HttpContext.Session.GetString("UserId");
            string userIdValue = userId ?? "";
            if (userIdValue == "")
            {
                TempData["ErrorMessage"] = "Signin to view more!";
                return RedirectToAction("Login", "User");
            }
            var user = await _userManager.FindByIdAsync(userIdValue);

            int FollowerCount = await _followRepository.GetFollowersCountAsync(userIdValue);
            int FollowingCount = await _followRepository.GetFollowingCountAsync(userIdValue);

            User user1 = new User
            {
                Id = userIdValue,
                FollowerCount = FollowerCount,
                FollowingCount = FollowingCount,
                UserName = user.UserName,
                Email = user.Email,
                Bio = user.Bio,
                ProfilePicture = user.ProfilePicture
            };
            if (user == null)
            {
                return NotFound();
            }
            return View("DetailedProfile", user1);
        }
        [HttpGet]
        public IActionResult ViewPicture(string pictureUrl)
        {
            return View((object)pictureUrl);
        }
        [HttpGet]
        public async Task<IActionResult> ViewOtherProfiles(string id)
        {
            string? userId = HttpContext.Session.GetString("UserId");
            string userIdValue = userId ?? "";
            if (userIdValue == "")
            {
                TempData["ErrorMessage"] = "Signin to view more!";
                return RedirectToAction("Login", "User");
            }
            UserViewModel user = new UserViewModel();
            var newUser = await _userManager.FindByIdAsync(id);
            if (newUser != null)
            {
                user.Id = id;
                user.Username = newUser.UserName;
                user.Bio = newUser.Bio;
                user.ProfilePicture = newUser.ProfilePicture;
                user.Email = newUser.Email;
                user.IsAdmin = newUser.IsAdmin;
                user.IsFollowing = await _followRepository.IsFollowingAsync(userIdValue, id);
                user.FollowerCount = await _followRepository.GetFollowersCountAsync(id);
                user.FollowingCount = await _followRepository.GetFollowingCountAsync(id);
            }
            ViewData["CurrentUserId"] = userId;

            return View(user);
        }
        
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email, string currentPassword, string newPassword)
        {

         
            var user = await _userManager.FindByEmailAsync(email);
            bool check = await _userManager.CheckPasswordAsync(user, currentPassword);
            if (!check)
            {
                ModelState.AddModelError(string.Empty, "Incorrect Current Password.");
                return View();
            }
            var success = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (success.Succeeded)
            {
                return RedirectToAction("Login");
            }
            else
            {
                // Handle the case where the password reset failed (user not found or error)
                return RedirectToAction("Register");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            string? currentUserId = HttpContext.Session.GetString("UserId");
            string userId = currentUserId ?? "";
            if (userId == "")
            {
                TempData["ErrorMessage"] = "Signin to view more!";
                return RedirectToAction("Login", "User");
            }

            var user = await _userManager.FindByIdAsync(userId);
           
            List<string> followingIds = await _followRepository.GetFollowingIdsAsync(userId);
            List<string> followerIds = await _followRepository.GetFollowersIdsAsync(userId);

            // Fetch the user details for each follower ID
            List<User> followers = new List<User>();
            foreach (var followerId in followerIds)
            {
                var user1 = await _userManager.FindByIdAsync(followerId); // Adjust this call to get user by ID
                if (user1 != null)
                {
                    followers.Add(user1);
                }
            }
            List<User> followings = new List<User>();
            foreach (string id in followingIds)
            {
                followings.Add(await _userManager.FindByIdAsync(id));
            }

            var notifications = await _notificationRepository.GetLatest10Notifications(userId);

            var viewModel = new DashboardViewModel
            {
                User = user,
                Notifications = notifications,
                Followers = followers,
                Followings = followings

            };

            return View(viewModel);
        }

        public JsonResult GetFollowerFollowingCounts()
        {
            string? userId = HttpContext.Session.GetString("UserId");
            string userIdValue = userId ?? "";

            var followers = _followRepository.GetFollowersCountAsync(userIdValue); //_context.Followers.Count(f => f.FollowedUserId == UserId);
            var following = _followRepository.GetFollowingCountAsync(userIdValue);

            return Json(new { followers, following });
        }

        //public IActionResult ViewOtherProfiles(int userId)
        //{
        //    User user = _userRepository.GetUserById(userId);
        //    return View(user);
        //}
        //public async Task<IActionResult> ViewOtherProfiles(int userId, int ? currentUserId)
        //{

        //    currentUserId = HttpContext.Session.GetInt32("UserId");
        //    if (currentUserId == null)
        //    {
        //        return BadRequest("User is not logged in.");
        //    }


        //    User user = await _userRepository.GetUserById();

        //    user.IsFollowing = await _followRepository.IsFollowingAsync(currentUserId, id);

        //    ViewData["CurrentUserId"] = currentUserId;

        //    return View(user);
        //    //User user =  _userRepository.GetUserById(userId);
        //    //bool isFollowing = await _followRepository.IsFollowing(currentUserId, userId);

        //    //UserViewModel viewModel = new UserViewModel
        //    //{
        //    //    Id = user.Id,
        //    //    Username = user.Username,
        //    //    ProfilePicture = user.ProfilePicture,
        //    //    Bio = user.Bio,
        //    //    IsFollowing = isFollowing
        //    //};

        //    //return View(viewModel);
        //}

    }
}
