//using Evolution.MongoDb.GenericRepository.Interfaces;
//using MongoDB.Bson.Serialization.Attributes;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Evolution.MongoDb.GenericRepository.Models
//{
//    /// <summary>
//    /// This class represents a basic document that can be stored in MongoDb.
//    /// Your document must implement this class in order for the MongoDbRepository to handle them.
//    /// </summary>
//    public class Document : IDocument
//    {
//        public Document()
//        {
//            Id = Guid.NewGuid();
//            AddedAtUtc = DateTime.UtcNow;
//        }
             
//        [BsonId]
//        public Guid Id { get; set; }
        
//        public DateTime AddedAtUtc { get; set; }
        
//        public int Version { get; set; }
//    }
//}