using System.Data;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _providerRepo;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<ProviderService> _logger;
        public ProviderService(IProviderRepository providerRepo, IUserRepository userRepo, ILogger<ProviderService> logger)
        {
            _providerRepo = providerRepo;
            _userRepo = userRepo;
            _logger = logger;
        }
        public async Task CreateProviderAsync(CreateProviderRequest request)
        {
            try
            {
                Provider existingProvider = await _providerRepo.FindByNameAsync(request.Name);
                if (existingProvider != null)
                {
                    throw new DataException($"Provider with name {request.Name} exists!");
                }
            }
            catch
            {
                _logger.LogInformation("Checking existing user...");
            }

            User existingUser = await _userRepo.FindByIdAsync(request.OwnerId);

            try
            {
                Provider providerOwner = await _providerRepo.FindByOwnerIdAsync(existingUser.Id);
                if (providerOwner != null)
                {
                    throw new DataException($"Owner with with name {existingUser.FirstName}");
                }
            }
            catch
            {
                _logger.LogInformation("Adding new provider...");
            }

            Provider newProvider = new Provider
            {
                Name = request.Name,
                Description = request.Description,
                OwnerId = request.OwnerId
            };

            await _providerRepo.InsertOneAsync(newProvider);

            await _userRepo.UpdateProviderAsync(request.OwnerId, newProvider.Id);
        }
    }
}