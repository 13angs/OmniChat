using OmniChat.Models;

namespace OmniChat.Interfaces
{
    public interface IAuthService
    {
        public Task<AuthResponse> RegisterNewUserAsync(RegisterRequest request);
        public Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}