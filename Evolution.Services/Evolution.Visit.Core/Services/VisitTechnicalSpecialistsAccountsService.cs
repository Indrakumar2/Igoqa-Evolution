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
    public class VisitTechnicalSpecialistsAccountsService : IVisitTechnicalSpecilaistAccountsService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<VisitTechnicalSpecialistsAccountsService> _logger = null;
        private readonly IVisitTechnicalSpecialistsAccountRepository _repository = null;
        private readonly IVisitRepository _visitRepository = null;
        private readonly ITechnicalSpecialistService _technicalSpecialistService = null;
        private readonly IVisitTechnicalSpecialistAccountsValidationService _validationService = null;
        private readonly JObject _messages = null;
        private readonly IAuditSearchService _auditSearchService = null;


        public VisitTechnicalSpecialistsAccountsService(IAppLogger<VisitTechnicalSpecialistsAccountsService> logger,
                                        IVisitTechnicalSpecialistsAccountRepository technicalSpecialistRepository,
                                        IVisitRepository visitRepository,
                                        ITechnicalSpecialistService technicalSpecialistService,
                                        IVisitTechnicalSpecialistAccountsValidationService techSpecValidationService,
                                        IMapper mapper,
                                        JObject messages,
                                        IAuditSearchService auditSearchService)
                                    
        {
            _mapper = mapper;
            _logger = logger;
            _repository = technicalSpecialistRepository;
            _visitRepository = visitRepository;
            _technicalSpecialistService = technicalSpecialistService;
            _validationService = techSpecValidationService;
            this._messages = messages;   
            _auditSearchService = auditSearchService;
        }

        #region Get
        public Response Get(DomainModel.VisitTechnicalSpecialist searchModel)
        {
            //DomainModel.VisitTechnicalSpecialistGrossMargin visitTechSpecs = new DomainModel.VisitTechnicalSpecialistGrossMargin();
            IList<DomainModel.VisitTechnicalSpecialist> technicalSpecialists = null;
            Exception exception = null;
            try
            {
                string[] includes = new string[] { "Visit", "TechnicalSpecialist" };
                //visitTechSpecs.VisitTechnicalSpecialists = this._repository.Search(searchModel, includes);
                //technicalSpecialists = this._repository.Search(searchModel, includes);
                 using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                   new TransactionOptions
                                                   {
                                                       IsolationLevel = IsolationLevel.ReadUncommitted
                                                   }))
                {
                    technicalSpecialists = this._repository.Search(searchModel, includes);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, technicalSpecialists, exception);
            //return new Response().ToPopulate(ResponseType.Success, null, null, null, visitTechSpecs, exception);
        }
        #endregion

        #region Add
        public Response Add(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? visitId = null)
        {
            if (visitId.HasValue)
                visitTechnicalSpecialists?.ToList().ForEach(x => { x.VisitId = (long)visitId; });

            IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.SqlauditModule> dbModule = null;

            return AddVisitTechnicalSpecialist(visitTechnicalSpecialists,
                                                    ref dbVisitTechnicalSpecialists,
                                                    ref dbTechnicalSpecialist,
                                                    ref dbVisit,
                                                    dbModule,
                                                    commitChange,
                                                    isDbValidationRequired);
        }

        public Response Add(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                            ref IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialists,
                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                            ref IList<DbModel.Visit> dbVisit,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? visitId = null)
        {
            if (visitId.HasValue)
                visitTechnicalSpecialists?.ToList().ForEach(x => { x.VisitId = (long)visitId; });
            return AddVisitTechnicalSpecialist(visitTechnicalSpecialists,
                                                    ref dbVisitTechnicalSpecialists,
                                                    ref dbTechnicalSpecialist,
                                                    ref dbVisit,
                                                    dbModule,
                                                    commitChange,
                                                    isDbValidationRequired);
        }
        #endregion

        #region Modify
        public Response Modify(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? visitId = null)
        {
            if (visitId.HasValue)
                visitTechnicalSpecialists?.ToList().ForEach(x => { x.VisitId = (long)visitId; });

            IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialist = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            return UpdateVisitTechnicalSpecialist(visitTechnicalSpecialists,
                                                       ref dbVisitTechnicalSpecialist,
                                                       ref dbTechnicalSpecialist,
                                                       ref dbVisit,
                                                       dbModule,
                                                       commitChange,
                                                       isDbValidationRequired);
        }

        public Response Modify(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                                ref IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialist,
                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                ref IList<DbModel.Visit> dbVisit,
                                IList<DbModel.SqlauditModule> dbModule,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? visitId = null)
        {
            if (visitId.HasValue)
                visitTechnicalSpecialists?.ToList().ForEach(x => { x.VisitId = (long)visitId; });

            return UpdateVisitTechnicalSpecialist(visitTechnicalSpecialists,
                                                        ref dbVisitTechnicalSpecialist,
                                                        ref dbTechnicalSpecialist,
                                                        ref dbVisit,
                                                        dbModule,
                                                        commitChange,
                                                        isDbValidationRequired);
        }


        #endregion

        #region Delete
        public Response Delete(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               long? visitId = null)
        {
            IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            if (visitId.HasValue)
                visitTechnicalSpecialists?.ToList().ForEach(x => { x.VisitId = (long)visitId; });

            return this.RemoveVisitTechnicalSpecialist(visitTechnicalSpecialists,
                                                            ref dbVisitTechnicalSpecialists,
                                                            ref dbTechnicalSpecialist,
                                                            ref dbVisit,
                                                            dbModule,
                                                            commitChange,
                                                            isDbValidationRequired);
        }

        public Response Delete(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                               ref IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialists,
                               ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                               ref IList<DbModel.Visit> dbVisit,
                               IList<DbModel.SqlauditModule> dbModule,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               long? visitId = null)
        {
            if (visitId.HasValue)
                visitTechnicalSpecialists?.ToList().ForEach(x => { x.VisitId = (long)visitId; });

            return this.RemoveVisitTechnicalSpecialist(visitTechnicalSpecialists,
                                                            ref dbVisitTechnicalSpecialists,
                                                            ref dbTechnicalSpecialist,
                                                            ref dbVisit,
                                                            dbModule,
                                                            commitChange,
                                                            isDbValidationRequired);
        }


        #endregion

        #region Record Valid Check
        public Response IsRecordValidForProcess(IList<DomainModel.VisitTechnicalSpecialist> timeTechnicalSpecialists,
                                                ValidationType validationType)
        {
            IList<DbModel.VisitTechnicalSpecialist> dbTimeTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialist> technicalSpecialists = null;
            IList<DbModel.Visit> dbVisits = null;
            return IsRecordValidForProcess(timeTechnicalSpecialists,
                                            validationType,
                                            ref dbVisits,
                                            ref dbTimeTechnicalSpecialists,
                                            ref technicalSpecialists);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitTechnicalSpecialist> timeTechnicalSpecialists,
                                                ValidationType validationType,
                                                ref IList<DbModel.Visit> dbVisits,
                                                ref IList<DbModel.VisitTechnicalSpecialist> dbTimeTechnicalSpecialists,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists)
        {
            IList<DomainModel.VisitTechnicalSpecialist> filteredAssignmentTechnicalSpecialist = null;
            return IsRecordValidForProcess(timeTechnicalSpecialists,
                                            validationType,
                                            ref filteredAssignmentTechnicalSpecialist,
                                            ref dbTimeTechnicalSpecialists,
                                            ref dbVisits,
                                            ref dbTechnicalSpecialists
                                           );
        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitTechnicalSpecialist> timeTechnicalSpecialists,
                                                ValidationType validationType,
                                                IList<DbModel.Visit> dbVisits,
                                                IList<DbModel.VisitTechnicalSpecialist> dbTimeTechnicalSpecialists,
                                                IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists)
        {
            return IsRecordValidForProcess(timeTechnicalSpecialists,
                                            validationType,
                                            ref dbVisits,
                                            ref dbTimeTechnicalSpecialists,
                                            ref dbTechnicalSpecialists);
        }
        #endregion


        #region Private Methods
        private IList<DomainModel.VisitTechnicalSpecialist> FilterRecord(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                                                                              ValidationType filterType)
        {
            IList<DomainModel.VisitTechnicalSpecialist> filteredVisitTechnicalSpecialist = null;

            if (filterType == ValidationType.Add)
                filteredVisitTechnicalSpecialist = visitTechnicalSpecialists?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredVisitTechnicalSpecialist = visitTechnicalSpecialists?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredVisitTechnicalSpecialist = visitTechnicalSpecialists?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredVisitTechnicalSpecialist;
        }

        private bool IsValidPayload(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(visitTechnicalSpecialists), validationType);
            //if (validationResults?.Count > 0)
            //    messages.Add(_messages, ModuleType.Visit, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }


        private IList<DbModel.VisitTechnicalSpecialist> GetVisitTechnicalspecialist(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                                                                                            ValidationType validationType)
        {
            IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialists = null;
            if (visitTechnicalSpecialists?.Count > 0)
            {
                var visitTechnicalSpecialistId = visitTechnicalSpecialists.Select(x => x.VisitTechnicalSpecialistId).Distinct().ToList();
                dbVisitTechnicalSpecialists = _repository.FindBy(x => visitTechnicalSpecialistId.Contains(x.Id)).ToList();
            }

            return dbVisitTechnicalSpecialists;
        }


        public bool IsVisitTechnicalSpecialistExistInDb(IList<long> visitTechnicalSpecialistIds,
                                                               IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialists,
                                                               ref IList<long> visitTechnicalSpecialistNotExists,
                                                               ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbVisitTechnicalSpecialists == null)
                dbVisitTechnicalSpecialists = new List<DbModel.VisitTechnicalSpecialist>();

            var validMessages = validationMessages;

            if (visitTechnicalSpecialistIds?.Count > 0)
            {
                visitTechnicalSpecialistNotExists = visitTechnicalSpecialistIds.Where(x => !dbVisitTechnicalSpecialists.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                visitTechnicalSpecialistNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.VisitTechnicalSpecialistNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                                                IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialists,
                                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = visitTechnicalSpecialists.Where(x => !dbVisitTechnicalSpecialists.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.VisitTechnicalSpecialistId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x.VisitTechnicalSpecialistId, MessageType.VisitTechnicalSpecialistUpdateCountMisMatch, x.VisitTechnicalSpecialistId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsUniqueVisitTechSpecialist(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                                                   IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialists,
                                                   ValidationType validationType,
                                                   ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.VisitTechnicalSpecialist> referenceTypeExists = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            referenceTypeExists = _repository.IsUniqueVisitReference(visitTechnicalSpecialists, dbVisitTechnicalSpecialists, validationType);
            referenceTypeExists?.ToList().ForEach(x =>
            {
                messages.Add(_messages, x.TechnicalSpecialist.Pin, MessageType.VisitTSDuplicateRecord, x.TechnicalSpecialist.Pin);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count > 0;
        }

        private bool IsVisitTechnicalSpecialistCanBeRemove(IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialists,
                                                               ref IList<ValidationMessage> messages)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (messages == null)
                messages = new List<ValidationMessage>();
            dbVisitTechnicalSpecialists?.Where(x => x.IsAnyCollectionPropertyContainValue())
                                           .ToList()
                                           .ForEach(x =>
                                           {
                                               validationMessages.Add(_messages, x.TechnicalSpecialistId, MessageType.VisitTechnicalSpecialistIsBeingUsed, x.TechnicalSpecialistId);
                                           });

            if (validationMessages.Count > 0)
                messages.AddRange(validationMessages);

            return messages.Count <= 0;
        }


        private bool IsRecordValidForAdd(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                                         IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialists,
                                         ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                         ref IList<DbModel.Visit> dbVisits,
                                         ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var visitIds = visitTechnicalSpecialists.Where(x => x.VisitId > 0).Select(x => x.VisitId).Distinct().ToList();
            var technicalSpecialistIds = visitTechnicalSpecialists.Where(x => x.Pin > 0).Select(x => x.Pin.ToString()).Distinct().ToList();

            if (IsValidVisit(visitIds, ref dbVisits, ref validationMessages, null)
                && Convert.ToBoolean(_technicalSpecialistService.IsRecordExistInDb(technicalSpecialistIds, ref dbTechnicalSpecialists, ref validationMessages).Result)
                && IsUniqueVisitTechSpecialist(visitTechnicalSpecialists, dbVisitTechnicalSpecialists, ValidationType.Add, ref validationMessages))

                messages = validationMessages;
            return messages?.Count <= 0;
        }

        private bool IsValidVisit(IList<long> visitId,
                                   ref IList<DbModel.Visit> dbVisits,
                                   ref IList<ValidationMessage> messages,
                                   params string[] includes)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (dbVisits == null)
            {
                var resultVisits = _visitRepository.FetchVisits(visitId, includes);
                var visitNotExists = visitId?.Where(x => !resultVisits.Any(x2 => x2.Id == x))?.ToList();
                visitNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.VisitNotExists.ToId();
                    message.Add(_messages, x, MessageType.VisitNotExists, x);
                });
                dbVisits = resultVisits;
                messages = message;
            }

            return messages?.Count <= 0;
        }

        private bool IsRecordValidForUpdate(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                                            IList<DbModel.VisitTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<DbModel.Visit> dbVisits,
                                            ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            IList<DbModel.Visit> dbVisit = null;
            if (dbVisits != null)
                dbVisit = dbVisits;

            var visitIds = visitTechnicalSpecialists.Where(x => x.VisitId > 0).Select(x => (long)x.VisitId).Distinct().ToList();
            var technicalSpecialistIds = visitTechnicalSpecialists.Where(x => x.Pin > 0).Select(x => x.Pin.ToString()).Distinct().ToList();
            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(visitTechnicalSpecialists, dbAssignmentTechnicalSpecialist, ref messages))
                    if (IsValidVisit(visitIds, ref dbVisit, ref messages, new string[] { "VisitTechnicalSpecialist", "VisitTechnicalSpecialist.TechnicalSpecialist" }))
                        if (Convert.ToBoolean(_technicalSpecialistService.IsRecordExistInDb(technicalSpecialistIds, ref dbTechnicalSpecialists, ref messages).Result))
                            IsUniqueVisitTechSpecialist(visitTechnicalSpecialists, dbVisit.ToList().SelectMany(x => x.VisitTechnicalSpecialist).ToList(), ValidationType.Update, ref messages);

            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }



        private Response IsRecordValidForProcess(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                                               ValidationType validationType,
                                               ref IList<DomainModel.VisitTechnicalSpecialist> filteredVisitTechSpec,
                                               ref IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialist,
                                               ref IList<DbModel.Visit> dbVisit,
                                               ref IList<DbModel.TechnicalSpecialist> dbTechSpecialist)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (visitTechnicalSpecialists != null && visitTechnicalSpecialists.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredVisitTechSpec == null || filteredVisitTechSpec.Count <= 0)
                        filteredVisitTechSpec = FilterRecord(visitTechnicalSpecialists, validationType);

                    if (filteredVisitTechSpec != null && filteredVisitTechSpec?.Count > 0)
                    {
                        result = IsValidPayload(filteredVisitTechSpec,
                                                validationType,
                                                ref validationMessages);
                        if (filteredVisitTechSpec?.Count > 0 && result)
                        {
                            IList<long> moduleNotExists = null;
                            var visitTechSpecId = filteredVisitTechSpec.Where(x => x.VisitTechnicalSpecialistId > 0)
                                                                                         .Select(x => (long)x.VisitTechnicalSpecialistId).Distinct().ToList();

                            if (dbVisitTechnicalSpecialist == null || dbVisitTechnicalSpecialist.Count <= 0)
                                dbVisitTechnicalSpecialist = GetVisitTechnicalspecialist(filteredVisitTechSpec, validationType);

                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                            {
                                result = IsVisitTechnicalSpecialistExistInDb(visitTechSpecId,
                                                                      dbVisitTechnicalSpecialist,
                                                                      ref moduleNotExists,
                                                                      ref validationMessages);

                                // This Validation not required currently.
                                // if (result && validationType == ValidationType.Delete)
                                //     result = IsVisitTechnicalSpecialistCanBeRemove(dbVisitTechnicalSpecialist,
                                //                                               ref validationMessages);

                                if (result && validationType == ValidationType.Update)
                                    result = IsRecordValidForUpdate(filteredVisitTechSpec,
                                                                    dbVisitTechnicalSpecialist,
                                                                    ref dbTechSpecialist,
                                                                    ref dbVisit,
                                                                    ref validationMessages);
                            }
                            else if (validationType == ValidationType.Add)
                                result = IsRecordValidForAdd(filteredVisitTechSpec,
                                                                dbVisitTechnicalSpecialist,
                                                                ref dbTechSpecialist,
                                                                ref dbVisit,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitTechnicalSpecialists);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
        #endregion

        private Response AddVisitTechnicalSpecialist(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                                                        ref IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialists,
                                                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                                        ref IList<DbModel.Visit> dbVisit,
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
                var recordToBeAdd = FilterRecord(visitTechnicalSpecialists, ValidationType.Add);
                eventId = visitTechnicalSpecialists?.FirstOrDefault()?.EventId;
                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(visitTechnicalSpecialists,
                                                            ValidationType.Add,
                                                            ref recordToBeAdd,
                                                            ref dbVisitTechnicalSpecialists,
                                                            ref dbVisit,
                                                            ref dbTechnicalSpecialist);
                else if (dbVisitTechnicalSpecialists?.Count <= 0)
                    dbVisitTechnicalSpecialists = GetVisitTechnicalspecialist(recordToBeAdd, ValidationType.Add);

                if (recordToBeAdd?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    _repository.AutoSave = false;
                    dbTechnicalSpecialists = dbTechnicalSpecialist;
                    dbVisitTechnicalSpecialists = _mapper.Map<IList<DbModel.VisitTechnicalSpecialist>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isVisitTSId"] = false;
                        opt.Items["TechSpec"] = dbTechnicalSpecialists;
                    });
                    _repository.Add(dbVisitTechnicalSpecialists);
                    if (commitChange)
                    {
                        int value=_repository.ForceSave();
                        if (value > 0)
                        {
                            dbVisitTechnicalSpecialists?.ToList().ForEach(x1 => _auditSearchService.AuditLog(recordToBeAdd.FirstOrDefault(), ref eventId, visitTechnicalSpecialists?.FirstOrDefault()?.ActionByUser,
                                                                                                    null,
                                                                                                    ValidationType.Add.ToAuditActionType(),
                                                                                                    SqlAuditModuleType.VisitSpecialistAccount,
                                                                                                    null,
                                                                                                    _mapper.Map<DomainModel.VisitTechnicalSpecialist>(x1)
                                                                                                    ,dbModule ));
                        }
                     }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitTechnicalSpecialists);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateVisitTechnicalSpecialist(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                                                            ref IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialist,
                                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                                            ref IList<DbModel.Visit> dbVisit,
                                                            IList<DbModel.SqlauditModule> dbModule,
                                                            bool commitChange,
                                                            bool isDbValidationRequired)
        {
            Exception exception = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DomainModel.VisitReference> result = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(visitTechnicalSpecialists, ValidationType.Update);
                eventId = visitTechnicalSpecialists?.FirstOrDefault()?.EventId;
                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(visitTechnicalSpecialists,
                                                            ValidationType.Update,
                                                            ref recordToBeModify,
                                                            ref dbVisitTechnicalSpecialist,
                                                            ref dbVisit,
                                                            ref dbTechnicalSpecialist);
                else if (dbVisitTechnicalSpecialist?.Count <= 0)
                    dbVisitTechnicalSpecialist = GetVisitTechnicalspecialist(recordToBeModify, ValidationType.Update);

                IList<DomainModel.VisitTechnicalSpecialist> domExistingVisitSpecialists = new List<DomainModel.VisitTechnicalSpecialist>();
                dbVisitTechnicalSpecialist?.ToList()?.ForEach(x =>
                {
                    domExistingVisitSpecialists.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.VisitTechnicalSpecialist>(x)));
                });


                if (recordToBeModify?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    if (dbVisitTechnicalSpecialist?.Count > 0)
                    {
                        dbTechnicalSpecialists = dbTechnicalSpecialist;
                        dbVisitTechnicalSpecialist.ToList().ForEach(dbVisitTS =>
                        {
                            var visitTSToBeModify = recordToBeModify.FirstOrDefault(x => x.VisitTechnicalSpecialistId == dbVisitTS.Id);
                            if (visitTSToBeModify != null)
                            {
                                dbVisitTS.TechnicalSpecialistId = dbTechnicalSpecialists.FirstOrDefault(x1 => x1.Pin == visitTSToBeModify.Pin).Id;
                                dbVisitTS.VisitId = (long)visitTSToBeModify.VisitId;
                                dbVisitTS.LastModification = DateTime.UtcNow;
                                dbVisitTS.UpdateCount = visitTSToBeModify.UpdateCount.CalculateUpdateCount();
                                dbVisitTS.ModifiedBy = visitTSToBeModify.ModifiedBy;
                            }
                        });
                        _repository.AutoSave = false;
                        _repository.Update(dbVisitTechnicalSpecialist);
                        if (commitChange)
                        {
                           int value= _repository.ForceSave();
                            if (value > 0)
                            {
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                        null,
                                                                        ValidationType.Update.ToAuditActionType(),
                                                                        SqlAuditModuleType.VisitSpecialistAccount,
                                                                        domExistingVisitSpecialists?.FirstOrDefault(x2 => x2.VisitTechnicalSpecialistId == x1.VisitTechnicalSpecialistId),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitTechnicalSpecialists);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveVisitTechnicalSpecialist(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                                                            ref IList<DbModel.VisitTechnicalSpecialist> dbVisitTechnicalSpecialists,
                                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                                            ref IList<DbModel.Visit> dbVisit,
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
                var recordToBeDelete = FilterRecord(visitTechnicalSpecialists, ValidationType.Delete);

                eventId = visitTechnicalSpecialists?.FirstOrDefault()?.EventId;
                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(visitTechnicalSpecialists,
                                                       ValidationType.Delete,
                                                       ref recordToBeDelete,
                                                       ref dbVisitTechnicalSpecialists,
                                                       ref dbVisit,
                                                       ref dbTechnicalSpecialist);

                if (recordToBeDelete?.Count > 0 && (response == null || response.Code == MessageType.Success.ToId()))
                {
                    if (dbVisitTechnicalSpecialists?.Count > 0)
                    {
                        _repository.AutoSave = false;
                        _repository.Delete(dbVisitTechnicalSpecialists);
                        if (commitChange)
                        {
                           int value= _repository.ForceSave();
                            if (value > 0)
                            {
                                visitTechnicalSpecialists?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                             null,
                                                                                                             ValidationType.Delete.ToAuditActionType(),
                                                                                                             SqlAuditModuleType.VisitSpecialistAccount,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitTechnicalSpecialists);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        public Response GetVisitTechnicalSpecialistAccount(DomainModel.VisitTechnicalSpecialist searchModel)
        {
            Exception exception = null;
            IList<DomainModel.VisitTechnicalSpecialist> result = null;
            try
            {
                result = this._repository.Search(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
      


    }
}
