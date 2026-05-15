using API.Entities;
using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class BlogLikeRepository(DataContext context, IMapper mapper) : IBlogLikeRepository
    {
        public BlogLike? GetUserBlogLike(AppUser targetUser, Blog targetBlog)
        {
            return targetBlog.BlogLikes.Where(l => l.Blog.UserId == targetUser.Id).FirstOrDefault();
        }

        public async Task LikeUserBlogAsync(Blog blog, int? userId)
        {
            var blogLike = new BlogLike
            {
                BlogId = blog.Id,
                UserId = userId,
            };

            await context.BlogLikes.AddAsync(blogLike);
        }

        public void DeleteUserBlogLike(BlogLike blogLike)
        {
            throw new NotImplementedException();
        }
    }
}
