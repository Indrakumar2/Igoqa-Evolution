using Evolution.Google.Model.Interfaces;
using Evolution.Google.Model.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Evolution.Common.Enums;
using Evolution.Google.Model.Enums;
using Google.Maps;
using Google.Maps.DistanceMatrix;

namespace Evolution.Google.Core.Services
{
    public class DistanceService : IDistanceService
    {
        public IList<DistanceSearchResult> DrivingDistance(string fromLocation, string toLocation)
        {
            if (string.IsNullOrEmpty(fromLocation) || string.IsNullOrEmpty((toLocation)))
                return null;

            var request = new DistanceMatrixService()
                            .GetResponse(PrepareDistanceMatrixRequest(fromLocations: new List<string>() { fromLocation },
                                                                      toLocations: new List<string>() { toLocation }));
            return PrepareSearchResult(request);
        }

        public IList<DistanceSearchResult> DrivingDistance(string fromLocation, GeoCoordinateInfo toPoint)
        {
            if (string.IsNullOrEmpty(fromLocation) || toPoint == null)
                return null;

            var request = new DistanceMatrixService()
                .GetResponse(PrepareDistanceMatrixRequest(fromLocations: new List<string>() { fromLocation },
                                                            toGeos: new List<GeoCoordinateInfo>() { toPoint }));
            return PrepareSearchResult(request);
        }

        public IList<DistanceSearchResult> DrivingDistance(GeoCoordinateInfo fromPoint, GeoCoordinateInfo toPoint)
        {
            if (fromPoint == null || toPoint == null)
                return null;

            var request = new DistanceMatrixService()
                                .GetResponse(PrepareDistanceMatrixRequest(fromGeos: new List<GeoCoordinateInfo>() { fromPoint }, toGeos: new List<GeoCoordinateInfo>() { toPoint }));
            return PrepareSearchResult(request);
        }

        public IList<DistanceSearchResult> DrivingDistance(GeoCoordinateInfo from, IList<GeoCoordinateInfo> to)
        {
            if (from==null || to?.Count == 0)
                return null;

            var request = new DistanceMatrixService()
                                .GetResponse(PrepareDistanceMatrixRequest(fromGeos: new List<GeoCoordinateInfo>() { from }, toGeos: to));
            return PrepareSearchResult(request);
        }

        public IList<DistanceSearchResult> DrivingDistance(IList<GeoCoordinateInfo> from, IList<GeoCoordinateInfo> to)
        {
            if (from?.Count == 0 || to?.Count == 0)
                return null;

            var request = new DistanceMatrixService().GetResponse(PrepareDistanceMatrixRequest(fromGeos: from, toGeos: to));
            return PrepareSearchResult(request);
        }

        public IList<DistanceSearchResult> DrivingDistance(IList<string> fromLocation, IList<string> toLocation)
        {
            if (fromLocation?.Count == 0 || toLocation?.Count == 0)
                return null;

            var request = new DistanceMatrixService().GetResponse(PrepareDistanceMatrixRequest(fromLocations: fromLocation, toLocations: toLocation));
            return PrepareSearchResult(request,fromLocation,toLocation);
        }

        private static IList<DistanceSearchResult> PrepareSearchResult(DistanceMatrixResponse distanceMatrixResponse,
                                                                        IList<string> fromLoc = null,
                                                                        IList<string> toLoc = null)
        {
            if (distanceMatrixResponse?.OriginAddresses == null || distanceMatrixResponse.DestinationAddresses == null)
                return null;

            var result = new List<DistanceSearchResult>();
            int originIndex = 0;
            fromLoc = fromLoc ?? distanceMatrixResponse.OriginAddresses;
            toLoc = toLoc ?? distanceMatrixResponse.DestinationAddresses;
            foreach (var origin in fromLoc)
            {
                if (originIndex < distanceMatrixResponse.Rows?.Length)
                {
                    int destIndex = 0;
                    foreach (var distance in distanceMatrixResponse.Rows[originIndex].Elements)
                    {
                        if (distance.Status == ServiceResponseStatus.Ok)
                        {
                            var destination = toLoc[destIndex];
                            var response = new DistanceSearchResult
                            {
                                Origin = origin,
                                GoogleOrigin= distanceMatrixResponse.OriginAddresses[originIndex],                                
                                Status = Enum.Parse<GoogleSearchStatus>(distance.Status.ToString()),
                                Destination = destination,
                                GoogleDestination= distanceMatrixResponse.DestinationAddresses[destIndex],
                                Distance = new DistanceInfo
                                {
                                    Kilometer = distance.distance.Text,
                                    Meter = distance.distance.Value
                                },
                                Duration = new DurationInfo
                                {
                                    Minute = distance.duration.Text,
                                    Second = distance.duration.Value
                                }
                            };
                            result.Add(response);
                        }
                        destIndex = destIndex + 1;
                    }
                }
                originIndex = originIndex + 1;
            }
            return result;
        }

        private static DistanceMatrixRequest PrepareDistanceMatrixRequest(IList<string> fromLocations = null,
                                                                            IList<string> toLocations = null,
                                                                            IList<GeoCoordinateInfo> fromGeos = null,
                                                                            IList<GeoCoordinateInfo> toGeos = null)
        {
#pragma warning disable 618
            var request = new DistanceMatrixRequest { Sensor = true };
#pragma warning restore 618
            if (fromLocations?.Count() > 0)
                fromLocations.ToList().ForEach(x => request.AddOrigin(new Location(x)));

            if (fromGeos?.Count > 0 && fromGeos.Any(x => x.Latitude >= 0 && x.Longitude >= 0))
                fromGeos.ToList().ForEach(x => request.AddOrigin(new LatLng(x.Latitude, x.Longitude)));

            if (toLocations?.Count() > 0)
                toLocations.ToList().ForEach(x => request.AddDestination(new Location(x)));

            if (toGeos?.Count > 0 && toGeos.Any(x => x.Latitude >= 0 && x.Longitude >= 0))
                toGeos.ToList().ForEach(x => request.AddDestination(new LatLng(x.Latitude, x.Longitude)));

            request.Mode = TravelMode.driving;
            return request;
        }
    }
}