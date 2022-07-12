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
using Evolution.Security.Domain.Interfaces.Security;
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
    public class ContractNoteService : IContractNoteService
    {
        private readonly IMapper _mapper = null;
        private IAppLogger<ContractNoteService> _logger = null;
        private IContractNoteRepository _repository = null;
        private IContractRepository _contractRepository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IContractNoteValidationService _validationService = null;
        private readonly IUserService _userService = null;

        private readonly IAuditSearchService _auditSearchService = null;

        public ContractNoteService(IMapper mapper, IAppLogger<ContractNoteService> logger, IContractNoteRepository repository, IContractRepository contractRepository,
                                       IContractNoteValidationService validationService, JObject messages, IAuditSearchService auditSearchService, IUserService userService)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._repository = repository;
            this._contractRepository = contractRepository;
            this._validationService = validationService;
            this._messageDescriptions = messages;
            this._userService = userService;
            _auditSearchService = auditSearchService;
        }

        public Response GetContractNote(ContractNote searchModel)
        {
            IList<DomainModel.ContractNote> result = null;
            Exception exception = null;

            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                 new TransactionOptions
                                                 {
                                                     IsolationLevel = IsolationLevel.ReadUncommitted
                                                 }))
                {
                    result = this._repository.Search(searchModel);
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

        public Response SaveContractNote(string contractNumber, IList<ContractNote> contractNotes, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = this.AddContractNotes(contractNumber, contractNotes, dbContract,dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractNote(new ContractNote() { ContractNumber = contractNumber });
            else
                return result;
        }

        public Response SaveContractNote(string contractNumber, IList<ContractNote> contractNotes, IList<DbModel.Contract> dbContract, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false)
        {
           var result = this.AddContractNotes(contractNumber, contractNotes, dbContract, dbModule,commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractNote(new ContractNote() { ContractNumber = contractNumber });
            else
                return result;
        }
       //D661 issue 8 Start
        public Response ModifyContractNote(string contractNumber, IList<ContractNote> contractNotes, bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = this.ModifyContractNote(contractNumber, contractNotes, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractNote(new ContractNote() { ContractNumber = contractNumber });
            else
                return result;
        }
       //D661 issue 8 End
        public Response DeleteContractNote(string contractNumber, IList<ContractNote> contractNotes, bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = RemoveContractNotes(contractNumber, contractNotes, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetContractNote(new ContractNote() { ContractNumber = contractNumber });
            else
                return result;
        }

        #region Private Methods

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

        private bool IsRecordValidForProcess(IList<ContractNote> ContractNotes, ValidationType validationType, ref IList<ContractNote> filteredNotes, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredNotes = ContractNotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredNotes = ContractNotes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (validationType == ValidationType.Update)       //D661 issue 8 
                filteredNotes = ContractNotes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            if (filteredNotes?.Count <= 0)
                return false;

            return IsContractNoteHasValidSchema(filteredNotes, validationType, ref errorMessages, ref validationMessages);
        }

        private Response AddContractNotes(string contractNumber, IList<ContractNote> contractNotes, IList<DbModel.Contract> dbContracts, IList<DbModel.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Contract dbContract = dbContracts?.Count > 0 ? dbContracts.FirstOrDefault() : null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<ContractNote> recordToBeInserted = null;
                eventId = contractNotes?.FirstOrDefault()?.EventId;
                if (this.IsRecordValidForProcess(contractNotes, ValidationType.Add, ref recordToBeInserted, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidContract(contractNumber, ref dbContract, ref errorMessages))
                    {
                        var dbNotesToBeInserted = recordToBeInserted.Select(x => new DbModel.ContractNote()
                        {
                            ContractId = dbContract.Id,
                            CreatedDate = x.CreatedOn,
                            CreatedBy = x.CreatedBy,
                            Note = x.Notes,
                            UpdateCount = x.UpdateCount,
                        }).ToList();

                        _repository.Add(dbNotesToBeInserted);

                        if (commitChange && !_repository.AutoSave && recordToBeInserted?.Count > 0)
                        {
                            int value = _repository.ForceSave();
                            if (value > 0)
                                
                                dbNotesToBeInserted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(recordToBeInserted.FirstOrDefault(), ref eventId, recordToBeInserted.FirstOrDefault().ActionByUser,
                                                                                                         null,
                                                                                                         ValidationType.Add.ToAuditActionType(),
                                                                                                         SqlAuditModuleType.ContractNote,
                                                                                                        null,
                                                                                                        _mapper.Map<ContractNote>(x1),
                                                                                                        dbModule));

                        }
                    }
                }


            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractNotes);

            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }
       //D661 issue 8 Start
        private Response ModifyContractNote(string contractNumber, IList<ContractNote> contractNotes, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Contract dbContract = null;
            IList<DbModel.ContractNote> dbContractNote = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                errorMessages = new List<MessageDetail>();
                IList<ContractNote> recordToBeModify = null;
                eventId = contractNotes?.FirstOrDefault()?.EventId;
                if (this.IsRecordValidForProcess(contractNotes, ValidationType.Update, ref recordToBeModify, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidContract(contractNumber, ref dbContract, ref errorMessages))
                    {
                        dbContractNote = GetContractNotes(recordToBeModify, ValidationType.Update);
                        if (dbContractNote?.Count > 0)
                        {
                            if (IsValidContractNote(recordToBeModify, dbContractNote, ref errorMessages))
                            {
                                if(this.IsRecordUpdateCountMatching(recordToBeModify, dbContractNote, ref errorMessages))
                                {
                                    if (recordToBeModify?.Count > 0)
                                    {
                                        dbContractNote.ToList().ForEach(dbContractNoteValue =>
                                        {
                                            var dbContractNoteToBeModify = recordToBeModify.FirstOrDefault(x => x.ContractNoteId == dbContractNoteValue.Id);
                                                if (dbContractNoteToBeModify != null)
                                                {
                                                    dbContractNoteValue.ContractId = dbContract.Id;
                                                    dbContractNoteValue.Id = dbContractNoteToBeModify.ContractNoteId;
                                                    dbContractNoteValue.CreatedBy = dbContractNoteToBeModify.CreatedBy;
                                                    dbContractNoteValue.Note = dbContractNoteToBeModify.Notes;
                                                    dbContractNoteValue.CreatedDate = DateTime.UtcNow;
                                                    dbContractNoteValue.LastModification = DateTime.UtcNow;
                                                    dbContractNoteValue.UpdateCount = dbContractNoteToBeModify.UpdateCount.CalculateUpdateCount();
                                                    dbContractNoteValue.ModifiedBy = dbContractNoteToBeModify.ModifiedBy;
                                                }

                                          });
                                        _repository.AutoSave = false;
                                        _repository.Update(dbContractNote);
                                        if (commitChange && recordToBeModify?.Count > 0)
                                            _repository.ForceSave();
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractNotes);

            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }
       //D661 issue 8 End
        private Response RemoveContractNotes(string contractNumber, IList<ContractNote> contractNotes, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Contract dbContract = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<ContractNote> recordToBeDeleted = null;
                eventId = contractNotes?.FirstOrDefault()?.EventId;

                if (this.IsRecordValidForProcess(contractNotes, ValidationType.Delete, ref recordToBeDeleted, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidContract(contractNumber, ref dbContract, ref errorMessages))
                    {
                        var delCont = _repository.DeleteNote(recordToBeDeleted.ToList());
                        if (delCont > 0)
                        {
                                contractNotes?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                               null,
                                                                                                               ValidationType.Delete.ToAuditActionType(),
                                                                                                              SqlAuditModuleType.ContractNote,
                                                                                                             x1, null));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractNotes);
            }
            finally
            {
                _repository.AutoSave = false;
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private bool IsContractNoteHasValidSchema(IList<DomainModel.ContractNote> models, ValidationType validationType, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
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
        //D661 issue 8 Start
        private bool IsValidContractNote(IList<ContractNote> contractNotes, IList<DbModel.ContractNote> dbContractNotes, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = contractNotes.Where(x => !dbContractNotes.ToList().Any(x1 => x1.Id == x.ContractNoteId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.NotesIdIsInvalidOrNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ContractNoteId)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsRecordUpdateCountMatching(IList<ContractNote> contractNotes, IList<DbModel.ContractNote> dbContractNotes, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = contractNotes.Where(x => !dbContractNotes.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.ContractNoteId)).ToList();

            notMatchedRecords?.ForEach(x =>
            {
                string errorCode = MessageType.NotesHasBeenUpdatedByOther.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.Notes)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private IList<DbModel.ContractNote> GetContractNotes(IList<DomainModel.ContractNote> contractNotes,ValidationType validationType)
        {
            IList<DbModel.ContractNote> dbContractNote = null;
            if (validationType != ValidationType.Add)
            {
                if (contractNotes?.Count > 0)
                {
                    var contractNoteId = contractNotes.Select(x => x.ContractNoteId).Distinct().ToList();
                    dbContractNote = _repository.FindBy(x => contractNoteId.Contains(x.Id)).ToList();
                }
            }
            return dbContractNote;
        }
       //D661 issue 8 End
    }

    #endregion

}
