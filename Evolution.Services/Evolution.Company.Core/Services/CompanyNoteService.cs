using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.Company.Domain.Interfaces.Validations;
using Evolution.Company.Domain.Models.Companies;
using Evolution.Logging.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DomainModel = Evolution.Company.Domain.Models.Companies;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Models.Security;
using System.Transactions;

namespace Evolution.Company.Core.Services
{
    public class CompanyNoteService : ICompanyNoteService
    {
        private readonly ICompanyNoteRepository _repository = null;
        private readonly IAppLogger<CompanyNoteService> _logger = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly JObject _MessageDescriptions = null;
        private readonly ICompanyNoteValidationService _validationService = null;

        public CompanyNoteService(ICompanyNoteRepository repository, IAppLogger<CompanyNoteService> logger, ICompanyRepository companyRepository, ICompanyNoteValidationService validationService, JObject messages, IUserService userService)
        {
            this._repository = repository;
            this._logger = logger;
            this._companyRepository = companyRepository;
            this._validationService = validationService;
            this._MessageDescriptions = messages;
        }

        #region Public Methods 
        public Response DeleteCompanyNote(string companyCode, IList<CompanyNote> companyNotes, bool commitChange = true)
        {
            return DeleteCompanyNotes(companyCode, companyNotes, commitChange);
        }

