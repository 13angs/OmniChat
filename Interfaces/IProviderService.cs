using OmniChat.Models;

namespace OmniChat.Interfaces
{
    public interface IProviderService
    {
        public Task<Provider> CreateProviderAsync(CreateProviderRequest request);
    }
}