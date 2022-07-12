//using Evolution.Common.Models.Mongo;
//using Evolution.MongoDb.GenericRepository.DbContexts;
//using Evolution.MongoDb.GenericRepository.Repositories;
//using Evolution.MongoDb.Model.Interfaces;
//using Evolution.Project.Domain.Interfaces.Data;
//using Microsoft.Extensions.Options;
//using MongoDB.Driver;

//namespace Evolution.Project.Infrastructure.Data
//{
//    public class ProjectMongoDocumentRepository : BaseMongoRepository, IProjectMongoDocumentRepository
//    {

//        #region Constructor

//        public ProjectMongoDocumentRepository(IOptions<MongoSetting> mongoSetting)
//            : base(mongoSetting.Value.ConnectionString, mongoSetting.Value.DatabaseName)
//        {
//            MongoSetting = mongoSetting.Value;
//        }

//        protected ProjectMongoDocumentRepository(string connectionString, string databaseName) : base(connectionString, databaseName)
//        {
//            MongoDbContext = new MongoDbContext(connectionString, databaseName);
//        }

//        protected ProjectMongoDocumentRepository(IMongoDbContext mongoDbContext) : base(mongoDbContext)
//        {
//            MongoDbContext = mongoDbContext;
//        }

//        protected ProjectMongoDocumentRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
//        {
//            MongoDbContext = new MongoDbContext(mongoDatabase);
//        }

//        protected MongoSetting MongoSetting { get; }

//        #endregion
//    }
//}
