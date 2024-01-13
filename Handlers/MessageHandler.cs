using OmniChat.Models;

namespace OmniChat.Handlers
{
    public static class MessageHandler
    {
        public static bool HandleSendUserMessage(MessageRequest request)
        {
            if (RequiredPropsHandler.HandleRequiredProps("provider_id", request.ProviderId) && 
                request.ChannelType == ChannelType.user)
            {
                request.OperationMode = OperationMode.manual;
                request.MessageExchange = MessageExchange.push;
                request.Platform = Platform.in_house;
                return true;
            }
            throw new NotImplementedException("Action not implemented");
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