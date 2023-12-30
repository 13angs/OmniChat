using System.Data;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepo;

        public AuthService(IJwtService jwtService, IUserRepository userRepo)
        {
            _jwtService = jwtService;
            _userRepo = userRepo;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await AuthenticateUserAsync(request.Username, request.Password);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            var token = _jwtService.GenerateJwtToken(user);
            return new AuthResponse
            {
                Token = token
            };
        }

        public async Task<AuthResponse> RegisterNewUserAsync(RegisterRequest request)
        {
            // Check if the username is already taken
            var existingUser = await _userRepo.FindByUsernameAsync(request.Username);

            if (existingUser != null)
            {
                throw new DataException("Username already exists");
            }

            // Create a new user
            var newUser = new User
            {
                ProviderId = string.Empty,
                Username = request.Username,
                Name = request.Name,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            // Hash the password
            PasswordService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            newUser.PasswordHash = passwordHash;
            newUser.PasswordSalt = passwordSalt;

            // Insert the new user into the database
            await _userRepo.InsertOneAsync(newUser);

            // Generate a JWT token for the newly registered user
            var token = _jwtService.GenerateJwtToken(newUser);
            return new AuthResponse
            {
                Token = token
            };
        }

        private async Task<User> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepo.FindByUsernameAsync(username);

            if (user != null && PasswordService.VerifyPassword(password, user.PasswordHash!, user.PasswordSalt!))
            {
                return user;
            }

            throw new UnauthorizedAccessException($"AuthenticationController.AuthenticateUserAsync: Failed authenticating {username}");
        }
    }
}