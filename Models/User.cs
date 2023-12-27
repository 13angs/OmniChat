using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace OmniChat.Models
{
    public class User : BaseEntity
    {
        [BsonElement("provider_id")]
        [JsonProperty("provider_id")]
        public required string ProviderId { get; set; }

        [BsonElement("username")]
        [JsonProperty("username")]
        public required string Username { get; set; }

        [BsonElement("password_hash")]
        [JsonProperty("password_hash")]
        public byte[]? PasswordHash { get; set; }

        [BsonElement("password_salt")]
        [JsonProperty("password_salt")]
        public byte[]? PasswordSalt { get; set; }

        [BsonElement("first_name")]
        [JsonProperty("first_name")]
        public required string FirstName { get; set; }

        [BsonElement("last_name")]
        [JsonProperty("last_name")]
        public required string LastName { get; set; }

        [BsonElement("avatar")]
        [JsonProperty("avatar")]
        public string? Avatar { get; set; }
    }

    public class LoginRequest
    {
        [JsonProperty("username")]
        public required string Username { get; set; }

        [JsonProperty("password")]
        public required string Password { get; set; }
    }

    public class RegisterRequest
    {
        [JsonProperty("username")]
        public required string Username { get; set; }

        [JsonProperty("password")]
        public required string Password { get; set; }

        [JsonProperty("first_name")]
        public required string FirstName { get; set; }

        [JsonProperty("last_name")]
        public required string LastName { get; set; }
    }
    public class RegisterResponse
    {
        [JsonProperty("token")]
        public string? Token { get; set; }
    }

    public class UserResponse
    {
        public UserResponse()
        {
            Users = new List<User>();
        }

        [JsonProperty("users", NullValueHandling = NullValueHandling.Ignore)]
        public List<User> Users { get; set; }
    }

    public class UserRequest : DefaultRequest
    {
        [FromQuery(Name = "provider_id")]
        public string? ProviderId { get; set; }
    }
}