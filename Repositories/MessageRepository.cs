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
        private readonly IMongoCollection<Message> _messagesCollection;
        public MessageRepository(IOptions<MongoConfig> mongoConfig, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(mongoConfig.Value.DbName);
            _messagesBsonCollection = database.GetCollection<BsonDocument>(mongoConfig.Value.Collections!.MessageCols);
            _messagesCollection = database.GetCollection<Message>(mongoConfig.Value.Collections!.MessageCols);
        }

        public IEnumerable<Message> FindMessagesByUserId(MessageRequest request)
        {
            var builder = Builders<Message>.Filter;
            var filter = builder.Or(
                builder.And(
                    builder.Eq(x => x.ProviderId, request.ProviderId),
                    builder.Eq(x => x.From.RefId, request.From.RefId),
                    builder.Eq(x => x.User.UserId, request.To.UserId)
                ),
                builder.And(
                    builder.Eq(x => x.ProviderId, request.ProviderId),
                    builder.Eq(x => x.From.RefId, request.To.UserId),
                    builder.Eq(x => x.User.UserId, request.From.RefId)
                )
            );

            return _messagesCollection
                .Find(filter)
                .ToEnumerable();
        }

        public async Task InsertOneAsync(Message message)
        {
            string strMessage = JsonConvert.SerializeObject(message);
            var document = BsonDocument.Parse(strMessage);
            await _messagesBsonCollection.InsertOneAsync(document);
        }
    }
}