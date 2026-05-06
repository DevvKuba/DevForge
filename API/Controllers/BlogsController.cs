using API.DTO_s;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class BlogsController(IUnitOfWork unitOfWork, IMapper mapper) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<BlogDto>>> GatherAllBlogsAsync(BlogParams blogParams)
        {
            var blogs = await unitOfWork.BlogRepository.GetAllBlogsAsync(blogParams);

            if (blogs.Count == 0 | blogs == null)
            {
                return NotFound("No blogs found");
            }

            return Ok(blogs);
        }

        [HttpGet]
        public async Task<ActionResult<List<BlogDto>>> GatherAllUserBlogsAsync(BlogParams blogParams)
        {
            var blogs = await unitOfWork.BlogRepository.GetAllUserBlogsAsync(blogParams);

            if (blogs.Count == 0 | blogs == null)
            {
                return NotFound("This user has not posted any blogs");
            }

            return Ok(blogs);
        }

        [HttpPost]
        public async Task<ActionResult> LikeUserBlogAsync(BlogDto userBlog)
        {
            var blog = await unitOfWork.BlogRepository.GetBlogByIdAsync(userBlog.Id);

            if (blog == null) return NotFound("User blog not found");

            await unitOfWork.BlogLikeRepository.LikeUserBlogAsync(blog);

            return Ok("User blog has been liked");
        }

    }
}
