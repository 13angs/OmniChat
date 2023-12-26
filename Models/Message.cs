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
        
        [BsonElement("manual_com")]
        [JsonProperty("manual_com")]
        public required MessageManualCom ManualCom { get; set; }
        
        [BsonElement("auto_com")]
        [JsonProperty("auto_com")]
        public required MessageAutoCom AutoCom { get; set; }
    }

    public class MessageManualCom
    {
        [BsonElement("from")]
        [JsonProperty("from")]
        public required string From { get; set; }
        
        [BsonElement("from_name")]
        [JsonProperty("from_name")]
        public required string FromName { get; set; }
        
        [BsonElement("to")]
        [JsonProperty("to")]
        public required string To { get; set; }
        
        [BsonElement("to_name")]
        [JsonProperty("to_name")]
        public required string ToName { get; set; }
    }
    public class MessageAutoCom
    {
        [BsonElement("ma_type")]
        [JsonProperty("ma_type")]
        public required MaType MaType { get; set; }
        
        [BsonElement("ma_id")]
        [JsonProperty("ma_id")]
        public required string MaId { get; set; }
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
}