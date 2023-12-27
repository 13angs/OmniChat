using OmniChat.Models;

namespace OmniChat.Interfaces
{
    public interface IUserService
    {
        public Task<UserResponse> GetUsersAsync(UserRequest request);
    }
}