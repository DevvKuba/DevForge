using API.DTO_s;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IBlogRepository
    {
        Task<Blog?> GetBlogByIdAsync(int id);

        Task<Blog?> GetLatestBlogByUserIdAsync(int userId);

        Task<List<BlogDto>> GetAllBlogsAsync(BlogParams blogParams);

        Task<List<BlogDto>> GetAllUserBlogsAsync(BlogParams blogParams);

        Task AddBlogAsync(AppUser user, string description);

        void RemoveBlog(Blog blog);
    }
}
