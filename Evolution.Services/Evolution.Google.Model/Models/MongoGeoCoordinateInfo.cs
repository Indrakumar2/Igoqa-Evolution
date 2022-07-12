using Evolution.MongoDb.Model.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using Mongo = Evolution.MongoDb.Model.Models;

namespace Evolution.Google.Model.Models
{
    [CollectionNameAttribute("EvolutionCoordinates")]
    public class MongoGeoCoordinateInfo : Mongo.Document
    {
        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Zip { get; set; }

        public bool IsPartiallyMatched { get; set; }
 
        // public string GoogleAddress { get; set; }
        // public string Address { get; set; }
         
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Location { get; set; }
    }
}
