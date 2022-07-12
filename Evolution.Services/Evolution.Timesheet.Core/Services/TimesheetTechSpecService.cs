using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
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
    public class TimesheetTechSpecService : ITimesheetTechSpecService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<TimesheetTechSpecService> _logger = null;
        private readonly ITimesheetTechnicalSpecialistRepository _repository = null;
        //private readonly ITimesheetService _timesheetService = null;
        private readonly ITimesheetRepository _timesheetRepository = null;
        private readonly ITechnicalSpecialistService _technicalSpecialistService = null;
        private readonly ITimesheetTechSpecValidationService _validationService = null;
        private readonly JObject _messages = null;
       private readonly IAuditSearchService _auditSearchService = null;

        public TimesheetTechSpecService(IAppLogger<TimesheetTechSpecService> logger,
                                        ITimesheetTechnicalSpecialistRepository technicalSpecialistRepository,
                                        //ITimesheetService timesheetService,
                                        ITimesheetRepository timesheetRepository,
                                        ITechnicalSpecialistService technicalSpecialistService,
                                        ITimesheetTechSpecValidationService techSpecValidationService,
                                        IMapper mapper,
                                        JObject messages,
                                        IAuditSearchService auditSearchService)

        {
            _mapper = mapper;
            _logger = logger;
            _repository = technicalSpecialistRepository;
            //_timesheetService = timesheetService;
            _timesheetRepository = timesheetRepository;
            _technicalSpecialistService = technicalSpecialistService;
            _validationService = techSpecValidationService;
            this._messages = messages;
            _auditSearchService = auditSearchService;
           
        }

        #region Get
        public Response Get(DomainModel.TimesheetTechnicalSpecialist searchModel)
        {
            DomainModel.TimesheetTechnicalSpecialistGrossMargin timesheetTechSpecs = new DomainModel.TimesheetTechnicalSpecialistGrossMargin();
            Exception exception = null;
            try
            {
                string[] includes = new string[] { "Timesheet", "TechnicalSpecialist" };
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                   new TransactionOptions
                                                   {
                                                       IsolationLevel = IsolationLevel.ReadUncommitted
                                                   }))
                {
                    timesheetTechSpecs.TimesheetTechnicalSpecialists = this._repository.Search(searchModel, includes);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, timesheetTechSpecs, exception);
        }
        #endregion

        #region Add
        public Response Add(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? timesheetId = null)
        {
            if (timesheetId.HasValue)
                timesheetTechnicalSpecialists?.ToList().ForEach(x => { x.TimesheetId = (long)timesheetId; });

            IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist = null;
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.SqlauditModule> dbModule = null;

            return AddTimesheetTechnicalSpecialist(timesheetTechnicalSpecialists,
                                                    ref dbTimesheetTechnicalSpecialists,
                                                    ref dbTechnicalSpecialist,
                                                    ref dbTimesheet,
                                                    dbModule,
                                                    commitChange,
                                                    isDbValidationRequired);
        }

        public Response Add(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                            ref IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialists,
                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                            ref IList<DbModel.Timesheet> dbTimesheet,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? timesheetId = null)
        {
            if (timesheetId.HasValue)
                timesheetTechnicalSpecialists?.ToList().ForEach(x => { x.TimesheetId = (long)timesheetId; });
            return AddTimesheetTechnicalSpecialist(timesheetTechnicalSpecialists,
                                                    ref dbTimesheetTechnicalSpecialists,
                                                    ref dbTechnicalSpecialist,
                                                    ref dbTimesheet,
                                                    dbModule,
                                                    commitChange,
                                                    isDbValidationRequired);
        }
        #endregion

        #region Modify
        public Response Modify(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? timesheetId = null)
        {
            if (timesheetId.HasValue)
                timesheetTechnicalSpecialists?.ToList().ForEach(x => { x.TimesheetId = (long)timesheetId; });

            IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialist = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist = null;
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            return UpdateTimesheetTechnicalSpecialist(timesheetTechnicalSpecialists,
                                                       ref dbTimesheetTechnicalSpecialist,
                                                       ref dbTechnicalSpecialist,
                                                       ref dbTimesheet,
                                                       dbModule,
                                                       commitChange,
                                                       isDbValidationRequired);
        }

        public Response Modify(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                                ref IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialist,
                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                ref IList<DbModel.Timesheet> dbTimesheet,
                                IList<DbModel.SqlauditModule> dbModule,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? timesheetId = null)
        {
            if (timesheetId.HasValue)
                timesheetTechnicalSpecialists?.ToList().ForEach(x => { x.TimesheetId = (long)timesheetId; });

            return UpdateTimesheetTechnicalSpecialist(timesheetTechnicalSpecialists,
                                                        ref dbTimesheetTechnicalSpecialist,
                                                        ref dbTechnicalSpecialist,
                                                        ref dbTimesheet,
                                                        dbModule,
                                                        commitChange,
                                                        isDbValidationRequired);
        }


        #endregion

        #region Delete
        public Response Delete(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               long? timesheetId = null)
        {
            IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist = null;
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            if (timesheetId.HasValue)
                timesheetTechnicalSpecialists?.ToList().ForEach(x => { x.TimesheetId = (long)timesheetId; });

            return this.RemoveTimesheetTechnicalSpecialist(timesheetTechnicalSpecialists,
                                                            ref dbTimesheetTechnicalSpecialists,
                                                            ref dbTechnicalSpecialist,
                                                            ref dbTimesheet,
                                                            dbModule,
                                                            commitChange,
                                                            isDbValidationRequired);
        }

        public Response Delete(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                               ref IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialists,
                               ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                               ref IList<DbModel.Timesheet> dbTimesheet,
                               IList<DbModel.SqlauditModule> dbModule,
                              bool commitChange = true,
                              bool isDbValidationRequired = true,
                              long? timesheetId = null)
        {
            if (timesheetId.HasValue)
                timesheetTechnicalSpecialists?.ToList().ForEach(x => { x.TimesheetId = (long)timesheetId; });

            return this.RemoveTimesheetTechnicalSpecialist(timesheetTechnicalSpecialists,
                                                            ref dbTimesheetTechnicalSpecialists,
                                                            ref dbTechnicalSpecialist,
                                                            ref dbTimesheet,
                                                            dbModule,
                                                            commitChange,
                                                            isDbValidationRequired);
        }


        #endregion

        #region Validation
        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetTechnicalSpecialist> timeTechnicalSpecialists,
                                                ValidationType validationType)
        {
            IList<DbModel.TimesheetTechnicalSpecialist> dbTimeTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialist> technicalSpecialists = null;
            IList<DbModel.Timesheet> dbTimesheets = null;
            return IsRecordValidForProcess(timeTechnicalSpecialists,
                                            validationType,
                                            ref dbTimesheets,
                                            ref dbTimeTechnicalSpecialists,
                                            ref technicalSpecialists);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetTechnicalSpecialist> timeTechnicalSpecialists,
                                                ValidationType validationType,
                                                ref IList<DbModel.Timesheet> dbTimesheets,
                                                ref IList<DbModel.TimesheetTechnicalSpecialist> dbTimeTechnicalSpecialists,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists)
        {
            IList<DomainModel.TimesheetTechnicalSpecialist> filteredAssignmentTechnicalSpecialist = null;
            return IsRecordValidForProcess(timeTechnicalSpecialists,
                                            validationType,
                                            ref filteredAssignmentTechnicalSpecialist,
                                            ref dbTimeTechnicalSpecialists,
                                            ref dbTimesheets,
                                            ref dbTechnicalSpecialists
                                           );
        }

        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetTechnicalSpecialist> timeTechnicalSpecialists,
                                                ValidationType validationType,
                                                IList<DbModel.Timesheet> dbTimesheets,
                                                IList<DbModel.TimesheetTechnicalSpecialist> dbTimeTechnicalSpecialists,
                                                IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists)
        {
            return IsRecordValidForProcess(timeTechnicalSpecialists,
                                            validationType,
                                            ref dbTimesheets,
                                            ref dbTimeTechnicalSpecialists,
                                            ref dbTechnicalSpecialists);
        }
        #endregion

        #region Private Methods
        private IList<DomainModel.TimesheetTechnicalSpecialist> FilterRecord(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                                                                              ValidationType filterType)
        {
            IList<DomainModel.TimesheetTechnicalSpecialist> filteredTimesheetTechnicalSpecialist = null;

            if (filterType == ValidationType.Add)
                filteredTimesheetTechnicalSpecialist = timesheetTechnicalSpecialists?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredTimesheetTechnicalSpecialist = timesheetTechnicalSpecialists?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredTimesheetTechnicalSpecialist = timesheetTechnicalSpecialists?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredTimesheetTechnicalSpecialist;
        }

        private bool IsValidPayload(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(timesheetTechnicalSpecialists), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Timesheet, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }


        private IList<DbModel.TimesheetTechnicalSpecialist> GetTimesheetTechnicalspecialist(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                                                                                            ValidationType validationType)
        {
            IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialists = null;
            if (validationType != ValidationType.Add)
            {
                if (timesheetTechnicalSpecialists?.Count > 0)
                {
                    var timesheetTechnicalSpecialistId = timesheetTechnicalSpecialists.Where(x => x.TimesheetTechnicalSpecialistId > 0).Select(x => x.TimesheetTechnicalSpecialistId).Distinct().ToList();
                    if (timesheetTechnicalSpecialistId != null && timesheetTechnicalSpecialistId.Count > 0)
                        dbTimesheetTechnicalSpecialists = _repository.FindBy(x => timesheetTechnicalSpecialistId.Contains(x.Id)).ToList();
                }
            }

            return dbTimesheetTechnicalSpecialists;
        }


        public bool IsTimesheetTechnicalSpecialistExistInDb(IList<long> timesheetTechnicalSpecialistIds,
                                                               IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialists,
                                                               ref IList<long> timesheetTechnicalSpecialistNotExists,
                                                               ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTimesheetTechnicalSpecialists == null)
                dbTimesheetTechnicalSpecialists = new List<DbModel.TimesheetTechnicalSpecialist>();

            var validMessages = validationMessages;

            if (timesheetTechnicalSpecialistIds?.Count > 0)
            {
                timesheetTechnicalSpecialistNotExists = timesheetTechnicalSpecialistIds.Where(x => !dbTimesheetTechnicalSpecialists.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                timesheetTechnicalSpecialistNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TimesheetTechnicalSpecialistNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                                                IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialists,
                                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = timesheetTechnicalSpecialists.Where(x => !dbTimesheetTechnicalSpecialists.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.TimesheetTechnicalSpecialistId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x.TimesheetTechnicalSpecialistId, MessageType.TimeTechnicalSpecialistUpdateCountMisMatch, x.TimesheetTechnicalSpecialistId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsUniqueTimesheetTechSpecialist(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                                                   IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialists,
                                                   ValidationType validationType,
                                                   ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.TimesheetTechnicalSpecialist> referenceTypeExists = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            referenceTypeExists = _repository.IsUniqueTimesheetTechspec(timesheetTechnicalSpecialists, dbTimesheetTechnicalSpecialists, validationType);
            referenceTypeExists?.ToList().ForEach(x =>
            {
                messages.Add(_messages, x.TechnicalSpecialist.Pin, MessageType.TimesheetTSDuplicateRecord, x.TechnicalSpecialist.Pin);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count > 0;
        }

        private bool IsTimesheetTechnicalSpecialistCanBeRemove(IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialists,
                                                               ref IList<ValidationMessage> messages)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (messages == null)
                messages = new List<ValidationMessage>();


            dbTimesheetTechnicalSpecialists.ToList()?.Where(x => x.IsAnyCollectionPropertyContainValue())
                                           .ToList()
                                           .ForEach(x =>
                                           {
                                               validationMessages.Add(_messages, dbTimesheetTechnicalSpecialists?.ToList()?.Where(x1 => x1.TechnicalSpecialistId == x.TechnicalSpecialistId)?.FirstOrDefault().TechnicalSpecialistId, MessageType.TimesheetTechnicalSpecialistIsBeingUsed, x.TechnicalSpecialistId);
                                           });

            if (validationMessages.Count > 0)
                messages.AddRange(validationMessages);

            return messages.Count <= 0;
        }


        private bool IsRecordValidForAdd(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                                         IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialists,
                                         ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                         ref IList<DbModel.Timesheet> dbTimesheets,
                                         ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var timesheetIds = timesheetTechnicalSpecialists.Where(x => x.TimesheetId > 0).Select(x => (long)x.TimesheetId).Distinct().ToList();
            var technicalSpecialistIds = timesheetTechnicalSpecialists.Where(x => x.Pin > 0).Select(x => x.Pin.ToString()).Distinct().ToList();

            if (IsValidTimesheet(timesheetIds, ref dbTimesheets, ref validationMessages, null)
                && Convert.ToBoolean(_technicalSpecialistService.IsRecordExistInDb(technicalSpecialistIds, ref dbTechnicalSpecialists, ref validationMessages).Result)
                && IsUniqueTimesheetTechSpecialist(timesheetTechnicalSpecialists, dbTimesheetTechnicalSpecialists, ValidationType.Add, ref validationMessages))

                messages = validationMessages;
            return messages?.Count <= 0;
        }

        public bool IsValidTimesheet(IList<long> timesheetId,
                                    ref IList<DbModel.Timesheet> dbTimesheets,
                                    ref IList<ValidationMessage> messages,
                                    params string[] includes)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (dbTimesheets == null)
            {
                //var dbTimesheet = _repository?.FindBy(x => timesheetId.Contains(x.Id), includes)?.ToList();
                var dbTimesheet = _timesheetRepository.FetchTimesheets(timesheetId, includes);
                var timesheetNotExists = timesheetId?.Where(x => !dbTimesheet.Any(x2 => x2.Id == x))?.ToList();
                timesheetNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.TimesheetNotExists.ToId();
                    message.Add(_messages, x, MessageType.TimesheetNotExists, x);
                });
                dbTimesheets = dbTimesheet;
                messages = message;
            }

            return messages?.Count <= 0;
        }

        private bool IsRecordValidForUpdate(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                                            IList<DbModel.TimesheetTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<DbModel.Timesheet> dbTimesheets,
                                            ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            IList<DbModel.Timesheet> dbTimesheet = null;
            if (dbTimesheets != null)
                dbTimesheet = dbTimesheets;

            var timesheetIds = timesheetTechnicalSpecialists.Where(x => x.TimesheetId > 0).Select(x => (long)x.TimesheetId).Distinct().ToList();
            var technicalSpecialistIds = timesheetTechnicalSpecialists.Where(x => x.Pin > 0).Select(x => x.Pin.ToString()).Distinct().ToList();
            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(timesheetTechnicalSpecialists, dbAssignmentTechnicalSpecialist, ref messages))
                    if (IsValidTimesheet(timesheetIds, ref dbTimesheet, ref messages, new string[] { "TimesheetTechnicalSpecialist", "TimesheetTechnicalSpecialist.TechnicalSpecialist" }))
                        if (Convert.ToBoolean(_technicalSpecialistService.IsRecordExistInDb(technicalSpecialistIds, ref dbTechnicalSpecialists, ref messages).Result))
                            IsUniqueTimesheetTechSpecialist(timesheetTechnicalSpecialists, dbTimesheet.ToList().SelectMany(x => x.TimesheetTechnicalSpecialist).ToList(), ValidationType.Update, ref messages);

            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }



        private Response IsRecordValidForProcess(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                                               ValidationType validationType,
                                               ref IList<DomainModel.TimesheetTechnicalSpecialist> filteredTimesheetTechSpec,
                                               ref IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialist,
                                               ref IList<DbModel.Timesheet> dbTimesheet,
                                               ref IList<DbModel.TechnicalSpecialist> dbTechSpecialist)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (timesheetTechnicalSpecialists != null && timesheetTechnicalSpecialists.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredTimesheetTechSpec == null || filteredTimesheetTechSpec.Count <= 0)
                        filteredTimesheetTechSpec = FilterRecord(timesheetTechnicalSpecialists, validationType);

                    if (filteredTimesheetTechSpec != null && filteredTimesheetTechSpec?.Count > 0)
                    {
                        result = IsValidPayload(filteredTimesheetTechSpec,
                                                validationType,
                                                ref validationMessages);
                        if (filteredTimesheetTechSpec?.Count > 0 && result)
                        {
                            IList<long> moduleNotExists = null;
                            var timesheetTechSpecId = filteredTimesheetTechSpec.Where(x => x.TimesheetTechnicalSpecialistId > 0)
                                                                                         .Select(x => (long)x.TimesheetTechnicalSpecialistId).Distinct().ToList();

                            if (dbTimesheetTechnicalSpecialist == null || dbTimesheetTechnicalSpecialist.Count <= 0)
                                dbTimesheetTechnicalSpecialist = GetTimesheetTechnicalspecialist(filteredTimesheetTechSpec, validationType);

                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                            {
                                result = IsTimesheetTechnicalSpecialistExistInDb(timesheetTechSpecId,
                                                                      dbTimesheetTechnicalSpecialist,
                                                                      ref moduleNotExists,
                                                                      ref validationMessages);
                                                                      
                                // This Validation not required currently.
                                // if (result && validationType == ValidationType.Delete)
                                //     result = IsTimesheetTechnicalSpecialistCanBeRemove(dbTimesheetTechnicalSpecialist,
                                //                                               ref validationMessages);

                                if (result && validationType == ValidationType.Update)
                                    result = IsRecordValidForUpdate(filteredTimesheetTechSpec,
                                                                    dbTimesheetTechnicalSpecialist,
                                                                    ref dbTechSpecialist,
                                                                    ref dbTimesheet,
                                                                    ref validationMessages);
                            }
                            else if (validationType == ValidationType.Add)
                                result = IsRecordValidForAdd(filteredTimesheetTechSpec,
                                                                dbTimesheetTechnicalSpecialist,
                                                                ref dbTechSpecialist,
                                                                ref dbTimesheet,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetTechnicalSpecialists);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response AddTimesheetTechnicalSpecialist(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                                                        ref IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialists,
                                                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                                        ref IList<DbModel.Timesheet> dbTimesheet,
                                                        IList<DbModel.SqlauditModule> dbModule,
                                                        bool commitChange,
                                                        bool isDbValidationRequired)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(timesheetTechnicalSpecialists, ValidationType.Add);
                eventId = timesheetTechnicalSpecialists.FirstOrDefault().EventId;
                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(timesheetTechnicalSpecialists,
                                                            ValidationType.Add,
                                                            ref recordToBeAdd,
                                                            ref dbTimesheetTechnicalSpecialists,
                                                            ref dbTimesheet,
                                                            ref dbTechnicalSpecialist);
                if (dbTimesheetTechnicalSpecialists?.Count <= 0)
                    dbTimesheetTechnicalSpecialists = GetTimesheetTechnicalspecialist(recordToBeAdd, ValidationType.Add);


                if (recordToBeAdd?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    _repository.AutoSave = false;
                    dbTechnicalSpecialists = dbTechnicalSpecialist;

                    dbTimesheetTechnicalSpecialists = _mapper.Map<IList<DbModel.TimesheetTechnicalSpecialist>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isTimesheetTSId"] = false;
                        opt.Items["TechSpec"] = dbTechnicalSpecialists;

                    });

                    _repository.Add(dbTimesheetTechnicalSpecialists);
                    if (commitChange)
                    {
                       int value= _repository.ForceSave();
                        dbTimesheetTechnicalSpecialists?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, timesheetTechnicalSpecialists?.FirstOrDefault()?.ActionByUser,
                                                                                                null,
                                                                                                ValidationType.Add.ToAuditActionType(),
                                                                                                SqlAuditModuleType.TimesheetTechnicalSpecialist,
                                                                                                null,
                                                                                                _mapper.Map<DomainModel.TimesheetTechnicalSpecialist>(x1)
                                                                                                ,dbModule  ));
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetTechnicalSpecialists);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateTimesheetTechnicalSpecialist(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                                                            ref IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialist,
                                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                                            ref IList<DbModel.Timesheet> dbTimesheet,
                                                            IList<DbModel.SqlauditModule> dbModule,
                                                            bool commitChange,
                                                            bool isDbValidationRequired)
        {
            Exception exception = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DomainModel.TimesheetReferenceType> result = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(timesheetTechnicalSpecialists, ValidationType.Update);
                eventId = timesheetTechnicalSpecialists?.FirstOrDefault()?.EventId;
                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(timesheetTechnicalSpecialists,
                                                            ValidationType.Update,
                                                            ref recordToBeModify,
                                                            ref dbTimesheetTechnicalSpecialist,
                                                            ref dbTimesheet,
                                                            ref dbTechnicalSpecialist);
                else if (dbTimesheetTechnicalSpecialist?.Count <= 0)
                    dbTimesheetTechnicalSpecialist = GetTimesheetTechnicalspecialist(recordToBeModify, ValidationType.Update);


                IList<DomainModel.TimesheetTechnicalSpecialist> domExistingTimeSheetTechSpl = new List<DomainModel.TimesheetTechnicalSpecialist>();
                dbTimesheetTechnicalSpecialist?.ToList()?.ForEach(x =>
                {
                    domExistingTimeSheetTechSpl.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.TimesheetTechnicalSpecialist>(x)));
                });


                if (recordToBeModify?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    if (dbTimesheetTechnicalSpecialist?.Count > 0)
                    {
                        dbTechnicalSpecialists = dbTechnicalSpecialist;
                        dbTimesheetTechnicalSpecialist.ToList().ForEach(dbTimesheetTS =>
                        {
                            var timesheetTSToBeModify = recordToBeModify.FirstOrDefault(x => x.TimesheetTechnicalSpecialistId == dbTimesheetTS.Id);
                            if (timesheetTSToBeModify != null)
                            {
                                dbTimesheetTS.TechnicalSpecialistId = dbTechnicalSpecialists.FirstOrDefault(x1 => x1.Pin == timesheetTSToBeModify.Pin).Id;
                                dbTimesheetTS.TimesheetId = (long)timesheetTSToBeModify.TimesheetId;
                                dbTimesheetTS.LastModification = DateTime.UtcNow;
                                dbTimesheetTS.UpdateCount = timesheetTSToBeModify.UpdateCount.CalculateUpdateCount();
                                dbTimesheetTS.ModifiedBy = timesheetTSToBeModify.ModifiedBy;
                            }
                        });
                        _repository.AutoSave = false;
                        _repository.Update(dbTimesheetTechnicalSpecialist);
                        if (commitChange)
                        {
                            int value= _repository.ForceSave();
                            if (value > 0)
                            {
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                       null,
                                                                       ValidationType.Update.ToAuditActionType(),
                                                                       SqlAuditModuleType.TimesheetTechnicalSpecialist,
                                                                       domExistingTimeSheetTechSpl?.FirstOrDefault(x2 => x2.TimesheetTechnicalSpecialistId == x1.TimesheetTechnicalSpecialistId),
                                                                       x1,dbModule
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetTechnicalSpecialists);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveTimesheetTechnicalSpecialist(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                                                            ref IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechnicalSpecialists,
                                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                                            ref IList<DbModel.Timesheet> dbTimesheet,
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
                var recordToBeDelete = FilterRecord(timesheetTechnicalSpecialists, ValidationType.Delete);
                eventId = timesheetTechnicalSpecialists?.FirstOrDefault()?.EventId;

                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(timesheetTechnicalSpecialists,
                                                       ValidationType.Delete,
                                                       ref recordToBeDelete,
                                                       ref dbTimesheetTechnicalSpecialists,
                                                       ref dbTimesheet,
                                                       ref dbTechnicalSpecialist);

                if (recordToBeDelete?.Count > 0 && (response == null || response.Code == MessageType.Success.ToId()))
                {
                    if (dbTimesheetTechnicalSpecialists?.Count > 0)
                    {
                        _repository.AutoSave = false;
                        _repository.Delete(dbTimesheetTechnicalSpecialists);
                        if (commitChange)
                        {
                           int value= _repository.ForceSave();
                            if (value > 0)
                            {
                                timesheetTechnicalSpecialists?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                                  null,
                                                                                                                  ValidationType.Delete.ToAuditActionType(),
                                                                                                                  SqlAuditModuleType.TimesheetTechnicalSpecialist,
                                                                                                                  x1,
                                                                                                                  null, dbModule
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetTechnicalSpecialists);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        #endregion
    }
}
