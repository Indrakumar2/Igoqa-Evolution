using Evolution.Document.Domain.Interfaces.Data;
using Evolution.Document.Domain.Models;
using Evolution.MongoDb.GenericRepository.DbContexts;
using Evolution.MongoDb.GenericRepository.Repositories;
using Evolution.MongoDb.Model.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Evolution.Document.Infrastructure.Data
{
    public class MongoDocumentRepository : BaseMongoRepository, IMongoDocumentRepository
    {
        #region Constructor

        public MongoDocumentRepository(IOptions<MongoSetting> mongoSetting)
            : base(mongoSetting.Value.ConnectionString, mongoSetting.Value.DatabaseName)
        {
            MongoSetting = mongoSetting.Value;
        }

        protected MongoDocumentRepository(string connectionString, string databaseName) : base(connectionString, databaseName)
        {
            MongoDbContext = new MongoDbContext(connectionString, databaseName);
        }

        protected MongoDocumentRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {
            MongoDbContext = mongoDbContext;
        }

        protected MongoDocumentRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
            MongoDbContext = new MongoDbContext(mongoDatabase);
        }

        protected MongoSetting MongoSetting { get; }

        #endregion
    }
}
