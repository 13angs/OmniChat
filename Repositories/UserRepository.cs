using MongoDB.Driver;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _usersCollection;
        public UserRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("omni_db");
            _usersCollection = database.GetCollection<User>("users");
        }
        public async Task InsertOneAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }
    }
}