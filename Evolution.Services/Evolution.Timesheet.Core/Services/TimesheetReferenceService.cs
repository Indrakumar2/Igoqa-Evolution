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
using Evolution.Timesheet.Domain.Interfaces.Data;
using Evolution.Timesheet.Domain.Interfaces.Timesheets;
using Evolution.Timesheet.Domain.Interfaces.Validations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;

namespace Evolution.Timesheet.Core.Services
{
    public class TimesheetReferenceService : ITimesheetReferenceService
    {
        private IMapper _mapper = null;
        private IAppLogger<TimesheetReferenceService> _logger = null;
        private ITimesheetReferenceRepository _repository = null;
        private ITimesheetReferenceValidationService _timesheetRefrenceValidationService = null;
        private IMasterService _masterService = null;
        private readonly ITimesheetService _timesheetService = null;
        private readonly JObject _messages = null;
        private readonly IAuditSearchService _auditSearchService = null;

        public TimesheetReferenceService(IAppLogger<TimesheetReferenceService> logger,
                                         ITimesheetReferenceRepository timesheetReferenceRepository,
                                         ITimesheetReferenceValidationService timesheetRefrenceValidationService,
                                         ITimesheetService timesheetService,
                                         IMasterService masterService,
                                         IMapper mapper,
                                         JObject messages,
                                        IAuditSearchService auditSearchService)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = timesheetReferenceRepository;
            _timesheetRefrenceValidationService = timesheetRefrenceValidationService;
            _timesheetService = timesheetService;
            _masterService = masterService;
            this._messages = messages;
            _auditSearchService = auditSearchService;
        }

