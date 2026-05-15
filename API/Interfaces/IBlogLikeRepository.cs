using API.Entities;

namespace API.Interfaces
{
    public interface IBlogLikeRepository
    {

        BlogLike? GetUserBlogLike(AppUser targetUser, Blog targetBlog);

        Task<bool> HasUserLikedTheBlog(AppUser user, Blog targetBlog);

        Task LikeUserBlogAsync(Blog blog, int? likingUserId);

        void DeleteUserBlogLike(BlogLike blogLike);
    }
}
