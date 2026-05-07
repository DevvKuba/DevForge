using API.Entities;

namespace API.Interfaces
{
    public interface IBlogCommentRepository
    {
        Task<BlogComment?> GetBlogCommentByIdAsync(int id);

        Task AddBlogCommentAsync(Blog blog, string content);

        void DeleteBlogCommentAsync(BlogComment blogComment);
    }
}
