using OmniChat.Models;

namespace OmniChat.Handlers
{
    public static class ChannelHandler
    {
        public static bool HandleCreateChannel(ChannelRequest request)
        {
            if (RequiredPropsHandler.HandleRequiredProps("provider_id", request.ProviderId) &&
                request.ChannelInfo != null &&
                request.CreatedBy != null &&
                RequiredPropsHandler.HandleRequiredProps("client_id", request.ChannelInfo.ClientId) &&
                RequiredPropsHandler.HandleRequiredProps("secret_id", request.ChannelInfo.SecretId) &&
                RequiredPropsHandler.HandleRequiredProps("access_token", request.ChannelInfo.AccessToken) &&
                RequiredPropsHandler.HandleRequiredProps("user_id", request.CreatedBy.UserId) &&
                RequiredPropsHandler.HandleRequiredProps("name", request.CreatedBy.Name))
            {
                return true;
            }

            throw new BadHttpRequestException("Action not implemented");
        }
    }
}