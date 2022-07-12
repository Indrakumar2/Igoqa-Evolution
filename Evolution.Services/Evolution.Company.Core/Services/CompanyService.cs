using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.Company.Domain.Interfaces.Validations;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Company.Domain.Models.Companies;

namespace Evolution.Company.Core.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository = null;
        private readonly IDataRepository _dataRepository = null;
        private readonly IAppLogger<CompanyService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messageDescriptions = null;
        private readonly ICompanyValidationService _validationService = null;
        private readonly ICountryService _countryService = null;
        private readonly IMongoDocumentService _mongoDocumentService = null;

        public CompanyService(IMapper mapper,
                              ICompanyRepository repository,
                              IDataRepository dataRepository,
                              IAppLogger<CompanyService> logger,
                              ICompanyValidationService validationService,
                              ICountryService countryService,
                              IMongoDocumentService mongoDocumentService,
                              JObject messages)
        {
            this._repository = repository;
            this._logger = logger;
            this._mapper = mapper;
            this._dataRepository = dataRepository;
            this._validationService = validationService;
            this._messageDescriptions = messages;
            this._countryService = countryService;
            this._mongoDocumentService = mongoDocumentService;
        }


        #region Public Exposed Method

        //public async Task<Response> GetCompany(DomainModel.CompanySearch searchModel)
        //{
        //    IList<DomainModel.Company> result = null;
        //    Exception exception = null;
        //    try
        //    {                
        //        result = this._repository.Search(searchModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        exception = ex;
        //        _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
        //    }

        //    return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        //}

        public async Task<Response> GetCompanyAsync(DomainModel.CompanySearch searchModel)
        {
            IList<DomainModel.Company> result = null;
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
                        var compCodes = mongoSearch.Select(x => x.Trim()).ToList();
                        searchModel.CompanyCodes = compCodes;
                        result = FetchCompany(searchModel);
                        if (result?.Count > 0)
                            result = result.Where(x => compCodes.Contains(x.CompanyCode.Trim())).ToList();
                    }
                    else
                        result = new List<DomainModel.Company>();
                }
                else
                {
                    //result = GetCompanyList(searchModel).Result.Populate<IList<DomainModel.Company>>();
                    result = FetchCompany(searchModel);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        private IList<DomainModel.Company> FetchCompany(DomainModel.CompanySearch searchModel)
        {
            IList<DomainModel.Company> result = null;
            using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                result = this._repository.Search(searchModel);
                tranScope.Complete();
            }
            return result;
        }

        public Response GetCompanyList(DomainModel.CompanySearch searchModel)
        {
            IList<DomainModel.Company> result = null;
            Exception exception = null;
            try
            {
                result = _repository.GetCompanyList(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response ModifyCompany(IList<DomainModel.Company> companies, bool commitChange = true)
        {
            IList<DomainModel.CompanyDetail> exsistingCompany = null;
            IList<DbModel.Company> dbCompany = null;
            var result = this.UpdateCompanies(companies, ref exsistingCompany, ref dbCompany, commitChange);
            return result;
        }
        public Response ModifyCompany(IList<DomainModel.Company> companies, ref IList<DomainModel.CompanyDetail> exsistingCompany, ref IList<DbModel.Company> dbCompany, bool commitChange = true)
        {
            var result = this.UpdateCompanies(companies, ref exsistingCompany, ref dbCompany, commitChange);
            return result;
        }
        public Response SaveCompany(IList<DomainModel.Company> companies, bool commitChange = true)
        {
            IList<DbModel.Company> dbCompanies = null;
            return this.AddCompanies(companies, ref dbCompanies, commitChange);
        }

        public Response SaveCompany(IList<DomainModel.Company> companies, ref IList<DbModel.Company> dbCompanies, bool commitChange = true)
        {
            return this.AddCompanies(companies, ref dbCompanies, commitChange);
        }

        public bool IsValidCompany(IList<string> companyCodes,
                                   ref IList<DbModel.Company> dbCompanies,
                                   ref IList<ValidationMessage> messages,
                                   params Expression<Func<DbModel.Company, object>>[] includes)
        {
            IList<ValidationMessage> valdMessage = new List<ValidationMessage>();
            if (companyCodes?.Count() > 0)
            {
                if (dbCompanies == null || dbCompanies?.Count <= 0)
                    dbCompanies = _repository?.FindBy(x => companyCodes.Contains(x.Code), includes).ToList();

                var dbCompany = dbCompanies;
                var companyCodeNotExists = companyCodes?.Where(x => !dbCompany.Any(x2 => x2.Code == x))?.ToList();
                companyCodeNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.InvalidCompanyCodeWithCode.ToId();
                    valdMessage.Add(_messageDescriptions, x, MessageType.InvalidCompanyCodeWithCode, x);
                });

                messages = valdMessage;
            }

            return messages?.Count <= 0;
        }

        public bool IsValidCompany(IList<string> companyCodes,
                                  ref IList<DbModel.Company> dbCompanies,
                                  ref IList<ValidationMessage> messages,
                                  string[] includes)
        {
            IList<ValidationMessage> valdMessage = new List<ValidationMessage>();
            if (companyCodes?.Count() > 0)
            {
                if (dbCompanies == null || dbCompanies?.Count <= 0)
                    dbCompanies = _repository?.FindBy(x => companyCodes.Contains(x.Code), includes).AsNoTracking().ToList();

                var dbCompany = dbCompanies;
                var companyCodeNotExists = companyCodes?.Where(x => !dbCompany.Any(x2 => x2.Code == x))?.ToList();
                companyCodeNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.InvalidCompanyCodeWithCode.ToId();
                    valdMessage.Add(_messageDescriptions, x, MessageType.InvalidCompanyCodeWithCode, x);
                });

                messages = valdMessage;
            }

            return messages?.Count <= 0;
        }

        public bool IsValidCompanyById(IList<int?> companyIds,
                                   ref IList<DbModel.Company> dbCompanies,
                                   ref IList<ValidationMessage> messages,
                                   params Expression<Func<DbModel.Company, object>>[] includes)
        {
            IList<ValidationMessage> valdMessage = new List<ValidationMessage>();
            if (companyIds?.Count() > 0)
            {
                if (dbCompanies == null || dbCompanies?.Count <= 0)
                    dbCompanies = _repository?.FindBy(x => companyIds.Contains(x.Id), includes).ToList();

                var dbCompany = dbCompanies;
                var companyCodeNotExists = companyIds?.Where(x => !dbCompany.Any(x2 => x2.Id == x))?.ToList();
                companyCodeNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.InvalidCompanyCodeWithCode.ToId();
                    valdMessage.Add(_messageDescriptions, x, MessageType.InvalidCompanyCodeWithCode, x);
                });

                messages = valdMessage;
            }

            return messages?.Count <= 0;
        }

        #endregion

        #region Private Exposed Methods

        private Response AddCompanies(IList<DomainModel.Company> companies, ref IList<DbModel.Company> dbCompany, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            IList<DomainModel.Company> result = null;
            IList<ValidationMessage> validationMessages = null;
            //IList<DbModel.Data> dbLogo = null;
            IList<DomainModel.Company> filteredCompanies = companies?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            IList<string> countryNames = filteredCompanies.Where(x => !string.IsNullOrEmpty(x.OperatingCountryName)).Distinct().Select(x => x.OperatingCountryName).ToList();
            IList<DbModel.Country> dbCountries = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                if (companies?.Count > 0)
                {
                    IList<DomainModel.Company> recordToBeInserted = null;
                    if (this.IsRecordValidForProcess(companies, ValidationType.Add, ref recordToBeInserted, ref errorMessages, ref validationMessages))
                    {
                        if (_countryService.IsValidCountryName(countryNames, ref dbCountries, ref validationMessages))
                        {


                            //if (!this.IsValidLogoText(recordToBeInserted, ref dbLogo, ref errorMessages))
                            //{
                            if (!this.IsCompanyAlreadyPresent(recordToBeInserted, ref errorMessages))
                            {
                                GenerateCompanyCode(ref filteredCompanies, ref errorMessages, ref dbCountries);
                                var dbCompanyToBeInserted = _mapper.Map<IList<DomainModel.Company>, IList<DbRepository.Models.SqlDatabaseContext.Company>>(recordToBeInserted);

                                _repository.Add(dbCompanyToBeInserted);
                                if (commitChange && recordToBeInserted?.Count > 0)
                                {
                                    _repository.ForceSave();
                                    result = _mapper.Map<IList<DomainModel.Company>>(_repository.FindBy(x => recordToBeInserted.Select(x1 => x1.CompanyCode).Contains(x.Code)));

                                }
                                else
                                    result = recordToBeInserted;
                                dbCompany = dbCompanyToBeInserted;
                                //}
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companies);
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages.ToList(), result, exception);
        }

        private Response UpdateCompanies(IList<DomainModel.Company> companies, ref IList<DomainModel.CompanyDetail> exsistingCompany, ref IList<DbModel.Company> dbCompany, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            IList<DomainModel.Company> result = null;
            IList<DbModel.Data> dbLogo = null;
            IList<ValidationMessage> validationMessages = null;
            IList<string> countryNames = companies?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList().Where(x => !string.IsNullOrEmpty(x.OperatingCountryName)).Distinct().Select(x => x.OperatingCountryName).ToList();
            IList<DbModel.Country> dbCountries = null;

            try
            {
                _repository.AutoSave = false;
                validationMessages = new List<ValidationMessage>();
                errorMessages = new List<MessageDetail>();
                if (companies?.Count > 0)
                {
                    IList<DomainModel.Company> recordToBeModify = null;
                    if (this.IsRecordValidForProcess(companies, ValidationType.Update, ref recordToBeModify, ref errorMessages, ref validationMessages))
                    {
                        if (_countryService.IsValidCountryName(countryNames, ref dbCountries, ref validationMessages))
                        {
                            var companyCodes = recordToBeModify.Select(x1 => x1.CompanyCode);
                            var existingCompanies = _repository.FindBy(x => companyCodes.Contains(x.Code)).ToList();
                            //     ,new string[]{
                            //         "CompanyDivision",
                            //         "CompanyDivision.Division",
                            //         "CompanyDivision.CompanyDivisionCostCenter",
                            //         "CompanyPayroll",
                            //         "CompanyPayroll.PayrollType",
                            //         "CompanyPayroll.CompanyPayrollPeriod",
                            //         "CompanyExpectedMargin",
                            //         "CompanyExpectedMargin.MarginType",
                            //         "CompanyOffice",
                            //         "CompanyOffice.Country",
                            //         "CompanyOffice.County",
                            //         "CompanyOffice.City",
                            //         "CompanyMessage",
                            //         "CompanyMessage.MessageType",
                            //         "CompanyQualificationType",
                            //         "CompanyQualificationType.QualificationType",
                            //    }

                            exsistingCompany.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.CompanyDetail>(existingCompanies?.FirstOrDefault())));
                            if (!this.IsValidLogoText(recordToBeModify, ref dbLogo, ref errorMessages))
                            {
                                if (this.IsRecordUpdateCountMatching(recordToBeModify, existingCompanies.ToList(), ref errorMessages))
                                {
                                    foreach (var company in existingCompanies)
                                    {
                                        var comp = recordToBeModify.FirstOrDefault(x => x.CompanyCode == company.Code);
                                        company.InvoiceCompanyName = comp.InvoiceName;
                                        company.IsActive = comp.IsActive;
                                        company.NativeCurrency = comp.Currency;
                                        company.SalesTaxDescription = comp.SalesTaxDescription;
                                        company.WithholdingTaxDescription = comp.WithholdingTaxDescription;
                                        company.InterCompanyExpenseAccRef = comp.InterCompanyExpenseAccRef;
                                        company.InterCompanyRoyaltyAccRef = comp.InterCompanyRoyaltyAccRef;
                                        //company.OperatingCountry = dbCountries.FirstOrDefault(x=>x.Name == comp.OperatingCountry)?.Id;  //we can't update Operating Country from Company
                                        company.CompanyMiiwaref = comp.CompanyMiiwaref;
                                        company.IsUseIctms = comp.IsUseIctms;
                                        company.IsFullUse = comp.IsFullUse;
                                        company.GfsCoa = comp.GfsCoa;
                                        company.GfsBu = comp.GfsBu;
                                        company.Region = comp.Region;
                                        company.IsCostOfSalesEmailOverrideAllow = comp.IsCOSEmailOverrideAllow;
                                        company.AverageTshourlycost = comp.AvgTSHourlyCost;
                                        company.VatTaxRegistrationNo = comp.VatTaxRegNo;
                                        company.Euvatprefix = comp.EUVatPrefix;
                                        company.Iaregion = comp.IARegion;
                                        company.CognosNumber = comp.CognosNumber;
                                        company.LastModification = DateTime.UtcNow;
                                        company.UpdateCount = comp.UpdateCount.CalculateUpdateCount();
                                        company.ModifiedBy = comp.ModifiedBy;
                                        company.ResourceOutsideDistance = comp.ResourceOutsideDistance;
                                        company.LogoId = dbLogo?.FirstOrDefault(x => x.Name == comp.LogoText)?.Id;
                                        company.VatregTextWithinEc = comp.VATRegulationTextWithinEC; // CR560
                                        company.VatregTextOutsideEc = comp.VATRegulationTextOutsideEC; // CR560
                                        _repository.Update(company);
                                    }
                                    dbCompany = existingCompanies;
                                    if (commitChange && recordToBeModify?.Count > 0)
                                    {
                                        _repository.ForceSave();
                                        result = _mapper.Map<IList<DomainModel.Company>>(_repository.FindBy(x => recordToBeModify.Select(x1 => x1.CompanyCode).Contains(x.Code)));

                                    }
                                    else
                                        result = recordToBeModify;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companies);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages.ToList(), result, exception);
        }

        private void GenerateCompanyCode(ref IList<DomainModel.Company> companies, ref List<MessageDetail> errorMessages, ref IList<DbModel.Country> countries)
        {
            string companyCode = string.Empty;
            List<MessageDetail> messages = null;

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            foreach (var comp in companies)
            {
                messages = new List<MessageDetail>();
                if (string.IsNullOrEmpty(comp.CompanyName))
                {
                    string errorCode = MessageType.CompanyNameCannotBeNullOrEmpty.ToId();
                    messages.Add(new MessageDetail(ModuleType.Company, errorCode, _messageDescriptions[errorCode].ToString()));
                }
                if (comp.CompanyMiiwaid == null || comp.CompanyMiiwaid <= 0)
                {
                    string errorCode = MessageType.CompanyMiiwaIdCannotBeNullOrZero.ToId();
                    messages.Add(new MessageDetail(ModuleType.Company, errorCode, _messageDescriptions[errorCode].ToString()));
                }

                if (messages.Count == 0)

                    comp.CompanyCode = string.Format("{0}", string.Join("", countries.Where(x => x.Name == comp.OperatingCountryName).Distinct().Select(x => x.Code).FirstOrDefault().ToString(), comp.CompanyMiiwaid?.ToString("D3")));
                else
                    errorMessages.AddRange(messages);

            }
        }

        private bool IsRecordValidForProcess(IList<DomainModel.Company> companies, ValidationType validationType, ref IList<DomainModel.Company> filteredCompanies, ref List<MessageDetail> errorMessages, ref IList<ValidationMessage> validationMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            // IList<string> countryNames = filteredCompanies.Where(x => !string.IsNullOrEmpty(x.OperatingCountry)).Distinct().Select(x => x.OperatingCountry).ToList();
            //IList<DbModel.Country> dbCountries = null;
            if (validationType == ValidationType.Add)
            {
                filteredCompanies = companies?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                // if (_countryService.IsValidCountryName(countryNames, ref dbCountries, ref validationMessages))
                // {
                //   GenerateCompanyCode(ref filteredCompanies, ref errorMessages,ref dbCountries);
                //}
                //else
                //{
                //    string errorCode = MessageType.MasterInvalidCountry.ToId();
                //    messages.Add(new MessageDetail(errorCode, string.Format(_messageDescriptions[errorCode].ToString(), countryNames)));
                //}
            }
            else if (validationType == ValidationType.Update)
            {
                filteredCompanies = companies?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                //if (!_countryService.IsValidCountryName(countryNames, ref dbCountries, ref validationMessages))
                //{
                //    string errorCode = MessageType.MasterInvalidCountry.ToId();
                //   messages.Add(new MessageDetail(errorCode, string.Format(_messageDescriptions[errorCode].ToString(), countryNames)));
                //}
                filteredCompanies.ToList().ForEach(x =>
            {
                if (string.IsNullOrEmpty(x.CompanyCode))
                {
                    string errorCode = MessageType.CompanyCodeCannotBeNullOrEmpty.ToId();
                    messages.Add(new MessageDetail(errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.CompanyName)));
                }
            });
            }

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return filteredCompanies?.Count > 0 ? IsCompanyHasvalidJsonSchema(filteredCompanies, validationType, ref validationMessages) : false;
        }

        private bool IsCompanyAlreadyPresent(IList<DomainModel.Company> companies, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            _repository?.FindBy(x => companies.Select(x1 => x1.CompanyCode).Contains(x.Code))?.ToList().ForEach(x =>
            {
                string errorCode = MessageType.CompanyAlreadyExist.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.Code)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count > 0;
        }

        private bool IsValidLogoText(IList<DomainModel.Company> companies, ref IList<DbModel.Data> dbLogo, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var logoText = companies?.Where(x => !string.IsNullOrEmpty(x.LogoText))?.Select(x1 => x1.LogoText)?.ToList();
            if (logoText.Count > 0)
            {
                var dbLogoText = _dataRepository?.FindBy(x => logoText.Contains(x.Name) && x.MasterDataTypeId == Convert.ToInt32(MasterType.Logo.ToId()))?.ToList();
                if (dbLogoText.Count <= 0)
                {
                    string errorCode = MessageType.CompanyLogoText.ToId();
                    messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_messageDescriptions[errorCode].ToString())));
                }

                dbLogo = dbLogoText;
            }

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count > 0;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.Company> companies, IList<DbRepository.Models.SqlDatabaseContext.Company> dbCompanies, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = companies.Where(x => !dbCompanies.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Code == x.CompanyCode)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.CompanyUpdateCountMismatch.ToId();
                messages.Add(new MessageDetail(errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.CompanyCode)));
            });
            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsCompanyHasvalidJsonSchema(IList<DomainModel.Company> companies, ValidationType validationType, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(companies), validationType);

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
