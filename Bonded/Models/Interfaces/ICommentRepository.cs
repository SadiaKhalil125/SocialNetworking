using Bonded.Models.ViewModels;

namespace Bonded.Models
{
    public interface ICommentRepository
    {
        Task<bool> AddCommentAsync(Comment comment);
        Task<List<Comment>> ViewCommentsAsync(int postId);
        Task<List<CommentsDetailViewModel>> ViewCommentsWithUserDetailsAsync(int postId);
        Task DeleteComment(int CommentId);
        void DeleteCommentsOfAPost(int postId);
    }
}


















//using Bonded.Models.ViewModels;

//namespace Bonded.Models.Interfaces
//{
//    public interface ICommentRepository
//    {
//        public bool AddComment(Comment comment);

//        public List<CommentsDetailViewModel> ViewComments(int postId);
//    }
//}
