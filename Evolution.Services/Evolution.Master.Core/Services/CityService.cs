using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Core
{
    public class CityService : ICityService
    {
        private readonly IAppLogger<CityService> _logger = null;
        private readonly ICityRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;

        public CityService(IMapper mapper, IAppLogger<CityService> logger, ICityRepository repository,JObject messages)
        {
            this._logger = logger;
            this._repository = repository;
            this._mapper = mapper;
            this._messages = messages;
        }

        public Response Modify(IList<City> datas)
        {
            throw new NotImplementedException();
        }

        public Response Save(IList<City> datas)
        {
            throw new NotImplementedException();
        }

        public Response Search(City search)
        {
            IList<City> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new City();

                result = _repository.FindBy(null)
                                        .Where(x => (string.IsNullOrEmpty(search.Name) || x.Name == search.Name)
                                                && (string.IsNullOrEmpty(search.State) || x.County.Name == search.State)
                                                && (string.IsNullOrEmpty(search.Country) || x.County.Country.Name == search.Country)
                                                && (search.StateId == null || x.County.Id == search.StateId)) //Added for D-1076
                                        .Select(x => new City() { Name = x.Name, State = x.County.Name, Country = x.County.Country.Name, Id = x.Id }).OrderBy(x => x.Name).ToList();

            }
          
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }

            return new Response().ToPopulate(responseType, result,result?.Count);
        }

        public bool IsValidCity(IList<string> names,
                                    IList<DbModel.County> dbConties,
                                    ref IList<DbModel.City> dbCities,
                                    ref IList<ValidationMessage> valdMessages,
                                    params Expression<Func<DbModel.City, object>>[] includes)
        {
            var messages = new List<ValidationMessage>();
            if (names?.Count() > 0)
            {
                if (dbConties != null && names.Count > 0)
                {
                    var cities = dbConties?.ToList()?.SelectMany(x => x.City)?.ToList();
                    dbCities = cities?.Where(x => names.Contains(x.Name)).ToList();
                }
       
                else if (dbCities == null && names.Count > 0)
                    dbCities = _repository.FindBy(x => names.Contains(x.Name) , includes) .ToList();

                IList<DbModel.City> dbDatas = dbCities;
                var cityNotExists = names.Where(x => !dbDatas.Any(x2 => x2.Name == x))?.ToList();
                cityNotExists?.ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.MasterInvalidCity, x);
                });
                valdMessages = messages;
            }
            return valdMessages?.Count <= 0;// dbCities != null ? dbCities?.Count() == names?.Count : true;
        }
        private IList<City> FilterRecord(IList<City> datas,
                                          ValidationType filterType)
        {
            IList<City> filteredCityData = null;

            if (filterType == ValidationType.Add)
                filteredCityData = datas?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredCityData = datas?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredCityData = datas?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredCityData;
        }

     
    }
}
