using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Services
{
    public class DataSeedingService
    {
        private readonly IUserRepository _userRepo;
        private readonly ILogger<DataSeedingService> _logger;
        public DataSeedingService(IUserRepository userRepo, ILogger<DataSeedingService> logger)
        {
            _userRepo = userRepo;
            _logger = logger;
        }
        public async Task SeedUsersAsync()
        {
            int userCount = _userRepo.FindAllUsers().Count();

            if (userCount == 2)
            {
                // do something here
                byte[] passwordHash, passwordSalt;
                string password = "P@ssw0rd";
                string[] providers = new string[] { "0087ca8e-6914-4839-8a4e-b616556d4425", "acac9094-c519-4f9e-9882-d82487672ae2" };

                foreach (string providerId in providers)
                {
                    PasswordService.CreatePasswordHash(password, out passwordHash, out passwordSalt);
                    var users = GenerateUserList(passwordHash, passwordSalt, providerId);
                    await _userRepo.InsertManyAsync(users);
                }
            }
            else if (userCount > 2)
            {
                _logger.LogInformation("Users is greater than 2");
            }
            else if (userCount < 2)
            {
                _logger.LogInformation("Users is less than 2");
            }
        }

        private static List<User> GenerateUserList(byte[] passwordHash, byte[] passwordSalt, string providerId)
        {
            var users = new List<User>();

            Random random = new Random();

            for (int i = 1; i <= 25; i++)
            {
                string randomName = RandomValueService.GenerateRandomString(random, 5); // You can specify the length you desire
                string randomUsername = $"user-{randomName}"; // You can specify the length you desire

                users.Add(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    ProviderId = providerId,
                    FirstName = randomName,
                    LastName = randomName,
                    Username = randomUsername,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                });
            }

            return users;
        }
    }
}