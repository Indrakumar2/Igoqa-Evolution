using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Models.Security;
using Evolution.Supplier.Domain.Interfaces.Data;
using Evolution.Supplier.Domain.Interfaces.Suppliers;
using Evolution.Supplier.Domain.Interfaces.Validations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Supplier.Core.Services
{
    public class SupplierNoteService : ISupplierNoteService
    {

        private readonly IMapper _mapper = null;
        private readonly IAppLogger<SupplierNoteService> _logger = null;
        private readonly ISupplierNoteRepository _repository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly ISupplierService _supplierService = null;
        private readonly ISupplierNoteValidationService _validationService = null;
        private readonly IAppLogger<LogEventGeneration> _applogger = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly IUserService _userService = null;

        #region Constructor

        public SupplierNoteService(IMapper mapper,
                                   IAppLogger<SupplierNoteService> logger,
                                   ISupplierNoteRepository repository,
                                   ISupplierService supplierService,
                                   ISupplierNoteValidationService ValidationService,
                                   IAppLogger<LogEventGeneration> applogger,
                                   JObject messages,
                                   IAuditSearchService auditSearchService,
                                   IUserService userService)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._repository = repository;
            this._supplierService = supplierService;
            this._applogger = applogger;
            this._validationService = ValidationService;
            this._messageDescriptions = messages;
            this._auditSearchService = auditSearchService;
            this._userService = userService;
        }

        #endregion

        #region Public Methods
        #region Get
        public Response Get(DomainModel.SupplierNote searchModel)
        {
            IList<DomainModel.SupplierNote> result = null;
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

        #endregion

        #region Add
        public Response Add(IList<DomainModel.SupplierNote> supplierNotes,
                          bool commitChange = true,
                          bool isDbValidationRequire = true)
        {
            IList<DbModel.SupplierNote> dbSupplierNotes = null;
            IList<DbModel.Supplier> dbSuppliers = null;
            return AddSupplierNote(supplierNotes,null, ref dbSupplierNotes, ref dbSuppliers, commitChange, isDbValidationRequire);
        }

        public Response Add(IList<DomainModel.SupplierNote> supplierNotes,
            IList<DbModel.SqlauditModule> dbModule,
                            ref IList<DbModel.SupplierNote> dbSupplierNotes,
                            ref IList<DbModel.Supplier> dbSuppliers,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            return AddSupplierNote(supplierNotes, dbModule, ref dbSupplierNotes, ref dbSuppliers, commitChange, isDbValidationRequire);
        }
        #endregion
        //D661 issue 8 Start
        public Response Update(IList<DomainModel.SupplierNote> supplierNotes,
            IList<DbModel.SqlauditModule> dbModule,
                            ref IList<DbModel.SupplierNote> dbSupplierNotes,
                            ref IList<DbModel.Supplier> dbSuppliers,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            return ModifySupplierNote(supplierNotes, dbModule, ref dbSupplierNotes, ref dbSuppliers, commitChange, isDbValidationRequire);
        }
        //D661 issue 8 Start

        #region Delete
        public Response Delete(IList<DomainModel.SupplierNote> supplierNotes,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               int? assignmentIds = null)
        {
            IList<DbModel.Supplier> dbSuppliers = null;
            IList<DbModel.SupplierNote> dbSupplierNotes = null;

            return this.RemoveSupplierNotes(supplierNotes,null,
                                                  ref dbSupplierNotes,
                                                  ref dbSuppliers,
                                                   commitChange,
                                                  isDbValidationRequired);
        }

        public Response Delete(IList<DomainModel.SupplierNote> supplierNotes,
            IList<DbModel.SqlauditModule> dbModule,
                               ref IList<DbModel.SupplierNote> dbSupplierNotes,
                               ref IList<DbModel.Supplier> dbSupplier,
                                bool commitChange = true,
                               bool isDbValidationRequired = true,
                               int? assignmentIds = null)
        {
            return this.RemoveSupplierNotes(supplierNotes, dbModule,
                                                  ref dbSupplierNotes,
                                                  ref dbSupplier,
                                                  commitChange,
                                                  isDbValidationRequired);
        }
        #endregion

        #region Record Valid Check
        public Response IsRecordValidForProcess(IList<DomainModel.SupplierNote> supplierNotes,
                                                ValidationType validationType)
        {
            IList<DbModel.SupplierNote> dbSupplierNotes = null;
            IList<DbModel.Supplier> dbSuppliers = null;
            return IsRecordValidForProcess(supplierNotes, validationType, ref dbSupplierNotes, ref dbSuppliers);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.SupplierNote> supplierNotes,
                                                ValidationType validationType,
                                                ref IList<DbModel.SupplierNote> dbSupplierNotes,
                                                ref IList<DbModel.Supplier> dbSuppliers)
        {
            IList<DomainModel.SupplierNote> filteredSupplierNote = null;
            IList<DbModel.Supplier> dbSupplier = null;

            return this.CheckRecordValidForProcess(supplierNotes, validationType, ref filteredSupplierNote, ref dbSupplierNotes, ref dbSupplier);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.SupplierNote> supplierNotes,
                                                ValidationType validationType,
                                                IList<DbModel.SupplierNote> dbSupplierNotes,
                                                IList<DbModel.Supplier> dbSuppliers)
        {
            return IsRecordValidForProcess(supplierNotes, validationType, ref dbSupplierNotes, ref dbSuppliers);
        }
        #endregion

        #endregion

        #region Private Metods
        #region Get
        private IList<DbModel.SupplierNote> GetSupplierNoteById(IList<int> supplierNoteIds,
                                                                 params Expression<Func<DbModel.SupplierNote, object>>[] includes)
        {
            IList<DbModel.SupplierNote> dbSupplierNotes = null;
            if (supplierNoteIds?.Count > 0)
                dbSupplierNotes = _repository.FindBy(x => supplierNoteIds.Contains(x.Id), includes).ToList();

            return dbSupplierNotes;
        }
        #endregion

        #region Add
        private bool IsRecordValidForAdd(IList<DomainModel.SupplierNote> supplierNotes,
                                         ref IList<DomainModel.SupplierNote> filteredSupplierNotes,
                                         ref IList<DbModel.SupplierNote> dbSupplierNotes,
                                         ref IList<DbModel.Supplier> dbSuppliers,
                                         ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (supplierNotes != null && supplierNotes.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredSupplierNotes == null || filteredSupplierNotes.Count <= 0)
                    filteredSupplierNotes = FilterRecord(supplierNotes, validationType);

                if (filteredSupplierNotes?.Count > 0 && IsValidPayload(filteredSupplierNotes, validationType, ref validationMessages))
                {
                    IList<int> supplierIds = filteredSupplierNotes.Select(x => x.SupplierId ?? 0).ToList();
                    result = Convert.ToBoolean(_supplierService.IsRecordExistInDb(supplierIds, ref dbSuppliers, ref validationMessages).Result);
                }
            }
            return result;
        }

        private Response AddSupplierNote(IList<DomainModel.SupplierNote> supplierNotes,
            IList<DbModel.SqlauditModule> dbModule,
                                         ref IList<DbModel.SupplierNote> dbSupplierNotes,
                                         ref IList<DbModel.Supplier> dbSuppliers,
                                         bool commitChange,
                                         bool isDbValidationRequire)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DomainModel.SupplierNote> recordToBeAdd = FilterRecord(supplierNotes, ValidationType.Add);
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(supplierNotes, ValidationType.Add, ref recordToBeAdd, ref dbSupplierNotes, ref dbSuppliers);
                eventId = supplierNotes?.FirstOrDefault()?.EventId;
                if ((!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result)) && recordToBeAdd?.Count > 0)
                {
                    _repository.AutoSave = false;
                    IList<DbModel.Supplier> dbSuppliersToBeAdd = dbSuppliers;

                    var mappedRecords = recordToBeAdd?.ToList().Select(x => new DbModel.SupplierNote()
                    {
                        SupplierId = (int)dbSuppliersToBeAdd?.FirstOrDefault(x1 => x1.Id == x.SupplierId)?.Id,
                        CreatedDate = x.CreatedOn != null ? (DateTime)x.CreatedOn : DateTime.UtcNow,
                        UpdateCount = x.UpdateCount,
                        CreatedBy = x.CreatedBy,
                        Note = x.Notes,
                    }).ToList();

                    _repository.Add(mappedRecords);
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        if (value > 0)
                        {
                            dbSupplierNotes = mappedRecords;
                            mappedRecords?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, supplierNotes?.FirstOrDefault()?.ActionByUser,
                                                                                                null,
                                                                                                ValidationType.Add.ToAuditActionType(),
                                                                                               SqlAuditModuleType.SupplierNote,
                                                                                                null,
                                                                                               _mapper.Map<DomainModel.SupplierNote>(x1),
                                                                                               dbModule));

                        }
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierNotes);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }
        #endregion
        //D661 issue 8 Start
        private bool IsRecordValidForUpdate(IList<DomainModel.SupplierNote> supplierNotes,
                                        ref IList<DomainModel.SupplierNote> filteredSupplierNotes,
                                        ref IList<DbModel.SupplierNote> dbSupplierNotes,
                                        ref IList<DbModel.Supplier> dbSuppliers,
                                        ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (supplierNotes != null && supplierNotes.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredSupplierNotes == null || filteredSupplierNotes.Count <= 0)
                    filteredSupplierNotes = FilterRecord(supplierNotes, validationType);

                if (filteredSupplierNotes?.Count > 0 && IsValidPayload(filteredSupplierNotes, validationType, ref validationMessages))
                {
                    IList<int> supplierIds = filteredSupplierNotes.Select(x => x.SupplierId ?? 0).ToList();
                    result = Convert.ToBoolean(_supplierService.IsRecordExistInDb(supplierIds, ref dbSuppliers, ref validationMessages).Result);
                }
            }
            return result;
        }

        private Response ModifySupplierNote(IList<DomainModel.SupplierNote> supplierNotes,
            IList<DbModel.SqlauditModule> dbModule,
                                         ref IList<DbModel.SupplierNote> dbSupplierNotes,
                                         ref IList<DbModel.Supplier> dbSuppliers,
                                         bool commitChange,
                                         bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DomainModel.SupplierNote> recordToBeModify = FilterRecord(supplierNotes, ValidationType.Update);
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                errorMessages = new List<MessageDetail>();
                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(supplierNotes, ValidationType.Update, ref recordToBeModify, ref dbSupplierNotes, ref dbSuppliers);
                eventId = supplierNotes?.FirstOrDefault()?.EventId;
                if ((!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result)) && recordToBeModify?.Count > 0)
                {
                    if (dbSupplierNotes?.Count > 0)
                    {
                        if (this.IsRecordUpdateCountMatching(recordToBeModify, dbSupplierNotes, ref errorMessages))
                        {
                            dbSupplierNotes.ToList().ForEach(dbSupplierNoteValue =>
                            {
                                var dbSupplierNoteToBeModify = recordToBeModify.FirstOrDefault(x => x.SupplierNoteId == dbSupplierNoteValue.Id);
                                if (dbSupplierNoteToBeModify != null)
                                {
                                    dbSupplierNoteValue.SupplierId = (int)dbSupplierNoteToBeModify.SupplierId;
                                    dbSupplierNoteValue.Id = (int)dbSupplierNoteToBeModify.SupplierNoteId;
                                    dbSupplierNoteValue.CreatedBy = dbSupplierNoteToBeModify.CreatedBy;
                                    dbSupplierNoteValue.Note = dbSupplierNoteToBeModify.Notes;
                                    dbSupplierNoteValue.CreatedDate = DateTime.UtcNow;
                                    dbSupplierNoteValue.LastModification = DateTime.UtcNow;
                                    dbSupplierNoteValue.UpdateCount = dbSupplierNoteToBeModify.UpdateCount.CalculateUpdateCount();
                                    dbSupplierNoteValue.ModifiedBy = dbSupplierNoteToBeModify.ModifiedBy;
                                }
                            });
                            _repository.AutoSave = false;
                            _repository.Update(dbSupplierNotes);
                            if (commitChange && recordToBeModify?.Count > 0)
                                _repository.ForceSave();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierNotes);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);

        }
        //D661 issue 8 End
        private Response RemoveSupplierNotes(IList<DomainModel.SupplierNote> supplierNotes,
            IList<DbModel.SqlauditModule> dbModule,
                                                 ref IList<DbModel.SupplierNote> dbSupplierNotes,
                                                 ref IList<DbModel.Supplier> dbSuppliers,
                                                 bool commitChange,
                                                 bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                Response response = null;
                var recordToBeDelete = FilterRecord(supplierNotes, ValidationType.Delete);
                eventId = supplierNotes?.FirstOrDefault()?.EventId;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(supplierNotes,
                                                       ValidationType.Delete,
                                                       ref dbSupplierNotes,
                                                       ref dbSuppliers
                                                       );

                if (recordToBeDelete?.Count > 0)
                {
                    if (!isDbValidationRequire || (Convert.ToBoolean(response.Result)) && dbSupplierNotes?.Count > 0)
                    {
                        _repository.AutoSave = false;
                        _repository.Delete(dbSupplierNotes);
                        if (commitChange)
                        {
                            int value = _repository.ForceSave();
                            if (value > 0)
                            {
                                supplierNotes?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                        null,
                                                                                                         ValidationType.Delete.ToAuditActionType(),
                                                                                                         SqlAuditModuleType.SupplierNote,
                                                                                                          x1,
                                                                                                           null,
                                                                                                           dbModule
                                                                                                          ));
                            }

                        }
                    }
                    else
                        return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierNotes);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        #region Common
        private Response CheckRecordValidForProcess(IList<DomainModel.SupplierNote> supplierNotes,
                                    ValidationType validationType,
                                    ref IList<DomainModel.SupplierNote> filteredSupplierNotes,
                                    ref IList<DbModel.SupplierNote> dbSupplierNotes,
                                    ref IList<DbModel.Supplier> dbSuppliers)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(supplierNotes, ref filteredSupplierNotes, ref dbSupplierNotes, ref dbSuppliers, ref validationMessages);
                if (validationType == ValidationType.Delete)
                    result = IsValidPayload(supplierNotes, ValidationType.Delete, ref validationMessages);
                if (validationType == ValidationType.Update)       //D661 issue 8 
                    result = IsRecordValidForUpdate(supplierNotes, ref filteredSupplierNotes, ref dbSupplierNotes, ref dbSuppliers, ref validationMessages);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierNotes);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }



        private IList<DomainModel.SupplierNote> FilterRecord(IList<DomainModel.SupplierNote> supplierNotes,
                                                                ValidationType filterType)
        {
            IList<DomainModel.SupplierNote> filterSupplierNotes = null;

            if (filterType == ValidationType.Add)
                filterSupplierNotes = supplierNotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            if (filterType == ValidationType.Delete)
                filterSupplierNotes = supplierNotes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            if (filterType == ValidationType.Update)       //D661 issue 8 
                filterSupplierNotes = supplierNotes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();


            return filterSupplierNotes;
        }

        private bool IsValidPayload(IList<DomainModel.SupplierNote> supplierNotes,
                            ValidationType validationType,
                            ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(supplierNotes), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Supplier, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }
        //D661 issue 8 Start
        private bool IsRecordUpdateCountMatching(IList<DomainModel.SupplierNote> supplierNotes, IList<DbModel.SupplierNote> dbSupplierNote, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();
            var notMatchedRecords = supplierNotes.Where(x => !dbSupplierNote.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.SupplierNoteId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.NotesHasBeenUpdatedByOther.ToId();
                messages.Add(new MessageDetail(ModuleType.Supplier, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.Notes)));
            });
            if (messages.Count > 0)
                errorMessages.AddRange(messages);
            return errorMessages?.Count <= 0;
        }
        //D661 issue 8 End
        #endregion
        #endregion
    }
}