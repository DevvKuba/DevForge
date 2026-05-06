using API.Entities;
using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class BlogLikeRepository(DataContext context, IMapper mapper) : IBlogLikeRepository
    {
        public async Task LikeUserBlogAsync(Blog blog)
        {
            var blogLike = new BlogLike
            {
                BlogId = blog.Id,
            };

            await context.BlogLikes.AddAsync(blogLike);
        }
    }
}
