using OmniChat.Models;

namespace OmniChat.Handlers
{
    public static class UserChannelHandler
    {
        public static bool HandleGetUserChannels(UserChannelRequest request)
        {
            if (request.By == RequestParam.provider && !string.IsNullOrEmpty(request.ProviderId))
            {
                return true;
            }
            else if (request.By == RequestParam.provider && string.IsNullOrEmpty(request.ProviderId))
            {
                throw new ArgumentNullException($"provider_id can not be null: ${request.ProviderId}");
            }

            return false;
        }
    }
}