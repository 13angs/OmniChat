using Microsoft.AspNetCore.Mvc;
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

        [BsonElement("description")]
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("created_by")]
        [JsonProperty("created_by")]
        public required MessageUser CreatedBy { get; set; }

        [BsonElement("channel_info")]
        [JsonProperty("channel_info")]
        public required ChannelInfo ChannelInfo { get; set; }
    }

    public class ChannelInfo
    {
        [BsonElement("client_id")]
        [JsonProperty("client_id")]
        public required string ClientId { get; set; }

        [BsonElement("secret_id")]
        [JsonProperty("secret_id")]
        public required string SecretId { get; set; }

        [BsonElement("access_token")]
        [JsonProperty("access_token")]
        public required string AccessToken { get; set; }

        [BsonElement("line")]
        [JsonProperty("line")]
        public LineChannelInfo? Line { get; set; }
    }
    public class LineChannelInfo
    {
        [BsonElement("user_id")]
        [JsonProperty("user_id")]
        public required string UserId { get; set; }

        [BsonElement("basic_id")]
        [JsonProperty("basic_id")]
        public required string BasicId { get; set; }

        [BsonElement("display_name")]
        [JsonProperty("display_name")]
        public required string DisplayName { get; set; }

        [BsonElement("picture_url")]
        [JsonProperty("picture_url")]
        public required string PictureUrl { get; set; }

        [BsonElement("chat_mode")]
        [JsonProperty("chat_mode")]
        public required string ChatMode { get; set; }

        [BsonElement("mark_as_read_mode")]
        [JsonProperty("mark_as_read_mode")]
        public required string MarkAsReadMode { get; set; }
    }

    public class ChannelRequest : DefaultRequest
    {
        [BsonElement("provider_id")]
        [JsonProperty("provider_id")]
        [FromQuery(Name = "provider_id")]
        public required string ProviderId { get; set; }

        [BsonElement("platform")]
        [JsonProperty("platform")]
        [FromQuery(Name = "platform")]
        public required Platform Platform { get; set; }

        [BsonElement("channel_type")]
        [JsonProperty("channel_type")]
        public required ChannelType ChannelType { get; set; }

        [BsonElement("description")]
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("created_by")]
        [JsonProperty("created_by")]
        public MessageUser? CreatedBy { get; set; }

        [BsonElement("channel_info")]
        [JsonProperty("channel_info")]
        public ChannelInfo? ChannelInfo { get; set; }
    }

    public class ChannelResponse
    {
        [JsonProperty("channels")]
        public List<Channel>? Channels { get; set; }
    }
}