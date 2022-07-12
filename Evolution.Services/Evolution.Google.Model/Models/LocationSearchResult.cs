using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Google.Model.Models
{
    public class LocationSearchResult : LocationSearchInfo
    {
        //public GeoCoordinateInfo Coordinate { get; set; }

        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Location { get; set; }
    }
}
