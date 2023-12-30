using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Services
{
    public class DataSeedingService
    {
        private readonly IUserRepository _userRepo;
        private readonly IProviderService _providerService;
        public DataSeedingService(IUserRepository userRepo, IProviderService providerService)
        {
            _userRepo = userRepo;
            _providerService = providerService;
        }
        public async Task SeedUsersAsync()
        {
            if (!_userRepo.FindAllUsers().Any())
            {
                // do something here
                byte[] passwordHash, passwordSalt;
                string password = "P@ssw0rd";
                IList<string> providers = new List<string>();

                PasswordService.CreatePasswordHash(password, out passwordHash, out passwordSalt);

                var adminUsers = new List<User>{
                    new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProviderId = string.Empty,
                        Name = "don",
                        FirstName = "don",
                        LastName = "uma",
                        Username = "don",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                    },
                    new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProviderId = string.Empty,
                        FirstName = "naya",
                        Name = "naya",
                        LastName = "cn",
                        Username = "naya",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                    }
                };

                await _userRepo.InsertManyAsync(adminUsers);
                
                foreach (var user in adminUsers)
                {
                    var providerRequest = new CreateProviderRequest
                    {
                        Name = $"{user.FirstName} provider",
                        OwnerId = user.Id
                    };

                    var provider = await _providerService.CreateProviderAsync(providerRequest);
                    providers.Add(provider.Id);
                }

                foreach (string providerId in providers)
                {
                    var users = GenerateUserList(passwordHash, passwordSalt, providerId);
                    await _userRepo.InsertManyAsync(users);
                }
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
                    Name = randomName,
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