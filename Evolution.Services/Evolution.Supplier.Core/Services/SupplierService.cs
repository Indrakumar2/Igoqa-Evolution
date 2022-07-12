using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Supplier.Domain.Interfaces.Data;
using Evolution.Supplier.Domain.Interfaces.Suppliers;
using Evolution.Supplier.Domain.Interfaces.Validations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Supplier.Core.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<SupplierService> _logger = null;
        private readonly ISupplierRepository _repository = null;
        private readonly ICountryService _countryService = null;
        private readonly IStateService _stateService = null;
        private readonly ICityService _cityService = null;
        private readonly JObject _messages = null;
        private readonly ISupplierValidationService _validationService = null;
        private readonly IAppLogger<LogEventGeneration> _applogger = null;
        private readonly IMongoDocumentService _mongoDocumentService = null;
        private readonly IAuditSearchService _auditSearchService = null;

        #region Constructor

        public SupplierService(IMapper mapper,
                               IAppLogger<SupplierService> logger,
                               ISupplierRepository repository,
                               ICityService cityService,
                               ICountryService countryService,
                               IStateService stateService,
                               ISupplierValidationService validationService,
                             //  IAuditLogger auditLogger,
                               IAppLogger<LogEventGeneration> applogger,
                               IMongoDocumentService mongoDocumentService,
                               JObject messages,
                               IAuditSearchService auditSearchService )
                               //ISqlAuditModuleService sqlAuditModuleService)
        {
            this._mongoDocumentService = mongoDocumentService;
            this._mapper = mapper;
            this._logger = logger;
            this._repository = repository;
            this._cityService = cityService;
            this._countryService = countryService;
            this._stateService = stateService;
            this._validationService = validationService;
           // this._auditLogger = auditLogger;
            this._applogger = applogger;
            this._messages = messages;
            _auditSearchService = auditSearchService;
        }
        #endregion

        #region Public Methods

        #region Get

        public async Task<Response> Get(DomainModel.SupplierSearch searchModel)
        {
            IList<DomainModel.Supplier> result = null;
            Exception exception = null;
            IList<string> mongoSearch = null;
            try
            {
                if (!string.IsNullOrEmpty(searchModel.SearchDocumentType) || !string.IsNullOrEmpty(searchModel.DocumentSearchText))
                {
                    var evoMongoDocSearch = _mapper.Map<Document.Domain.Models.Document.EvolutionMongoDocumentSearch>(searchModel);
                    mongoSearch = await this._mongoDocumentService.SearchDistinctFieldAsync(evoMongoDocSearch);
                    if (mongoSearch != null && mongoSearch.Count > 0)
                    { 
                        searchModel.SupplierIds = mongoSearch;
                        using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                              new TransactionOptions
                                              {
                                                  IsolationLevel = IsolationLevel.ReadUncommitted
                                              }))
                        {
                            result = this._repository.Search(searchModel, city => city.City, county => county.City.County, country => country.City.County.Country);
                            tranScope.Complete();
                        }
                        if (result?.Count > 0)
                            result = result.Where(x => mongoSearch.Contains(x.SupplierId.ToString())).ToList();
                    }
                    else
                        result = new List<DomainModel.Supplier>();
                }
                else
                {
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                 new TransactionOptions
                                                 {
                                                     IsolationLevel = IsolationLevel.ReadUncommitted
                                                 }))
                    {
                        result = this._repository.Search(searchModel, city => city.City, county => county.City.County, country => country.City.County.Country);
                        tranScope.Complete();
                    }
                }

                //result = _mapper.Map<IList<DomainModel.Supplier>>(result);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> supplierNames)
        {
            IList<DomainModel.Supplier> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<DomainModel.Supplier>>(this.GetSupplierBySupplierName(supplierNames));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierNames);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> supplierIds)
        {
            IList<DomainModel.Supplier> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<DomainModel.Supplier>>(this.GetSupplierById(supplierIds));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response IsRecordExistInDb(IList<int> supplierIds,
                                          ref IList<DbModel.Supplier> dbSuppliers,
                                          ref IList<ValidationMessage> validationMessages,
                                          params Expression<Func<DbModel.Supplier, object>>[] includes)
        {
            IList<int> supplierIdNotExists = null;
            return IsRecordExistInDb(supplierIds, ref dbSuppliers, ref supplierIdNotExists, ref validationMessages, includes);
        }

        public Response IsRecordExistInDb(IList<int> supplierIds,
                                          ref IList<DbModel.Supplier> dbSuppliers,
                                          ref IList<int> supplierIdNotExists,
                                          ref IList<ValidationMessage> validationMessages,
                                          params Expression<Func<DbModel.Supplier, object>>[] includes)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                if (dbSuppliers == null && supplierIds?.Count > 0)
                    dbSuppliers = GetSupplierById(supplierIds, includes);

                result = IsSupplierExistInDb(supplierIds, dbSuppliers, ref supplierIdNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierIds);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }


        #endregion

        #region Add

        public Response Add(IList<DomainModel.Supplier> suppliers,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            IList<DbModel.Supplier> dbSuppliers = null;
            IList<DbModel.City> dbCities = null;
            IList<DbModel.Country> dbCountries = null;
            IList<DbModel.County> dbCounties = null;
            long? eventId = null;

            return AddSupplier(suppliers,null, ref dbSuppliers, ref dbCountries, ref dbCounties, ref dbCities, ref eventId, commitChange, isDbValidationRequire);
        }

        public Response Add(IList<DomainModel.Supplier> suppliers,
            IList<DbModel.SqlauditModule> dbModule,
                            ref IList<DbModel.Supplier> dbSuppliers,
                            ref IList<DbModel.Country> dbCountries,
                            ref IList<DbModel.County> dbCounties,
                            ref IList<DbModel.City> dbCities,
                            ref long? eventId,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            return AddSupplier(suppliers,  dbModule, ref dbSuppliers, ref dbCountries, ref dbCounties, ref dbCities, ref eventId, commitChange, isDbValidationRequire);
        }

        #endregion

        #region Modify

        public Response Modify(IList<DomainModel.Supplier> suppliers,
                               bool commitChange = true,
                               bool isDbValidationRequire = true)
        {
            IList<DbModel.Supplier> dbSuppliers = null;
            IList<DbModel.City> dbCities = null;
            IList<DbModel.Country> dbCountries = null;
            IList<DbModel.County> dbCounties = null;
            long? eventId = null;
            return UpdateSupplier(suppliers,null, ref dbSuppliers, ref dbCountries, ref dbCounties, ref dbCities, ref eventId, commitChange, isDbValidationRequire);
        }

        public Response Modify(IList<DomainModel.Supplier> suppliers,
            IList<DbModel.SqlauditModule> dbModule,
                               ref IList<DbModel.Supplier> dbSuppliers,
                               ref IList<DbModel.Country> dbCountries,
                               ref IList<DbModel.County> dbCounties,
                               ref IList<DbModel.City> dbCities,
                               ref long? eventId,
                               bool commitChange = true,
                               bool isDbValidationRequire = true)
        {
            return UpdateSupplier(suppliers, dbModule, ref dbSuppliers, ref dbCountries, ref dbCounties, ref dbCities, ref eventId, commitChange, isDbValidationRequire);
        }

        #endregion

        #region Delete

        public Response Delete(IList<DomainModel.Supplier> suppliers,
                               bool commitChange = true,
                               bool isDbValidationRequire = true)
        {
            IList<DbModel.Supplier> dbSuppliers = null;
            long? eventId = null;
            return this.RemoveSuppliers(suppliers,null, ref dbSuppliers, ref eventId, commitChange, isDbValidationRequire);
        }
        public Response Delete(IList<DomainModel.Supplier> suppliers,
            IList<DbModel.SqlauditModule> dbModule,
                               ref IList<DbModel.Supplier> dbSuppliers,
                               ref long? eventId,
                               bool commitChange = true,
                               bool isDbValidationRequire = true)
        {
            return this.RemoveSuppliers(suppliers, dbModule, ref dbSuppliers, ref eventId, commitChange, isDbValidationRequire);
        }

        #endregion

        #region Record Valid Check

        public Response IsRecordValidForProcess(IList<DomainModel.Supplier> suppliers,
                                                ValidationType validationType)
        {
            IList<DbModel.Supplier> dbSuppliers = null;
            IList<DbModel.City> dbCities = null;
            IList<DbModel.Country> dbCountries = null;
            IList<DbModel.County> dbCounties = null;

            return IsRecordValidForProcess(suppliers, validationType, ref dbSuppliers, ref dbCountries, ref dbCounties, ref dbCities);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.Supplier> suppliers,
                                                ValidationType validationType,
                                                ref IList<DbModel.Supplier> dbSuppliers,
                                                ref IList<DbModel.Country> dbCountries,
                                                ref IList<DbModel.County> dbCounties,
                                                ref IList<DbModel.City> dbCities)
        {
            IList<DomainModel.Supplier> filteredSupplier = null;
            return this.CheckRecordValidForProcess(suppliers, validationType, ref filteredSupplier, ref dbSuppliers, ref dbCountries, ref dbCounties, ref dbCities);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.Supplier> suppliers,
                                                ValidationType validationType,
                                                IList<DbModel.Supplier> dbSuppliers,
                                                IList<DbModel.Country> dbCountries,
                                                IList<DbModel.County> dbCounties,
                                                IList<DbModel.City> dbCities)
        {
            return IsRecordValidForProcess(suppliers, validationType, ref dbSuppliers, ref dbCountries, ref dbCounties, ref dbCities);
        }

        #endregion

        #endregion

        #region Private Metods

        #region Get

        private IList<DbModel.Supplier> GetSupplierBySupplierName(IList<string> supplierNames, params Expression<Func<DbModel.Supplier, object>>[] includes)
        {
            IList<DbModel.Supplier> dbSuppliers = null;
            if (supplierNames?.Count > 0)
                dbSuppliers = _repository.Get(supplierNames).ToList();

            return dbSuppliers;
        }

        private IList<DbModel.Supplier> GetSupplierById(IList<int> supplierIds, params Expression<Func<DbModel.Supplier, object>>[] includes)
        {
            IList<DbModel.Supplier> dbSuppliers = null;
            if (supplierIds?.Count > 0)
                dbSuppliers = _repository.Get(supplierIds).ToList();

            return dbSuppliers;
        }

        #endregion

        #region Add

        private bool IsRecordValidForAdd(IList<DomainModel.Supplier> suppliers,
                                         ref IList<DomainModel.Supplier> filteredSuppliers,
                                         ref IList<DbModel.Supplier> dbSuppliers,
                                         ref IList<DbModel.Country> dbCountries,
                                         ref IList<DbModel.County> dbCounties,
                                         ref IList<DbModel.City> dbCity,
                                         ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (suppliers != null && suppliers.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredSuppliers == null || filteredSuppliers.Count <= 0)
                    filteredSuppliers = FilterRecord(suppliers, validationType);

                if (filteredSuppliers?.Count > 0 && IsValidPayload(filteredSuppliers, validationType, ref validationMessages))
                {
                    var cities = filteredSuppliers.Where(x => !string.IsNullOrEmpty(x.City)).Select(x1 => x1.City).Distinct().ToList();
                    var countries = filteredSuppliers.Where(x => !string.IsNullOrEmpty(x.Country)).Select(x1 => x1.Country).Distinct().ToList();
                    var counties = filteredSuppliers.Where(x => !string.IsNullOrEmpty(x.State)).Select(x1 => x1.State).Distinct().ToList();

                    if (_countryService.IsValidCountryName(countries, ref dbCountries, ref validationMessages)
                       && _stateService.IsValidCounty(counties, ref dbCounties, ref validationMessages)
                       && _cityService.IsValidCity(cities, null, ref dbCity, ref validationMessages))


                        result = validationMessages.Count == 0 ? !result : result;




                }
            }
            return result;
        }

        private Response AddSupplier(IList<DomainModel.Supplier> suppliers,
            IList<DbModel.SqlauditModule> dbModule,
                                     ref IList<DbModel.Supplier> dbSuppliers,
                                     ref IList<DbModel.Country> dbCountries,
                                     ref IList<DbModel.County> dbCounties,
                                     ref IList<DbModel.City> dbCity,
                                     ref long? eventId,
                                     bool commitChange = true,
                                     bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventID = 0;
            try
            {
                Response valdResponse = null;
                
                IList<DomainModel.Supplier> recordToBeAdd = FilterRecord(suppliers, ValidationType.Add);
                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(suppliers, ValidationType.Add, ref recordToBeAdd, ref dbSuppliers, ref dbCountries, ref dbCounties, ref dbCity);

                if ((!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result)) && recordToBeAdd?.Count > 0)
                {
                    _repository.AutoSave = false;
                    IList<DbModel.City> dbCityToAdd = dbCity;
                    IList<DbModel.Country> dbCountryToAdd = dbCountries;
                    IList<DbModel.County> dbCountiesToAdd = dbCounties;

                    var mappedRecords = recordToBeAdd?.ToList().Select(x => new DbModel.Supplier()
                    {
                        SupplierName = x.SupplierName,
                        //CountryId = dbCountryToAdd.FirstOrDefault(x1 => x1.Name == x.Country)?.Id,
                        //CountyId = dbCountiesToAdd.FirstOrDefault(x1 => x1.Name == x.State)?.Id,
                        //CityId = dbCityToAdd?.FirstOrDefault(x1 => x1.Name == x.City)?.Id,
                        CountryId = x.CountryId,   //Added for D-1076
                        CountyId =x.StateId,      //Added for D-1076
                        CityId=x.CityId,          //Added for D-1076
                        PostalCode = x.PostalCode,
                        UpdateCount = 0,
                        ModifiedBy = x.ModifiedBy,
                        Address = x.SupplierAddress,
                    }).ToList();
                    _repository.Add(mappedRecords);

                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        if (value > 0)
                        {
                            dbSuppliers = mappedRecords;
                            dbSuppliers?.ToList().ForEach(x =>
                            recordToBeAdd?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1,ref eventID, x1.ActionByUser,
                                                                                               "{"+ AuditSelectType.Id+":" + x.Id.ToString() + "}${" + AuditSelectType.Name + ":" + x.SupplierName.ToString()+"}",
                                                                                               ValidationType.Add.ToAuditActionType(),
                                                                                               SqlAuditModuleType.Supplier,
                                                                                                null,
                                                                                                _mapper.Map<DomainModel.Supplier>(x),
                                                                                                dbModule
                                                                                              )));


                            eventId = eventID;
                        }
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), suppliers);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        #endregion

        #region Modify

        private bool IsRecordValidForUpdate(IList<DomainModel.Supplier> suppliers,
                                            ref IList<DomainModel.Supplier> filteredSuppliers,
                                            ref IList<DbModel.Supplier> dbSuppliers,
                                            ref IList<DbModel.Country> dbCountries,
                                            ref IList<DbModel.County> dbCounties,
                                            ref IList<DbModel.City> dbCity,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (suppliers != null && suppliers.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredSuppliers == null || filteredSuppliers.Count <= 0)
                    filteredSuppliers = FilterRecord(suppliers, validationType);
                if (filteredSuppliers?.Count > 0 && IsValidPayload(filteredSuppliers, validationType, ref validationMessages))
                {
                    if (this.IsValidSupplier(filteredSuppliers, ref dbSuppliers, ref validationMessages))
                    {
                        var countries = filteredSuppliers.Where(x => !string.IsNullOrEmpty(x.Country)).Select(x1 => x1.Country).Distinct().ToList();
                        var states = filteredSuppliers.Where(x => !string.IsNullOrEmpty(x.State)).Select(x1 => x1.State).Distinct().ToList();
                        var cities = filteredSuppliers.Where(x => !string.IsNullOrEmpty(x.City)).Select(x1 => x1.City).Distinct().ToList();
                        if (_countryService.IsValidCountryName(countries, ref dbCountries, ref validationMessages)
                        && _stateService.IsValidCounty(states, ref dbCounties, ref validationMessages)
                        && _cityService.IsValidCity(cities, dbCounties, ref dbCity, ref validationMessages)) //Added dbCounties for Sanity Defect 198


                            result = validationMessages.Count == 0 ? !result : result;

                    }
                }
            }
            return result;
        }

        private Response UpdateSupplier(IList<DomainModel.Supplier> suppliers,
            IList<DbModel.SqlauditModule> dbModule,
                                        ref IList<DbModel.Supplier> dbSuppliers,
                                        ref IList<DbModel.Country> dbCountries,
                                        ref IList<DbModel.County> dbCounties,
                                        ref IList<DbModel.City> dbCity,
                                        ref long? eventId,
                                        bool commitChange = true,
                                        bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventID = 0;

            try
            {
                Response valdResponse = null;
                IList<DomainModel.Supplier> dbExistingSuppliers = new List<DomainModel.Supplier>();
                var recordToBeModify = FilterRecord(suppliers, ValidationType.Update);

                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(suppliers, ValidationType.Update, ref recordToBeModify, ref dbSuppliers, ref dbCountries, ref dbCounties, ref dbCity);

                if ((dbSuppliers == null || (dbSuppliers?.Count <= 0 && valdResponse == null)) && recordToBeModify?.Count > 0)
                    dbSuppliers = _repository.Get(recordToBeModify?.Select(x => x.SupplierId ?? 0).ToList());

                IList<DbModel.City> dbCityToUpdate = dbCity;
                IList<DbModel.Country> dbCountryToUpdate = dbCountries;
                IList<DbModel.County> dbCountiesToUpdate = dbCounties;

                if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result) && dbSuppliers?.Count > 0))
                {

                    dbSuppliers.ToList().ForEach(x =>
                    {
                        dbExistingSuppliers.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.Supplier>(x)));
                    });

                    dbSuppliers?.ToList().ForEach(x =>
                    {
                        var recordToBeModified = recordToBeModify?.FirstOrDefault(x1 => x1.SupplierId == x.Id);
                        x.SupplierName = recordToBeModified?.SupplierName;
                        x.Address = recordToBeModified?.SupplierAddress;
                        //x.CountryId = dbCountryToUpdate?.FirstOrDefault(x1 => x1.Name == recordToBeModified.Country)?.Id;
                        //x.CountyId = dbCountiesToUpdate?.FirstOrDefault(x1 => x1.Name == recordToBeModified.State)?.Id;
                        //x.CityId = dbCityToUpdate?.FirstOrDefault(x1 => x1.Name == recordToBeModified.City)?.Id;
                        x.CountryId = recordToBeModified.CountryId;  //Added for D-1076
                        x.CountyId = recordToBeModified.StateId;     //Added for D-1076
                        x.CityId = recordToBeModified.CityId;        //Added for D-1076
                        x.PostalCode = recordToBeModified?.PostalCode;
                        x.UpdateCount = recordToBeModified.UpdateCount.CalculateUpdateCount();
                        x.LastModification = DateTime.UtcNow;
                        x.ModifiedBy = recordToBeModified.ModifiedBy;
                    });

                    _repository.AutoSave = false;
                    _repository.Update(dbSuppliers);
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        if (value > 0)
                        {
                            dbSuppliers?.ToList().ForEach(x =>
                               recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventID, x1.ActionByUser,
                                                                         "{" + AuditSelectType.Id + ":" + x.Id.ToString() + "}${" + AuditSelectType.Name + ":" + x.SupplierName.ToString() + "}",
                                                                         ValidationType.Update.ToAuditActionType(),
                                                                          SqlAuditModuleType.Supplier,
                                                                         _mapper.Map<DomainModel.Supplier>(dbExistingSuppliers?.FirstOrDefault(x2 => x2.SupplierId == x1.SupplierId)),
                                                                         x1,
                                                                         dbModule)));



                            eventId = eventID;
                        }
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), suppliers);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        #endregion

        #region Remove

        private bool IsRecordValidForRemove(IList<DomainModel.Supplier> suppliers,
                                         ref IList<DomainModel.Supplier> filteredSuppliers,
                                         ref IList<DbModel.Supplier> dbSuppliers,
                                         ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;

            if (suppliers != null && suppliers.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredSuppliers == null || filteredSuppliers.Count <= 0)
                    filteredSuppliers = FilterRecord(suppliers, validationType);

                if (filteredSuppliers?.Count > 0 && IsValidPayload(filteredSuppliers, validationType, ref validationMessages))
                {
                    if (this.IsValidSupplier(filteredSuppliers, ref dbSuppliers, ref messages))
                        result = this.IsRecordCanBeDeleted(filteredSuppliers, dbSuppliers, ref validationMessages);
                }

                if (messages.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return result;
        }

        private Response RemoveSuppliers(IList<DomainModel.Supplier> suppliers,
            IList<DbModel.SqlauditModule> dbModule,
                                               ref IList<DbModel.Supplier> dbSuppliers,
                                               ref long? eventId,
                                               bool commitChange, bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.City> dbCities = null;
            IList<DbModel.Country> dbCountries = null;
            IList<DbModel.County> dbCounties = null;
            IList<DbModel.Supplier> dbSupplierToDelete = null;
            long? eventID = 0;
            
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(suppliers, ValidationType.Delete, ref dbSuppliers, ref dbCountries, ref dbCounties, ref dbCities);
                
                if (!isDbValidationRequire || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result) && dbSuppliers?.Count > 0))
                {
                    dbSupplierToDelete = dbSuppliers;
                    _repository.AutoSave = false;
                    _repository.Delete(dbSuppliers);
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        if (value > 0)
                        {
                            dbSuppliers?.ToList().ForEach(x =>
                                 dbSupplierToDelete?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventID, suppliers?.FirstOrDefault()?.ActionByUser,
                                                                      "{" + AuditSelectType.Id + ":" + x.Id.ToString() + "}${" + AuditSelectType.Name + ":" + x.SupplierName.ToString() + "}",
                                                                    ValidationType.Delete.ToAuditActionType(),
                                                                    SqlAuditModuleType.Supplier,
                                                                    x1,
                                                                   null,
                                                                   dbModule
                                                                    )));


                            eventId = eventID;
                        }
                    }
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), suppliers);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        #endregion

        #region Common

        private Response CheckRecordValidForProcess(IList<DomainModel.Supplier> suppliers,
                                                    ValidationType validationType,
                                                    ref IList<DomainModel.Supplier> filteredSuppliers,
                                                    ref IList<DbModel.Supplier> dbSuppliers,
                                                    ref IList<DbModel.Country> dbCountries,
                                                    ref IList<DbModel.County> dbCounties,
                                                    ref IList<DbModel.City> dbCity)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(suppliers, ref filteredSuppliers, ref dbSuppliers, ref dbCountries, ref dbCounties, ref dbCity, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(suppliers, ref filteredSuppliers, ref dbSuppliers, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(suppliers, ref filteredSuppliers, ref dbSuppliers, ref dbCountries, ref dbCounties, ref dbCity, ref validationMessages);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), suppliers);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private IList<DomainModel.Supplier> FilterRecord(IList<DomainModel.Supplier> suppliers,
                                                         ValidationType filterType)
        {
            IList<DomainModel.Supplier> filterSuppliers = null;

            if (filterType == ValidationType.Add)
                filterSuppliers = suppliers?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterSuppliers = suppliers?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterSuppliers = suppliers?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterSuppliers;
        }

        private bool IsValidPayload(IList<DomainModel.Supplier> suppliers,
                                   ValidationType validationType,
                                   ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(suppliers), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Supplier, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }


        private bool IsValidSupplier(IList<DomainModel.Supplier> suppliers,
                                     ref IList<DbModel.Supplier> dbSupplier,
                                     ref IList<ValidationMessage> errorMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (errorMessages == null)
                errorMessages = new List<ValidationMessage>();

            var supplierId = suppliers?.Where(x => x.SupplierId != 0)?.Select(x1 => x1.SupplierId)?.ToList();
            var dbSupplierId = this._repository?.FindBy(x => supplierId.Contains(x.Id))?.ToList();
            var supplierNotExists = suppliers?.Where(x => x.SupplierId != 0)?.Where(x1 => !dbSupplierId.Any(x2 => x2.Id == x1.SupplierId))?.ToList();

            supplierNotExists?.ForEach(x =>
            {
                messages.Add(_messages, x.SupplierName, MessageType.InvalidSupplier, x.SupplierName);
            });

            dbSupplier = dbSupplierId;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.Supplier> suppliers,
                                                 IList<DbModel.Supplier> dbSuppliers,
                                                 ref IList<ValidationMessage> errorMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (errorMessages == null)
                errorMessages = new List<ValidationMessage>();

            var notMatchedRecords = suppliers.Where(x => !dbSuppliers.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.SupplierId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x.SupplierName, MessageType.SupplierUpdatedByOtherUser, x.SupplierName);
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsRecordCanBeDeleted(IList<DomainModel.Supplier> suppliers,
                                          IList<DbModel.Supplier> dbSuppliers,
                                          ref IList<ValidationMessage> errorMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (errorMessages == null)
                errorMessages = new List<ValidationMessage>();

            suppliers.Where(x => dbSuppliers.Any(x1 => x1.Id == x.SupplierId && (x1.Visit.Count > 0 || x1.SupplierPurchaseOrder?.Count > 0))).ToList()
            .ForEach(x =>
            {
                string errorCode = MessageType.SupplierCannotBeDeleted.ToId();
                messages.Add(_messages, x.SupplierName, MessageType.SupplierCannotBeDeleted, x.SupplierName);
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsSupplierExistInDb(IList<int> supplierIds,
                                        IList<DbModel.Supplier> dbSuppliers,
                                        ref IList<int> supplierIdNotExists,
                                        ref IList<ValidationMessage> validationMessages,
                                        params Expression<Func<DbModel.Supplier, object>>[] includes)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbSuppliers == null)
                dbSuppliers = new List<DbModel.Supplier>();

            var validMessages = validationMessages;

            if (supplierIds?.Count > 0)
            {
                supplierIdNotExists = supplierIds.Where(id => !dbSuppliers.Any(x1 => x1.Id == id))
                                     .Select(id => id)
                                     .ToList();

                supplierIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.InvalidSupplier, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }


        

        #endregion


        #endregion
    }
}