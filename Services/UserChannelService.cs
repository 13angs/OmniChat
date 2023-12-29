using System.Data;
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
        public async Task AddFriendAsync(UserChannelRequest request)
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
                return;
            }

            UserChannel userChannel = new UserChannel
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
                    new RelatedUser{
                        UserId=request.From.RefId!,
                    },
                    new RelatedUser{
                        UserId=request.To.UserId!,
                    }
                }
            };

            await _userChannelRepo.InsertOneAsync(userChannel);

            var friends = new List<UserFriend>
            {
                new UserFriend{ // from
                    ProviderId = request.ProviderId,
                    UserChannelId = userChannel.Id,
                    UserId = request.From.RefId!,
                    CurrentStatus = RelationshipStatus.follow,
                    RelatedUser = new RelatedUser
                    {
                        UserId = request.To.UserId!
                    }
                },
                new UserFriend{ // to
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
        }
    }
}