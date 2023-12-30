using OmniChat.Models;

namespace OmniChat.Interfaces
{
    public interface IUserChannelRepository
    {
        public Task<UserChannel> FindRelatedUsersAsync(string from, string to);
        public Task InsertOneAsync(UserChannel userChannel);
        public Task<List<UserChannel>> FindByProviderIdAsync(string providerId);
        public Task ReplaceRelatedUsersAsync(UserChannel userChannel);
    }
}