using Bonded.Models.ViewModels;
using Bonded.Models;
using Microsoft.AspNetCore.Mvc;
using Bonded.Models.Interfaces;

namespace Bonded.Components
{
    public class ShowCommentDetails:ViewComponent
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;

        public ShowCommentDetails(ICommentRepository commentRepository,IPostRepository postrepository)
        {
          _commentRepository = commentRepository;
            _postRepository = postrepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(int postId)
        {
            // Fetch the post and its comments
            string? userId = HttpContext.Session.GetString("UserId");
            string userIdValue = userId ?? "";
            List<CommentsDetailViewModel> comments = await _commentRepository.ViewCommentsWithUserDetailsAsync(postId);
            string idOfPoster = _postRepository.GetUserIdByPostId(postId);
            foreach (CommentsDetailViewModel comment in comments)
            {
                comment.CanUserDelete = false;
                if((comment.User.Id == userIdValue) || (idOfPoster == userIdValue)) 
                {
                    comment.CanUserDelete = true;
                }
            }
            if (comments == null)
            {
                comments = new List<CommentsDetailViewModel>();
            }
            // Pass the post and its comments to the view
            return View(comments);
       
        }
    }
}
