using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using OmniChat.Configurations;
using OmniChat.Interfaces;
using OmniChat.Models;

namespace OmniChat.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMongoCollection<BsonDocument> _messagesBsonCollection;
        public MessageRepository(IOptions<MongoConfig> mongoConfig, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(mongoConfig.Value.DbName);
            _messagesBsonCollection = database.GetCollection<BsonDocument>(mongoConfig.Value.Collections!.MessageCols);
        }
        public async Task InsertOneAsync(Message message)
        {
            string strMessage = JsonConvert.SerializeObject(message);
            var document = BsonDocument.Parse(strMessage);
            await _messagesBsonCollection.InsertOneAsync(document);
        }
    }
}