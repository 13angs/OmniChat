using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using OmniChat.Models;

namespace OmniChat.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        public async Task AddToProvider()
        {
            string? providerId = Context?.User?.FindFirst("provider_id")?.Value;
            await Groups.AddToGroupAsync(Context!.ConnectionId, providerId!);
        }
        public async Task RemoveFromProvider()
        {
            string? providerId = Context?.User?.FindFirst("provider_id")?.Value;
            await Groups.RemoveFromGroupAsync(Context!.ConnectionId, providerId!);
        }
        public async Task SendMessageToProvider(Message message)
        {
            // ... (existing code for sending messages)
            // Broadcast the message to all clients
            await Clients.Group(message.ProviderId).SendAsync("ReceiveMessageFromProvider", message);
        }
        public async Task SendUserChannelToProvider(UserChannel UserChannel)
        {
            // ... (existing code for sending UserChannels)
            // Broadcast the UserChannel to all clients
            await Clients.Group(UserChannel.ProviderId).SendAsync("ReceiveUserChannelFromProvider", UserChannel);
        }
    }
}