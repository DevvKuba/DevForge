using API.DTO_s;
using API.Entities;
using API.Extensions;
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
        [HttpGet("GatherAllBlogs")]
        public async Task<ActionResult<List<BlogDto>>> GatherAllBlogsAsync([FromQuery] BlogParams blogParams)
        {
            var blogs = await unitOfWork.BlogRepository.GetAllBlogsAsync(blogParams);

            if (blogs.Count == 0 | blogs == null)
            {
                return NotFound(false);
            }

            return Ok(blogs);
        }

        [HttpGet("GatherUserBlogs")]
        public async Task<ActionResult<List<BlogDto>>> GatherAllSpecificUserBlogsAsync([FromQuery] BlogParams blogParams)
        {
            if (blogParams.UserId == null) return NotFound(false);

            var blogs = await unitOfWork.BlogRepository.GetAllUserBlogsAsync(blogParams);

            if (blogs.Count == 0 | blogs == null)
            {
                return NotFound(false);
            }

            return Ok(blogs);
        }

        [HttpPut("UpdateBlogComment")]
        public async Task<ActionResult> UpdateCurrentBlogCommentAsync(BlogCommentDto updatedBlogComment)
        {
            var blogComment = await unitOfWork.BlogCommentRepository.GetBlogCommentByIdAsync(updatedBlogComment.Id);

            if (blogComment == null) return NotFound("Blog comment to updage had not been found");

            mapper.Map(updatedBlogComment, blogComment);
            blogComment.UpdatedAt = DateTime.UtcNow;

            if (!await unitOfWork.Complete())
            {
                return BadRequest(false);
            }
            return Ok(true);
        }

        [HttpPut("UpdateBlogPost")]
        public async Task<ActionResult> UpdateCurrentBlogPostAsync(BlogDto updatedBlog)
        {
            var blog = await unitOfWork.BlogRepository.GetBlogByIdAsync(updatedBlog.Id);

            if (blog == null) return NotFound(false);

            mapper.Map(updatedBlog, blog);
            blog.UpdatedAt = DateTime.UtcNow;

            if (!await unitOfWork.Complete())
            {
                return BadRequest(false);
            }
            return Ok(true);
        }

        [HttpPost("AddBlogLike")]
        public async Task<ActionResult> LikeUserBlogAsync(BlogDto userBlog)
        {
            var blog = await unitOfWork.BlogRepository.GetBlogByIdAsync(userBlog.Id);

            if (blog == null) return NotFound(false);

            await unitOfWork.BlogLikeRepository.LikeUserBlogAsync(blog, userBlog.UserId);

            return Ok(true);
        }

        [HttpPost("AddBlogComment")]
        public async Task<ActionResult> AddBlogCommentAsync(BlogCommentDto blogComment)
        {
            var blog = await unitOfWork.BlogRepository.GetBlogByIdWithLikesAsync(blogComment.BlogId);

            if (blog == null || blogComment.UserId == null) return NotFound(false);

            await unitOfWork.BlogCommentRepository.AddBlogCommentAsync(blog, blogComment.UserId, blogComment.Content);

            if (!await unitOfWork.Complete())
            {
                return BadRequest(false);
            }
            return Ok(true);
        }

        [HttpPost("AddBlog")]
        public async Task<ActionResult> AddNewBlogAsync(BlogDto newBlog)
        {
            var userId = User.GetUserId();

            var postingUser = await unitOfWork.UserRepository.GetUserByIdAsync(userId);

            if (postingUser == null) return NotFound(false);

            await unitOfWork.BlogRepository.AddBlogAsync(postingUser, newBlog.Title, newBlog.Description);

            if (!await unitOfWork.Complete())
            {
                return BadRequest(false);
            }
            return Ok(true);
        }

        [HttpDelete("DeleteBlog")]
        public async Task<ActionResult> DeleteUserBlogAsync([FromBody] BlogDto userBlog)
        {
            var blog = await unitOfWork.BlogRepository.GetBlogByIdAsync(userBlog.Id);

            if (blog == null) return NotFound(false);

            unitOfWork.BlogRepository.RemoveBlog(blog);

            if (!await unitOfWork.Complete())
            {
                return BadRequest(false);
            }
            return Ok(true);
        }

        [HttpDelete("UndoBlogLike")]
        public async Task<ActionResult> DeleteUserBlogLikeAsync([FromBody] BlogDto userBlog)
        {
            var blog = await unitOfWork.BlogRepository.GetBlogByIdWithLikesAsync(userBlog.Id);

            if (blog == null) return NotFound(false);

            var user = await unitOfWork.UserRepository.GetUserByIdAsync(userBlog.Id);

            if (user == null) return NotFound(false);

            var userBlogLike = unitOfWork.BlogLikeRepository.GetUserBlogLike(user, blog);

            if (userBlogLike == null) return NotFound(false);

            unitOfWork.BlogLikeRepository.DeleteUserBlogLike(userBlogLike);

            if(!await unitOfWork.Complete())
            {
                return BadRequest(false);
            }
            return Ok(true);
        }

        [HttpDelete("DeleteBlogComment")]
        public async Task<ActionResult> DeleteUserBlogCommentAsync([FromBody] BlogCommentDto deletionBlogComment)
        {
            var blogComment = await unitOfWork.BlogCommentRepository.GetBlogCommentByIdAsync(deletionBlogComment.Id);

            if (blogComment == null) return NotFound(false);

            unitOfWork.BlogCommentRepository.DeleteBlogCommentAsync(blogComment);

            if(!await unitOfWork.Complete())
            {
                return BadRequest(false);
            }
            return Ok(true);
        }
    }
}
