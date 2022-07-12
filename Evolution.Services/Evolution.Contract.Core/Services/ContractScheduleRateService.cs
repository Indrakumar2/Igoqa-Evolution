using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Evolution.Contract.Domain.Interfaces.Data;
using Evolution.Contract.Domain.Interfaces.Validations;
using Evolution.Contract.Domain.Models.Contracts;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
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
    public class ContractScheduleRateService : IContractScheduleRateService
    {
        private readonly IMapper _mapper = null;
        private IAppLogger<ContractScheduleRateService> _logger = null;
        private IContractScheduleRateRepository _contractScheduleRateRepository = null;
        private readonly IContractScheduleRepository _contractScheduleRepository = null;
        private IContractRepository _contractRepository = null;
        private readonly IDataRepository _dataRepository = null;
        private readonly ICompanyInspectionTypeChargeRateRepository _companyInspectionTypeChargeRateRepository = null;
        private readonly IContractScheduleRateValidationService _validationService = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IAuditSearchService _auditSearchService = null;

        public ContractScheduleRateService(IMapper mapper, IAppLogger<ContractScheduleRateService> logger, IContractScheduleRateRepository contractScheduleRateRepository, IContractRepository contractRepository, IDataRepository dataRepository, IContractScheduleRepository contractScheduleRepository,
                                          ICompanyInspectionTypeChargeRateRepository companyInspectionTypeChargeRateRepository, IContractScheduleRateValidationService validationService, JObject messages, IAuditSearchService auditSearchService)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._contractScheduleRateRepository = contractScheduleRateRepository;
            this._contractRepository = contractRepository;
            this._dataRepository = dataRepository;
            this._contractScheduleRepository = contractScheduleRepository;
            this._companyInspectionTypeChargeRateRepository = companyInspectionTypeChargeRateRepository;
            this._validationService = validationService;
            this._messageDescriptions = messages;
            this._auditSearchService = auditSearchService;

        }

        #region Public Exposed Method

        public Response DeleteContractScheduleRate(string contractNumber, string ScheduleName, IList<ContractScheduleRate> deleteModel, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Contract> dbContracts = null;
            var result = this.RemoveContractScheduleRates(contractNumber, ScheduleName, deleteModel, dbContracts, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractScheduleRate(new ContractScheduleRate() { ContractNumber = contractNumber, ScheduleName = ScheduleName });
            else
                return result;
        }

        public Response DeleteContractScheduleRate(string contractNumber, string ScheduleName, IList<ContractScheduleRate> deleteModel, IList<DbModel.Contract> dbContracts, bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = this.RemoveContractScheduleRates(contractNumber, ScheduleName, deleteModel, dbContracts, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractScheduleRate(new ContractScheduleRate() { ContractNumber = contractNumber, ScheduleName = ScheduleName });
            else
                return result;
        }

        public Response GetContractScheduleRate(ContractScheduleRate searchModel)
        {
            IList<DomainModel.ContractScheduleRate> result = null;
            Exception exception = null;
            
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                  new TransactionOptions
                                                  {
                                                      IsolationLevel = IsolationLevel.ReadUncommitted
                                                  }))
                {
                    result = this._contractScheduleRateRepository.Search(searchModel).OrderBy(x => x.ChargeType).ToList();
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

        public Response ModifyContractScheduleRate(string contractNumber, string ScheduleName, IList<ContractScheduleRate> contractScheduleRates, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Contract> dbContracts = null;
            IList<DbModel.Data> dbContractExpense = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = this.UpdateContractScheduleRates(contractNumber, ScheduleName, contractScheduleRates, dbContracts, dbContractExpense, dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractScheduleRate(new ContractScheduleRate() { ContractNumber = contractNumber, ScheduleName = ScheduleName });
            else
                return result;
        }

        public Response ModifyContractScheduleRate(string contractNumber, string ScheduleName, IList<ContractScheduleRate> contractScheduleRates, IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbContractExpense, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = this.UpdateContractScheduleRates(contractNumber, ScheduleName, contractScheduleRates, dbContracts, dbContractExpense, dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractScheduleRate(new ContractScheduleRate() { ContractNumber = contractNumber, ScheduleName = ScheduleName });
            else
                return result;
        }

        public Response SaveContractScheduleRate(string contractNumber, string ScheduleName, IList<ContractScheduleRate> contractScheduleRates,ValidationType validationType, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Contract> dbContracts = null;
            IList<DbModel.Data> dbContractExpense = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = this.AddContractScheduleRates(contractNumber, ScheduleName, contractScheduleRates, dbContracts, dbContractExpense, dbModule, commitChange, validationType);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractScheduleRate(new ContractScheduleRate() { ContractNumber = contractNumber, ScheduleName = ScheduleName });
            else
                return result;
        }

        public Response SaveContractScheduleRate(string contractNumber, string ScheduleName, IList<ContractScheduleRate> contractScheduleRates, IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbContractExpense, IList<DbModel.SqlauditModule> dbModule,ValidationType validationType, bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = this.AddContractScheduleRates(contractNumber, ScheduleName, contractScheduleRates, dbContracts, dbContractExpense, dbModule, commitChange, validationType);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractScheduleRate(new ContractScheduleRate() { ContractNumber = contractNumber, ScheduleName = ScheduleName });
            else
                return result;
        }

        public void AssignContractScheduleRatesToSchedule(string contractNumber, ref List<DbModel.ContractSchedule> dbContractSchedules,
                                                            IList<ContractScheduleRate> contractScheduleRates, IList<DbModel.Contract> dbContracts,
                                                            IList<DbModel.Data> dbExpense, ref List<MessageDetail> errorMessages)
        {
            Exception exception = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                if (errorMessages == null)
                    errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();

                IList<ContractScheduleRate> recordToBeInserted = null;
                if (this.IsRecordValidForProcess(contractScheduleRates, ValidationType.Add, ref recordToBeInserted, ref errorMessages, ref validationMessages))
                {
                    DbModel.Contract dbContract = dbContracts?.Count > 0 ? dbContracts.FirstOrDefault() : null; 
                    if (this.IsValidContract(contractNumber, ref dbContract, ref errorMessages))
                    {
                        IList<DbModel.Data> dbExpenceTypes = dbExpense?.Count > 0 ? dbExpense : null;
                        if (this.IsValidChargeType(recordToBeInserted, ref dbExpenceTypes, ref errorMessages))
                        {
                            IList<DbModel.CompanyInspectionTypeChargeRate> companyInspectionTypeChargeRates = null;
                            if (this.IsValidStandardInspectionTypeChargeRate(dbContract.ContractHolderCompanyId, recordToBeInserted, ref companyInspectionTypeChargeRates, ref errorMessages))
                            {
                                //if (this.IsValidContractBaseRateId(recordToBeInserted, ref errorMessages)) // This was needed earlier as we are taking values from dbBaseSchedule object, now the Id is direrctly passed
                                //{
                                if (recordToBeInserted?.Any() == true)
                                {
                                    dbContractSchedules?.ForEach(x =>
                                    {
                                        x.ContractRate = recordToBeInserted.Where(x1 => x.Name == x1.ScheduleName)?.Select(rate => new DbModel.ContractRate
                                        {
                                            ContractScheduleId = rate.ScheduleId,
                                            ExpenseTypeId = dbExpenceTypes.FirstOrDefault(x1 => x1.Name == rate.ChargeType).Id,
                                            Rate = Convert.ToDecimal(rate.ChargeValue),
                                            Description = rate.Description,
                                            IsPrintDescriptionOnInvoice = rate.IsDescriptionPrintedOnInvoice,
                                            FromDate = rate.EffectiveFrom,
                                            ToDate = rate.EffectiveTo,
                                            IsActive = rate.IsActive,
                                            //LastModification = rate.LastModification, //D789 Sync Issue Fix
                                            StandardValue = rate.StandardValue,
                                            //DiscountApplied = rate.DiscountApplied,
                                            DiscountApplied = Convert.ToDecimal(rate.ChargeValue)- Convert.ToDecimal(rate.StandardValue),
                                            //Percentage = rate.Percentage,
                                            Percentage = rate.StandardValue == Decimal.Zero ? Decimal.Zero : (Convert.ToDecimal(rate.ChargeValue) - rate.StandardValue)/ rate.StandardValue*100,
                                            StandardInspectionTypeChargeRateId = companyInspectionTypeChargeRates?.FirstOrDefault(x2 => x2.Id == rate.StandardInspectionTypeChargeRateId)?.Id,
                                            BaseRateId = rate.BaseRateId,
                                            BaseScheduleId = rate.BaseScheduleId,
                                            ModifiedBy = rate.ModifiedBy,
                                            UpdateCount = rate.UpdateCount,
                                        }).ToList();
                                    });

                                    //foreach (var rate in recordToBeInserted)
                                    //{
                                    //    //var dbContractScheduleRateToBeInserted = _mapper.Map<DbModel.ContractRate>(rate);
                                    //    var dbContractScheduleRateToBeInserted =new DbModel.ContractRate();
                                    //    dbContractScheduleRateToBeInserted.Id = 0;
                                    //    dbContractScheduleRateToBeInserted.ContractScheduleId = rate.ScheduleId;
                                    //    dbContractScheduleRateToBeInserted.ExpenseTypeId = dbExpenceTypes.FirstOrDefault(x1 => x1.Name == rate.ChargeType).Id;
                                    //    dbContractScheduleRateToBeInserted.Rate = rate.ChargeValue;
                                    //    dbContractScheduleRateToBeInserted.Description = rate.Description;
                                    //    dbContractScheduleRateToBeInserted.IsPrintDescriptionOnInvoice = rate.IsDescriptionPrintedOnInvoice;
                                    //    dbContractScheduleRateToBeInserted.FromDate = rate.EffectiveFrom;
                                    //    dbContractScheduleRateToBeInserted.ToDate = rate.EffectiveTo;
                                    //    dbContractScheduleRateToBeInserted.IsActive = rate.IsActive;
                                    //    dbContractScheduleRateToBeInserted.LastModification = rate.LastModification;
                                    //    dbContractScheduleRateToBeInserted.StandardValue = rate.StandardValue;
                                    //    dbContractScheduleRateToBeInserted.DiscountApplied = rate.DiscountApplied;
                                    //    dbContractScheduleRateToBeInserted.Percentage = rate.Percentage;
                                    //    dbContractScheduleRateToBeInserted.StandardInspectionTypeChargeRateId = companyInspectionTypeChargeRates?.FirstOrDefault(x2 => x2.Id == rate.StandardInspectionTypeChargeRateId)?.Id;
                                    //    dbContractScheduleRateToBeInserted.BaseRateId = rate.BaseRateId;
                                    //    dbContractScheduleRateToBeInserted.BaseScheduleId = rate.BaseScheduleId;
                                    //    dbContractScheduleRateToBeInserted.ModifiedBy = rate.ModifiedBy;
                                    //    dbContractScheduleRateToBeInserted.UpdateCount = rate.UpdateCount;

                                    //    //Add scheduleRate to schedule
                                    //    dbContractSchedules?.FirstOrDefault(x => x.Name == rate.ScheduleName)?.ContractRate?.Add(dbContractScheduleRateToBeInserted);    //Added Null Check for Defect 789

                                    //}
                                }
                                //}
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractScheduleRates);
            }
        }


        #endregion

        #region Private Exposed Methods

        private Response AddContractScheduleRates(string contractNumber, string ScheduleName, IList<ContractScheduleRate> contractScheduleRates, IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbContractExpense, IList<DbModel.SqlauditModule> dbModule, bool commitChange,ValidationType validationType)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Contract dbContract = dbContracts?.Count > 0 ? dbContracts.FirstOrDefault() : null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _contractScheduleRateRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<ContractScheduleRate> recordToBeInserted = null;
                eventId = contractScheduleRates?.FirstOrDefault()?.EventId;
                if (this.IsRecordValidForProcess(contractScheduleRates, ValidationType.Add, ref recordToBeInserted, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidContract(contractNumber, ref dbContract, ref errorMessages))
                    {
                        IList<DbModel.ContractSchedule> dbContractSchedules = null;
                        if (this.IsValidContractSchedule(dbContract.Id, ScheduleName, recordToBeInserted, ref dbContractSchedules, ref errorMessages))
                        {
                            IList<DbModel.Data> dbExpenceTypes = dbContractExpense?.Count > 0 ? dbContractExpense : null;

                            if (this.IsValidChargeType(recordToBeInserted, ref dbExpenceTypes, ref errorMessages))
                            {
                                IList<DbModel.CompanyInspectionTypeChargeRate> companyInspectionTypeChargeRates = null;

                                if (this.IsValidStandardInspectionTypeChargeRate(dbContract.ContractHolderCompanyId, recordToBeInserted, ref companyInspectionTypeChargeRates, ref errorMessages))
                                {
                                    //if (this.IsValidContractBaseRateId(recordToBeInserted, ref errorMessages)) // Thsi was need earlier as we were not assigning Base ScheduleId dirrectly
                                    //{
                                    //TODO : Need to Check on how Self references datas are inserted 
                                    var dbContractScheduleRateToBeInserted = recordToBeInserted.Select(x =>
                                    new DbModel.ContractRate()
                                    {
                                        ContractScheduleId = dbContractSchedules?.FirstOrDefault(x1 => x1.Name == ScheduleName)?.Id,
                                        ExpenseTypeId = dbExpenceTypes.FirstOrDefault(x1 => x1.Name == x.ChargeType).Id,
                                        Rate = Convert.ToDecimal(x.ChargeValue),
                                        Description = x.Description,
                                        IsPrintDescriptionOnInvoice = x.IsDescriptionPrintedOnInvoice,
                                        FromDate = x.EffectiveFrom,
                                        ToDate = x.EffectiveTo,
                                        IsActive = x.IsActive,
                                        StandardValue = x.StandardValue,
                                        //DiscountApplied = x.DiscountApplied,
                                        DiscountApplied = Convert.ToDecimal(x.ChargeValue) - x.StandardValue,
                                        //Percentage = x.Percentage,
                                        Percentage = x.StandardValue== Decimal.Zero ? Decimal.Zero : (Convert.ToDecimal(x.ChargeValue) - x.StandardValue) / x.StandardValue * 100,
                                        StandardInspectionTypeChargeRateId = companyInspectionTypeChargeRates?.FirstOrDefault(x2 => x2.Id == x.StandardInspectionTypeChargeRateId)?.Id,
                                        BaseRateId = x.BaseRateId,
                                        /*AddScheduletoRF*/
                                        BaseScheduleId = x.BaseScheduleId,
                                        //BaseScheduleId = dbContractSchedules?.FirstOrDefault(x1 => x1.Name == x.BaseScheduleName)?.Id,
                                        ModifiedBy = x.ModifiedBy
                                    }).ToList();

                                    _contractScheduleRateRepository.Add(dbContractScheduleRateToBeInserted);

                                    if (commitChange && !_contractScheduleRateRepository.AutoSave && recordToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                                    {
                                        int value = _contractScheduleRateRepository.ForceSave();
                                        if (value > 0 && validationType == ValidationType.Update)
                                            dbContractScheduleRateToBeInserted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, contractScheduleRates?.FirstOrDefault()?.ActionByUser,
                                                                                                                                    null, ValidationType.Add.ToAuditActionType(),
                                                                                                                                    SqlAuditModuleType.ContractRate,
                                                                                                                                    null,
                                                                                                                                    _mapper.Map<ContractScheduleRate>(x1)
                                                                                                                                     , dbModule));
                                    }
                                    //}
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractScheduleRates);
            }
            finally
            {
                _contractScheduleRateRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response UpdateContractScheduleRates(string contractNumber, string ScheduleName, IList<ContractScheduleRate> contractScheduleRates, IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbContractExpense, IList<DbModel.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Contract dbContract = dbContracts?.Count > 0 ? dbContracts.FirstOrDefault() : null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _contractScheduleRateRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<ContractScheduleRate> recordToBeModified = null;
                eventId = contractScheduleRates?.FirstOrDefault()?.EventId;
                if (this.IsRecordValidForProcess(contractScheduleRates, ValidationType.Update, ref recordToBeModified, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidContract(contractNumber, ref dbContract, ref errorMessages))
                    {
                        IList<DbModel.ContractSchedule> dbContractSchedules = null;
                        if (this.IsValidContractSchedule(dbContract.Id, ScheduleName, recordToBeModified, ref dbContractSchedules, ref errorMessages))
                        {
                            var modRecordContractRatesId = recordToBeModified.Select(x => x.RateId).ToList();
                            var compScheduleRates = _contractScheduleRateRepository.FindBy(x => x.ContractSchedule.Contract.Id == dbContract.Id && x.ContractSchedule.Name == ScheduleName && modRecordContractRatesId.Contains(x.Id));

                            if (this.IsValidContractScheduleRates(recordToBeModified, compScheduleRates.ToList(), ref errorMessages))
                            {
                                IList<DbModel.Data> dbExpenceTypes = dbContractExpense?.Count > 0 ? dbContractExpense : null;

                                if (this.IsValidChargeType(recordToBeModified, ref dbExpenceTypes, ref errorMessages))
                                {
                                    IList<DbModel.CompanyInspectionTypeChargeRate> companyInspectionTypeChargeRates = null;

                                    if (this.IsValidStandardInspectionTypeChargeRate(dbContract.ContractHolderCompanyId, recordToBeModified, ref companyInspectionTypeChargeRates, ref errorMessages))
                                    {
                                        if (this.IsContractScheduleRatesCanBeUpdated(recordToBeModified, compScheduleRates.ToList(), ref errorMessages))
                                        {
                                            IList<DomainModel.ContractScheduleRate> domExistingContractRates = new List<DomainModel.ContractScheduleRate>();
                                            compScheduleRates.ToList().ForEach(x =>
                                              {
                                                  domExistingContractRates.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.ContractScheduleRate>(x)));
                                              });
                                            foreach (var dbContractScheduleRate in compScheduleRates)
                                            {
                                                var contractScheduleRate = recordToBeModified?.FirstOrDefault(x => x.RateId == dbContractScheduleRate.Id);

                                                dbContractScheduleRate.ExpenseTypeId = dbExpenceTypes.FirstOrDefault(x1 => x1.Name == contractScheduleRate.ChargeType).Id;
                                                dbContractScheduleRate.Rate = Convert.ToDecimal(contractScheduleRate.ChargeValue);
                                                dbContractScheduleRate.Description = contractScheduleRate.Description;
                                                dbContractScheduleRate.IsPrintDescriptionOnInvoice = contractScheduleRate.IsDescriptionPrintedOnInvoice;
                                                dbContractScheduleRate.FromDate = contractScheduleRate.EffectiveFrom;
                                                dbContractScheduleRate.ToDate = contractScheduleRate.EffectiveTo;
                                                dbContractScheduleRate.IsActive = contractScheduleRate.IsActive;
                                                dbContractScheduleRate.StandardValue = contractScheduleRate.StandardValue;
                                                //dbContractScheduleRate.DiscountApplied = contractScheduleRate.DiscountApplied;
                                                dbContractScheduleRate.DiscountApplied = Convert.ToDecimal(contractScheduleRate.ChargeValue) - contractScheduleRate.StandardValue;
                                                //dbContractScheduleRate.Percentage = contractScheduleRate.Percentage;
                                                dbContractScheduleRate.Percentage = contractScheduleRate.StandardValue == Decimal.Zero ? Decimal.Zero:(Convert.ToDecimal(contractScheduleRate.ChargeValue) - contractScheduleRate.StandardValue) / contractScheduleRate.StandardValue * 100;
                                                dbContractScheduleRate.StandardInspectionTypeChargeRateId = contractScheduleRate?.StandardInspectionTypeChargeRateId > 0 ? companyInspectionTypeChargeRates?.FirstOrDefault(x2 => x2.Id == contractScheduleRate.StandardInspectionTypeChargeRateId).Id : null; //Changes for ITK - D1508
                                                // dbContractScheduleRate.BaseRateId = contractScheduleRate.BaseRateId;
                                                // dbContractScheduleRate.BaseScheduleId = dbContractSchedules?.FirstOrDefault(x1 => x1.Name == contractScheduleRate.BaseScheduleName).Id;
                                                dbContractScheduleRate.ModifiedBy = contractScheduleRate.ModifiedBy;
                                                dbContractScheduleRate.LastModification = DateTime.UtcNow;
                                                dbContractScheduleRate.UpdateCount = contractScheduleRate.UpdateCount.CalculateUpdateCount();
                                                _contractScheduleRateRepository.Update(dbContractScheduleRate);
                                            }

                                            if (commitChange && !_contractScheduleRateRepository.AutoSave && recordToBeModified?.Count > 0 && errorMessages.Count <= 0)
                                            {
                                                int value = _contractScheduleRateRepository.ForceSave();
                                                if (value > 0)
                                                {

                                                    recordToBeModified?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                                            null,
                                                                                                                            ValidationType.Update.ToAuditActionType(),
                                                                                                                            SqlAuditModuleType.ContractRate,
                                                                                                                            domExistingContractRates?.FirstOrDefault(x2 => x2.RateId == x1.RateId),
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
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractScheduleRates);
            }
            finally
            {
                _contractScheduleRateRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response RemoveContractScheduleRates(string contractNumber, string ScheduleName, IList<ContractScheduleRate> contractScheduleRates, IList<DbModel.Contract> dbContracts, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Contract dbContract = dbContracts?.Count > 0 ? dbContracts.FirstOrDefault() : null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _contractScheduleRateRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<ContractScheduleRate> recordToBeDeleted = null;
                eventId = contractScheduleRates?.FirstOrDefault()?.EventId;

                if (this.IsRecordValidForProcess(contractScheduleRates, ValidationType.Delete, ref recordToBeDeleted, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidContract(contractNumber, ref dbContract, ref errorMessages))
                    {
                        //IList<DbModel.ContractSchedule> dbContractSchedules = null;
                        //if (this.IsValidContractSchedule(dbContract.Id, ScheduleName, recordToBeDeleted, ref dbContractSchedules, ref errorMessages))
                        //{
                            var modRecordContractRatesId = recordToBeDeleted.Select(x => x.RateId).ToList();
                            var dbContractScheduleRates = _contractScheduleRateRepository.FindBy(x => x.ContractSchedule.Contract.Id == dbContract.Id && x.ContractSchedule.Name == ScheduleName && modRecordContractRatesId.Contains(x.Id));

                            //if (this.IsValidContractScheduleRates(recordToBeDeleted, dbContractScheduleRates.ToList(), ref errorMessages))
                            //{
                                //Added for Bug-248
                                if (IsRecordCanBeDelete(dbContractScheduleRates?.ToList(), ref errorMessages))
                                {
                                    var delCont = _contractScheduleRateRepository.DeleteContractRate(recordToBeDeleted.ToList());
                                    if (delCont > 0)
                                    {
                                        contractScheduleRates?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                                  null,
                                                                                                                  ValidationType.Delete.ToAuditActionType(),
                                                                                                                  SqlAuditModuleType.ContractRate,
                                                                                                                  x1,
                                                                                                                   null
                                                                                                                    ));
                                    }
                                }
                            //}
                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractScheduleRates);
            }
            finally
            {
                _contractScheduleRateRepository.AutoSave = true;
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

        private bool IsValidContractSchedule(int contractId, string ScheduleName, IList<ContractScheduleRate> contractRates, ref IList<DbModel.ContractSchedule> contractSchedules, ref List<MessageDetail> errorMessages)
        {
            IList<DbModel.ContractSchedule> dbContractSchedules = null;
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var scheduleNames = contractRates?.Where(x => !string.IsNullOrEmpty(x.BaseScheduleName))?.Select(x => x.BaseScheduleName).ToList();
            scheduleNames.Add(ScheduleName);
            if (scheduleNames.Count > 0)
                dbContractSchedules = this._contractScheduleRepository.FindBy(x => scheduleNames.Contains(x.Name) && x.ContractId == contractId).ToList();

            var contractScheduleNotExists = contractRates.Where(x => !dbContractSchedules.Any(x1 => x1.Name == x.ScheduleName)).ToList();
            contractScheduleNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.ScheduleNameDoesNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ScheduleName)));
            });

            contractSchedules = dbContractSchedules;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidChargeType(IList<ContractScheduleRate> contractRates, ref IList<DbModel.Data> dbExpenceTypes, ref List<MessageDetail> errorMessages)
        {
            IList<DbModel.Data> dbChargeTypes = null;
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            if (dbExpenceTypes == null)
            {
                var chargeTypeNames = contractRates?.Select(x => x.ChargeType).ToList();
                if (chargeTypeNames.Count > 0)
                    dbChargeTypes = this._dataRepository.FindBy(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.ExpenseType) &&
                                                                       chargeTypeNames.Contains(x.Name)).ToList();

                var chargeTypeNotExists = contractRates?.Where(x => !dbChargeTypes.Any(x1 => x1.Name == x.ChargeType)).ToList();
                chargeTypeNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.ContractScheduleRateChargeTypeNotExists.ToId();
                    messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ChargeType)));
                });


                dbExpenceTypes = dbChargeTypes;
            }

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidContractScheduleRates(IList<ContractScheduleRate> contractRates, IList<DbModel.ContractRate> dbContractRates, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = contractRates?.Where(x => !dbContractRates.ToList().Any(x1 => x1.Id == x.RateId)).ToList();

            notMatchedRecords?.ForEach(x =>
            {
                string errorCode = MessageType.ContractScheduleRateIdIsInvalidOrNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.RateId)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidContractBaseRateId(IList<ContractScheduleRate> contractRates, ref List<MessageDetail> errorMessages)
        {

            IList<DbModel.ContractRate> dbContractScheduleRates = null;

            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var baseRateIds = contractRates?.Where(x => x.BaseRateId != null && x.BaseRateId > 0)?.Select(x => x.BaseRateId).ToList();
            if (baseRateIds.Count > 0)
                dbContractScheduleRates = this._contractScheduleRateRepository.FindBy(x => baseRateIds.Contains(x.Id)).ToList();

            var contractScheduleNotExists = contractRates.Where(x => (x.BaseRateId != null && x.BaseRateId > 0) && !dbContractScheduleRates.Any(x1 => x1.Id == x.BaseRateId)).ToList();
            contractScheduleNotExists.ForEach(x =>
            {
                string errorCode = MessageType.ContractScheduleRateBaseRateIdDoesNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.BaseRateId)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        public void StandardInspectionTypeChargeRate(int companyId, IList<ContractScheduleRate> contractRates, ref IList<DbModel.CompanyInspectionTypeChargeRate> companyInspectionTypeChargeRates, ref List<MessageDetail> errorMessages)
        {
            IsValidStandardInspectionTypeChargeRate(companyId, contractRates, ref companyInspectionTypeChargeRates, ref errorMessages);
        }

        private bool IsValidStandardInspectionTypeChargeRate(int companyId, IList<ContractScheduleRate> contractRates, ref IList<DbModel.CompanyInspectionTypeChargeRate> companyInspectionTypeChargeRates, ref List<MessageDetail> errorMessages)
        {
            IList<DbModel.CompanyInspectionTypeChargeRate> dbStandardInspectionTypeChargeRates = null;
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var standardInspectionTypeChargeRates = contractRates?.Where(x => x.StandardInspectionTypeChargeRateId > 0)?.Select(x => x.StandardInspectionTypeChargeRateId).ToList();
            if (standardInspectionTypeChargeRates?.Count > 0)
                dbStandardInspectionTypeChargeRates = this._companyInspectionTypeChargeRateRepository.FindBy(x => standardInspectionTypeChargeRates.Contains(x.Id) && x.CompanyChgSchInspGrpInspectionType.CompanyChgSchInspGroup.CompanyChargeSchedule.CompanyId == companyId).ToList();

            var standardInspectionTypeChargeRateNotExists = contractRates.Where(x => x.StandardInspectionTypeChargeRateId > 0 && !dbStandardInspectionTypeChargeRates.Any(x1 => x1.Id == x.StandardInspectionTypeChargeRateId)).ToList();
            standardInspectionTypeChargeRateNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.ContractScheduleRateStandardInspectionTypeChargeRateNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.StandardInspectionTypeChargeRateId, x.Description)));
            });

            companyInspectionTypeChargeRates = dbStandardInspectionTypeChargeRates;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsContractScheduleRatesCanBeUpdated(IList<ContractScheduleRate> contractRates, IList<DbModel.ContractRate> dbContractRates, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = contractRates?.Where(x => !dbContractRates.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.RateId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.ContractScheduleRateRecordUpdatedAlready.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ChargeType, x.Description)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsRecordValidForProcess(IList<ContractScheduleRate> contractRates, ValidationType validationType, ref IList<ContractScheduleRate> filteredContractRates, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredContractRates = contractRates?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredContractRates = contractRates?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (validationType == ValidationType.Update)
                filteredContractRates = contractRates?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            if (filteredContractRates?.Count <= 0)
                return false;

            return IsContractScheduleRateHasValidSchema(filteredContractRates, validationType, ref errorMessages, ref validationMessages);
        }

        private bool IsRecordCanBeDelete(IList<DbModel.ContractRate> dbContractRates, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            dbContractRates.Where(x => x.VisitTechnicalSpecialistAccountItemConsumable.Count > 0
                                || x.VisitTechnicalSpecialistAccountItemExpense.Count > 0
                                || x.VisitTechnicalSpecialistAccountItemTime.Count > 0
                                || x.VisitTechnicalSpecialistAccountItemTravel.Count > 0
                                || x.TimesheetTechnicalSpecialistAccountItemConsumable.Count > 0
                                || x.TimesheetTechnicalSpecialistAccountItemExpense.Count > 0
                                || x.TimesheetTechnicalSpecialistAccountItemTime.Count > 0
                                || x.TimesheetTechnicalSpecialistAccountItemTravel.Count > 0

                                ).ToList()
          .ForEach(x =>
          {
              string errorCode = MessageType.ContractScheduleRateCannotBeDeleted.ToId();
              messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.Rate, x.Description)));
          });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }
        private bool IsContractScheduleRateHasValidSchema(IList<DomainModel.ContractScheduleRate> models, ValidationType validationType, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {

            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(models), validationType);

            validationResults.ToList().ForEach(x =>
            {
                messages.Add(new ValidationMessage(x.Code, new List<MessageDetail> { new MessageDetail(ModuleType.Contract, x.Code, x.Message) }));
            });

            if (messages?.Count > 0)
            {
                validationMessages.AddRange(messages);
            }
            return validationMessages?.Count <= 0;

        }


        #endregion
    }
}
