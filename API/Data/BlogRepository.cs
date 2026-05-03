using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class BlogRepository(DataContext context, IMapper mapper) : IBlogRepository
    {
        public async Task<Blog?> GetBlogByIdAsync(int id)
        {
            return await context.Blogs.Where(b => b.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Blog?> GetLatestBlogByUserIdAsync(int userId)
        {
            return await context.Blogs
                .OrderByDescending(b => b.PublishedAt)
                .Where(b => b.UserId == userId).FirstOrDefaultAsync();
        }
        public async Task<List<Blog>> GetAllUserBlogsAsync(AppUser user)
        {
            return await context.Blogs.Where(b => b.UserId == user.Id).ToListAsync();
        }

        public async Task AddBlogAsync(AppUser user, string description)
        {
            var blog = new Blog
            {
                Description = description,
                UserId = user.Id,
            };

            await context.Blogs.AddAsync(blog);
        }

        public void RemoveBlog(Blog blog)
        {
            context.Remove(blog);
        }
    }
}
