using OmniChat.Models;

namespace OmniChat.Interfaces
{
    public interface IJwtService
    {
        public string GenerateJwtToken(User user);
        public JwtPayloadData DecodeJwtPayloadData(string token);
    }
}