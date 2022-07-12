using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.Company.Domain.Interfaces.Validations;
using Evolution.Company.Domain.Models.Companies;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.Logging.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel = Evolution.Company.Domain.Models.Companies;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Linq.Expressions;
using System.Transactions;

namespace Evolution.Company.Core.Services
{
    public class CompanyOfficeService : ICompanyOfficeService
    {
        private readonly ICompanyAddressRepository _repository = null;
        private readonly IAppLogger<CompanyOfficeService> _logger = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly ICityRepository _cityRepository = null;
        private readonly ICountyRepository _countyRepository = null;
        private readonly ICountryRepository _countryRepository = null;
        private readonly JObject _MessageDescriptions = null;
        private readonly IMapper _mapper = null;
        private readonly ICompanyOfficeValidationService _validationService = null;

        public CompanyOfficeService(IMapper mapper,
                                    ICompanyAddressRepository repository,
                                    IAppLogger<CompanyOfficeService> logger,
                                    ICompanyRepository companyRepository,
                                    ICityRepository cityRepository,
                                    ICountyRepository countyRepository,
                                    ICountryRepository countryRepository,
                                    ICompanyOfficeValidationService validationService, JObject messages)
        {
            this._mapper = mapper;
            this._repository = repository;
            this._logger = logger;
            this._cityRepository = cityRepository;
            this._countyRepository = countyRepository;
            this._countryRepository = countryRepository;
            this._companyRepository = companyRepository;
            this._validationService = validationService;
            this._MessageDescriptions = messages;
        }

        #region Public

