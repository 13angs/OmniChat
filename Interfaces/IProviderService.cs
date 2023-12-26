using OmniChat.Models;

namespace OmniChat.Interfaces
{
    public interface IProviderService
    {
        public Task CreateProviderAsync(CreateProviderRequest request);
    }
}