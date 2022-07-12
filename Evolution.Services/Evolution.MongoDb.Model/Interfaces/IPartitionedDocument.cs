namespace Evolution.MongoDb.Model.Interfaces
{
    public interface IPartitionedDocument
    {
        /// <summary>
        /// The partition key used to partition your collection.
        /// </summary>
        string PartitionKey { get; set; }
    }
}
