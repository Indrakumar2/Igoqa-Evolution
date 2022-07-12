using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Google.Model.Interfaces;
using Evolution.Google.Model.Models;
using Evolution.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GoogleModel = Evolution.Google.Model;
using MongoDB.Driver;

namespace Evolution.Google.Core.Services
{
    public class MongoGeoCoordinateService : IMongoGeoCoordinateService
    {
        private readonly IMongoGeoCoordinateRepository _mongoGeoCoordinateRepository = null;
        private readonly IAppLogger<MongoGeoCoordinateService> _logger = null;
        private readonly GoogleModel.Interfaces.IGeoCoordinateService _googleCoordinateService = null;

        public MongoGeoCoordinateService(IAppLogger<MongoGeoCoordinateService> logger,
                                         IMongoGeoCoordinateRepository mongoGeoCoordinateRepository,
                                         GoogleModel.Interfaces.IGeoCoordinateService googleCoordinateService)
        {
            _logger = logger;
            _mongoGeoCoordinateRepository = mongoGeoCoordinateRepository;
            _googleCoordinateService = googleCoordinateService;
        }

        public async Task<MongoGeoCoordinateInfo> AddAsync(MongoGeoCoordinateInfo model)
        {
            MongoGeoCoordinateInfo result = null;
            try
            {
                await _mongoGeoCoordinateRepository.AddOneAsync<MongoGeoCoordinateInfo>(model);
                //var addedDocument = await _mongoGeoCoordinateRepository.GetOneAsync<MongoGeoCoordinateInfo>(f => f.Epin == model.Epin);
                result = model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return result;
        }

        public MongoGeoCoordinateInfo Add(MongoGeoCoordinateInfo model)
        {
            MongoGeoCoordinateInfo result = null;
            try
            {
                _mongoGeoCoordinateRepository.AddOne<MongoGeoCoordinateInfo>(model);
                result = model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return result;
        }

        public async Task<MongoGeoCoordinateInfo> ModifyAsync(MongoGeoCoordinateInfo model)
        {
            MongoGeoCoordinateInfo result = null;
            try
            {
                await _mongoGeoCoordinateRepository.UpdateOneAsync<MongoGeoCoordinateInfo>(model);
                var updatedDocument = await _mongoGeoCoordinateRepository.GetOneAsync<MongoGeoCoordinateInfo>(f => f.Id == model.Id);
                result = updatedDocument;
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return result;
        }

        public MongoGeoCoordinateInfo Modify(MongoGeoCoordinateInfo model)
        {
            MongoGeoCoordinateInfo result = null;
            try
            {
                _mongoGeoCoordinateRepository.UpdateOne<MongoGeoCoordinateInfo>(model);
                var updatedDocument = _mongoGeoCoordinateRepository.GetOne<MongoGeoCoordinateInfo>(f => f.Id == model.Id);
                result = updatedDocument;
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return result;
        }

        public async Task<bool> RemoveAsync(MongoGeoCoordinateInfo model)
        {
            bool result = false;
            try
            {
                await _mongoGeoCoordinateRepository.DeleteOneAsync<MongoGeoCoordinateInfo>(model);
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return result;
        }

        public bool Remove(MongoGeoCoordinateInfo model)
        {
            bool result = false;
            try
            {
                _mongoGeoCoordinateRepository.DeleteOne<MongoGeoCoordinateInfo>(model);
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return result;
        }

        public async Task<IList<MongoGeoCoordinateInfo>> SearchAsync(MongoGeoCoordinateInfo search)
        {
            IList<MongoGeoCoordinateInfo> result = null;
            try
            {
                if (search == null)
                    search = new MongoGeoCoordinateInfo();
                var searchResponse = await _mongoGeoCoordinateRepository.GetAllAsync<MongoGeoCoordinateInfo>(search.ToExpression<MongoGeoCoordinateInfo>());

                result = searchResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);
            }

            return result;
        }

        public IList<MongoGeoCoordinateInfo> Search(MongoGeoCoordinateInfo search)
        {
            IList<MongoGeoCoordinateInfo> result = null;
            try
            {
                if (search == null)
                    search = new MongoGeoCoordinateInfo();

                var searchResponse = _mongoGeoCoordinateRepository.GetAll<MongoGeoCoordinateInfo>(search.ToExpression<MongoGeoCoordinateInfo>());
                result = searchResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);
            }
            return result;
        }

        //public IList<MongoGeoCoordinateInfo> Search(IList<MongoGeoCoordinateInfo> search)
        //{
        //    IList<MongoGeoCoordinateInfo> result = null;
        //    try
        //    {
        //        Expression<Func<MongoGeoCoordinateInfo, bool>> expressions = null;
        //        if (search?.Count > 0)
        //        {
        //            search.ToList()?.ForEach(x =>
        //            {
        //                if (x != null)
        //                {
        //                    var expression = x.ToExpression<MongoGeoCoordinateInfo>(new List<string>()
        //                                                                            {
        //                                                                                nameof(x.Id),
        //                                                                                nameof(x.IsPartiallyMatched),
        //                                                                                nameof(x.Version),
        //                                                                                nameof(x.AddedAtUtc),
        //                                                                                nameof(x.Coordinate)
        //                                                                            }, "expr");
        //                    if (expression != null)
        //                    {
        //                        if (expressions == null)
        //                            expressions = expression;
        //                        else
        //                            expressions = expressions.Or(expression, "expr");
        //                    }
        //                }
        //            });
        //        }

        //        var searchResponse = _mongoGeoCoordinateRepository.GetAll<MongoGeoCoordinateInfo>(expressions);
        //        result = searchResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);
        //    }
        //    return result;
        //}

        public IList<MongoGeoCoordinateInfo> Search(IList<MongoGeoCoordinateInfo> search)
        {
            IList<MongoGeoCoordinateInfo> result = new List<MongoGeoCoordinateInfo>();

            if (search?.Count > 0)
            {
                search.ToList()?.ForEach(x =>
                {
                    if (x != null)
                    {
                        try
                        {
                            var expression = x.ToExpression<MongoGeoCoordinateInfo>(new List<string>()
                                                                                    {
                                                                                        nameof(x.Id),
                                                                                        nameof(x.IsPartiallyMatched),
                                                                                        nameof(x.Version),
                                                                                        nameof(x.AddedAtUtc),
                                                                                        nameof(x.Location)
                                                                                    }, "expr");
                            if (expression != null)
                            {
                                var searchResponse = _mongoGeoCoordinateRepository.GetAll<MongoGeoCoordinateInfo>(expression);
                                if (searchResponse?.Count > 0)
                                    result.AddRange(searchResponse);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), search);
                        }
                    }
                });
            }
            return result;
        }

        public IList<MongoGeoCoordinateInfo> SearchAndSyncToMongo(IList<MongoGeoCoordinateInfo> searchModels)
        {
            IList<MongoGeoCoordinateInfo> searchCoordinate = null;

            if (searchModels?.Count > 0)
            {
                IList<MongoGeoCoordinateInfo> coordinateNotExists = null;
                searchCoordinate = this.GetCoordinateNotExists(searchModels, ref coordinateNotExists);

                IList<MongoGeoCoordinateInfo> googleCoordinates = null;
                if (coordinateNotExists?.Count > 0)
                    googleCoordinates = this.GetGoogleCoordinate(coordinateNotExists)?
                                            .Select(x => new MongoGeoCoordinateInfo()
                                            {
                                                City = x.City,
                                                Location = x.Location,
                                                Country = x.Country,
                                                State = x.State,
                                                Zip = x.Zip,
                                                // Address=x.Address,
                                                // GoogleAddress=x.GoogleAddress,
                                                IsPartiallyMatched = x.IsPartiallyMatched
                                            }).ToList();

                if (googleCoordinates?.Count > 0)
                {
                    searchCoordinate.AddRange(googleCoordinates);

                    googleCoordinates.ToList().ForEach(x =>
                    {
                        this.Add(x);
                    });
                }
            }
            else
                searchCoordinate = new List<MongoGeoCoordinateInfo>();

            return searchCoordinate;
        }

        private IList<LocationSearchResult> GetGoogleCoordinate(IList<MongoGeoCoordinateInfo> records)
        {
            var googleCoordinates = new List<LocationSearchResult>();

            records?.Where(x=>!string.IsNullOrEmpty(x.Country))?.Select(x => new LocationSearchInfo(x.Country, x.State, x.City, x.Zip))
                    .ToList()
                    .ForEach(x =>
                    {
                        var geoCoordnate = this._googleCoordinateService.GetLocationCoordinate(x);
                        if (geoCoordnate != null)
                            googleCoordinates.Add(geoCoordnate);
                    });

            return googleCoordinates;
        }

        private IList<MongoGeoCoordinateInfo> GetCoordinateNotExists(IList<MongoGeoCoordinateInfo> searchModels, ref IList<MongoGeoCoordinateInfo> coordinateNotExists)
        {
            IList<MongoGeoCoordinateInfo> result = null;

            result = this.Search(searchModels);
            coordinateNotExists = searchModels.Where(x => !result.Any(x1 => x1.Country?.Trim() == x.Country?.Trim() &&
                                                                            x1.State?.Trim() == x.State?.Trim() &&
                                                                            x1.City?.Trim() == x.City?.Trim() &&
                                                                            x1.Zip?.Trim() == x.Zip?.Trim()))
                                                                            .GroupBy(x => new { x.Country, x.State, x.City, x.Zip })
                                                                            .Select(x => new MongoGeoCoordinateInfo
                                                                            {
                                                                                Country = x.Key.Country,
                                                                                State = x.Key.State,
                                                                                City = x.Key.City,
                                                                                Zip=x.Key.Zip 
                                                                            }).ToList();
            return result;
        }

        public bool InsureIndex()
        {
            try
            {
                //Add these lines to application start code to create the index the first time an application is launched.
                var collection = _mongoGeoCoordinateRepository.DocumentCollection;
                if (collection != null)
                {
                    var builder = Builders<MongoGeoCoordinateInfo>.IndexKeys;
                    var keys = builder.Geo2DSphere(tag => tag.Location);                   
                   collection.Indexes.CreateOneAsync(keys);                    
                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
                throw ex;
            }
        }
    }
}