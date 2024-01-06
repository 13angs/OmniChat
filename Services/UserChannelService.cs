using System.Data;
using OmniChat.Handlers;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Services
{
    public class UserChannelService
    {
        private readonly IUserChannelRepository _userChannelRepo;
        private readonly IUserFriendRepository _userFriendRepo;
        public UserChannelService(IUserChannelRepository userChannelRepo, IUserFriendRepository userFriendRepo)
        {
            _userChannelRepo = userChannelRepo;
            _userFriendRepo = userFriendRepo;
        }
        public async Task<OkResponse<string>> AddFriendAsync(UserChannelRequest request)
        {
            if (string.IsNullOrEmpty(request.ProviderId))
            {
                throw new ArgumentNullException(nameof(request), "provider_id can not be null");
            }
            if (request.From == null)
            {
                throw new ArgumentNullException(nameof(request), "from can not be null");
            }
            if (request.To == null)
            {
                throw new ArgumentNullException(nameof(request), "to can not be null");
            }

            bool friendExist = await _userChannelRepo
                .FindRelatedUsersAsync(request.From.RefId!, request.To.UserId!) != null;

            UserFriend userFriend = await _userFriendRepo
                .FindFollowedUserAsync(request.From.RefId!, request.To.UserId!);

            if (friendExist && userFriend.CurrentStatus == RelationshipStatus.follow)
            {
                throw new DataException("You already become friend");
            }
            else if (friendExist && (userFriend.CurrentStatus == RelationshipStatus.unfollow || userFriend.CurrentStatus == RelationshipStatus.blocked))
            {
                userFriend.CurrentStatus = RelationshipStatus.follow;
                await _userFriendRepo.UpdateCurrentStatusAsync(userFriend);
                return new OkResponse<string>
                {
                    Message = $"Added {request.To.Name} as friend"
                };
            }

            UserChannel userChannel = new()
            {
                Platform = Platform.in_house,
                ProviderId = request.ProviderId,
                ChannelType = ChannelType.user,
                OperationMode = OperationMode.manual,
                MessageExchange = MessageExchange.follow,
                From = request.From,
                To = request.To,
                LatestMessage = string.Empty,
                RelatedUsers = new List<RelatedUser>{
                    new() { // from
                        UserId=request.From.RefId!,
                        Avatar=request.From.Avatar,
                        Name=request.From.Name
                    },
                    new() { // to
                        UserId=request.To.UserId!,
                        Avatar=request.To.Avatar,
                        Name=request.To.Name
                    }
                }
            };

            await _userChannelRepo.InsertOneAsync(userChannel);

            var friends = new List<UserFriend>
            {
                new() { // from
                    ProviderId = request.ProviderId,
                    UserChannelId = userChannel.Id,
                    UserId = request.From.RefId!,
                    CurrentStatus = RelationshipStatus.follow,
                    RelatedUser = new RelatedUser
                    {
                        UserId = request.To.UserId!
                    }
                },
                new() { // to
                    ProviderId = request.ProviderId,
                    UserChannelId = userChannel.Id,
                    UserId = request.To.UserId!,
                    CurrentStatus = RelationshipStatus.unfollow,
                    RelatedUser = new RelatedUser
                    {
                        UserId = request.From.RefId!
                    }
                }
            };

            await _userFriendRepo.InsertManyAsync(friends);
            return new OkResponse<string>
            {
                Message = $"Added {request.To.Name} as friend"
            };
        }

        public async Task<UserChannelResponse> GetUserChannelsAsync(UserChannelRequest request)
        {
            // If by=provider, get all user_channels by provider_id
            if (UserChannelHandler.HandleGetUserChannels(request))
            {
                return new UserChannelResponse
                {
                    UserChannels = await _userChannelRepo.FindByProviderIdAsync(request.ProviderId)
                };
            }
            // If by=friend, get all user_channels by provider_id, user_id
            else if (UserChannelHandler.HandleGetUserChannelsByUser(request))
            {
                return new UserChannelResponse
                {
                    UserChannels = await _userChannelRepo
                        .FindByUserAsync(request.ProviderId, request.From.RefId!)
                };
            }

            throw new NotImplementedException($"by={request.By} is not implemented");
        }
    }
}