
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OmniChat.Configurations;
using OmniChat.Models;

namespace OmniChat.Repositories
{
    public class ChannelRepository
    {
        private readonly IMongoCollection<Channel> _channelsCollection;
        public ChannelRepository(IMongoClient mongoClient, IOptions<MongoConfig> mongoConfig)
        {
            var database = mongoClient.GetDatabase(mongoConfig.Value.DbName);
            _channelsCollection = database.GetCollection<Channel>(mongoConfig.Value.Collections!.ChannelCols);
        }
        public async Task<Channel> FindByClientIdAsync(ChannelRequest request)
        {
            return await _channelsCollection
                .Find(x => x.ChannelInfo.ClientId == request.ChannelInfo.ClientId)
                .FirstOrDefaultAsync();
        }
        public async Task InsertOneAsync(Channel channel)
        {
            await _channelsCollection.InsertOneAsync(channel);
        }
    }
}