using Evolution.MongoDb.GenericRepository.Extensions;
using Evolution.MongoDb.Model.Attributes;
using Evolution.MongoDb.Model.Interfaces;
using MongoDB.Driver;
using System.Linq;
using System.Reflection;

namespace Evolution.MongoDb.GenericRepository.DbContexts
{
    public class MongoDbContext : IMongoDbContext
    {        
        public IMongoClient Client { get; }
        
        public IMongoDatabase Database { get; }

        /// <summary>
        /// Sets the Guid representation of the MongoDB Driver.
        /// </summary>
        /// <param name="guidRepresentation">The new value of the GuidRepresentation</param>
        public virtual void SetGuidRepresentation(MongoDB.Bson.GuidRepresentation guidRepresentation)
        {
            MongoDefaults.GuidRepresentation = guidRepresentation;
        }

        /// <summary>
        /// Initialize the Guid representation of the MongoDB Driver.
        /// Override this method to change the default GuidRepresentation.
        /// </summary>
        protected virtual void InitializeGuidRepresentation()
        {
            // by default, avoid legacy UUID representation: use Binary 0x04 subtype.
            MongoDefaults.GuidRepresentation = MongoDB.Bson.GuidRepresentation.Standard;
        }
                
        public MongoDbContext(IMongoDatabase mongoDatabase)
        {
            // Avoid legacy UUID representation: use Binary 0x04 subtype.
            InitializeGuidRepresentation();
            Database = mongoDatabase;
            Client = Database.Client;
        }
                
        public MongoDbContext(string connectionString, string databaseName)
        {
            InitializeGuidRepresentation();
            Client = new MongoClient(connectionString);
            Database = Client.GetDatabase(databaseName);
        }
                
        public MongoDbContext(MongoClient client, string databaseName)
        {
            InitializeGuidRepresentation();
            Client = client;
            Database = client.GetDatabase(databaseName);
        }

        /// <summary>
        /// Extracts the CollectionName attribute from the entity type, if any.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <returns>The name of the collection in which the TDocument is stored.</returns>
        private string GetAttributeCollectionName<TDocument>()
        {
            return (typeof(TDocument).GetTypeInfo()
                                     .GetCustomAttributes(typeof(CollectionNameAttribute))
                                     .FirstOrDefault() as CollectionNameAttribute)?.Name;
        }

        /// <summary>
        /// Returns a collection for a document type. Also handles document types with a partition key.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="partitionKey">The optional value of the partition key.</param>
        public IMongoCollection<TDocument> GetCollection<TDocument>(string partitionKey = null)
        {
            return Database.GetCollection<TDocument>(GetCollectionName<TDocument>(partitionKey));
        }

        /// <summary>
        /// Drops a collection, use very carefully.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        public void DropCollection<TDocument>(string partitionKey = null)
        {
            Database.DropCollection(GetCollectionName<TDocument>(partitionKey));
        }

        /// <summary>
        /// Given the document type and the partition key, returns the name of the collection it belongs to.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <param name="partitionKey">The value of the partition key.</param>
        /// <returns>The name of the collection.</returns>
        private string GetCollectionName<TDocument>(string partitionKey)
        {
            var collectionName = GetAttributeCollectionName<TDocument>() ?? Pluralize<TDocument>();
            if (string.IsNullOrEmpty(partitionKey))
            {
                return collectionName;
            }
            return $"{partitionKey}-{collectionName}";
        }

        /// <summary>
        /// Very naively pluralizes a TDocument type name.
        /// </summary>
        /// <typeparam name="TDocument">The type representing a Document.</typeparam>
        /// <returns>The pluralized document name.</returns>
        private string Pluralize<TDocument>()
        {
            return (typeof(TDocument).Name.Pluralize()).Camelize();
        }
    }
}
