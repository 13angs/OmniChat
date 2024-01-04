using Newtonsoft.Json;

namespace OmniChat.Models
{
    public class OkResponse<T>
    {
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public T? Data { get; set; }
        
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string? Message { get; set; }
    }
}