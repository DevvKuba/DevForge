using API.Entities;

namespace API.Interfaces
{
    public interface IBlogRepository
    {
        Task<Blog> GetBlogByIdAsync(int id);

        Task<Blog> GetBlogByUserIdAsync(int userId);

        Task<Blog[]> GetAllUserBlogsAsync { get; set; }

        Task UpdateBlogAsync(Blog blog);

        Task AddBlogAsync(AppUser user, string description);

        void RemoveBlog(Blog blog);
    }
}