        public Response GetCompanyNote(CompanyNote searchModel)
        {
            IList<DomainModel.CompanyNote> result = null;
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

        public Response ModifyCompanyNote(string companyCode, IList<CompanyNote> companyNotes, bool commitChange = true)
        {
            var result = this.ModifyComapnyNotes(companyCode, companyNotes, commitChange);
            if (result.Code == MessageType.Success.ToId())
                return this.GetCompanyNote(new CompanyNote() { CompanyCode = companyCode });
            else
                return result;
        }

        public Response SaveCompanyNote(string companyCode, IList<CompanyNote> companyNotes, bool commitChange = true)
        {
            var result = this.AddCompanyNotes(companyCode, companyNotes, commitChange);
            if (result.Code == MessageType.Success.ToId())
                return this.GetCompanyNote(new CompanyNote() { CompanyCode = companyCode });
            else
                return result;
        }
        #endregion

        #region Private Methods

        private Response AddCompanyNotes(string companyCode, IList<CompanyNote> companyNotes, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Company dbCompany = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                if (this.IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
                {
                    IList<CompanyNote> recordToBeInserted = null;
                    if (this.IsRecordValidForProcess(companyNotes, ValidationType.Add, ref recordToBeInserted, ref errorMessages, ref validationMessages))
                    {
                        var dbNotesToBeInserted = recordToBeInserted.Select(x => new DbModel.CompanyNote()
                        {
                            CompanyId = dbCompany.Id,
                            CreatedDate = x.CreatedOn,
                            CreatedBy = x.CreatedBy,
                            Note = x.Notes,
                            UpdateCount = x.UpdateCount,


                        }).ToList();
                        _repository.Add(dbNotesToBeInserted);

                        if (commitChange && !_repository.AutoSave && recordToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                            _repository.ForceSave();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyNotes);
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response ModifyComapnyNotes(string companyCode, IList<CompanyNote> companyNotes, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Company dbCompany = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                if (this.IsValidCompany(companyCode, ref dbCompany, ref errorMessages))
                {
                    IList<CompanyNote> recordToBeModify = null;
                    if (this.IsRecordValidForProcess(companyNotes, ValidationType.Update, ref recordToBeModify, ref errorMessages, ref validationMessages))
                    {
                        var compNotes = _repository.FindBy(x => x.Company.Code == companyCode && recordToBeModify.Select(x1 => x1.CompanyNoteId).Contains(x.Id));

                        if (IsValidCompanyNote(recordToBeModify, compNotes.ToList(), ref errorMessages))
                        {
                            if (this.IsRecordUpdateCountMatching(recordToBeModify, compNotes.ToList(), ref errorMessages))
                            {
                                foreach (var notes in compNotes)
                                {
                                    var Note = recordToBeModify.FirstOrDefault(x => x.CompanyNoteId == notes.Id);
                                    notes.CompanyId = dbCompany.Id;
                                    notes.Id = Note.CompanyNoteId;
                                    notes.CreatedBy = Note.CreatedBy;
                                    notes.Note = Note.Notes;
                                    notes.CreatedDate = DateTime.UtcNow;
                                    notes.LastModification = DateTime.UtcNow;
                                    notes.UpdateCount = Note.UpdateCount.CalculateUpdateCount();
                                    notes.ModifiedBy = Note.ModifiedBy;

                                    _repository.Update(notes);
                                }

                                if (commitChange && recordToBeModify?.Count > 0)
                                    _repository.ForceSave();

                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {

                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyNotes);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response DeleteCompanyNotes(string companyCode, IList<CompanyNote> companyNotes, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();

                if (!string.IsNullOrEmpty(companyCode))
                {
                    IList<CompanyNote> recordToBeDelete = null;

                    if (this.IsRecordValidForProcess(companyNotes, ValidationType.Delete, ref recordToBeDelete, ref errorMessages, ref validationMessages))
                    {
                        var dbCompanyNotes = _repository.FindBy(x => recordToBeDelete.Select(x1 => x1.CompanyNoteId).Contains(x.Id) && x.Company.Code == companyCode).ToList();

                        if (IsValidCompanyNote(recordToBeDelete, dbCompanyNotes, ref errorMessages))
                        {
                            foreach (var notes in dbCompanyNotes)
                            {
                                _repository.Delete(notes);
                            }

                            if (commitChange && !_repository.AutoSave && dbCompanyNotes?.Count > 0 && errorMessages.Count <= 0)
                                _repository.ForceSave();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyNotes);
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private bool IsValidCompany(string companyCode, ref DbModel.Company company, ref List<MessageDetail> errorMessages)
        {
            MessageType messageType = MessageType.Success;

            if (string.IsNullOrEmpty(companyCode))
                messageType = MessageType.InvalidCompanyCode;
            else
            {
                company = _companyRepository.FindBy(x => x.Code == companyCode).FirstOrDefault();
                if (company == null)
                    messageType = MessageType.InvalidCompanyCode;
            }

            if (messageType != MessageType.Success)
                errorMessages.Add(new MessageDetail(ModuleType.Company, MessageType.InvalidCompanyCode.ToId(), _MessageDescriptions[MessageType.InvalidCompanyCode.ToId()].ToString()));

            return messageType == MessageType.Success;
        }

        private bool IsRecordValidForProcess(IList<CompanyNote> CompanyNotes, ValidationType validationType, ref IList<CompanyNote> filteredNotes, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredNotes = CompanyNotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredNotes = CompanyNotes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (validationType == ValidationType.Update)
                filteredNotes = CompanyNotes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            return filteredNotes?.Count > 0 ? IsCompanyNotesHasValidSchema(filteredNotes, validationType, ref validationMessages) : false;
        }

        private bool IsRecordUpdateCountMatching(IList<CompanyNote> companyNotes, IList<DbModel.CompanyNote> dbCompanyNotes, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = companyNotes.Where(x => !dbCompanyNotes.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.CompanyNoteId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.NotesHasBeenUpdatedByOther.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.Notes)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsValidCompanyNote(IList<CompanyNote> companyNotes, IList<DbModel.CompanyNote> dbCompanyNotes, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = companyNotes.Where(x => !dbCompanyNotes.ToList().Any(x1 => x1.Id == x.CompanyNoteId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.NotesIdIsInvalidOrNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Company, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.CompanyNoteId)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsCompanyNotesHasValidSchema(IList<CompanyNote> companyNotes, ValidationType validationType, ref List<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(companyNotes), validationType);

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