        public Response GetCompanyAddress(CompanyAddress searchModel)
        {
            IList<DomainModel.CompanyAddress> result = null;
            Exception exception = null;
            try
            {
                //using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                //{
                    result = this._repository.Search(searchModel);
                //    tranScope.Complete();
                //}
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response ModifyCompanyAddress(string companyCode, IList<CompanyAddress> companyAdress, bool commitChange = true, bool returnResultSet = true)
        {
            return this.UpdateCompanyAddress(companyCode, companyAdress, commitChange, returnResultSet);
        }

        public Response DeleteCompanyAddress(string companyCode, IList<CompanyAddress> companyAddress, bool commitChange = true, bool returnResultSet = true)
        {
            return RemoveCompanyAddress(companyCode, companyAddress, commitChange, returnResultSet);
        }

        public Response SaveCompanyAddress(string companyCode, IList<CompanyAddress> companyAddress, bool commitChange, bool returnResultSet = true)
        {
            return this.AddCompanyAddresss(companyCode, companyAddress, commitChange, returnResultSet);
        }

        public bool IsValidCompanyAddress(IList<KeyValuePair<string, string>> companyCodeAndOfficeNames,
                                            ref IList<DbModel.CompanyOffice> dbCompanyOffices,
                                            ref IList<ValidationMessage> messages,
                                            params Expression<Func<DbModel.CompanyOffice, object>>[] includes)
        {
            IList<ValidationMessage> valdMessage = new List<ValidationMessage>();
            if (companyCodeAndOfficeNames?.Count() > 0)
            {
                var companyCodes = companyCodeAndOfficeNames.Select(x => x.Key).Where(x => !string.IsNullOrEmpty(x)).ToList();
                var companyOffNames = companyCodeAndOfficeNames.Select(x => x.Value).Where(x => !string.IsNullOrEmpty(x)).ToList();

                if (dbCompanyOffices == null || dbCompanyOffices?.Count <= 0)
                    dbCompanyOffices = _repository?.FindBy(x => companyCodes.Contains(x.Company.Code) && companyOffNames.Contains(x.OfficeName), includes).ToList();

                foreach (var item in companyCodeAndOfficeNames)
                {
                    var compOffice = dbCompanyOffices?.FirstOrDefault(x => x.Company.Code == item.Key && x.OfficeName.Trim() == item.Value.Trim());
                    if (compOffice == null)
                        valdMessage.Add(_MessageDescriptions, item, MessageType.InvalidCompanyOfficeName, item.Value);
                }
                messages = valdMessage;
            }

            return messages?.Count <= 0;
        }

        #endregion

        #region Private Methods

        private Response UpdateCompanyAddress(string companyCode, IList<CompanyAddress> companyAddress, bool commitChange, bool returnResultSet)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            IList<CompanyAddress> result = null;
            List<DbRepository.Models.SqlDatabaseContext.City> cities = new List<DbRepository.Models.SqlDatabaseContext.City>();
            List<DbRepository.Models.SqlDatabaseContext.County> counties = new List<DbRepository.Models.SqlDatabaseContext.County>();
            List<DbRepository.Models.SqlDatabaseContext.Country> countries = new List<DbRepository.Models.SqlDatabaseContext.Country>();
            List<ValidationMessage> validationMessages = null;
            DbRepository.Models.SqlDatabaseContext.Company dbCompany = null;
            try
            {
                _repository.AutoSave = false;
                validationMessages = new List<ValidationMessage>();
                errorMessages = new List<MessageDetail>();
                if (this.IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
                {
                    IList<CompanyAddress> recordToBeModify = null;
                    //var cities = _cityRepository.FindBy(x => companyAddress.Any(x1 => x1.City == x.Name && x1.State == x.County.Name)).ToList();

                    if (this.IsRecordValidForProcess(companyAddress, ValidationType.Update, ref recordToBeModify, ref errorMessages, ref validationMessages))
                    {
                        if (!this.IsAddressAlreadyAssociatedToCompany(companyCode, recordToBeModify, ValidationType.Update, ref errorMessages))
                        {
                            if (!this.IsAccountReferenceAlreadyAssociatedToOffice(recordToBeModify, dbCompany, ValidationType.Update, ref errorMessages))
                            {
                                IList<DbRepository.Models.SqlDatabaseContext.CompanyOffice> dbCompanyAddress = null;

                                if (IsValidCompanyAddress(companyCode, recordToBeModify, ref dbCompanyAddress, ref errorMessages))
                                {
                                    if (this.IsRecordUpdateCountMatching(recordToBeModify, dbCompanyAddress, ref errorMessages))
                                    {
                                        if (IsValidCountryStateCity(recordToBeModify, ref cities, ref counties, ref countries, ref errorMessages))
                                        {
                                            foreach (var office in dbCompanyAddress)
                                            {
                                                var address = recordToBeModify.FirstOrDefault(x => x.AddressId == office.Id);
                                                office.CompanyId = dbCompany.Id;
                                                office.Id = address.AddressId;
                                                office.OfficeName = address.OfficeName;
                                                office.AccountRef = address.AccountRef;
                                                office.Address = address.FullAddress;
                                                office.CountryId = address.CountryId; //Added for ITK D1536
                                                office.CountyId = address.StateId; //Added for ITK D1536
                                                office.CityId = address.CityId; //Added for ITK D1536
                                                //office.CountryId = countries?.Find(x2 => x2.Name == address.Country)?.Id;
                                                //office.CountyId = counties?.Find(x3 => !string.IsNullOrEmpty(address.State) && x3.Name == address.State)?.Id;
                                                //office.CityId = cities?.Find(x4 => !string.IsNullOrEmpty(address.City) && x4.Name == address.City)?.Id;
                                                office.PostalCode = address.PostalCode;
                                                office.LastModification = DateTime.UtcNow;
                                                office.ModifiedBy = address.ModifiedBy;
                                                office.UpdateCount = address.UpdateCount.CalculateUpdateCount();
                                                _repository.Update(office);
                                            }
                                        }

                                        if (commitChange && !_repository.AutoSave && recordToBeModify?.Count > 0)
                                        {
                                            _repository.ForceSave();
                                            if (returnResultSet)
                                                result = _mapper.Map<IList<DomainModel.CompanyAddress>>(_repository.FindBy(x => recordToBeModify.Select(x1 => x1.CompanyCode).Contains(x.Company.Code)));
                                        }
                                        else if (returnResultSet)
                                            result = recordToBeModify;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyAddress);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, result, exception);
        }

        private Response RemoveCompanyAddress(string companyCode, IList<CompanyAddress> companyAddress, bool commitChange, bool returnResultSet)
        {
            Exception exception = null;
            IList<CompanyAddress> result = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            DbRepository.Models.SqlDatabaseContext.Company dbCompany = null;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();

                if (this.IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
                {
                    IList<CompanyAddress> recordToBeDelete = null;
                    if (this.IsRecordValidForProcess(companyAddress, ValidationType.Delete, ref recordToBeDelete, ref errorMessages, ref validationMessages))
                    {
                        IList<DbRepository.Models.SqlDatabaseContext.CompanyOffice> dbCompanyAddress = null;

                        if (IsValidCompanyAddress(companyCode, recordToBeDelete, ref dbCompanyAddress, ref errorMessages))
                        {
                            if (this.IsRecordCanBeDelete(companyCode, companyAddress, dbCompanyAddress, ref errorMessages))
                            {
                                foreach (var address in dbCompanyAddress)
                                {
                                    _repository.Delete(address);
                                }

                                if (commitChange && !_repository.AutoSave && recordToBeDelete?.Count > 0 && errorMessages.Count <= 0)
                                {
                                    _repository.ForceSave();
                                    if (returnResultSet)
                                        result = _mapper.Map<IList<DomainModel.CompanyAddress>>(_repository.FindBy(x => recordToBeDelete.Select(x1 => x1.CompanyCode).Contains(x.Company.Code)));
                                }
                                else if (returnResultSet)
                                    result = recordToBeDelete;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyAddress);
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response AddCompanyAddresss(string companyCode, IList<CompanyAddress> companyAddress, bool commitChange, bool returnResultSet)
        {
            Exception exception = null;
            IList<CompanyAddress> result = null;
            List<MessageDetail> errorMessages = null;
            List<DbRepository.Models.SqlDatabaseContext.City> cities = new List<DbRepository.Models.SqlDatabaseContext.City>();
            List<DbRepository.Models.SqlDatabaseContext.County> counties = new List<DbRepository.Models.SqlDatabaseContext.County>();
            List<DbRepository.Models.SqlDatabaseContext.Country> countries = new List<DbRepository.Models.SqlDatabaseContext.Country>();
            List<ValidationMessage> validationMessages = null;

            DbRepository.Models.SqlDatabaseContext.Company dbCompany = null;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();

                if (this.IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
                {
                    IList<CompanyAddress> recordToBeInserted = null;
                    if (this.IsRecordValidForProcess(companyAddress, ValidationType.Add, ref recordToBeInserted, ref errorMessages, ref validationMessages))
                    {

                        if (!this.IsAddressAlreadyAssociatedToCompany(companyCode, recordToBeInserted, ValidationType.Add, ref errorMessages))
                        {
                            if (!this.IsAccountReferenceAlreadyAssociatedToOffice(recordToBeInserted, dbCompany, ValidationType.Add, ref errorMessages))
                            {
                                if (IsValidCountryStateCity(recordToBeInserted, ref cities, ref counties, ref countries, ref errorMessages))
                                {
                                    var dbAddressToBeInserted = recordToBeInserted.Select(x => new DbRepository.Models.SqlDatabaseContext.CompanyOffice()
                                    {
                                        CompanyId = dbCompany.Id,
                                        OfficeName = x.OfficeName,
                                        AccountRef = x.AccountRef,
                                        Address = x.FullAddress,
                                        CountryId =  x.CountryId, //Added for ITK D1536
                                        CountyId = x.StateId, //Added for ITK D1536
                                        CityId = x.CityId, //Added for ITK D1536
                                        //CountryId = countries?.Find(x2 => x2.Name == x.Country)?.Id,
                                        //CountyId = counties?.Find(x3 => !string.IsNullOrEmpty(x.State) && x3.Name == x.State)?.Id,
                                        //CityId = cities?.Find(x4 => !string.IsNullOrEmpty(x.City) && x4.Name == x.City)?.Id,
                                        PostalCode = x.PostalCode,
                                        UpdateCount = 0,
                                        ModifiedBy = x.ModifiedBy

                                    }).ToList();

                                    _repository.Add(dbAddressToBeInserted);

                                    if (commitChange && !_repository.AutoSave && recordToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                                    {
                                        _repository.ForceSave();
                                        if (returnResultSet)
                                            result = _mapper.Map<IList<DomainModel.CompanyAddress>>(_repository.FindBy(x => recordToBeInserted.Select(x1 => x1.CompanyCode).Contains(x.Company.Code)));
                                    }
                                    else if (returnResultSet)
                                        result = recordToBeInserted;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyAddress);
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, result, exception);
        }

        private bool IsValidCompany(string companyCode, ref DbRepository.Models.SqlDatabaseContext.Company company, ref List<MessageDetail> errorMessages)
        {
            MessageType messageType = MessageType.Success;

            if (string.IsNullOrEmpty(companyCode))
                messageType = MessageType.InvalidCompanyCode;
            else
            {
                company = _companyRepository.FindBy(x => x.Code == companyCode, new string[] { "CompanyOffice" }).FirstOrDefault();
                if (company == null)
                    messageType = MessageType.InvalidCompanyCode;
            }

            if (messageType != MessageType.Success)
                errorMessages.Add(new MessageDetail(ModuleType.Company, MessageType.InvalidCompanyCode.ToId(), _MessageDescriptions[MessageType.InvalidCompanyCode.ToId()].ToString()));

            return messageType == MessageType.Success;
        }

        private bool IsRecordValidForProcess(IList<CompanyAddress> companyAddress, ValidationType validationType, ref IList<CompanyAddress> filteredOffices, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredOffices = companyAddress?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredOffices = companyAddress?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (validationType == ValidationType.Update)
                filteredOffices = companyAddress?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            return filteredOffices?.Count > 0 ? IsCompanyOfficeHasValidSchema(filteredOffices, validationType, ref validationMessages) : false;
        }

        private bool IsAddressAlreadyAssociatedToCompany(string companyCode, IList<CompanyAddress> companyAddress, ValidationType validationType, ref List<MessageDetail> errorMessages)
        {
            // IList<DbRepository.Models.SqlDatabaseContext.CompanyOffice> addressExists = null;
            IList<string> addressExists = null;
            List<MessageDetail> messages = new List<MessageDetail>();
            var filterExpressions = new List<Expression<Func<DbModel.CompanyOffice, bool>>>();
            Expression<Func<DbModel.CompanyOffice, bool>> predicate = null;
            Expression<Func<DbModel.CompanyOffice, bool>> containsExpression = null;


            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var address = companyAddress.Select(x => new { x.OfficeName, x.AddressId }).ToList();
            if (address?.Count > 0)
            {
                if (validationType == ValidationType.Add)
                {
                    foreach (var addr in address)
                    {
                        containsExpression = a => a.OfficeName == addr.OfficeName;
                        filterExpressions.Add(containsExpression);
                    }
                }
                if (validationType == ValidationType.Update)
                {
                     foreach (var addr in address)
                    {
                        containsExpression = a => a.OfficeName == addr.OfficeName && a.Id != addr.AddressId;
                        filterExpressions.Add(containsExpression);
                    }
                }
                predicate = filterExpressions.CombinePredicates<DbModel.CompanyOffice>(Expression.OrElse);
                containsExpression = a => a.Company.Code == companyCode;
                predicate = containsExpression.CombineWithAndAlso(predicate);
                
                addressExists = _repository.FindBy(predicate).Select(x => x.OfficeName).ToList();
                addressExists?.ToList().ForEach(x =>
                {
                    string errorCode = MessageType.OfficeAddressAlreadyExsists.ToId();
                    messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x)));
                });
            }

            //if (validationType == ValidationType.Add)
            //    addressExists = _repository.FindBy(x => address.Any(x1 => x.OfficeName == x1.OfficeName && x.Company.Code == companyCode)).ToList();

            //if (validationType == ValidationType.Update)
            //    addressExists = _repository.FindBy(x => address.Any(x1 => x.OfficeName == x1.OfficeName && x.Company.Code == companyCode && x.Id != x1.AddressId)).ToList();


            //addressExists?.ToList().ForEach(x =>
            //{
            //    string errorCode = MessageType.OfficeAddressAlreadyExsists.ToId();
            //    messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.OfficeName)));
            //});

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count > 0;
        }

        private bool IsValidCountryStateCity(IList<CompanyAddress> companyAddress, ref List<DbRepository.Models.SqlDatabaseContext.City> cities,
            ref List<DbRepository.Models.SqlDatabaseContext.County> counties, ref List<DbRepository.Models.SqlDatabaseContext.Country> countries,
            ref List<MessageDetail> errorMessages)
        {
            MessageType messageType = MessageType.Success;
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            DbRepository.Models.SqlDatabaseContext.City city = new DbRepository.Models.SqlDatabaseContext.City();
            DbRepository.Models.SqlDatabaseContext.County county = new DbRepository.Models.SqlDatabaseContext.County();
            DbRepository.Models.SqlDatabaseContext.Country country = new DbRepository.Models.SqlDatabaseContext.Country();

            foreach (var x in companyAddress)
            {
                if (!string.IsNullOrEmpty(x.Country) && !string.IsNullOrEmpty(x.State) && !string.IsNullOrEmpty(x.City))
                {
                    city = _cityRepository.FindBy(x1 => x1.Name == x.City && x1.County.Name == x.State
                                               && x1.County.Country.Name == x.Country)?.FirstOrDefault();
                    if (city != null)
                    {
                        cities.Add(city);
                        counties.Add(city.County);
                        countries.Add(city.County.Country);
                    }
                    if (city == null)
                        messageType = MessageType.OfficeCountryStateCityNotExists;
                }
                else if (!string.IsNullOrEmpty(x.Country) && !string.IsNullOrEmpty(x.State) && string.IsNullOrEmpty(x.City))
                {
                    county = _countyRepository.FindBy(x1 => x1.Name == x.State)?.FirstOrDefault();
                    if (county != null)
                    {
                        counties.Add(county);
                        countries.Add(county.Country);
                    }
                    if (county == null)
                        messageType = MessageType.OfficeCountryStateNotExists;
                }
                else if (!string.IsNullOrEmpty(x.Country) && string.IsNullOrEmpty(x.State) && string.IsNullOrEmpty(x.City))
                {
                    country = _countryRepository.FindBy(x1 => x1.Name == x.Country)?.FirstOrDefault();
                    if (country != null)
                        countries.Add(country);
                    if (country == null)
                        messageType = MessageType.OfficeCountryNotExists;
                }

                if (messageType != MessageType.Success)
                    messages.Add(new MessageDetail(ModuleType.Company, messageType.ToId(), _MessageDescriptions[messageType.ToId()].ToString()));
            }

            if (messages.Count > 0)
                errorMessages.AddRange(messages);
            return errorMessages?.Count <= 0;
        }


        //private DbRepository.Models.SqlDatabaseContext.City IsCityValid(string city, string state, ref List<MessageDetail> errorMessages)
        //{
        //    MessageType messageType = MessageType.Success;
        //    var dbCity = _cityRepository.FindBy(x => x.Name == city && x.County.Name == state).FirstOrDefault();
        //    if (dbCity != null)
        //        return dbCity;
        //    else
        //    {
        //        messageType = MessageType.CustAddr_CityIsInvalid;
        //        errorMessages.Add(new MessageDetail(MessageType.CustAddr_CityIsInvalid.ToId(), _MessageDescriptions[MessageType.CustAddr_CityIsInvalid.ToId()].ToString()));
        //        return null;
        //    }
        //}

        private bool IsRecordUpdateCountMatching(IList<CompanyAddress> companyAddress, IList<DbRepository.Models.SqlDatabaseContext.CompanyOffice> dbCompanyAddress, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = companyAddress.Where(x => !dbCompanyAddress.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AddressId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.OfficeAddressHasBeenUpdatedByOther.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.OfficeName)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsRecordCanBeDelete(string companyCode, IList<CompanyAddress> companyAddress, IList<DbRepository.Models.SqlDatabaseContext.CompanyOffice> dbCompanyOffice, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            dbCompanyOffice.Where(x => x.IsAnyCollectionPropertyContainValue()).ToList()
                                .ForEach(x =>
                                {
                                    string errorCode = MessageType.OfficeAddressCannotBeDeleted.ToId();
                                    messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.OfficeName)));
                                });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCompanyAddress(string companyCode, IList<CompanyAddress> companyAddress, ref IList<DbRepository.Models.SqlDatabaseContext.CompanyOffice> dbCompanyAddress, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var dbCompOffice = _repository?.FindBy(x => companyAddress.Select(x1 => x1.AddressId).Contains(x.Id) && x.Company.Code == companyCode).ToList();

            var notMatchedRecords = companyAddress.Where(x => !dbCompOffice.Any(x1 => x1.Id == x.AddressId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.OfficeIdIsInvalidOrNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.AddressId)));
            });

            dbCompanyAddress = dbCompOffice;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsAccountReferenceAlreadyAssociatedToOffice(IList<DomainModel.CompanyAddress> companyOffices, DbRepository.Models.SqlDatabaseContext.Company dbCompany, ValidationType validationType, ref List<MessageDetail> errorMessages)
        {
            List<string> matchingAccountReferences = null;
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            var dbCompanyOffices = dbCompany.CompanyOffice?.ToList();

            if (validationType == ValidationType.Add)
                matchingAccountReferences = companyOffices.Where(x => dbCompanyOffices.Any(x1 => x1.AccountRef == x.AccountRef)).Select(x => x.AccountRef).ToList();
            if (validationType == ValidationType.Update)
                matchingAccountReferences = companyOffices.Where(x => dbCompanyOffices.Any(x1 => x1.AccountRef == x.AccountRef && x1.Id != x.AddressId)).Select(x => x.AccountRef).ToList();

            matchingAccountReferences?.ForEach(x =>
            {
                string errorCode = MessageType.AccountReferenceAlreadyExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);
            return errorMessages?.Count > 0;

        }

        private bool IsCompanyOfficeHasValidSchema(IList<CompanyAddress> companyAddresses, ValidationType validationType, ref List<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            companyAddresses?.ToList().ForEach(x => //Changes for API Null issues
            {
                if (x.OfficeName.Trim() == "")
                    x.OfficeName = "";

                if (x.AccountRef.Trim() == "")
                    x.AccountRef = "";

                if (x.FullAddress.Trim() == "")
                    x.FullAddress = "";
            });

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(companyAddresses), validationType);

            validationResults?.ToList().ForEach(x =>
            {
                messages.Add(new ValidationMessage(x.Code, new List<MessageDetail> { new MessageDetail(ModuleType.Company, x.Code, x.Message) }));
            });
            if (messages.Count > 0)
            {
                validationMessages.AddRange(messages);
            }
            return validationMessages?.Count <= 0;
        }

        #endregion
    }
}
