using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Validations;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Core.Services
{
    public class TechnicalSpecialistContactInfoService : ITechnicalSpecialistContactService
    {
        private readonly IAppLogger<TechnicalSpecialistContactInfoService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly ITechnicalSpecialistContactRepository _repository = null;
        private readonly JObject _messages = null;
        private readonly ITechnicalSpecialistContactValidationService _validationService = null;
        //private readonly ITechnicalSpecialistService _technSpecServices = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly ICountryService _countryService = null;
        private readonly IStateService _countyService = null;

        private readonly IAuditSearchService _auditSearchService = null;
        private readonly ICityService _cityService = null;

        #region Constructor

        public TechnicalSpecialistContactInfoService(ITechnicalSpecialistContactRepository repository, 
                                                     IAppLogger<TechnicalSpecialistContactInfoService> logger,
                                                     IMapper mapper, JObject messages, 
                                                     ITechnicalSpecialistContactValidationService validationService,
                                                     //ITechnicalSpecialistService technSpecServices,
                                                     ITechnicalSpecialistRepository technicalSpecialistRepository,
                                                     ICountryService countryService, 
                                                     IStateService countyService, 
                                                     ICityService cityService, 
                                                     IAuditSearchService auditSearchService)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _validationService = validationService;
            _messages = messages;
            //_technSpecServices = technSpecServices;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _countryService = countryService;
            _countyService = countyService;
            _cityService = cityService;
            _auditSearchService = auditSearchService; 
        }
        #endregion

        #region Public Methods

        #region Get
        public Response Get(TechnicalSpecialistContactInfo searchModel, int takeCount)
        {
            IList<TechnicalSpecialistContactInfo> result = null;
            Exception exception = null;
            try
            {
                var searchResponse = _repository.Search(searchModel, takeCount);
                
                //if (takeCount > 0)
                //    searchResponse = searchResponse.Take(takeCount).ToList();

                result = _mapper.Map<IList<TechnicalSpecialistContactInfo>>(searchResponse);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(TechnicalSpecialistContactInfo searchModel)
        {
            IList<TechnicalSpecialistContactInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistContactInfo>>(_repository.Search(searchModel));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> contactIds)
        {
            IList<TechnicalSpecialistContactInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistContactInfo>>(GetContactInfoById(contactIds));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contactIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByPinId(IList<string> pinIds)
        {
            IList<TechnicalSpecialistContactInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistContactInfo>>(GetContactInfoByPin(pinIds));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), pinIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByPinAndContactType(IList<string> pins, IList<string> contactTypes)
        {
            IList<TechnicalSpecialistContactInfo> result = null;
            Exception exception = null;
            try
            {
                if (pins?.Count > 0)
                {
                    result = _mapper.Map<IList<TechnicalSpecialistContactInfo>>(_repository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString()) && contactTypes.Contains(x.ContactType)).ToList());
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), pins, contactTypes);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByPinAndContactType(IList<int> pins, IList<string> contactTypes)
        {
            IList<TechnicalSpecialistContactInfo> result = null;
            Exception exception = null;
            try
            {
                if (pins?.Count > 0)
                {
                    result = _mapper.Map<IList<TechnicalSpecialistContactInfo>>(_repository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin) && contactTypes.Contains(x.ContactType)).ToList());
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), pins, contactTypes);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }



        #endregion

        #region Add 

        public Response Add(IList<TechnicalSpecialistContactInfo> tsContacts,
                            bool commitChange = true,
                            bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistContact> dbTsContact = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Country> dbCountries = null;
            IList<DbModel.County> dbCounties = null;
            IList<DbModel.City> dbCity = null;

            return AddTechSpecialistContact(tsContacts, ref dbTsContact, ref dbTechnicalSpecialists, ref dbCountries, ref dbCounties, ref dbCity, commitChange, isDbValidationRequired);
        }

        public Response Add(IList<TechnicalSpecialistContactInfo> tsContacts,
                            ref IList<DbModel.TechnicalSpecialistContact> dbTsContacts,
                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                            ref IList<DbModel.Country> dbCountries,
                            ref IList<DbModel.County> dbcounties,
                            ref IList<DbModel.City> dbcities,
                            bool commitChange = true,
                            bool isDbValidationRequired = true)
        {
            return AddTechSpecialistContact(tsContacts, ref dbTsContacts, ref dbTechnicalSpecialists, ref dbCountries, ref dbcounties, ref dbcities, commitChange, isDbValidationRequired);
        }
        #endregion

        #region Modify

        public Response Modify(IList<TechnicalSpecialistContactInfo> tsContacts,
                                bool commitChange = true,
                                bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistContact> dbTsContact = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Country> dbCountries = null;
            IList<DbModel.County> dbCounties = null;
            IList<DbModel.City> dbCity = null;

            return UpdateTechSpecialistContacts(tsContacts, ref dbTsContact, ref dbTechnicalSpecialists, ref dbCountries, ref dbCounties, ref dbCity, commitChange, isDbValidationRequired);
        }

        public Response Modify(IList<TechnicalSpecialistContactInfo> tsContacts,
                                ref IList<DbModel.TechnicalSpecialistContact> dbTsContacts,
                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                ref IList<DbModel.Country> dbCountries,
                                ref IList<DbModel.County> dbcounties,
                                ref IList<DbModel.City> dbcities,
                                bool commitChange = true,
                                bool isDbValidationRequired = true)
        {
            return UpdateTechSpecialistContacts(tsContacts, ref dbTsContacts, ref dbTechnicalSpecialists, ref dbCountries, ref dbcounties, ref dbcities, commitChange, isDbValidationRequired);
        }


        public Response UpdateContactSyncStatus(IList<int> tsContactIds)
        {
            bool result = true;
            Exception exception = null;
            try
            {
                if (tsContactIds?.Count > 0)
                    result = _repository.UpdateContactSyncStatus(tsContactIds);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsContactIds);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }

        #endregion

        #region Delete

        public Response Delete(IList<TechnicalSpecialistContactInfo> tsContacts,
                               bool commitChange = true,
                               bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistContact> dbTsContacts = null;
            return RemoveTechSpecialistContacts(tsContacts, ref dbTsContacts, commitChange, isDbValidationRequired);
        }


        public Response Delete(IList<TechnicalSpecialistContactInfo> tsContacts,
            ref IList<DbModel.TechnicalSpecialistContact> dbTsContacts,
                               bool commitChange = true,
                               bool isDbValidationRequired = true)
        {
            return RemoveTechSpecialistContacts(tsContacts, ref dbTsContacts, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Validation

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistContactInfo> tsContacts,
                                                ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialistContact> dbTsContact = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Country> dbCountries = null;
            IList<DbModel.County> dbCounties = null;
            IList<DbModel.City> dbCity = null;

            return IsRecordValidForProcess(tsContacts, validationType, ref dbTechnicalSpecialists, ref dbCountries, ref dbCounties, ref dbCity, ref dbTsContact);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistContactInfo> tsContacts, ValidationType validationType,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                ref IList<DbModel.Country> dbCountries,
                                                ref IList<DbModel.County> dbCounties,
                                                ref IList<DbModel.City> dbCity,
                                                ref IList<DbModel.TechnicalSpecialistContact> dbTsContact,
                                                bool isDraft = false)
        {
            IList<TechnicalSpecialistContactInfo> filteredTSContacts = null;

            return CheckRecordValidForProcess(tsContacts, validationType, ref filteredTSContacts, ref dbTsContact,
                                                   ref dbTechnicalSpecialists, ref dbCountries, ref dbCounties, ref dbCity, isDraft);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistContactInfo> tsContacts,
                                                 ValidationType validationType,
                                                 IList<DbModel.TechnicalSpecialistContact> dbTsContacts)
        {
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Country> dbCountries = null;
            IList<DbModel.County> dbCounties = null;
            IList<DbModel.City> dbCity = null;

            return IsRecordValidForProcess(tsContacts, validationType, ref dbTechnicalSpecialists, ref dbCountries, ref dbCounties, ref dbCity, ref dbTsContacts);
        }

        public Response IsRecordExistInDb(IList<int> tsContactIds,
                                        ref IList<DbModel.TechnicalSpecialistContact> dbTsContact,
                                        ref IList<ValidationMessage> validationMessages)
        {
            IList<int> tsContactIdNotExists = null;
            return IsRecordExistInDb(tsContactIds, ref dbTsContact, ref tsContactIdNotExists, ref validationMessages);
        }

        public Response IsRecordExistInDb(IList<int> tsContactIds,
                                          ref IList<DbModel.TechnicalSpecialistContact> dbTsContact,
                                          ref IList<int> tsContactIdNotExists,
                                          ref IList<ValidationMessage> validationMessages)
        {

            Exception exception = null;
            bool result = true;
            try
            {
                if (dbTsContact == null && tsContactIds?.Count > 0)
                    dbTsContact = GetContactInfoById(tsContactIds);

                result = IsTSContactExistInDb(tsContactIds, dbTsContact, ref tsContactIdNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsContactIds);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        #endregion

        #endregion

        #region Private Methods

        #region Get
        private IList<DbModel.TechnicalSpecialistContact> GetContactInfoByPin(IList<string> pins)
        {
            IList<DbModel.TechnicalSpecialistContact> dbTsContactInfos = null;
            if (pins?.Count > 0)
            {
                dbTsContactInfos = _repository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString())).ToList();
            }
            return dbTsContactInfos;
        }

        private IList<DbModel.TechnicalSpecialistContact> GetContactInfoById(IList<int> tsContactIds)
        {
            IList<DbModel.TechnicalSpecialistContact> dbTsContactInfos = null;
            if (tsContactIds?.Count > 0)
                dbTsContactInfos = _repository.FindBy(x => tsContactIds.Contains(x.Id)).ToList();

            return dbTsContactInfos;
        }
        #endregion

        private Response AddTechSpecialistContact(IList<TechnicalSpecialistContactInfo> tsContacts,
                                                    ref IList<DbModel.TechnicalSpecialistContact> dbTsContacts,
                                                    ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                    ref IList<DbModel.Country> dbCountries,
                                                    ref IList<DbModel.County> dbcounties,
                                                    ref IList<DbModel.City> dbcities,
                                                    bool commitChange = true,
                                                    bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                IList<TechnicalSpecialistContactInfo> recordToBeAdd = null;
                IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
                IList<DbModel.Country> dbCountry = null;
                IList<DbModel.County> dbcounty = null;
                IList<DbModel.City> dbCity = null;
                eventId = tsContacts?.FirstOrDefault().EventId;
                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsContacts, ValidationType.Add, ref recordToBeAdd,
                                                              ref dbTsContacts, ref dbTechnicalSpecialists, ref dbCountries,
                                                              ref dbcounties, ref dbcities);
                }

                if (!isDbValidationRequired && tsContacts?.Count > 0)
                {
                    recordToBeAdd = FilterRecord(tsContacts, ValidationType.Add);
                }

                if (!isDbValidationRequired || Convert.ToBoolean(valdResponse.Result))
                {
                    dbTechSpecialists = dbTechnicalSpecialists;
                    dbCountry = dbCountries;
                    dbcounty = dbcounties;
                    dbCity = dbcities;

                    recordToBeAdd = recordToBeAdd.Select(x => { x.Id = 0; return x; }).ToList();
                    _repository.AutoSave = false;
                    var mappedRecords = recordToBeAdd.Select(x => new DbRepository.Models.SqlDatabaseContext.TechnicalSpecialistContact()
                    {
                        TechnicalSpecialistId = dbTechSpecialists.FirstOrDefault(x1 => x1.Pin == x.Epin).Id,
                        Address = (x.ContactType.ToEnum<ContactType>() == ContactType.PrimaryAddress) ? x.Address : null,
                        CountryId = (x.ContactType.ToEnum<ContactType>() == ContactType.PrimaryAddress) ? x.CountryId : null, // Added For ITK DEf 1536
                        CountyId = (x.ContactType.ToEnum<ContactType>() == ContactType.PrimaryAddress) ? x.CountyId : null, // Added For ITK DEf 1536
                        CityId = (x.ContactType.ToEnum<ContactType>() == ContactType.PrimaryAddress) ? x.CityId : null, // Added For ITK DEf 1536
                        //CountryId = (x.ContactType.ToEnum<ContactType>() == ContactType.PrimaryAddress) ? dbCountry?.FirstOrDefault(x1 => x1.Name == x.Country)?.Id : null,
                        //CountyId = (x.ContactType.ToEnum<ContactType>() == ContactType.PrimaryAddress) ? dbcounty?.FirstOrDefault(x1 => x1.Name == x.County)?.Id : null,
                        //CityId = (x.ContactType.ToEnum<ContactType>() == ContactType.PrimaryAddress) ? dbCity?.FirstOrDefault(x1 => x1.Name == x.City && x1.County.Name ==  x.County)?.Id : null,
                        PostalCode = (x.ContactType.ToEnum<ContactType>() == ContactType.PrimaryAddress) ? x.PostalCode : null,
                        EmailAddress = ((x.ContactType.ToEnum<ContactType>() == ContactType.PrimaryEmail || x.ContactType.ToEnum<ContactType>() == ContactType.SecondaryEmail)) ? x.EmailAddress : null,
                        TelephoneNumber = ((x.ContactType.ToEnum<ContactType>() == ContactType.PrimaryPhone) || (x.ContactType.ToEnum<ContactType>() == ContactType.Emergency)) ? x.TelephoneNumber : null,
                        FaxNumber = (x.ContactType.ToEnum<ContactType>() == ContactType.Fax) ? x.FaxNumber : null,
                        MobileNumber = (x.ContactType.ToEnum<ContactType>() == ContactType.PrimaryMobile) ? x.MobileNumber : null,
                        EmergencyContactName = (x.ContactType.ToEnum<ContactType>() == ContactType.Emergency) ? x.EmergencyContactName : null,
                        ContactType = x.ContactType,
                        DisplayOrder = 1,
                        ModifiedBy = x.ModifiedBy,
                        LastModification = DateTime.UtcNow,
                        UpdateCount = x.UpdateCount,
                        IsGeoCordinateSync = (x.ContactType.ToEnum<ContactType>() == ContactType.PrimaryAddress) ? false : (bool?)null,
                    }).ToList();
                    _repository.Add(mappedRecords);

                    if (commitChange)
                        _repository.ForceSave();

                    dbTsContacts.AddRange(mappedRecords);

                    if (mappedRecords?.Count > 0)
                        mappedRecords?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, tsContacts.FirstOrDefault().ActionByUser,
                                                                                          null,
                                                                                         ValidationType.Add.ToAuditActionType(),
                                                                                           SqlAuditModuleType.TechnicalSpecialistContact,
                                                                                           null,
                                                                                          _mapper.Map<TechnicalSpecialistContactInfo>(x1)
                                                                                           ));
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsContacts);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateTechSpecialistContacts(IList<TechnicalSpecialistContactInfo> tsContacts,
                                                      ref IList<DbModel.TechnicalSpecialistContact> dbTsContacts,
                                                      ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                      ref IList<DbModel.Country> dbCountries,
                                                      ref IList<DbModel.County> dbcounties,
                                                      ref IList<DbModel.City> dbcities,
                                                      bool commitChange = true,
                                                      bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            IList<DbModel.Country> dbCountry = null;
            IList<DbModel.County> dbCounty = null;
            IList<DbModel.City> dbCity = null;
            long? eventId = 0;
            Response valdResponse = null;
            IList<TechnicalSpecialistContactInfo> recordToBeModify = null;
            bool valdResult = false;
            try
            {
                eventId = tsContacts?.FirstOrDefault().EventId;
                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsContacts, ValidationType.Update, ref recordToBeModify, ref dbTsContacts, ref dbTechnicalSpecialists, ref dbCountries, ref dbcounties, ref dbcities);
                    valdResult = Convert.ToBoolean(valdResponse.Result);
                }
                if (!isDbValidationRequired && tsContacts?.Count > 0)
                {
                    recordToBeModify = FilterRecord(tsContacts, ValidationType.Update);
                }
                if (recordToBeModify?.Count > 0)
                {
                    if (dbTsContacts == null || (dbTsContacts?.Count <= 0 && !valdResult))
                    {
                        dbTsContacts = _repository.Get(recordToBeModify?.Select(x => x.Id).ToList());
                    }

                    if (recordToBeModify.Any(x => x.ContactType.ToEnum<ContactType>() == ContactType.PrimaryAddress))
                    {
                        if (dbTechnicalSpecialists == null || (dbTechnicalSpecialists?.Count <= 0 && !valdResult))
                        {
                            //valdResult = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages).Result);
                            valdResult = IsTechSpecialistExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                        }
                        if (dbCountries == null || (dbCountries?.Count <= 0 && !valdResult))
                        {
                            valdResult = _countryService.IsValidCountryName(recordToBeModify.Select(x => x.Country).ToList(), ref dbCountries, ref validationMessages, county => county.County);
                        }
                        if (dbcounties == null || (dbcounties?.Count <= 0 && !valdResult))
                        {
                            valdResult = _countyService.IsValidCounty(recordToBeModify.Select(x => x.County).ToList(), ref dbcounties, ref validationMessages, county => county.City);
                        }
                        if (dbcities == null || (dbcities?.Count <= 0 && !valdResult))
                        {
                            valdResult = _cityService.IsValidCity(recordToBeModify.Select(x => x.City).ToList(), dbcounties, ref dbcities, ref validationMessages);
                        }
                    }
                    IList<TechnicalSpecialistContactInfo> domExsistingTechSplContacts = new List<TechnicalSpecialistContactInfo>();
                    if (!isDbValidationRequired || (valdResult && dbTsContacts?.Count > 0))
                    {
                        dbTechSpecialists = dbTechnicalSpecialists;
                        dbCountry = dbCountries;
                        dbCounty = dbcounties;
                        dbCity = dbcities;
                         
                        domExsistingTechSplContacts.AddRange(ObjectExtension.Clone(_mapper.Map<List<TechnicalSpecialistContactInfo>>(dbTsContacts)));
                         
                        dbTsContacts.ToList().ForEach(tsContactInfo =>
                        { 
                            var dbContactInfo = recordToBeModify.FirstOrDefault(x => x.Id == tsContactInfo.Id);
                            if (dbContactInfo != null)
                            {
                                if (!string.Equals(dbContactInfo.Address, tsContactInfo.Address) ||
                               !string.Equals(dbContactInfo.PostalCode, tsContactInfo.PostalCode) ||
                               dbCountry?.FirstOrDefault(x1 => x1.Id == dbContactInfo.CountryId)?.Id != tsContactInfo.CountryId ||
                               dbCounty?.FirstOrDefault(x1 => x1.Id == dbContactInfo.CountyId)?.Id != tsContactInfo.CountyId ||
                               dbCity?.FirstOrDefault(x1 => x1.Id == dbContactInfo.CityId)?.Id != tsContactInfo.CityId)
                                {
                                    tsContactInfo.IsGeoCordinateSync = false;
                                }
                                tsContactInfo.Address = dbContactInfo.ContactType.ToEnum<ContactType>() == ContactType.PrimaryAddress ? dbContactInfo.Address : null;
                                tsContactInfo.CountryId = dbContactInfo.ContactType.ToEnum<ContactType>() == ContactType.PrimaryAddress ? dbContactInfo.CountryId : null;
                                tsContactInfo.CountyId = dbContactInfo.ContactType.ToEnum<ContactType>() == ContactType.PrimaryAddress ? dbContactInfo.CountyId : null;
                                tsContactInfo.CityId = dbContactInfo.ContactType.ToEnum<ContactType>() == ContactType.PrimaryAddress ? dbContactInfo.CityId : null;
                                tsContactInfo.PostalCode = dbContactInfo.ContactType.ToEnum<ContactType>() == ContactType.PrimaryAddress ? dbContactInfo.PostalCode : null;
                                tsContactInfo.EmailAddress = (dbContactInfo.ContactType.ToEnum<ContactType>() == ContactType.PrimaryEmail || dbContactInfo.ContactType.ToEnum<ContactType>() == ContactType.SecondaryEmail) ? dbContactInfo.EmailAddress : null;
                                tsContactInfo.TelephoneNumber = ((dbContactInfo.ContactType.ToEnum<ContactType>() == ContactType.PrimaryPhone) || dbContactInfo.ContactType.ToEnum<ContactType>() == ContactType.Emergency) ? dbContactInfo.TelephoneNumber : null;
                                tsContactInfo.FaxNumber = dbContactInfo.ContactType.ToEnum<ContactType>() == ContactType.Fax ? dbContactInfo.FaxNumber : null;
                                tsContactInfo.MobileNumber = dbContactInfo.ContactType.ToEnum<ContactType>() == ContactType.PrimaryMobile ? dbContactInfo.MobileNumber : null;
                                tsContactInfo.EmergencyContactName = dbContactInfo.ContactType.ToEnum<ContactType>() == ContactType.Emergency ? dbContactInfo.EmergencyContactName : null;
                                tsContactInfo.ContactType = dbContactInfo.ContactType;
                                tsContactInfo.DisplayOrder = 0;
                                tsContactInfo.LastModification = DateTime.UtcNow;
                                tsContactInfo.UpdateCount = tsContactInfo.UpdateCount.CalculateUpdateCount(); 
                                tsContactInfo.ModifiedBy = dbContactInfo.ModifiedBy;
                                _repository.Update(tsContactInfo);
                            }
                        });
                        _repository.AutoSave = false;

                        if (commitChange && !_repository.AutoSave && recordToBeModify?.Count > 0)
                        { 
                            _repository.ForceSave();
                            if (recordToBeModify?.Count > 0)
                            {
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                      null,
                                                                                                      ValidationType.Update.ToAuditActionType(),
                                                                                                      SqlAuditModuleType.TechnicalSpecialistContact,
                                                                                                      domExsistingTechSplContacts?.FirstOrDefault(x2 => x2.Id == x1.Id),
                                                                                                      x1
                                                                                                       ));
                            }
                        }
                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsContacts);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }
        private Response RemoveTechSpecialistContacts(IList<TechnicalSpecialistContactInfo> tsContacts,
                ref IList<DbModel.TechnicalSpecialistContact> dbTsContacts,
                                                    bool commitChange = true,
                                                    bool isDbValidationRequire = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            IList<DbModel.Country> dbCountries = null;
            IList<DbModel.County> dbcounties = null;
            IList<DbModel.City> dbCity = null;
            IList<TechnicalSpecialistContactInfo> recordToBeDeleted = null;
            long? eventId = 0;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                eventId = tsContacts?.FirstOrDefault().EventId;

                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(tsContacts, ValidationType.Delete, ref dbTechSpecialists, ref dbCountries, ref dbcounties, ref dbCity, ref dbTsContacts);
                 
                if (tsContacts?.Count > 0)
                {
                    recordToBeDeleted = FilterRecord(tsContacts, ValidationType.Delete);
                }

                if (recordToBeDeleted?.Count > 0 && (response == null || Convert.ToBoolean(response.Result)) && dbTsContacts?.Count > 0)
                {
                    var dbTsContactToBeDeleted = dbTsContacts?.Where(x => recordToBeDeleted.Any(x1 => x1.Id == x.Id)).ToList();
                    _repository.AutoSave = false;
                    _repository.Delete(dbTsContactToBeDeleted);
                    if (commitChange)
                    { 
                        _repository.ForceSave();
                        if (recordToBeDeleted.Count > 0)
                            recordToBeDeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                   null,
                                                                                                    ValidationType.Delete.ToAuditActionType(),
                                                                                                    SqlAuditModuleType.TechnicalSpecialistContact,
                                                                                                     x1,
                                                                                                     null
                                                                                                    ));
                    }
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsContacts);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<TechnicalSpecialistContactInfo> tsContacts,
                                                 ValidationType validationType,
                                                 ref IList<TechnicalSpecialistContactInfo> filteredTsContacts,
                                                 ref IList<DbModel.TechnicalSpecialistContact> dbTsContacts,
                                                 ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                 ref IList<DbModel.Country> dbCountry,
                                                 ref IList<DbModel.County> dbCounty,
                                                 ref IList<DbModel.City> dbcity,
                                                  bool isDraft = false)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tsContacts, ref filteredTsContacts, ref dbTsContacts, ref dbTechnicalSpecialists, ref dbCountry, ref dbCounty, ref dbcity, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(tsContacts, ref filteredTsContacts, ref dbTsContacts, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tsContacts, ref filteredTsContacts, ref dbTsContacts, ref dbTechnicalSpecialists, ref dbCountry, ref dbCounty, ref dbcity, ref validationMessages, isDraft);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsContacts);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsRecordValidForAdd(IList<TechnicalSpecialistContactInfo> tsContacts,
                                        ref IList<TechnicalSpecialistContactInfo> filteredTsContact,
                                        ref IList<DbModel.TechnicalSpecialistContact> dbTsContact,
                                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                        ref IList<DbModel.Country> dbCountry,
                                        ref IList<DbModel.County> dbcounty,
                                        ref IList<DbModel.City> dbCity,
                                        ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsContacts != null && tsContacts.Count > 0)
            {
                ValidationType validationType = ValidationType.Add; 
                    validationMessages = validationMessages ??  new List<ValidationMessage>();

                if (filteredTsContact == null || filteredTsContact.Count <= 0)
                    filteredTsContact = FilterRecord(tsContacts, validationType);

                if (filteredTsContact?.Count > 0 && IsValidPayload(filteredTsContact, validationType, ref validationMessages))
                {
                    IList<string> tsEpin = filteredTsContact.Select(x => x.Epin.ToString()).ToList();
                    IList<string> countries = filteredTsContact.Where(x => !string.IsNullOrEmpty(x.Country)).Select(x => x.Country).ToList();
                    IList<string> counties = filteredTsContact.Where(x => !string.IsNullOrEmpty(x.County)).Select(x => x.County).ToList();
                    IList<string> cities = filteredTsContact.Where(x => !string.IsNullOrEmpty(x.City)).Select(x => x.City).ToList();


                    if (tsEpin?.Count > 0)
                    {
                        //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                        result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages);
                    }
                    if (result && countries?.Count > 0)
                        result = _countryService.IsValidCountryName(countries, ref dbCountry, ref validationMessages);
                    if (result && counties?.Count > 0)
                    {
                        result = _countyService.IsValidCounty(counties, ref dbcounty, ref validationMessages, county => county.City);
                    }
                    if (result && cities?.Count > 0)
                    {
                        result = _cityService.IsValidCity(cities, dbcounty, ref dbCity, ref validationMessages);
                    }

                }
            }
            return result;
        }
        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistContactInfo> tsContact,
                                        ref IList<TechnicalSpecialistContactInfo> filteredTsContact,
                                        ref IList<DbModel.TechnicalSpecialistContact> dbTsContact,
                                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                        ref IList<DbModel.Country> dbCountry,
                                        ref IList<DbModel.County> dbcounty,
                                        ref IList<DbModel.City> dbcity,
                                        ref IList<ValidationMessage> validationMessages,
                                        bool isDraft = false)
        {
            bool result = false;
            if (tsContact != null && tsContact.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>(); 
                    validationMessages = validationMessages ??  new List<ValidationMessage>();

                if (filteredTsContact == null || filteredTsContact.Count <= 0)
                    filteredTsContact = FilterRecord(tsContact, validationType);

                if (filteredTsContact?.Count > 0 && IsValidPayload(filteredTsContact, validationType, ref messages))
                {
                    GetTsContactDbInfo(filteredTsContact, ref dbTsContact);
                    IList<int> tsContactIds = filteredTsContact.Select(x => x.Id).ToList();
                    var dbTsInfosByIds = dbTsContact;
                    var idNotExists = tsContactIds.Where(id => !dbTsInfosByIds.Any(tp => tp.Id == id)).ToList();
                    if (idNotExists != null && idNotExists?.Count > 0) //Invalid  Id found.
                    {
                        var techSpecialistTaxonomyList = filteredTsContact;
                        idNotExists?.ForEach(tsId =>
                        {
                            var tsContactID = techSpecialistTaxonomyList.FirstOrDefault(x1 => x1.Id == tsId);
                            messages.Add(_messages, tsContactID, MessageType.InvalidContactInfo);
                        });
                    }
                    else
                    {
                        result = isDraft ? true : IsRecordUpdateCountMatching(filteredTsContact, dbTsContact, ref messages);
                        if (result)
                        {
                            IList<string> tsEpin = filteredTsContact.Select(x => x.Epin.ToString()).ToList();
                            IList<string> countries = filteredTsContact.Where(x => !string.IsNullOrEmpty(x.Country)).Select(x => x.Country).ToList();
                            IList<string> counties = filteredTsContact.Where(x => !string.IsNullOrEmpty(x.County)).Select(x => x.County).ToList();
                            IList<string> cities = filteredTsContact.Where(x => !string.IsNullOrEmpty(x.City)).Select(x => x.City).ToList();

                            if (tsEpin?.Count > 0)
                            {
                                //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                                result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages);
                            }
                            if (result && countries?.Count > 0)
                                result = _countryService.IsValidCountryName(countries, ref dbCountry, ref validationMessages);
                            if (result && counties?.Count > 0)
                            {
                                result = _countyService.IsValidCounty(counties, ref dbcounty, ref validationMessages, county => county.City);
                            }
                            if (result && cities?.Count > 0)
                            {
                                result = _cityService.IsValidCity(cities, dbcounty, ref dbcity, ref validationMessages);
                            }

                        }

                    }
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return result;
        }

        private bool IsRecordValidForRemove(IList<TechnicalSpecialistContactInfo> tsContact,
                                           ref IList<TechnicalSpecialistContactInfo> filteredTsContact,
                                           ref IList<DbModel.TechnicalSpecialistContact> dbTsContact,
                                           ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsContact != null && tsContact.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete; 
                    validationMessages = validationMessages ??  new List<ValidationMessage>();

                if (filteredTsContact == null || filteredTsContact.Count <= 0)
                    filteredTsContact = FilterRecord(tsContact, validationType);

                if (filteredTsContact?.Count > 0 && IsValidPayload(filteredTsContact, validationType, ref validationMessages))
                {
                    GetTsContactDbInfo(filteredTsContact, ref dbTsContact);
                    IList<int> tsTaxonomyIdNotExists = null;
                    var tsTaxonomyIds = filteredTsContact.Select(x => x.Id).Distinct().ToList();
                    result = IsTSContactExistInDb(tsTaxonomyIds, dbTsContact, ref tsTaxonomyIdNotExists, ref validationMessages);

                }
            }
            return result;
        }

        private bool IsRecordUpdateCountMatching(IList<TechnicalSpecialistContactInfo> tsContact,
                                              IList<DbModel.TechnicalSpecialistContact> dbTsContact,
                                              ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>(); 
                validationMessages = validationMessages ?? new List<ValidationMessage>();

            var notMatchedRecords = tsContact.Where(x => !dbTsContact.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x, MessageType.ContactUpdatedByOtherUser, x.Epin);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }
        private IList<TechnicalSpecialistContactInfo> FilterRecord(IList<TechnicalSpecialistContactInfo> tsContact, ValidationType filterType)
        {
            IList<TechnicalSpecialistContactInfo> filterTsInfos = null;

            if (filterType == ValidationType.Add)
                filterTsInfos = tsContact?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsInfos = tsContact?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsInfos = tsContact?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsInfos;
        }
        private bool IsValidPayload(IList<TechnicalSpecialistContactInfo> tsContact,
                           ValidationType validationType,
                           ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>(); 
                validationMessages = validationMessages ??  new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(tsContact), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Security, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }
        private void GetTsContactDbInfo(IList<TechnicalSpecialistContactInfo> filteredTsContact,
                                        ref IList<DbModel.TechnicalSpecialistContact> dbTsContact)
        {
            IList<int> tsContactIds = filteredTsContact?.Select(x => x.Id).Distinct().ToList();
            if (dbTsContact == null || dbTsContact.Count <= 0)
                dbTsContact = GetContactInfoById(tsContactIds); 
        }

        private bool IsTSContactExistInDb(IList<int> tsContactIds,
                                              IList<DbModel.TechnicalSpecialistContact> dbTsContact,
                                              ref IList<int> tsContactIdNotExists,
                                              ref IList<ValidationMessage> validationMessages)
        {
            var validMessages= validationMessages = validationMessages ?? new List<ValidationMessage>(); 
                dbTsContact = dbTsContact ?? new List<DbModel.TechnicalSpecialistContact>(); 

            if (tsContactIds?.Count > 0)
            {
                tsContactIdNotExists = tsContactIds.Where(id => !dbTsContact.Any(x1 => x1.Id == id))
                                     .Select(id => id)
                                     .ToList();

                tsContactIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.InvalidContactInfo, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsTechSpecialistExistInDb(IList<string> tsPins, ref IList<DbModel.TechnicalSpecialist> dbTsInfos, ref IList<ValidationMessage> validationMessages)
        {
            if ((dbTsInfos?.Count == 0 || dbTsInfos == null) && tsPins?.Count > 0)
                dbTsInfos = _technicalSpecialistRepository.FindBy(x => tsPins.Contains(x.Pin.ToString())).ToList();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTsInfos == null)
                dbTsInfos = new List<DbModel.TechnicalSpecialist>();

            var validMessages = validationMessages;
            var dbTechSpecs = dbTsInfos;

            if (tsPins?.Count > 0)
            {
                IList<string> tsPinNotExists = tsPins.Where(pin => !dbTechSpecs.Any(x1 => x1.Pin.ToString() == pin))
                                        .Select(pin => pin)
                                        .ToList();

                tsPinNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsEPinDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        #endregion


    }
}
