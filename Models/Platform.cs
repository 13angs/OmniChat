using Newtonsoft.Json;

namespace OmniChat.Models
{
    public enum Platform
    {
        in_house,
        line
    }

    public class PlatformResponse
    {
        [JsonProperty("platforms")]
        public List<string>? Platforms { get; set; }

        [JsonProperty("platform_names")]
        public Dictionary<string, string>? PlatformNames { get; set; }
    }
}