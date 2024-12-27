using Microsoft.AspNetCore.Mvc;
using Bonded.Models;
using Bonded.Models.Interfaces;
using Bonded.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Bonded.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly UserManager<User> _userManager;
        public CommentController(ICommentRepository commentRepository, IPostRepository postRepository, INotificationRepository notificationRepository, UserManager<User> userManager)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _notificationRepository = notificationRepository;
            _userManager = userManager;
        }
        public IActionResult AddComment(int postId)
        {
            // Check if the post exists (optional, based on your logic)


            // Pass the PostId to the view for use in the form
            ViewBag.PostId = postId; // or use ViewData["PostId"]

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCommentAsync(int postId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                ModelState.AddModelError("Content", "Comment content cannot be empty.");
                ViewBag.PostId = postId;
                return View();
            }
            string? userId = HttpContext.Session.GetString("UserId");
            string userIdValue = userId ?? "";
            
            if (userIdValue == "")
            {
                TempData["ErrorMessage"] = "Signin to view more!";
                return RedirectToAction("Login", "User");

            }
           

            var comment = new Comment
            {
                PostId = postId,
                Content = content,
                UserId = userIdValue
            };
            string userIdVal = _postRepository.GetUserIdByPostId(postId);

            var user = await _userManager.FindByIdAsync(userIdValue);
            await _notificationRepository.AddNotificationAsync(userIdVal, $"{user.UserName} has commented on your post: {comment.Content}", postId);
            await _commentRepository.AddCommentAsync(comment);


            return RedirectToAction("AddComment", "Comment", new { postId = postId });
        }
        [HttpGet]
        public async Task<IActionResult> ViewCommentsAsync(int postId)
        {
            // Fetch the post and its comments
            List<CommentsDetailViewModel> comments = await _commentRepository.ViewCommentsWithUserDetailsAsync(postId);
            if (comments == null)
            {
                comments = new List<CommentsDetailViewModel>();
            }
            // Pass the post and its comments to the view
            return View(comments);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteComment(int CommentId, int PostId)
        {
            await _commentRepository.DeleteComment(CommentId);
            Post post = _postRepository.GetPostById(PostId);
            List<CommentsDetailViewModel> comments = await _commentRepository.ViewCommentsWithUserDetailsAsync(PostId);
            if (comments == null)
            {
                comments = new List<CommentsDetailViewModel>();
            }
            return RedirectToAction("AddComment", new { postId = PostId });
        }

    }
}

