using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.ExchangeRate;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Evolution.NumberSequence.InfraStructure.Interface;
using Evolution.Security.Domain.Interfaces.Data;
using Evolution.Timesheet.Domain.Interfaces.Data;
using Evolution.Timesheet.Domain.Interfaces.Timesheets;
using Evolution.Timesheet.Domain.Interfaces.Validations;
using Evolution.Timesheet.Domain.Models.Timesheets;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainContractModel = Evolution.Contract.Domain.Models.Contracts;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;

namespace Evolution.Timesheet.Core.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<TimesheetService> _logger = null;
        private readonly ITimesheetRepository _repository = null;
        private readonly ITimesheetValidationService _timesheetValidationService = null;
        private readonly JObject _messages = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly INumberSequenceRepository _numberSequenceRepository = null;
        private readonly IMongoDocumentService _mongoDocumentService = null;
        private readonly IContractExchangeRateService _contractExchangeRateService = null;
        private readonly ICurrencyExchangeRateService _currencyExchangeRateService = null;
        private readonly IModuleRepository _moduleRepository = null;
        private readonly IMasterRepository _masterRepository = null;
        private readonly AppEnvVariableBaseModel _environment = null;

        public TimesheetService(IAppLogger<TimesheetService> logger,
                                ITimesheetRepository timesheetRepository,
                                ITimesheetValidationService timesheetValidationService,
                                IMapper mapper,
                                JObject messages,
                                IAuditSearchService auditSearchService,
                                IMongoDocumentService mongoDocumentService,
                                INumberSequenceRepository numberSequenceRepository,
                                IContractExchangeRateService contractExchangeRateService,
                                ICurrencyExchangeRateService currencyExchangeRateService,
                                IModuleRepository moduleRepository,
                                IMasterRepository masterRepository,
                                IOptions<AppEnvVariableBaseModel> environment)
        {
            _mongoDocumentService = mongoDocumentService;
            _mapper = mapper;
            _logger = logger;
            _repository = timesheetRepository;
            _timesheetValidationService = timesheetValidationService;
            this._messages = messages;
            _auditSearchService = auditSearchService;
            _numberSequenceRepository = numberSequenceRepository;
            _contractExchangeRateService = contractExchangeRateService;
            _currencyExchangeRateService = currencyExchangeRateService;
            _moduleRepository = moduleRepository;
            _masterRepository = masterRepository;
            _environment = environment.Value;
        }

        #region Get
        public Response GetTimesheets(DomainModel.BaseTimesheet searchModel, AdditionalFilter filter = null)
        {
            IList<DomainModel.BaseTimesheet> result = null;
            Int32? count = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                              new TransactionOptions
                                              {
                                                  IsolationLevel = IsolationLevel.ReadUncommitted
                                              }))
                {
                    if (filter != null && !filter.IsRecordCountOnly)
                        result = this._repository.Search(searchModel);
                    else
                        count = this._repository.GetCount(searchModel);

                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, count);
        }

        public Response GetTimesheetData(DomainModel.BaseTimesheet searchModel)
        {
            IList<DomainModel.BaseTimesheet> result = null;
            Exception exception = null;
            try
            {
                result = this._repository.GetTimesheet(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result.Count);
        }

        public async Task<Response> GetTimesheet(DomainModel.TimesheetSearch searchModel)
        {
            IList<DomainModel.TimesheetSearch> result = null;
            Result export = null;
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            IList<string> mongoSearch = null;
            try
            {
                //Mongo Doc Search
                if (!string.IsNullOrEmpty(searchModel.SearchDocumentType) || !string.IsNullOrEmpty(searchModel.DocumentSearchText))
                {
                    var evoMongoDocSearch = _mapper.Map<Document.Domain.Models.Document.EvolutionMongoDocumentSearch>(searchModel);
                    mongoSearch = await this._mongoDocumentService.SearchDistinctFieldAsync(evoMongoDocSearch);
                    if (mongoSearch != null && mongoSearch.Count > 0)
                    {
                        var ids = mongoSearch.Where(x => !string.IsNullOrEmpty(x) && x != "0" && x != "null" && x != "undefined").Distinct().ToList();
                        searchModel.TimesheetIds = ids.Select(x => Convert.ToInt64(x)).ToList();
                        if (searchModel.IsExport == true)
                            export = SearchTimesheets(searchModel, _environment.ChunkSize);
                        else
                            result = SearchTimesheets(searchModel, _environment.TimesheetRecordSize)?.TimesheetSearch;
                    }
                    else
                        result = new List<DomainModel.TimesheetSearch>();
                }
                else
                {
                    if (searchModel.IsExport == true)
                        export = SearchTimesheets(searchModel, _environment.ChunkSize);
                    else
                        result = SearchTimesheets(searchModel, _environment.TimesheetRecordSize)?.TimesheetSearch;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            if (searchModel.IsExport == true)
                return new Response().ToPopulate(responseType, null, null, null, export, exception, export?.TimesheetSearch?.FirstOrDefault()?.TotalCount);
            else
                return new Response().ToPopulate(responseType, null, null, null, result, exception, result?.FirstOrDefault()?.TotalCount);
        }

        /*This is used for Document Approval Dropdown binding*/
        public Response GetTimesheetForDocumentApproval(DomainModel.BaseTimesheet searchModel)
        {
            IList<DomainModel.BaseTimesheet> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = _repository.GetTimesheetForDocumentApproval(searchModel)?.Select(x => new DomainModel.BaseTimesheet
                    {
                        TimesheetDescription = x.TimesheetDescription,
                        TimesheetNumber = x.TimesheetNumber,
                        TimesheetId = x.Id,
                        DocumentApprovalVisitValue = string.IsNullOrEmpty(x.TimesheetDescription) ? Convert.ToString(x.Id) : x.TimesheetDescription
                    })?.ToList();
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        private Result SearchTimesheets(DomainModel.TimesheetSearch searchModel, int fetchCount)
        {
            Result result = null;
            using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                               new TransactionOptions
                                               {
                                                   IsolationLevel = IsolationLevel.ReadUncommitted
                                               }))
            {
                searchModel.FetchCount = fetchCount;
                result = this._repository.SearchTimesheets(searchModel);
                tranScope.Complete();
            }
            return result;
        }

        public Response Get(DomainModel.Timesheet searchModel)
        {
            IList<DomainModel.Timesheet> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                              new TransactionOptions
                                              {
                                                  IsolationLevel = IsolationLevel.ReadUncommitted
                                              }))
                {
                    result = _repository.Get(searchModel);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, null);
        }
        #endregion

        #region Add Skeleton Timesheet
        public Response AddSkeletonTimesheet(DbModel.Timesheet dbTimesheet, ref DbModel.Timesheet dbSavedTimesheet, bool commitChange)
        {
            Exception exception = null;
            try
            {
                if (dbTimesheet != null)
                    _repository.Add(dbTimesheet);

                if (commitChange)
                    _repository.ForceSave();
                dbSavedTimesheet = dbTimesheet;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), dbTimesheet);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }
        #endregion

        #region Add
        public Response Add(IList<DomainModel.Timesheet> timesheets,
                            ref long? eventId,
                            bool commitChange = true,
                            bool isValidationRequire = true,
                            bool isProcessNumberSequence = false)
        {
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            return AddTimesheet(timesheets,
                                ref dbTimesheet,
                                ref dbAssignment,
                                dbModule,
                                ref eventId,
                                commitChange,
                                isValidationRequire,
                                isProcessNumberSequence);
        }

        public Response Add(IList<DomainModel.Timesheet> timesheets,
                           ref IList<DbModel.Timesheet> dbTimesheet,
                           ref IList<DbModel.Assignment> dbAssignment,
                           IList<DbModel.SqlauditModule> dbModule,
                           ref long? eventId,
                           bool commitChange = true,
                           bool isValidationRequire = true,
                           bool isProcessNumberSequence = false)
        {
            return AddTimesheet(timesheets,
                                ref dbTimesheet,
                                ref dbAssignment,
                                dbModule,
                                ref eventId,
                                commitChange,
                                isValidationRequire,
                                isProcessNumberSequence);
        }
        #endregion

        #region Modify
        public Response Modify(IList<DomainModel.Timesheet> timesheets,
                             ref long? eventId,
                            bool commitChange = true,
                            bool isValidationRequire = true)
        {
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            return UpdateTimesheet(timesheets,
                                    ref dbTimesheet,
                                    ref dbAssignment,
                                    dbModule,
                                    ref eventId,
                                    commitChange,
                                    isValidationRequire);

        }



        public Response Modify(IList<DomainModel.Timesheet> timesheets,
                           ref IList<DbModel.Timesheet> dbTimesheet,
                           ref IList<DbModel.Assignment> dbAssignment,
                           IList<DbModel.SqlauditModule> dbModule,
                           ref long? eventId,
                           bool commitChange = true,
                           bool isValidationRequire = true)
        {
            return UpdateTimesheet(timesheets,
                                    ref dbTimesheet,
                                    ref dbAssignment,
                                    dbModule,
                                    ref eventId,
                                    commitChange,
                                    isValidationRequire);
        }

        #endregion

        #region Delete


        public Response Delete(IList<DomainModel.Timesheet> timesheets,
                               ref long? eventId,
                               bool commitChange = true,
                               bool isValidationRequire = true)
        {
            IList<DbModel.SqlauditModule> dbModule = null;
            return this.RemoveTimesheet(timesheets,
                                        dbModule,
                                        ref eventId,
                                        commitChange,
                                        isValidationRequire);
        }


        public Response Delete(IList<DomainModel.Timesheet> timesheets,
                               IList<DbModel.SqlauditModule> dbModule,
                               ref long? eventId,
                               bool commitChange = true,
                               bool isValidationRequire = true)
        {
            return this.RemoveTimesheet(timesheets,
                                        dbModule,
                                        ref eventId,
                                        commitChange,
                                        isValidationRequire);
        }



        #endregion

        #region Validation

        public Response IsRecordValidForProcess(IList<DomainModel.Timesheet> timesheets,
                                                ValidationType validationType)
        {
            IList<DbModel.Timesheet> dbTimesheets = null;
            IList<DbModel.Assignment> dbAssignments = null;
            return IsRecordValidForProcess(timesheets,
                                           validationType,
                                           ref dbTimesheets,
                                           ref dbAssignments);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.Timesheet> timesheets,
                                                ValidationType validationType,
                                                ref IList<DbModel.Timesheet> dbTimesheets,
                                                ref IList<DbModel.Assignment> dbAssignments)
        {
            IList<DomainModel.Timesheet> filteredTimesheets = null;
            return IsRecordValidForProcess(timesheets,
                                           validationType,
                                           ref filteredTimesheets,
                                           ref dbTimesheets,
                                           ref dbAssignments);
        }



        public Response IsRecordValidForProcess(IList<DomainModel.Timesheet> timesheets,
                                                ValidationType validationType,
                                                IList<DbModel.Timesheet> dbTimesheets,
                                                IList<DbModel.Assignment> dbAssignments)
        {
            return IsRecordValidForProcess(timesheets,
                                             validationType,
                                             ref dbTimesheets,
                                             ref dbAssignments);
        }

        public bool IsValidTimesheet(IList<long> timesheetId,
                                    ref IList<DbModel.Timesheet> dbTimesheets,
                                    ref IList<ValidationMessage> messages,
                                    params string[] includes)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (dbTimesheets == null)
            {
                var dbTimesheet = _repository?.FindBy(x => timesheetId.Contains(x.Id), includes)?.ToList();
                var timesheetNotExists = timesheetId?.Where(x => !dbTimesheet.Any(x2 => x2.Id == x))?.ToList();
                timesheetNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.TimesheetNotExists.ToId();
                    message.Add(_messages, x, MessageType.TimesheetNotExists, x);
                });
                dbTimesheets = dbTimesheet;
                messages = message;
            }

            return messages?.Count <= 0;
        }

        public bool IsValidTimesheetData(IList<long> timesheetId,
                                  ref IList<DbModel.Timesheet> dbTimesheets,
                                  ref IList<ValidationMessage> messages)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (dbTimesheets == null)
            {
                var dbTimesheet = _repository.Get(x => timesheetId.Contains(x.Id), x => new DbModel.Timesheet { Id = x.Id })?.ToList();
                var timesheetNotExists = timesheetId?.Where(x => !dbTimesheet.Any(x2 => x2.Id == x))?.ToList();
                timesheetNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.TimesheetNotExists.ToId();
                    message.Add(_messages, x, MessageType.TimesheetNotExists, x);
                });
                dbTimesheets = dbTimesheet;
                messages = message;
            }

            return messages?.Count <= 0;
        }

        public Response GetTimesheetValidationData(DomainModel.BaseTimesheet searchModel)
        {
            DomainModel.TimesheetValidationData timesheetValidationData = new DomainModel.TimesheetValidationData();
            Exception exception = null;
            try
            {
                timesheetValidationData = _repository.GetTimesheetValidationData(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, timesheetValidationData, exception);
        }

        #endregion

        public void AddNumberSequence(DbModel.NumberSequence data, int parentId, int parentRefId, int assignmentId, ref List<DbModel.NumberSequence> numberSequence)
        {
            NumberSequence(data, parentId, parentRefId, assignmentId, ref numberSequence);
        }

        #region Private methods

        private Response IsRecordValidForProcess(IList<DomainModel.Timesheet> timesheets,
                                                 ValidationType validationType,
                                                 ref IList<DomainModel.Timesheet> filteredTimesheets,
                                                 ref IList<DbModel.Timesheet> dbTimesheets,
                                                 ref IList<DbModel.Assignment> dbAssignments)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> messages = null;
            try
            {
                if (filteredTimesheets == null || filteredTimesheets.Count <= 0)
                    filteredTimesheets = FilterRecord(timesheets, validationType);

                if (filteredTimesheets != null && filteredTimesheets.Count > 0)
                {
                    if (messages == null)
                        messages = new List<ValidationMessage>();
                    result = IsValidPayload(filteredTimesheets, validationType, ref messages);

                    if (result)
                    {
                        List<long> timesheetNotExists = null;
                        var timesheetIds = filteredTimesheets.Where(x => x.TimesheetId > 0).Select(x => (long)x.TimesheetId).Distinct().ToList();
                        var assignmentIds = filteredTimesheets.Where(x => x.TimesheetAssignmentId > 0).Select(x => x.TimesheetAssignmentId).Distinct().ToList();
                        if ((dbTimesheets == null || dbTimesheets?.Count <= 0) && validationType != ValidationType.Add)
                            dbTimesheets = GetData(filteredTimesheets, "Assignment");

                        if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                        {
                            result = IsTimesheetExistInDb(timesheetIds,
                                                           dbTimesheets,
                                                           ref timesheetNotExists,
                                                           ref messages);

                            if (result && validationType == ValidationType.Delete)
                                result = IsTimesheetCanBeRemove(dbTimesheets, ref messages);

                            else if (result && validationType == ValidationType.Update)
                                result = IsRecordValidForUpdate(filteredTimesheets,
                                                                dbTimesheets,
                                                                ref dbAssignments,
                                                                ref messages);
                        }
                        else if (validationType == ValidationType.Add)
                            result = IsRecordValidForAdd(filteredTimesheets,
                                                         ref dbAssignments,
                                                         ref messages);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), filteredTimesheets);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, messages?.ToList(), result, exception);
        }


        public Response GetExpenseLineItemChargeExchangeRates(IList<ExchangeRate> models, string ContractNumber)
        {
            List<ExchangeRate> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                var currenciesValueNeedToFetch = models?.Where(x => x.CurrencyFrom != x.CurrencyTo).ToList();

                if (currenciesValueNeedToFetch != null && currenciesValueNeedToFetch?.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ContractNumber))
                    {
                        var contractExchangeRateSearchModel = new DomainContractModel.ContractExchangeRate
                        {
                            ContractNumber = ContractNumber
                        };
                        //Commented the code because of IGO QC Defect 894 - If not an issue need to revert
                        var listContractExchangeRate = this._contractExchangeRateService
                                                       .GetContractExchangeRate(contractExchangeRateSearchModel)
                                                       .Result
                                                       .Populate<List<DomainContractModel.ContractExchangeRate>>();
                        if (listContractExchangeRate != null && listContractExchangeRate.Count > 0)
                        {
                            foreach (var currencyVal in currenciesValueNeedToFetch)
                            {
                                DateTime? MaxEffDate = null;
                                var MaxEffDateQuery = listContractExchangeRate.Where(cer => cer.FromCurrency == currencyVal.CurrencyFrom
                                                                      && cer.ToCurrency == currencyVal.CurrencyTo
                                                                      && cer.EffectiveFrom <= currencyVal.EffectiveDate)?
                                                                      .Select(x => x);
                                if (MaxEffDateQuery.Any())
                                {
                                    MaxEffDate = MaxEffDateQuery.Max(x => x.EffectiveFrom);
                                }
                                if (MaxEffDate != null)
                                {
                                    currencyVal.Rate = Convert.ToDecimal(listContractExchangeRate.Where(cer => cer.FromCurrency == currencyVal.CurrencyFrom
                                                                       && cer.ToCurrency == currencyVal.CurrencyTo
                                                                       && cer.EffectiveFrom == MaxEffDate)
                                                                    .FirstOrDefault(x => x.EffectiveFrom == MaxEffDate).ExchangeRate);
                                }
                            }
                            var exchangeCurrenciesWithRates = currenciesValueNeedToFetch.Where(r => r.Rate != 0).ToList();
                            if (exchangeCurrenciesWithRates != null && exchangeCurrenciesWithRates.Count > 0)
                            {
                                result = result ?? new List<ExchangeRate>();
                                result.AddRange(exchangeCurrenciesWithRates);
                            }
                        }
                    }
                    var exchangeCurrenciesWithOutRates = currenciesValueNeedToFetch.Where(r => r.Rate == 0).ToList();
                    var fetchResult = this._currencyExchangeRateService
                                            .GetMiiwaExchangeRates(exchangeCurrenciesWithOutRates)
                                            .Result
                                            .Populate<List<ExchangeRate>>();

                    if (fetchResult != null && fetchResult.Count > 0)
                    {
                        result = result ?? new List<ExchangeRate>();
                        result.AddRange(fetchResult);
                    }
                }

                var sameCurrencies = models?.Where(x => x.CurrencyFrom == x.CurrencyTo).ToList();
                sameCurrencies.ForEach(x1 => { x1.Rate = 1; });

                result = result ?? new List<ExchangeRate>();

                if (sameCurrencies?.Count > 0)
                    result.AddRange(sameCurrencies);
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);
        }

        private Response AddTimesheet(IList<DomainModel.Timesheet> timesheets,
                                     ref IList<DbModel.Timesheet> dbTimesheets,
                                     ref IList<DbModel.Assignment> dbAssignments,
                                     IList<DbModel.SqlauditModule> dbModule,
                                     ref long? eventId,
                                     bool commitChange,
                                     bool isValidationRequire,
                                      bool isProcessNumberSequence)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.Assignment> dbAssignment = null;
            List<DbModel.NumberSequence> numberSequenceToAdd = null;
            List<DbModel.NumberSequence> numberSequenceToUpdate = null;
            long? eventID = 0;
            try
            {
                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(timesheets, ValidationType.Add);
                if (isValidationRequire)
                    valdResponse = IsRecordValidForProcess(timesheets,
                                                           ValidationType.Add,
                                                           ref dbTimesheets,
                                                           ref dbAssignments);

                if (!isValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    _repository.AutoSave = false;
                    dbAssignment = dbAssignments;
                    if (isProcessNumberSequence)
                    {
                        _numberSequenceRepository.AutoSave = false;
                        numberSequenceToAdd = new List<DbModel.NumberSequence>();
                        numberSequenceToUpdate = new List<DbModel.NumberSequence>();

                        recordToBeAdd.ToList()?.ForEach(x =>
                        {
                            x.TimesheetNumber = ProcessNumberSequence(x.TimesheetAssignmentId,
                            ref numberSequenceToAdd, ref numberSequenceToUpdate);
                        });
                    }

                    var dbRecordToBeInserted = _mapper.Map<IList<DbModel.Timesheet>>(recordToBeAdd, opt =>
                                                                                   {
                                                                                       opt.Items["Assignment"] = dbAssignment;
                                                                                       opt.Items["isTimeId"] = false;
                                                                                       opt.Items["isTimesheetNumber"] = true;
                                                                                   });
                    _repository.Add(dbRecordToBeInserted);
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        if (isProcessNumberSequence)
                        {
                            if (numberSequenceToAdd.Count > 0)
                                _numberSequenceRepository.Add(numberSequenceToAdd);
                            if (numberSequenceToUpdate.Count > 0)
                                _numberSequenceRepository.Update(numberSequenceToUpdate);
                            _numberSequenceRepository.ForceSave();
                        }
                        if (dbRecordToBeInserted?.Count > 0 && recordToBeAdd?.Count > 0 && value > 0)
                        {
                            int? timesheetNumber = _numberSequenceRepository.FindBy(x => x.ModuleData == timesheets.FirstOrDefault().TimesheetAssignmentId && x.ModuleRefId == 18 && x.ModuleId == 5)?.FirstOrDefault()?.LastSequenceNumber;
                            dbRecordToBeInserted?.ToList().ForEach(x =>
                             recordToBeAdd?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventID, x1.ActionByUser,
                                                                                                "{" + AuditSelectType.Id + ":" + x.Id + "}${" +
                                                                                                AuditSelectType.TimesheetDescription + ":" + x.TimesheetDescription?.Trim() + "}${" +
                                                                                                AuditSelectType.JobReferenceNumber + ":" + x.Assignment.Project.ProjectNumber + "-" + x.Assignment.AssignmentNumber + "-" + timesheetNumber + "}${" +
                                                                                                AuditSelectType.ProjectAssignment + ":" + x.Assignment.Project.ProjectNumber + "-" + x.Assignment.AssignmentNumber + "}",
                                                                                                ValidationType.Add.ToAuditActionType(),
                                                                                                SqlAuditModuleType.Timesheet,
                                                                                                null,
                                                                                               _mapper.Map<DomainModel.Timesheet>(x1),
                                                                                               dbModule
                                                                                               )));
                            eventId = eventID;
                        }
                    }

                    dbTimesheets = dbRecordToBeInserted;
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheets);
            }
            finally
            {
                _repository.AutoSave = true;
                numberSequenceToAdd = null;
                dbAssignment = null;
                numberSequenceToUpdate = null;
                _numberSequenceRepository.AutoSave = true;
            }

            return new Response().ToPopulate(responseType, null, null, validationMessages?.ToList(), dbTimesheets, exception);
        }

        private void NumberSequence(DbModel.NumberSequence data,
                                    int parentId,
                                    int parentRefId,
                                    int assignmentId,
                                    ref List<DbModel.NumberSequence> numberSequence)
        {
            if (numberSequence == null)
                numberSequence = new List<DbModel.NumberSequence>();
            if (data != null)
            {
                data.LastSequenceNumber = data.LastSequenceNumber + 1;
                numberSequence.Add(data);
            }
            else
                if (parentId > 0 && parentRefId > 0)
                numberSequence.Add(new DbModel.NumberSequence() { ModuleId = parentId, ModuleRefId = parentRefId, ModuleData = assignmentId, LastSequenceNumber = 1 });
        }

        private Response UpdateTimesheet(IList<DomainModel.Timesheet> timesheets,
                                         ref IList<DbModel.Timesheet> dbTimesheets,
                                         ref IList<DbModel.Assignment> dbAssignments,
                                         IList<DbModel.SqlauditModule> dbModule,
                                         ref long? eventId,
                                         bool commitChange,
                                         bool isValidationRequire)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.Assignment> dbAssignment = null;
            long? eventID = 0;
            try
            {
                bool isAuditUpdate = (timesheets != null && timesheets.Count > 0 ? (timesheets[0].IsAuditUpdate == true ? true : false) : false);
                var recordToBeModify = FilterRecord(timesheets, ValidationType.Update);
                Response valdResponse = null;

                if (isValidationRequire)
                    valdResponse = IsRecordValidForProcess(timesheets,
                                                           ValidationType.Update,
                                                           ref recordToBeModify,
                                                           ref dbTimesheets,
                                                           ref dbAssignments);

                if (!isValidationRequire || (Convert.ToBoolean(valdResponse.Result) && dbTimesheets?.Count > 0))
                {
                    IList<DomainModel.Timesheet> domExistingTimesheet = new List<DomainModel.Timesheet>();
                    dbAssignment = dbAssignments;
                    dbTimesheets?.ToList().ForEach(x =>
                    {
                        domExistingTimesheet.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.Timesheet>(x)));
                    });
                    dbTimesheets.ToList().ForEach(timesheet =>
                    {
                        var timesheetToBeModify = recordToBeModify.FirstOrDefault(x => x.TimesheetId == timesheet.Id);
                        _mapper.Map(timesheetToBeModify, timesheet, opt =>
                        {
                            opt.Items["isTimeId"] = true;
                            opt.Items["isTimesheetNumber"] = false;
                            opt.Items["Assignment"] = dbAssignment;
                        });
                        timesheet.LastModification = DateTime.UtcNow;
                        timesheet.UpdateCount = timesheetToBeModify.UpdateCount.CalculateUpdateCount();
                        timesheet.ModifiedBy = timesheetToBeModify.ModifiedBy;
                    });

                    _repository.AutoSave = false;
                    _repository.Update(dbTimesheets);
                    if (commitChange && isAuditUpdate)
                    {
                        int value = _repository.ForceSave();
                        if (dbTimesheets?.Count > 0 && recordToBeModify?.Count > 0 && value > 0)
                        {
                            dbTimesheets?.ToList().ForEach(x => recordToBeModify?.ToList().ForEach(x1 =>
                            {
                                string result = string.Empty;
                                string olddata = JsonConvert.SerializeObject(_mapper.Map<DomainModel.Timesheet>(domExistingTimesheet?.FirstOrDefault(x2 => x2.TimesheetId == x1.TimesheetId)));
                                string newdata = JsonConvert.SerializeObject(x1);
                                result = CompareJson.CompareVisitObject(olddata, newdata, new List<string>() { "TechSpecialists", "RecordStatus", "ModifiedBy", "ActionByUser" });

                                if (result != "{}" && result?.Length > 0)
                                    _auditSearchService.AuditLog(x1, ref eventID, x1.ActionByUser,
                                                                                                    "{" + AuditSelectType.Id + ":" + x.Id + "}${" +
                                                                                                    AuditSelectType.TimesheetDescription + ":" + x.TimesheetDescription?.Trim() + "}${" +
                                                                                                    AuditSelectType.JobReferenceNumber + ":" + x.Assignment.Project.ProjectNumber + "-" + x.Assignment.AssignmentNumber + "-" + x.TimesheetNumber + "}${" +
                                                                                                    AuditSelectType.ProjectAssignment + ":" + x.Assignment.Project.ProjectNumber + "-" + x.Assignment.AssignmentNumber + "}",
                                                                                                    ValidationType.Update.ToAuditActionType(),
                                                                                                    SqlAuditModuleType.Timesheet,
                                                                                                    _mapper.Map<DomainModel.Timesheet>(domExistingTimesheet?.FirstOrDefault(x2 => x2.TimesheetId == x1.TimesheetId)),
                                                                                                    x1, dbModule
                                                                                                   );
                            }
                            ));
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheets);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response RemoveTimesheet(IList<DomainModel.Timesheet> timesheets,
                                         IList<DbModel.SqlauditModule> dbModule,
                                         ref long? eventId,
                                         bool commitChange,
                                         bool isValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.Timesheet> dbTimesheets = null;
            IList<DbModel.Assignment> dbAssignments = null;
            long? eventID = 0;

            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                var recordToDelete = FilterRecord(timesheets, ValidationType.Delete);
                eventId = timesheets?.FirstOrDefault()?.EventId;

                Response response = null;
                if (isValidationRequire)
                    response = IsRecordValidForProcess(timesheets,
                                                       ValidationType.Delete,
                                                       ref recordToDelete,
                                                       ref dbTimesheets,
                                                       ref dbAssignments);

                if (!isValidationRequire || (Convert.ToBoolean(response.Code == ResponseType.Success.ToId()) && dbTimesheets?.Count > 0))
                {
                    _repository.AutoSave = false;
                    _repository.Delete(dbTimesheets);
                    if (commitChange)
                    {
                        _repository.ForceSave();
                        if (dbTimesheets?.Count > 0 && timesheets?.Count > 0)
                        {
                            var dbAssignment = dbAssignments?.FirstOrDefault();
                            dbTimesheets?.ToList().ForEach(x =>
                            timesheets?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventID, x1.ActionByUser,
                                                                     "{" + AuditSelectType.Id + ":" + x.Id + "}${" +
                                                                      AuditSelectType.TimesheetDescription + ":" + x.TimesheetDescription?.Trim() + "}${" +
                                                                      AuditSelectType.JobReferenceNumber + ":" + x.Assignment.Project.ProjectNumber + "-" + x.Assignment.AssignmentNumber + "-" + x.TimesheetNumber + "}${" +
                                                                      AuditSelectType.ProjectAssignment + ":" + x.Assignment.Project.ProjectNumber + "-" + x.Assignment.AssignmentNumber + "}",
                                                                      ValidationType.Delete.ToAuditActionType(),
                                                                      SqlAuditModuleType.Timesheet,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheets);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private IList<DomainModel.Timesheet> FilterRecord(IList<DomainModel.Timesheet> timesheets,
                                                           ValidationType filterType)
        {
            IList<DomainModel.Timesheet> filteredModules = null;

            if (filterType == ValidationType.Add)
                filteredModules = timesheets?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredModules = timesheets?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredModules = timesheets?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredModules;
        }

        private IList<DbModel.Timesheet> GetData(IList<DomainModel.Timesheet> timesheets,
                                                        params string[] includes)
        {
            IList<DbModel.Timesheet> dbTimesheets = null;
            if (timesheets?.Count > 0)
            {
                var timesheetId = timesheets.Where(x => x.TimesheetId > 0).Select(x => x.TimesheetId).Distinct().ToList();

                if (timesheetId?.Count > 0)
                    dbTimesheets = _repository.FindBy(x => timesheetId.Contains(x.Id), includes).ToList();
            }
            return dbTimesheets;
        }

        private bool IsValidPayload(IList<DomainModel.Timesheet> timesheets,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> messages)
        {
            if (messages == null)
                messages = new List<ValidationMessage>();

            var validationResults = _timesheetValidationService.Validate(JsonConvert.SerializeObject(timesheets), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Timesheet, validationResults);

            return messages?.Count <= 0;
        }


        private bool IsRecordValidForAdd(IList<DomainModel.Timesheet> filteredData,
                                         ref IList<DbModel.Assignment> dbAssignments,
                                         ref IList<ValidationMessage> messages)
        {
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (messages == null)
                messages = new List<ValidationMessage>();

            var assignmentIds = filteredData.Where(x => x.TimesheetAssignmentId > 0).Select(x1 => x1.TimesheetAssignmentId).Distinct().ToList();
            if (messages?.Count <= 0)
            {
                //TO-DO Prathap: Testing for 500 issue fixes and remove   _assignmentService.IsValidAssignment
                //_assignmentService.IsValidAssignment(assignmentIds, ref dbAssignments, ref messages);
                IsValidAssignment(assignmentIds, ref dbAssignments, ref messages);
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;

        }

        private bool IsRecordValidForUpdate(IList<DomainModel.Timesheet> filteredData,
                                           IList<DbModel.Timesheet> dbTimesheets,
                                           ref IList<DbModel.Assignment> dbAssignments,
                                           ref IList<ValidationMessage> messages)
        {
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (messages == null)
                messages = new List<ValidationMessage>();

            var assignmentIds = filteredData.Where(x => x.TimesheetAssignmentId > 0).Select(x1 => x1.TimesheetAssignmentId).Distinct().ToList();
            if (messages?.Count <= 0)
                if (IsRecordUpdateCountMatching(filteredData, dbTimesheets, ref messages))
                {
                    //TO-DO Prathap: Testing for 500 issue fixes and remove   _assignmentService.IsValidAssignment
                    //_assignmentService.IsValidAssignment(assignmentIds, ref dbAssignments, ref messages);
                    IsValidAssignment(assignmentIds, ref dbAssignments, ref messages);
                }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsTimesheetExistInDb(List<long> timesheetIds,
                                           IList<DbModel.Timesheet> dbTimesheets,
                                           ref List<long> timesheetNotExists,
                                           ref IList<ValidationMessage> messages)
        {
            if (messages == null)
                messages = new List<ValidationMessage>();

            if (dbTimesheets == null)
                dbTimesheets = new List<DbModel.Timesheet>();

            var validMessages = messages;
            if (timesheetIds?.Count > 0)
            {
                timesheetNotExists = timesheetIds.Where(x => !dbTimesheets.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                timesheetNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TimesheetNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                messages = validMessages;

            return messages.Count <= 0;
        }


        private bool IsTimesheetCanBeRemove(IList<DbModel.Timesheet> dbTimesheets,
                                           ref IList<ValidationMessage> messages)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (messages == null)
                messages = new List<ValidationMessage>();

            dbTimesheets.ToList().ForEach(x =>
            {
                bool result = x.IsAnyCollectionPropertyContainValue();
                if (result)
                    validationMessages.Add(_messages, x.Id, MessageType.TimesheetIsBeingUsed, x.TimesheetNumber + ":" + x.TimesheetDescription);
            });

            if (validationMessages.Count > 0)
                messages.AddRange(validationMessages);

            return messages.Count <= 0;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.Timesheet> filteredData,
                                                 IList<DbModel.Timesheet> dbTimesheets,
                                                 ref IList<ValidationMessage> messages)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (messages == null)
                messages = new List<ValidationMessage>();

            var notMatchedRecords = filteredData.Where(x => !dbTimesheets.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.TimesheetId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                validationMessages.Add(_messages, x, MessageType.TimesheetUpdateCountMismatch, string.Format("{0:D5} : {1}", x.TimesheetAssignmentNumber, x.TimesheetDescription ?? "N/A"));
            });

            if (validationMessages.Count > 0)
                messages.AddRange(validationMessages);

            return messages?.Count <= 0;
        }

        private bool IsValidAssignment(IList<int> assignmentId,
                                      ref IList<DbModel.Assignment> dbAssignments,
                                      ref IList<ValidationMessage> messages,
                                      params string[] includes)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (dbAssignments == null)
            {
                //var dbAssignment = _assignmentRepository?.FindBy(x => assignmentId.Contains(x.Id), includes).AsNoTracking()?.ToList();
                var dbAssignment = _repository.GetDBTimesheetAssignments(assignmentId, includes);
                var assignmentNotExists = assignmentId?.Where(x => !dbAssignment.Any(x2 => x2.Id == x))?.ToList();
                assignmentNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.AssignmentNotExists.ToId();
                    message.Add(_messages, x, MessageType.AssignmentNotExists, x);
                });
                dbAssignments = dbAssignment;
                messages = message;
            }

            return messages?.Count <= 0;
        }

        #endregion
        #region SequenceNumber
        private int ProcessNumberSequence(int assignmentId,
                                        ref List<DbModel.NumberSequence> lstTimesheetNumberSequenceAdd,
                                        ref List<DbModel.NumberSequence> lstTimesheetNumberSequenceUpdate)
        {
            DbModel.NumberSequence timesheetNumberSequence = null;
            int TimesheetNumber = ProcessNumberSequence(assignmentId, ref timesheetNumberSequence);
            if (timesheetNumberSequence.LastSequenceNumber == 1)
            {
                lstTimesheetNumberSequenceAdd.Add(timesheetNumberSequence);
            }
            else if (timesheetNumberSequence.LastSequenceNumber > 1)
            {
                lstTimesheetNumberSequenceUpdate.Add(timesheetNumberSequence);
            }
            return TimesheetNumber;
        }

        public int ProcessNumberSequence(int assignmentId, ref DbModel.NumberSequence timesheetNumberSequence)
        {
            //var modules = _moduleRepository.FindBy(x => x.Name == "Assignment" || x.Name == "TimeSheet").ToList();

            //int AssignmentModuleId = modules.FirstOrDefault(x1 => x1.Name == "Assignment").Id;
            //int TimeSheetModuleId = modules.FirstOrDefault(x1 => x1.Name == "TimeSheet").Id;

            //DbModel.NumberSequence dbTimesheetNumberSequence = _numberSequenceRepository.FindBy(x => x.ModuleData == assignmentId 
            //&& x.Module.Name == "Assignment" && x.ModuleRef.Name  == "TimeSheet").FirstOrDefault();

            int parentId = 0;
            int parentRefId = 0;
            int timesheetNumber = _numberSequenceRepository.GetLastNumber(ModuleCodeType.ASGMNT,
                                                                          ModuleCodeType.TIME,
                                                                          ref parentId,
                                                                          ref parentRefId,
                                                                          assignmentId,
                                                                          ref timesheetNumberSequence);

            if (timesheetNumberSequence != null && timesheetNumberSequence.Id > 0)
            {
                timesheetNumberSequence.LastSequenceNumber = timesheetNumberSequence.LastSequenceNumber + 1;
            }
            else
            {
                timesheetNumberSequence = new DbModel.NumberSequence()
                {
                    LastSequenceNumber = 1,
                    ModuleId = parentId,
                    ModuleData = assignmentId,
                    ModuleRefId = parentRefId,
                };
            }
            return timesheetNumber;
        }

        public Response SaveNumberSequence(DbModel.NumberSequence timesheetNumberSequence)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                _numberSequenceRepository.AutoSave = false;

                if (timesheetNumberSequence.LastSequenceNumber == 1)
                    _numberSequenceRepository.Add(timesheetNumberSequence);
                if (timesheetNumberSequence.LastSequenceNumber > 1)
                    _numberSequenceRepository.Update(timesheetNumberSequence, x => x.LastSequenceNumber);
                _numberSequenceRepository.ForceSave();
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetNumberSequence);
            }
            finally
            {
                _numberSequenceRepository.AutoSave = true;
            }
            return new Response().ToPopulate(responseType, null, null, null, timesheetNumberSequence, exception);
        }

        public void AddTimesheetHistory(long timesheetId, string historyItemCode, string changedBy)
        {
            try
            {
                using (var tranScope = new TransactionScope())
                {
                    MasterData searchModel = new MasterData
                    {
                        MasterDataTypeId = (int)(MasterType.HistoryTable),
                        Code = historyItemCode.ToString()
                    };
                    var masterData = _masterRepository.Search(searchModel);
                    if (masterData != null && masterData.Count > 0)
                    {
                        _repository.AddTimesheetHistory(timesheetId, masterData[0].Id, changedBy);
                    }
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
        }

        #endregion
    }
}