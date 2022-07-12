//using MongoDB.Driver;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Evolution.MongoDb.GenericRepository.Interfaces
//{
//    /// <summary>
//    /// This is the interface of the IMongoDbContext which is managed by the <see cref="BaseMongoRepository"/>.
//    /// </summary>
//    public interface IMongoDbContext
//    {      
//        IMongoClient Client { get; }
        
//        IMongoDatabase Database { get; }
        
//        IMongoCollection<TDocument> GetCollection<TDocument>(string partitionKey = null);
        
//        void DropCollection<TDocument>(string partitionKey = null);
        
//        void SetGuidRepresentation(MongoDB.Bson.GuidRepresentation guidRepresentation);
//    }
//}
