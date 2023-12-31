using OmniChat.Models;

namespace OmniChat.Handlers
{
    public static class UserHandler
    {
        public static bool HandleGetMyProfile(UserRequest request)
        {
            if (!string.IsNullOrEmpty(request.Token))
            {
                return true;
            }

            throw new BadHttpRequestException("token can not be null");
        }
    }
}