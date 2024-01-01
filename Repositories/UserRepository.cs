using System.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OmniChat.Configurations;
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

        public async Task<List<User>> FindUsersByProviderId(UserRequest request)
        {

            return await _usersCollection.Find(u => u.ProviderId == request.ProviderId)
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