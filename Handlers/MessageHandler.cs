using OmniChat.Models;

namespace OmniChat.Handlers
{
    public static class MessageHandler
    {
        public static bool HandleSendMessage(MessageRequest request)
        {
            if (!string.IsNullOrEmpty(request.ProviderId) && request.ChannelType == ChannelType.user)
            {
                request.OperationMode=OperationMode.manual;
                request.MessageExchange=MessageExchange.push;
                request.Platform=Platform.in_house;
                return true;
            }
            else if (string.IsNullOrEmpty(request.ProviderId) && request.ChannelType == ChannelType.user)
            {
                throw new ArgumentNullException($"provider_id can not be null: {request.ProviderId}");
            }

            return false;
        }
    }
}