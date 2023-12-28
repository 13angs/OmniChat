using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace OmniChat.Models
{
    public class UserChannel : BaseEntity
    {
        public UserChannel()
        {
            Profile = new UserChannelProfile(); 
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
        
        [BsonElement("manual_com")]
        [JsonProperty("manual_com")]
        public required MessageManualCom ManualCom { get; set; }
        
        [BsonElement("auto_com")]
        [JsonProperty("auto_com")]
        public required MessageAutoCom AutoCom { get; set; }
        
        [BsonElement("friends")]
        [JsonProperty("friends")]
        public required List<UserChannelFriend> Friends { get; set; }
        
        [BsonElement("latest_message")]
        [JsonProperty("latest_message")]
        public required string LatestMessage { get; set; }
        
        [BsonElement("profile")]
        [JsonProperty("profile")]
        public UserChannelProfile Profile { get; set; }
        
        [BsonElement("is_read")]
        [JsonProperty("is_read")]
        public bool IsRead { get; set; }
        
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
}