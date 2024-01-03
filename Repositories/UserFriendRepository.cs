using System.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OmniChat.Configurations;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Repositories
{
    public class UserFriendRepository : IUserFriendRepository
    {
        private readonly IUserRepository _userRepo;
        private readonly IMongoCollection<UserFriend> _userFriendsCollection;
        public UserFriendRepository(IOptions<MongoConfig> mongoConfig, IMongoClient mongoClient, IUserRepository userRepo)
        {
            var database = mongoClient.GetDatabase(mongoConfig.Value.DbName);
            _userFriendsCollection = database.GetCollection<UserFriend>(mongoConfig.Value.Collections!.UserFriendCols);
            _userRepo = userRepo;
        }
        public async Task<UserFriend> FindFollowedUserAsync(string from, string to)
        {
            User fromUser = await _userRepo.FindByIdAsync(from);
            if (fromUser == null)
            {
                throw new DataException("From(user) does not exist!");
            }

            User toUser = await _userRepo.FindByIdAsync(from);
            if (toUser == null)
            {
                throw new DataException("To(user) does not exist!");
            }

            return await _userFriendsCollection.Find(u => u.UserId == from &&
                u.RelatedUser.UserId == to)
                .FirstOrDefaultAsync();
        }

        public IEnumerable<UserFriend> FindUserFriendsByFriend(string providerId, string userId, RelationshipStatus currentStatus)
        {

            var builder = Builders<UserFriend>.Filter;

            // current_status=follow
            var filter = builder.And(
                builder.Eq(x => x.ProviderId, providerId),
                builder.Eq(x => x.UserId, userId),
                builder.Eq(x => x.CurrentStatus, currentStatus)
            );

            if (currentStatus == RelationshipStatus.follow)
            {
                return _userFriendsCollection
                    .Find(filter)
                    .ToEnumerable();
            }

            throw new NotImplementedException("Query not implemented");
        }

        public async Task InsertManyAsync(List<UserFriend> friends)
        {
            await _userFriendsCollection.InsertManyAsync(friends);
        }

        public async Task InsertOneAsync(UserFriend userFriend)
        {
            await _userFriendsCollection
                .InsertOneAsync(userFriend);
        }

        public async Task UpdateCurrentStatusAsync(UserFriend friend)
        {
            var update = Builders<UserFriend>.Update.Set(u => u.CurrentStatus, friend.CurrentStatus);
            await _userFriendsCollection
                .UpdateOneAsync(u => u.Id == friend.Id, update);
        }
    }
}