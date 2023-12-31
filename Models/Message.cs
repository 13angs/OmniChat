using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace OmniChat.Models
{
    public class Message : BaseEntity
    {
        [BsonElement("platform")]
        [JsonProperty("platform")]
        public required Platform Platform { get; set; }

        [BsonElement("provider_id")]
        [JsonProperty("provider_id")]
        public required string ProviderId { get; set; }

        [BsonElement("channel_id")]
        [JsonProperty("channel_id")]
        public string? ChannelId { get; set; }

        [BsonElement("channel_type")]
        [JsonProperty("channel_type")]
        public required ChannelType ChannelType { get; set; }

        [BsonElement("operation_mode")]
        [JsonProperty("operation_mode")]
        public required OperationMode OperationMode { get; set; }

        [BsonElement("message_exchange")]
        [JsonProperty("message_exchange")]
        public required MessageExchange MessageExchange { get; set; }

        [BsonElement("message_object")]
        [JsonProperty("message_object")]
        public required object MessageObject { get; set; }

        [BsonElement("from")]
        [JsonProperty("from")]
        public required MessageFrom From { get; set; }

        [BsonElement("to")]
        [JsonProperty("to")]
        public required MessageUser User { get; set; }
    }

    public class MessageRequest : DefaultRequest
    {
        [JsonProperty("provider_id")]
        [FromQuery(Name = "provider_id")]
        public required string ProviderId { get; set; }
        
        [BsonElement("platform")]
        [JsonProperty("platform")]
        public required Platform Platform { get; set; }

        [JsonProperty("channel_type")]
        public required ChannelType ChannelType { get; set; }

        [BsonElement("operation_mode")]
        [JsonProperty("operation_mode")]
        public required OperationMode OperationMode { get; set; }

        [BsonElement("message_exchange")]
        [JsonProperty("message_exchange")]
        public required MessageExchange MessageExchange { get; set; }
        
        [BsonElement("message_object")]
        [JsonProperty("message_object")]
        public required object MessageObject { get; set; }

        [BsonElement("from")]
        [JsonProperty("from")]
        [FromQuery(Name = "from")]
        public required MessageFrom From { get; set; }

        [BsonElement("to")]
        [JsonProperty("to")]
        [FromQuery(Name = "to")]
        public required MessageUser To { get; set; }
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
        [FromQuery(Name = "ref_id")]
        public string? RefId { get; set; }

        [JsonProperty("name")]
        [BsonElement("name")]
        [FromQuery(Name = "name")]
        public string? Name { get; set; }
    }
    public class MessageUser
    {
        [JsonProperty("user_id")]
        [BsonElement("user_id")]
        [FromQuery(Name = "user_id")]
        public string? UserId { get; set; }

        [JsonProperty("name")]
        [BsonElement("name")]
        [FromQuery(Name = "name")]
        public string? Name { get; set; }
    }

    public class MessageResponse
    {
        public MessageResponse()
        {
            Messages=new List<Message>();
        }

        [JsonProperty("messages")]
        public IEnumerable<Message> Messages { get; set; }
    }
}