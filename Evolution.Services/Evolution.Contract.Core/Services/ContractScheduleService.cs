using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Constants;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Contract.Domain.Enums;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Evolution.Contract.Domain.Interfaces.Data;
using Evolution.Contract.Domain.Interfaces.Validations;
using Evolution.Contract.Domain.Models.Contracts;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.Logging.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;


namespace Evolution.Contract.Core.Services
{
    public class ContractScheduleService : IContractScheduleService
    {
        private readonly IMapper _mapper = null;
        private IAppLogger<ContractScheduleService> _logger = null;
        private IContractScheduleRepository _contractScheduleRepository = null;
        private IDataRepository _dataRepository = null;
        private IContractRepository _contractRepository = null;
        private readonly IContractScheduleRateService _contractScheduleRateService = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IContractScheduleValidationService _validationService = null;
        private readonly IContractBatchRepository _contractBatchRepository = null;
        private IContractScheduleRateRepository _contractScheduleRateRepository = null;
        private IAuditSearchService _auditSearchService = null;
        private readonly IAuditLogger _auditLogger = null;

        public ContractScheduleService(IMapper mapper, IAppLogger<ContractScheduleService> logger, IContractScheduleRepository contractScheduleRepository, IDataRepository dataRepository, IContractRepository contractRepository, IContractScheduleRateRepository contractScheduleRateRepository,
                                 IContractScheduleRateService contractScheduleRateService, IContractScheduleValidationService validationService,
                                 JObject messages, IAuditSearchService auditSearchService, IContractBatchRepository contractBatchRepository,
                                 IAuditLogger auditLogger)
        {
            _contractBatchRepository = contractBatchRepository;
            this._mapper = mapper;
            this._logger = logger;
            this._contractScheduleRepository = contractScheduleRepository;
            this._dataRepository = dataRepository;
            this._contractScheduleRateRepository = contractScheduleRateRepository;
            this._contractRepository = contractRepository;
            this._contractScheduleRateService = contractScheduleRateService;
            this._validationService = validationService;
            this._messageDescriptions = messages;
            this._auditSearchService = auditSearchService;
            this._auditLogger = auditLogger;
        }

        #region Public Exposed Method

        public Response DeleteContractSchedule(string contractNumber, IList<ContractSchedule> contractSchedules, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModels.Contract> dbContract = null;
            IList<DbModels.SqlauditModule> dbModule = null;
            var response = RemoveContractScheule(contractNumber, contractSchedules, dbContract, dbModule, commitChange);
            if (response.Code == MessageType.Success.ToId() && isResultSetRequired)
            {
                return GetContractSchedule(new ContractSchedule() { ContractNumber = contractNumber });
            }
            else
            {
                return response;
            }
        }

        public Response DeleteContractSchedule(string contractNumber, IList<ContractSchedule> contractSchedules, IList<DbModels.Contract> dbContract, IList<DbModels.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false)
        {
            var response = RemoveContractScheule(contractNumber, contractSchedules, dbContract, dbModule, commitChange);
            if (response.Code == MessageType.Success.ToId() && isResultSetRequired)
            {
                return GetContractSchedule(new ContractSchedule() { ContractNumber = contractNumber });
            }
            else
            {
                return response;
            }
        }

