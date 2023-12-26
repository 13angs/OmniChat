using Newtonsoft.Json;

namespace OmniChat.Models
{
    public class ErrorResponse
    {
        [JsonProperty("message")]
        public required string Message { get; set; }
        
        [JsonProperty("status")]
        public required int Status { get; set; }
    }
}