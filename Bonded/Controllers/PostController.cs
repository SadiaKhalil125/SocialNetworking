using Bonded.Models;
using Bonded.Models.Interfaces;
using Bonded.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bonded.Controllers
{
    public class PostController : Controller
    {

        private readonly IPostRepository _postRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly UserManager<User> _userManager;
        private readonly ICommentRepository _commentRepository;

        public PostController(IPostRepository postRepository, ILikeRepository likeRepository, UserManager<User> userManager, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _likeRepository = likeRepository;
            _userManager = userManager;
            _commentRepository = commentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ViewPost(int id)
        {
            string? userId = HttpContext?.Session?.GetString("UserId");
            string currentUserId = userId ?? "";

         
            var post = await _postRepository.GetPostByIdAsync(id);
            post.Poster = await _userManager.FindByIdAsync(currentUserId);
            if (post == null)
            {
                return NotFound("Post not found.");
            }

            string? idd = post.PosterId;
            if (string.IsNullOrEmpty(idd))
            {
                return BadRequest("Invalid poster information.");
            }

     
            if (string.IsNullOrEmpty(currentUserId))
            {
                TempData["ErrorMessage"] = "Signin to view more!";
                return RedirectToAction("Login", "User");
            }

            bool check = idd == currentUserId;

            bool isLiked = await _likeRepository.IsPostLikedByUserAsync(currentUserId, id);
            int likeCount = await _likeRepository.GetLikeCountAsync(id);

            var viewModel = new PostDetailsViewModel
            {
                Post = post,
                LikeCount = likeCount,
                LikedByUser = isLiked,
                LoginnedUser = check
            };

            return View(viewModel);
        }


        //public IActionResult ViewPost(int id)
        //{
        //    Post post = _postRepository.GetPostById(id); 
        //    PostDetailsViewModel viewModel = new PostDetailsViewModel();
        //    viewModel.Post = post;
        //    viewModel.
        //    if (post == null)
        //    {
        //        return NotFound("Post not found.");
        //    }

        //    return View(post); 
        //}
        public IActionResult CreatePost()
        {
            return View();  // Returns a view with a form to create a new post
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(string caption, IFormFile? imageFile, bool? check)
        {
         
            Console.WriteLine($"Checkbox Value: {check}");

            if (string.IsNullOrEmpty(caption))
            {
                caption = "";
            }

            string? userId = HttpContext.Session.GetString("UserId");
            string currentUserId = userId ?? "";

            if (string.IsNullOrEmpty(currentUserId))
            {
                TempData["ErrorMessage"] = "Signin to view more!";
                return RedirectToAction("Login", "User");
            }

            // Handle the checkbox value directly from the form if it's not passed through model binding
            bool isPrivate = check ?? false; // Default to 'false' if null

            var newPost = new Post
            {
                UserId = 1,
                PosterId = currentUserId,
                Poster = await _userManager.FindByIdAsync(currentUserId),
                Content = caption,
                IsPrivate = isPrivate // Use the checkbox value here
            };

            if (imageFile != null && imageFile.Length > 0)
            {
                string imagePath = Path.Combine("wwwroot", "images", imageFile.FileName);

                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                newPost.ImagePath = "/images/" + imageFile.FileName;
            }

            _postRepository.StorePost(newPost);

            return RedirectToAction("DetailedProfile", "User");
        }


        [HttpGet]
        public async Task<IActionResult> ShowEditForm(int postId)
        {
            var post = _postRepository.GetPostById(postId); // Adjust based on your implementation
            if (post == null)
            {
                return NotFound("Post not found.");
            }

            string? userId = HttpContext.Session.GetString("UserId");
            string currentUserId = userId ?? "";

            if (currentUserId == "")
            {
                TempData["ErrorMessage"] = "Signin to view more!";
                return RedirectToAction("Login", "User");
            }

            bool isLiked = await _likeRepository.IsPostLikedByUserAsync(currentUserId, postId);
            int likeCount = await _likeRepository.GetLikeCountAsync(postId);

            PostDetailsViewModel viewModel = new PostDetailsViewModel
            {
                Post = post,
                LikeCount = likeCount,
                LikedByUser = isLiked
            };

            return View(viewModel);
        }


        // Edit Post - POST (Only Caption)
        [HttpGet]

        public IActionResult EditPost(int postId, string updatedContent)
        {
            var post = _postRepository.GetPostById(postId);
            if (post == null)
            {
                return NotFound("Post not found.");
            }
            post.Content = updatedContent;
            // Save the changes to the database
            _postRepository.EditPost(postId, post);

            // Redirect back to the post view page
            return RedirectToAction("ViewPost", "Post", new { id = postId });
        }


        [HttpGet]

        public async Task<IActionResult> DeletePost(int postId)
        {
            var post = _postRepository.GetPostById(postId);
            if (post == null)
            {
                return NotFound("Post not found.");
            }
            _likeRepository.DeleteLikesOfAPost(postId);
            _commentRepository.DeleteCommentsOfAPost(postId);
            await _postRepository.DeletePostAsync(postId);
            return RedirectToAction("DetailedProfile", "User"); // Or another appropriate redirect
        }

    }
}

