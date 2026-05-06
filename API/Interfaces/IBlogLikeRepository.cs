using API.Entities;

namespace API.Interfaces
{
    public interface IBlogLikeRepository
    {
        Task LikeUserBlogAsync(Blog blog);
    }
}
