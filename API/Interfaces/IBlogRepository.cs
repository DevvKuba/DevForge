using API.DTO_s;
using API.Entities;

namespace API.Interfaces
{
    public interface IBlogRepository
    {
        Task<BlogDto?> GetBlogByIdAsync(int id);

        Task<BlogDto?> GetLatestBlogByUserIdAsync(int userId);

        Task<List<BlogDto>> GetAllBlogsAsync();

        Task<List<BlogDto>> GetAllUserBlogsAsync(AppUser user);

        Task AddBlogAsync(AppUser user, string description);

        void RemoveBlog(BlogDto blog);
    }
}
