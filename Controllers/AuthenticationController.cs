using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using OmniChat.Hubs;
using OmniChat.Models;

namespace OmniChat.Controllers
{
    [Route("api/v1/auth")]
    public class AuthenticationController : Controller
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IMongoClient mongoClient, IHubContext<ChatHub> chatHubContext, IConfiguration configuration)
        {
            var database = mongoClient.GetDatabase("omni_db");
            _usersCollection = database.GetCollection<User>("users");
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody]RegisterRequest registerRequest)
        {
            // Check if the username is already taken
            var existingUser = await _usersCollection.Find(u => u.Username == registerRequest.Username).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return BadRequest("Username already exists");
            }

            // Create a new user
            var newUser = new User
            {
                ProviderId = string.Empty,
                Username = registerRequest.Username,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
            };

            Console.WriteLine(registerRequest.Password);

            // Hash the password
            PasswordHasher.CreatePasswordHash(registerRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);

            newUser.PasswordHash = passwordHash;
            newUser.PasswordSalt = passwordSalt;

            // Insert the new user into the database
            await _usersCollection.InsertOneAsync(newUser);

            // Generate a JWT token for the newly registered user
            var token = GenerateJwtToken(newUser);

            return Ok(token);
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await AuthenticateUserAsync(loginRequest.Username, loginRequest.Password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            var token = GenerateJwtToken(user);
            return Ok(token);
        }

        private async Task<User> AuthenticateUserAsync(string username, string password)
        {
            var user = await _usersCollection.Find(u => u.Username == username).FirstOrDefaultAsync();

            if (user != null && VerifyPassword(password, user.PasswordHash!, user.PasswordSalt!))
            {
                return user;
            }

            throw new UnauthorizedAccessException($"AuthenticationController.AuthenticateUserAsync: Failed authenticating {username}");
        }

        private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                        return false;
                }
            }
            return true;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id),
                    // Add additional claims if needed
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public static class PasswordHasher
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}