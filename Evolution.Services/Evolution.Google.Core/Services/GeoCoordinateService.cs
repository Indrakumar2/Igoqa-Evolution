using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Google.Model.Interfaces;
using Evolution.Google.Model.Models;
using Evolution.Logging.Interfaces;
using Google.Maps;
using Google.Maps.Geocoding;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Evolution.Google.Core.Services
{
    public class GeoCoordinateService : IGeoCoordinateService
    {
         private readonly IMongoGeoCoordinateRepository _mongoGeoCoordinateRepository = null;
        private readonly IAppLogger<GeoCoordinateService> _logger = null;

        public GeoCoordinateService(IAppLogger<GeoCoordinateService> logger,
                                     IMongoGeoCoordinateRepository mongoGeoCoordinateRepository)
        {
            _logger = logger;
            _mongoGeoCoordinateRepository = mongoGeoCoordinateRepository;
        }
        public LocationSearchResult GetLocationCoordinate(LocationSearchInfo searchModel)
        {
            var request = new GeocodingRequest
            {
                Address = PopulateAddress(searchModel)
            };
            var coordinate = GetLocationCoordinate(request);
            if (coordinate != null)
            {
                coordinate.Country = searchModel.Country;
                coordinate.State = searchModel.State;
                coordinate.City = searchModel.City;
                coordinate.Zip = searchModel.Zip;
            }

            return coordinate;
        }

        public LocationSearchResult GetLocationCoordinate(string address)
        {
            var request = new GeocodingRequest
            {
                Address = address
            };

            var coordinate = GetLocationCoordinate(request);

            if (coordinate != null)
            {
                coordinate.Address = address;
            }
            return coordinate;
        }
 
        private LocationSearchResult GetLocationCoordinate(GeocodingRequest request)
        {
            var response = new GeocodingService().GetResponse(request);
            if (response.Status == ServiceResponseStatus.Ok && (response.Results != null && response.Results.Any()))
            {
                var result = response.Results.First();
                var coordinate = new LocationSearchResult
                {
                    GoogleAddress = result.FormattedAddress,
                    IsPartiallyMatched = result.PartialMatch,
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(
                        new GeoJson2DGeographicCoordinates(result.Geometry.Location.Longitude,
                            result.Geometry.Location.Latitude))
                };
                return coordinate;
            }
            return null;
        }

        private string PopulateAddress(LocationSearchInfo searchInfo)
        {
            if (!string.IsNullOrEmpty(searchInfo.Country))
            {
                var requestQuery = searchInfo.Country;
                if (!string.IsNullOrEmpty(searchInfo.Zip))
                    requestQuery = searchInfo.Zip + ", " + requestQuery;

                if (!string.IsNullOrEmpty(searchInfo.State))
                    requestQuery = searchInfo.State + ", " + requestQuery;

                if (!string.IsNullOrEmpty(searchInfo.City))
                    requestQuery = searchInfo.City + ", " + requestQuery;

                return requestQuery;
            }

            throw new InvalidOperationException($"Invalid Search Request ({searchInfo.ToText()})");
        }
 

    }
}
