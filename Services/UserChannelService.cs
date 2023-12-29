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

            UserFriend userFriend = await _userFriendRepo
                .FindFollowedUserAsync(request.From.RefId!, request.To.UserId!);

            if (userFriend != null)
            {
                throw new DataException("You already become friend");
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
                    new RelatedUser{ // for from
                        UserId=request.From.RefId!,
                        IsRead=true,
                        CurrentStatus=RelationshipStatus.follow
                    }
                }
            };
            userChannel.RelatedUsers.Add(new RelatedUser
            { // for to
                UserId = request.To.UserId!,
                IsRead = false,
                CurrentStatus = RelationshipStatus.unfollow
            });

            await _userChannelRepo.InsertOneAsync(userChannel);

            userFriend = new UserFriend
            {
                ProviderId = request.ProviderId,
                UserChannelId = userChannel.Id,
                UserId = request.From.RefId!,
                CurrentStatus = RelationshipStatus.follow,
                RelatedUser = new RelatedUser
                {
                    UserId = request.To.UserId!,
                    CurrentStatus = RelationshipStatus.unfollow,
                }
            };

            await _userFriendRepo.InsertOneAsync(userFriend);
        }
    }
}