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
using Evolution.Visit.Domain.Enum;
using Evolution.Visit.Domain.Interfaces.Data;
using Evolution.Visit.Domain.Interfaces.Validations;
using Evolution.Visit.Domain.Interfaces.Visits;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Core.Services
{
    public class VisitTechnicalSpecialistConsumableService : IVisitTechnicalSpecialistConsumableService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<VisitTechnicalSpecialistConsumableService> _logger = null;
        private readonly IVisitTechnicalSpecialistConsumableRepository _repository = null;
        private readonly IVisitTechnicalSpecialistAccountItemConsumableValidationService _techSpecAccountItemConsumableValidationService = null;
        private readonly IVisitRepository _visitRepository = null;
        private readonly IMasterService _masterService = null;
        private readonly JObject _messages = null;
     
        private readonly IAuditSearchService _auditSearchService=null;

        public VisitTechnicalSpecialistConsumableService(IAppLogger<VisitTechnicalSpecialistConsumableService> logger,
                                                IVisitTechnicalSpecialistConsumableRepository visitConsumableRepository,
                                                IVisitTechnicalSpecialistAccountItemConsumableValidationService validationService,
                                                IVisitRepository visitRepository,
                                                IMasterService masterService,
                                                IMapper mapper,
                                                JObject messages,
                                                IAuditSearchService auditSearchService)
                                              
        {
            _mapper = mapper;
            _logger = logger;
            _repository = visitConsumableRepository;
            _visitRepository = visitRepository;
            _masterService = masterService;
            _techSpecAccountItemConsumableValidationService = validationService;
            this._messages = messages;
            _auditSearchService = auditSearchService;
           
        }

        public Response Get(DomainModel.VisitSpecialistAccountItemConsumable searchModel)
        {
            IList<DomainModel.VisitSpecialistAccountItemConsumable> result = null;
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

        public async Task<Response> GetAsync(DomainModel.VisitSpecialistAccountItemConsumable searchModel)
        {
            IList<DomainModel.VisitSpecialistAccountItemConsumable> result = null;
            Exception exception = null;
            try
            {
                // result = await Task.Run(() => this._repository.Search(searchModel));
                 using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                   new TransactionOptions
                                                   {
                                                       IsolationLevel = IsolationLevel.ReadUncommitted
                                                   },TransactionScopeAsyncFlowOption.Enabled))
                {
                    result = await Task.Run(() => this._repository.Search(searchModel));
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

        public Response Add(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                          bool commitChange = true,
                          bool isDbValidationRequired = true,
                          long? visitId = null)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            if (visitId.HasValue)
                accountItemConsumables?.ToList().ForEach(x => { x.VisitId = visitId; });

            return AddTSConsumable(accountItemConsumables,
                                         ref dbSpecialistAccountItemConsumable,
                                         ref dbVisit,
                                         ref dbAssignment,
                                         ref dbProject,
                                         ref dbContract,
                                         ref dbExpenseType,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }


        public Response Add(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                            ref IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                            ref IList<DbModel.Visit> dbVisit,
                            ref IList<DbModel.Assignment> dbAssignment,
                            ref IList<DbModel.Project> dbProject,
                            ref IList<DbModel.Contract> dbContract,
                            ref IList<DbModel.Data> dbExpenseType,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? visitId = null)
        {
            if (visitId.HasValue)
                accountItemConsumables?.ToList().ForEach(x => { x.VisitId = visitId; });

            return AddTSConsumable(accountItemConsumables,
                                         ref dbSpecialistAccountItemConsumable,
                                         ref dbVisit,
                                         ref dbAssignment,
                                         ref dbProject,
                                         ref dbContract,
                                         ref dbExpenseType,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                  bool commitChange = true,
                                  bool isDbValidationRequired = true,
                                  long? visitId = null)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            if (visitId.HasValue)
                accountItemConsumables?.ToList().ForEach(x => { x.VisitId = visitId; });

            return RemoveTSConsumable(accountItemConsumables,
                                     ref dbSpecialistAccountItemConsumable,
                                     ref dbVisit,
                                     ref dbAssignment,
                                     ref dbProject,
                                     ref dbContract,
                                     ref dbExpenseType,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                ref IList<DbModel.Visit> dbVisit,
                                ref IList<DbModel.Assignment> dbAssignment,
                                ref IList<DbModel.Project> dbProject,
                                ref IList<DbModel.Contract> dbContract,
                                ref IList<DbModel.Data> dbExpenseType,
                                IList<DbModel.SqlauditModule> dbModule,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? visitId = null)
        {
            if (visitId.HasValue)
                accountItemConsumables?.ToList().ForEach(x => { x.VisitId = visitId; });
            return RemoveTSConsumable(accountItemConsumables,
                                                 ref dbSpecialistAccountItemConsumable,
                                                 ref dbVisit,
                                                 ref dbAssignment,
                                                 ref dbProject,
                                                 ref dbContract,
                                                 ref dbExpenseType,
                                                 dbModule,
                                                 commitChange,
                                                 isDbValidationRequired);
        }


        public Response Modify(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                              bool commitChange = true,
                              bool isDbValidationRequired = true,
                              long? visitId = null)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            if (visitId.HasValue)
                accountItemConsumables?.ToList().ForEach(x => { x.VisitId = visitId; });

            return UpdateTSConsumable(accountItemConsumables,
                                     ref dbSpecialistAccountItemConsumable,
                                     ref dbVisit,
                                     ref dbAssignment,
                                     ref dbProject,
                                     ref dbContract,
                                     ref dbExpenseType,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequired);
        }



        public Response Modify(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                    ref IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                    ref IList<DbModel.Visit> dbVisit,
                                    ref IList<DbModel.Assignment> dbAssignment,
                                    ref IList<DbModel.Project> dbProject,
                                    ref IList<DbModel.Contract> dbContract,
                                    ref IList<DbModel.Data> dbExpenseType,
                                    IList<DbModel.SqlauditModule> dbModule,
                                    bool commitChange = true,
                                    bool isDbValidationRequired = true,
                                    long? visitId = null)
        {
            if (visitId.HasValue)
                accountItemConsumables?.ToList().ForEach(x => { x.VisitId = visitId; });

            return UpdateTSConsumable(accountItemConsumables,
                                                 ref dbSpecialistAccountItemConsumable,
                                                 ref dbVisit,
                                                 ref dbAssignment,
                                                 ref dbProject,
                                                 ref dbContract,
                                                 ref dbExpenseType,
                                                 dbModule,
                                                 commitChange,
                                                 isDbValidationRequired);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                                ValidationType validationType)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable = null;
            return IsRecordValidForProcess(accountItemConsumables,
                                            validationType,
                                             ref dbSpecialistAccountItemConsumable,
                                             ref dbVisit,
                                             ref dbAssignment,
                                             ref dbProject,
                                             ref dbContract,
                                             ref dbExpenseType);

        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                                ValidationType validationType,
                                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType)
        {
            IList<DomainModel.VisitSpecialistAccountItemConsumable> filteredAccountItemConsumable = null;
            return IsRecordValidForProcess(accountItemConsumables,
                                             validationType,
                                             ref filteredAccountItemConsumable,
                                             ref dbSpecialistAccountItemConsumable,
                                             ref dbVisit,
                                             ref dbAssignment,
                                             ref dbProject,
                                             ref dbContract,
                                             ref dbExpenseType);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                                ValidationType validationType,
                                                 IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType)
        {
            return IsRecordValidForProcess(accountItemConsumables,
                                          validationType,
                                           dbSpecialistAccountItemConsumable,
                                           ref dbVisit,
                                           ref dbAssignment,
                                           ref dbProject,
                                           ref dbContract,
                                           ref dbExpenseType);
        }



        private Response AddTSConsumable(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                    ref IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                    ref IList<DbModel.Visit> dbVisit,
                                    ref IList<DbModel.Assignment> dbAssignment,
                                    ref IList<DbModel.Project> dbProject,
                                    ref IList<DbModel.Contract> dbContract,
                                    ref IList<DbModel.Data> dbExpenseType,
                                    IList<DbModel.SqlauditModule> dbModule,
                                    bool commitChange,
                                    bool isDbValidationRequired)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.Data> dbExpenseTypes = null;
            IList<DbModel.Project> dbProjects = null;
            IList<DbModel.Contract> dbContracts = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(accountItemConsumables, ValidationType.Add);
                eventId = accountItemConsumables.FirstOrDefault().EventId;
                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(accountItemConsumables,
                                                            ValidationType.Add,
                                                            ref recordToBeAdd,
                                                            ref dbSpecialistAccountItemConsumable,
                                                            ref dbVisit,
                                                            ref dbAssignment,
                                                            ref dbProject,
                                                            ref dbContract,
                                                            ref dbExpenseType);
                else if (dbSpecialistAccountItemConsumable?.Count <= 0)
                    dbSpecialistAccountItemConsumable = GetVisitTSConsumable(recordToBeAdd, ValidationType.Add);

                if (recordToBeAdd?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    _repository.AutoSave = false;
                    dbExpenseTypes = dbExpenseType;
                    dbProjects = dbProject;
                    dbContracts = dbContract;
                    dbSpecialistAccountItemConsumable = _mapper.Map<IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isVisitTSConsumableId"] = false;
                        opt.Items["Project"] = dbProjects;
                        opt.Items["Contract"] = dbContracts;
                        opt.Items["ExpenseType"] = dbExpenseTypes;
                    });
 
                    _repository.Add(dbSpecialistAccountItemConsumable);
                    if (commitChange)
                    {
                        var savCnt = _repository.ForceSave();
                        if (savCnt > 0)
                        {
                           
                            dbSpecialistAccountItemConsumable?.ToList().ForEach(x1 => _auditSearchService.AuditLog(recordToBeAdd.FirstOrDefault(), ref eventId, accountItemConsumables?.FirstOrDefault()?.ActionByUser,
                                                                                                       null,
                                                                                                       ValidationType.Add.ToAuditActionType(),
                                                                                                       SqlAuditModuleType.VisitTechnicalSpecialistAccountItemConsumable,
                                                                                                       null,
                                                                                                        _mapper.Map<DomainModel.VisitSpecialistAccountItemConsumable>(x1)
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemConsumables);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }


        private Response UpdateTSConsumable(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                      ref IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                      ref IList<DbModel.Visit> dbVisit,
                                      ref IList<DbModel.Assignment> dbAssignment,
                                      ref IList<DbModel.Project> dbProject,
                                      ref IList<DbModel.Contract> dbContract,
                                      ref IList<DbModel.Data> dbExpenseType,
                                      IList<DbModel.SqlauditModule> dbModule,
                                      bool commitChange,
                                      bool isDbValidationRequired)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.Data> dbExpenseTypes = null;
            IList<DbModel.Project> dbProjects = null;
            IList<DbModel.Contract> dbContracts = null;
            IList<DomainModel.VisitSpecialistAccountItemTravel> result = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(accountItemConsumables, ValidationType.Update);
                eventId = accountItemConsumables?.FirstOrDefault()?.EventId;

                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(accountItemConsumables,
                                                            ValidationType.Update,
                                                            ref recordToBeModify,
                                                            ref dbSpecialistAccountItemConsumable,
                                                            ref dbVisit,
                                                            ref dbAssignment,
                                                            ref dbProject,
                                                            ref dbContract,
                                                            ref dbExpenseType);
                else if (dbSpecialistAccountItemConsumable?.Count <= 0)
                    dbSpecialistAccountItemConsumable = GetVisitTSConsumable(recordToBeModify, ValidationType.Update);

                if (recordToBeModify?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    if (dbSpecialistAccountItemConsumable?.Count > 0)
                    {
                        dbExpenseTypes = dbExpenseType;
                        dbProjects = dbProject;
                        dbContracts = dbContract;
                        IList<DomainModel.VisitSpecialistAccountItemConsumable> domExistingVisitSpecialistAccountItemConsumbles = new List<DomainModel.VisitSpecialistAccountItemConsumable>();
                        dbSpecialistAccountItemConsumable?.ToList()?.ForEach(x =>
                        {
                            domExistingVisitSpecialistAccountItemConsumbles.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.VisitSpecialistAccountItemConsumable>(x)));
                        });


                        dbSpecialistAccountItemConsumable.ToList().ForEach(visitTS =>
                        {
                            var visitTSConsumableToBeModify = recordToBeModify.FirstOrDefault(x => x.VisitTechnicalSpecialistAccountConsumableId == visitTS.Id);
                            _mapper.Map(visitTSConsumableToBeModify, visitTS, opt =>
                            {
                                opt.Items["isVisitTSConsumableId"] = true;
                                opt.Items["Project"] = dbProjects;
                                opt.Items["Contract"] = dbContracts;
                                opt.Items["ExpenseType"] = dbExpenseTypes;
                            });
                            visitTS.LastModification = DateTime.UtcNow;
                            visitTS.UpdateCount = visitTSConsumableToBeModify.UpdateCount.CalculateUpdateCount();
                            visitTS.ModifiedBy = visitTSConsumableToBeModify.ModifiedBy;
                        });

                        _repository.AutoSave = false;
                        _repository.Update(dbSpecialistAccountItemConsumable);
                        if (commitChange)
                        {
                            var updCnt = _repository.ForceSave();
                            if (updCnt > 0)
                            {
                               
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                   null,
                                                                   ValidationType.Update.ToAuditActionType(),
                                                                   SqlAuditModuleType.VisitTechnicalSpecialistAccountItemConsumable,
                                                                   domExistingVisitSpecialistAccountItemConsumbles?.FirstOrDefault(x2 => x2.VisitTechnicalSpecialistAccountConsumableId == x1.VisitTechnicalSpecialistAccountConsumableId),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemConsumables);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveTSConsumable(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                      ref IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                      ref IList<DbModel.Visit> dbVisit,
                                      ref IList<DbModel.Assignment> dbAssignment,
                                      ref IList<DbModel.Project> dbProject,
                                      ref IList<DbModel.Contract> dbContract,
                                      ref IList<DbModel.Data> dbExpenseType,
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
                var recordToBeDelete = FilterRecord(accountItemConsumables, ValidationType.Delete);
                eventId = accountItemConsumables?.FirstOrDefault()?.EventId;

                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(accountItemConsumables,
                                                       ValidationType.Delete,
                                                       ref recordToBeDelete,
                                                       ref dbSpecialistAccountItemConsumable,
                                                       ref dbVisit,
                                                       ref dbAssignment,
                                                       ref dbProject,
                                                       ref dbContract,
                                                       ref dbExpenseType);

                if (recordToBeDelete?.Count > 0 && (response == null || response.Code == MessageType.Success.ToId()))
                {
                    if (dbSpecialistAccountItemConsumable?.Count > 0)
                    {
                        _repository.AutoSave = false;
                        _repository.Delete(dbSpecialistAccountItemConsumable);
                        if (commitChange)
                        {
                            var delCnt = _repository.ForceSave();
                            if (delCnt > 0)
                            {

                                dbSpecialistAccountItemConsumable?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, accountItemConsumables?.FirstOrDefault()?.ActionByUser,
                                                                                                          null,
                                                                                                          ValidationType.Delete.ToAuditActionType(),
                                                                                                          SqlAuditModuleType.VisitTechnicalSpecialistAccountItemConsumable,
                                                                                                          _mapper.Map<DomainModel.VisitSpecialistAccountItemConsumable>(x1),
                                                                                                          null,
                                                                                                          dbModule ));
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemConsumables);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }


        private IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> GetVisitTSConsumable(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                                                       ValidationType validationType)
        {
            IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable = null;
            if (validationType != ValidationType.Add)
            {
                if (accountItemConsumables?.Count > 0)
                {
                    var visitTechnicalSpecialistAccountConsumableId = accountItemConsumables.Select(x => x.VisitTechnicalSpecialistAccountConsumableId).Distinct().ToList();
                    dbSpecialistAccountItemConsumable = _repository.FindBy(x => visitTechnicalSpecialistAccountConsumableId.Contains(x.Id)).ToList();
                }
            }
            return dbSpecialistAccountItemConsumable;
        }


        private IList<DomainModel.VisitSpecialistAccountItemConsumable> FilterRecord(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                                                    ValidationType filterType)
        {
            IList<DomainModel.VisitSpecialistAccountItemConsumable> filteredTSConsumable = null;

            if (filterType == ValidationType.Add)
                filteredTSConsumable = accountItemConsumables?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredTSConsumable = accountItemConsumables?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredTSConsumable = accountItemConsumables?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredTSConsumable;
        }


        private bool IsValidPayload(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _techSpecAccountItemConsumableValidationService.Validate(JsonConvert.SerializeObject(accountItemConsumables), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Visit, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }


        private void FetchMasterData(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                     ref IList<DbModel.Data> masterData)
        {
            if (masterData == null)
            {
                IList<MasterType> types = new List<MasterType>() { MasterType.ExpenseType, MasterType.Currency };
                masterData = _masterService.Get(types);
            }
        }


        private bool IsValidExpenseType(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                         ref IList<DbModel.Data> dbExpenseType,
                                         IList<DbModel.Data> masterData,
                                         ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var dbExpense = masterData.Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType &&
                                                x.Type == ExpenseType.C.ToString() || x.Type == ExpenseType.Q.ToString()).ToList();

            var expenseTypeNotExists = accountItemConsumables.Where(x=>!string.IsNullOrEmpty(x.ChargeExpenseType))?.Where(x => !dbExpense.Any(x1 => x1.Name == x.ChargeExpenseType)).ToList();
            expenseTypeNotExists.ToList().ForEach(x =>
            {
                messages.Add(_messages, x.ChargeExpenseType, MessageType.VisitTSExpenseTypeInvalid, x.ChargeExpenseType);
            });

            if (dbExpenseType?.Count == 0 || dbExpenseType == null)
                dbExpenseType = dbExpense;

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsValidCurrency(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                         IList<DbModel.Data> masterData,
                                         ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (masterData != null)
            {
                var dbChargeCurrency = masterData.Where(x => x.MasterDataTypeId == (int)MasterType.Currency).ToList();
                var dbPayCurrency = masterData.Where(x => x.MasterDataTypeId == (int)MasterType.Currency).ToList();

                var chargeNotExists = accountItemConsumables.Where(x => !string.IsNullOrEmpty(x.ChargeRateCurrency))?.Where(x => !dbChargeCurrency.Any(x1 => x1.Code == x.ChargeRateCurrency)).ToList();
                var payNotExists = accountItemConsumables.Where(x => !string.IsNullOrEmpty(x.PayRateCurrency))?.Where(x => !dbPayCurrency.Any(x1 => x1.Code == x.PayRateCurrency)).ToList();
                chargeNotExists.ToList().ForEach(x =>
                {
                    messages.Add(_messages, x.ChargeExpenseType, MessageType.VisitTSChargeCurrencyInvalid, x.ChargeExpenseType);
                });

                payNotExists.ToList().ForEach(x =>
                {
                    messages.Add(_messages, x.ChargeExpenseType, MessageType.VisitTSPayCurrencyInvalid, x.ChargeExpenseType);
                });
            }


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsValidContractProjectAssignmentVisit(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                                               IList<DbModel.Visit> dbVisit,
                                                               ref IList<ValidationMessage> validationMessages)
        {

            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbVisit != null)
            {
                var dbContract = dbVisit?.ToList()?.Select(x => x.Assignment?.Project?.Contract).ToList();
                if (dbContract == null || dbContract.Count == 0)
                    messages.Add(_messages, null, MessageType.VisitTSVisitContractInvalid, null);

                if (messages.Count > 0)
                    validationMessages.AddRange(messages);
            }

            return messages?.Count <= 0;
        }



        private bool IsRecordUpdateCountMatching(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                                 IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = accountItemConsumables.Where(x => !dbSpecialistAccountItemConsumable.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() &&
                                                                                                             x1.Id == x.VisitTechnicalSpecialistAccountConsumableId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x.VisitTechnicalSpecialistAccountConsumableId, MessageType.VisitConsumableUpdateCountMisMatch, x.VisitTechnicalSpecialistAccountConsumableId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsVisitConsumablesExistInDb(IList<long> accountItemTravelIds,
                                                   IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                                  ref IList<long> visitTravelIdNotExists,
                                                  ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbSpecialistAccountItemConsumable == null)
                dbSpecialistAccountItemConsumable = new List<DbModel.VisitTechnicalSpecialistAccountItemConsumable>();

            var validMessages = validationMessages;

            if (accountItemTravelIds?.Count > 0)
            {
                visitTravelIdNotExists = accountItemTravelIds.Where(x => !dbSpecialistAccountItemConsumable.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                visitTravelIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.VisitTravelInvalid, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidForAdd(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                        IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                        ref IList<DbModel.Visit> dbVisit,
                                        ref IList<DbModel.Assignment> dbAssignment,
                                        ref IList<DbModel.Project> dbProject,
                                        ref IList<DbModel.Contract> dbContract,
                                        ref IList<DbModel.Data> dbExpenseType,
                                        ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.Data> dbMasterData = null;
            IList<long> notExists = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var visitIds = accountItemConsumables.Where(x => x.VisitId > 0).Select(x => (long)x.VisitId).Distinct().ToList();
            var visitTechSpecIds = accountItemConsumables.Where(x => x.VisitTechnicalSpecialistId > 0).Select(x => (long)x.VisitTechnicalSpecialistId).Distinct().ToList();

            if (dbExpenseType?.Count > 0)
                dbMasterData = dbExpenseType;
            else
                FetchMasterData(accountItemConsumables, ref dbMasterData);

            if (IsValidVisit(visitIds, ref dbVisit, ref messages, new string[] {  "VisitTechnicalSpecialist",
                                                                                                                "Assignment",
                                                                                                                "Assignment.Project",
                                                                                                                "Assignment.Project.Contract"}))
                if (IsValidExpenseType(accountItemConsumables, ref dbExpenseType, dbMasterData, ref messages))
                    if (IsValidCurrency(accountItemConsumables, dbMasterData, ref messages))
                        if (IsVisitTechnicalSpecialistExistInDb(visitTechSpecIds, dbVisit.ToList().SelectMany(x => x.VisitTechnicalSpecialist).ToList(), ref notExists, ref messages))
                            IsValidContractProjectAssignmentVisit(accountItemConsumables, dbVisit, ref messages);

            dbAssignment = dbVisit?.ToList().Select(x => x.Assignment)?.ToList();
            dbProject = dbVisit?.ToList().Select(x => x.Assignment.Project)?.ToList();
            dbContract = dbVisit?.ToList().Select(x => x.Assignment.Project.Contract)?.ToList();


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

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

        private bool IsVisitTechnicalSpecialistExistInDb(IList<long> visitTechnicalSpecialistIds,
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

        private bool IsRecordValidForUpdate(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                         IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                        ref IList<DbModel.Visit> dbVisit,
                                        ref IList<DbModel.Assignment> dbAssignment,
                                        ref IList<DbModel.Project> dbProject,
                                        ref IList<DbModel.Contract> dbContract,
                                        ref IList<DbModel.Data> dbExpenseType,
                                        ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            IList<DbModel.Data> dbMasterData = null;
            IList<long> notExists = null;

            var visitIds = accountItemConsumables.Where(x => x.VisitId > 0).Select(x => (long)x.VisitId).Distinct().ToList();
            var visitTechSpecIds = accountItemConsumables.Where(x => x.VisitTechnicalSpecialistId > 0).Select(x => (long)x.VisitTechnicalSpecialistId).Distinct().ToList();
            if (dbExpenseType?.Count > 0)
                dbMasterData = dbExpenseType;
            else
                FetchMasterData(accountItemConsumables, ref dbMasterData);
            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(accountItemConsumables, dbSpecialistAccountItemConsumable, ref messages))
                    if (IsValidVisit(visitIds, ref dbVisit, ref messages, new string[] {  "VisitTechnicalSpecialist",
                                                                                                                        "Assignment",
                                                                                                                        "Assignment.Project",
                                                                                                                        "Assignment.Project.Contract"}))
                        if (IsValidExpenseType(accountItemConsumables, ref dbExpenseType, dbMasterData, ref messages))
                            if (IsValidCurrency(accountItemConsumables, dbMasterData, ref messages))
                                if (IsVisitTechnicalSpecialistExistInDb(visitTechSpecIds, dbVisit.ToList().SelectMany(x => x.VisitTechnicalSpecialist).ToList(), ref notExists, ref messages))
                                    IsValidContractProjectAssignmentVisit(accountItemConsumables, dbVisit, ref messages);
            }

            dbAssignment = dbVisit?.ToList().Select(x => x.Assignment)?.ToList();
            dbProject = dbVisit?.ToList().Select(x => x.Assignment.Project)?.ToList();
            dbContract = dbVisit?.ToList().Select(x => x.Assignment.Project.Contract)?.ToList();


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemConsumable> accountItemConsumables,
                                                ValidationType validationType,
                                                ref IList<DomainModel.VisitSpecialistAccountItemConsumable> filteredAccountItemConsumables,
                                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (accountItemConsumables != null && accountItemConsumables.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredAccountItemConsumables == null || filteredAccountItemConsumables.Count <= 0)
                        filteredAccountItemConsumables = FilterRecord(accountItemConsumables, validationType);

                    if (filteredAccountItemConsumables != null && filteredAccountItemConsumables?.Count > 0)
                    {
                        result = IsValidPayload(filteredAccountItemConsumables,
                                                validationType,
                                                ref validationMessages);
                        if (filteredAccountItemConsumables?.Count > 0 && result)
                        {
                            IList<long> moduleNotExists = null;
                            var visitTSConsumableId = filteredAccountItemConsumables.Where(x => x.VisitTechnicalSpecialistAccountConsumableId.HasValue)
                                                                                         .Select(x => (long)x.VisitTechnicalSpecialistAccountConsumableId).Distinct().ToList();

                            if (dbSpecialistAccountItemConsumable == null || dbSpecialistAccountItemConsumable.Count <= 0)
                                dbSpecialistAccountItemConsumable = GetVisitTSConsumable(filteredAccountItemConsumables, validationType);
                            else
                            {
                                IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbItemConsumable = GetVisitTSConsumable(filteredAccountItemConsumables, validationType);
                                if(dbItemConsumable != null && dbItemConsumable.Count > 0)
                                    dbSpecialistAccountItemConsumable.AddRange(dbItemConsumable);
                            }

                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                            {
                                result = IsVisitConsumablesExistInDb(visitTSConsumableId,
                                                                      dbSpecialistAccountItemConsumable,
                                                                      ref moduleNotExists,
                                                                      ref validationMessages);

                                if (result && validationType == ValidationType.Update)
                                    result = IsRecordValidForUpdate(filteredAccountItemConsumables,
                                                                    dbSpecialistAccountItemConsumable,
                                                                    ref dbVisit,
                                                                    ref dbAssignment,
                                                                    ref dbProject,
                                                                    ref dbContract,
                                                                    ref dbExpenseType,
                                                                    ref validationMessages);
                            }
                            else if (validationType == ValidationType.Add)
                                result = IsRecordValidForAdd(filteredAccountItemConsumables,
                                                                 dbSpecialistAccountItemConsumable,
                                                                 ref dbVisit,
                                                                 ref dbAssignment,
                                                                 ref dbProject,
                                                                 ref dbContract,
                                                                 ref dbExpenseType,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemConsumables);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
    }
}
