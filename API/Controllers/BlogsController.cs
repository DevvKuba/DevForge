using API.DTO_s;
using API.Entities;
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
        public async Task<ActionResult<List<BlogDto>>> GetAllUserBlogs()
        {

        }
    }
}
