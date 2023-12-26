using System.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OmniChat.Configurations;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IPasswordService _pwdService;
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepo;

        public UserService(IOptions<MongoConfig> mongoConfig, IMongoClient mongoClient, IPasswordService pwdService, IJwtService jwtService, IUserRepository userRepo)
        {
            var database = mongoClient.GetDatabase(mongoConfig.Value.DbName);
            _usersCollection = database.GetCollection<User>(mongoConfig.Value.Collections!.UserCols);
            _pwdService = pwdService;
            _jwtService = jwtService;
            _userRepo = userRepo;
        }
        public async Task<RegisterResponse> RegisterNewUserAsync(RegisterRequest request)
        {
            // Check if the username is already taken
            var existingUser = await _usersCollection.Find(u => u.Username == request.Username).FirstOrDefaultAsync();

            if (existingUser != null)
            {
                throw new DataException("Username already exists");
            }

            // Create a new user
            var newUser = new User
            {
                ProviderId = string.Empty,
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            // Hash the password
            _pwdService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            newUser.PasswordHash = passwordHash;
            newUser.PasswordSalt = passwordSalt;

            // Insert the new user into the database
            await _userRepo.InsertOneAsync(newUser);

            // Generate a JWT token for the newly registered user
            var token = _jwtService.GenerateJwtToken(newUser);
            return new RegisterResponse
            {
                Token = token
            };
        }
    }
}