        #region Public Methods
        public Response Get(DomainModel.TimesheetReferenceType searchModel)
        {
            IList<DomainModel.TimesheetReferenceType> result = null;
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

        public Response Add(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? timesheetId = null)
        {
            IList<DbModel.Data> dbReferenceType = null;
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.TimesheetReference> dbTimesheetReference = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            if (timesheetId.HasValue)
                timesheetReferences?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return AddTimesheetReference(timesheetReferences,
                                         ref dbTimesheetReference,
                                         ref dbTimesheet,
                                         ref dbReferenceType,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }

        public Response Add(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                            ref IList<DbModel.TimesheetReference> dbTimesheetReferences,
                            ref IList<DbModel.Timesheet> dbTimesheet,
                            ref IList<DbModel.Data> dbReferenceType,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? timesheetId = null)
        {
            if (timesheetId.HasValue)
                timesheetReferences?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return AddTimesheetReference(timesheetReferences,
                                       ref dbTimesheetReferences,
                                       ref dbTimesheet,
                                       ref dbReferenceType,
                                       dbModule,
                                       commitChange,
                                       isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               long? timesheetId = null)
        {
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.Data> dbReferenceType = null;
            IList<DbModel.TimesheetReference> dbTimesheetReference = null;
            IList<DbModel.SqlauditModule> dbModule=null;
            if (timesheetId.HasValue)
                timesheetReferences?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return this.RemoveTimesheetReference(timesheetReferences,
                                                  ref dbTimesheetReference,
                                                  ref dbTimesheet,
                                                  ref dbReferenceType,
                                                  dbModule,
                                                  commitChange,
                                                  isDbValidationRequired);
        }



        public Response Delete(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                               ref IList<DbModel.TimesheetReference> dbTimesheetReferences,
                               ref IList<DbModel.Timesheet> dbTimesheet,
                               ref IList<DbModel.Data> dbReferenceType,
                               IList<DbModel.SqlauditModule> dbModule,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               long? timesheetId = null)
        {
            if (timesheetId.HasValue)
                timesheetReferences?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return this.RemoveTimesheetReference(timesheetReferences,
                                                  ref dbTimesheetReferences,
                                                  ref dbTimesheet,
                                                  ref dbReferenceType,
                                                  dbModule,
                                                  commitChange,
                                                  isDbValidationRequired);
        }


        public Response Modify(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? timesheetId = null)
        {
            IList<DbModel.Data> dbReferenceType = null;
            IList<DbModel.Timesheet> dbTimesheet = null;
            if (timesheetId.HasValue)
                timesheetReferences?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            IList<DbModel.TimesheetReference> dbTimesheetReference = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            return UpdateTimesheetReference(timesheetReferences,
                                             ref dbTimesheetReference,
                                             ref dbTimesheet,
                                             ref dbReferenceType,
                                             dbModule,
                                             commitChange,
                                             isDbValidationRequired);
        }



        public Response Modify(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                ref IList<DbModel.TimesheetReference> dbTimesheetReferences,
                                ref IList<DbModel.Timesheet> dbTimesheet,
                                ref IList<DbModel.Data> dbReferenceType,
                                IList<DbModel.SqlauditModule> dbModule,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? timesheetId = null)
        {
            if (timesheetId.HasValue)
                timesheetReferences?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return UpdateTimesheetReference(timesheetReferences,
                                             ref dbTimesheetReferences,
                                             ref dbTimesheet,
                                             ref dbReferenceType,
                                             dbModule,
                                             commitChange,
                                             isDbValidationRequired);
        }


        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                                ValidationType validationType)
        {
            IList<DbModel.TimesheetReference> dbTimesheetReferences = null;
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.Data> dbReferenceType = null;
            return IsRecordValidForProcess(timesheetReferences,
                                            validationType,
                                            ref dbTimesheetReferences,
                                            ref dbTimesheet,
                                            ref dbReferenceType);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                                ValidationType validationType,
                                                ref IList<DbModel.TimesheetReference> dbTimesheetReferences,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
                                                ref IList<DbModel.Data> dbReferenceType)
        {
            IList<DomainModel.TimesheetReferenceType> filteredTimesheetReference = null;
            return IsRecordValidForProcess(timesheetReferences,
                                            validationType,
                                            ref filteredTimesheetReference,
                                            ref dbTimesheetReferences,
                                            ref dbTimesheet,
                                            ref dbReferenceType);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                                ValidationType validationType,
                                                IList<DbModel.TimesheetReference> dbTimesheetReferences,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
                                                ref IList<DbModel.Data> dbReferenceType)
        {
            return IsRecordValidForProcess(timesheetReferences,
                                          validationType,
                                          ref dbTimesheetReferences,
                                          ref dbTimesheet,
                                          ref dbReferenceType);
        }
        #endregion

        #region Private Methods

        private IList<DbModel.TimesheetReference> GetTimesheetReference(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                                                        ValidationType validationType)
        {
            IList<DbModel.TimesheetReference> dbTimesheetRefrenceType = null;
            if (validationType != ValidationType.Add)
            {
                if (timesheetReferences?.Count > 0)
                {
                    var timesheetRefrenceTypeId = timesheetReferences.Select(x => x.TimesheetReferenceId).Distinct().ToList();
                    dbTimesheetRefrenceType = _repository.FindBy(x => timesheetRefrenceTypeId.Contains(x.Id)).ToList();
                }
            }
            return dbTimesheetRefrenceType;
        }

        private Response AddTimesheetReference(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                               ref IList<DbModel.TimesheetReference> dbTimesheetReferences,
                                               ref IList<DbModel.Timesheet> dbTimesheet,
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
                var recordToBeAdd = FilterRecord(timesheetReferences, ValidationType.Add);
                eventId = timesheetReferences.FirstOrDefault().EventId;

                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(timesheetReferences,
                                                            ValidationType.Add,
                                                            ref recordToBeAdd,
                                                            ref dbTimesheetReferences,
                                                            ref dbTimesheet,
                                                            ref dbReferenceType);
                else if (dbTimesheetReferences?.Count <= 0)
                    dbTimesheetReferences = GetTimesheetReference(recordToBeAdd, ValidationType.Add);

                if (recordToBeAdd?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    _repository.AutoSave = false;
                    dbReferenceTypes = dbReferenceType;
                    dbTimesheetReferences = _mapper.Map<IList<DbModel.TimesheetReference>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isTimesheetId"] = false;
                        opt.Items["ReferenceTypes"] = dbReferenceTypes;
                    });

                    _repository.Add(dbTimesheetReferences);
                    if (commitChange)
                    {
                      int value=  _repository.ForceSave();
                       
                        dbTimesheetReferences?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, timesheetReferences?.FirstOrDefault()?.ActionByUser,
                                                                                                null,
                                                                                                ValidationType.Add.ToAuditActionType(),
                                                                                                SqlAuditModuleType.TimesheetReference,
                                                                                                null,
                                                                                                _mapper.Map<DomainModel.TimesheetReferenceType>(x1),
                                                                                                dbModule
                                                                                                  ));


                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetReferences);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateTimesheetReference(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                                  ref IList<DbModel.TimesheetReference> dbTimesheetReference,
                                                  ref IList<DbModel.Timesheet> dbTimesheet,
                                                  ref IList<DbModel.Data> dbReferenceType,
                                                  IList<DbModel.SqlauditModule> dbModule,
                                                  bool commitChange,
                                                  bool isDbValidationRequired)
        {
            Exception exception = null;
            IList<DbModel.Data> dbReferenceTypes = null;
            IList<DomainModel.TimesheetReferenceType> result = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(timesheetReferences, ValidationType.Update);
                eventId = timesheetReferences?.FirstOrDefault()?.EventId;
                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(timesheetReferences,
                                                            ValidationType.Update,
                                                            ref recordToBeModify,
                                                            ref dbTimesheetReference,
                                                            ref dbTimesheet,
                                                            ref dbReferenceType);
                else if (dbTimesheetReference?.Count <= 0)
                    dbTimesheetReference = GetTimesheetReference(recordToBeModify, ValidationType.Update);

                if (recordToBeModify?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    if (dbTimesheetReference?.Count > 0)
                    {
                        dbReferenceTypes = dbReferenceType;
                        IList<DomainModel.TimesheetReferenceType> domExistingTimeSheetReference = new List<DomainModel.TimesheetReferenceType>();
                        dbTimesheetReference.ToList().ForEach(x =>
                        {
                            domExistingTimeSheetReference.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.TimesheetReferenceType>(x)));
                        });

                        dbTimesheetReference.ToList().ForEach(dbTimesheetReferenceType =>
                        {
                            var timesheetReferenceToBeModify = recordToBeModify.FirstOrDefault(x => x.TimesheetReferenceId == dbTimesheetReferenceType.Id);
                            if (timesheetReferenceToBeModify != null)
                            {
                                dbTimesheetReferenceType.AssignmentReferenceTypeId = dbReferenceTypes.FirstOrDefault(x1 => x1.Name == timesheetReferenceToBeModify.ReferenceType).Id;
                                dbTimesheetReferenceType.TimesheetId = (long)timesheetReferenceToBeModify.TimesheetId;
                                dbTimesheetReferenceType.ReferenceValue = timesheetReferenceToBeModify.ReferenceValue;
                                dbTimesheetReferenceType.LastModification = DateTime.UtcNow;
                                dbTimesheetReferenceType.UpdateCount = timesheetReferenceToBeModify.UpdateCount.CalculateUpdateCount();
                                dbTimesheetReferenceType.ModifiedBy = timesheetReferenceToBeModify.ModifiedBy;
                            }
                        });
                        _repository.AutoSave = false;
                        _repository.Update(dbTimesheetReference);
                        if (commitChange)
                        {
                           int value= _repository.ForceSave();
                            if (value > 0)
                            {
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                        null,
                                                                       ValidationType.Update.ToAuditActionType(),
                                                                       SqlAuditModuleType.TimesheetReference,
                                                                       domExistingTimeSheetReference?.FirstOrDefault(x2 => x2.TimesheetReferenceId == x1.TimesheetReferenceId),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetReferences);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveTimesheetReference(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                                  ref IList<DbModel.TimesheetReference> dbAssignmentReference,
                                                  ref IList<DbModel.Timesheet> dbTimesheet,
                                                  ref IList<DbModel.Data> dbReferenceType,
                                                  IList<DbModel.SqlauditModule> dbModule,
                                                  bool commitChange,
                                                  bool isDbValidationRequired)
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
                var recordToBeDelete = FilterRecord(timesheetReferences, ValidationType.Delete);
                eventId = timesheetReferences?.FirstOrDefault()?.EventId;

                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(timesheetReferences,
                                                       ValidationType.Delete,
                                                       ref recordToBeDelete,
                                                       ref dbAssignmentReference,
                                                       ref dbTimesheet,
                                                       ref dbReferenceType);

                if (recordToBeDelete?.Count > 0 && (response == null || response.Code == MessageType.Success.ToId()))
                {
                    if (dbAssignmentReference?.Count > 0)
                    {
                        _repository.AutoSave = false;
                        _repository.Delete(dbAssignmentReference);
                        if (commitChange)
                        {
                           int value= _repository.ForceSave();
                            recordToBeDelete?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser, null,
                                                                                                   ValidationType.Delete.ToAuditActionType(),
                                                                                                   SqlAuditModuleType.TimesheetReference,
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
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetReferences);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }


        private IList<DomainModel.TimesheetReferenceType> FilterRecord(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                                                       ValidationType filterType)
        {
            IList<DomainModel.TimesheetReferenceType> filteredTimesheetReference = null;

            if (filterType == ValidationType.Add)
                filteredTimesheetReference = timesheetReferences?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredTimesheetReference = timesheetReferences?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredTimesheetReference = timesheetReferences?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredTimesheetReference;
        }


        private bool IsValidPayload(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _timesheetRefrenceValidationService.Validate(JsonConvert.SerializeObject(timesheetReferences), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Timesheet, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsValidReferenceType(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                         ref IList<DbModel.Data> dbReferenceTypes,
                                         ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            IList<DbModel.Data> dbReference = null;
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            IList<MasterType> types = new List<MasterType>() { MasterType.AssignmentReferenceType };

            var referenceTypeNames = timesheetReferences.Select(x => x.ReferenceType).ToList();
            if (dbReferenceTypes?.Count == 0 || dbReferenceTypes == null)
                dbReference = _masterService.Get(types);
            else
                dbReference = dbReferenceTypes;

            var referenceTypeNotExists = timesheetReferences.Where(x => !dbReference.Any(x1 => x1.Name == x.ReferenceType)).ToList();
            referenceTypeNotExists.ToList().ForEach(x =>
            {
                messages.Add(_messages, x.ReferenceType, MessageType.TimesheetReferenceInvalid, x.ReferenceType);
            });

            if (dbReferenceTypes?.Count == 0 || dbReferenceTypes == null)
                dbReferenceTypes = dbReference;

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }



        private bool IsUniqueTimesheetRefrenceType(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                                   IList<DbModel.TimesheetReference> dbTimesheetReferences,
                                                   ValidationType validationType,
                                                   ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.TimesheetReference> referenceTypeExists = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            referenceTypeExists = _repository.IsUniqueTimesheetReference(timesheetReferences, dbTimesheetReferences, validationType);
            referenceTypeExists?.ToList().ForEach(x =>
            {
                messages.Add(_messages, x.AssignmentReferenceType.Name, MessageType.TimesheetReferenceDuplicateRecord, x.AssignmentReferenceType.Name);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count > 0;
        }


        private bool IsRecordUpdateCountMatching(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                                IList<DbModel.TimesheetReference> dbTimesheetReferences,
                                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = timesheetReferences.Where(x => !dbTimesheetReferences.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() &&
                                                                                                             x1.Id == x.TimesheetReferenceId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x.TimesheetReferenceId, MessageType.TimesheetReferenceUpdateCountMisMatch, x.TimesheetReferenceId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsTimesheetRefrenceExistInDb(IList<long> timesheetRefrenceId,
                                                  IList<DbModel.TimesheetReference> dbTimesheetReferences,
                                                  ref IList<long> timesheetRefrenceNotExists,
                                                  ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTimesheetReferences == null)
                dbTimesheetReferences = new List<DbModel.TimesheetReference>();

            var validMessages = validationMessages;

            if (timesheetRefrenceId?.Count > 0)
            {
                timesheetRefrenceNotExists = timesheetRefrenceId.Where(x => !dbTimesheetReferences.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                timesheetRefrenceNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TimesheetReferenceInvalid, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidForAdd(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                          IList<DbModel.TimesheetReference> dbTimesheetReference,
                                          ref IList<DbModel.Timesheet> dbTimesheet,
                                          ref IList<DbModel.Data> dbReferenceType,
                                          ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.Timesheet> dbTimesheets = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();


            if (dbTimesheet != null)
                dbTimesheets = dbTimesheet;

            var timesheetIds = timesheetReferences.Where(x => x.TimesheetId > 0).Select(x => (long)x.TimesheetId).Distinct().ToList();

            if (_timesheetService.IsValidTimesheet(timesheetIds, ref dbTimesheets, ref messages, null))
                if (IsValidReferenceType(timesheetReferences, ref dbReferenceType, ref messages))
                    IsUniqueTimesheetRefrenceType(timesheetReferences, dbTimesheetReference, ValidationType.Add, ref messages);

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsRecordValidForUpdate(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                             IList<DbModel.TimesheetReference> dbTimesheetReference,
                                             ref IList<DbModel.Timesheet> dbTimesheet,
                                             ref IList<DbModel.Data> dbRefrenceType,
                                             ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            IList<DbModel.Timesheet> dbTimesheets = null;
            if (dbTimesheet != null)
                dbTimesheets = dbTimesheet;

            var timesheetIds = timesheetReferences.Where(x => x.TimesheetId > 0).Select(x => (long)x.TimesheetId).Distinct().ToList();
            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(timesheetReferences, dbTimesheetReference, ref messages))
                    if (_timesheetService.IsValidTimesheet(timesheetIds, ref dbTimesheet, ref messages, new string[] { "TimesheetReference" }))
                        if (IsValidReferenceType(timesheetReferences, ref dbRefrenceType, ref messages))
                            IsUniqueTimesheetRefrenceType(timesheetReferences, dbTimesheet.ToList().SelectMany(x => x.TimesheetReference).ToList(), ValidationType.Update, ref messages);

            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private Response IsRecordValidForProcess(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                               ValidationType validationType,
                                               ref IList<DomainModel.TimesheetReferenceType> filteredTimesheetRefrenceTypes,
                                               ref IList<DbModel.TimesheetReference> dbTimesheetReference,
                                               ref IList<DbModel.Timesheet> dbTimesheet,
                                               ref IList<DbModel.Data> dbReferenceType)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (timesheetReferences != null && timesheetReferences.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredTimesheetRefrenceTypes == null || filteredTimesheetRefrenceTypes.Count <= 0)
                        filteredTimesheetRefrenceTypes = FilterRecord(timesheetReferences, validationType);

                    if (filteredTimesheetRefrenceTypes != null && filteredTimesheetRefrenceTypes?.Count > 0)
                    {
                        result = IsValidPayload(filteredTimesheetRefrenceTypes,
                                                validationType,
                                                ref validationMessages);
                        if (filteredTimesheetRefrenceTypes?.Count > 0 && result)
                        {
                            IList<long> moduleNotExists = null;
                            var timesheetRefrenceTypeId = filteredTimesheetRefrenceTypes.Where(x => x.TimesheetReferenceId.HasValue)
                                                                                         .Select(x => (long)x.TimesheetReferenceId).Distinct().ToList();

                            if (dbTimesheetReference == null || dbTimesheetReference.Count <= 0)
                                dbTimesheetReference = GetTimesheetReference(filteredTimesheetRefrenceTypes, validationType);

                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                            {
                                result = IsTimesheetRefrenceExistInDb(timesheetRefrenceTypeId,
                                                                      dbTimesheetReference,
                                                                      ref moduleNotExists,
                                                                      ref validationMessages);

                                if (result && validationType == ValidationType.Update)
                                    result = IsRecordValidForUpdate(filteredTimesheetRefrenceTypes,
                                                                    dbTimesheetReference,
                                                                    ref dbTimesheet,
                                                                    ref dbReferenceType,
                                                                    ref validationMessages);
                            }
                            else if (validationType == ValidationType.Add)
                                result = IsRecordValidForAdd(filteredTimesheetRefrenceTypes,
                                                                dbTimesheetReference,
                                                                ref dbTimesheet,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetReferences);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
        

        #endregion
    }
}
