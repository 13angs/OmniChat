using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using OmniChat.Configurations;
using OmniChat.Interfaces;
using OmniChat.Models;
using OmniChat.Services;

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

        public async Task<List<UserChannel>> FindByProviderIdAsync(UserChannelRequest request)
        {
            var combinedSortDefinition = SortingService.Sort<UserChannel>(request.SortBy!, request.SortOrder!);
            return await _userChannelsCollection
                .Find(u => u.ProviderId == request.ProviderId)
                .Sort(combinedSortDefinition)
                .Limit(request.Limit)
                .ToListAsync();
        }

        public async Task<List<UserChannel>> FindByUserAsync(UserChannelRequest request)
        {
            var builder = Builders<UserChannel>.Filter;
            var combinedSortDefinition = SortingService.Sort<UserChannel>(request.SortBy!, request.SortOrder!);
            var filter = builder.And(
                builder.Eq(x => x.ProviderId, request.ProviderId),
                builder.ElemMatch(x => x.RelatedUsers, u => u.UserId == request.From.RefId)
            );

            return await _userChannelsCollection
                .Find(filter)
                .Sort(combinedSortDefinition)
                .Limit(request.Limit)
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

        public async Task<UpdateResult> ReadMessageAsync(UserChannelRequest request)
        {
            var builder = Builders<UserChannel>.Filter;
            var filter = builder.And(
                builder.Eq(x=>x.Id, request.UserChannelId),
                builder.ElemMatch(x=>x.RelatedUsers, u=>u.UserId==request.To.UserId)
            );

            var update = Builders<UserChannel>.Update.Set(x=>x.RelatedUsers.FirstMatchingElement().IsRead, true);
            return await _userChannelsCollection.UpdateOneAsync(filter, update);
        }

        public async Task<UserChannel> FindByIdAsync(string userChannelId)
        {
            return await _userChannelsCollection
                .Find(x=>x.Id==userChannelId)
                .FirstOrDefaultAsync();
        }

        public async Task ReplaceRelatedUsersAsync(UserChannel userChannel)
        {
            userChannel.ModifiedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            await _userChannelsCollection
                .ReplaceOneAsync(x => x.Id == userChannel.Id, userChannel);
        }
    }
}