using Microsoft.AspNetCore.SignalR;
using OmniChat.Models;

namespace OmniChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task AddToProvider(string providerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, providerId);
        }
        public async Task RemoveFromProvider(string providerId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, providerId);
        }
        public async Task SendMessageToProvider(Message message)
        {
            // ... (existing code for sending messages)
            // Broadcast the message to all clients
            await Clients.Group(message.ProviderId).SendAsync("ReceiveMessageFromProvider", message);
        }
    }
}