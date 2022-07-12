using Evolution.Google.Model.Enums;

namespace Evolution.Google.Model.Models
{
    public class DistanceSearchResult
    {
        public string Origin { get; set; }

        public string Destination { get; set; }

        public string GoogleOrigin { get; set; }

        public GeoCoordinateInfo GoogleOriginGeoInfo { get; set; }

        public string GoogleDestination { get; set; }

        public GeoCoordinateInfo GoogleDestinationGeoInfo { get; set; }

        public GoogleSearchStatus Status { get; set; }

        public DistanceInfo Distance { get; set; }

        public DurationInfo Duration { get; set; }
    }
}
