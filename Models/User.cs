using System.ComponentModel.DataAnnotations;
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
        [JsonIgnore]
        public byte[]? PasswordHash { get; set; }

        [BsonElement("password_salt")]
        [JsonProperty("password_salt")]
        [JsonIgnore]
        public byte[]? PasswordSalt { get; set; }

        [BsonElement("name")]
        [JsonProperty("name")]
        public required string Name { get; set; }
        
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
        [Required]
        public required string Username { get; set; }

        [JsonProperty("password")]
        [Required]
        public required string Password { get; set; }
    }

    public class RegisterRequest : LoginRequest
    {
        [JsonProperty("display_name")]
        public required string Name { get; set; }
        
        [JsonProperty("first_name")]
        public required string FirstName { get; set; }

        [JsonProperty("last_name")]
        public required string LastName { get; set; }
    }
    public class AuthResponse
    {
        [JsonProperty("token")]
        public string? Token { get; set; }
    }

    public class UserResponse
    {
        [JsonProperty("users", NullValueHandling = NullValueHandling.Ignore)]
        public List<User>? Users { get; set; }
        
        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public User? User { get; set; }
    }

    public class UserRequest : DefaultRequest
    {
        [FromQuery(Name = "provider_id")]
        public string? ProviderId { get; set; }
        
        [JsonProperty("token", NullValueHandling=NullValueHandling.Ignore)]
        [FromQuery(Name = "token")]
        public string? Token { get; set; }
        
        // [JsonProperty("user_id")]
        [FromQuery(Name = "user_id")]
        public string? UserId { get; set; }
    }

    public class RelatedUser
    {
        [JsonProperty("user_id")]
        [BsonElement("user_id")]
        public required string UserId { get; set; }
        
        [JsonProperty("is_read")]
        [BsonElement("is_read")]
        public bool IsRead { get; set; }
    }
}