using OmniChat.DTOs;
using OmniChat.Models;

namespace OmniChat.Interfaces
{
    public interface IUserRepository
    {
        public Task InsertOneAsync(User user);
        public Task<User> FindByIdAsync(string id);
        public Task<User> FindByUsernameAsync(string username);
        public Task UpdateProviderAsync(string userId, string providerId);
        public IEnumerable<User> FindAllUsers();
        public Task InsertManyAsync(List<User> users);
        public Task<List<UserDto>> FindUsersByProviderId(UserRequest request);
        public IEnumerable<UserDto> FindUsersByFriend(string providerId, IEnumerable<string> userIds, bool isIn=false);
    }
}