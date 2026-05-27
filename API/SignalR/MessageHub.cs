using System.Threading.Tasks.Dataflow;
using API.DTO_s;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class MessageHub(IUnitOfWork unitOfWork, IMapper mapper, IXpService xpService, IHubContext<PresenceHub> presenceHub) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUserIdString = httpContext?.Request.Query["userId"];
            
            if (Context.User == null || string.IsNullOrEmpty(otherUserIdString)) throw new Exception("Cannot join group");

            int otherUserId = int.Parse(otherUserIdString!);
            var otherUser = await unitOfWork.UserRepository.GetUserByIdAsync(otherUserId);

            if (otherUser == null) throw new Exception("Other user no longer exists");

            if (otherUser.UserName == null) throw new Exception("Username must be set in order to join a group");

            var groupName = GetGroupName(Context.User.GetUsername(), otherUser.UserName);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var group = await AddToGroup(groupName);

            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var messages = await unitOfWork.MessageRepository.GetMessageThread(Context.User.GetUserId(), otherUserId);

            if (unitOfWork.HasChanges()) await unitOfWork.Complete();

            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);

        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task<ApiResponse<string>> SendMessage(CreateMessageDto createMessageDto)
        {
            var userId = Context.User?.GetUserId() ?? throw new Exception("could not get user");
            if (userId == createMessageDto.RecipientId)
            {
                throw new HubException("You cannot message yourself");
            }

            var sender = await unitOfWork.UserRepository.GetUserByIdAsync(userId);
            var recipient = await unitOfWork.UserRepository.GetUserByIdAsync(createMessageDto.RecipientId);

            if (recipient == null || sender == null || sender.UserName == null || recipient.UserName == null)
                throw new HubException("Cannot send message at this time");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content,
            };

            var groupName = GetGroupName(sender.UserName, recipient.UserName);
            var group = await unitOfWork.MessageRepository.GetMessageGroup(groupName);

            // recipient is connected / "active"
            if (group != null && group.Connections.Any(x => x.UserId == recipient.Id))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            // if user it not currently connected .. notification
            {
                var connections = await PresenceTracker.GetConnectionsForUser(recipient.UserName);
                if (connections != null && connections?.Count != null)
                {
                    await presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                        new { username = sender.UserName, knownAs = sender.KnownAs });
                }
            }

            unitOfWork.MessageRepository.AddMessage(message);

            xpService.AwardXp(sender, (int)XpActions.SendMessageToUser);

            if (await unitOfWork.Complete())
            {
                await Clients.Group(groupName).SendAsync("NewMessage", mapper.Map<MessageDto>(message));

                return new ApiResponse<string>
                {
                    Success = true,
                    XpDetails = new UserXpDetailDto
                    {
                        Level = sender.Level,
                        AppExperiencePoints = sender.AppExperiencePoints,
                        LevelThreshold = xpService.GetXpThresholdForLevel(sender.Level)
                    }
                };
            }
            else
            {
                throw new HubException("Failed to save message");
            }

        }

        private async Task<Group> AddToGroup(string groupName)
        {
            var userId = Context.User?.GetUserId() ?? throw new Exception("Cannot get user Id");
            var group = await unitOfWork.MessageRepository.GetMessageGroup(groupName);
            var connection = new Connection { ConnectionId = Context.ConnectionId, UserId = userId };

            if (group == null)
            {
                group = new Group { Name = groupName };
                unitOfWork.MessageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);
            if (await unitOfWork.Complete()) return group;

            throw new HubException("Failed to join group");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await unitOfWork.MessageRepository.GetGroupForConnection(Context.ConnectionId);
            var connection = group?.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (connection != null && group != null)
            {
                unitOfWork.MessageRepository.RemoveConnection(connection);
                if (await unitOfWork.Complete()) return group;
            }

            throw new Exception("Failed to remove from group");
        }

        private string GetGroupName(string caller, string? other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}
