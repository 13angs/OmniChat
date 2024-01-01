using OmniChat.Models;

namespace OmniChat.Handlers
{
    public static class AuthHandler
    {
        public static bool HandleLogin(LoginRequest request)
        {
            if (!string.IsNullOrEmpty(request.Username) && !string.IsNullOrEmpty(request.Password))
            {
                return true;
            }

            return false;
        }
    }
}