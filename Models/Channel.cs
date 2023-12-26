using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace OmniChat.Models
{
    public class Channel : BaseEntity
    {
        [BsonElement("platform")]
        [JsonProperty("platform")]
        public required Platform Platform { get; set; }
        
        [BsonElement("provider_id")]
        [JsonProperty("provider_id")]
        public required string ProviderId { get; set; }
        
        [BsonElement("channel_type")]
        [JsonProperty("channel_type")]
        public required ChannelType ChannelType { get; set; }

        [BsonElement("name")]
        [JsonProperty("name")]
        public required string Name { get; set; }

        [BsonElement("description")]
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;
        
        [BsonElement("created_by")]
        [JsonProperty("created_by")]
        public required string CreatedBy { get; set; }
    }
}