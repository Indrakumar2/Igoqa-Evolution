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
using Evolution.SupplierPO.Domain.Interfaces.Data;
using Evolution.SupplierPO.Domain.Interfaces.SupplierPO;
using Evolution.SupplierPO.Domain.Interfaces.Validations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.SupplierPO.Core.Services
{
    public class SupplierPONoteService : ISupplierPONoteService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<SupplierPONoteService> _logger = null;
        private readonly ISupplierPONoteRepository _repository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly ISupplierPORepository _supplierPORepository = null;
        private readonly ISupplierPONoteValidationService _validationService = null;
        private readonly ISupplierPOService _supplierPOService = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly IAppLogger<LogEventGeneration> _applogger = null;

        #region Constructor

        public SupplierPONoteService(IMapper mapper,
                                     IAppLogger<SupplierPONoteService> logger,
                                     ISupplierPONoteRepository repository,
                                     ISupplierPORepository supplierPORepository,
                                     ISupplierPOService supplierPOService,
                                     ISupplierPONoteValidationService validationService,
                                    IAuditSearchService auditSearchService,
                                     IAppLogger<LogEventGeneration> applogger,
                                     JObject messages)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = repository;
            _supplierPORepository = supplierPORepository;
            _supplierPOService = supplierPOService;
            _validationService = validationService;
            _auditSearchService = auditSearchService;
            _applogger = applogger;
            _messageDescriptions = messages;
        }

        #endregion

        #region Public Methods

        #region Get
        public Response Get(DomainModel.SupplierPONote searchModel)
        {
            IList<DomainModel.SupplierPONote> result = null;
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
        public Response Add(IList<DomainModel.SupplierPONote> supplierPONotes,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONotes = null;
            IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs = null;
            return AddSupplierPONotes(supplierPONotes,null, ref dbSupplierPONotes, ref dbSupplierPOs, commitChange, isDbValidationRequire);
        }

        public Response Add(IList<DomainModel.SupplierPONote> supplierPONotes,
            IList<DbModel.SqlauditModule> dbModule,
                            ref IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONotes,
                            ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            return AddSupplierPONotes(supplierPONotes, dbModule, ref dbSupplierPONotes, ref dbSupplierPOs, commitChange, isDbValidationRequire);
        }
        //D661 issue 8 Start
        public Response Update(IList<DomainModel.SupplierPONote> supplierPONotes,
            IList<DbModel.SqlauditModule> dbModule,
                          ref IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONotes,
                          ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                          bool commitChange = true,
                          bool isDbValidationRequire = true)
        {
            return ModifySupplierPONotes(supplierPONotes, dbModule, ref dbSupplierPONotes, ref dbSupplierPOs, commitChange, isDbValidationRequire);
        }
        //D661 issue 8 End


        public Response Delete(IList<DomainModel.SupplierPONote> supplierPONotes,
                             bool commitChange = true,
                             bool isDbValidationRequired = true,
                             int? supplierPos = null)
        {
            IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONotes = null;
            IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs = null;

            return this.RemoveSupplierPONotes(supplierPONotes,
                null,
                                               ref dbSupplierPONotes,
                                               ref dbSupplierPOs,
                                               commitChange,
                                               isDbValidationRequired);
        }

        public Response Delete(IList<DomainModel.SupplierPONote> supplierNotes,
                               IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONotes,
                               IList<DbModel.SqlauditModule> dbModule,
                               ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                bool commitChange = true,
                               bool isDbValidationRequired = true)
        {
            return this.RemoveSupplierPONotes(supplierNotes,dbModule,
                                                  ref dbSupplierPONotes,
                                                  ref dbSupplierPOs,
                                                  commitChange,
                                                  isDbValidationRequired);
        }


        #endregion

        #region Record Valid Check
        public Response IsRecordValidForProcess(IList<DomainModel.SupplierPONote> supplierPONotes,
                                                ValidationType validationType)
        {
            IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONotes = null;
            IList<DbModel.SupplierPurchaseOrder> dbSupplierPos = null;
            return IsRecordValidForProcess(supplierPONotes, validationType, ref dbSupplierPONotes, ref dbSupplierPos);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.SupplierPONote> supplierPONotes,
                                                ValidationType validationType,
                                                ref IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONotes,
                                                ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPos)
        {
            IList<DomainModel.SupplierPONote> filteredSupplierPONote = null;
            return this.CheckRecordValidForProcess(supplierPONotes, validationType, ref filteredSupplierPONote, ref dbSupplierPONotes, ref dbSupplierPos);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.SupplierPONote> supplierPONotes,
                                                ValidationType validationType,
                                                IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONotes,
                                                IList<DbModel.SupplierPurchaseOrder> dbSupplierPos)
        {
            return IsRecordValidForProcess(supplierPONotes, validationType, ref dbSupplierPONotes, ref dbSupplierPos);
        }
        #endregion

        #endregion

        #region Private Metods

        #region Get
        private IList<DbModel.SupplierPurchaseOrderNote> GetSupplierPONoteById(IList<int> supplierPONoteIds,
                                                                               params Expression<Func<DbModel.SupplierPurchaseOrderNote, object>>[] includes)
        {
            IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONotes = null;
            if (supplierPONoteIds?.Count > 0)
                dbSupplierPONotes = _repository.FindBy(x => supplierPONoteIds.Contains(x.Id), includes).ToList();

            return dbSupplierPONotes;
        }
        #endregion

        #region Add
        private bool IsRecordValidForAdd(IList<DomainModel.SupplierPONote> supplierPONotes,
                                         ref IList<DomainModel.SupplierPONote> filteredSupplierPONotes,
                                         ref IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONotes,
                                         ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                         ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (supplierPONotes != null && supplierPONotes.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredSupplierPONotes == null || filteredSupplierPONotes.Count <= 0)
                    filteredSupplierPONotes = FilterRecords(supplierPONotes, validationType);

                if (filteredSupplierPONotes?.Count > 0 && IsValidPayload(filteredSupplierPONotes, validationType, ref validationMessages))
                {
                    IList<int> supplierPOIds = filteredSupplierPONotes.Select(x => (int)x.SupplierPOId).ToList();
                    result = Convert.ToBoolean(_supplierPOService.IsRecordExistInDb(supplierPOIds, ref dbSupplierPOs, ref validationMessages).Result);
                }
            }

            return result;
        }

        private Response AddSupplierPONotes(IList<DomainModel.SupplierPONote> supplierPONotes,
            IList<DbModel.SqlauditModule> dbModule,
                                            ref IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPoNotes,
                                            ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                            bool commitChange = true,
                                            bool IsDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;

            try
            {
                Response valdResponse = null;
                IList<DomainModel.SupplierPONote> recordToBeAdd = FilterRecords(supplierPONotes, ValidationType.Add);
                eventId = supplierPONotes.Select(x => x.EventId).FirstOrDefault();
                var recordToAdd = FilterRecords(supplierPONotes, ValidationType.Add);
                if (IsDbValidationRequired)
                    valdResponse = CheckRecordValidForProcess(supplierPONotes, ValidationType.Add, ref recordToAdd, ref dbSupplierPoNotes, ref dbSupplierPOs);

                if (!IsDbValidationRequired || Convert.ToBoolean(valdResponse.Result))
                {
                    recordToAdd = recordToAdd.Select(x => { x.SupplierPONoteId = 0; return x; }).ToList();
                    var mappedRecords = _mapper.Map<IList<DbModel.SupplierPurchaseOrderNote>>(recordToAdd);
                    _repository.AutoSave = false;
                    _repository.Add(mappedRecords);
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        if (value > 0)
                        {
                            mappedRecords?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.CreatedBy,
                                                                                               null,
                                                                                                ValidationType.Add.ToAuditActionType(),
                                                                                                SqlAuditModuleType.SupplierPONote,
                                                                                                 null,
                                                                                                  _mapper.Map<DomainModel.SupplierPONote>(x1),
                                                                                                  dbModule
                                                                                                 ));
                        }
                    }
                }
                else
                    return valdResponse;

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPONotes);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }
        //D661 issue 8 Start
        private bool IsRecordValidForUpdate(IList<DomainModel.SupplierPONote> supplierPONotes,
                                        ref IList<DomainModel.SupplierPONote> filteredSupplierPONotes,
                                        ref IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONotes,
                                        ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                        ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (supplierPONotes != null && supplierPONotes.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredSupplierPONotes == null || filteredSupplierPONotes.Count <= 0)
                    filteredSupplierPONotes = FilterRecords(supplierPONotes, validationType);

                if (filteredSupplierPONotes?.Count > 0 && IsValidPayload(filteredSupplierPONotes, validationType, ref validationMessages))
                {
                    IList<int> supplierPOIds = filteredSupplierPONotes.Select(x => (int)x.SupplierPOId).ToList();
                    result = Convert.ToBoolean(_supplierPOService.IsRecordExistInDb(supplierPOIds, ref dbSupplierPOs, ref validationMessages).Result);
                }
            }

            return result;
        }

        private Response ModifySupplierPONotes(IList<DomainModel.SupplierPONote> supplierPONotes,
            IList<DbModel.SqlauditModule> dbModule,
                                           ref IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPoNotes,
                                           ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                           bool commitChange = true,
                                           bool IsDbValidationRequired = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;

            try
            {
                Response valdResponse = null;
                eventId = supplierPONotes.Select(x => x.EventId).FirstOrDefault();
                var recordToModify = FilterRecords(supplierPONotes, ValidationType.Update);
                if (IsDbValidationRequired)
                    valdResponse = CheckRecordValidForProcess(supplierPONotes, ValidationType.Update, ref recordToModify, ref dbSupplierPoNotes, ref dbSupplierPOs);
                if ((!IsDbValidationRequired || Convert.ToBoolean(valdResponse.Result)) && recordToModify?.Count > 0)
                {
                    if (dbSupplierPoNotes?.Count > 0)
                    {
                        if (this.IsRecordUpdateCountMatching(recordToModify, dbSupplierPoNotes, ref errorMessages))
                        {
                            dbSupplierPoNotes.ToList().ForEach(dbSupplierPONoteValue =>
                            {
                                var dbSupplierPONoteToBeModify = recordToModify.FirstOrDefault(x => x.SupplierPONoteId == dbSupplierPONoteValue.Id);
                                if (dbSupplierPONoteToBeModify != null)
                                {
                                    dbSupplierPONoteValue.SupplierPurchaseOrderId = (int)dbSupplierPONoteToBeModify.SupplierPOId;
                                    dbSupplierPONoteValue.Id = (int)dbSupplierPONoteToBeModify.SupplierPONoteId;
                                    dbSupplierPONoteValue.CreatedBy = dbSupplierPONoteToBeModify.CreatedBy;
                                    dbSupplierPONoteValue.Note = dbSupplierPONoteToBeModify.Notes;
                                    dbSupplierPONoteValue.CreatedDate = DateTime.UtcNow;
                                    dbSupplierPONoteValue.LastModification = DateTime.UtcNow;
                                    dbSupplierPONoteValue.UpdateCount = dbSupplierPONoteToBeModify.UpdateCount.CalculateUpdateCount();
                                    dbSupplierPONoteValue.ModifiedBy = dbSupplierPONoteToBeModify.ModifiedBy;
                                }
                            });
                            _repository.AutoSave = false;
                            _repository.Update(dbSupplierPoNotes);
                            if (commitChange && recordToModify?.Count > 0)
                                _repository.ForceSave();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPONotes);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);

        }
        //D661 issue 8 End
        private Response RemoveSupplierPONotes(IList<DomainModel.SupplierPONote> supplierPONotes,
            IList<DbModel.SqlauditModule> dbModule,
                                               ref IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPoNotes,
                                               ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
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
                eventId = supplierPONotes?.FirstOrDefault()?.EventId;
                var recordToBeDelete = FilterRecords(supplierPONotes, ValidationType.Delete);
                if (recordToBeDelete?.Count > 0)
                {
                    if (isDbValidationRequire)
                        response = IsRecordValidForProcess(recordToBeDelete,
                                                           ValidationType.Delete,
                                                           ref dbSupplierPoNotes,
                                                           ref dbSupplierPOs
                                                           );


                    if (!isDbValidationRequire || (response == null || Convert.ToBoolean(response.Result)) && dbSupplierPoNotes?.Count > 0)
                    {
                        _repository.AutoSave = false;
                        _repository.Delete(dbSupplierPoNotes);
                        if (commitChange)
                        {
                            int value = _repository.ForceSave();
                            if (value > 0)
                            {
                                supplierPONotes?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                      null,
                                                                                                       ValidationType.Delete.ToAuditActionType(),
                                                                                                       SqlAuditModuleType.SupplierPONote,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPONotes);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        #endregion

        #region Common
        private Response CheckRecordValidForProcess(IList<DomainModel.SupplierPONote> supplierPONotes,
                                                    ValidationType validationType,
                                                    ref IList<DomainModel.SupplierPONote> filteredSupplierNotes,
                                                    ref IList<DbModel.SupplierPurchaseOrderNote> dbSupplierNotes,
                                                    ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(supplierPONotes, ref filteredSupplierNotes, ref dbSupplierNotes, ref dbSupplierPOs, ref validationMessages);
                if (validationType == ValidationType.Delete)
                    result = IsValidPayload(supplierPONotes, ValidationType.Delete, ref validationMessages);
                if (validationType == ValidationType.Update)       //D661 issue 8 
                    result = IsRecordValidForUpdate(supplierPONotes, ref filteredSupplierNotes, ref dbSupplierNotes, ref dbSupplierPOs, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPONotes);

            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);

        }

        private IList<DomainModel.SupplierPONote> FilterRecords(IList<DomainModel.SupplierPONote> supplierPONotes, ValidationType validationType)
        {
            IList<DomainModel.SupplierPONote> filteredNotes = null;

            if (validationType == ValidationType.Add)
                filteredNotes = supplierPONotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

            if (validationType == ValidationType.Delete)
                filteredNotes = supplierPONotes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            if (validationType == ValidationType.Update)       //D661 issue 8 
                filteredNotes = supplierPONotes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            return filteredNotes;
        }

        private bool IsValidPayload(IList<DomainModel.SupplierPONote> supplierPONotes, ValidationType validationType, ref IList<ValidationMessage> validationMessages)
        {

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validMessages = validationMessages;

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(supplierPONotes), validationType);

            if (validationResults?.Count > 0)
            {
                validMessages.Add(_messageDescriptions, ModuleType.Supplier, validationResults);
                validationMessages.AddRange(validMessages);
            }

            return validationMessages?.Count <= 0;

        }
        //D661 issue 8 Start
        private bool IsRecordUpdateCountMatching(IList<DomainModel.SupplierPONote> supplierPONotes, IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONote, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = supplierPONotes.Where(x => !dbSupplierPONote.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.SupplierPONoteId)).ToList();

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