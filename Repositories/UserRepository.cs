using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using OmniChat.Configurations;
using OmniChat.DTOs;
using OmniChat.Exceptions;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _usersCollection;
        public UserRepository(IOptions<MongoConfig> mongoConfig, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(mongoConfig.Value.DbName);
            _usersCollection = database.GetCollection<User>(mongoConfig.Value.Collections!.UserCols);
        }

        public IEnumerable<User> FindAllUsers()
        {
            return _usersCollection.Find(_ => true).ToEnumerable();
        }

        public async Task<User> FindByIdAsync(string id)
        {
            User user = await _usersCollection
                .Find(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new NotFoundException($"User with id {id} does not exist");
            }
            return user;
        }

        public async Task<User> FindByUsernameAsync(string username)
        {
            User user = await _usersCollection
                .Find(u => u.Username == username)
                .FirstOrDefaultAsync();
            return user;
        }

        public IEnumerable<UserDto> FindUsersByFriend(string providerId, IEnumerable<string> userIds, bool isIn = false)
        {
            var builder = Builders<User>.Filter;

            // is not in userIds
            var filter = builder.And(
                builder.Eq(x => x.ProviderId, providerId),
                builder.Nin(x => x.Id, userIds)
            );

            if (!isIn)
            {
                return _usersCollection
                    .Aggregate()
                    .Match(filter)
                    .AppendStage<BsonDocument>(new BsonDocument
                    {
                        { "$addFields", new BsonDocument
                            {
                                { "current_status", RelationshipStatus.unfollow } // Corrected the value to be a string
                            }
                        }
                    })
                    .Project(new BsonDocument{
                        { "_id", 1 },
                        { "provider_id", 1 },
                        { "username", 1 },
                        { "name", 1 },
                        { "first_name", 1 },
                        { "last_name", 1 },
                        { "avatar", 1 },
                        { "created_timestamp", 1 },
                        { "modified_timestamp", 1 },
                        { "current_status", 1 },
                    })
                    .As<UserDto>()
                    .ToEnumerable();
            }
            throw new NotImplementedException("Query not implemented");
        }

        public async Task<List<UserDto>> FindUsersByProviderId(UserRequest request)
        {

            return await _usersCollection.Find(u => u.ProviderId == request.ProviderId)
                .As<UserDto>()
                .ToListAsync();
        }

        public async Task InsertManyAsync(List<User> users)
        {
            await _usersCollection.InsertManyAsync(users);
        }

        public async Task InsertOneAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }

        public async Task UpdateProviderAsync(string userId, string providerId)
        {
            var update = Builders<User>.Update.Set(u => u.ProviderId, providerId);
            await _usersCollection.UpdateOneAsync(x => x.Id == userId, update);
        }
    }
}