using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private const string expireInMonths = "MONTH";
        private const string expireInDays = "DAY";
        private const string expireInHours = "HOUR";
        private const string expireInMinutes = "MINUTE";
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new("provider_id", user.ProviderId),
                    new("user_id", user.Id),
                    // Add additional claims if needed
                }),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Expires = GetExpireTime(),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private DateTime GetExpireTime()
        {
            _ = int.TryParse(_configuration["Jwt:Expires:Value"], out int expireValue);

            if (_configuration["Jwt:Expires:Type"] == expireInMonths)
            {
                return DateTime.UtcNow.AddDays(expireValue);
            }
            else if (_configuration["Jwt:Expires:Type"] == expireInDays)
            {
                return DateTime.UtcNow.AddDays(expireValue);
            }
            else if (_configuration["Jwt:Expires:Type"] == expireInHours)
            {
                return DateTime.UtcNow.AddHours(expireValue);
            }
            else if (_configuration["Jwt:Expires:Type"] == expireInMinutes)
            {
                return DateTime.UtcNow.AddMinutes(expireValue);
            }
            return DateTime.UtcNow.AddDays(expireValue);
        }

        public JwtPayloadData DecodeJwtPayloadData(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            string strPayload = jwtToken.Payload.SerializeToJson();
            return JsonConvert.DeserializeObject<JwtPayloadData>(strPayload)!;
        }
    }
}