using API.Entities;

namespace API.Interfaces
{
    public interface IBlogRepository
    {
        Task<Blog?> GetBlogByIdAsync(int id);

        Task<Blog?> GetLatestBlogByUserIdAsync(int userId);

        Task<List<Blog>> GetAllUserBlogsAsync(AppUser user);

        Task UpdateBlogAsync(Blog blog);

        Task AddBlogAsync(AppUser user, string description);

        void RemoveBlog(Blog blog);
    }
}
