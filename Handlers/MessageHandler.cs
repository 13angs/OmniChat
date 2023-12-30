using OmniChat.Models;

namespace OmniChat.Handlers
{
    public static class MessageHandler
    {
        public static bool HandleSendMessage(MessageRequest request)
        {
            if (!string.IsNullOrEmpty(request.ProviderId) && request.ChannelType == ChannelType.user)
            {
                request.OperationMode = OperationMode.manual;
                request.MessageExchange = MessageExchange.push;
                request.Platform = Platform.in_house;
                return true;
            }
            else if (string.IsNullOrEmpty(request.ProviderId) && request.ChannelType == ChannelType.user)
            {
                throw new ArgumentNullException($"provider_id can not be null: {request.ProviderId}");
            }

            return false;
        }

        public static bool HandleGetMessages(MessageRequest request)
        {
            if (request.By==RequestParam.user && !string.IsNullOrEmpty(request.ProviderId) && request.From != null && request.To != null)
            {
                return true;
            }
            else
            {
                throw new BadHttpRequestException($"One or more property is not given(by,provider_id,from,to)");
            }
        }
    }
}