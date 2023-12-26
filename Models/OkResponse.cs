using Newtonsoft.Json;

namespace OmniChat.Models
{
    public class OkResponse<T>
    {
        [JsonProperty("data")]
        public required T Data { get; set; }
    }
}