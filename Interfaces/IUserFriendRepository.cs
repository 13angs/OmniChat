using OmniChat.Models;

namespace OmniChat.Interfaces
{
    public interface IUserFriendRepository
    {
        public Task<UserFriend> FindFollowedUserAsync(string from, string to);
        public Task InsertOneAsync(UserFriend userFriend);
    }
}