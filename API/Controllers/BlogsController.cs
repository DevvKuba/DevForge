using API.DTO_s;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
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
    }
}
