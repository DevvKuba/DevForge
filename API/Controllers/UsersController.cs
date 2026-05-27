using API.DTO_s;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // user needs to be autherized / logged in before utilising get requests
    [Authorize]
    public class UsersController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService, IXpService xpService) : BaseApiController
    {
        [HttpGet]
        // specifices to look in query string => [FromQuery]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            userParams.CurrentUserId = User.GetUserId();
            var users = await unitOfWork.UserRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(users);

            return Ok(users);
        }

        // route parameter
        [HttpGet("GetUserById")]
        public async Task<ActionResult<MemberDto>> GetUserAsync([FromQuery] int userId)
        {
            var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId);

            if (user == null) return NotFound( "No one is logged in, cannot proceed");

            if (user.UserName == null) return BadRequest("User does not have a username");

            var memberDto = await unitOfWork.UserRepository.GetMemberAsync(user.Id);

            if (memberDto == null) return NotFound("No member like this exists");

            memberDto.LevelThreshold = xpService.GetXpThresholdForLevel(memberDto.Level);

            return memberDto;

        }

        [HttpGet("GetUserByIdWithXp")]
        public async Task<ActionResult<ApiResponse<MemberDto>>> GetUserAsyncWithXp([FromQuery] int userId)
        {
            var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId);

            if (user == null) return NotFound(new ApiResponse<MemberDto> { Message = "No one is logged in, cannot proceed" });

            if (user.UserName == null) return BadRequest(new ApiResponse<MemberDto> { Message = "User does not have a username" });

            var memberDto = await unitOfWork.UserRepository.GetMemberAsync(user.Id);

            if (memberDto == null) return NotFound(new ApiResponse<MemberDto> { Message = "No member like this exists" });

            memberDto.LevelThreshold = xpService.GetXpThresholdForLevel(memberDto.Level);

            return new ApiResponse<MemberDto>
            {
                Data = memberDto,
                Success = true,
                XpDetails = new UserXpDetailDto
                {
                    Level = memberDto.Level,
                    AppExperiencePoints = memberDto.AppExperiencePoints,
                    LevelThreshold = memberDto.LevelThreshold
                }
            };

        }

        // put requests to update database
        [HttpPut]
        public async Task<ActionResult> UpdateUserAsync(MemberUpdateDto memberUpdateDto)
        {
            // user from datbase aquired by entity framework
            var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());

            if (user == null) return BadRequest("Could not find user");

            mapper.Map(memberUpdateDto, user!);

            if (await unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to update user");

        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhotoAsync(IFormFile file)
        {
            var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());

            if (user == null) return BadRequest("Cannot update user");

            var result = await photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl!.AbsoluteUri,
                PublicId = result.PublicId!,
            };

            // auto sets to main photo if its their first photo
            if (user.Photos.Count == 0) photo.IsMain = true;

            user.Photos.Add(photo);
            if (await unitOfWork.Complete())
                return CreatedAtAction(nameof(GetUserAsync), new { username = user.UserName }, mapper.Map<PhotoDto>(photo));
            // adds a location header of the user, to our response
            return BadRequest("Photos couldn't be added");
        }

        [HttpPut("set-main-photo/{photoId:int}")]
        public async Task<ActionResult> SetMainPhotoAsync(int photoId)
        {
            var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());

            if (user == null) return BadRequest("Could not find user");

            var photo = user!.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null || photo.IsMain) return BadRequest("Cannot use this as a main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await unitOfWork.Complete()) return NoContent();

            return BadRequest("Probelm setting main photo");

        }

        [HttpDelete("delete-photo/{photoId:int}")]
        public async Task<ActionResult> DeletePhotoAsync(int photoId)
        {
            var user = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());

            if (user == null) return BadRequest("User not found");

            var photo = await unitOfWork.PhotoRepository.GetPhotoById(photoId);

            if (photo == null || photo.IsMain) return BadRequest("This photo cannot be deleted");

            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await unitOfWork.Complete()) return Ok();

            return BadRequest("Problem deleted photo");
        }

    }

}
