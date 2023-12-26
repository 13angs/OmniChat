using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Newtonsoft.Json;
using OmniChat.Hubs;
using OmniChat.Models;

namespace OmniChat.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IMongoCollection<Message> _messagesCollection;
        private readonly IHubContext<ChatHub> _chatHubContext;
        private readonly IConfiguration _configuration;

        public ChatController(IMongoClient mongoClient, IHubContext<ChatHub> chatHubContext, IConfiguration configuration)
        {
            var database = mongoClient.GetDatabase("omni_db");
            _usersCollection = database.GetCollection<User>("users");
            _messagesCollection = database.GetCollection<Message>("messages");
            _chatHubContext = chatHubContext;
            _configuration = configuration;
        }

        // [HttpGet("users")]
        // // [Authorize]
        // public ActionResult<IEnumerable<User>> GetUsers()
        // {
        //     var users = _usersCollection.Find(user => true).ToList();
        //     return Ok(users);
        // }

        // [HttpPost("sendMessage")]
        // // [Authorize]
        // public async Task<ActionResult> SendMessage(MessageRequest messageRequest)
        // {
        //     if (messageRequest == null || string.IsNullOrWhiteSpace(messageRequest.Text) || string.IsNullOrEmpty(messageRequest.UserId))
        //     {
        //         return BadRequest("Invalid message request");
        //     }

        //     User selectedUser = await _usersCollection.Find(u => u.Id == messageRequest.UserId).FirstOrDefaultAsync();

        //     if (selectedUser == null)
        //     {
        //         return NotFound("User not found");
        //     }

        //     var newMessage = new Message
        //     {
        //         Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
        //     };
        //     await _messagesCollection.InsertOneAsync(newMessage);

        //     // Notify clients about the new message using SignalR
        //     await _chatHubContext.Clients.All.SendAsync("ReceiveMessage", JsonConvert.SerializeObject(newMessage));

        //     return Ok();
        // }

        // [HttpGet("messages")]
        // // [Authorize]
        // public ActionResult<IEnumerable<Message>> GetUserMessages([FromQuery] MessageParams messageParams)
        // {
        //     if (string.IsNullOrEmpty(messageParams.UserId))
        //     {
        //         var messages = _messagesCollection.Find(message => true).ToList();
        //         return Ok(messages);
        //     }

        //     var userMessages = _messagesCollection.Find(message => message.User.To == messageParams.UserId).ToList();

        //     if (userMessages == null || userMessages.Count == 0)
        //     {
        //         return NotFound("No messages found for the specified user");
        //     }

        //     return Ok(userMessages);
        // }
    }
}