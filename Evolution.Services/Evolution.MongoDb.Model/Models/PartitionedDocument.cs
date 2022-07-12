using Evolution.MongoDb.Model.Interfaces;

namespace Evolution.MongoDb.Model.Models
{
    public class PartitionedDocument : Document, IPartitionedDocument
    {
        public PartitionedDocument(string partitionKey)
        {
            PartitionKey = partitionKey;
        }

        /// <summary>
        /// The name of the property used for partitioning the collection
        /// This will not be inserted into the collection.
        /// This partition key will be prepended to the collection name to create a new collection.
        /// </summary>
        public string PartitionKey { get; set; }
    }
}
