using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OmniChat.Configurations;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Repositories
{
    public class UserChannelRepository : IUserChannelRepository
    {
        private readonly IMongoCollection<UserChannel> _userChannelsCollection;
        public UserChannelRepository(IOptions<MongoConfig> mongoConfig, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(mongoConfig.Value.DbName);
            _userChannelsCollection = database.GetCollection<UserChannel>(mongoConfig.Value.Collections!.UserChannelCols);
        }

        public async Task<List<UserChannel>> FindByProviderIdAsync(string providerId)
        {
            return await _userChannelsCollection
                .Find(u => u.ProviderId == providerId)
                .ToListAsync();
        }

        public async Task<UserChannel> FindRelatedUsersAsync(string from, string to)
        {

            var filter = Builders<UserChannel>.Filter.ElemMatch(x => x.RelatedUsers, u => u.UserId == from) &
                     Builders<UserChannel>.Filter.ElemMatch(x => x.RelatedUsers, u => u.UserId == to);

            UserChannel userChannel = await _userChannelsCollection
                .Find(filter)
                .FirstOrDefaultAsync();
            return userChannel;
        }

        public async Task InsertOneAsync(UserChannel userChannel)
        {
            await _userChannelsCollection.InsertOneAsync(userChannel);
        }

        public async Task ReplaceRelatedUsersAsync(UserChannel userChannel)
        {
            userChannel.ModifiedTimestamp=DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            await _userChannelsCollection
                .ReplaceOneAsync(x=>x.Id==userChannel.Id, userChannel);
        }
    }
}