using API.DTO_s;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController(IUnitOfWork unitOfWork, IXpService xpService, IMapper mapper) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<ApiResponse<MessageDto>>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var userId = User.GetUserId();

            var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId);

            if (userId == createMessageDto.RecipientId) return BadRequest(new ApiResponse<MessageDto> { });

            if (user == null) return NotFound(new ApiResponse<MessageDto> { });


            var sender = await unitOfWork.UserRepository.GetUserByIdAsync(userId);
            var recipient = await unitOfWork.UserRepository.GetUserByIdAsync(createMessageDto.RecipientId);

            if (recipient == null || sender == null)
                return BadRequest(new ApiResponse<MessageDto> { Message = "Cannot send message at this time" });

            var message = new Message
            {
                Sender = sender!,
                Recipient = recipient!,
                SenderUsername = sender.UserName!,
                RecipientUsername = recipient.UserName!,
                Content = createMessageDto.Content,
            };

            unitOfWork.MessageRepository.AddMessage(message);

            xpService.AwardXp(user, (int)XpActions.SendMessageToUser);

            if (await unitOfWork.Complete())
            {
                return Ok(new ApiResponse<MessageDto> 
                {
                  Data = mapper.Map<MessageDto>(message),
                  Success = true,
                  XpDetails = new UserXpDetailDto
                  {
                      AppExperiencePoints = user.AppExperiencePoints,
                      Level = user.Level,
                      LevelThreshold = xpService.GetXpThresholdForLevel(user.Level),
                  }
                });
            }

            return BadRequest(new ApiResponse<MessageDto> { });
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Id = User.GetUserId();

            var messages = await unitOfWork.MessageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages);

            return messages;
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(int recipientId)
        {
            var senderId = User.GetUserId();

            return Ok(await unitOfWork.MessageRepository.GetMessageThread(senderId, recipientId));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var userId = User.GetUserId();
            var message = await unitOfWork.MessageRepository.GetMessage(id);

            if (message == null) return BadRequest("Cannot delete this message");

            // check if the person deleting is either the sender or reciever of the message
            if (message.SenderId != userId && message.RecipientId != userId) return Forbid();

            if (message.SenderId == userId) message.SenderDeleted = true;
            if (message.RecipientId == userId) message.RecipientDeleted = true;

            if (message is { SenderDeleted: true, RecipientDeleted: true })
            {
                unitOfWork.MessageRepository.DeleteMessage(message);
            }

            if (await unitOfWork.Complete()) return Ok();

            return BadRequest("Problem deleting the message");


        }
    }
}
