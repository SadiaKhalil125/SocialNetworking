using Bonded.Models;
using Microsoft.AspNetCore.Mvc;
using Bonded.Models.Interfaces;
namespace Bonded.Components
{
    public class ShowPosts:ViewComponent
    {
        private readonly IPostRepository _postRepository;

        public ShowPosts(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string? userId = HttpContext.Session.GetString("UserId");
            string userIdValue = userId ?? "";

            var posts = await _postRepository.ShowPostsAsync(userIdValue);

            //if (posts == null)
            //{
            //    return NotFound();
            //}

            return View(posts);
        }
    }
}
