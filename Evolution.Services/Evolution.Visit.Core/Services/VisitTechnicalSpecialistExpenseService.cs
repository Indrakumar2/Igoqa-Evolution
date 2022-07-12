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
    public class VisitTechnicalSpecialistExpenseService : IVisitTechnicalSpecialistExpenseService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<VisitTechnicalSpecialistExpenseService> _logger = null;
        private readonly IVisitTechnicalSpecialistExpenseRepository _repository = null;
        private readonly IVisitTechnicalSpecialistAccountItemExpenseValidationService _techSpecAccountItemExpenseValidationService = null;
        private readonly IVisitRepository _visitRepository = null;
        private readonly IMasterService _masterService = null;
        private readonly JObject _messages = null;
        private readonly IAuditSearchService _auditSearchService = null;

        public VisitTechnicalSpecialistExpenseService(IAppLogger<VisitTechnicalSpecialistExpenseService> logger,
                                                IVisitTechnicalSpecialistExpenseRepository visitExpenseRepository,
                                                IVisitTechnicalSpecialistAccountItemExpenseValidationService validationService,
                                                IVisitRepository visitRepository,
                                                IMasterService masterService,
                                                IMapper mapper,
                                                JObject messages, IAuditSearchService auditSearchService)
                                         
        {
            _mapper = mapper;
            _logger = logger;
            _repository = visitExpenseRepository;
            _visitRepository = visitRepository;
            _masterService = masterService;
            _techSpecAccountItemExpenseValidationService = validationService;
            this._messages = messages;
            _auditSearchService = auditSearchService;

        }

        public Response Get(DomainModel.VisitSpecialistAccountItemExpense searchModel)
        {
            IList<DomainModel.VisitSpecialistAccountItemExpense> result = null;
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

        public async Task<Response> GetAsync(DomainModel.VisitSpecialistAccountItemExpense searchModel)
        {
            IList<DomainModel.VisitSpecialistAccountItemExpense> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                   new TransactionOptions
                                                   {
                                                       IsolationLevel = IsolationLevel.ReadUncommitted
                                                   },TransactionScopeAsyncFlowOption.Enabled))
                {
                    await Task.Run(() => result = this._repository.Search(searchModel));
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

        public Response Add(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                          bool commitChange = true,
                          bool isDbValidationRequired = true,
                          long? visitId = null)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            if (visitId.HasValue)
                accountItemExpense?.ToList().ForEach(x => { x.VisitId = visitId; });

            return AddTSTravel(accountItemExpense,
                                         ref dbSpecialistAccountItemExpense,
                                         ref dbVisit,
                                         ref dbAssignment,
                                         ref dbProject,
                                         ref dbContract,
                                         ref dbExpenseType,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }


        public Response Add(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                            ref IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
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
                accountItemExpense?.ToList().ForEach(x => { x.VisitId = visitId; });

            return AddTSTravel(accountItemExpense,
                                         ref dbSpecialistAccountItemExpense,
                                         ref dbVisit,
                                         ref dbAssignment,
                                         ref dbProject,
                                         ref dbContract,
                                         ref dbExpenseType,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                  bool commitChange = true,
                                  bool isDbValidationRequired = true,
                                  long? visitId = null)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense = null;
            if (visitId.HasValue)
                accountItemExpense?.ToList().ForEach(x => { x.VisitId = visitId; });

            return RemoveTSTravel(accountItemExpense,
                                     ref dbSpecialistAccountItemExpense,
                                     ref dbVisit,
                                     ref dbAssignment,
                                     ref dbProject,
                                     ref dbContract,
                                     ref dbExpenseType,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
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
                accountItemExpense?.ToList().ForEach(x => { x.VisitId = visitId; });
            return RemoveTSTravel(accountItemExpense,
                                                 ref dbSpecialistAccountItemExpense,
                                                 ref dbVisit,
                                                 ref dbAssignment,
                                                 ref dbProject,
                                                 ref dbContract,
                                                 ref dbExpenseType,
                                                 dbModule,
                                                 commitChange,
                                                 isDbValidationRequired);
        }


        public Response Modify(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                              bool commitChange = true,
                              bool isDbValidationRequired = true,
                              long? visitId = null)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense = null;
            if (visitId.HasValue)
                accountItemExpense?.ToList().ForEach(x => { x.VisitId = visitId; });

            return UpdateTSTravel(accountItemExpense,
                                     ref dbSpecialistAccountItemExpense,
                                     ref dbVisit,
                                     ref dbAssignment,
                                     ref dbProject,
                                     ref dbContract,
                                     ref dbExpenseType,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequired);
        }



        public Response Modify(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                    ref IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
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
                accountItemExpense?.ToList().ForEach(x => { x.VisitId = visitId; });

            return UpdateTSTravel(accountItemExpense,
                                                 ref dbSpecialistAccountItemExpense,
                                                 ref dbVisit,
                                                 ref dbAssignment,
                                                 ref dbProject,
                                                 ref dbContract,
                                                 ref dbExpenseType,
                                                 dbModule,
                                                 commitChange,
                                                 isDbValidationRequired);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                                ValidationType validationType)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense = null;
            return IsRecordValidForProcess(accountItemExpense,
                                            validationType,
                                             ref dbSpecialistAccountItemExpense,
                                             ref dbVisit,
                                             ref dbAssignment,
                                             ref dbProject,
                                             ref dbContract,
                                             ref dbExpenseType);

        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                                ValidationType validationType,
                                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType)
        {
            IList<DomainModel.VisitSpecialistAccountItemExpense> filteredAccountItemTravel = null;
            return IsRecordValidForProcess(accountItemExpense,
                                             validationType,
                                             ref filteredAccountItemTravel,
                                             ref dbSpecialistAccountItemExpense,
                                             ref dbVisit,
                                             ref dbAssignment,
                                             ref dbProject,
                                             ref dbContract,
                                             ref dbExpenseType);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                                ValidationType validationType,
                                                IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType)
        {
            return IsRecordValidForProcess(accountItemExpense,
                                          validationType,
                                           dbSpecialistAccountItemExpense,
                                           ref dbVisit,
                                           ref dbAssignment,
                                           ref dbProject,
                                           ref dbContract,
                                           ref dbExpenseType);
        }



        private Response AddTSTravel(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                    ref IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
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
                var recordToBeAdd = FilterRecord(accountItemExpense, ValidationType.Add);
                eventId = accountItemExpense?.FirstOrDefault()?.EventId;

                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(accountItemExpense,
                                                            ValidationType.Add,
                                                            ref recordToBeAdd,
                                                            ref dbSpecialistAccountItemExpense,
                                                            ref dbVisit,
                                                            ref dbAssignment,
                                                            ref dbProject,
                                                            ref dbContract,
                                                            ref dbExpenseType);
                else if (dbSpecialistAccountItemExpense?.Count <= 0)
                    dbSpecialistAccountItemExpense = GetVisitTSExpense(recordToBeAdd, ValidationType.Add);

                if (recordToBeAdd?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    _repository.AutoSave = false;
                    dbExpenseTypes = dbExpenseType;
                    dbProjects = dbProject;
                    dbContracts = dbContract;
                    dbSpecialistAccountItemExpense = _mapper.Map<IList<DbModel.VisitTechnicalSpecialistAccountItemExpense>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isVisitTSExpenseId"] = false;
                        opt.Items["Project"] = dbProjects;
                        opt.Items["Contract"] = dbContracts;
                        opt.Items["ExpenseType"] = dbExpenseTypes;

                    });
  
                    _repository.Add(dbSpecialistAccountItemExpense);
                    if (commitChange)
                    {
                        var savCnt = _repository.ForceSave();
                        if (savCnt > 0)
                        {
                              dbSpecialistAccountItemExpense?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, accountItemExpense?.FirstOrDefault()?.ActionByUser,
                                                                                                                       null,
                                                                                                                       ValidationType.Add.ToAuditActionType(),
                                                                                                                       SqlAuditModuleType.VisitTechnicalSpecialistAccountItemExpense,
                                                                                                                       null,
                                                                                                                       _mapper.Map<DomainModel.VisitSpecialistAccountItemExpense>(x1)
                                                                                                                       ,dbModule));

                        }
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemExpense);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateTSTravel(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                      ref IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
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
            IList<DomainModel.VisitSpecialistAccountItemExpense> result = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(accountItemExpense, ValidationType.Update);

                eventId = accountItemExpense.Select(x => x.EventId).FirstOrDefault();
                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(accountItemExpense,
                                                            ValidationType.Update,
                                                            ref recordToBeModify,
                                                            ref dbSpecialistAccountItemExpense,
                                                            ref dbVisit,
                                                            ref dbAssignment,
                                                            ref dbProject,
                                                            ref dbContract,
                                                            ref dbExpenseType);
                else if (dbSpecialistAccountItemExpense?.Count <= 0)
                    dbSpecialistAccountItemExpense = GetVisitTSExpense(recordToBeModify, ValidationType.Update);

                if (recordToBeModify?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    if (dbSpecialistAccountItemExpense?.Count > 0)
                    {
                        dbExpenseTypes = dbExpenseType;
                        dbProjects = dbProject;
                        dbContracts = dbContract;
                        IList<DomainModel.VisitSpecialistAccountItemExpense> domExistingVisitSpecialistAccountItemExpense = new List<DomainModel.VisitSpecialistAccountItemExpense>();
                        dbSpecialistAccountItemExpense?.ToList()?.ForEach(x =>
                        {
                            domExistingVisitSpecialistAccountItemExpense.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.VisitSpecialistAccountItemExpense>(x)));
                        });
                        dbSpecialistAccountItemExpense.ToList().ForEach(visitTS =>
                        {
                            var visitTSExpenseToBeModify = recordToBeModify.FirstOrDefault(x => x.VisitTechnicalSpecialistAccountExpenseId == visitTS.Id);
                            _mapper.Map(visitTSExpenseToBeModify, visitTS, opt =>
                            {
                                opt.Items["isVisitTSExpenseId"] = true;
                                opt.Items["Project"] = dbProjects;
                                opt.Items["Contract"] = dbContracts;
                                opt.Items["ExpenseType"] = dbExpenseTypes;
                            });
                            visitTS.LastModification = DateTime.UtcNow;
                            visitTS.UpdateCount = visitTSExpenseToBeModify.UpdateCount.CalculateUpdateCount();
                            visitTS.ModifiedBy = visitTSExpenseToBeModify.ModifiedBy;
                        });

                        _repository.AutoSave = false;
                        _repository.Update(dbSpecialistAccountItemExpense);
                        if (commitChange)
                        {
                            var updCnt = _repository.ForceSave();
                            if (updCnt > 0)
                            {
                                  recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                   null,
                                                                   ValidationType.Update.ToAuditActionType(),
                                                                   SqlAuditModuleType.VisitTechnicalSpecialistAccountItemExpense,
                                                                   domExistingVisitSpecialistAccountItemExpense?.FirstOrDefault(x2 => x2.VisitTechnicalSpecialistAccountExpenseId == x1.VisitTechnicalSpecialistAccountExpenseId),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemExpense);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveTSTravel(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                      ref IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
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
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                Response response = null;
                long? eventId = 0;
                var recordToBeDelete = FilterRecord(accountItemExpense, ValidationType.Delete);
                eventId = accountItemExpense.Select(x => x.EventId).FirstOrDefault();
                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(accountItemExpense,
                                                       ValidationType.Delete,
                                                       ref recordToBeDelete,
                                                       ref dbSpecialistAccountItemExpense,
                                                       ref dbVisit,
                                                       ref dbAssignment,
                                                       ref dbProject,
                                                       ref dbContract,
                                                       ref dbExpenseType);

                if (recordToBeDelete?.Count > 0 && (response == null || response.Code == MessageType.Success.ToId()))
                {
                    if (dbSpecialistAccountItemExpense?.Count > 0)
                    {
                        _repository.AutoSave = false;
                        _repository.Delete(dbSpecialistAccountItemExpense);
                        if (commitChange)
                        {
                            var delCnt = _repository.ForceSave();
                            if (delCnt > 0)
                            {
                                dbSpecialistAccountItemExpense?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, accountItemExpense?.FirstOrDefault()?.ActionByUser,
                                                                                                      null,
                                                                                                      ValidationType.Delete.ToAuditActionType(),
                                                                                                      SqlAuditModuleType.VisitTechnicalSpecialistAccountItemExpense,
                                                                                                      _mapper.Map<DomainModel.VisitSpecialistAccountItemExpense>(x1),
                                                                                                      null,
                                                                                                      dbModule));
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemExpense);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }


        private IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> GetVisitTSExpense(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemTravel,
                                                                       ValidationType validationType)
        {
            IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense = null;
            if (validationType != ValidationType.Add)
            {
                if (accountItemTravel?.Count > 0)
                {
                    var visitTechnicalSpecialistAccountExpenseId = accountItemTravel.Select(x => x.VisitTechnicalSpecialistAccountExpenseId).Distinct().ToList();
                    dbSpecialistAccountItemExpense = _repository.FindBy(x => visitTechnicalSpecialistAccountExpenseId.Contains(x.Id)).ToList();
                }
            }
            return dbSpecialistAccountItemExpense;
        }


        private IList<DomainModel.VisitSpecialistAccountItemExpense> FilterRecord(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemTravel,
                                                                    ValidationType filterType)
        {
            IList<DomainModel.VisitSpecialistAccountItemExpense> filteredTSExpense = null;

            if (filterType == ValidationType.Add)
                filteredTSExpense = accountItemTravel?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredTSExpense = accountItemTravel?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredTSExpense = accountItemTravel?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredTSExpense;
        }


        private bool IsValidPayload(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemTravel,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _techSpecAccountItemExpenseValidationService.Validate(JsonConvert.SerializeObject(accountItemTravel), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Visit, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }


        private void FetchMasterData(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                     ref IList<DbModel.Data> masterData)
        {
            if (masterData == null)
            {
                IList<MasterType> types = new List<MasterType>() { MasterType.ExpenseType, MasterType.Currency };
                masterData = _masterService.Get(types);
            }
        }


        private bool IsValidExpenseType(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                         ref IList<DbModel.Data> dbExpenseType,
                                         IList<DbModel.Data> masterData,
                                         ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var dbExpense = masterData.Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType &&
                                                x.Type == ExpenseType.E.ToString()).ToList();

            var expenseTypeNotExists = accountItemExpense.Where(x => !string.IsNullOrEmpty(x.ChargeExpenseType))?.Where(x => !dbExpense.Any(x1 => x1.Name == x.ChargeExpenseType)).ToList();
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

        private bool IsValidCurrency(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
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

                var chargeNotExists = accountItemExpense.Where(x => !string.IsNullOrEmpty(x.ChargeRateCurrency))?.Where(x => !dbChargeCurrency.Any(x1 => x1.Code == x.ChargeRateCurrency)).ToList();
                var payNotExists = accountItemExpense.Where(x => !string.IsNullOrEmpty(x.PayRateCurrency))?.Where(x => !dbPayCurrency.Any(x1 => x1.Code == x.PayRateCurrency)).ToList();
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


        private bool IsValidContractProjectAssignmentVisit(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
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



        private bool IsRecordUpdateCountMatching(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                                IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = accountItemExpense.Where(x => !dbSpecialistAccountItemExpense.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() &&
                                                                                                             x1.Id == x.VisitTechnicalSpecialistAccountExpenseId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x.VisitTechnicalSpecialistAccountExpenseId, MessageType.VisitExpenseUpdateCountMisMatch, x.VisitTechnicalSpecialistAccountExpenseId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsVisitExpenseExistInDb(IList<long> accountItemExpenseIds,
                                                  IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                                                  ref IList<long> visitExpenseIdNotExists,
                                                  ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbSpecialistAccountItemExpense == null)
                dbSpecialistAccountItemExpense = new List<DbModel.VisitTechnicalSpecialistAccountItemExpense>();

            var validMessages = validationMessages;

            if (accountItemExpenseIds?.Count > 0)
            {
                visitExpenseIdNotExists = accountItemExpenseIds.Where(x => !dbSpecialistAccountItemExpense.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                visitExpenseIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.VisitExpenseInvalid, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidForAdd(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                        IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
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

            var visitIds = accountItemExpense.Where(x => x.VisitId > 0).Select(x => (long)x.VisitId).Distinct().ToList();
            var visitTechSpecIds = accountItemExpense.Where(x => x.VisitTechnicalSpecialistId > 0).Select(x => (long)x.VisitTechnicalSpecialistId).Distinct().ToList();
            if (dbExpenseType?.Count > 0)
                dbMasterData = dbExpenseType;
            else
                FetchMasterData(accountItemExpense, ref dbMasterData);

            if (IsValidVisit(visitIds, ref dbVisit, ref messages, new string[] {  "VisitTechnicalSpecialist",
                                                                                    "Assignment",
                                                                                    "Assignment.Project",
                                                                                    "Assignment.Project.Contract"}))
                if (IsValidExpenseType(accountItemExpense, ref dbExpenseType, dbMasterData, ref messages))
                    if (IsValidCurrency(accountItemExpense, dbMasterData, ref messages))
                        if (IsVisitTechnicalSpecialistExistInDb(visitTechSpecIds, dbVisit.ToList().SelectMany(x => x.VisitTechnicalSpecialist).ToList(), ref notExists, ref messages))
                            IsValidContractProjectAssignmentVisit(accountItemExpense, dbVisit, ref messages);

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


        private bool IsRecordValidForUpdate(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                        IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
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

            var visitIds = accountItemExpense.Where(x => x.VisitId > 0).Select(x => (long)x.VisitId).Distinct().ToList();
            var visitTechSpecIds = accountItemExpense.Where(x => x.VisitTechnicalSpecialistId > 0).Select(x => (long)x.VisitTechnicalSpecialistId).Distinct().ToList();
            FetchMasterData(accountItemExpense, ref dbMasterData);
            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(accountItemExpense, dbSpecialistAccountItemExpense, ref messages))
                    if (IsValidVisit(visitIds, ref dbVisit, ref messages, new string[] {  "VisitTechnicalSpecialist",
                                                                                                                        "Assignment",
                                                                                                                        "Assignment.Project",
                                                                                                                        "Assignment.Project.Contract"}))
                        if (IsValidExpenseType(accountItemExpense, ref dbExpenseType, dbMasterData, ref messages))
                            if (IsValidCurrency(accountItemExpense, dbMasterData, ref messages))
                                if (IsVisitTechnicalSpecialistExistInDb(visitTechSpecIds, dbVisit.ToList().SelectMany(x => x.VisitTechnicalSpecialist).ToList(), ref notExists, ref messages))
                                    IsValidContractProjectAssignmentVisit(accountItemExpense, dbVisit, ref messages);
            }

            dbAssignment = dbVisit?.ToList().Select(x => x.Assignment)?.ToList();
            dbProject = dbVisit?.ToList().Select(x => x.Assignment.Project)?.ToList();
            dbContract = dbVisit?.ToList().Select(x => x.Assignment.Project.Contract)?.ToList();


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemExpense> accountItemExpense,
                                                ValidationType validationType,
                                                ref IList<DomainModel.VisitSpecialistAccountItemExpense> filteredAccountItemExpense,
                                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
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
                if (accountItemExpense != null && accountItemExpense.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredAccountItemExpense == null || filteredAccountItemExpense.Count <= 0)
                        filteredAccountItemExpense = FilterRecord(accountItemExpense, validationType);

                    if (filteredAccountItemExpense != null && filteredAccountItemExpense?.Count > 0)
                    {
                        result = IsValidPayload(filteredAccountItemExpense,
                                                validationType,
                                                ref validationMessages);
                        if (filteredAccountItemExpense?.Count > 0 && result)
                        {
                            IList<long> moduleNotExists = null;
                            var visitTSExpenseId = filteredAccountItemExpense.Where(x => x.VisitTechnicalSpecialistAccountExpenseId.HasValue)
                                                                                         .Select(x => (long)x.VisitTechnicalSpecialistAccountExpenseId).Distinct().ToList();

                            if (dbSpecialistAccountItemExpense == null || dbSpecialistAccountItemExpense.Count <= 0)
                                dbSpecialistAccountItemExpense = GetVisitTSExpense(filteredAccountItemExpense, validationType);
                            else
                            {
                                IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbItemExpense = GetVisitTSExpense(filteredAccountItemExpense, validationType);
                                if(dbItemExpense != null && dbItemExpense.Count > 0)
                                    dbSpecialistAccountItemExpense.AddRange(dbItemExpense);
                            }

                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                            {
                                result = IsVisitExpenseExistInDb(visitTSExpenseId,
                                                                      dbSpecialistAccountItemExpense,
                                                                      ref moduleNotExists,
                                                                      ref validationMessages);

                                if (result && validationType == ValidationType.Update)
                                    result = IsRecordValidForUpdate(filteredAccountItemExpense,
                                                                    dbSpecialistAccountItemExpense,
                                                                    ref dbVisit,
                                                                    ref dbAssignment,
                                                                    ref dbProject,
                                                                    ref dbContract,
                                                                    ref dbExpenseType,
                                                                    ref validationMessages);
                            }
                            else if (validationType == ValidationType.Add)
                                result = IsRecordValidForAdd(filteredAccountItemExpense,
                                                                 dbSpecialistAccountItemExpense,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemExpense);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
    }
}
