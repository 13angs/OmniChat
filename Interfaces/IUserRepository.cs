using OmniChat.Models;

namespace OmniChat.Interfaces
{
    public interface IUserRepository
    {
        public Task InsertOneAsync(User user);
        public Task<User> FindByIdAsync(string id);
        public Task UpdateProviderAsync(string userId, string providerId);
        public IEnumerable<User> FindAllUsers();
        public Task InsertManyAsync(List<User> users);
    }
}