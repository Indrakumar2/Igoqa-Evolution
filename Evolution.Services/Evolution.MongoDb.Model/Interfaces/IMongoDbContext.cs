using MongoDB.Driver;

namespace Evolution.MongoDb.Model.Interfaces
{
    public interface IMongoDbContext
    {
        IMongoClient Client { get; }

        IMongoDatabase Database { get; }

        IMongoCollection<TDocument> GetCollection<TDocument>(string partitionKey = null);

        void DropCollection<TDocument>(string partitionKey = null);

        void SetGuidRepresentation(MongoDB.Bson.GuidRepresentation guidRepresentation);
    }
}
