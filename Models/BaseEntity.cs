using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace OmniChat.Models
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            CreatedTimestamp = timestamp;
            ModifiedTimestamp = timestamp;
        }
        [BsonElement("_id")]
        [JsonProperty("_id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [BsonElement("created_timestamp")]
        [JsonProperty("created_timestamp")]
        public long CreatedTimestamp { get; set; }

        [BsonElement("modified_timestamp")]
        [JsonProperty("modified_timestamp")]
        public long ModifiedTimestamp { get; set; }
    }
}