        public Response GetContractSchedule(ContractSchedule searchModel)
        {
            IList<DomainModel.ContractSchedule> result = null;
            Exception exception = null;

            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                   new TransactionOptions
                                                   {
                                                       IsolationLevel = IsolationLevel.ReadUncommitted
                                                   }))
                {
                    // result = this._contractScheduleRepository.Search(searchModel).OrderBy(x => x.ScheduleName).ToList();
                    result = this._contractScheduleRepository.Search(searchModel).ToList(); //D1039 - Handled Sort Order in UI
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

        public Response ModifyContractSchedule(string contractNumber, IList<ContractSchedule> contractSchedules, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModels.Contract> dbContracts = null;
            IList<DbModels.Data> dbCurrency = null;
            IList<DbModels.SqlauditModule> dbModule = null;
            var response = UpdateContractSchedule(contractNumber, contractSchedules, dbContracts, dbCurrency, dbModule, commitChange);
            if (response.Code == MessageType.Success.ToId() && isResultSetRequired)
            {
                return GetContractSchedule(new ContractSchedule() { ContractNumber = contractNumber });
            }
            else
            {
                return response;
            }
        }

        public Response ModifyContractSchedule(string contractNumber, IList<ContractSchedule> contractSchedules, IList<DbModels.Contract> dbContracts, IList<DbModels.Data> dbCurrency, IList<DbModels.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false)
        {
            var response = UpdateContractSchedule(contractNumber, contractSchedules, dbContracts, dbCurrency, dbModule, commitChange);
            if (response.Code == MessageType.Success.ToId() && isResultSetRequired)
            {
                return GetContractSchedule(new ContractSchedule() { ContractNumber = contractNumber });
            }
            else
            {
                return response;
            }
        }


        public Response SaveContractSchedule(string contractNumber, IList<ContractSchedule> contractSchedules, bool commitChange = true, bool isResultSetRequired = false)
        {
            var response = AddContractSchedule(contractNumber, contractSchedules, commitChange);
            if (response.Code == MessageType.Success.ToId() && isResultSetRequired)
            {
                return GetContractSchedule(new ContractSchedule() { ContractNumber = contractNumber });
            }
            else
            {
                return response;
            }
        }

        //Response SaveContractScheduleAndScheduleRate(string contractNumber, IList<Models.Contracts.ContractSchedule> contractSchedules, IList<Models.Contracts.ContractScheduleRate> contractScheduleRates, IList<DbModels.Contract> dbContracts, IList<DbModels.Data> dbCurrency, IList<DbModels.Data> dbExpense, bool commitChange = true, bool isResultSetRequired = false);
        public Response SaveContractScheduleAndScheduleRate(string contractNumber, IList<ContractSchedule> contractSchedules, IList<ContractScheduleRate> contractScheduleRates, IList<DbModels.Contract> dbContracts, IList<DbModels.Data> dbCurrency, IList<DbModels.Data> dbExpense, IList<DbModels.SqlauditModule> dbModule, ref IList<DbModels.ContractRate> dbContractRate, ref IList<DbModels.ContractSchedule> dbInsertedContractSchedules, ValidationType validationType, bool commitChange = true, bool isResultSetRequired = false)
        {
            var response = AddContractScheduleAndScheduleRates(contractNumber, contractSchedules, contractScheduleRates, dbContracts, dbCurrency, dbExpense, dbModule, ref dbContractRate, ref dbInsertedContractSchedules, validationType, commitChange);
            if (response.Code == MessageType.Success.ToId() && isResultSetRequired)
            {
                return GetContractSchedule(new ContractSchedule() { ContractNumber = contractNumber });
            }
            else
            {
                return response;
            }
        }


        #endregion

        #region Private Exposed Methods
        private Response AddContractSchedule(string contractNumber, IList<ContractSchedule> contractSchedules, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModels.Contract contract = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _contractScheduleRepository.AutoSave = true;
                errorMessages = new List<MessageDetail>();
                eventId = contractSchedules?.FirstOrDefault()?.EventId;
                if (IsValidContractNumber(contractNumber, ref contract, ref errorMessages))
                {
                    IList<DomainModel.ContractSchedule> recordToBeInserted = null;
                    if (IsRecordValidForProcess(ValidationType.Add, contractSchedules, ref recordToBeInserted, ref errorMessages, ref validationMessages))
                    {
                        var chargeCurrencyList = recordToBeInserted.Select(x => x.ChargeCurrency).ToList();
                        if (IsValidCurrency(chargeCurrencyList, ref errorMessages))
                        {
                            IList<DbModels.ContractSchedule> dbContractSchedules = null;
                            if (IsScheduleNameAlreadyAssociatedToContract(contractNumber, recordToBeInserted, ValidationType.Add, ref dbContractSchedules, ref errorMessages, contractSchedules))
                            {
                                var dbSchedule = recordToBeInserted.Select(x => new DbModels.ContractSchedule()
                                {
                                    Id = 0,
                                    ContractId = contract.Id,
                                    Name = x.ScheduleName,
                                    ScheduleNoteForInvoice = x.ScheduleNameForInvoicePrint,
                                    Currency = x.ChargeCurrency,
                                    BaseScheduleId = x.BaseScheduleId,
                                    ModifiedBy = x.ModifiedBy,
                                }).ToList();

                                if (dbSchedule?.Any() == true)
                                    _contractScheduleRepository.Add(dbSchedule);

                                if (commitChange && !_contractScheduleRepository.AutoSave && recordToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                                {
                                    int value = _contractScheduleRepository.ForceSave();
                                    if (value > 0)
                                    {
                                        recordToBeInserted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(recordToBeInserted.FirstOrDefault(), ref eventId, recordToBeInserted?.FirstOrDefault()?.ActionByUser,
                                                                                                                 null,
                                                                                                               ValidationType.Add.ToAuditActionType(),
                                                                                                             SqlAuditModuleType.ContractSchedule,
                                                                                                             null,
                                                                                                            _mapper.Map<ContractSchedule>(x1)
                                                                                                            ));
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractSchedules);
            }
            finally
            {
                _contractScheduleRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response AddContractScheduleAndScheduleRates(string contractNumber, IList<ContractSchedule> contractSchedules, IList<ContractScheduleRate> contractScheduleRates,
                                                            IList<DbModels.Contract> dbContracts, IList<DbModels.Data> dbCurrency, IList<DbModels.Data> dbExpense,
                                                            IList<DbModels.SqlauditModule> dbModule, ref IList<DbModels.ContractRate> dbContractRate,
                                                            ref IList<DbModels.ContractSchedule> dbInsertedContractSchedules, ValidationType validationType,
                                                            bool commitChange = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModels.Contract contract = dbContracts?.Count > 0 ? dbContracts.FirstOrDefault() : null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModels.CompanyInspectionTypeChargeRate> companyInspectionTypeChargeRates = null;
            long? eventId = 0;
            int thresholdCount = 100;
            try
            {
                errorMessages = new List<MessageDetail>();
                _contractScheduleRepository.AutoSave = false;
                eventId = contractSchedules?.FirstOrDefault()?.EventId;
                if (IsValidContractNumber(contractNumber, ref contract, ref errorMessages))
                {
                    IList<DomainModel.ContractSchedule> recordToBeInserted = null;
                    if (IsRecordValidForProcess(ValidationType.Add, contractSchedules, ref recordToBeInserted, ref errorMessages, ref validationMessages))
                    {
                        if (recordToBeInserted?.Any() == true)
                        {
                            var chargeCurrencyList = recordToBeInserted.Select(x => x.ChargeCurrency).ToList();
                            if (IsValidCurrency(chargeCurrencyList, dbCurrency, ref errorMessages))
                            {
                                IList<DbModels.ContractSchedule> dbContractSchedules = null;
                                if (IsScheduleNameAlreadyAssociatedToContract(contractNumber, recordToBeInserted, ValidationType.Add, ref dbContractSchedules, ref errorMessages, contractSchedules))
                                {
                                    var newDbSchedules = recordToBeInserted.Select(x => new DbModels.ContractSchedule
                                    {
                                        ContractId = contract.Id,
                                        Name = x.ScheduleName,
                                        ScheduleNoteForInvoice = x.ScheduleNameForInvoicePrint,
                                        Currency = x.ChargeCurrency,
                                        /*AddScheduletoRF*/
                                        BaseScheduleId = x.BaseScheduleId,
                                        ModifiedBy = x.ModifiedBy,
                                    }).ToList();


                                    if (dbExpense?.Any() == true && dbContracts?.Any() == true && (dbContracts?.FirstOrDefault().ContractType == ContractType.IRF.ToString() ||
                                        dbContracts?.FirstOrDefault().ContractType == ContractType.FRW.ToString()) && (contractSchedules?.Count > thresholdCount))
                                    {
                                        _contractScheduleRateService.StandardInspectionTypeChargeRate(dbContracts.FirstOrDefault().ContractHolderCompanyId, contractScheduleRates, ref companyInspectionTypeChargeRates, ref errorMessages);
                                        _contractBatchRepository.BulkInsertSchedule(dbContracts.FirstOrDefault().Id, newDbSchedules, contractScheduleRates, companyInspectionTypeChargeRates, dbExpense, ref dbContractRate, ref dbInsertedContractSchedules);
                                    }

                                    else
                                    {
                                        if (contractScheduleRates?.Count > 0)
                                            this._contractScheduleRateService.AssignContractScheduleRatesToSchedule(contractNumber, ref newDbSchedules, contractScheduleRates, dbContracts, dbExpense, ref errorMessages);
                                        if (newDbSchedules?.Any() == true)
                                        {
                                            _contractScheduleRepository.Add(newDbSchedules);
                                            if (commitChange && !_contractScheduleRepository.AutoSave && recordToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                                            {
                                                int value = _contractScheduleRepository.ForceSave();
                                                if (value > 0)
                                                    dbInsertedContractSchedules = newDbSchedules;
                                                if (value > 0 && validationType == ValidationType.Update)
                                                {
                                                    newDbSchedules?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, contractSchedules?.FirstOrDefault()?.ActionByUser,
                                                                                                                       null,
                                                                                                                       ValidationType.Add.ToAuditActionType(),
                                                                                                                       SqlAuditModuleType.ContractSchedule,
                                                                                                                        null,
                                                                                                                       _mapper.Map<ContractSchedule>(x1),
                                                                                                                       dbModule
                                                                                                                        ));
                                                    if (contractScheduleRates?.Count > 0)
                                                    {
                                                        newDbSchedules?.ToList().ForEach(x1 => x1.ContractRate?.ToList().ForEach(x2 => _auditSearchService.AuditLog(recordToBeInserted.FirstOrDefault(), ref eventId, contractScheduleRates?.FirstOrDefault()?.ActionByUser,
                                                                                                                                                                     null,
                                                                                                                                                                     ValidationType.Add.ToAuditActionType(),
                                                                                                                                                                     SqlAuditModuleType.ContractRate,
                                                                                                                                                                    null,
                                                                                                                                                                    _mapper.Map<ContractScheduleRate>(x2),
                                                                                                                                                                    dbModule
                                                                                                                                                                     )));
                                                    }
                                                }
                                            }
                                        }
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractSchedules);
            }
            finally
            {
                _contractScheduleRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }


        public void AddRates(IList<DbModels.ContractRate> dbContractRate)
        {
            try
            {
                _contractBatchRepository.BulkInsertRate(dbContractRate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
        }

        public void AuditSchedules(long? eventId, int contractId, string contractNumber, List<DbModels.ContractSchedule> dbContractSchedule, IList<DbModels.SqlauditModule> dbModule, IList<DbModels.Data> dbExpense, bool IsBulk)
        {
            try
            {
                _contractBatchRepository.BulkAuditInsertSchedule(eventId, contractId, contractNumber, dbContractSchedule, dbModule, dbExpense, IsBulk);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
        }

        private Response UpdateContractSchedule(string contractNumber, IList<ContractSchedule> contractSchedules, IList<DbModels.Contract> dbContracts, IList<DbModels.Data> dbCurrency, IList<DbModels.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModels.Contract contract = dbContracts?.Count > 0 ? dbContracts.FirstOrDefault() : null; ;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                errorMessages = new List<MessageDetail>();
                _contractScheduleRepository.AutoSave = false;
                eventId = contractSchedules?.FirstOrDefault()?.EventId;

                if (IsValidContractNumber(contractNumber, ref contract, ref errorMessages))
                {
                    IList<DomainModel.ContractSchedule> recordToBeUpdated = null;
                    if (IsRecordValidForProcess(ValidationType.Update, contractSchedules, ref recordToBeUpdated, ref errorMessages, ref validationMessages))
                    {
                        var chargeCurrencyList = recordToBeUpdated.Select(x => x.ChargeCurrency).ToList();
                        if (IsValidCurrency(chargeCurrencyList, dbCurrency, ref errorMessages))
                        {
                            IList<DbModels.ContractSchedule> dbContractSchedules = null;
                            if (IsScheduleNameAlreadyAssociatedToContract(contractNumber, recordToBeUpdated, ValidationType.Update, ref dbContractSchedules, ref errorMessages, contractSchedules))
                            {
                                if (IsScheduleCanBeUpdated(contractNumber, recordToBeUpdated, dbContractSchedules, ref errorMessages))
                                {
                                    IList<DomainModel.ContractSchedule> domExistingContractSchedules = new List<DomainModel.ContractSchedule>();
                                    dbContractSchedules.ToList().ForEach(x =>
                                    {
                                        domExistingContractSchedules.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.ContractSchedule>(x)));
                                    });
                                    foreach (var schedule in recordToBeUpdated)
                                    {
                                        var dbSchedule = dbContractSchedules.Where(x => x.Id == schedule.ScheduleId).FirstOrDefault();
                                        dbSchedule.Name = schedule.ScheduleName;
                                        dbSchedule.Currency = schedule.ChargeCurrency;
                                        dbSchedule.ScheduleNoteForInvoice = schedule.ScheduleNameForInvoicePrint;
                                        dbSchedule.LastModification = DateTime.UtcNow;
                                        dbSchedule.ModifiedBy = schedule.ModifiedBy;
                                        dbSchedule.UpdateCount = dbSchedule.UpdateCount.CalculateUpdateCount();
                                        _contractScheduleRepository.Update(dbSchedule);
                                    }


                                    if (commitChange && !_contractScheduleRepository.AutoSave && recordToBeUpdated?.Count > 0 && errorMessages.Count <= 0)
                                    {
                                        int value = _contractScheduleRepository.ForceSave();
                                        if (value > 0)
                                        {
                                            recordToBeUpdated?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                      null,
                                                                     ValidationType.Update.ToAuditActionType(),
                                                                     SqlAuditModuleType.ContractSchedule,
                                                                       domExistingContractSchedules?.FirstOrDefault(x2 => x2.ScheduleId == x1.ScheduleId),
                                                                     x1,
                                                                     dbModule
                                                                     ));
                                        }

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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractSchedules);
            }
            finally
            {
                _contractScheduleRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response RemoveContractScheule(string contractNumber, IList<ContractSchedule> contractSchedules, IList<DbModels.Contract> dbContracts, IList<DbModels.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModels.Contract contract = dbContracts?.Count > 0 ? dbContracts.FirstOrDefault() : null; ;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                errorMessages = new List<MessageDetail>();
                _contractScheduleRepository.AutoSave = false;
                eventId = contractSchedules?.FirstOrDefault()?.EventId;

                if (IsValidContractNumber(contractNumber, ref contract, ref errorMessages))
                {
                    IList<DomainModel.ContractSchedule> recordToBeDeleted = null;
                    if (IsRecordValidForProcess(ValidationType.Delete, contractSchedules, ref recordToBeDeleted, ref errorMessages, ref validationMessages))
                    {
                        IList<DbModels.ContractSchedule> dbContractSchedules = null;
                        if (IsScheduleNameAlreadyAssociatedToContract(contractNumber, recordToBeDeleted, ValidationType.Delete, ref dbContractSchedules, ref errorMessages, contractSchedules))
                        {
                            if (IsScheduleCanBeDeleted(contractNumber, recordToBeDeleted, dbContractSchedules, ref errorMessages))
                            {
                                var delCont = _contractScheduleRepository.DeleteSchedule(recordToBeDeleted.ToList());
                                if (delCont > 0)
                                {
                                    contractSchedules?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                                  null,
                                                                                                      ValidationType.Delete.ToAuditActionType(),
                                                                                                       SqlAuditModuleType.ContractSchedule,
                                                                                                       x1,
                                                                                                        null,
                                                                                                        dbModule
                                                                                                       ));
                                }

                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractSchedules);
            }
            finally
            {
                _contractScheduleRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private bool IsValidContractNumber(string contractNumber, ref DbModels.Contract contract, ref List<MessageDetail> errorMessages)
        {
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            if (contract == null)
                contract = _contractRepository.FindBy(x => x.ContractNumber == contractNumber)?.FirstOrDefault();
            if (contract == null)
                errorMessages.Add(new MessageDetail(ModuleType.Contract, MessageType.ContractNumberDoesNotExists.ToId(), _messageDescriptions[MessageType.ContractNumberDoesNotExists.ToId()].ToString()));

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCurrency(List<string> currencyCode, ref List<MessageDetail> errorMessages)
        {

            var currencyList = _dataRepository.FindBy(x => currencyCode.Contains(x.Code) && x.MasterDataTypeId == (int)MasterType.Currency)?.ToList();
            if (currencyList.Count() <= 0)
            {
                errorMessages.Add(new MessageDetail(ModuleType.Contract, MessageType.InvalidChargeCurrency.ToId(), _messageDescriptions[MessageType.InvalidChargeCurrency.ToId()].ToString()));
            }

            return currencyList.Count > 0;

        }

        private bool IsValidCurrency(List<string> currencyCode, IList<DbModels.Data> dbCurrency, ref List<MessageDetail> errorMessages)
        {
            if (dbCurrency == null)
            {
                var currencyList = _dataRepository.FindBy(x => currencyCode.Contains(x.Code) && x.MasterDataTypeId == (int)MasterType.Currency)?.ToList();
                if (currencyList.Count() <= 0)
                {
                    errorMessages.Add(new MessageDetail(ModuleType.Contract, MessageType.InvalidChargeCurrency.ToId(), _messageDescriptions[MessageType.InvalidChargeCurrency.ToId()].ToString()));
                }

                return currencyList.Count > 0;
            }
            else
            {
                return true;
            }
        }

        private bool IsScheduleNameAlreadyAssociatedToContract(string contractNumber, IList<DomainModel.ContractSchedule> contractSchedules, ValidationType validationType, ref IList<DbModels.ContractSchedule> dbContractSchedules, ref List<MessageDetail> errorMessages, IList<DomainModel.ContractSchedule> allContractSchedules)//Added allContractSchedules to check with existing schedules - Contract Validation issue
        {
            IList<DbModels.ContractSchedule> contractScheduleExists = null;
            if (errorMessages == null)
            {
                errorMessages = new List<MessageDetail>();
            }
            if (!string.IsNullOrEmpty(contractNumber))
            {
                dbContractSchedules = _contractScheduleRepository.FindBy(x => x.Contract.ContractNumber == contractNumber).ToList();
                List<int> ids = null;
                if (allContractSchedules?.Count > 0 && allContractSchedules != null)
                {
                    ids = allContractSchedules.Select(x => x.ScheduleId).ToList();
                }

                if (validationType == ValidationType.Add && ids != null)
                {
                    //contractScheduleExists = dbContractSchedules.Where(x => contractSchedules.Any(x1 => x.Name == x1.ScheduleName && x.Currency == x1.ChargeCurrency && !(ids.Contains(x1.ScheduleId)))).ToList();
                    //contractScheduleExists = dbContractSchedules.Where(x => contractSchedules.Any(x1 => x.Name == x1.ScheduleName && x.Currency == x1.ChargeCurrency && (ids.Any(x2=> x2==x1.ScheduleId)))).ToList();

                    contractScheduleExists = dbContractSchedules.Where(x => contractSchedules.Any(x1 => x.Name == x1.ScheduleName && x.Currency == x1.ChargeCurrency && x1.ContractNumber == contractNumber)).ToList();
                    if (contractScheduleExists.Count > 0)
                    {
                        errorMessages.Add(new MessageDetail(ModuleType.Contract, MessageType.ScheduleNameAlreadyExists.ToId(), _messageDescriptions[MessageType.ScheduleNameAlreadyExists.ToId()].ToString()));
                    }
                }
                if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                {
                    contractScheduleExists = dbContractSchedules.Where(x => contractSchedules.Any(x1 => x.Id == x1.ScheduleId)).ToList();
                    if (contractScheduleExists.Count <= 0)
                    {
                        errorMessages.Add(new MessageDetail(ModuleType.Contract, MessageType.ScheduleNameDoesNotExists.ToId(), _messageDescriptions[MessageType.ScheduleNameDoesNotExists.ToId()].ToString()));
                    }
                }
                if (validationType == ValidationType.Delete)
                {
                    var assignmentContractSchedule = dbContractSchedules?.Where(x => contractSchedules.Any(x1 => x.Id == x1.ScheduleId) && x.AssignmentContractSchedule.Any())?.ToList();  //ITK D-588
                    if (assignmentContractSchedule?.Count > 0)
                    {
                        errorMessages.Add(new MessageDetail(ModuleType.Contract, MessageType.ContractAssignmentScheduleNameAlreadyExists.ToId(), _messageDescriptions[MessageType.ContractAssignmentScheduleNameAlreadyExists.ToId()].ToString()));
                    }
                }
            }
            return errorMessages.Count <= 0;
        }

        private bool IsScheduleCanBeUpdated(string contractNumber, IList<DomainModel.ContractSchedule> contractSchedules, IList<DbModels.ContractSchedule> dbontractSchedules, ref List<MessageDetail> errorMessages)
        {
            var notMatchedRecords = dbontractSchedules.Where(x => contractSchedules.Any(x1 => x1.UpdateCount != x.UpdateCount && x.Id == x1.ScheduleId)).ToList();

            if (notMatchedRecords.Count > 0)
            {
                errorMessages.Add(new MessageDetail(ModuleType.Contract, MessageType.ScheduleAlreadyUdpated.ToId(), _messageDescriptions[MessageType.ScheduleAlreadyUdpated.ToId()].ToString()));
            }

            return errorMessages.Count <= 0;

        }

        private bool IsScheduleCanBeDeleted(string ContractNumber, IList<DomainModel.ContractSchedule> contractSchedules, IList<DbModels.ContractSchedule> dbContractSchedules, ref List<MessageDetail> errorMessages)
        {
            if (errorMessages == null)
            {
                errorMessages = new List<MessageDetail>();
            }

            var dbSeclectedContractSchedule = dbContractSchedules.Where(x => contractSchedules.Any(x1 => x.Name == x1.ScheduleName && x.Currency == x1.ChargeCurrency)).ToList();

            var scheduleRates = dbSeclectedContractSchedule.Where(x => x.AssignmentContractSchedule.Count > 0 || x.AssignmentTechnicalSpecialistSchedule.Count > 0).ToList();
            if (scheduleRates.Count > 0)
            {
                errorMessages.Add(new MessageDetail(ModuleType.Contract, MessageType.ScheduleCannotBeDeleted.ToId(), _messageDescriptions[MessageType.ScheduleCannotBeDeleted.ToId()].ToString()));
            }

            return errorMessages.Count <= 0;

        }

        private bool IsRecordValidForProcess(ValidationType validationType, IList<DomainModel.ContractSchedule> contractSchedules, ref IList<DomainModel.ContractSchedule> filteredRecords, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            if (errorMessages == null)
            {
                errorMessages = new List<MessageDetail>();
            }

            if (validationType == ValidationType.Add)
            {
                filteredRecords = contractSchedules.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            }
            else if (validationType == ValidationType.Update)
            {
                filteredRecords = contractSchedules.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            }
            else if (validationType == ValidationType.Delete)
            {
                filteredRecords = contractSchedules.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            }

            if (filteredRecords?.Count <= 0)
            {
                return false;
            }

            return IsContractScheduleHasValidSchema(filteredRecords, validationType, ref errorMessages, ref validationMessages);
        }
        // Need Clarification On Base schedule 
        //private bool IsBaseScheduleValid(string contractNumber, IList<DomainModel.ContractSchedule> contractSchedules, ref IList<DbModels.ContractSchedule> dbContractSchedules, ref List<MessageDetail> errorMessages)
        //{
        //    IList<DbModels.ContractSchedule> baseScheduleExists = null;
        //    List<MessageDetail> messageDetails = new List<MessageDetail>();

        //    if (errorMessages == null)
        //        errorMessages = new List<MessageDetail>();

        //    var baseSchedules = contractSchedules.Where(x => !String.IsNullOrEmpty(x.BaseScheduleName)).Select(x => x.BaseScheduleName).ToList();

        //    if (baseSchedules.Count > 0)
        //        baseScheduleExists = dbContractSchedules.Where(x => baseSchedules.Any(x1 => x.Name == x1)).ToList();

        //    if (baseScheduleExists?.Count > 0)
        //        errorMessages.Add(new MessageDetail(ModuleType.Contract, MessageType.BaseScheduleDoesNotExists.ToId(), _messageDescriptions[MessageType.BaseScheduleDoesNotExists.ToId()].ToString()));

        //    return errorMessages.Count <= 0;
        //}
        private bool IsContractScheduleHasValidSchema(IList<DomainModel.ContractSchedule> models, ValidationType validationType, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {

            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
            {
                validationMessages = new List<ValidationMessage>();
            }

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(models), validationType);

            validationResults.ToList().ForEach(x =>
            {
                messages.Add(new ValidationMessage(x.Code, new List<MessageDetail> { new MessageDetail(ModuleType.Contract, x.Code, x.Message) }));
            });

            if (messages.Count > 0)
            {
                validationMessages.AddRange(messages);
            }
            return validationMessages?.Count <= 0;

        }

        public bool IsValidContractSchedule(IList<int> contractScheduleIds, ref IList<DbModels.ContractSchedule> dbContractSchedules, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
            {
                validationMessages = new List<ValidationMessage>();
            }

            if (dbContractSchedules == null)
            {
                var dbContScheduls = _contractScheduleRepository.FindBy(x => contractScheduleIds.Contains(x.Id)).AsNoTracking().ToList();
                var invalidContractScheduleIds = contractScheduleIds.Where(x => !dbContScheduls.Any(x1 => x1.Id == x));
                invalidContractScheduleIds.ToList().ForEach(x =>
                {
                    messages.Add(_messageDescriptions, x, MessageType.ContractScheduleInvalidId, x);
                });

                dbContractSchedules = dbContScheduls;
            }

            if (messages.Count > 0)
            {
                validationMessages.AddRange(messages);
            }

            return messages?.Count <= 0;
        }

        // Check related framework contract exists for the framework contract
        public Response IsRelatedFrameworkContractExists(int contractId)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = new List<MessageDetail>();
            List<ValidationMessage> validationMessages = null;
            try
            {
                var relatedFrameworkContracts = _contractRepository.FindBy(x => x.FrameworkContractId == contractId)?.Select(x1 => x1.Id)?.ToList();
                if (relatedFrameworkContracts?.Count <= 0)
                {
                    errorMessages.Add(new MessageDetail(ModuleType.Contract, MessageType.RelatedFrameworkContractNotExists.ToId(), _messageDescriptions[MessageType.RelatedFrameworkContractNotExists.ToId()].ToString()));
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractId);
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        public Response IsDuplicateFrameworkSchedulesExists(int contractId)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = new List<MessageDetail>();
            List<ValidationMessage> validationMessages = null;
            Object duplicateSchedule = new Object();
            try
            {
                IList<DbModels.ContractSchedule> frameworkSchedules = new List<DbModels.ContractSchedule>();
                IList<DbModels.ContractSchedule> relatedFrameworkSchedules = new List<DbModels.ContractSchedule>();
                frameworkSchedules = _contractScheduleRepository.FindBy(x => x.ContractId == contractId)?.ToList();
                var relatedFrameworkContracts = _contractRepository.FindBy(x => x.FrameworkContractId == contractId &&
                x.CreatedDate >= Convert.ToDateTime(ContractConstants.DateAfter) &&
                x.ContractType == ContractConstants.IsRelatedFrameworkContractType)?.Select(x1 => x1.Id)?.Distinct().ToList();   //Only Related Framework Contract with Start Date after August 2018 to be fetched ,because previous contracts will not have base schedule id
                //var relatedFrameworkContracts = _contractRepository.FindBy(x => x.FrameworkContractId == contractId)?.Select(x1 => x1.Id)?.Distinct().ToList();
                //relatedFrameworkSchedules = _contractScheduleRepository.FindBy(x => relatedFrameworkContracts.Contains(x.ContractId) && x.BaseScheduleId == null)?.ToList();
                relatedFrameworkSchedules = _contractScheduleRepository.FindBy(x => relatedFrameworkContracts.Contains(x.ContractId))?.ToList(); //Changes for ITK D789

                duplicateSchedule = frameworkSchedules.Join
                    (relatedFrameworkSchedules,
                    frameworkSchedule => new { id = frameworkSchedule?.Name.ToLower() },
                    relatedSchedule => new { id = relatedSchedule?.Name.ToLower() },
                    (frameworkSchedule, relatedSchedule) => new { frameworkSchedule, relatedSchedule }).Where(x => x.frameworkSchedule.Id != x.relatedSchedule.BaseScheduleId || x.relatedSchedule.BaseScheduleId == null)
                    .Select(x2 => new
                    {
                        x2.frameworkSchedule.Name,
                        x2.relatedSchedule.Contract.ContractNumber
                    })?.ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractId);
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, duplicateSchedule, exception);
        }

        /*AddScheduletoRF*/
        public Response CopyCStoRFC(IList<DomainModel.ContractSchedule> contractSchedule)
        {
            Exception exception = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                DbModels.ContractSchedule recordToBeInserted = null;
                DbModels.ContractRate recordToBeInsertedContractRate = null;
                IList<DbModels.ContractSchedule> dbListcontractSchedules = null;
                IList<DbModels.ContractSchedule> contractSchedules = null;
                DbModels.ContractSchedule dbContractSchedule = null;
                eventId = contractSchedule?.FirstOrDefault()?.EventId;
                contractSchedule?.ToList().ForEach(x =>
                {

                    string[] includes = new string[] { "ContractSchedule", "ContractSchedule.ContractRate" };

                    var ContractDetails = _contractRepository.FindBy(a => a.FrameworkContract.ContractNumber == x.ContractNumber.ToString() || a.ContractNumber == x.ContractNumber.ToString(), includes)?.ToList();
                    var IsRelatedContractDetail = ContractDetails?.Where(y => y.ContractType == nameof(ContractType.IRF)).ToList();
                    var FrameworkContractDetail = ContractDetails?.Where(y => y.ContractType == nameof(ContractType.FRW)).ToList();
                    if (ContractDetails?.Count > 0)
                    {
                        contractSchedules = new List<DbModels.ContractSchedule>();
                        ContractDetails.ForEach(x1 =>
                        {
                            contractSchedules.AddRange(_mapper.Map<IList<DbModels.ContractSchedule>>(x1.ContractSchedule));

                        });
                    }
                    if (IsRelatedContractDetail?.Count > 0)
                    {
                        dbContractSchedule = new DbModels.ContractSchedule();

                        IsRelatedContractDetail?.ToList().ForEach(relatedConId =>
                        {
                            var dbFContracts = FrameworkContractDetail.FirstOrDefault();
                            //var contractSchedules = _contractScheduleRepository.FindBy(y => y.Name == x.ScheduleName && (y.ContractId == dbFContracts.Id || y.ContractId == relatedConId.Id));
                            var currentContractSchedule = contractSchedules?.Where(y => y.Name == x.ScheduleName && y.ContractId == dbFContracts.Id).FirstOrDefault();
                            var updatedContractSchedule = contractSchedules?.Where(y => y.Name == x.ScheduleName && y.ContractId == relatedConId.Id).FirstOrDefault();

                            if (IsFCtoIRFContractScheduleExisist(x.ScheduleId, relatedConId.Id, contractSchedules.ToList(), ref dbContractSchedule))
                            {
                                dbListcontractSchedules = new List<DbModels.ContractSchedule>();
                                dbContractSchedule.Name = currentContractSchedule.Name;
                                dbContractSchedule.Currency = currentContractSchedule.Currency;
                                dbContractSchedule.ScheduleNoteForInvoice = currentContractSchedule.ScheduleNoteForInvoice;
                                dbContractSchedule.LastModification = DateTime.UtcNow;
                                dbContractSchedule.ModifiedBy = currentContractSchedule.ModifiedBy;
                                dbContractSchedule.UpdateCount = dbContractSchedule.UpdateCount.CalculateUpdateCount();
                                dbListcontractSchedules.Add(dbContractSchedule);
                                _contractScheduleRepository.Update(dbContractSchedule);
                                _contractScheduleRepository.ForceSave();
                                List<int> rateIds = dbContractSchedule?.ContractRate.Select(b => b.Id).ToList();
                                _contractScheduleRateRepository.DeleteContractRate(rateIds);
                                var recordToBeInsertedContractRates = currentContractSchedule?.ContractRate?.Select(a => new DbModels.ContractRate
                                {
                                    ContractScheduleId = dbContractSchedule.Id,
                                    ExpenseTypeId = a.ExpenseTypeId,
                                    Rate = a.Rate,
                                    Description = a.Description,
                                    IsPrintDescriptionOnInvoice = a.IsPrintDescriptionOnInvoice,
                                    FromDate = a.FromDate,
                                    ToDate = a.ToDate,
                                    IsActive = a.IsActive,
                                    LastModification = DateTime.UtcNow,
                                    StandardValue = a.StandardValue,
                                    DiscountApplied = a.DiscountApplied,
                                    Percentage = a.Percentage,
                                    StandardInspectionTypeChargeRateId = a.StandardInspectionTypeChargeRateId,
                                    BaseRateId = a.Id,
                                    BaseScheduleId = a.ContractScheduleId,
                                    UpdateCount = 0,
                                    ModifiedBy = x.ModifiedBy
                                }).ToList();
                                foreach (var rate in recordToBeInsertedContractRates)
                                {
                                    recordToBeInsertedContractRate = rate;
                                    _contractScheduleRateRepository.Add(recordToBeInsertedContractRate);
                                    _contractScheduleRateRepository.ForceSave();
                                }
                                _auditSearchService.AuditLog(x, ref eventId, null,
                                                             null,
                                                               ValidationType.Update.ToAuditActionType(),
                                                              SqlAuditModuleType.ContractSchedule,
                                                              currentContractSchedule, dbContractSchedule);

                                //AuditLog(x, ValidationType.Update.ToAuditActionType(), SqlAuditModuleType.ContractSchedule, currentContractSchedule, dbContractSchedule);
                            }
                            else
                            {
                                if (updatedContractSchedule == null)
                                {
                                    recordToBeInserted = contractSchedules?.Where(y => y.Id == currentContractSchedule.Id && y.ContractId == currentContractSchedule.ContractId).Select(a => new DbModels.ContractSchedule
                                    {
                                        Name = a.Name,
                                        ScheduleNoteForInvoice = a.ScheduleNoteForInvoice,
                                        ContractId = relatedConId.Id,
                                        Currency = a.Currency,
                                        BaseScheduleId = a.Id,
                                        LastModification = DateTime.UtcNow,
                                        UpdateCount = 0,
                                        ModifiedBy = x.ModifiedBy

                                    }).FirstOrDefault();
                                    _contractScheduleRepository.Add(recordToBeInserted);
                                    int value = _contractScheduleRepository.ForceSave();
                                    var recordToBeInsertedContractRates = currentContractSchedule?.ContractRate?.Select(a => new DbModels.ContractRate
                                    {
                                        ContractScheduleId = recordToBeInserted.Id,
                                        ExpenseTypeId = a.ExpenseTypeId,
                                        Rate = a.Rate,
                                        Description = a.Description,
                                        IsPrintDescriptionOnInvoice = a.IsPrintDescriptionOnInvoice,
                                        FromDate = a.FromDate,
                                        ToDate = a.ToDate,
                                        IsActive = a.IsActive,
                                        LastModification = DateTime.UtcNow,
                                        StandardValue = a.StandardValue,
                                        DiscountApplied = a.DiscountApplied,
                                        Percentage = a.Percentage,
                                        StandardInspectionTypeChargeRateId = a.StandardInspectionTypeChargeRateId,
                                        BaseRateId = a.Id,
                                        BaseScheduleId = a.ContractScheduleId,
                                        UpdateCount = 0,
                                        ModifiedBy = x.ModifiedBy
                                    }).ToList();

                                    foreach (var rate in recordToBeInsertedContractRates)
                                    {
                                        recordToBeInsertedContractRate = rate;
                                        _contractScheduleRateRepository.Add(recordToBeInsertedContractRate);
                                        _contractScheduleRateRepository.ForceSave();
                                    }


                                }
                                _auditSearchService.AuditLog(x, ref eventId, x.ActionByUser,
                                                             null,
                                                               ValidationType.Add.ToAuditActionType(),
                                                              SqlAuditModuleType.ContractSchedule,
                                                              null,
                                                               recordToBeInserted);
                            }
                        });

                    }
                }
                );

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractSchedule);
            }
            finally
            {
                _contractScheduleRepository.AutoSave = true;
                _contractScheduleRateRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages, null, exception);
        }

        // updated copy cs to rfc
        public string RelatedFrameworkContractBatch(int frameworkContractId, string createdBy)
        {
            string message = string.Empty;
            try
            {
                List<DbModels.SqlauditModule> dbModule = _auditSearchService.GetAuditModule(
                    new List<string>()
                    {
                        SqlAuditModuleType.Contract.ToString()
                        , SqlAuditModuleType.ContractSchedule.ToString()
                        , SqlAuditModuleType.ContractRate.ToString()
                    }).ToList();
                message = _contractBatchRepository.RelatedFrameworkContractBatch(frameworkContractId, createdBy, dbModule, _auditLogger);
            }

            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message)
                    && (ex.Message.ToLower().Contains("timed out")
                    || (ex.Message.ToLower().Contains("network"))))
                    message = "Copy has failed due to server not responding, please contact support";
                else
                    message = ex.Message;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), frameworkContractId);
            }
            finally
            {
            }
            return message;
        }

        private List<DbModels.SqlauditLogDetail> ProcessAudit(string createdBy, List<DbModels.SqlauditModule> dbModule,
            List<int> relatedFrameworkContracts, List<DbModels.Contract> relatedFrameworkContract,
            IList<DbModels.ContractSchedule> relatedFrameworkSchedules,
            List<DbModels.ContractRate> isRFScheduleRates,
            IList<DbModels.ContractSchedule> schedulesToInsert,
            IList<DbModels.ContractSchedule> schedulesToUpdate,
            IList<DbModels.ContractRate> ratesToUpdate,
            IList<DbModels.ContractRate> ratesToInsert,
            IList<DbModels.ContractRate> ratesToDelete,
            IList<DbModels.Data> expenseType)
        {
            List<DbModels.SqlauditLogDetail> totalAudit = new List<DbModels.SqlauditLogDetail>();
            relatedFrameworkContracts.ForEach(id =>
            {
                long? eventId = 0;
                DbModels.Contract item = relatedFrameworkContract?.FirstOrDefault(a => a.Id == id);
                List<DomainModel.ContractSchedule> schedulesToInserts = schedulesToInsert?.Where(a => a.ContractId == id)?.
                    Select(a => new DomainModel.ContractSchedule()
                    {
                        ScheduleId = a.Id,
                        ContractNumber = item.ContractNumber,
                        ScheduleName = a.Name,
                        ScheduleNameForInvoicePrint = a.ScheduleNoteForInvoice,
                        ChargeCurrency = a.Currency
                    })?.ToList();
                List<DomainModel.ContractSchedule> schedulesToUpdates = schedulesToUpdate?.Where(a => a.ContractId == id)?
                    .Select(a => new DomainModel.ContractSchedule()
                    {
                        ScheduleId = a.Id,
                        ContractNumber = item.ContractNumber,
                        ScheduleName = a.Name,
                        ScheduleNameForInvoicePrint = a.ScheduleNoteForInvoice,
                        ChargeCurrency = a.Currency
                    })?.ToList();
                List<DomainModel.ContractScheduleRate> ratesToUpdates = ratesToUpdate?.Where(a => a.ContractSchedule?.ContractId == id)?.
                    Select(a => new DomainModel.ContractScheduleRate()
                    {
                        RateId = a.Id,
                        ContractNumber = item.ContractNumber,
                        ScheduleName = a.ContractSchedule?.Name,  //a.ContractSchedule.Name,
                        Currency = a.ContractSchedule?.Currency,
                        StandardValue = a.StandardValue,
                        ChargeValue = Convert.ToString(a.Rate),
                        ChargeType = expenseType != null ? expenseType.FirstOrDefault(b => b.Id == a.ExpenseTypeId)?.Name : string.Empty,
                        Description = a.Description,
                        DiscountApplied = Convert.ToString(a.DiscountApplied),
                        Percentage = a.Percentage,
                        IsDescriptionPrintedOnInvoice = a.IsPrintDescriptionOnInvoice,
                        EffectiveFrom = a.FromDate,
                        EffectiveTo = a.ToDate,
                        IsActive = a.IsActive

                    })?.ToList();

                List<DomainModel.ContractScheduleRate> ratesToInserts = new List<ContractScheduleRate>();
                ratesToInserts = ratesToInsert?.Where(a => a.ContractSchedule?.ContractId == id)?.
                        Select(a => new DomainModel.ContractScheduleRate()
                        {
                            RateId = a.Id,
                            ContractNumber = item.ContractNumber,
                            ScheduleName = a.ContractSchedule?.Name,
                            Currency = a.ContractSchedule?.Currency,
                            StandardValue = a.StandardValue,
                            ChargeValue = Convert.ToString(a.Rate),
                            ChargeType = expenseType != null ? expenseType.FirstOrDefault(b => b.Id == a.ExpenseTypeId)?.Name : string.Empty,
                            Description = a.Description,
                            DiscountApplied = Convert.ToString(a.DiscountApplied),
                            Percentage = a.Percentage,
                            IsDescriptionPrintedOnInvoice = a.IsPrintDescriptionOnInvoice,
                            EffectiveFrom = a.FromDate,
                            EffectiveTo = a.ToDate,
                            StandardInspectionTypeChargeRateId = a.StandardInspectionTypeChargeRateId,
                            IsActive = a.IsActive
                        })?.ToList();

                List<DomainModel.ContractScheduleRate> ratesToDeletes = ratesToDelete?.Where(a => a.ContractSchedule?.ContractId == id)?.
                    Select(a => new DomainModel.ContractScheduleRate()
                    {
                        RateId = a.Id,
                        ContractNumber = item.ContractNumber,
                        ScheduleName = a.ContractSchedule?.Name,
                        Currency = a.ContractSchedule?.Currency,
                        StandardValue = a.StandardValue,
                        ChargeType = expenseType != null ? expenseType.FirstOrDefault(b => b.Id == a.ExpenseTypeId)?.Name : string.Empty,
                        ChargeValue = Convert.ToString(a.Rate),
                        Description = a.Description,
                        DiscountApplied = Convert.ToString(a.DiscountApplied),
                        Percentage = a.Percentage,
                        IsDescriptionPrintedOnInvoice = a.IsPrintDescriptionOnInvoice,
                        EffectiveFrom = a.FromDate,
                        EffectiveTo = a.ToDate,
                        IsActive = a.IsActive
                    })?.ToList();

                List<DomainModel.ContractSchedule> oldContractSchedule = relatedFrameworkSchedules?.
                    Where(a => a.ContractId == id)?.Select(
                            a => new DomainModel.ContractSchedule()
                            {
                                ScheduleId = a.Id,
                                ContractNumber = item.ContractNumber,
                                ScheduleName = a.Name,
                                ScheduleNameForInvoicePrint = a.ScheduleNoteForInvoice,
                                ChargeCurrency = a.Currency
                            }
                        )?.ToList();
                List<DomainModel.ContractScheduleRate> oldRates = isRFScheduleRates?.Where(a => a.ContractSchedule?.ContractId == id)?.
                    Select(a => new DomainModel.ContractScheduleRate()
                    {
                        RateId = a.Id,
                        ContractNumber = item.ContractNumber,
                        ScheduleName = a.ContractSchedule?.Name,
                        Currency = a.ContractSchedule?.Currency,
                        StandardValue = a.StandardValue,
                        ChargeType = expenseType != null ? expenseType.FirstOrDefault(b => b.Id == a.ExpenseTypeId)?.Name : string.Empty,
                        ChargeValue = Convert.ToString(a.Rate),
                        Description = a.Description,
                        DiscountApplied = Convert.ToString(a.DiscountApplied),
                        Percentage = a.Percentage,
                        IsDescriptionPrintedOnInvoice = a.IsPrintDescriptionOnInvoice,
                        EffectiveFrom = a.FromDate,
                        EffectiveTo = a.ToDate,
                        IsActive = a.IsActive
                    })?.ToList();

                LogEventGeneration logEvent = new LogEventGeneration(_auditLogger);
                eventId = logEvent.GetEventLogId(eventId,
                                     ValidationType.Update.ToAuditActionType(),
                                     createdBy,
                                      "{" + AuditSelectType.Id + ":" + item.Id + "}" +
                                      "${" + AuditSelectType.ContractNumber + ":" + item.ContractNumber?.Trim() + "}" +
                                      "${" + AuditSelectType.CustomerContractNumber + ":" + item.CustomerContractNumber?.Trim() + "}"
                                      , SqlAuditModuleType.Contract.ToString());
                List<DbModels.SqlauditLogDetail> auditSchedules = FormAuditLogSchedules(eventId, dbModule?.ToList(), schedulesToUpdates, schedulesToInserts, oldContractSchedule);
                List<DbModels.SqlauditLogDetail> auditRates = FormAuditLogRates(eventId, dbModule?.ToList(), ratesToUpdates, ratesToInserts, ratesToDeletes, oldRates);
                List<DbModels.SqlauditLogDetail> temp = auditSchedules.Union(auditRates).ToList();
                if (auditSchedules.Count > 0 || auditRates.Count > 0)
                {
                    DomainModel.Contract contract = new DomainModel.Contract()
                    {
                        Id = item.Id,
                        ContractNumber = item.ContractNumber,
                        CustomerContractNumber = item.CustomerContractNumber
                    };
                    temp.Add(new DbModels.SqlauditLogDetail()
                    {
                        NewValue = contract.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                        OldValue = contract.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                        SqlAuditSubModuleId = dbModule.FirstOrDefault(x1 => x1.ModuleName == SqlAuditModuleType.Contract.ToString()).Id,
                        SqlAuditLogId = (long)eventId,
                    });
                }
                totalAudit.AddRange(temp);
            });
            return totalAudit;
        }

        private List<DbModels.SqlauditLogDetail> FormAuditLogSchedules(long? eventId, List<DbModels.SqlauditModule> dbModule, List<DomainModel.ContractSchedule> schedulesToUpdate, List<DomainModel.ContractSchedule> schedulesToInsert, List<DomainModel.ContractSchedule> oldRecords)
        {
            List<DbModels.SqlauditLogDetail> sqlAuditLogDetail = new List<DbModels.SqlauditLogDetail>();
            int moduleTypeId = dbModule.FirstOrDefault(x1 => x1.ModuleName == SqlAuditModuleType.ContractSchedule.ToString()).Id;
            schedulesToUpdate.ForEach(item =>
            {
                var oldValue = oldRecords?.FirstOrDefault(a => a.ScheduleId == item.ScheduleId);
                sqlAuditLogDetail.Add(new DbModels.SqlauditLogDetail()
                {
                    NewValue = item.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                    OldValue = oldValue?.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                    SqlAuditSubModuleId = moduleTypeId,
                    SqlAuditLogId = (long)eventId,
                });
            });

            schedulesToInsert.ForEach(item =>
            {
                sqlAuditLogDetail.Add(new DbModels.SqlauditLogDetail()
                {
                    NewValue = item.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                    OldValue = null,
                    SqlAuditSubModuleId = moduleTypeId,
                    SqlAuditLogId = (long)eventId,
                });
            });
            return sqlAuditLogDetail;
        }

        private List<DbModels.SqlauditLogDetail> FormAuditLogRates(long? eventId, List<DbModels.SqlauditModule> dbModule, List<DomainModel.ContractScheduleRate> ratesToUpdate, List<DomainModel.ContractScheduleRate> ratesToInsert, List<DomainModel.ContractScheduleRate> ratesToDelete, List<DomainModel.ContractScheduleRate> oldRates)
        {
            List<DbModels.SqlauditLogDetail> sqlAuditLogDetail = new List<DbModels.SqlauditLogDetail>();
            int moduleTypeId = dbModule.FirstOrDefault(x1 => x1.ModuleName == SqlAuditModuleType.ContractRate.ToString()).Id;
            ratesToUpdate.ForEach(item =>
            {
                var oldValue = oldRates?.FirstOrDefault(a => a.RateId == item.RateId);
                sqlAuditLogDetail.Add(new DbModels.SqlauditLogDetail()
                {
                    NewValue = item.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                    OldValue = oldValue?.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                    SqlAuditSubModuleId = moduleTypeId,
                    SqlAuditLogId = (long)eventId,
                });
            });
            ratesToInsert.ForEach(item =>
            {
                sqlAuditLogDetail.Add(new DbModels.SqlauditLogDetail()
                {
                    NewValue = item.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                    OldValue = null,
                    SqlAuditSubModuleId = moduleTypeId,
                    SqlAuditLogId = (long)eventId,
                });
            });
            ratesToDelete.ForEach(item =>
            {
                sqlAuditLogDetail.Add(new DbModels.SqlauditLogDetail()
                {
                    NewValue = null,
                    OldValue = item.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName"),
                    SqlAuditSubModuleId = moduleTypeId,
                    SqlAuditLogId = (long)eventId,
                });
            });
            return sqlAuditLogDetail;
        }

        private bool IsFCtoIRFContractScheduleExisist(int ScheduleId, int ContractId, IList<DbModels.ContractSchedule> contractSchedules, ref DbModels.ContractSchedule dbContractSchedules)
        {
            int count = 0;
            var dbFRtoIRFSchedule = contractSchedules.Where(x => x.BaseScheduleId == ScheduleId && x.ContractId == ContractId);
            dbFRtoIRFSchedule.ToList().ForEach(x =>
            {
                count++;
            });
            if (count > 0)
            {
                dbContractSchedules = dbFRtoIRFSchedule.FirstOrDefault();
            }

            return count > 0;
        }

        #endregion
    }
}