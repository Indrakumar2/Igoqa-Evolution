using Evolution.Google.Model.Models;
using System.Collections.Generic;

namespace Evolution.Google.Model.Interfaces
{
    public interface IDistanceService
    {
        IList<DistanceSearchResult> DrivingDistance(GeoCoordinateInfo from, GeoCoordinateInfo to);

        IList<DistanceSearchResult> DrivingDistance(GeoCoordinateInfo from,IList<GeoCoordinateInfo> to);

        IList<DistanceSearchResult> DrivingDistance(IList<GeoCoordinateInfo> from, IList<GeoCoordinateInfo> to);

        IList<DistanceSearchResult> DrivingDistance(string fromLocation, GeoCoordinateInfo toPoint);

        IList<DistanceSearchResult> DrivingDistance(string fromLocation, string toLocation);

        IList<DistanceSearchResult> DrivingDistance(IList<string> fromLocation, IList<string> toLocation);
    }
}
