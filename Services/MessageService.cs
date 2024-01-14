using System.Data;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using OmniChat.Handlers;
using OmniChat.Hubs;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Services
{
    public class MessageService
    {
        private readonly IUserFriendRepository _userFriendRepo;
        private readonly IUserChannelRepository _userChannelRepo;
        private readonly IMessageRepository _messageRepo;
        private readonly IHubContext<ChatHub> _chatHubContext;

        public MessageService(IUserFriendRepository userFriendRepo, IUserChannelRepository userChannelRepo, IMessageRepository messageRepo, IHubContext<ChatHub> chatHubContext)
        {
            _userFriendRepo = userFriendRepo;
            _userChannelRepo = userChannelRepo;
            _messageRepo = messageRepo;
            _chatHubContext = chatHubContext;
        }

        // Asynchronously sends a message and returns a response
        public async Task<OkResponse<string>> SendMessageAsync(MessageRequest request)
        {
            // Check if the message send action is handled
            if (MessageHandler.HandleSendUserMessage(request))
            {
                // Find the user friend to whom the message is being sent
                UserFriend userFriend = await _userFriendRepo.FindFollowedUserAsync(request.To.UserId!, request.From.RefId!);

                if (userFriend == null)
                {
                    // Throw an exception if the user is not a friend
                    throw new DataException($"You haven't been friends with {request.To.Name} yet!");
                }

                // Find the user channel related to the sender and receiver
                UserChannel userChannel = await _userChannelRepo
                    .FindRelatedUsersAsync(request.From.RefId!, request.To.UserId!);

                // Reset the 'is_read' property to true for all related users
                foreach (var relatedUser in userChannel.RelatedUsers)
                {
                    relatedUser.IsRead = true;
                }

                // Set 'is_read' to false for the target user
                foreach (var relatedUser in userChannel.RelatedUsers)
                {
                    if (relatedUser.UserId == request.From.RefId)
                    {
                        relatedUser.IsRead = false;
                    }
                }

                // Save the updated user channel
                await _userChannelRepo.ReplaceRelatedUsersAsync(userChannel);

                // Create a new message object
                Message newMessage = new()
                {
                    Platform = request.Platform,
                    ProviderId = request.ProviderId,
                    ChannelType = request.ChannelType,
                    UserChannelId = userChannel.Id,
                    OperationMode = request.OperationMode,
                    MessageExchange = request.MessageExchange,
                    MessageObject = request.MessageObject,
                    From = request.From,
                    User = request.To
                };

                // Insert the message into the repository
                await _messageRepo.InsertOneAsync(newMessage);

                // Notify clients about the new message using SignalR
                await _chatHubContext.Clients.Group(newMessage.ProviderId).SendAsync("ReceiveMessageFromProvider", JsonConvert.SerializeObject(newMessage));

                // Return a success response
                return new OkResponse<string>
                {
                    Message = $"Message sent to: {request.To.Name}"
                };
            }

            // Throw an exception if the action is not implemented
            throw new NotImplementedException("Action not implemented");
        }

        // Retrieves messages based on a given request
        public MessageResponse GetMessages(MessageRequest request)
        {
            // Check if the get messages action is handled
            if (MessageHandler.HandleGetMessages(request))
            {
                // Return a response with the messages found for the user
                return new MessageResponse
                {
                    Messages = _messageRepo.FindMessagesByUserId(request)
                };
            }

            // Throw an exception if the query is not implemented
            throw new NotImplementedException("Query is not implemented");
        }
    }
}
