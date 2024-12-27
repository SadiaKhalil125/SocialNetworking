using Bonded.Models;
using Bonded.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class LikeController : Controller
{
    private readonly ILikeRepository _likeRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IPostRepository _postRepository;
    private readonly UserManager<User> _userManager;

    public LikeController(ILikeRepository likeRepository, INotificationRepository notificationRepository, IPostRepository postRepository, UserManager<User> userManager)
    {
        _likeRepository = likeRepository;
        _notificationRepository = notificationRepository;
        _postRepository = postRepository;
        _userManager = userManager;

    }

    // Like a post


    [HttpPost]
    public async Task<IActionResult> ToggleLikeAsync(int postId)
    {
        string? userId = HttpContext.Session.GetString("UserId");
        string userIdValue = userId ?? "";
        if (userIdValue == "")
        {
            TempData["ErrorMessage"] = "Signin to view more!";
            return RedirectToAction("Login", "User");
          
        }
        var isLiked = _likeRepository.IsPostLikedByUser(postId, userIdValue);  // Check if post is liked by the current user

        if (isLiked)
        {
            // Unlike the post (remove like)
            await _likeRepository.UnlikePostAsync(postId, userIdValue);
            await _notificationRepository.DeleteNotificationForUnlikePostAsync(userIdValue, postId);
        }
        else
        {
            string userIdVal = _postRepository.GetUserIdByPostId(postId);
            var user = await _userManager.FindByIdAsync(userIdValue);
            await _notificationRepository.AddNotificationAsync(userIdVal, $"{user.UserName} has liked your post!", postId);

            await _likeRepository.LikePostAsync(postId, userIdValue);
        }

        // After toggling, return the updated post page or view with updated like status
        return RedirectToAction("ViewPost", "Post", new { id = postId });
    }
}

