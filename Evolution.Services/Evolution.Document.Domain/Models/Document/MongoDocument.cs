using Evolution.MongoDb.Model.Attributes;
using System.Collections.Generic;
using Mongo = Evolution.MongoDb.Model.Models;

namespace Evolution.Document.Domain.Models.Document
{
    public class BaseMongoDocument : Mongo.Document
    {
        public string ReferenceCode { get; set; }

        public string SubReferenceCode { get; set; }

        public string UniqueName { get; set; }

        public string Text { get; set; }

        public string ModuleCode { get; set; }
    }

    [CollectionNameAttribute("EvolutionDocuments")]
    public class EvolutionMongoDocument : BaseMongoDocument
    {
        public string DocumentType { get; set; }
    }

    public class EvolutionMongoDocumentSearch  :BaseMongoDocument
    {
        public IList<string> DocumentTypes { get; set; }
    }
}
