using Evolution.MongoDb.Model.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Evolution.MongoDb.Model.Models
{
    public class Document : IDocument
    {
        public Document()
        {
            Id = Guid.NewGuid();
            AddedAtUtc = DateTime.UtcNow;
        }

        [BsonId]
        public Guid Id { get; set; }

        public DateTime AddedAtUtc { get; set; }

        public int Version { get; set; }
    }
}
