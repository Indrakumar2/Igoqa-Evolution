using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Interfaces.Validations;
using Evolution.Master.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Core
{
    public class CountryService : ICountryService
    {
        private readonly IAppLogger<CountryService> _logger = null;
        private readonly ICountryRepository _repository = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        private readonly ICountryValidationService _countryValidationService = null;
        private readonly IRegionService _regionService = null;
        private readonly IMemoryCache _memoryCache = null;
        private readonly AppEnvVariableBaseModel _environment = null;

        public CountryService(IMapper mapper,
                              IAppLogger<CountryService> logger,
                              ICountryRepository repository,
                              ICountryValidationService countryValidationService,
                              IRegionService regionService,
                              JObject messages,
                              IMemoryCache memoryCache,
                              IOptions<AppEnvVariableBaseModel> environment)
        {
            this._logger = logger;
            this._repository = repository;
            this._mapper = mapper;
            this._messages = messages;
            this._countryValidationService = countryValidationService;
            this._regionService = regionService;
            _memoryCache = memoryCache;
            _environment = environment.Value;
        }

        public Response GetCountryEUVatPrefix()
        {
            IList<string> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                result = _repository.GetAll().Where(e => !string.IsNullOrEmpty(e.Euvatprefix)).OrderBy(a => a.Euvatprefix).Select(x => x.Euvatprefix.Trim()).Distinct().ToList();
            }

            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response(responseType.ToId(), responseType.ToString(), result);
        }

        public Response Modify(IList<Country> datas)
        {
            Exception exception = null;
            List<ValidationMessage> validationMessages = null;
            validationMessages = new List<ValidationMessage>();
            IList<DbModel.Country> dbCountries = null;
            IList<DbModel.Data> dbRegions = null;
            try
            {
                if (datas != null)
                {
                    _repository.AutoSave = false;
                    var recordToBeUpdate = FilterRecord(datas, ValidationType.Update);

                    var validResponse = IsRecordValidForProcess(recordToBeUpdate, ValidationType.Update, recordToBeUpdate, ref dbCountries, ref dbRegions);

                    if (Convert.ToBoolean(validResponse.Result) && dbCountries != null)
                    {
                        dbCountries.ToList().ForEach(countryData =>
                        {
                            _repository.AutoSave = false;
                            var masterDataToBeModify = recordToBeUpdate.FirstOrDefault(x => x.Id == countryData.Id);
                            _mapper.Map(masterDataToBeModify, countryData, opt =>
                            {
                                opt.Items["isCountryId"] = true;
                                opt.Items["isRegionId"] = dbRegions;
                            });

                            if (masterDataToBeModify != null)
                            {
                                countryData.UpdateCount = masterDataToBeModify.UpdateCount.CalculateUpdateCount();
                                countryData.ModifiedBy = masterDataToBeModify.ModifiedBy;
                            }
                        });
                        _repository.Update(dbCountries);
                        _repository.ForceSave();
                    }
                    else
                        return validResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), datas);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        public Response Save(IList<Country> datas)
        {
            Exception exception = null;
            List<ValidationMessage> validationMessages = null;
            validationMessages = new List<ValidationMessage>();
            IList<DbModel.Data> dbRegions = null;
            try
            {
                if (datas != null)
                {

                    IList<DbModel.Country> dbCountries = null;
                    var recordToBeAdd = FilterRecord(datas, ValidationType.Add);
                    var validResponse = IsRecordValidForProcess(datas, ValidationType.Add, recordToBeAdd, ref dbCountries, ref dbRegions);



                    if (Convert.ToBoolean(validResponse.Result) && dbCountries != null && dbRegions != null)
                    {
                        _repository.AutoSave = false;
                        dbCountries = _mapper.Map<IList<DbModel.Country>>(recordToBeAdd, opt =>
                        {
                            opt.Items["isCountryId"] = false;
                            opt.Items["isRegionId"] = dbRegions;
                        });
                        _repository.Add(dbCountries);
                        _repository.ForceSave();
                    }
                    else
                        return validResponse;
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), datas);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);

        }

        public Response Delete(IList<Country> datas)
        {
            Exception exception = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.Country> dbCountries = null;
            IList<DbModel.Data> dbRegions = null;
            try
            {

                validationMessages = new List<ValidationMessage>();
                Response response = null;
                var recordToBeDelete = FilterRecord(datas, ValidationType.Delete);


                response = IsRecordValidForProcess(datas,
                                                   ValidationType.Delete,
                                                    recordToBeDelete,
                                                   ref dbCountries,
                                                   ref dbRegions);

                if (recordToBeDelete?.Count > 0)
                {
                    if ((Convert.ToBoolean(response.Result)) && dbCountries?.Count > 0)
                    {
                        _repository.AutoSave = false;
                        _repository.Delete(dbCountries);

                        _repository.ForceSave();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), datas);
            }


            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages, null, exception);
        }

        public Response Search(Country search)
        {
            IList<Country> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                var cacheKey = "Country";
                if (search == null)
                    search = new Country();

                if (search.IsFromRefresh)
                {
                    result = CountryList(search);
                    if (_memoryCache.TryGetValue(cacheKey, out result))
                    {
                        _memoryCache.Remove(cacheKey);
                        _memoryCache.Set(cacheKey, result, ObjectExtension.CacheExpiry(_environment.AbsoluteExpiration, _environment.SlidingExpiration));
                    }
                }
                else
                {
                    if (!_memoryCache.TryGetValue(cacheKey, out result))
                    {
                        result = CountryList(search);
                        _memoryCache.Set(cacheKey, result, ObjectExtension.CacheExpiry(_environment.AbsoluteExpiration, _environment.SlidingExpiration));
                    }
                }
            }

            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);
        }

        private IList<Country> CountryList(Country search)
        {
             return _repository.FindBy(null)
                                          .Where(x => (string.IsNullOrEmpty(search.Name) || x.Name == search.Name) && (string.IsNullOrEmpty(search.Code) || x.Code == search.Code)
                                          ).AsNoTracking()
                                          .Select(x => new Country() { Id = x.Id, Code = x.Code, Name = x.Name, EUVatPrefix = x.Euvatprefix }).OrderBy(a => a.Name).ToList();
        }

        public bool IsValidCountryName(IList<string> names,
                                        ref IList<DbModel.Country> dbCountries,
                                        ref IList<ValidationMessage> valdMessages,
                                        params Expression<Func<DbModel.Country, object>>[] includes)
        {
            var messages = new List<ValidationMessage>();
            if (names?.Count() > 0)
            {
                if (dbCountries == null && names.Count > 0)
                    dbCountries = _repository.FindBy(x => names.Contains(x.Name), includes).ToList();

                IList<DbModel.Country> dbDatas = dbCountries;
                var countryNotExists = names.Where(x => !dbDatas.Any(x2 => x2.Name == x))?.ToList();
                countryNotExists?.ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.MasterInvalidCountry, x);
                });
                valdMessages = messages;
            }
            return valdMessages.Count <= 0;// dbCountries != null ? dbCountries?.Count() == names?.Count: true;
        }

        public bool IsValidCountryName(IList<string> names,
                                        ref IList<DbModel.Country> dbCountries,
                                        ref IList<ValidationMessage> valdMessages,
                                        string[] includes)
        {
            var messages = new List<ValidationMessage>();
            if (names?.Count() > 0)
            {
                if (dbCountries == null && names.Count > 0)
                    dbCountries = _repository.FindBy(x => names.Contains(x.Name.Trim()), includes).ToList();

                IList<DbModel.Country> dbDatas = dbCountries;
                var countryNotExists = names.Where(x => !dbDatas.Any(x2 => x2.Name.Trim() == x))?.ToList();
                countryNotExists?.ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.MasterInvalidCountry, x);
                });
                valdMessages = messages;
            }
            return valdMessages.Count <= 0;// dbCountries != null ? dbCountries?.Count() == names?.Count: true;
        }

        private IList<Country> FilterRecord(IList<Country> datas,
                                             ValidationType filterType)
        {
            IList<Country> filteredCountryData = null;
            if (filterType == ValidationType.Add)
                filteredCountryData = datas?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredCountryData = datas?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredCountryData = datas?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredCountryData;
        }

        private bool IsCountryNameDuplicate(IList<Country> datas,
                                            ref IList<DbModel.Country> dbCountries,
                                            ValidationType validationType,
                                            ref IList<ValidationMessage> valdMessages)
        {
            var messages = new List<ValidationMessage>();
            if (datas?.Count > 0)
            {
                var CountryCode = datas?.Where(x => !string.IsNullOrEmpty(x.Code)).Select(x => x.Code).ToList();
                if (dbCountries == null)
                    dbCountries = _repository.FindBy(x => CountryCode.Contains(x.Code)).ToList();

                IList<DbModel.Country> dbCountry = dbCountries;
                IList<Country> CountryAlreadyExsists = null;
                if (validationType == ValidationType.Add)
                {
                    CountryAlreadyExsists = datas?.Where(x => !string.IsNullOrEmpty(x.Code)).Where(x1 => dbCountry.Any(x2 => x2.Code == x1.Code))?.ToList();
                }

                if (validationType == ValidationType.Update)
                {
                    CountryAlreadyExsists = datas?.Where(x => !string.IsNullOrEmpty(x.Name)).Where(x1 => dbCountry.Any(x2 => x2.Code == x1.Code && x2.Id != x1.Id))?.ToList();
                }
                CountryAlreadyExsists.ToList().ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.CountryAlreadyExsists, x);
                });
            }
            if (messages.Count > 0)
                valdMessages = messages;
            return valdMessages.Count <= 0;


        }

        public Response IsRecordValidForProcess(IList<Country> datas,
                                                 ValidationType validationType,
                                                 IList<Country> FilteredDatas,
                                                 ref IList<DbModel.Country> dbCountries,
                                                 ref IList<DbModel.Data> dbRegions)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;

            try
            {
                if (datas?.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (FilteredDatas == null || FilteredDatas.Count <= 0)
                        FilteredDatas = FilterRecord(datas, validationType);

                    if (FilteredDatas != null && FilteredDatas?.Count > 0)
                    {
                        result = IsValidPayload(datas, validationType, ref validationMessages);

                        if (FilteredDatas?.Count > 0 && result)
                        {
                            IList<int> moduleNotExists = null;
                            var countryId = FilteredDatas.Where(x => x.Id.HasValue).Select(x => x.Id.Value).Distinct().ToList();

                            if ((dbCountries == null || dbCountries.Count <= 0) && validationType != ValidationType.Add)
                                dbCountries = GetCountryData(FilteredDatas);


                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                            {
                                result = IsCountryIdExistInDb(countryId,
                                                            dbCountries,
                                                            ref moduleNotExists,
                                                            ref validationMessages);

                                if (validationType == ValidationType.Update && result)
                                {
                                    result = IsRecordValidForUpdate(datas, ref dbCountries, ref dbRegions, ref validationMessages);
                                }
                            }
                            else if (validationType == ValidationType.Add)
                            {
                                result = IsRecordValidForAdd(datas, ref dbCountries, ref dbRegions, ref validationMessages);
                            }
                            else
                                result = false;

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), datas);

            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsValidPayload(IList<Country> datas,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _countryValidationService.Validate(JsonConvert.SerializeObject(datas), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Master, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private IList<DbModel.Country> GetCountryData(IList<Country> datas)
        {
            IList<DbModel.Country> dbCountries = null;
            if (datas?.Count > 0)
            {
                var countryId = datas.Select(x => x.Id).Distinct().ToList();
                dbCountries = _repository.FindBy(x => countryId.Contains(x.Id)).ToList();
            }

            return dbCountries;
        }

        private bool IsCountryIdExistInDb(IList<int> countryIds,
                                        IList<DbModel.Country> dbCountries,
                                        ref IList<int> CountriesNotExsists,
                                        ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbCountries == null)
                dbCountries = new List<DbModel.Country>();

            var validMessages = validationMessages;

            if (countryIds?.Count > 0)
            {
                CountriesNotExsists = countryIds.Where(x => !dbCountries.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                CountriesNotExsists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.MasterInvalidCountry, x);
                });
            }



            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidForUpdate(IList<Country> datas,
                                             ref IList<DbModel.Country> dbCountries,
                                             ref IList<DbModel.Data> dbRegions,
                                             ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            IList<string> regionNames = datas.Select(x => x.Region).ToList();

            if (IsCountryNameDuplicate(datas, ref dbCountries, ValidationType.Update, ref messages))
                _regionService.IsValidRegion(regionNames, ref dbRegions, ref messages);


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsRecordValidForAdd(IList<Country> datas,
                                           ref IList<DbModel.Country> dbCountries,
                                           ref IList<DbModel.Data> dbRegions,
                                           ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            IList<string> regionNames = datas.Select(x => x.Region).ToList();

            if (IsCountryNameDuplicate(datas, ref dbCountries, ValidationType.Add, ref messages))
                _regionService.IsValidRegion(regionNames, ref dbRegions, ref messages);


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }




    }





}
