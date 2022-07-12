using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Visit.Domain.Interfaces.Data;
using Evolution.Visit.Domain.Interfaces.Validations;
using Evolution.Visit.Domain.Interfaces.Visits;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Core.Services
{
    public class VisitReferencesService : IVisitReferenceService
    {
        private IMapper _mapper = null;
        private IAppLogger<VisitReferencesService> _logger = null;
        private IVisitReferencesRepository _repository = null;
        private IVisitReferenceValidationService _visitRefrenceValidationService = null;
        private IMasterService _masterService = null;
        private readonly IVisitService _visitService = null;
        private readonly JObject _messages = null;
        private readonly IAuditSearchService _auditSearchService = null;

        public VisitReferencesService(IAppLogger<VisitReferencesService> logger,
                                         IVisitReferencesRepository visitReferenceRepository,
                                         IVisitReferenceValidationService visitRefrenceValidationService,
                                         IVisitService visitService,
                                         IMasterService masterService,
                                         IMapper mapper,
                                         JObject messages,
                                         IAuditSearchService auditSearchService)
                                     
        {
            _mapper = mapper;
            _logger = logger;
            _repository = visitReferenceRepository;
            _visitRefrenceValidationService = visitRefrenceValidationService;
            _visitService = visitService;
            _masterService = masterService;
            this._messages = messages;
            this._auditSearchService = auditSearchService;
        }

        public Response Get(DomainModel.VisitReference searchModel)
        {
            IList<DomainModel.VisitReference> result = null;
            Exception exception = null;
            try
            {
                // result = this._repository.Search(searchModel);
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

        public Response Add(IList<DomainModel.VisitReference> visitReferences,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? visitId = null)
        {
            IList<DbModel.Data> dbReferenceType = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.VisitReference> dbVisitReference = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            if (visitId.HasValue)
                visitReferences?.ToList().ForEach(x => { x.VisitId = visitId; });

            return AddVisitReference(visitReferences,
                                         ref dbVisitReference,
                                         ref dbVisit,
                                         ref dbReferenceType,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }

        public Response Add(IList<DomainModel.VisitReference> visitReferences,
                            ref IList<DbModel.VisitReference> dbVisitReferences,
                            ref IList<DbModel.Visit> dbVisit,
                            ref IList<DbModel.Data> dbReferenceType,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? visitId = null)
        {
            if (visitId.HasValue)
                visitReferences?.ToList().ForEach(x => { x.VisitId = visitId; });

            return AddVisitReference(visitReferences,
                                       ref dbVisitReferences,
                                       ref dbVisit,
                                       ref dbReferenceType,
                                       dbModule,
                                       commitChange,
                                       isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.VisitReference> visitReferences,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               long? visitId = null)
        {
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.Data> dbReferenceType = null;
            IList<DbModel.VisitReference> dbVisitReference = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            if (visitId.HasValue)
                visitReferences?.ToList().ForEach(x => { x.VisitId = visitId; });

            return this.RemoveVisitReference(visitReferences,
                                                  ref dbVisitReference,
                                                  ref dbVisit,
                                                  ref dbReferenceType,
                                                  dbModule,
                                                  commitChange,
                                                  isDbValidationRequired);
        }



        public Response Delete(IList<DomainModel.VisitReference> visitReferences,
                               ref IList<DbModel.VisitReference> dbVisitReferences,
                               ref IList<DbModel.Visit> dbVisit,
                               ref IList<DbModel.Data> dbReferenceType,
                               IList<DbModel.SqlauditModule> dbModule,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               long? visitId = null)
        {
            if (visitId.HasValue)
                visitReferences?.ToList().ForEach(x => { x.VisitId = visitId; });

            return this.RemoveVisitReference(visitReferences,
                                                  ref dbVisitReferences,
                                                  ref dbVisit,
                                                  ref dbReferenceType,
                                                  dbModule,
                                                  commitChange,
                                                  isDbValidationRequired);
        }


        public Response Modify(IList<DomainModel.VisitReference> visitReferences,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? visitId = null)
        {
            IList<DbModel.Data> dbReferenceType = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            if (visitId.HasValue)
                visitReferences?.ToList().ForEach(x => { x.VisitId = visitId; });

            IList<DbModel.VisitReference> dbVisitReference = null;
            return UpdateVisitReference(visitReferences,
                                             ref dbVisitReference,
                                             ref dbVisit,
                                             ref dbReferenceType,
                                             dbModule,
                                             commitChange,
                                             isDbValidationRequired);
        }



        public Response Modify(IList<DomainModel.VisitReference> visitReferences,
                                ref IList<DbModel.VisitReference> dbVisitReferences,
                                ref IList<DbModel.Visit> dbVisit,
                                ref IList<DbModel.Data> dbReferenceType,
                                IList<DbModel.SqlauditModule> dbModule,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? visitId = null)
        {
            if (visitId.HasValue)
                visitReferences?.ToList().ForEach(x => { x.VisitId = visitId; });

            return UpdateVisitReference(visitReferences,
                                             ref dbVisitReferences,
                                             ref dbVisit,
                                             ref dbReferenceType,
                                             dbModule,
                                             commitChange,
                                             isDbValidationRequired);
        }


        public Response IsRecordValidForProcess(IList<DomainModel.VisitReference> visitReferences,
                                                ValidationType validationType)
        {
            IList<DbModel.VisitReference> dbVisitReferences = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.Data> dbReferenceType = null;
            return IsRecordValidForProcess(visitReferences,
                                            validationType,
                                            ref dbVisitReferences,
                                            ref dbVisit,
                                            ref dbReferenceType);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitReference> visitReferences,
                                                ValidationType validationType,
                                                ref IList<DbModel.VisitReference> dbVisitReferences,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Data> dbReferenceType)
        {
            IList<DomainModel.VisitReference> filteredVisitReference = null;
            return IsRecordValidForProcess(visitReferences,
                                            validationType,
                                            ref filteredVisitReference,
                                            ref dbVisitReferences,
                                            ref dbVisit,
                                            ref dbReferenceType);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitReference> visitReferences,
                                                ValidationType validationType,
                                                IList<DbModel.VisitReference> dbVisitReferences,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Data> dbReferenceType)
        {
            return IsRecordValidForProcess(visitReferences,
                                          validationType,
                                          ref dbVisitReferences,
                                          ref dbVisit,
                                          ref dbReferenceType);
        }

        private IList<DbModel.VisitReference> GetVisitReference(IList<DomainModel.VisitReference> visitReferences,
                                                                        ValidationType validationType)
        {
            IList<DbModel.VisitReference> dbVisitRefrenceType = null;
            if (validationType != ValidationType.Add)
            {
                if (visitReferences?.Count > 0)
                {
                    var visitRefrenceTypeId = visitReferences.Select(x => x.VisitReferenceId).Distinct().ToList();
                    dbVisitRefrenceType = _repository.FindBy(x => visitRefrenceTypeId.Contains(x.Id)).ToList();
                }
            }
            return dbVisitRefrenceType;
        }

        private Response AddVisitReference(IList<DomainModel.VisitReference> visitReferences,
                                               ref IList<DbModel.VisitReference> dbVisitReferences,
                                               ref IList<DbModel.Visit> dbVisit,
                                               ref IList<DbModel.Data> dbReferenceType,
                                               IList<DbModel.SqlauditModule> dbModule,
                                               bool commitChange,
                                               bool isDbValidationRequired)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.Data> dbReferenceTypes = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(visitReferences, ValidationType.Add);
                eventId = visitReferences.FirstOrDefault().EventId;

                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(visitReferences,
                                                            ValidationType.Add,
                                                            ref recordToBeAdd,
                                                            ref dbVisitReferences,
                                                            ref dbVisit,
                                                            ref dbReferenceType);
                else if (dbVisitReferences?.Count <= 0)
                    dbVisitReferences = GetVisitReference(recordToBeAdd, ValidationType.Add);

                if (recordToBeAdd?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    _repository.AutoSave = false;
                    dbReferenceTypes = dbReferenceType;
                    
                    dbVisitReferences = _mapper.Map<IList<DbModel.VisitReference>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isVisitId"] = false;
                        opt.Items["ReferenceTypes"] = dbReferenceTypes;
                    });
                    _repository.Add(dbVisitReferences);
                    if (commitChange)
                    { 
                        var savCnt=_repository.ForceSave();
                        if (savCnt>0)
                        {
                             dbVisitReferences?.ToList().ForEach(x1 => _auditSearchService.AuditLog(recordToBeAdd.FirstOrDefault(), ref eventId, recordToBeAdd?.FirstOrDefault()?.ActionByUser, null,
                                                                                                   ValidationType.Add.ToAuditActionType(),
                                                                                                   SqlAuditModuleType.VisitReference,
                                                                                                   null,
                                                                                                   _mapper.Map<DomainModel.VisitReference>(x1),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitReferences);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateVisitReference(IList<DomainModel.VisitReference> visitReferences,
                                                  ref IList<DbModel.VisitReference> dbVisitReference,
                                                  ref IList<DbModel.Visit> dbVisit,
                                                  ref IList<DbModel.Data> dbReferenceType,
                                                  IList<DbModel.SqlauditModule> dbModule,
                                                  bool commitChange,
                                                  bool isDbValidationRequired)
        {
            Exception exception = null;
            IList<DbModel.Data> dbReferenceTypes = null;
            IList<DomainModel.VisitReference> result = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(visitReferences, ValidationType.Update);
                eventId = visitReferences?.FirstOrDefault()?.EventId;
                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(visitReferences,
                                                            ValidationType.Update,
                                                            ref recordToBeModify,
                                                            ref dbVisitReference,
                                                            ref dbVisit,
                                                            ref dbReferenceType);
                else if (dbVisitReference?.Count <= 0)
                    dbVisitReference = GetVisitReference(recordToBeModify, ValidationType.Update);

                if (recordToBeModify?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    if (dbVisitReference?.Count > 0)
                    {
                        dbReferenceTypes = dbReferenceType;
                        IList<DomainModel.VisitReference> domExistingVisitReference = new List<DomainModel.VisitReference>();
                        dbVisitReference?.ToList()?.ForEach(x =>
                        {
                            domExistingVisitReference.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.VisitReference>(x)));
                        });

                        dbVisitReference.ToList().ForEach(dbVisitReferenceType =>
                        {
                            var visitReferenceToBeModify = recordToBeModify.FirstOrDefault(x => x.VisitReferenceId == dbVisitReferenceType.Id);
                            if (visitReferenceToBeModify != null)
                            {
                                dbVisitReferenceType.AssignmentReferenceTypeId = dbReferenceTypes.FirstOrDefault(x1 => x1.Name == visitReferenceToBeModify.ReferenceType).Id;
                                dbVisitReferenceType.VisitId = (long)visitReferenceToBeModify.VisitId;
                                dbVisitReferenceType.ReferenceValue = visitReferenceToBeModify.ReferenceValue;
                                dbVisitReferenceType.LastModification = DateTime.UtcNow;
                                dbVisitReferenceType.UpdateCount = visitReferenceToBeModify.UpdateCount.CalculateUpdateCount();
                                dbVisitReferenceType.ModifiedBy = visitReferenceToBeModify.ModifiedBy;
                            }
                        });
                        _repository.AutoSave = false;
                        _repository.Update(dbVisitReference);
                        if (commitChange)
                        {
                           var updCnt= _repository.ForceSave();
                            if (updCnt > 0)
                            {
                               recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                     null,
                                                                    ValidationType.Update.ToAuditActionType(),
                                                                    SqlAuditModuleType.VisitReference,
                                                                    domExistingVisitReference?.FirstOrDefault(x2 => x2.VisitReferenceId == x1.VisitReferenceId),
                                                                    x1,
                                                                    dbModule
                                                                    ));

                            }
                        }
                    }
                    else
                        return valdResponse;
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitReferences);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveVisitReference(IList<DomainModel.VisitReference> visitReferences,
                                                  ref IList<DbModel.VisitReference> dbAssignmentReference,
                                                  ref IList<DbModel.Visit> dbVisit,
                                                  ref IList<DbModel.Data> dbReferenceType,
                                                  IList<DbModel.SqlauditModule> dbModule,
                                                  bool commitChange,
                                                  bool isDbValidationRequired)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                Response response = null;
                var recordToBeDelete = FilterRecord(visitReferences, ValidationType.Delete);
                eventId = visitReferences?.FirstOrDefault().EventId;


                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(visitReferences,
                                                       ValidationType.Delete,
                                                       ref recordToBeDelete,
                                                       ref dbAssignmentReference,
                                                       ref dbVisit,
                                                       ref dbReferenceType);

                if (recordToBeDelete?.Count > 0 && (response == null || response.Code == MessageType.Success.ToId()))
                {
                    if (dbAssignmentReference?.Count > 0)
                    {
                        _repository.AutoSave = false;
                        _repository.Delete(dbAssignmentReference);
                        if (commitChange)
                        {
                           var delCnt= _repository.ForceSave();
                            if (delCnt > 0)
                            {
                                 visitReferences?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                          null,
                                                                                                        ValidationType.Delete.ToAuditActionType(),
                                                                                                        SqlAuditModuleType.VisitReference,
                                                                                                        x1,
                                                                                                        null,
                                                                                                        dbModule
                                                                                                        ));

                            }

                        }
                    }
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitReferences);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }


        private IList<DomainModel.VisitReference> FilterRecord(IList<DomainModel.VisitReference> visitReferences,
                                                                       ValidationType filterType)
        {
            IList<DomainModel.VisitReference> filteredVisitReference = null;

            if (filterType == ValidationType.Add)
                filteredVisitReference = visitReferences?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredVisitReference = visitReferences?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredVisitReference = visitReferences?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredVisitReference;
        }


        private bool IsValidPayload(IList<DomainModel.VisitReference> visitReferences,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _visitRefrenceValidationService.Validate(JsonConvert.SerializeObject(visitReferences), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Visit, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsValidReferenceType(IList<DomainModel.VisitReference> visitReferences,
                                         ref IList<DbModel.Data> dbReferenceTypes,
                                         ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            IList<DbModel.Data> dbReference = null;
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            IList<MasterType> types = new List<MasterType>() { MasterType.AssignmentReferenceType };

            var referenceTypeNames = visitReferences.Select(x => x.ReferenceType).ToList();
            if (dbReferenceTypes?.Count == 0 || dbReferenceTypes == null)
                dbReference = _masterService.Get(types);
            else
                dbReference = dbReferenceTypes;

            var referenceTypeNotExists = visitReferences.Where(x => !dbReference.Any(x1 => x1.Name == x.ReferenceType)).ToList();
            referenceTypeNotExists.ToList().ForEach(x =>
            { 
                messages.Add(_messages, x.ReferenceType, MessageType.VisitReferenceInvalid, x.ReferenceType);
            });

            if (dbReferenceTypes?.Count == 0 || dbReferenceTypes == null)
                dbReferenceTypes = dbReference;

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }



        private bool IsUniqueVisitRefrenceType(IList<DomainModel.VisitReference> visitReferences,
                                                   IList<DbModel.VisitReference> dbVisitReferences,
                                                   ValidationType validationType,
                                                   ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.VisitReference> referenceTypeExists = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            referenceTypeExists = _repository.IsUniqueVisitReference(visitReferences, dbVisitReferences, validationType);
            referenceTypeExists?.ToList().ForEach(x =>
            {
                messages.Add(_messages, x.AssignmentReferenceType.Name, MessageType.VisitReferenceDuplicateRecord, x.AssignmentReferenceType.Name);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count > 0;
        }


        private bool IsRecordUpdateCountMatching(IList<DomainModel.VisitReference> visitReferences,
                                                IList<DbModel.VisitReference> dbVisitReferences,
                                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = visitReferences.Where(x => !dbVisitReferences.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() &&
                                                                                                             x1.Id == x.VisitReferenceId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x.VisitReferenceId, MessageType.VisitReferenceUpdateCountMisMatch, x.VisitReferenceId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsVisitRefrenceExistInDb(IList<long> visitRefrenceId,
                                                  IList<DbModel.VisitReference> dbVisitReferences,
                                                  ref IList<long> visitRefrenceNotExists,
                                                  ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbVisitReferences == null)
                dbVisitReferences = new List<DbModel.VisitReference>();

            var validMessages = validationMessages;

            if (visitRefrenceId?.Count > 0)
            {
                visitRefrenceNotExists = visitRefrenceId.Where(x => !dbVisitReferences.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                visitRefrenceNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.VisitReferenceInvalid, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidForAdd(IList<DomainModel.VisitReference> visitReferences,
                                          IList<DbModel.VisitReference> dbVisitReference,
                                          ref IList<DbModel.Visit> dbVisit,
                                          ref IList<DbModel.Data> dbReferenceType,
                                          ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.Visit> dbVisits = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();


            if (dbVisit != null)
                dbVisits = dbVisit;

            var visitIds = visitReferences.Where(x => x.VisitId > 0).Select(x => (long)x.VisitId).Distinct().ToList();

            if (_visitService.IsValidVisit(visitIds, ref dbVisits, ref messages, null))
                if (IsValidReferenceType(visitReferences, ref dbReferenceType, ref messages))
                    IsUniqueVisitRefrenceType(visitReferences, dbVisitReference, ValidationType.Add, ref messages);

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsRecordValidForUpdate(IList<DomainModel.VisitReference> visitReferences,
                                             IList<DbModel.VisitReference> dbVisitReference,
                                             ref IList<DbModel.Visit> dbVisit,
                                             ref IList<DbModel.Data> dbRefrenceType,
                                             ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            IList<DbModel.Visit> dbVisits = null;
            if (dbVisit != null)
                dbVisits = dbVisit;

            var visitIds = visitReferences.Where(x => x.VisitId > 0).Select(x => (long)x.VisitId).Distinct().ToList();
            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(visitReferences, dbVisitReference, ref messages))
                    if (_visitService.IsValidVisit(visitIds, ref dbVisit, ref messages, new string[] { "VisitReference" }))
                        if (IsValidReferenceType(visitReferences, ref dbRefrenceType, ref messages))
                            IsUniqueVisitRefrenceType(visitReferences, dbVisit.ToList().SelectMany(x => x.VisitReference).ToList(), ValidationType.Update, ref messages);

            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private Response IsRecordValidForProcess(IList<DomainModel.VisitReference> visitReferences,
                                               ValidationType validationType,
                                               ref IList<DomainModel.VisitReference> filteredVisitRefrenceTypes,
                                               ref IList<DbModel.VisitReference> dbVisitReference,
                                               ref IList<DbModel.Visit> dbVisit,
                                               ref IList<DbModel.Data> dbReferenceType)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (visitReferences != null && visitReferences.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredVisitRefrenceTypes == null || filteredVisitRefrenceTypes.Count <= 0)
                        filteredVisitRefrenceTypes = FilterRecord(visitReferences, validationType);

                    if (filteredVisitRefrenceTypes != null && filteredVisitRefrenceTypes?.Count > 0)
                    {
                        result = IsValidPayload(filteredVisitRefrenceTypes,
                                                validationType,
                                                ref validationMessages);
                        if (filteredVisitRefrenceTypes?.Count > 0 && result)
                        {
                            IList<long> moduleNotExists = null;
                            var visitRefrenceTypeId = filteredVisitRefrenceTypes.Where(x => x.VisitReferenceId.HasValue)
                                                                                         .Select(x => (long)x.VisitReferenceId).Distinct().ToList();

                            if (dbVisitReference == null || dbVisitReference.Count <= 0)
                                dbVisitReference = GetVisitReference(filteredVisitRefrenceTypes, validationType);

                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                            {
                                result = IsVisitRefrenceExistInDb(visitRefrenceTypeId,
                                                                      dbVisitReference,
                                                                      ref moduleNotExists,
                                                                      ref validationMessages);

                                if (result && validationType == ValidationType.Update)
                                    result = IsRecordValidForUpdate(filteredVisitRefrenceTypes,
                                                                    dbVisitReference,
                                                                    ref dbVisit,
                                                                    ref dbReferenceType,
                                                                    ref validationMessages);
                            }
                            else if (validationType == ValidationType.Add)
                                result = IsRecordValidForAdd(filteredVisitRefrenceTypes,
                                                                dbVisitReference,
                                                                ref dbVisit,
                                                                ref dbReferenceType,
                                                                ref validationMessages);
                        }
                        else
                            result = false;
                    }

                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitReferences);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
    }

}
