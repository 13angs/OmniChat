using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OmniChat.Configurations;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Controllers
{
    [Route("api/v1/auth")]
    public class AuthenticationController : Controller
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IJwtService _jwtService;
        private readonly IAuthService _authService;
        public AuthenticationController(IOptions<MongoConfig> mongoConfig, IMongoClient mongoClient, IJwtService jwtService, IAuthService authService)
        {
            var database = mongoClient.GetDatabase(mongoConfig.Value.DbName);
            _usersCollection = database.GetCollection<User>(mongoConfig.Value.Collections!.UserCols);
            _jwtService = jwtService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                RegisterResponse response = await _authService.RegisterNewUserAsync(registerRequest);
                return Ok(new OkResponse<RegisterResponse>
                {
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await AuthenticateUserAsync(loginRequest.Username, loginRequest.Password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            var token = _jwtService.GenerateJwtToken(user);
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
    }
}