using Bonded.Models.Interfaces;
using Bonded.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bonded.Components
{
    public class ShowOtherPost:ViewComponent
    {

        private readonly IPostRepository _postRepository;

        public ShowOtherPost(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(string userIdValue)
        {
            

            List<Post> posts = await _postRepository.ShowPostsAsync(userIdValue);
            List<Post> posts1 = new List<Post>();
            
            foreach (var post in posts)
            {
                if (!post.IsPrivate)
                {
                    posts1.Add(post);
                }
            }
           
            return View(posts1);
        }
    }
}
