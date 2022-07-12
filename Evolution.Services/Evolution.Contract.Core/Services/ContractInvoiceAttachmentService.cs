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
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
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
    public class ContractInvoiceAttachmentService : IContractInvoiceAttachmentService
    {
        private readonly IMapper _mapper = null;
        private IAppLogger<ContractInvoiceAttachmentService> _logger = null;
        private IContractInvoiceAttachmentRepository _contractInvAttachmentRepository = null;
        private IContractRepository _contractRepository = null;
        private IModuleDocumentTypeRepository _documentTypeRepository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IContractInvoiceAttachmentValidationService _validationService = null;
        private readonly IAuditSearchService _auditSearchService = null;

        public ContractInvoiceAttachmentService(IMapper mapper, IAppLogger<ContractInvoiceAttachmentService> logger, IContractInvoiceAttachmentRepository contractInvAttachmentRepository, IContractRepository contractRepository,
                                                IModuleDocumentTypeRepository documentTypeRepository, IContractInvoiceAttachmentValidationService validationService, JObject messages, IAuditSearchService auditSearchService)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._contractInvAttachmentRepository = contractInvAttachmentRepository;
            this._contractRepository = contractRepository;
            this._documentTypeRepository = documentTypeRepository;
            this._validationService = validationService;

            this._messageDescriptions = messages;
            _auditSearchService = auditSearchService;
        }

        #region Public Exposed Method

        public Response DeleteContractInvoiceAttachment(string contractNumber, IList<ContractInvoiceAttachment> deleteModel, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Contract> dbContracts = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = this.RemoveContractInvoiceAttachment(contractNumber, deleteModel, dbContracts, dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractInvoiceAttachment(new ContractInvoiceAttachment() { ContractNumber = contractNumber });
            else
                return result;
        }
        public Response DeleteContractInvoiceAttachment(string contractNumber, IList<ContractInvoiceAttachment> deleteModel, IList<DbModel.Contract> dbContracts, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = this.RemoveContractInvoiceAttachment(contractNumber, deleteModel, dbContracts, dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractInvoiceAttachment(new ContractInvoiceAttachment() { ContractNumber = contractNumber });
            else
                return result;
        }

        public Response GetContractInvoiceAttachment(ContractInvoiceAttachment searchModel)
        {
            IList<DomainModel.ContractInvoiceAttachment> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                   new TransactionOptions
                                                   {
                                                       IsolationLevel = IsolationLevel.ReadUncommitted
                                                   }))
                {
                    result = _contractInvAttachmentRepository.Search(searchModel);
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
        public Response ModifyContractInvoiceAttachment(string contractNumber, IList<ContractInvoiceAttachment> contractInvoiceAttachments, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Contract> dbContracts = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = this.UpdateContractInvoiceAttachment(contractNumber, contractInvoiceAttachments, dbContracts, dbModule,commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractInvoiceAttachment(new ContractInvoiceAttachment() { ContractNumber = contractNumber });
            else
                return result;
        }
        public Response ModifyContractInvoiceAttachment(string contractNumber, IList<ContractInvoiceAttachment> contractInvoiceAttachments, IList<DbModel.Contract> dbContracts, IList<DbModel.SqlauditModule> dbModule,bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = this.UpdateContractInvoiceAttachment(contractNumber, contractInvoiceAttachments, dbContracts, dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractInvoiceAttachment(new ContractInvoiceAttachment() { ContractNumber = contractNumber });
            else
                return result;
        }
        public Response SaveContractInvoiceAttachment(string contractNumber, IList<ContractInvoiceAttachment> contractInvoiceAttachments, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Contract> dbContracts = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = this.AddContractInvoiceAttachment(contractNumber, contractInvoiceAttachments, dbContracts, dbModule,commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractInvoiceAttachment(new ContractInvoiceAttachment() { ContractNumber = contractNumber });
            else
                return result;
        }
        public Response SaveContractInvoiceAttachment(string contractNumber, IList<ContractInvoiceAttachment> contractInvoiceAttachments, IList<DbModel.Contract> dbContracts, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = this.AddContractInvoiceAttachment(contractNumber, contractInvoiceAttachments, dbContracts, dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractInvoiceAttachment(new ContractInvoiceAttachment() { ContractNumber = contractNumber });
            else
                return result;
        }

        #endregion

        #region Private Exposed Methods

        private Response AddContractInvoiceAttachment(string contractNumber, IList<ContractInvoiceAttachment> contractInvoiceAttachments, IList<DbModel.Contract> dbContracts, IList<DbModel.SqlauditModule> dbModule,bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Contract dbContract = dbContracts?.Count > 0 ? dbContracts.FirstOrDefault() : null; ;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _contractInvAttachmentRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<ContractInvoiceAttachment> recordToBeInserted = null;
                eventId = contractInvoiceAttachments?.FirstOrDefault()?.EventId;
                if (this.IsRecordValidForProcess(contractInvoiceAttachments, ValidationType.Add, ref recordToBeInserted, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidContract(contractNumber, ref dbContract, ref errorMessages))
                    {
                        IList<DbModel.ModuleDocumentType> dbDocumentTypes = null;
                        if (this.IsValidDocumentType(recordToBeInserted, ref dbDocumentTypes, ref errorMessages))
                        {
                            if (!this.IsContractInvoiceAttachmentAlreadyAssociated(dbContract.Id, recordToBeInserted, ValidationType.Add, ref errorMessages))
                            {
                                var dbContractInvAttachmentToBeInserted = recordToBeInserted.Select(x => new DbModel.ContractInvoiceAttachment()
                                {
                                    ContractId = dbContract.Id,
                                    DocumentTypeId = dbDocumentTypes.FirstOrDefault(x1 => x1.DocumentType.Name == x.DocumentType).DocumentTypeId,
                                    ModifiedBy = x.ModifiedBy,
                                }).ToList();

                                _contractInvAttachmentRepository.Add(dbContractInvAttachmentToBeInserted);

                                if (commitChange && !_contractInvAttachmentRepository.AutoSave && recordToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                                {
                                    int value = _contractInvAttachmentRepository.ForceSave();
                                    if (value > 0)

                                        dbContractInvAttachmentToBeInserted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(recordToBeInserted?.FirstOrDefault(), ref eventId, recordToBeInserted?.FirstOrDefault()?.ActionByUser,
                                                                                                null,
                                                                                                 ValidationType.Add.ToAuditActionType(),
                                                                                                 SqlAuditModuleType.ContractInvoiceAttachment,
                                                                                                  null,
                                                                                                   _mapper.Map<ContractInvoiceAttachment>(x1),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractInvoiceAttachments);
            }
            finally
            {
                _contractInvAttachmentRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response UpdateContractInvoiceAttachment(string contractNumber, IList<ContractInvoiceAttachment> contractInvoiceAttachments, IList<DbModel.Contract> dbContracts, IList<DbModel.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Contract dbContract = dbContracts?.Count > 0 ? dbContracts.FirstOrDefault() : null; ;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _contractInvAttachmentRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<ContractInvoiceAttachment> recordToBeupdated = null;
                eventId = contractInvoiceAttachments?.FirstOrDefault()?.EventId;

                if (this.IsRecordValidForProcess(contractInvoiceAttachments, ValidationType.Update, ref recordToBeupdated, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidContract(contractNumber, ref dbContract, ref errorMessages))
                    {
                        IList<DbModel.ModuleDocumentType> dbDocumentTypes = null;

                        if (this.IsValidDocumentType(recordToBeupdated, ref dbDocumentTypes, ref errorMessages))
                        {
                            if (!this.IsContractInvoiceAttachmentAlreadyAssociated(dbContract.Id, recordToBeupdated, ValidationType.Update, ref errorMessages))
                            {
                                var modRecordContractInvoiceAttachmentId = recordToBeupdated.Select(x => x.ContractInvoiceAttachmentId).ToList();
                                var compInvoiceAttachments = _contractInvAttachmentRepository.FindBy(x => x.ContractId == dbContract.Id && modRecordContractInvoiceAttachmentId.Contains(x.Id));

                                if (IsValidContractInvoiceAttachment(recordToBeupdated, compInvoiceAttachments.ToList(), ref errorMessages))
                                {
                                    if (this.IsContractInvoiceAttachmentCanBeUpdated(recordToBeupdated, compInvoiceAttachments.ToList(), ref errorMessages))
                                    {
                                        IList<DomainModel.ContractInvoiceAttachment> domExistingContractAttachments = new List<DomainModel.ContractInvoiceAttachment>();
                                        compInvoiceAttachments.ToList().ForEach(x =>
                                        {
                                            domExistingContractAttachments.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.ContractInvoiceAttachment>(x)));
                                        });
                                        foreach (var dbContractInvAttachment in compInvoiceAttachments)
                                        {
                                            var contractInvoiceAttachment = recordToBeupdated.FirstOrDefault(x => x.ContractInvoiceAttachmentId == dbContractInvAttachment.Id);

                                            dbContractInvAttachment.DocumentTypeId = dbDocumentTypes.FirstOrDefault(x1 => x1.DocumentType.Name == contractInvoiceAttachment.DocumentType).DocumentTypeId;
                                            dbContractInvAttachment.LastModification = DateTime.UtcNow;
                                            dbContractInvAttachment.UpdateCount = contractInvoiceAttachment.UpdateCount.CalculateUpdateCount();
                                            dbContractInvAttachment.ModifiedBy = contractInvoiceAttachment.ModifiedBy;
                                            _contractInvAttachmentRepository.Update(dbContractInvAttachment);
                                        }

                                        if (commitChange && recordToBeupdated?.Count > 0)
                                        {
                                            int value = _contractInvAttachmentRepository.ForceSave();
                                            if (value > 0)
                                            {
                                                recordToBeupdated?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                                       null,
                                                                                                                       ValidationType.Update.ToAuditActionType(),
                                                                                                                        SqlAuditModuleType.ContractInvoiceAttachment,
                                                                                                                        domExistingContractAttachments?.FirstOrDefault(x2 => x2.ContractInvoiceAttachmentId == x1.ContractInvoiceAttachmentId),
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

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractInvoiceAttachments);
            }
            finally
            {
                _contractInvAttachmentRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response RemoveContractInvoiceAttachment(string contractNumber, IList<ContractInvoiceAttachment> deleteModel, IList<DbModel.Contract> dbContracts, IList<DbModel.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Contract dbContract = dbContracts?.Count > 0 ? dbContracts.FirstOrDefault() : null; ;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _contractInvAttachmentRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<ContractInvoiceAttachment> recordToBedeleted = null;
                eventId = deleteModel?.FirstOrDefault()?.EventId;

                if (this.IsRecordValidForProcess(deleteModel, ValidationType.Delete, ref recordToBedeleted, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidContract(contractNumber, ref dbContract, ref errorMessages))
                    {
                        var modRecordContractInvoiceAttachmentId = recordToBedeleted.Select(x => x.ContractInvoiceAttachmentId).ToList();
                        var dbContractInvoiceAttachments = _contractInvAttachmentRepository.FindBy(x => x.ContractId == dbContract.Id && modRecordContractInvoiceAttachmentId.Contains(x.Id));

                        if (IsValidContractInvoiceAttachment(recordToBedeleted, dbContractInvoiceAttachments.ToList(), ref errorMessages))
                        {
                            var delCont = _contractInvAttachmentRepository.DeleteInvoiceAttachment(recordToBedeleted.ToList());
                            if (delCont > 0)
                            {

                                recordToBedeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(recordToBedeleted.FirstOrDefault(), ref eventId, recordToBedeleted?.FirstOrDefault()?.ActionByUser,
                                                                                                       null,
                                                                                                       ValidationType.Delete.ToAuditActionType(),
                                                                                                      SqlAuditModuleType.ContractInvoiceAttachment,
                                                                                                        x1,
                                                                                                         null,
                                                                                                         dbModule
                                                                                                        ));
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), deleteModel);
            }
            finally
            {
                _contractInvAttachmentRepository.AutoSave = true;
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

        private bool IsValidDocumentType(IList<ContractInvoiceAttachment> contractInvoiceAttachments, ref IList<DbModel.ModuleDocumentType> dbDocumentTypes, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var documentTypeNames = contractInvoiceAttachments.Select(x => x.DocumentType).ToList();

            var dbDocTypes = this._documentTypeRepository.FindBy(x => documentTypeNames.Contains(x.DocumentType.Name)).ToList();

            var documentTypeNotExists = contractInvoiceAttachments.Where(x => !dbDocTypes.Any(x1 => x1.DocumentType.Name == x.DocumentType)).ToList();
            documentTypeNotExists.ForEach(x =>
            {
                string errorCode = MessageType.DocumentTypeNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.DocumentType)));
            });

            dbDocumentTypes = dbDocTypes;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsContractInvoiceAttachmentCanBeUpdated(IList<ContractInvoiceAttachment> contractInvoiceAttachments, IList<DbModel.ContractInvoiceAttachment> dbContractInvoiceAttachments, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = contractInvoiceAttachments?.Where(x => !dbContractInvoiceAttachments.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.ContractInvoiceAttachmentId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.ContractInvoiceAttachementRecordUpdatedAlready.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.DocumentType)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsContractInvoiceAttachmentAlreadyAssociated(int contractId, IList<ContractInvoiceAttachment> contractInvoiceAttachments, ValidationType validationType, ref List<MessageDetail> errorMessages)
        {
            IList<DbModel.ContractInvoiceAttachment> contractInvoiceAttachmentExists = null;
            List<MessageDetail> messages = new List<MessageDetail>();

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var contInvAttachments = contractInvoiceAttachments?.Select(x => new { x.ContractNumber, x.DocumentType, x.ContractInvoiceAttachmentId }).ToList();
            if (contInvAttachments?.Count > 0)
            {
                var filterExpressions = new List<Expression<Func<DbModel.ContractInvoiceAttachment, bool>>>();
                Expression<Func<DbModel.ContractInvoiceAttachment, bool>> predicate = null;
                Expression<Func<DbModel.ContractInvoiceAttachment, bool>> containsExpression = null;
                if (validationType == ValidationType.Add)
                {
                    // contractInvoiceAttachmentExists = this._contractInvAttachmentRepository?.FindBy(x => x.ContractId == contractId && contInvAttachments.Any(x1 => x.DocumentType.Name == x1.DocumentType && x.ContractId == contractId)).ToList();
                    foreach (var cia in contInvAttachments)
                    {
                        containsExpression = a => a.ContractId == contractId && a.DocumentType.Name == cia.DocumentType;
                        filterExpressions.Add(containsExpression);
                    }
                }
                else if (validationType == ValidationType.Update)
                {
                    // contractInvoiceAttachmentExists = this._contractInvAttachmentRepository?.FindBy(x => x.ContractId == contractId && contInvAttachments.Any(x1 => x.DocumentType.Name == x1.DocumentType && x.ContractId == contractId && x.Id != x1.ContractInvoiceAttachmentId)).ToList();
                    foreach (var cia in contInvAttachments)
                    {
                        containsExpression = a => a.ContractId == contractId && a.DocumentType.Name == cia.DocumentType && a.Id != cia.ContractInvoiceAttachmentId;
                        filterExpressions.Add(containsExpression);
                    }
                }
                predicate = filterExpressions.CombinePredicates<DbModel.ContractInvoiceAttachment>(Expression.OrElse);

                contractInvoiceAttachmentExists = this._contractInvAttachmentRepository?.FindBy(predicate).ToList();

                contractInvoiceAttachmentExists?.ToList().ForEach(x =>
                {
                    string errorCode = MessageType.ContractInvoiceAttachementExists.ToId();
                    messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.DocumentType.Name)));
                });
            }

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count > 0;
        }

        private bool IsRecordValidForProcess(IList<ContractInvoiceAttachment> contractInvoiceAttachments, ValidationType validationType, ref IList<ContractInvoiceAttachment> filteredContractInvoiceAttachments, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredContractInvoiceAttachments = contractInvoiceAttachments?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredContractInvoiceAttachments = contractInvoiceAttachments?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (validationType == ValidationType.Update)
                filteredContractInvoiceAttachments = contractInvoiceAttachments?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            if (filteredContractInvoiceAttachments?.Count <= 0)
                return false;

            return IsContractInvoiceAttachmentHasValidSchema(filteredContractInvoiceAttachments, validationType, ref errorMessages, ref validationMessages);
        }

        private bool IsValidContractInvoiceAttachment(IList<ContractInvoiceAttachment> contractInvoiceAttachments, IList<DbModel.ContractInvoiceAttachment> dbContractInvoiceAttachments, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = contractInvoiceAttachments?.Where(x => !dbContractInvoiceAttachments.ToList().Any(x1 => x1.Id == x.ContractInvoiceAttachmentId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.ContractInvoiceAttachementIdIsInvalidOrNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractInvoiceAttachmentId)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }
        private bool IsContractInvoiceAttachmentHasValidSchema(IList<DomainModel.ContractInvoiceAttachment> models, ValidationType validationType, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
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
