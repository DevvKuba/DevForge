using API.Entities;

namespace API.Interfaces
{
    public interface IBlogLikeRepository
    {

        BlogLike? GetUserBlogLike(AppUser targetUser, Blog targetBlog);

        Task LikeUserBlogAsync(Blog blog, int? likingUserId);

        void DeleteUserBlogLike(BlogLike blogLike);
    }
}
