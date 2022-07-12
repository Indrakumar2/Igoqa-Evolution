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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.Contract.Core.Services
{
    public class ContractInvoiceReferenceTypeService : IContractInvoiceReferenceTypeService
    {
        private IMapper _mapper = null;
        private IAppLogger<ContractInvoiceReferenceTypeService> _logger = null;
        private IContractInvoiceReferenceTypeRepository _contractInvoiceReferenceRepository = null;
        private IContractRepository _contractRepository = null;
        private IDataRepository _dataRepository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IContractInvoiceReferenceTypeValidationService _validationService = null;
        private IAuditSearchService _auditSearchService = null;

        public ContractInvoiceReferenceTypeService(IMapper mapper, IAppLogger<ContractInvoiceReferenceTypeService> logger, IContractInvoiceReferenceTypeRepository contractInvoiceReferenceRepository,
                                                  IContractRepository contractRepository, IDataRepository dataRepository, IContractInvoiceReferenceTypeValidationService validationService, JObject messages, IAuditSearchService auditSearchService)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._contractInvoiceReferenceRepository = contractInvoiceReferenceRepository;
            this._contractRepository = contractRepository;
            this._dataRepository = dataRepository;
            this._validationService = validationService;
            this._messageDescriptions = messages;
           _auditSearchService = auditSearchService;
        }

        #region Public Exposed Method

        public Response DeleteContractInvoiceReferenceType(string contractNumber, IList<ContractInvoiceReferenceType> deleteModel, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = RemoveContractInvoiceReferenceType(contractNumber, deleteModel, dbContract, dbModule,commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractInvoiceReferenceType(new ContractInvoiceReferenceType() { ContractNumber = contractNumber });
            else
                return result;
        }

        public Response DeleteContractInvoiceReferenceType(string contractNumber, IList<ContractInvoiceReferenceType> deleteModel, IList<DbModel.Contract> dbContract, IList<DbModel.SqlauditModule> dbModule , bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = RemoveContractInvoiceReferenceType(contractNumber, deleteModel, dbContract, dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractInvoiceReferenceType(new ContractInvoiceReferenceType() { ContractNumber = contractNumber });
            else
                return result;
        }

        public Response GetContractInvoiceReferenceType(ContractInvoiceReferenceType searchModel)
        {
            IList<DomainModel.ContractInvoiceReferenceType> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                   new TransactionOptions
                                                   {
                                                       IsolationLevel = IsolationLevel.ReadUncommitted
                                                   }))
                {
                    result = _contractInvoiceReferenceRepository.Search(searchModel);
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

        public Response ModifyContractInvoiceReferenceType(string contractNumber, IList<ContractInvoiceReferenceType> contractInvoiceReferenceTypes, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.Data> dbContractRef = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = this.UpdateContractInvoiceReferenceType(contractNumber, contractInvoiceReferenceTypes, dbContract, dbContractRef, dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractInvoiceReferenceType(new ContractInvoiceReferenceType() { ContractNumber = contractNumber });
            else
                return result;
        }

        public Response ModifyContractInvoiceReferenceType(string contractNumber, IList<ContractInvoiceReferenceType> contractInvoiceReferenceTypes,
                                                           IList<DbModel.Contract> dbContract, IList<DbModel.Data> dbContractRef, IList<DbModel.SqlauditModule> dbModule,
                                                            bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = this.UpdateContractInvoiceReferenceType(contractNumber, contractInvoiceReferenceTypes, dbContract, dbContractRef, dbModule,commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractInvoiceReferenceType(new ContractInvoiceReferenceType() { ContractNumber = contractNumber });
            else
                return result;
        }

        public Response SaveContractInvoiceReferenceType(string contractNumber, IList<ContractInvoiceReferenceType> contractInvoiceReferenceTypes,
                                                        bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Contract> dbContract = null; 
            IList< DbModel.Data > dbContractRef = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = this.AddContractInvoiceReferenceType(contractNumber, contractInvoiceReferenceTypes, dbContract, dbContractRef, dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractInvoiceReferenceType(new ContractInvoiceReferenceType() { ContractNumber = contractNumber });
            else
                return result;
        }

        public Response SaveContractInvoiceReferenceType(string contractNumber, IList<ContractInvoiceReferenceType> contractInvoiceReferenceTypes, 
                                                        IList<DbModel.Contract> dbContract, IList<DbModel.Data> dbContractRef, IList<DbModel.SqlauditModule> dbModule,
                                                        bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = this.AddContractInvoiceReferenceType(contractNumber, contractInvoiceReferenceTypes, dbContract, dbContractRef, dbModule,commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractInvoiceReferenceType(new ContractInvoiceReferenceType() { ContractNumber = contractNumber });
            else
                return result;
        }

        #endregion

        #region Private Exposed Methods

        private Response AddContractInvoiceReferenceType(string contractNumber, IList<ContractInvoiceReferenceType> contractInvoiceReferenceTypes,IList<DbModel.Contract> dbContracts,IList<DbModel.Data> dbContractRef, IList<DbModel.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Contract dbContract = dbContracts?.Count > 0 ? dbContracts.FirstOrDefault() : null; 
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                this._contractInvoiceReferenceRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<ContractInvoiceReferenceType> recordToBeInserted = null;
                eventId = contractInvoiceReferenceTypes?.FirstOrDefault()?.EventId;
                if (this.IsRecordValidForProcess(contractInvoiceReferenceTypes, ValidationType.Add, ref recordToBeInserted, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidContract(contractNumber, ref dbContract, ref errorMessages))
                    {
                        IList<DbModel.Data> dbReferenceTypes = dbContractRef?.Count > 0 ? dbContractRef : null; ;
                        if (this.IsValidReferenceType(recordToBeInserted, ref dbReferenceTypes, ref errorMessages))
                        {
                            if (!this.IsContractInvocieReferenceAlreadyAssociated(dbContract.Id, recordToBeInserted, ValidationType.Add, ref errorMessages))
                            {
                                var dbContractReferenceTypeToBeInserted = recordToBeInserted.Select(x => new DbModel.ContractInvoiceReference()
                                {
                                    ContractId = dbContract.Id,
                                    AssignmentReferenceTypeId = dbReferenceTypes.FirstOrDefault(x1 => x1.Name == x.ReferenceType).Id,
                                    SortOrder = Convert.ToByte(x.DisplayOrder),
                                    IsAssignment = x.IsVisibleToAssignment,
                                    IsVisit = x.IsVisibleToVisit,
                                    IsTimesheet = x.IsVisibleToTimesheet,
                                    ModifiedBy = x.ModifiedBy,
                                }).ToList();

                                this._contractInvoiceReferenceRepository.Add(dbContractReferenceTypeToBeInserted);

                                if (commitChange && !_contractInvoiceReferenceRepository.AutoSave && recordToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                                {
                                    int value = _contractInvoiceReferenceRepository.ForceSave();
                                    if (value > 0)

                                        dbContractReferenceTypeToBeInserted?.ToList().ForEach(x1 => {
                                            var contractInvoiceReference = _mapper.Map<ContractInvoiceReferenceType>(x1);
                                            contractInvoiceReference.ReferenceType = dbReferenceTypes?.ToList()?.FirstOrDefault(referenceType => referenceType.Id == x1.AssignmentReferenceTypeId)?.Name;
                                            _auditSearchService.AuditLog(recordToBeInserted.FirstOrDefault(), ref eventId, recordToBeInserted?.FirstOrDefault()?.ActionByUser,
                                                                        null,
                                                                        ValidationType.Add.ToAuditActionType(),
                                                                        SqlAuditModuleType.ContractReferences,
                                                                        null,
                                                                        contractInvoiceReference,
                                                                        dbModule
                                                                        );
                                        });

                                }
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractInvoiceReferenceTypes);
            }
            finally
            {
                _contractInvoiceReferenceRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response UpdateContractInvoiceReferenceType(string contractNumber, IList<ContractInvoiceReferenceType> contractInvoiceReferenceTypes, IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbContractRef, IList<DbModel.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Contract dbContract = dbContracts?.Count > 0 ? dbContracts.FirstOrDefault() : null; 
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                this._contractInvoiceReferenceRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<ContractInvoiceReferenceType> recordToBeModified = null;
                eventId = contractInvoiceReferenceTypes?.FirstOrDefault()?.EventId;
                if (this.IsRecordValidForProcess(contractInvoiceReferenceTypes, ValidationType.Update, ref recordToBeModified, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidContract(contractNumber, ref dbContract, ref errorMessages))
                    {
                        IList<DbModel.Data> dbReferenceTypes = dbContractRef?.Count > 0 ? dbContractRef : null; 
                        if (this.IsValidReferenceType(recordToBeModified, ref dbReferenceTypes, ref errorMessages))
                        {
                            if (!this.IsContractInvocieReferenceAlreadyAssociated(dbContract.Id, recordToBeModified, ValidationType.Update, ref errorMessages))
                            {
                                var modRecordContractInvoiceReferenceId = recordToBeModified.Select(x => x.ContractInvoiceReferenceTypeId).ToList();
                                var cntrInvoiceReferenceTypes = _contractInvoiceReferenceRepository.FindBy(x => x.ContractId == dbContract.Id && modRecordContractInvoiceReferenceId.Contains(x.Id));

                                if (IsValidContractInvoiceReference(recordToBeModified, cntrInvoiceReferenceTypes.ToList(), ref errorMessages))
                                {
                                    if (this.IsContractInvoiceReferenceCanBeUpdated(recordToBeModified, cntrInvoiceReferenceTypes.ToList(), ref errorMessages))
                                    {
                                        IList<DomainModel.ContractInvoiceReferenceType> domExistingContractRef = new List<DomainModel.ContractInvoiceReferenceType>();
                                        cntrInvoiceReferenceTypes.ToList().ForEach(x =>
                                        {
                                            domExistingContractRef.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.ContractInvoiceReferenceType>(x)));
                                        });
                                        foreach (var dbContractInvReference in cntrInvoiceReferenceTypes)
                                        {
                                            var contractInvoiceReference = recordToBeModified.FirstOrDefault(x => x.ContractInvoiceReferenceTypeId == dbContractInvReference.Id);

                                            dbContractInvReference.AssignmentReferenceTypeId = dbReferenceTypes.FirstOrDefault(x1 => x1.Name == contractInvoiceReference.ReferenceType).Id;
                                            dbContractInvReference.SortOrder = Convert.ToByte(contractInvoiceReference.DisplayOrder);
                                            dbContractInvReference.IsAssignment = contractInvoiceReference.IsVisibleToAssignment;
                                            dbContractInvReference.IsVisit = contractInvoiceReference.IsVisibleToVisit;
                                            dbContractInvReference.IsTimesheet = contractInvoiceReference.IsVisibleToTimesheet;
                                            dbContractInvReference.LastModification = DateTime.UtcNow;
                                            dbContractInvReference.UpdateCount = contractInvoiceReference.UpdateCount.CalculateUpdateCount();
                                            dbContractInvReference.ModifiedBy = contractInvoiceReference.ModifiedBy;

                                            _contractInvoiceReferenceRepository.Update(dbContractInvReference);
                                        }

                                        if (commitChange && recordToBeModified?.Count > 0)
                                        {
                                            int value = _contractInvoiceReferenceRepository.ForceSave();
                                            if (value > 0)
                                               
                                                recordToBeModified?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                                         null,
                                                                                                                       ValidationType.Update.ToAuditActionType(),
                                                                                                                       SqlAuditModuleType.ContractReferences,
                                                                                                                       domExistingContractRef?.FirstOrDefault(x2 => x2.ContractInvoiceReferenceTypeId == x1.ContractInvoiceReferenceTypeId),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractInvoiceReferenceTypes);
            }
            finally
            {
                _contractInvoiceReferenceRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response RemoveContractInvoiceReferenceType(string contractNumber, IList<ContractInvoiceReferenceType> contractInvoiceReferenceTypes, IList<DbModel.Contract> dbContracts, IList<DbModel.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Contract dbContract = dbContracts?.Count > 0 ? dbContracts.FirstOrDefault() : null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _contractInvoiceReferenceRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<ContractInvoiceReferenceType> recordToBedeleted = null;
                eventId = contractInvoiceReferenceTypes?.FirstOrDefault()?.EventId;
                if (this.IsRecordValidForProcess(contractInvoiceReferenceTypes, ValidationType.Delete, ref recordToBedeleted, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidContract(contractNumber, ref dbContract, ref errorMessages))
                    {
                        var modRecordContractReferenceTypeId = recordToBedeleted.Select(x => x.ContractInvoiceReferenceTypeId).ToList();
                        var dbContractInvoiceReferenceTypes = _contractInvoiceReferenceRepository.FindBy(x => x.ContractId == dbContract.Id && modRecordContractReferenceTypeId.Contains(x.Id));

                        if (IsValidContractInvoiceReference(recordToBedeleted, dbContractInvoiceReferenceTypes.ToList(), ref errorMessages))
                        {
                            var delCont = _contractInvoiceReferenceRepository.DeleteInvoiceReference(recordToBedeleted.ToList());
                            if (delCont > 0)
                            {
                                    recordToBedeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(recordToBedeleted.FirstOrDefault(), ref eventId, recordToBedeleted?.FirstOrDefault()?.ActionByUser,
                                                                                                            null,
                                                                                                            ValidationType.Delete.ToAuditActionType(),
                                                                                                            SqlAuditModuleType.ContractReferences,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractInvoiceReferenceTypes);
            }
            finally
            {
                _contractInvoiceReferenceRepository.AutoSave = true;
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

        private bool IsValidReferenceType(IList<ContractInvoiceReferenceType> contractInvoiceReferenceTypes, ref IList<DbModel.Data> dbInvoiceReferenceTypes, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            if (dbInvoiceReferenceTypes == null)
            {
                var referenceTypeNames = contractInvoiceReferenceTypes.Select(x => x.ReferenceType).ToList();

                var dbReferenceTypes = this._dataRepository.FindBy(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.AssignmentReferenceType) &&
                                                                     referenceTypeNames.Contains(x.Name)).ToList();

                var referenceTypeNotExists = contractInvoiceReferenceTypes.Where(x => !dbReferenceTypes.Any(x1 => x1.Name == x.ReferenceType)).ToList();
                referenceTypeNotExists.ForEach(x =>
                {
                    string errorCode = MessageType.ContractInvoiceReferenceTypeNotExists.ToId();
                    messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ReferenceType)));
                });

                dbInvoiceReferenceTypes = dbReferenceTypes;
            }

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsContractInvoiceReferenceCanBeUpdated(IList<ContractInvoiceReferenceType> contractInvoiceReferenceTypes, IList<DbModel.ContractInvoiceReference> dbContractInvoiceReferences, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = contractInvoiceReferenceTypes?.Where(x => !dbContractInvoiceReferences.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.ContractInvoiceReferenceTypeId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.ContractInvoiceReferenceRecordUpdatedAlready.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ReferenceType)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsContractInvocieReferenceAlreadyAssociated(int contractId, IList<ContractInvoiceReferenceType> contractInvoiceReferenceTypes, ValidationType validationType, ref List<MessageDetail> errorMessages)
        {
            IList<DbModel.ContractInvoiceReference> contractInvoiceReferenceExists = null;
            List<MessageDetail> messages = new List<MessageDetail>();

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var contInvReferences = contractInvoiceReferenceTypes?.Select(x => new { x.ReferenceType, x.ContractInvoiceReferenceTypeId }).ToList();
            if (contInvReferences?.Count > 0)
            {

                var filterExpressions = new List<Expression<Func<DbModel.ContractInvoiceReference, bool>>>();
                Expression<Func<DbModel.ContractInvoiceReference, bool>> predicate = null;
                Expression<Func<DbModel.ContractInvoiceReference, bool>> containsExpression = null;
                if (validationType == ValidationType.Add)
                {
                    //contractInvoiceReferenceExists = this._contractInvoiceReferenceRepository?.FindBy(x => x.ContractId == contractId && contInvReferences.Any(x1 => x.AssignmentReferenceType.Name == x1.ReferenceType && x.ContractId == contractId)).ToList();
                    foreach (var cir in contInvReferences)
                    {
                        containsExpression = a => a.ContractId == contractId && a.AssignmentReferenceType.Name == cir.ReferenceType;
                        filterExpressions.Add(containsExpression);
                    }
                }
                else if (validationType == ValidationType.Update)
                {
                    // contractInvoiceReferenceExists = this._contractInvoiceReferenceRepository?.FindBy(x => x.ContractId == contractId && contInvReferences.Any(x1 => x.AssignmentReferenceType.Name == x1.ReferenceType && x.ContractId == contractId && x.Id != x1.ContractInvoiceReferenceTypeId)).ToList();
                    foreach (var cir in contInvReferences)
                    {
                        containsExpression = a => a.ContractId == contractId && a.AssignmentReferenceType.Name == cir.ReferenceType && a.Id != cir.ContractInvoiceReferenceTypeId;
                        filterExpressions.Add(containsExpression);
                    }
                }
                predicate = filterExpressions.CombinePredicates<DbModel.ContractInvoiceReference>(Expression.OrElse);

                contractInvoiceReferenceExists = this._contractInvoiceReferenceRepository?.FindBy(predicate).ToList();

                //if (validationType == ValidationType.Add)
                //    contractInvoiceReferenceExists = this._contractInvoiceReferenceRepository?.FindBy(x => x.ContractId == contractId && contInvReferences.Any(x1 => x.AssignmentReferenceType.Name == x1.ReferenceType && x.ContractId == contractId)).ToList();

                //else if (validationType == ValidationType.Update)
                //    contractInvoiceReferenceExists = this._contractInvoiceReferenceRepository?.FindBy(x => x.ContractId == contractId && contInvReferences.Any(x1 => x.AssignmentReferenceType.Name == x1.ReferenceType && x.ContractId == contractId && x.Id != x1.ContractInvoiceReferenceTypeId)).ToList();

                contractInvoiceReferenceExists?.ToList().ForEach(x =>
                {
                    string errorCode = MessageType.ContractInvoiceReferenceExists.ToId();
                    messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.AssignmentReferenceType.Name)));
                });
            }
                
            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count > 0;
        }

        private bool IsRecordValidForProcess(IList<ContractInvoiceReferenceType> ContractInvoiceReferenceTypes, ValidationType validationType, ref IList<ContractInvoiceReferenceType> filteredContractInvoiceReferenceTypes, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredContractInvoiceReferenceTypes = ContractInvoiceReferenceTypes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredContractInvoiceReferenceTypes = ContractInvoiceReferenceTypes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (validationType == ValidationType.Update)
                filteredContractInvoiceReferenceTypes = ContractInvoiceReferenceTypes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            if (filteredContractInvoiceReferenceTypes?.Count <= 0)
                return false;

            return IsContractInvoiceReferenceHasValidSchema(filteredContractInvoiceReferenceTypes, validationType, ref errorMessages, ref validationMessages);
        }

        private bool IsValidContractInvoiceReference(IList<ContractInvoiceReferenceType> ContractInvoiceReferenceTypes, IList<DbModel.ContractInvoiceReference> dbContractInvoiceReferenceTypes, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = ContractInvoiceReferenceTypes?.Where(x => !dbContractInvoiceReferenceTypes.ToList().Any(x1 => x1.Id == x.ContractInvoiceReferenceTypeId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.ContractInvoiceReferenceTypeIdIsInvalidOrNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractInvoiceReferenceTypeId)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }
        private bool IsContractInvoiceReferenceHasValidSchema(IList<DomainModel.ContractInvoiceReferenceType> models, ValidationType validationType, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
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
