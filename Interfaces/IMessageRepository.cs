using OmniChat.Models;

namespace OmniChat.Interfaces
{
    public interface IMessageRepository
    {
        public Task InsertOneAsync(Message message);
    }
}