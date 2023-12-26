using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace OmniChat.Models
{
    public class Provider : BaseEntity
    {   
        [BsonElement("name")]
        [JsonProperty("name")]
        public required string Name { get; set; }
        
        [BsonElement("description")]
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("owner_id")]
        [JsonProperty("owner_id")]
        public required string OwnerId { get; set; }
    }
}