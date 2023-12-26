using OmniChat.Models;

namespace OmniChat.Interfaces
{
    public interface IUserRepository
    {
        public Task InsertOneAsync(User user);
    }
}