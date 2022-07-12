using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Interfaces.Validations;
using Evolution.Master.Domain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;


namespace Evolution.Master.Core
{
    public class StateService : IStateService
    {
        private readonly IAppLogger<StateService> _logger = null;
        private readonly ICountyRepository _repository = null;
        private readonly ICountryService _countryService = null;
        private readonly ICountyValidationService _countyValidationService = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        public StateService(IMapper mapper, 
                            IAppLogger<StateService> logger, 
                            ICountyRepository repository,
                            ICountryService countryService,
                            ICountyValidationService countyValidationService,
                            JObject messages)
        {
            this._logger = logger;
            this._repository = repository;
            this._countryService = countryService;
            this._countyValidationService = countyValidationService;
            this._mapper = mapper;
            this._messages = messages;
        }

        public Response Modify(IList<State> counties)
        {
            Exception exception = null;
            List<ValidationMessage> validationMessages = null;
            validationMessages = new List<ValidationMessage>();
            try
            {
                if (counties != null)
                {
                    IList<DbModel.Country> dbCountries = null;
                    IList<DbModel.County> dbCounties = null;

                    var recordToBeUpdate = FilterRecord(counties, ValidationType.Update);
                    var validResponse = IsRecordValidForProcess(recordToBeUpdate, ValidationType.Update, recordToBeUpdate, ref dbCountries, ref dbCounties);

                    if (Convert.ToBoolean(validResponse.Result) && dbCountries != null)
                    {
                        dbCounties.ToList().ForEach(countryData =>
                        {
                            _repository.AutoSave = false;
                            var masterDataToBeModify = recordToBeUpdate.FirstOrDefault(x => x.Id == countryData.Id);
                            _mapper.Map(masterDataToBeModify, countryData, opt =>
                            {
                                opt.Items["isCountyId"] = true;
                                opt.Items["isCountry"] = dbCountries;
                            });

                            countryData.UpdateCount = masterDataToBeModify.UpdateCount.CalculateUpdateCount();
                            countryData.ModifiedBy = masterDataToBeModify.ModifiedBy;
                        });
                        _repository.Update(dbCounties);
                        _repository.ForceSave();
                    }
                    else
                        return validResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), counties);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        public Response Save(IList<State> counties)
        {
            Exception exception = null;
            List<ValidationMessage> validationMessages = null;
            validationMessages = new List<ValidationMessage>();
            try
            {
                if (counties != null)
                {
                    IList<DbModel.Country> dbCountries = null;
                    IList<DbModel.County> dbCounties = null;
                    var recordToBeAdd = FilterRecord(counties, ValidationType.Add);
                    var validResponse = IsRecordValidForProcess(counties, ValidationType.Add, recordToBeAdd,ref dbCountries, ref dbCounties);
                    if (Convert.ToBoolean(validResponse.Result) && dbCountries != null && dbCounties != null)
                    {
                        _repository.AutoSave = false;
                        dbCounties = _mapper.Map<IList<DbModel.County>>(recordToBeAdd, opt =>
                        {
                            opt.Items["isCountyId"] = false;
                            opt.Items["isCountry"] = dbCountries;
                        });
                        _repository.Add(dbCounties);
                        _repository.ForceSave();
                    }
                    else
                        return validResponse;
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), counties);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        public Response Delete(IList<State> counties)
        {
            Exception exception = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.Country> dbCountries = null;
            IList<DbModel.County> dbCounties = null;
            try
            {
                validationMessages = new List<ValidationMessage>();
                Response response = null;
                var recordToBeDelete = FilterRecord(counties, ValidationType.Delete);
                response = IsRecordValidForProcess(counties,
                                                   ValidationType.Delete,
                                                   recordToBeDelete,
                                                   ref dbCountries,
                                                   ref dbCounties);

                if (recordToBeDelete?.Count > 0)
                {
                    if ((Convert.ToBoolean(response.Result)) && dbCountries?.Count > 0)
                    {
                        _repository.AutoSave = false;
                        _repository.Delete(dbCounties);
                        _repository.ForceSave();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), counties);
            }


            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages, null, exception);
        }

        public Response Search(State search)
        {
            IList<State> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (search == null)
                    search = new State();

                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))                
                {
                    result = _repository.FindBy(null)
                                            .Where(x => (string.IsNullOrEmpty(search.Name) || x.Name == search.Name)
                                                    && (string.IsNullOrEmpty(search.Country) || x.Country.Name == search.Country)
                                                    && (search.CountryId ==null || x.Country.Id == search.CountryId)) //Added for D-1076
                                            .Select(x => new State() { Name = x.Name, Country = x.Country.Name,Id=x.Id }).OrderBy(x=>x.Name).ToList();
                    tranScope.Complete();
                }
            }
           
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }

            return new Response().ToPopulate(responseType, result,result?.Count);
        }

        public bool IsValidCounty(IList<string> names,
                                    ref IList<DbModel.County> dbCounties,
                                    ref IList<ValidationMessage> valdMessages,
                                    params Expression<Func<DbModel.County, object>>[] includes)
        {
            var messages = new List<ValidationMessage>();
            if (names?.Count() > 0)
            {
                if (dbCounties == null && names.Count > 0)
                    dbCounties = _repository.FindBy(x => names.Contains(x.Name), includes).ToList();

                IList<DbModel.County> dbDatas = dbCounties;
                var countyNotExists = names.Where(x => !dbDatas.Any(x2 => x2.Name == x))?.ToList();
                countyNotExists?.ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.MasterInvalidCounty, x);
                });
                valdMessages = messages;
            }
            return valdMessages?.Count <= 0;// dbCounties != null ? dbCounties?.Count() == names?.Count : true;
        }

        private bool IsCountyExistInDb(IList<int> countyIds,
                                          IList<DbModel.County> dbCounties,
                                          ref IList<int> countiesNotExsists,
                                          ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbCounties == null)
                dbCounties = new List<DbModel.County>();

            var validMessages = validationMessages;

            if (countyIds?.Count > 0)
            {
                countiesNotExsists = countyIds.Where(x => !dbCounties.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                countiesNotExsists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.MasterInvalidCounty, x);
                });
            }
            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }


        private bool IsCountyNameDuplicate(IList<State> counties,
                                           IList<DbModel.Country> dbCountries,
                                           ref IList<DbModel.County> dbCounties,
                                           ValidationType validationType,
                                           ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (counties?.Count > 0)
            {
                var countries = counties?.Where(x => !string.IsNullOrEmpty(x.Country)).Select(x => x.Country.Trim()).ToList();
                if (dbCountries == null)
                    _countryService.IsValidCountryName(countries, ref dbCountries, ref validationMessages, new string[] { "County" });

                IList<State> countyExists = null;
                if (validationType == ValidationType.Add)
                    countyExists=counties?.Where(x => !string.IsNullOrEmpty(x.Name.Trim()))?.Where(x1 => dbCountries.SelectMany(x2=>x2.County)
                                                                                                       .Any(x3 => x3.Name.Trim() == x1.Name))?.ToList();

                if (validationType == ValidationType.Update)
                    countyExists= counties?.Where(x => !string.IsNullOrEmpty(x.Name.Trim())).Where(x1 => dbCountries.SelectMany(x2 => x2.County)
                                                                                                                     .Any(x3 => x3.Name.Trim() == x1.Name 
                                                                                                                                && x3.Id != x1.Id))?.ToList();

                countyExists.ToList().ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.CountyAlreadyExsists, x);
                });
            }
            if (messages.Count > 0)
                validationMessages = messages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidForUpdate(IList<State> counties,
                                            ref IList<DbModel.Country> dbCountries,
                                            ref IList<DbModel.County> dbCounties,
                                            ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            IList<string> countries = dbCountries.Select(x => x.Name.Trim()).ToList();

            if (_countryService.IsValidCountryName(countries, ref dbCountries, ref validationMessages, new string[] { "County" }))
                 IsCountyNameDuplicate(counties, dbCountries, ref dbCounties, ValidationType.Update, ref messages);


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsRecordValidForAdd(IList<State> counties,
                                         ref IList<DbModel.Country> dbCountries,
                                         ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            IList<DbModel.County> dbCounties = null;
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            List<string> countries = dbCountries.Select(x => x.Name.Trim()).ToList();

            if (_countryService.IsValidCountryName(countries, ref dbCountries, ref validationMessages, new string[] { "County" }))
                IsCountyNameDuplicate(counties, dbCountries, ref dbCounties, ValidationType.Add, ref messages);

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private IList<State> FilterRecord(IList<State> counties,
                                          ValidationType filterType)
        {
            IList<State> filteredCountyData = null;
            if (filterType == ValidationType.Add)
                filteredCountyData = counties?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredCountyData = counties?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredCountyData = counties?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredCountyData;
        }

        private bool IsValidPayload(IList<State> counties,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _countyValidationService.Validate(JsonConvert.SerializeObject(counties), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Master, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }


        public Response IsRecordValidForProcess(IList<State> counties,
                                                 ValidationType validationType,
                                                 IList<State> filteredData,
                                                 ref IList<DbModel.Country> dbCountries,
                                                 ref IList<DbModel.County> dbCounties)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (counties?.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredData == null || filteredData.Count <= 0)
                        filteredData = FilterRecord(counties, validationType);

                    if (filteredData != null && filteredData?.Count > 0)
                    {
                        result = IsValidPayload(counties, validationType, ref validationMessages);
                        if (filteredData?.Count > 0 && result)
                        {
                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                                if (validationType == ValidationType.Update && result)
                                    result = IsRecordValidForUpdate(counties, ref dbCountries,ref dbCounties, ref validationMessages);

                            else if (validationType == ValidationType.Add)
                                result = IsRecordValidForAdd(counties, ref dbCountries, ref validationMessages);
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), counties);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
    }
}
