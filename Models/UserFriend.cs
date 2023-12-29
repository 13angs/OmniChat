using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace OmniChat.Models
{
    public class UserFriend : BaseEntity
    {
        [BsonElement("provider_id")]
        [JsonProperty("provider_id")]
        public required string ProviderId { get; set; }

        [BsonElement("user_channel_id")]
        [JsonProperty("user_channel_id")]
        public required string UserChannelId { get; set; }

        [BsonElement("user_id")]
        [JsonProperty("user_id")]
        public required string UserId { get; set; }

        [BsonElement("current_status")]
        [JsonProperty("current_status")]
        public required RelationshipStatus CurrentStatus { get; set; }
        [BsonElement("related_user")]
        [JsonProperty("related_user")]
        public required RelatedUser RelatedUser { get; set; }
    }
}