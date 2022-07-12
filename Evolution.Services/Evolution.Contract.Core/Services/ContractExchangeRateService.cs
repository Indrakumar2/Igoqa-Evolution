using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.ExchangeRate;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Evolution.Contract.Domain.Interfaces.Data;
using Evolution.Contract.Domain.Interfaces.Validations;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.Logging.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.Contract.Core.Services
{
    public class ContractExchangeRateService : IContractExchangeRateService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<ContractExchangeRateService> _logger = null;
        private readonly IContractExchangeRateRepository _exchangeRateRepository = null;
        private readonly IContractExchangeRateValidationService _validationService = null;
        private readonly IDataRepository _dataRepository = null;
        private readonly IContractRepository _contractRepository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IAuditSearchService _auditSearchService = null;

        public ContractExchangeRateService(IMapper mapper, IAppLogger<ContractExchangeRateService> logger, IContractExchangeRateRepository exchangeRateRepository, IDataRepository dataRepository,
                                            IContractRepository contractRepository, IContractExchangeRateValidationService validationService, JObject messages, IAuditSearchService auditSerachService)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._exchangeRateRepository = exchangeRateRepository;
            this._dataRepository = dataRepository;
            this._contractRepository = contractRepository;
            this._validationService = validationService;
            this._messageDescriptions = messages;
            this._auditSearchService = auditSerachService;
        }

        #region Public Exposed Method

        public Response DeleteContractExchangeRate(string contractNumber, IList<DomainModel.ContractExchangeRate> contractExchangeRates, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Contract> dbContracts = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            bool IsFixedExchangeRateUsed = true;
            var response = RemoveContractExchangeRate(contractNumber, contractExchangeRates, dbContracts,dbModule, IsFixedExchangeRateUsed, commitChange);
            if (response.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractExchangeRate(new DomainModel.ContractExchangeRate() { ContractNumber = contractNumber });
            else
                return response;
        }

        public Response DeleteContractExchangeRate(string contractNumber, IList<DomainModel.ContractExchangeRate> contractExchangeRates, IList<DbModel.Contract> dbContracts, IList<DbModel.SqlauditModule> dbModule, bool IsFixedExchangeRateUsed = true, bool commitChange = true, bool isResultSetRequired = false)
        {
            var response = RemoveContractExchangeRate(contractNumber, contractExchangeRates, dbContracts,dbModule, IsFixedExchangeRateUsed, commitChange);
            if (response.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractExchangeRate(new DomainModel.ContractExchangeRate() { ContractNumber = contractNumber });
            else
                return response;
        }

        public Response GetContractExchangeRate(DomainModel.ContractExchangeRate searchModel)
        {
            IList<DomainModel.ContractExchangeRate> result = null;
            Exception exception = null;
            try
            {
                //Getting Exception - transaction enlisted error
                // using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                //                                    new TransactionOptions
                //                                    {
                //                                        IsolationLevel = IsolationLevel.ReadUncommitted
                //                                    }))
                // {
                     result = _exchangeRateRepository.Search(searchModel);
                    //tranScope.Complete();
                //}
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);

        }

        public Response ModifyContractExchangeRate(string contractNumber, IList<DomainModel.ContractExchangeRate> contractExchangeRates, IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbCurrency, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false)
        {
            var response = UpdateContractExchangeRate(contractNumber, contractExchangeRates, dbContracts, dbCurrency,dbModule, commitChange);
            if (response.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractExchangeRate(new DomainModel.ContractExchangeRate() { ContractNumber = contractNumber });
            else
                return response;
        }
        public Response ModifyContractExchangeRate(string contractNumber, IList<DomainModel.ContractExchangeRate> contractExchangeRates, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Contract> dbContracts = null;
            IList<DbModel.Data> dbCurrency = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            var response = UpdateContractExchangeRate(contractNumber, contractExchangeRates, dbContracts, dbCurrency, dbModule,commitChange);
            if (response.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractExchangeRate(new DomainModel.ContractExchangeRate() { ContractNumber = contractNumber });
            else
                return response;
        }

        public Response SaveContractExchangeRate(string contractNumber, IList<DomainModel.ContractExchangeRate> contractExchangeRates, IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbCurrency, IList<DbModel.SqlauditModule> dbModule, bool IsFixedExchangeRateUsed = true, bool commitChange = true, bool isResultSetRequired = false)
        {
            var response = AddContractExchangeRate(contractNumber, contractExchangeRates, dbContracts, dbCurrency, dbModule,IsFixedExchangeRateUsed, commitChange);
            if (response.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractExchangeRate(new DomainModel.ContractExchangeRate() { ContractNumber = contractNumber });
            else
                return response;
        }

        public Response SaveContractExchangeRate(string contractNumber, IList<DomainModel.ContractExchangeRate> contractExchangeRates, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Contract> dbContracts = null;
            IList<DbModel.Data> dbCurrency = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            bool IsFixedExchangeRateUsed = true;
            var response = AddContractExchangeRate(contractNumber, contractExchangeRates, dbContracts, dbCurrency,dbModule, IsFixedExchangeRateUsed, commitChange);
            if (response.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractExchangeRate(new DomainModel.ContractExchangeRate() { ContractNumber = contractNumber });
            else
                return response;
        }

        public Response GetContractExchangeRates(IList<int> contractIds)
        {
            IList<Common.Models.ExchangeRate.ContractExchangeRate> result = null;
            Exception exception = null;
            try
            {
               // using(var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, TransactionScopeAsyncFlowOption.Enabled, IsolationLevel = IsolationLevel.ReadUncommitted }))
                //using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                //{
                    result = _exchangeRateRepository.GetContractExchangeRates(contractIds);
                //    tranScope.Complete();
                //}
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public decimal FetchExchangeRate(int contractId, string currencyFrom, string currencyTo, ref IList<ExchangeRate> exchangeRates, ref IList<Common.Models.ExchangeRate.ContractExchangeRate> contractExchangeRates)
        {
            decimal? rate = null;
            rate = contractExchangeRates?.FirstOrDefault(x => x.ContractId == contractId && x.CurrencyFrom == currencyFrom && x.CurrencyTo == currencyTo)?.Rate;
            if (!rate.HasValue)
            {
                rate = exchangeRates?.FirstOrDefault(x => x.CurrencyFrom == currencyFrom && x.CurrencyTo == currencyTo)?.Rate;
            }
            return rate ?? 0;
        }

        #endregion

        #region Private Exposed Methods

        private Response AddContractExchangeRate(string contractNumber, IList<DomainModel.ContractExchangeRate> contractExchangeRates, IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbCurrency, IList<DbModel.SqlauditModule> dbModule, bool IsFixedExchangeRateUsed, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Contract contract = dbContracts?.Count > 0 ? dbContracts.FirstOrDefault() : null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _exchangeRateRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<DomainModel.ContractExchangeRate> recordToBeInserted = null;
                eventId = contractExchangeRates.FirstOrDefault().EventId;
                if (IsRecordsValidToProcessed(contractExchangeRates, ValidationType.Add, ref recordToBeInserted, ref errorMessages, ref validationMessages))
                {
                    if (IsValidContract(contractNumber, ref contract, ref errorMessages))
                    {
                        if (IsValidFixedExchangeRateFromAndToCurrencySame(recordToBeInserted, ref errorMessages))
                        {
                            if (IsValidCurrency(recordToBeInserted, dbCurrency, ref errorMessages))
                            {
                                IList<DbModel.ContractExchangeRate> dbContractExchangeRates = null;
                                if (IsExchangeRateAlreadyAssociatedToContract(contractNumber, recordToBeInserted, ValidationType.Add, ref dbContractExchangeRates, ref errorMessages))
                                {
                                    var dbRecordsToInsert = _mapper.Map<IList<DbModel.ContractExchangeRate>>(recordToBeInserted);
                                    dbRecordsToInsert.ToList().ForEach(x => { x.ContractId = contract.Id; x.Id = 0; });
                                    _exchangeRateRepository.Add(dbRecordsToInsert);
                                    if (IsFixedExchangeRateUsed)
                                        UpdateContract(contract, true, contractExchangeRates);

                                    if (commitChange && !_exchangeRateRepository.AutoSave && recordToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                                    {
                                        int value = _exchangeRateRepository.ForceSave();
                                        if (value >= 0)
                                        {
                                            dbRecordsToInsert?.ToList().ForEach(x1 => _auditSearchService.AuditLog(recordToBeInserted.FirstOrDefault(), ref eventId, recordToBeInserted.FirstOrDefault().ActionByUser,
                                                                                                 null,
                                                                                                  ValidationType.Add.ToAuditActionType(),
                                                                                                 SqlAuditModuleType.ContractExchange,
                                                                                                   null,
                                                                                                    _mapper.Map<DomainModel.ContractExchangeRate>(x1),
                                                                                                   dbModule));
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractExchangeRates);
            }
            finally
            {
                _exchangeRateRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response UpdateContractExchangeRate(string contractNumber, IList<DomainModel.ContractExchangeRate> contractExchangeRates, IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbCurrency, IList<DbModel.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            DbModel.Contract contract = dbContracts?.Count > 0 ? dbContracts.FirstOrDefault() : null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _exchangeRateRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<DomainModel.ContractExchangeRate> recordToBeUpdated = null;
                eventId = contractExchangeRates.FirstOrDefault().EventId;
                if (IsRecordsValidToProcessed(contractExchangeRates, ValidationType.Update, ref recordToBeUpdated, ref errorMessages, ref validationMessages))
                {
                    if (IsValidContract(contractNumber, ref contract, ref errorMessages))
                    {
                        if (IsValidFixedExchangeRateFromAndToCurrencySame(recordToBeUpdated, ref errorMessages))
                        {
                            if (IsValidCurrency(recordToBeUpdated, dbCurrency, ref errorMessages))
                            {
                                IList<DbModel.ContractExchangeRate> dbContractExchangeRates = null;
                                if (IsExchangeRateAlreadyAssociatedToContract(contractNumber, recordToBeUpdated, ValidationType.Update, ref dbContractExchangeRates, ref errorMessages))
                                {
                                    if (IsExchangeRateCanBeUpdated(recordToBeUpdated, dbContractExchangeRates, ref errorMessages))
                                    {
                                        IList<DomainModel.ContractExchangeRate> domExistingContractExchange = new List<DomainModel.ContractExchangeRate>();
                                        dbContractExchangeRates.ToList().ForEach(x =>
                                       {
                                           domExistingContractExchange.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.ContractExchangeRate>(x)));
                                       });
                                        foreach (var exchangeRate in recordToBeUpdated)
                                        {
                                            var dbExchangeRate = dbContractExchangeRates.Where(x => x.Id == exchangeRate.ExchangeRateId).FirstOrDefault();
                                            dbExchangeRate.CurrencyFrom = exchangeRate.FromCurrency;
                                            dbExchangeRate.CurrencyTo = exchangeRate.ToCurrency;
                                            dbExchangeRate.EffectiveFrom = exchangeRate.EffectiveFrom;
                                            dbExchangeRate.ExchangeRate = (decimal)exchangeRate.ExchangeRate;
                                            dbExchangeRate.LastModification = DateTime.UtcNow;
                                            dbExchangeRate.UpdateCount = dbExchangeRate.UpdateCount.CalculateUpdateCount();
                                            dbExchangeRate.ModifiedBy = exchangeRate.ModifiedBy;
                                            _exchangeRateRepository.Update(dbExchangeRate);
                                        }

                                        if (commitChange && !_exchangeRateRepository.AutoSave && recordToBeUpdated?.Count > 0 && errorMessages.Count <= 0)
                                        {
                                            int value = _exchangeRateRepository.ForceSave();
                                            if (value > 0)


                                                recordToBeUpdated?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                                        null,
                                                                                                                        ValidationType.Update.ToAuditActionType(),
                                                                                                                        SqlAuditModuleType.ContractExchange,
                                                                                                                         domExistingContractExchange?.FirstOrDefault(x2 => x2.ExchangeRateId == x1.ExchangeRateId),
                                                                                                                         x1,
                                                                                                                         dbModule));
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractExchangeRates);
            }
            finally
            {
                _exchangeRateRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response RemoveContractExchangeRate(string contractNumber, IList<DomainModel.ContractExchangeRate> contractExchangeRates, IList<DbModel.Contract> dbContracts, IList<DbModel.SqlauditModule> dbModule, bool IsFixedExchangeRateUsed, bool commitChange)
        {
            Exception exception = null;
            DbModel.Contract contract = dbContracts?.Count > 0 ? dbContracts.FirstOrDefault() : null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _exchangeRateRepository.AutoSave = true;
                errorMessages = new List<MessageDetail>();
                IList<DomainModel.ContractExchangeRate> recordToBeDeleted = null;
                eventId = contractExchangeRates.FirstOrDefault().EventId;
                if (IsRecordsValidToProcessed(contractExchangeRates, ValidationType.Delete, ref recordToBeDeleted, ref errorMessages, ref validationMessages))
                {
                    if (IsValidContract(contractNumber, ref contract, ref errorMessages))
                    {
                        IList<DbModel.ContractExchangeRate> dbContractExchangeRates = null;
                        if (IsExchangeRateAlreadyAssociatedToContract(contractNumber, recordToBeDeleted, ValidationType.Update, ref dbContractExchangeRates, ref errorMessages))
                        {
                            var delCont = _exchangeRateRepository.DeleteExchangeRate(recordToBeDeleted.ToList());
                            if (IsFixedExchangeRateUsed)
                                UpdateContract(contract, false, contractExchangeRates);

                            if (delCont > 0)
                            {
                                contractExchangeRates?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                            null,
                                                                                                            ValidationType.Delete.ToAuditActionType(),
                                                                                                             SqlAuditModuleType.ContractExchange,
                                                                                                             x1,
                                                                                                            null,
                                                                                                            dbModule));
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractExchangeRates);
            }
            finally
            {
                _exchangeRateRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private bool IsValidContract(string contractNumber, ref DbModel.Contract contract, ref List<MessageDetail> errorMessages)
        {
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            if (contract == null)
                contract = _contractRepository.FindBy(x => x.ContractNumber == contractNumber)?.FirstOrDefault();
            if (contract == null)
                errorMessages.Add(new MessageDetail(ModuleType.Contract, MessageType.ContractNumberDoesNotExists.ToId(), _messageDescriptions[MessageType.ContractNumberDoesNotExists.ToId()].ToString()));

            return errorMessages?.Count <= 0;
        }
        private bool IsValidFixedExchangeRateFromAndToCurrencySame(IList<DomainModel.ContractExchangeRate> recordToBeInserted, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            var FromAndToCurrencySame = recordToBeInserted.Where(x => x.FromCurrency == x.ToCurrency).ToList();
            FromAndToCurrencySame.ForEach(x =>
            {
                string errorcode = MessageType.FixedExchangeRateFromAndToCurrencySame.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorcode, string.Format(_messageDescriptions[errorcode].ToString(), x.FromCurrency)));

            });
            if (messages.Count > 0)
                errorMessages.AddRange(messages);
            return errorMessages?.Count <= 0;

        }

        private bool IsValidCurrency(IList<DomainModel.ContractExchangeRate> recordToBeInserted, IList<DbModel.Data> dbCurrency, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            if (dbCurrency?.Count == 0)
            {
                var currencyCodes = recordToBeInserted.Select(x => x.FromCurrency).Union(recordToBeInserted.Select(x => x.ToCurrency)).ToList();
                if (currencyCodes?.Count > 0)
                {
                    var dbCurrencies = this._dataRepository.FindBy(x => currencyCodes.Contains(x.Code) && x.MasterDataTypeId == (int)MasterType.Currency)?.ToList();
                    var currencyNotExists = currencyCodes.Where(x => !dbCurrencies.Any(x1 => x1.Code == x)).ToList();

                    currencyNotExists.ForEach(x =>
                    {
                        string errorCode = MessageType.InvalidCurrencyFromOrCurrencyTo.ToId();
                        messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x)));
                    });
                }
            }

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsRecordsValidToProcessed(IList<DomainModel.ContractExchangeRate> contractExchangeRates, ValidationType validationType, ref IList<DomainModel.ContractExchangeRate> filteredExchangeRates, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {

            if (validationType == ValidationType.Add)
                filteredExchangeRates = contractExchangeRates.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            if (validationType == ValidationType.Update)
                filteredExchangeRates = contractExchangeRates.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            if (validationType == ValidationType.Delete)
                filteredExchangeRates = contractExchangeRates.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            if (filteredExchangeRates?.Count <= 0)
                return false;

            return IsContractExchangeRateHasValidSchema(filteredExchangeRates, validationType, ref errorMessages, ref validationMessages);

        }


        private bool IsExchangeRateAlreadyAssociatedToContract(string contractNumber, IList<DomainModel.ContractExchangeRate> exchangeRates, ValidationType validationType, ref IList<DbModel.ContractExchangeRate> dbContractExchangeRates, ref List<MessageDetail> errorMessages)
        {
            IList<DbModel.ContractExchangeRate> contractExchangeRateExists = null;
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            if (contractNumber != null)
                dbContractExchangeRates = _exchangeRateRepository.FindBy(x => x.Contract.ContractNumber == contractNumber)?.ToList();
            if (validationType == ValidationType.Add && dbContractExchangeRates?.Any()==true)
            {
                contractExchangeRateExists = dbContractExchangeRates.Where(x => exchangeRates.Any(x1 => x.CurrencyFrom == x1.FromCurrency && x1.ToCurrency == x.CurrencyTo && x1.EffectiveFrom == x.EffectiveFrom && x1.ContractNumber == contractNumber)).ToList();
                if (contractExchangeRateExists.Count > 0)
                    errorMessages.Add(new MessageDetail(ModuleType.Contract, MessageType.FixedExchangeRateAlreadyExists.ToId(), _messageDescriptions[MessageType.FixedExchangeRateAlreadyExists.ToId()].ToString()));

            }
            if (validationType == ValidationType.Update || validationType == ValidationType.Delete && dbContractExchangeRates?.Any() == true)
            {
                contractExchangeRateExists = dbContractExchangeRates.Where(x => exchangeRates.Any(x1 => x.Id == x1.ExchangeRateId)).ToList();
                if (contractExchangeRateExists.Count <= 0)
                    errorMessages.Add(new MessageDetail(ModuleType.Contract, MessageType.FixedExchangeRateDoesNotExists.ToId(), _messageDescriptions[MessageType.FixedExchangeRateDoesNotExists.ToId()].ToString()));
            }

            return errorMessages?.Count <= 0;
        }

        private bool IsExchangeRateCanBeUpdated(IList<DomainModel.ContractExchangeRate> contractExchangeRates, IList<DbModel.ContractExchangeRate> dbContractExchangeRates, ref List<MessageDetail> errorMessages)
        {
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            var notMatchedReocrds = contractExchangeRates.Where(x =>
                                                        dbContractExchangeRates.ToList().Any(x1 => x1.UpdateCount.ToInt() != x.UpdateCount.ToInt() && x.ExchangeRateId == x1.Id)).ToList();
            if (notMatchedReocrds.Count > 0)
                errorMessages.Add(new MessageDetail(ModuleType.Contract, MessageType.FixedExchangeRateAlreadyUpdated.ToId(), _messageDescriptions[MessageType.FixedExchangeRateAlreadyUpdated.ToId()].ToString()));
            return errorMessages?.Count <= 0;
        }

        private void UpdateContract(DbModel.Contract contract, bool UseFixedExchangeRates, IList<DomainModel.ContractExchangeRate> contractExchange)
        {
            contract.IsUseFixedExchangeRates = UseFixedExchangeRates;
            if (UseFixedExchangeRates == false)
            {
                var contractExchangeRates = _exchangeRateRepository.FindBy(x => x.Contract.ContractNumber == contract.ContractNumber)?.ToList();
                if (contractExchangeRates == null || contractExchangeRates.Count <= 0)
                {
                    _contractRepository.Update(contract);
                }
            }
            else
                _contractRepository.Update(contract);

            //AuditLog(contractExchange.FirstOrDefault(),
            //         ValidationType.Delete.ToAuditActionType(),
            //         SqlAuditModuleType.ContractExchange,
            //         _mapper.Map<DomainModel.Contract>(contract),
            //         null);

        }

        private bool IsContractExchangeRateHasValidSchema(IList<DomainModel.ContractExchangeRate> models, ValidationType validationType, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {

            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

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

        #endregion
    }
}
