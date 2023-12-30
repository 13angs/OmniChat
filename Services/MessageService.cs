using System.Data;
using OmniChat.Handlers;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Services
{
    public class MessageService
    {
        private readonly IUserFriendRepository _userFriendRepo;
        private readonly IUserChannelRepository _userChannelRepo;
        private readonly IMessageRepository _messageRepo;
        public MessageService(IUserFriendRepository userFriendRepo, IUserChannelRepository userChannelRepo, IMessageRepository messageRepo)
        {
            _userFriendRepo = userFriendRepo;
            _userChannelRepo = userChannelRepo;
            _messageRepo = messageRepo;
        }
        public async Task SendMessageAsync(MessageRequest request)
        {
            if (MessageHandler.HandleSendMessage(request))
            {
                // find your friend
                UserFriend userFriend = await _userFriendRepo.FindFollowedUserAsync(request.To.UserId!, request.From.RefId!);

                if (userFriend == null)
                {
                    throw new DataException($"You haven't been friend with {request.To.Name} yet!");
                }

                UserChannel userChannel = await _userChannelRepo
                    .FindRelatedUsersAsync(request.From.RefId!, request.To.UserId!);

                // reset to is_read=true
                foreach (var relatedUser in userChannel.RelatedUsers)
                {
                    relatedUser.IsRead = true;
                }

                foreach (var relatedUser in userChannel.RelatedUsers)
                {
                    if (relatedUser.UserId == request.To.UserId)
                    {
                        relatedUser.IsRead = false;
                    }
                }

                await _userChannelRepo.ReplaceRelatedUsersAsync(userChannel);

                Message newMessage = new Message
                {
                    Platform = request.Platform,
                    ProviderId = request.ProviderId,
                    ChannelType = request.ChannelType,
                    OperationMode = request.OperationMode,
                    MessageExchange = request.MessageExchange,
                    MessageObject = request.MessageObject,
                    From = request.From,
                    User = request.To
                };

                await _messageRepo.InsertOneAsync(newMessage);
            }
        }
    
        public MessageResponse GetMessages(MessageRequest request)
        {
            // get messages for by=user
            if(MessageHandler.HandleGetMessages(request))
            {
                return new MessageResponse{
                    Messages=_messageRepo.FindMessagesByUserId(request)
                };
            }

            throw new NotImplementedException("Query is not implemented");
        }
    }
}