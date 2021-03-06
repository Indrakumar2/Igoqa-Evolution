//using Evolution.MongoDb.GenericRepository.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Evolution.MongoDb.GenericRepository.Models
//{
//    /// <summary>
//    /// This class represents a document that can be inserted in a collection that can be partitioned.
//    /// The partition key allows for the creation of different collections having the same document schema.
//    /// </summary>
//    public class PartitionedDocument : Document, IPartitionedDocument
//    {
//        public PartitionedDocument(string partitionKey)
//        {
//            PartitionKey = partitionKey;
//        }

//        /// <summary>
//        /// The name of the property used for partitioning the collection
//        /// This will not be inserted into the collection.
//        /// This partition key will be prepended to the collection name to create a new collection.
//        /// </summary>
//        public string PartitionKey { get; set; }
//    }
//}
