using API.DTO_s;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        public async Task<List<BlogDto>> GetAllBlogsAsync(BlogParams blogParams)
        {
            var query = context.Blogs
                .Include(b => b.BlogComments)
                .Include(b => b.BlogLikes)
                .OrderByDescending(b => b.PublishedAt)
                .AsQueryable();

            var blogs = query.ProjectTo<BlogDto>(mapper.ConfigurationProvider);

            return await PagedList<BlogDto>.CreateAsync(blogs, blogParams.PageNumber, blogParams.PageSize);
        }

        public async Task<List<BlogDto>> GetAllUserBlogsAsync(BlogParams blogParams)
        {
            var query = context.Blogs
                .Where(b => b.UserId == blogParams.UserId)
                .Include(b => b.BlogComments)
                .Include(b => b.BlogLikes)
                .OrderByDescending(b => b.PublishedAt)
                .AsQueryable();

            var userBlogs = query.ProjectTo<BlogDto>(mapper.ConfigurationProvider);

            return await PagedList<BlogDto>.CreateAsync(userBlogs, blogParams.PageNumber, blogParams.PageSize);
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
