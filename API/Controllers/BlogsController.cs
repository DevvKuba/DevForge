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
        public async Task<ActionResult<List<BlogDto>>> GatherAllBlogsAsync(BlogParams blogParams)
        {
            var blogs = await unitOfWork.BlogRepository.GetAllBlogsAsync(blogParams);

            if (blogs.Count == 0 | blogs == null)
            {
                return NotFound("No blogs found");
            }

            return Ok(blogs);
        }

        [HttpGet("GatherUserBlogs")]
        public async Task<ActionResult<List<BlogDto>>> GatherAllSpecificUserBlogsAsync(BlogParams blogParams)
        {
            if (blogParams.UserId == null) return NotFound("");

            var blogs = await unitOfWork.BlogRepository.GetAllUserBlogsAsync(blogParams);

            if (blogs.Count == 0 | blogs == null)
            {
                return NotFound("This user has not posted any blogs");
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
                return BadRequest("Not able to update user blog comment");
            }
            return Ok("Correctly updated user blog comment");
        }

        [HttpPut("UpdateBlogPost")]
        public async Task<ActionResult> UpdateCurrentBlogPostAsync(BlogDto updatedBlog)
        {
            var blog = await unitOfWork.BlogRepository.GetBlogByIdAsync(updatedBlog.Id);

            if (blog == null) return NotFound("Blog to updage had not been found");

            mapper.Map(updatedBlog, blog);
            blog.UpdatedAt = DateTime.UtcNow;

            if (!await unitOfWork.Complete())
            {
                return BadRequest("Not able to update user blog");
            }
            return Ok("Correctly updated user blog");
        }

        [HttpPost("AddBlogLike")]
        public async Task<ActionResult> LikeUserBlogAsync(BlogDto userBlog)
        {
            var blog = await unitOfWork.BlogRepository.GetBlogByIdAsync(userBlog.Id);

            if (blog == null) return NotFound("User blog not found");

            await unitOfWork.BlogLikeRepository.LikeUserBlogAsync(blog);

            return Ok("User blog has been liked");
        }

        [HttpPost("AddBlogComment")]
        public async Task<ActionResult> AddBlogCommentAsync(BlogCommentDto blogComment)
        {
            var blog = await unitOfWork.BlogRepository.GetBlogByIdWithLikesAsync(blogComment.BlogId);

            if (blog == null) return NotFound("User blog not found");

            await unitOfWork.BlogCommentRepository.AddBlogCommentAsync(blog, blogComment.Content);

            if (!await unitOfWork.Complete())
            {
                return BadRequest("Blog comment was not added successfully");
            }
            return Ok("New blog comment was successfully added");
        }

        [HttpPost("AddBlog")]
        public async Task<ActionResult> AddNewBlogAsync(BlogDto newBlog)
        {
            var userId = User.GetUserId();

            var postingUser = await unitOfWork.UserRepository.GetUserByIdAsync(userId);

            if (postingUser == null) return NotFound("Blog posting user was not found");

            await unitOfWork.BlogRepository.AddBlogAsync(postingUser, newBlog.Title, newBlog.Description);

            if (!await unitOfWork.Complete())
            {
                return BadRequest("Blog was not added successfully");
            }
            return Ok($"New blog titled: '{newBlog.Title}', has been published");
        }

        [HttpDelete("DeleteBlog")]
        public async Task<ActionResult> DeleteUserBlogAsync(BlogDto userBlog)
        {
            var blog = await unitOfWork.BlogRepository.GetBlogByIdAsync(userBlog.Id);

            if (blog == null) return NotFound("Blog for deletion was not found");

            unitOfWork.BlogRepository.RemoveBlog(blog);

            if (!await unitOfWork.Complete())
            {
                return BadRequest("Not able to delete the blog post");
            }
            return Ok("Successfully deleted the blog post");
        }

        [HttpDelete("UndoBlogLike")]
        public async Task<ActionResult> DeleteUserBlogLikeAsync(BlogDto userBlog)
        {
            var blog = await unitOfWork.BlogRepository.GetBlogByIdWithLikesAsync(userBlog.Id);

            if (blog == null) return NotFound("User blog not found");

            var user = await unitOfWork.UserRepository.GetUserByIdAsync(userBlog.Id);

            if (user == null) return NotFound("User not found");

            var userBlogLike = unitOfWork.BlogLikeRepository.GetUserBlogLike(user, blog);

            if (userBlogLike == null) return NotFound("User has not liked this blog post");

            unitOfWork.BlogLikeRepository.DeleteUserBlogLike(userBlogLike);

            if(!await unitOfWork.Complete())
            {
                return BadRequest("Not able to undo the like of the blog post");
            }
            return Ok("Successfully deleted the like for the blog post");
        }

        [HttpDelete("DeleteBlogComment")]
        public async Task<ActionResult> DeleteUserBlogCommentAsync(BlogCommentDto deletionBlogComment)
        {
            var blogComment = await unitOfWork.BlogCommentRepository.GetBlogCommentByIdAsync(deletionBlogComment.Id);

            if (blogComment == null) return NotFound("Blog comment for deletion not found");

            unitOfWork.BlogCommentRepository.DeleteBlogCommentAsync(blogComment);

            if(!await unitOfWork.Complete())
            {
                return BadRequest("Blog comment was not removed successfully");
            }
            return Ok("Blog comment was removed successfully");
        }
    }
}
