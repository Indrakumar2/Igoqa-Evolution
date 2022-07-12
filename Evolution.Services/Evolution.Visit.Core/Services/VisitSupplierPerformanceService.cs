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
    public class VisitSupplierPerformanceService : IVisitSupplierPerformanceService
    {
        private IMapper _mapper = null;
        private IAppLogger<VisitSupplierPerformanceService> _logger = null;
        private IVisitSupplierPerformanceRepository _repository = null;
        private IVisitSupplierPerformanceValidationService _visitSupplierPerformanceValidationService = null;
        private readonly IMasterService _masterService = null;
        private readonly IVisitService _visitService = null;
        private readonly JObject _messages = null;
        private readonly IAuditSearchService _auditSearchService = null;
     

        public VisitSupplierPerformanceService(IAppLogger<VisitSupplierPerformanceService> logger,
                                         IVisitSupplierPerformanceRepository visitSupplierPerformanceRepository,
                                         IVisitSupplierPerformanceValidationService visitSupplierPerformanceValidationService,
                                         IVisitService visitService,
                                         IMasterService masterService,
                                         IMapper mapper,
                                         JObject messages,
                                         IAuditSearchService auditSearchService)
                                       
        {
            _mapper = mapper;
            _logger = logger;
            _repository = visitSupplierPerformanceRepository;
            _visitSupplierPerformanceValidationService = visitSupplierPerformanceValidationService;
            _visitService = visitService;
            _masterService = masterService;
            this._messages = messages;
            _auditSearchService = auditSearchService;
          
        }


        public Response Get(DomainModel.VisitSupplierPerformanceType searchModel)
        {
            IList<DomainModel.VisitSupplierPerformanceType> result = null;
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

        public Response Add(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? visitId = null)
        {
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            if (visitId.HasValue)
                visitSupplierPerformance?.ToList().ForEach(x => { x.VisitId = visitId; });

            return AddVisitSupplierPerformance(visitSupplierPerformance,
                                         ref dbVisitSupplierPerformance,
                                         ref dbVisit,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }

        public Response Add(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                            ref IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance,
                            ref IList<DbModel.Visit> dbVisit,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? visitId = null)
        {
            if (visitId.HasValue)
                visitSupplierPerformance?.ToList().ForEach(x => { x.VisitId = visitId; });

            return AddVisitSupplierPerformance(visitSupplierPerformance,
                                       ref dbVisitSupplierPerformance,
                                       ref dbVisit,
                                       dbModule,
                                       commitChange,
                                       isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               long? visitId = null)
        {
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance = null;
            if (visitId.HasValue)
                visitSupplierPerformance?.ToList().ForEach(x => { x.VisitId = visitId; });

            return this.RemoveVisitSupplierPerformance(visitSupplierPerformance,
                                                  ref dbVisitSupplierPerformance,
                                                  ref dbVisit,
                                                  dbModule,
                                                  commitChange,
                                                  isDbValidationRequired);
        }



        public Response Delete(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                               ref IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance,
                               ref IList<DbModel.Visit> dbVisit,
                               IList<DbModel.SqlauditModule> dbModule,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               long? visitId = null)
        {
            if (visitId.HasValue)
                visitSupplierPerformance?.ToList().ForEach(x => { x.VisitId = visitId; });

            return this.RemoveVisitSupplierPerformance(visitSupplierPerformance,
                                                  ref dbVisitSupplierPerformance,
                                                  ref dbVisit,
                                                  dbModule,
                                                  commitChange,
                                                  isDbValidationRequired);
        }


        public Response Modify(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? visitId = null)
        {
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            if (visitId.HasValue)
                visitSupplierPerformance?.ToList().ForEach(x => { x.VisitId = visitId; });

            IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance = null;
            return UpdateVisitSupplierPerformance(visitSupplierPerformance,
                                             ref dbVisitSupplierPerformance,
                                             ref dbVisit,
                                             dbModule,
                                             commitChange,
                                             isDbValidationRequired);
        }



        public Response Modify(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                                ref IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance,
                                ref IList<DbModel.Visit> dbVisit,
                                IList<DbModel.SqlauditModule> dbModule,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? visitId = null)
        {
            if (visitId.HasValue)
                visitSupplierPerformance?.ToList().ForEach(x => { x.VisitId = visitId; });

            return UpdateVisitSupplierPerformance(visitSupplierPerformance,
                                             ref dbVisitSupplierPerformance,
                                             ref dbVisit,
                                             dbModule,
                                             commitChange,
                                             isDbValidationRequired);
        }


        public Response IsRecordValidForProcess(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                                                ValidationType validationType)
        {
            IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance = null;
            IList<DbModel.Visit> dbVisit = null;
            return IsRecordValidForProcess(visitSupplierPerformance,
                                            validationType,
                                            ref dbVisitSupplierPerformance,
                                            ref dbVisit);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                                                ValidationType validationType,
                                                ref IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance,
                                                ref IList<DbModel.Visit> dbVisit)
        {
            IList<DomainModel.VisitSupplierPerformanceType> filteredVisitSupplierPerformance = null;
            return IsRecordValidForProcess(visitSupplierPerformance,
                                            validationType,
                                            ref filteredVisitSupplierPerformance,
                                            ref dbVisitSupplierPerformance,
                                            ref dbVisit);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                                                ValidationType validationType,
                                                IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance,
                                                ref IList<DbModel.Visit> dbVisit)
        {
            return IsRecordValidForProcess(visitSupplierPerformance,
                                          validationType,
                                          ref dbVisitSupplierPerformance,
                                          ref dbVisit);
        }

        private IList<DbModel.VisitSupplierPerformance> GetVisitSupplierPerformance(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                                                                        ValidationType validationType)
        {
            IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformanceType = null;
            if (validationType != ValidationType.Add)
            {
                if (visitSupplierPerformance?.Count > 0)
                {
                    var visitSupplierPerformanceTypeId = visitSupplierPerformance.Select(x => x.VisitSupplierPerformanceTypeId).Distinct().ToList();
                    dbVisitSupplierPerformanceType = _repository.FindBy(x => visitSupplierPerformanceTypeId.Contains(x.Id)).ToList();
                }
            }
            return dbVisitSupplierPerformanceType;
        }

        private Response AddVisitSupplierPerformance(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                                               ref IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance,
                                               ref IList<DbModel.Visit> dbVisit,
                                               IList<DbModel.SqlauditModule> dbModule,
                                               bool commitChange,
                                               bool isDbValidationRequired)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(visitSupplierPerformance, ValidationType.Add);
                eventId = visitSupplierPerformance?.FirstOrDefault()?.EventId;
                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(visitSupplierPerformance,
                                                            ValidationType.Add,
                                                            ref recordToBeAdd,
                                                            ref dbVisitSupplierPerformance,
                                                            ref dbVisit);
                else if (dbVisitSupplierPerformance?.Count <= 0)
                    dbVisitSupplierPerformance = GetVisitSupplierPerformance(recordToBeAdd, ValidationType.Add);

                if (recordToBeAdd?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    _repository.AutoSave = false;
                    dbVisitSupplierPerformance = _mapper.Map<IList<DbModel.VisitSupplierPerformance>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isVisitId"] = false;
                    });
                    _repository.Add(dbVisitSupplierPerformance);
                    if (commitChange)
                    {
                        var savCnt = _repository.ForceSave();
                        if (savCnt > 0)
                        {
                           dbVisitSupplierPerformance?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, visitSupplierPerformance?.FirstOrDefault()?.ActionByUser,
                                                                                                null,
                                                                                                 ValidationType.Add.ToAuditActionType(),
                                                                                                 SqlAuditModuleType.VisitSupplierPerformance,
                                                                                                  null,
                                                                                                   _mapper.Map<DomainModel.VisitSupplierPerformanceType>(x1)
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitSupplierPerformance);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateVisitSupplierPerformance(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                                                  ref IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance,
                                                  ref IList<DbModel.Visit> dbVisit,
                                                  IList<DbModel.SqlauditModule> dbModule,
                                                  bool commitChange,
                                                  bool isDbValidationRequired)
        {
            Exception exception = null;
            IList<DomainModel.VisitSupplierPerformanceType> result = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(visitSupplierPerformance, ValidationType.Update);
                eventId = visitSupplierPerformance?.FirstOrDefault().EventId;


                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(visitSupplierPerformance,
                                                            ValidationType.Update,
                                                            ref recordToBeModify,
                                                            ref dbVisitSupplierPerformance,
                                                            ref dbVisit);
                else if (dbVisitSupplierPerformance?.Count <= 0)
                    dbVisitSupplierPerformance = GetVisitSupplierPerformance(recordToBeModify, ValidationType.Update);

                if (recordToBeModify?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    if (dbVisitSupplierPerformance?.Count > 0)
                    {
                        IList<DomainModel.VisitSupplierPerformanceType> domExistingVisitSupplierPerformanceType = new List<DomainModel.VisitSupplierPerformanceType>();
                        dbVisitSupplierPerformance?.ToList()?.ForEach(x =>
                        {
                            domExistingVisitSupplierPerformanceType.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.VisitSupplierPerformanceType>(x)));
                        });

                        dbVisitSupplierPerformance.ToList().ForEach(dbVisitSupplierPerformanceType =>
                        {
                            var visitSupplierPerformanceToBeModify = recordToBeModify.FirstOrDefault(x => x.VisitSupplierPerformanceTypeId == dbVisitSupplierPerformanceType.Id);
                            if (visitSupplierPerformanceToBeModify != null)
                            {                                
                                dbVisitSupplierPerformanceType.VisitId = (long)visitSupplierPerformanceToBeModify.VisitId;
                                dbVisitSupplierPerformanceType.PerformanceType = visitSupplierPerformanceToBeModify.SupplierPerformance;
                                dbVisitSupplierPerformanceType.Score = visitSupplierPerformanceToBeModify.NCRReference;
                                dbVisitSupplierPerformanceType.NcrcloseOutDate = visitSupplierPerformanceToBeModify.NCRCloseOutDate;
                                dbVisitSupplierPerformanceType.LastModification = DateTime.UtcNow;
                                dbVisitSupplierPerformanceType.UpdateCount = visitSupplierPerformanceToBeModify.UpdateCount.CalculateUpdateCount();
                                dbVisitSupplierPerformanceType.ModifiedBy = visitSupplierPerformanceToBeModify.ModifiedBy;
                            }
                        });
                        _repository.AutoSave = false;
                        _repository.Update(dbVisitSupplierPerformance);
                        if (commitChange)
                        {
                            var updCnt = _repository.ForceSave();
                            if (updCnt > 0)
                            {
                                 recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                            null,
                                                                                                       ValidationType.Update.ToAuditActionType(),
                                                                                                        SqlAuditModuleType.VisitSupplierPerformance,
                                                                                                         domExistingVisitSupplierPerformanceType?.FirstOrDefault(x2 => x2.VisitSupplierPerformanceTypeId == x1.VisitSupplierPerformanceTypeId),
                                                                                                         x1));
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitSupplierPerformance);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveVisitSupplierPerformance(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                                                  ref IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance,
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
                var recordToBeDelete = FilterRecord(visitSupplierPerformance, ValidationType.Delete);
                eventId = visitSupplierPerformance?.FirstOrDefault()?.EventId;
                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(visitSupplierPerformance,
                                                       ValidationType.Delete,
                                                       ref recordToBeDelete,
                                                       ref dbVisitSupplierPerformance,
                                                       ref dbVisit);

                if (recordToBeDelete?.Count > 0 && (response == null || response.Code == MessageType.Success.ToId()))
                {
                    if (dbVisitSupplierPerformance?.Count > 0)
                    {
                        _repository.AutoSave = false;
                        _repository.Delete(dbVisitSupplierPerformance);
                        if (commitChange)
                        {
                            var delCnt = _repository.ForceSave();
                            if (delCnt > 0)
                            {
                              
                                recordToBeDelete?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                      null,
                                                                                                        ValidationType.Delete.ToAuditActionType(),
                                                                                                         SqlAuditModuleType.VisitSupplierPerformance,
                                                                                                         x1,
                                                                                                          null
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitSupplierPerformance);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }


        private IList<DomainModel.VisitSupplierPerformanceType> FilterRecord(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                                                                       ValidationType filterType)
        {
            IList<DomainModel.VisitSupplierPerformanceType> filteredVisitSupplierPerformance = null;

            if (filterType == ValidationType.Add)
                filteredVisitSupplierPerformance = visitSupplierPerformance?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredVisitSupplierPerformance = visitSupplierPerformance?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredVisitSupplierPerformance = visitSupplierPerformance?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredVisitSupplierPerformance;
        }


        private bool IsValidPayload(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _visitSupplierPerformanceValidationService.Validate(JsonConvert.SerializeObject(visitSupplierPerformance), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Visit, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        //private bool IsUniqueVisitSupplierPerformanceType(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
        //                                           IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance,
        //                                           ValidationType validationType,
        //                                           ref IList<ValidationMessage> validationMessages)
        //{
        //    IList<DbModel.VisitSupplierPerformance> referenceTypeExists = null;
        //    IList<ValidationMessage> messages = new List<ValidationMessage>();
        //    if (validationMessages == null)
        //        validationMessages = new List<ValidationMessage>();

        //    referenceTypeExists = _repository.IsUniqueVisitSupplierPerformance(visitSupplierPerformance, dbVisitSupplierPerformance, validationType);
        //    referenceTypeExists?.ToList().ForEach(x =>
        //    {
        //        messages.Add(_messages, x.VisitSupplierPerformanceType.Name, MessageType.VisitSupplierPerformanceDuplicateRecord, x.VisitSupplierPerformanceType.Name);
        //    });

        //    if (messages.Count > 0)
        //        validationMessages.AddRange(messages);

        //    return messages?.Count > 0;
        //}


        private bool IsRecordUpdateCountMatching(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                                                IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance,
                                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = visitSupplierPerformance.Where(x => !dbVisitSupplierPerformance.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() &&
                                                                                                             x1.Id == x.VisitSupplierPerformanceTypeId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x.VisitSupplierPerformanceTypeId, MessageType.VisitSupplierPerformanceUpdateCountMisMatch, x.VisitSupplierPerformanceTypeId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsVisitSupplierPerformanceExistInDb(IList<long> visitSupplierPerformanceId,
                                                  IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance,
                                                  ref IList<long> visitSupplierPerformanceNotExists,
                                                  ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbVisitSupplierPerformance == null)
                dbVisitSupplierPerformance = new List<DbModel.VisitSupplierPerformance>();

            var validMessages = validationMessages;

            if (visitSupplierPerformanceId?.Count > 0)
            {
                visitSupplierPerformanceNotExists = visitSupplierPerformanceId.Where(x => !dbVisitSupplierPerformance.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                visitSupplierPerformanceNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.VisitSupplierPerformanceInvalid, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidForAdd(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                                          IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance,
                                          ref IList<DbModel.Visit> dbVisit,
                                          ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.Visit> dbVisits = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();


            if (dbVisit != null)
                dbVisits = dbVisit;

            var visitIds = visitSupplierPerformance.Where(x => x.VisitId > 0).Select(x => (long)x.VisitId).Distinct().ToList();

            //if (_visitService.IsValidVisit(visitIds, ref dbVisits, ref messages, null))
            //    if (IsValidReferenceType(visitSupplierPerformance, ref dbReferenceType, ref messages))
            //        IsUniqueVisitSupplierPerformanceType(visitSupplierPerformance, dbVisitSupplierPerformance, ValidationType.Add, ref messages);

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsRecordValidForUpdate(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                                             IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance,
                                             ref IList<DbModel.Visit> dbVisit,
                                             ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            IList<DbModel.Visit> dbVisits = null;
            if (dbVisit != null)
                dbVisits = dbVisit;

            var visitIds = visitSupplierPerformance.Where(x => x.VisitId > 0).Select(x => (long)x.VisitId).Distinct().ToList();
            //if (messages?.Count <= 0)
            //{
            //    if (IsRecordUpdateCountMatching(visitSupplierPerformance, dbVisitSupplierPerformance, ref messages))
            //        if (_visitService.IsValidVisit(visitIds, ref dbVisit, ref messages, new string[] { "VisitSupplierPerformance" }))
            //            //if (IsValidReferenceType(visitSupplierPerformance, ref dbRefrenceType, ref messages))
            //            //    IsUniqueVisitSupplierPerformanceType(visitSupplierPerformance, dbVisit.ToList().SelectMany(x => x.VisitSupplierPerformance).ToList(), ValidationType.Update, ref messages);

            //}

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private Response IsRecordValidForProcess(IList<DomainModel.VisitSupplierPerformanceType> visitSupplierPerformance,
                                               ValidationType validationType,
                                               ref IList<DomainModel.VisitSupplierPerformanceType> filteredVisitSupplierPerformanceTypes,
                                               ref IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance,
                                               ref IList<DbModel.Visit> dbVisit)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (visitSupplierPerformance != null && visitSupplierPerformance.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredVisitSupplierPerformanceTypes == null || filteredVisitSupplierPerformanceTypes.Count <= 0)
                        filteredVisitSupplierPerformanceTypes = FilterRecord(visitSupplierPerformance, validationType);

                    if (filteredVisitSupplierPerformanceTypes != null && filteredVisitSupplierPerformanceTypes?.Count > 0)
                    {
                        result = IsValidPayload(filteredVisitSupplierPerformanceTypes,
                                                validationType,
                                                ref validationMessages);
                        if (filteredVisitSupplierPerformanceTypes?.Count > 0 && result)
                        {
                            IList<long> moduleNotExists = null;
                            var visitSupplierPerformanceTypeId = filteredVisitSupplierPerformanceTypes.Where(x => x.VisitSupplierPerformanceTypeId.HasValue)
                                                                                         .Select(x => (long)x.VisitSupplierPerformanceTypeId).Distinct().ToList();

                            if (dbVisitSupplierPerformance == null || dbVisitSupplierPerformance.Count <= 0)
                                dbVisitSupplierPerformance = GetVisitSupplierPerformance(filteredVisitSupplierPerformanceTypes, validationType);

                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                            {
                                result = IsVisitSupplierPerformanceExistInDb(visitSupplierPerformanceTypeId,
                                                                      dbVisitSupplierPerformance,
                                                                      ref moduleNotExists,
                                                                      ref validationMessages);

                                if (result && validationType == ValidationType.Update)
                                    result = IsRecordValidForUpdate(filteredVisitSupplierPerformanceTypes,
                                                                    dbVisitSupplierPerformance,
                                                                    ref dbVisit,
                                                                    ref validationMessages);
                            }
                            else if (validationType == ValidationType.Add)
                                result = IsRecordValidForAdd(filteredVisitSupplierPerformanceTypes,
                                                                dbVisitSupplierPerformance,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitSupplierPerformance);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

     
    }
}
