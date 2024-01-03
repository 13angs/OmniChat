using OmniChat.Models;

namespace OmniChat.Interfaces
{
    public interface IUserFriendRepository
    {
        public Task<UserFriend> FindFollowedUserAsync(string from, string to);
        public Task InsertOneAsync(UserFriend userFriend);
        public Task InsertManyAsync(List<UserFriend> friends);
        public Task UpdateCurrentStatusAsync(UserFriend friend);
        public IEnumerable<UserFriend> FindUserFriendsByFriend(string providerId, string userId, RelationshipStatus currentStatus);
    }
}