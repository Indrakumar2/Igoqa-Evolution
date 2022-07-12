using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Evolution.Contract.Domain.Interfaces.Data;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Logging.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomModel = Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.Contract.Core.Services
{
    public class ContractDetailService : IContractDetailService
    {
        private readonly IAppLogger<ContractDetailService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly IContractService _contractService = null;
        private readonly IContractExchangeRateService _contractExchangeRateService = null;
        private readonly IContractInvoiceAttachmentService _contractInvoiceAttachmentService = null;
        private readonly IContractInvoiceReferenceTypeService _contractReferenceTypeService = null;
        private readonly IContractScheduleService _contractScheduleService = null;
        private readonly IContractScheduleRateService _contractScheduleRateService = null;
        private readonly IDocumentService _documentService = null;
        private readonly IContractNoteService _contractNoteService = null;
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IContractRepository _contractRepository = null;
        private readonly IAuditLogger _auditLogger = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly JObject _messageDescriptions = null;

        public ContractDetailService(EvolutionSqlDbContext dbContext,
                                        IContractService contractService,
                                        IContractExchangeRateService contractExchangeRateService,
                                        IContractInvoiceAttachmentService contractInvoiceAttachmentService,
                                        IContractInvoiceReferenceTypeService contractReferenceTypeService,
                                        IContractScheduleService contractScheduleService,
                                        IContractScheduleRateService contractScheduleRateService,
                                        IDocumentService documentService,
                                        IContractNoteService contractNoteService,
                                        IContractRepository contractRepository,
                                        IAuditLogger auditLogger,
                                        IAuditSearchService auditSearchService,
                                        JObject messages,
                                        IMapper mapper,
                                        IAppLogger<ContractDetailService> logger)
        {
            this._dbContext = dbContext;
            this._contractService = contractService;
            this._contractExchangeRateService = contractExchangeRateService;
            this._contractInvoiceAttachmentService = contractInvoiceAttachmentService;
            this._contractReferenceTypeService = contractReferenceTypeService;
            this._contractScheduleService = contractScheduleService;
            this._contractScheduleRateService = contractScheduleRateService;
            this._documentService = documentService;
            this._contractNoteService = contractNoteService;
            this._contractRepository = contractRepository;
            this._auditLogger = auditLogger;
            this._messageDescriptions = messages;
            _auditSearchService = auditSearchService;
            _mapper = mapper;
            this._logger = logger;
        }

        public Response SaveContractDetail(DomModel.ContractDetail contractDetail)
        {
            Exception exception = null;
            try
            {
                if (contractDetail != null && contractDetail.ContractInfo != null)
                    return ProcessContractDetail(contractDetail, ValidationType.Add);
                else if (contractDetail == null || contractDetail?.ContractInfo == null)
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messageDescriptions, contractDetail, MessageType.InvalidPayLoad, contractDetail }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractDetail);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response UpdateContractDetail(DomModel.ContractDetail contractDetail)
        {
            Exception exception = null;
            try
            {
                if (contractDetail != null && contractDetail.ContractInfo != null)
                    return ProcessContractDetail(contractDetail, ValidationType.Update);
                else if (contractDetail == null || contractDetail?.ContractInfo == null)
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messageDescriptions, contractDetail, MessageType.InvalidPayLoad, contractDetail }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractDetail);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response DeleteContractDetail(DomModel.ContractDetail contractDetail)
        {
            Exception exception = null;
            Response response = null;
            long? eventId = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            try
            {
                if (contractDetail != null && contractDetail.ContractInfo != null)
                {
                    IList<DbModel.Contract> dbContracts = null;
                    response = this._contractService.ContractValidForDeletion(new List<DomModel.Contract> { contractDetail.ContractInfo }, ref dbContracts);
                    dbModule = _auditSearchService.GetAuditModule(new List<string>() { SqlAuditModuleType.Contract.ToString() });
                    if (response.Code == MessageType.Success.ToId())
                    {
                        //To-Do: Will create helper method get TransactionScope instance based on requirement
                        using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                        //using (var tranScope = new TransactionScope())
                        {
                            var count = _contractRepository.DeleteContract(dbContracts.FirstOrDefault().Id, dbContracts.FirstOrDefault().ContractNumber); //Changes for IGO D947
                            if (count > 0)
                            {
                                //LogEventGeneration logEventGeneration = new LogEventGeneration(_auditLogger);
                                _auditSearchService.AuditLog(contractDetail, ref eventId, contractDetail.ContractInfo.ActionByUser,
                                     "{" + AuditSelectType.Id + ":" + dbContracts.FirstOrDefault().Id + "}${" + AuditSelectType.ContractNumber + ":" + contractDetail.ContractInfo.ContractNumber.Trim()
                                      + "}${" + AuditSelectType.CustomerContractNumber + ":" + contractDetail.ContractInfo.CustomerContractNumber.Trim() + "}"
                                     , SqlAuditActionType.D, SqlAuditModuleType.Contract, contractDetail?.ContractInfo, null, dbModule);

                                //eventId = logEventGeneration.GetEventLogId(eventId,
                                //                            SqlAuditActionType.D,
                                //                            contractDetail.ContractInfo.ActionByUser,
                                //                             "{" + AuditSelectType.Id + ":" + dbContracts.FirstOrDefault().Id + "}${" + AuditSelectType.ContractNumber + ":" + contractDetail.ContractInfo.ContractNumber.Trim()
                                //                                                                            + "}${" + AuditSelectType.CustomerContractNumber + ":" + contractDetail.ContractInfo.CustomerContractNumber.Trim() + "}",
                                //                            SqlAuditModuleType.Contract.ToString());

                                //_auditLogger.LogAuditData((long)eventId, SqlAuditModuleType.Contract, contractDetail.ContractInfo, null, dbModule);
                                tranScope.Complete();
                                response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
                            }
                            else
                                response = new Response(ResponseType.Validation.ToId(), ResponseType.Validation.ToId(), contractDetail);


                        }
                    }
                }
                else if (contractDetail == null || contractDetail?.ContractInfo == null)
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messageDescriptions, contractDetail, MessageType.InvalidPayLoad, contractDetail }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractDetail);
            }

            return response;
        }

        private Response ProcessContractDetail(DomModel.ContractDetail contractDetail, ValidationType validationType)
        {
            string contractNumber = string.Empty;
            int contractId = 0;
            string tempContractNumber = string.Empty;
            bool commitChanges = true;
            Response response = null;
            Exception exception = null;
            DomModel.Contract newDbContract = null;
            long? eventId = null;
            int thresholdCount = 100;
            try
            {
                //var stopWatch = StopWatch.Start();
                if (contractDetail != null)
                {
                    IList<DbModel.Contract> dbContracts = null;
                    IList<DbModel.Data> dbContractRef = null;
                    IList<DbModel.Data> dbContractExpense = null;
                    IList<DbModel.Data> dbCurrency = null;
                    IList<DbModel.SqlauditModule> dbModule = null;
                    IList<DbModel.ContractRate> dbContractRate = null;
                    IList<DbModel.ContractSchedule> dbInsertedContractSchedules = null;
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                    {
                        this._contractRepository.AutoSave = false;
                        if (contractDetail.ContractScheduleRates?.Count > 0)
                            contractDetail.ContractInfo.ContractExpense = contractDetail.ContractScheduleRates?.Where(x => !string.IsNullOrEmpty(x.ChargeType))?.Select(x => x.ChargeType)?.ToList();

                        if (contractDetail.ContractInvoiceReferences?.Count > 0)
                            contractDetail.ContractInfo.ContractRef = contractDetail.ContractInvoiceReferences?.Where(x => !string.IsNullOrEmpty(x.ReferenceType))?.Select(x => x.ReferenceType)?.ToList();

                        if (contractDetail.ContractExchangeRates?.Count > 0)
                        {
                            contractDetail.ContractInfo.ContractCurrency = contractDetail.ContractExchangeRates?.Select(x => x.FromCurrency).Union(contractDetail.ContractExchangeRates.Select(x => x.ToCurrency)).ToList();
                            contractDetail.ContractInfo.ContractExchange = contractDetail.ContractExchangeRates;
                            //    if (validationType== ValidationType.Add)
                            //     contractDetail.ContractInfo.IsFixedExchangeRateUsed = contractDetail.ContractExchangeRates?.Where(x => x.RecordStatus.IsRecordStatusNew())?.Count() > 0 ? true : contractDetail.ContractInfo.IsFixedExchangeRateUsed;
                        }
                        contractDetail.ContractInfo.ContractSaveOrigin = validationType.ToString();

                        response = this.ProcessContractInfo(new List<DomModel.Contract> { contractDetail.ContractInfo }, ref dbContracts, ref dbContractRef, ref dbContractExpense, ref dbCurrency, ref dbModule, ref eventId, commitChanges, validationType);

                        if (response.Code == MessageType.Success.ToId())
                        {
                            if (response.Result != null)
                            {
                                tempContractNumber = contractDetail.ContractInfo.ContractNumber;
                                var newlyAddedDbContracts = response.Result?.Populate<IList<DomModel.Contract>>()?.ToList();
                                foreach (var dbContract in newlyAddedDbContracts)
                                {
                                    contractNumber = dbContract.ContractNumber = dbContract.ContractNumber ?? string.Format("{0}/{1}", contractDetail.ContractInfo.ContractCustomerCode.Trim(), Regex.Match(dbContract.Id.ToString(), @"(.{4})\s*$"));
                                    newDbContract = dbContract;
                                    contractId = (int)dbContract.Id;
                                    AppendEvent(contractDetail, eventId);
                                    response = this.ProcessContractExchangeRates(contractNumber, contractDetail.ContractExchangeRates, dbContracts, dbCurrency, dbModule, commitChanges);
                                    if (response.Code != MessageType.Success.ToId())
                                        return response;

                                    response = this.ProcessContractInvoiceAttachments(contractNumber, contractDetail.ContractInvoiceAttachments, dbContracts, dbModule, commitChanges);
                                    if (response.Code != MessageType.Success.ToId())
                                        return response;

                                    response = this.ProcessContractReferenceTypes(contractNumber, contractDetail.ContractInvoiceReferences, dbContracts, dbContractRef, dbModule, commitChanges);
                                    if (response.Code != MessageType.Success.ToId())
                                        return response;

                                    response = this.ProcessContractSchedules(contractNumber, contractDetail.ContractSchedules, contractDetail.ContractScheduleRates, dbContracts, dbCurrency,
                                                                            dbContractExpense, dbModule, ref dbContractRate, ref dbInsertedContractSchedules, commitChanges, validationType);
                                    if (response.Code != MessageType.Success.ToId())
                                        return response;
                                    else
                                    {   // This will remove rates linked to newly created schedules . To avoid duplicate record insert. 
                                        contractDetail.ContractScheduleRates = RemoveAlreadyInsertedNewContractScheduleRatesFromModel(contractDetail);
                                    }

                                    response = this.ProcessContractScheduleRates(contractNumber, contractDetail.ContractScheduleRates, dbContracts, dbContractExpense, dbModule, commitChanges, validationType);
                                    if (response.Code != MessageType.Success.ToId())
                                        return response;

                                    response = this.ProcessContractDocument(contractNumber, contractDetail.ContractDocuments, dbModule, commitChanges, contractDetail, validationType.ToAuditActionType(), ref eventId, tempContractNumber);
                                    if (response.Code != MessageType.Success.ToId())
                                        return response;

                                    response = this.ProcessContractNote(contractNumber, contractDetail.ContractNotes, dbContracts, dbModule, commitChanges);
                                    if (response.Code != MessageType.Success.ToId())
                                        return response;

                                    if (response != null && response.Code == MessageType.Success.ToId())
                                    {
                                        if (contractDetail.ContractSchedules?.Count > thresholdCount)
                                            _contractScheduleService.AddRates(dbContractRate);
                                        tranScope.Complete();
                                    }
                                }
                            }
                        }
                        else
                            return response;
                    }
                    if (dbContracts?.Any() == true)
                    {
                        if (validationType == ValidationType.Add)
                        {
                            UpdateContractNumber(dbContracts?.ToList());
                            _contractScheduleService.AuditSchedules(contractDetail.ContractInfo.EventId, contractId, contractNumber, dbInsertedContractSchedules?.ToList(), dbModule, dbContractExpense, contractDetail.ContractSchedules?.Count > thresholdCount);
                        }
                    }
                }
                //StopWatch.Stop(stopWatch);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractDetail);
            }
            finally
            {
                this._contractRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, newDbContract, exception);
        }

        private Response ProcessContractInfo(IList<DomModel.Contract> contracts, ref IList<DbModel.Contract> dbContracts, ref IList<DbModel.Data> dbContractRef, ref IList<DbModel.Data> dbContractExpense, ref IList<DbModel.Data> dbCurrency, ref IList<DbModel.SqlauditModule> dbModule, ref long? eventId, bool commitChanges, ValidationType validationType)
        {
            Exception exception = null;
            try
            {
                if (contracts != null)
                {
                    if (validationType == ValidationType.Delete)
                        return this._contractService.DeleteContract(contracts, dbModule, ref eventId, commitChanges);
                    else if (validationType == ValidationType.Add)
                        return this._contractService.SaveContract(contracts, ref dbContracts, ref dbContractRef, ref dbContractExpense, ref dbCurrency, ref dbModule, ref eventId, commitChanges);
                    else if (validationType == ValidationType.Update)
                        return this._contractService.ModifyContract(contracts, ref dbContracts, ref dbContractRef, ref dbContractExpense, ref dbCurrency, ref dbModule, ref eventId, commitChanges);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contracts);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private void UpdateContractNumber(List<DbModel.Contract> dbContractNumberToBeUpdated)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            _contractRepository.AutoSave = true;
            try
            {
                dbContractNumberToBeUpdated?.ToList().ForEach(x =>
                {
                    _contractRepository.Update(x, c => x.ContractNumber);
                });
            }
            catch (Exception ex)
            {
                string errorCode = MessageType.ContractNumberNotGenerated.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), ex.Message)));
            }
        }

        private Response ProcessContractExchangeRates(string contractNumber, IList<DomModel.ContractExchangeRate> contractExchangeRates, IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbCurrency, IList<DbModel.SqlauditModule> dbModule, bool commitChanges)
        {
            Response response = null;
            Exception exception = null;
            try
            {
                if (contractExchangeRates != null && contractExchangeRates.Count > 0)
                {

                    response = this._contractExchangeRateService.DeleteContractExchangeRate(contractNumber, contractExchangeRates, dbContracts, dbModule, false, commitChanges);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        response = this._contractExchangeRateService.SaveContractExchangeRate(contractNumber, contractExchangeRates, dbContracts, dbCurrency, dbModule, false, commitChanges);
                        if (response.Code == MessageType.Success.ToId())
                        {
                            response = this._contractExchangeRateService.ModifyContractExchangeRate(contractNumber, contractExchangeRates, dbContracts, dbCurrency, dbModule, commitChanges);
                        }
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractExchangeRates);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessContractInvoiceAttachments(string contractNumber, IList<DomModel.ContractInvoiceAttachment> contractInvoiceAttachments, IList<DbModel.Contract> dbContracts, IList<DbModel.SqlauditModule> dbModule, bool commitChanges)
        {
            Response response = null;
            Exception exception = null;
            try
            {
                if (contractInvoiceAttachments != null && contractInvoiceAttachments.Count > 0)
                {
                    response = this._contractInvoiceAttachmentService.DeleteContractInvoiceAttachment(contractNumber, contractInvoiceAttachments, dbContracts, dbModule, commitChanges);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        response = this._contractInvoiceAttachmentService.SaveContractInvoiceAttachment(contractNumber, contractInvoiceAttachments, dbContracts, dbModule, commitChanges);
                        if (response.Code == MessageType.Success.ToId())
                        {
                            response = this._contractInvoiceAttachmentService.ModifyContractInvoiceAttachment(contractNumber, contractInvoiceAttachments, dbContracts, dbModule, commitChanges);
                        }
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractInvoiceAttachments);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessContractReferenceTypes(string contractNumber, IList<DomModel.ContractInvoiceReferenceType> contractInvoiceReferenceTypes, IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbCotractRef, IList<DbModel.SqlauditModule> dbModule, bool commitChanges)
        {
            Response response = null;
            Exception exception = null;
            try
            {
                if (contractInvoiceReferenceTypes != null && contractInvoiceReferenceTypes.Count > 0)
                {
                    response = this._contractReferenceTypeService.DeleteContractInvoiceReferenceType(contractNumber, contractInvoiceReferenceTypes, dbContracts, dbModule, commitChanges);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        response = this._contractReferenceTypeService.SaveContractInvoiceReferenceType(contractNumber, contractInvoiceReferenceTypes, dbContracts, dbCotractRef, dbModule, commitChanges);
                        if (response.Code == MessageType.Success.ToId())
                        {
                            response = this._contractReferenceTypeService.ModifyContractInvoiceReferenceType(contractNumber, contractInvoiceReferenceTypes, dbContracts, dbCotractRef, dbModule, commitChanges);
                        }
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractInvoiceReferenceTypes);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessContractSchedules(string contractNumber, IList<DomModel.ContractSchedule> contractSchedules, IList<DomModel.ContractScheduleRate> contractScheduleRates,
                                                IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbContractExpense, IList<DbModel.Data> dbCurrency,
                                                IList<DbModel.SqlauditModule> dbModule, ref IList<DbModel.ContractRate> dbContractRate,
                                                ref IList<DbModel.ContractSchedule> dbInsertedContractSchedules, bool commitChanges,
                                                ValidationType validationType)
        {
            Response response = null;
            Exception exception = null;
            try
            {
                //Removing schedule rates before deleting schedule.
                if (contractScheduleRates?.Count > 0)
                {
                    response = this.RemoveContractScheduleRates(contractNumber, contractScheduleRates, commitChanges);
                    if (response.Code != MessageType.Success.ToId())
                        return response;
                }

                if (contractSchedules != null && contractSchedules.Count > 0)
                {
                    response = this._contractScheduleService.DeleteContractSchedule(contractNumber, contractSchedules, dbContracts, dbModule, commitChanges);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        response = this._contractScheduleService.SaveContractScheduleAndScheduleRate(contractNumber, contractSchedules, contractScheduleRates, dbContracts, dbContractExpense, dbCurrency, dbModule, ref dbContractRate, ref dbInsertedContractSchedules, validationType, commitChanges);
                        if (response.Code == MessageType.Success.ToId())
                        {
                            response = this._contractScheduleService.ModifyContractSchedule(contractNumber, contractSchedules, dbContracts, dbCurrency, dbModule, commitChanges);
                        }
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractSchedules);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessContractScheduleRates(string contractNumber, IList<DomModel.ContractScheduleRate> contractScheduleRates, IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbContractExpense, IList<DbModel.SqlauditModule> dbModule, bool commitChanges,ValidationType validationType)
        {
            Response response = null;
            Exception exception = null;
            try
            {
                if (contractScheduleRates != null && contractScheduleRates.Count > 0)
                {
                    var groupByScheduleName = contractScheduleRates.GroupBy(x => x.ScheduleName);
                    if (groupByScheduleName.Count() > 0)
                    {
                        foreach (var scheduleNameGroup in groupByScheduleName)
                        {
                            response = this._contractScheduleRateService.SaveContractScheduleRate(contractNumber, scheduleNameGroup.Key, scheduleNameGroup.Select(x => x).ToList(), dbContracts, dbContractExpense, dbModule,validationType, commitChanges);
                            if (response.Code == MessageType.Success.ToId())
                            {
                                response = this._contractScheduleRateService.ModifyContractScheduleRate(contractNumber, scheduleNameGroup.Key, scheduleNameGroup.Select(x => x).ToList(), dbContracts, dbContractExpense, dbModule, commitChanges);
                                if (response.Code != MessageType.Success.ToId())
                                    break;
                            }
                            else
                                break;
                        }
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractScheduleRates);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessContractDocument(string contractNumber, IList<ModuleDocument> contractDocuments, IList<DbModel.SqlauditModule> dbModule, bool commitChanges,
            DomModel.ContractDetail contractDetail, SqlAuditActionType sqlAuditActionType, ref long? eventId, string tempContractNumber = null)
        {
            Response response = null;
            Exception exception = null;
            List<DbModel.Document> dbDocuments = null;
            try
            {
                if (contractDocuments != null && contractDocuments.Count > 0)
                {
                    contractDocuments?.ToList()?.ForEach(x => { x.ModuleRefCode = contractNumber; });
                    contractDetail.ContractDocuments = contractDocuments;
                    var auditContractDetails = ObjectExtension.Clone(contractDetail);

                    if (contractDocuments.Any(x => x.RecordStatus.IsRecordStatusDeleted()))
                        response = this._documentService.Delete(contractDocuments, commitChanges);

                    if (contractDocuments.Any(x => x.RecordStatus.IsRecordStatusNew()))
                    {
                        response = this._documentService.Save(contractDocuments, ref dbDocuments, commitChanges);
                        auditContractDetails.ContractDocuments = contractDocuments.Where(x => x.RecordStatus.IsRecordStatusNew()).Select(x =>
                        {
                            x.CreatedOn = dbDocuments?.FirstOrDefault(y => y.DocumentUniqueName == x.DocumentUniqueName)?.CreatedDate;
                            return x;
                        }).ToList(); // audit create date issue fix
                    }

                    if (contractDocuments.Any(x => x.RecordStatus.IsRecordStatusModified()))
                        response = this._documentService.Modify(contractDocuments, ref dbDocuments, commitChanges);
                     
                    if (response.Code == MessageType.Success.ToId())
                    {
                        DocumentAudit(auditContractDetails.ContractDocuments, sqlAuditActionType, auditContractDetails, ref eventId, ref dbDocuments, dbModule);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractDocuments);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private void DocumentAudit(IList<ModuleDocument> contractDocuments, SqlAuditActionType sqlAuditActionType, DomModel.ContractDetail contractDetail, ref long? eventId, ref List<DbModel.Document> dbDocuments, IList<DbModel.SqlauditModule> dbModule)
        {
            //For Document Audit
            if (contractDocuments.Count > 0)
            {
                object newData;
                object oldData;
                var newDocument = contractDocuments?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
                var modifiedDocument = contractDocuments?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
                var deletedDocument = contractDocuments?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();
                if (newDocument.Count > 0)
                {
                    newData = newDocument;
                    _auditSearchService.AuditLog(contractDetail, ref eventId, contractDetail?.ContractInfo.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.ContractDocument, null, newData, dbModule);
                }
                if (modifiedDocument.Count > 0)
                {
                    newData = modifiedDocument?.OrderBy(x => x.Id)?.ToList();
                    oldData = (dbDocuments != null && dbDocuments.Count > 0) ? _mapper.Map<List<ModuleDocument>>(dbDocuments)?.OrderBy(x => x.Id)?.ToList() : null;
                    _auditSearchService.AuditLog(contractDetail, ref eventId, contractDetail?.ContractInfo.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.ContractDocument, oldData, newData, dbModule);
                }
                if (deletedDocument.Count > 0)
                {
                    oldData = deletedDocument;
                    _auditSearchService.AuditLog(contractDetail, ref eventId, contractDetail?.ContractInfo.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.ContractDocument, oldData, null, dbModule);
                }
            }
        }

        private Response ProcessContractNote(string contractNumber, IList<DomModel.ContractNote> contractNotes, IList<DbModel.Contract> dbContracts, IList<DbModel.SqlauditModule> dbModule, bool commitChanges)
        {
            Exception exception = null;
            try
            {
                if (contractNotes != null && contractNotes.Count > 0)
                {
                    var response = this._contractNoteService.DeleteContractNote(contractNumber, contractNotes, commitChanges);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        response = this._contractNoteService.SaveContractNote(contractNumber, contractNotes, dbContracts, dbModule, commitChanges);
                        if (response.Code == MessageType.Success.ToId())        //D661 issue 8 
                            response = this._contractNoteService.ModifyContractNote(contractNumber, contractNotes, commitChanges);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractNotes);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response RemoveContractScheduleRates(string contractNumber, IList<DomModel.ContractScheduleRate> contractScheduleRates, bool commitChange)
        {
            Response response = null;
            var groupByScheduleName = contractScheduleRates?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).GroupBy(x => x.ScheduleName);
            if (groupByScheduleName.Count() > 0)
            {
                foreach (var ScheduleNameGroup in groupByScheduleName)
                {
                    response = this._contractScheduleRateService.DeleteContractScheduleRate(contractNumber, ScheduleNameGroup.Key, ScheduleNameGroup.Select(x => x).ToList(), commitChange);
                    if (response.Code != MessageType.Success.ToId())
                        break;
                }
            }
            return response ?? new Response().ToPopulate(ResponseType.Success);
        }

        private IList<DomModel.ContractScheduleRate> RemoveAlreadyInsertedNewContractScheduleRatesFromModel(DomModel.ContractDetail contractDetail)
        {
            List<DomModel.ContractScheduleRate> scheduleRates = contractDetail?.ContractScheduleRates?.ToList();
            var NewSchedules = contractDetail?.ContractSchedules?.Where(x => x.RecordStatus.IsRecordStatusNew());

            if (NewSchedules?.Any() == true)
            {
                foreach (var schedule in NewSchedules)
                {
                    scheduleRates?.RemoveAll(x => x.ScheduleName == schedule.ScheduleName);
                }
            }
            return scheduleRates;
        }

        private void RollbackTransaction()
        {
            if (_dbContext.Database.CurrentTransaction != null)
                _dbContext.Database.RollbackTransaction();
        }


        private void AppendEvent(DomModel.ContractDetail contractDetail,
                                 long? eventId)
        {
            ObjectExtension.SetPropertyValue(contractDetail.ContractInfo, "EventId", eventId);
            ObjectExtension.SetPropertyValue(contractDetail.ContractExchangeRates, "EventId", eventId);
            ObjectExtension.SetPropertyValue(contractDetail.ContractInvoiceAttachments, "EventId", eventId);
            ObjectExtension.SetPropertyValue(contractDetail.ContractInvoiceReferences, "EventId", eventId);
            ObjectExtension.SetPropertyValue(contractDetail.ContractSchedules, "EventId", eventId);
            ObjectExtension.SetPropertyValue(contractDetail.ContractScheduleRates, "EventId", eventId);
            ObjectExtension.SetPropertyValue(contractDetail.ContractNotes, "EventId", eventId);
            ObjectExtension.SetPropertyValue(contractDetail.ContractDocuments, "EventId", eventId);
        }
    }
}