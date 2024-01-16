using MongoDB.Driver;
using OmniChat.Models;

namespace OmniChat.Interfaces
{
    public interface IUserChannelRepository
    {
        public Task<UserChannel> FindRelatedUsersAsync(string from, string to);
        public Task InsertOneAsync(UserChannel userChannel);
        public Task<List<UserChannel>> FindByProviderIdAsync(UserChannelRequest request);
        public Task ReplaceRelatedUsersAsync(UserChannel userChannel);
        public Task<List<UserChannel>> FindByUserAsync(UserChannelRequest request);
        public Task<UpdateResult> ReadMessageAsync(UserChannelRequest request);
        public Task<UserChannel> FindByIdAsync(string userChannelId);
    }
}