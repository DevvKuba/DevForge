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
    public class BlogsController(IUnitOfWork unitOfWork, IXpService xpService, IMapper mapper) : BaseApiController
    {
        [HttpGet("GatherAllBlogs")]
        public async Task<ActionResult<List<BlogDto>>> GatherAllBlogsAsync([FromQuery] BlogParams blogParams)
        {
            List<BlogDto> blogs = [];

            if (blogParams.TitleSearchTerm != null)
            {
                blogs = await unitOfWork.BlogRepository.GetAllBlogsWithSpecificTitleAsync(blogParams);
            }
            else
            {
                blogs = await unitOfWork.BlogRepository.GetAllBlogsAsync(blogParams);
            }

            if (blogs.Count == 0 | blogs == null) return NotFound(false);

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

        [HttpGet("CheckIfBlogIsLikedByUser")]
        public async Task<ActionResult<bool>> HasBlogBeenLikedByUserAsync(int blogId, int interactingUserId)
        {
            var blog = await unitOfWork.BlogRepository.GetBlogByIdAsync(blogId);

            if (blog == null) return NotFound(false);

            var interactingUser = await unitOfWork.UserRepository.GetUserByIdAsync(interactingUserId);

            if (interactingUser == null) return NotFound(false);

            var hasUserLikedBlog = await unitOfWork.BlogLikeRepository.HasUserLikedTheBlog(interactingUser, blog);

            return Ok(hasUserLikedBlog);

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

            await unitOfWork.BlogLikeRepository.LikeUserBlogAsync(blog, userBlog.InteractingUserId);

            if (!await unitOfWork.Complete()) return BadRequest(false);

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
        public async Task<ActionResult<ApiResponse<string>>> AddNewBlogAsync(BlogDto newBlog)
        {
            var userId = User.GetUserId();

            var postingUser = await unitOfWork.UserRepository.GetUserByIdAsync(userId);

            if (postingUser == null) return NotFound(new ApiResponse<string> { });

            await unitOfWork.BlogRepository.AddBlogAsync(postingUser, newBlog.Title, newBlog.Description);

            xpService.AwardXp

            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponse<string> { });
            }
            return Ok(new ApiResponse<string> {Success = true, XpDetails = new UserXpDetailDto { } });
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

            if (blog == null || userBlog.InteractingUserId == null) return NotFound(false);

            var interactingUser = await unitOfWork.UserRepository.GetUserByIdAsync((int)userBlog.InteractingUserId);

            if (interactingUser == null) return NotFound(false);

            var userBlogLike = await unitOfWork.BlogLikeRepository.GetUserBlogLike(interactingUser, blog);

            if (userBlogLike == null) return NotFound(false);

            unitOfWork.BlogLikeRepository.DeleteUserBlogLike(userBlogLike);

            if(!await unitOfWork.Complete()) return BadRequest(false);

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
