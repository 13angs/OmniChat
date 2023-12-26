using System.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OmniChat.Configurations;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Repositories
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly IMongoCollection<Provider> _providersCollection;
        public ProviderRepository(IOptions<MongoConfig> mongoConfig, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(mongoConfig.Value.DbName);
            _providersCollection = database.GetCollection<Provider>(mongoConfig.Value.Collections!.ProviderCols);
        }
        public async Task InsertOneAsync(Provider provider)
        {
            await _providersCollection.InsertOneAsync(provider);
        }

        public async Task<Provider> FindByNameAsync(string name)
        {
            Provider provider = await _providersCollection
                .Find(p => p.Name.ToLower() == name)
                .FirstOrDefaultAsync();

            if (provider == null)
            {
                throw new DataException($"Provider with name {name} does not exist");
            }
            return provider;
        }

        public async Task<Provider> FindByOwnerIdAsync(string ownerId)
        {
            Provider provider = await _providersCollection
                .Find(p => p.OwnerId == ownerId)
                .FirstOrDefaultAsync();

            if (provider == null)
            {
                throw new DataException($"Provider with owner_id {ownerId} does not exist");
            }
            return provider;
        }
    }
}