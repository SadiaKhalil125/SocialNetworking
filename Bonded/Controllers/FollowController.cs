using Bonded.Models;
using Bonded.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class FollowController : Controller
{
    private readonly IFollowRepository _followRepository;
    private readonly UserManager<User> _userManager;
    private readonly INotificationRepository _notificationRepository;
    public FollowController(IFollowRepository followRepository, UserManager<User> userManager, INotificationRepository notificationRepository)
    {
        _followRepository = followRepository;
        _userManager = userManager;
        _notificationRepository = notificationRepository;
    }
    [HttpGet]
    public async Task<IActionResult> ShowFollowingAsync(string userId)
    {
        // Get the list of users that the specified user is following
        var followingIds = await _followRepository.GetFollowingIdsAsync(userId);
        List<User> followings = new List<User>();
        foreach (string id in followingIds)
        {
            followings.Add(await _userManager.FindByIdAsync(id));
        }

        return View(followings);// Return the list to the view
    }
    public async Task<IActionResult> ShowFollowerAsync(string userId)
    {
        // Fetch the followers' IDs
        List<string> followerIds = await _followRepository.GetFollowersIdsAsync(userId);

        // Fetch the user details for each follower ID
        List<User> followers = new List<User>();
        foreach (var followerId in followerIds)
        {
            var user = await _userManager.FindByIdAsync(followerId); // Adjust this call to get user by ID
            if (user != null)
            {
                followers.Add(user);
            }
        }

        // Pass the list of followers to the view
        return View(followers);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleFollow(string followerId, string followingId)
    {
        if (followerId == followingId)
        {
            TempData["ErrorMessage"] = "Can't follow yourself!";
           
            return RedirectToAction("ViewOtherProfiles", "User", new { id = followingId });
            //return BadRequest("You cannot follow yourself.");
        }

        var isFollowing = await _followRepository.IsFollowingAsync(followerId, followingId);

        if (isFollowing)
        {

            await _followRepository.UnfollowUserAsync(followerId, followingId);
            await _notificationRepository.DeleteNotificationForUnfollowAsync(followerId, 1);

        }
        else
        {
            await _followRepository.FollowUserAsync(followerId, followingId);
            var user = await _userManager.FindByIdAsync(followerId);
            await _notificationRepository.AddNotificationAsync(followingId, $"{user.UserName} has followed you!", 1);
        }

        return RedirectToAction("ViewOtherProfiles", "User", new { id = followingId });
    }

    public JsonResult GetFollowerFollowingCounts()
    {
        string? userId = HttpContext.Session.GetString("UserId");
        string userIdValue = userId ?? "";

        var followers = _followRepository.GetFollowersCountAsync(userIdValue); //_context.Followers.Count(f => f.FollowedUserId == UserId);
        var following = _followRepository.GetFollowingCountAsync(userIdValue); 

        return Json(new { followers, following });
    }



}

