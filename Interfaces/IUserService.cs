using OmniChat.Models;

namespace OmniChat.Interfaces
{
    public interface IUserService
    {
        public Task<UserResponse> GetUsersAsync(UserRequest request);
        public Task<UserResponse> GetMyProfileAsync(UserRequest request);
        public Task<UserResponse> GetUserProfileAsync(UserRequest request);
    }
}