using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace OmniChat.Models
{
    public class UserChannel : BaseEntity
    {
        public UserChannel()
        {
            RelatedUsers = new List<RelatedUser>();
        }

        [BsonElement("platform")]
        [JsonProperty("platform")]
        public required Platform Platform { get; set; }

        [BsonElement("provider_id")]
        [JsonProperty("provider_id")]
        public required string ProviderId { get; set; }

        [BsonElement("channel_id")]
        [JsonProperty("channel_id")]
        public string ChannelId { get; set; } = string.Empty;

        [BsonElement("channel_type")]
        [JsonProperty("channel_type")]
        public required ChannelType ChannelType { get; set; } // "user" or "group" or "platform"

        [BsonElement("operation_mode")]
        [JsonProperty("operation_mode")]
        public required OperationMode OperationMode { get; set; }

        [BsonElement("message_exchange")]
        [JsonProperty("message_exchange")]
        public required MessageExchange MessageExchange { get; set; }

        [BsonElement("from")]
        [JsonProperty("from")]
        public required MessageFrom From { get; set; }

        [BsonElement("to")]
        [JsonProperty("to")]
        public required MessageTo To { get; set; }

        [BsonElement("latest_message")]
        [JsonProperty("latest_message")]
        public required string LatestMessage { get; set; }

        [BsonElement("is_read")]
        [JsonProperty("is_read")]
        public bool IsRead { get; set; }

        [BsonElement("related_users")]
        [JsonProperty("related_users")]
        public List<RelatedUser> RelatedUsers { get; set; }

    }

    public class UserChannelFriend
    {
        [BsonElement("user_id")]
        [JsonProperty("user_id")]
        public required string UserId { get; set; }

        [BsonElement("first_name")]
        [JsonProperty("first_name")]
        public required string FirstName { get; set; }

        [BsonElement("avatar")]
        [JsonProperty("avatar")]
        public virtual string Avatar { get; set; } = string.Empty;
    }

    public class UserChannelProfile
    {
        [BsonElement("user_id")]
        [JsonProperty("user_id")]
        public string? UserId { get; set; }

        [BsonElement("first_name")]
        [JsonProperty("first_name")]
        public string? FirstName { get; set; }

        [BsonElement("avatar")]
        [JsonProperty("avatar")]
        public string? Avatar { get; set; }
    }

    public class UserChannelRequest
    {
        [BsonElement("platform")]
        [JsonProperty("platform")]
        public required Platform Platform { get; set; }

        [BsonElement("provider_id")]
        [JsonProperty("provider_id")]
        public required string ProviderId { get; set; }
        [BsonElement("channel_type")]
        [JsonProperty("channel_type")]
        public required ChannelType ChannelType { get; set; } // "user" or "group" or "platform"

        [BsonElement("operation_mode")]
        [JsonProperty("operation_mode")]
        public required OperationMode OperationMode { get; set; }

        [BsonElement("message_exchange")]
        [JsonProperty("message_exchange")]
        public required MessageExchange MessageExchange { get; set; }

        [BsonElement("from")]
        [JsonProperty("from")]
        public required MessageFrom From { get; set; }

        [BsonElement("to")]
        [JsonProperty("to")]
        public required MessageTo To { get; set; }
    }
}