using System;

namespace Evolution.MongoDb.Model.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionNameAttribute : Attribute
    {
        /// <summary>
        /// The name of the collection in which your documents are stored.
        /// </summary>
		public string Name { get; set; }

        public CollectionNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
