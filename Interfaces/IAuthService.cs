using OmniChat.Models;

namespace OmniChat.Interfaces
{
    public interface IAuthService
    {
        public Task<RegisterResponse> RegisterNewUserAsync(RegisterRequest request);
    }
}