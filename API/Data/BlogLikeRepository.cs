using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class BlogLikeRepository(DataContext context, IMapper mapper) : IBlogLikeRepository
    {
        public async Task<BlogLike?> GetUserBlogLike(AppUser targetUser, Blog targetBlog)
        {
            return await context.BlogLikes.Where(l => l.UserId == targetUser.Id && l.BlogId == targetBlog.Id).FirstOrDefaultAsync();
        }

        public async Task<bool> HasUserLikedTheBlog(AppUser user, Blog targetBlog)
        {
            var like = await context.BlogLikes.Where(l => l.UserId == user.Id && l.BlogId == targetBlog.Id).FirstOrDefaultAsync();

            if (like == null) return false;
            return true;
        }

        public async Task LikeUserBlogAsync(Blog blog, int? likingUserId)
        {
            var blogLike = new BlogLike
            {
                BlogId = blog.Id,
                UserId = likingUserId,
            };

            await context.BlogLikes.AddAsync(blogLike);
        }

        public void DeleteUserBlogLike(BlogLike blogLike)
        {
            context.BlogLikes.Remove(blogLike);
        }
    }
}
