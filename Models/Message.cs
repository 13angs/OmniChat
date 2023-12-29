using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace OmniChat.Models
{
    public class Message : BaseEntity
    {   
        [BsonElement("platform")]
        [JsonProperty("platform")]
        public required string Platform { get; set; }
        
        [BsonElement("provider_id")]
        [JsonProperty("provider_id")]
        public required string ProviderId { get; set; }
        
        [BsonElement("channel_id")]
        [JsonProperty("channel_id")]
        public string? ChannelId { get; set; }

        // if Platform is "line" type can be (platform)
        // if Platform is "internal" type can be (user, group)
        [BsonElement("channel_type")]
        [JsonProperty("channel_type")]
        public required ChannelType ChannelType { get; set; }
        
        [BsonElement("com_type")]
        [JsonProperty("com_type")]
        public required string ComType { get; set; }

        [BsonElement("message_object")]
        [JsonProperty("message_object")]
        public required object MessageObject { get; set; }
    }

    public class MessageRequest
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; } = string.Empty;

        [JsonProperty("text")]
        public string? Text { get; set; }
    }

    public class MessageParams
    {
        [FromQuery(Name = "user_id")]
        public string UserId { get; set; } = string.Empty;
    }

    public class MessageFrom
    {
        [JsonProperty("ref_id")]
        [BsonElement("ref_id")]
        public string? RefId { get; set; }
        
        [JsonProperty("ref_name")]
        [BsonElement("ref_name")]
        public string? RefName { get; set; }
    }
    public class MessageTo
    {
        [JsonProperty("user_id")]
        [BsonElement("user_id")]
        public string? UserId { get; set; }
        
        [JsonProperty("name")]
        [BsonElement("name")]
        public string? Name { get; set; }
    }


}