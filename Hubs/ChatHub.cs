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
    }
}