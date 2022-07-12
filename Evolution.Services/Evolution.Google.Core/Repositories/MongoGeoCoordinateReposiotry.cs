using Evolution.Document.Domain.Models;
using Evolution.Google.Model.Interfaces;
using Evolution.Google.Model.Models;
using Evolution.MongoDb.GenericRepository.DbContexts;
using Evolution.MongoDb.GenericRepository.Repositories;
using Evolution.MongoDb.Model.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Evolution.Google.Core.Repositories
{
    public class MongoGeoCoordinateReposiotry : BaseMongoRepository, IMongoGeoCoordinateRepository
    {
        #region Constructor

        public MongoGeoCoordinateReposiotry(IOptions<MongoSetting> mongoSetting) : base(mongoSetting.Value.ConnectionString, mongoSetting.Value.DatabaseName)
        {
            MongoSetting = mongoSetting.Value;
        }

        protected MongoGeoCoordinateReposiotry(string connectionString, string databaseName) : base(connectionString, databaseName)
        {
            MongoDbContext = new MongoDbContext(connectionString, databaseName);
        }

        protected MongoGeoCoordinateReposiotry(IMongoDbContext mongoDbContext) : base(mongoDbContext)
        {
            MongoDbContext = mongoDbContext;
        }

        protected MongoGeoCoordinateReposiotry(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
            MongoDbContext = new MongoDbContext(mongoDatabase);
            var v = GetCollection<MongoGeoCoordinateInfo>();
        }

        protected MongoSetting MongoSetting { get; }

        public IMongoCollection<MongoGeoCoordinateInfo> DocumentCollection
        {
            get
            {
                return GetCollection<MongoGeoCoordinateInfo>();
            }
        }

        #endregion
    }
}