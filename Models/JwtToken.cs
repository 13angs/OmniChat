using Newtonsoft.Json;

namespace OmniChat.Models
{
    public class JwtPayloadData
    {
        [JsonProperty("user_id")]
        public string? UserId { get; set; }

        [JsonProperty("nbf")]
        public long Nbf { get; set; }

        [JsonProperty("exp")]
        public long Exp { get; set; }

        [JsonProperty("iat")]
        public long Iat { get; set; }
    }
}