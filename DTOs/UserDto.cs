using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using OmniChat.Models;

namespace OmniChat.DTOs
{
    public class UserDto : User
    {
        [JsonProperty("current_status")]
        [BsonElement("current_status")]
        public RelationshipStatus CurrentStatus { get; set; }
    }
}