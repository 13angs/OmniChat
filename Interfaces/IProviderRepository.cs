using OmniChat.Models;

namespace OmniChat.Interfaces
{
    public interface IProviderRepository
    {
        public Task InsertOneAsync(Provider provider);
        public Task<Provider> FindByNameAsync(string name);
        public Task<Provider> FindByOwnerIdAsync(string ownerId);
    }
}