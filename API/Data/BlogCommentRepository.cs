using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class BlogCommentRepository(DataContext context, IMapper mapper) : IBlogCommentRepository
    {
        public async Task<BlogComment?> GetBlogCommentByIdAsync(int id)
        {
            return await context.BlogComments.Where(c => c.Id == id).FirstAsync();
        }

        public async Task AddBlogCommentAsync(Blog blog, int? commentingUserId, string content)
        {
            var blogComment = new BlogComment
            {
                BlogId = blog.Id,
                UserId = commentingUserId,
                Content = content,
            };

            await context.BlogComments.AddAsync(blogComment);
        }

        public void DeleteBlogCommentAsync(BlogComment blogComment)
        {
            context.BlogComments.Remove(blogComment);
        }
    }
}
