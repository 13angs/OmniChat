using System.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OmniChat.Configurations;
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

        public async Task<User> FindByIdAsync(string id)
        {
            User user = await _usersCollection
                .Find(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new DataException($"User with id {id} does not exist");
            }
            return user;
        }

        public async Task InsertOneAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }

        public async Task UpdateProviderAsync(string userId, string providerId)
        {
            var update = Builders<User>.Update.Set(u=>u.ProviderId, providerId);
            await _usersCollection.UpdateOneAsync(x=> x.Id==userId, update);
        }
    }
}