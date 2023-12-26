using OmniChat.Models;

namespace OmniChat.Interfaces
{
    public interface IUserService
    {
        public Task<RegisterResponse> RegisterNewUserAsync(RegisterRequest request);
    }
}