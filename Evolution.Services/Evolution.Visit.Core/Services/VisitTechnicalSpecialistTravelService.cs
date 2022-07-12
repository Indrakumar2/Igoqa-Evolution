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
    public class VisitTechnicalSpecialistTravelService : IVisitTechnicalSpecialistTravelService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<VisitTechnicalSpecialistTravelService> _logger = null;
        private readonly IVisitTechnicalSpecialistTravelRepository _repository = null;
        private readonly IVisitTechnicalSpecialistAccountItemTravelValidationService _techSpecAccountItemTravelValidationService = null;
        private readonly IVisitRepository _visitRepository = null;
        private readonly IMasterService _masterService = null;
        private readonly JObject _messages = null;
        private readonly IAuditSearchService _auditSearchService = null;

        public VisitTechnicalSpecialistTravelService(IAppLogger<VisitTechnicalSpecialistTravelService> logger,
                                                IVisitTechnicalSpecialistTravelRepository visitTravelRepository,
                                                IVisitTechnicalSpecialistAccountItemTravelValidationService validationService,
                                                IVisitRepository visitRepository,
                                                IMasterService masterService,
                                                IAuditSearchService auditSearchService,
                                                IMapper mapper,
                                                JObject messages)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = visitTravelRepository;
            _visitRepository = visitRepository;
            _masterService = masterService;
            _techSpecAccountItemTravelValidationService = validationService;
            this._messages = messages;
            this._auditSearchService = auditSearchService;
        }

        public Response Get(DomainModel.VisitSpecialistAccountItemTravel searchModel)
        {
            IList<DomainModel.VisitSpecialistAccountItemTravel> result = null;
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

        public async Task<Response> GetAsync(DomainModel.VisitSpecialistAccountItemTravel searchModel)
        {
            IList<DomainModel.VisitSpecialistAccountItemTravel> result = null;
            Exception exception = null;
            try
            {
                // result = await Task.Run(() => this._repository.Search(searchModel));
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                                   new TransactionOptions
                                                   {
                                                       IsolationLevel = IsolationLevel.ReadUncommitted
                                                   }, TransactionScopeAsyncFlowOption.Enabled))
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

        public Response Add(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
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
            IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel = null;
            if (visitId.HasValue)
                accountItemTravel?.ToList().ForEach(x => { x.VisitId = visitId; });

            return AddTSTravel(accountItemTravel,
                                         ref dbSpecialistAccountItemTravel,
                                         ref dbVisit,
                                         ref dbAssignment,
                                         ref dbProject,
                                         ref dbContract,
                                         ref dbExpenseType,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }


        public Response Add(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                            ref IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
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
                accountItemTravel?.ToList().ForEach(x => { x.VisitId = visitId; });

            return AddTSTravel(accountItemTravel,
                                         ref dbSpecialistAccountItemTravel,
                                         ref dbVisit,
                                         ref dbAssignment,
                                         ref dbProject,
                                         ref dbContract,
                                         ref dbExpenseType,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                  bool commitChange = true,
                                  bool isDbValidationRequired = true,
                                  long? visitId = null)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            if (visitId.HasValue)
                accountItemTravel?.ToList().ForEach(x => { x.VisitId = visitId; });

            return RemoveTSTravel(accountItemTravel,
                                     ref dbSpecialistAccountItemTravel,
                                     ref dbVisit,
                                     ref dbAssignment,
                                     ref dbProject,
                                     ref dbContract,
                                     ref dbExpenseType,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
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
                accountItemTravel?.ToList().ForEach(x => { x.VisitId = visitId; });
            return RemoveTSTravel(accountItemTravel,
                                                 ref dbSpecialistAccountItemTravel,
                                                 ref dbVisit,
                                                 ref dbAssignment,
                                                 ref dbProject,
                                                 ref dbContract,
                                                 ref dbExpenseType,
                                                 dbModule,
                                                 commitChange,
                                                 isDbValidationRequired);
        }


        public Response Modify(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
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
            IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel = null;
            if (visitId.HasValue)
                accountItemTravel?.ToList().ForEach(x => { x.VisitId = visitId; });

            return UpdateTSTravel(accountItemTravel,
                                     ref dbSpecialistAccountItemTravel,
                                     ref dbVisit,
                                     ref dbAssignment,
                                     ref dbProject,
                                     ref dbContract,
                                     ref dbExpenseType,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequired);
        }



        public Response Modify(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                    ref IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
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
                accountItemTravel?.ToList().ForEach(x => { x.VisitId = visitId; });

            return UpdateTSTravel(accountItemTravel,
                                                 ref dbSpecialistAccountItemTravel,
                                                 ref dbVisit,
                                                 ref dbAssignment,
                                                 ref dbProject,
                                                 ref dbContract,
                                                 ref dbExpenseType,
                                                 dbModule,
                                                 commitChange,
                                                 isDbValidationRequired);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                                ValidationType validationType)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel = null;
            return IsRecordValidForProcess(accountItemTravel,
                                            validationType,
                                             ref dbSpecialistAccountItemTravel,
                                             ref dbVisit,
                                             ref dbAssignment,
                                             ref dbProject,
                                             ref dbContract,
                                             ref dbExpenseType);

        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                                ValidationType validationType,
                                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType)
        {
            IList<DomainModel.VisitSpecialistAccountItemTravel> filteredAccountItemTravel = null;
            return IsRecordValidForProcess(accountItemTravel,
                                             validationType,
                                             ref filteredAccountItemTravel,
                                             ref dbSpecialistAccountItemTravel,
                                             ref dbVisit,
                                             ref dbAssignment,
                                             ref dbProject,
                                             ref dbContract,
                                             ref dbExpenseType);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                                ValidationType validationType,
                                                IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType)
        {
            return IsRecordValidForProcess(accountItemTravel,
                                          validationType,
                                           dbSpecialistAccountItemTravel,
                                           ref dbVisit,
                                           ref dbAssignment,
                                           ref dbProject,
                                           ref dbContract,
                                           ref dbExpenseType);
        }



        private Response AddTSTravel(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                    ref IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
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
                var recordToBeAdd = FilterRecord(accountItemTravel, ValidationType.Add);
                eventId = accountItemTravel?.FirstOrDefault()?.EventId;
                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(accountItemTravel,
                                                            ValidationType.Add,
                                                            ref recordToBeAdd,
                                                            ref dbSpecialistAccountItemTravel,
                                                            ref dbVisit,
                                                            ref dbAssignment,
                                                            ref dbProject,
                                                            ref dbContract,
                                                            ref dbExpenseType);
                else if (dbSpecialistAccountItemTravel?.Count <= 0)
                    dbSpecialistAccountItemTravel = GetVisitTSTravel(recordToBeAdd, ValidationType.Add);

                if (recordToBeAdd?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    _repository.AutoSave = false;
                    dbExpenseTypes = dbExpenseType;
                    dbProjects = dbProject;
                    dbContracts = dbContract;
                    dbSpecialistAccountItemTravel = _mapper.Map<IList<DbModel.VisitTechnicalSpecialistAccountItemTravel>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isVisitTSTravelId"] = false;
                        opt.Items["Project"] = dbProjects;
                        opt.Items["Contract"] = dbContracts;
                        opt.Items["ExpenseType"] = dbExpenseTypes;

                    });
                    _repository.Add(dbSpecialistAccountItemTravel);
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();

                        if (value > 0)
                        { 
                                dbSpecialistAccountItemTravel?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, accountItemTravel?.FirstOrDefault()?.ActionByUser,
                                                                                                    null,
                                                                                                    ValidationType.Add.ToAuditActionType(),
                                                                                                    SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTravel,
                                                                                                    null,
                                                                                                    _mapper.Map<DomainModel.VisitSpecialistAccountItemTravel>(x1)
                                                                                                    ,dbModule  ));
                        }

                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemTravel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateTSTravel(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                      ref IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
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
                var recordToBeModify = FilterRecord(accountItemTravel, ValidationType.Update);
                eventId = accountItemTravel?.FirstOrDefault()?.EventId;
                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(accountItemTravel,
                                                            ValidationType.Update,
                                                            ref recordToBeModify,
                                                            ref dbSpecialistAccountItemTravel,
                                                            ref dbVisit,
                                                            ref dbAssignment,
                                                            ref dbProject,
                                                            ref dbContract,
                                                            ref dbExpenseType);
                else if (dbSpecialistAccountItemTravel?.Count <= 0)
                    dbSpecialistAccountItemTravel = GetVisitTSTravel(recordToBeModify, ValidationType.Update);

                IList<DomainModel.VisitSpecialistAccountItemTravel> domExistingVisitSpecialistAccountItemTravel = new List<DomainModel.VisitSpecialistAccountItemTravel>();
                dbSpecialistAccountItemTravel?.ToList()?.ForEach(x =>
                {
                    domExistingVisitSpecialistAccountItemTravel.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.VisitSpecialistAccountItemTravel>(x)));
                });


                if (recordToBeModify?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    if (dbSpecialistAccountItemTravel?.Count > 0)
                    {
                        dbExpenseTypes = dbExpenseType;
                        dbProjects = dbProject;
                        dbContracts = dbContract;
                        dbSpecialistAccountItemTravel.ToList().ForEach(visitTS =>
                        {
                            var visitTSTravelToBeModify = recordToBeModify.FirstOrDefault(x => x.VisitTechnicalSpecialistAccountTravelId == visitTS.Id);
                            _mapper.Map(visitTSTravelToBeModify, visitTS, opt =>
                            {
                                opt.Items["isVisitTSTravelId"] = true;
                                opt.Items["Project"] = dbProjects;
                                opt.Items["Contract"] = dbContracts;
                                opt.Items["ExpenseType"] = dbExpenseTypes;
                            });
                            visitTS.LastModification = DateTime.UtcNow;
                            visitTS.UpdateCount = visitTSTravelToBeModify.UpdateCount.CalculateUpdateCount();
                            visitTS.ModifiedBy = visitTSTravelToBeModify.ModifiedBy;
                        });

                        _repository.AutoSave = false;
                        _repository.Update(dbSpecialistAccountItemTravel);
                        if (commitChange)
                        {
                            int value = _repository.ForceSave();
                            if (value > 0)
                            {
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                         null,
                                                                        ValidationType.Update.ToAuditActionType(),
                                                                        SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTravel,
                                                                        domExistingVisitSpecialistAccountItemTravel?.FirstOrDefault(x2 => x2.VisitTechnicalSpecialistAccountTravelId == x1.VisitTechnicalSpecialistAccountTravelId),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemTravel);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveTSTravel(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                      ref IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
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
                var recordToBeDelete = FilterRecord(accountItemTravel, ValidationType.Delete);
                eventId = accountItemTravel?.FirstOrDefault()?.EventId;


                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(accountItemTravel,
                                                       ValidationType.Delete,
                                                       ref recordToBeDelete,
                                                       ref dbSpecialistAccountItemTravel,
                                                       ref dbVisit,
                                                       ref dbAssignment,
                                                       ref dbProject,
                                                       ref dbContract,
                                                       ref dbExpenseType);

                if (recordToBeDelete?.Count > 0 && (response == null || response.Code == MessageType.Success.ToId()))
                {
                    if (dbSpecialistAccountItemTravel?.Count > 0)
                    {
                        _repository.AutoSave = false;
                        _repository.Delete(dbSpecialistAccountItemTravel);
                        if (commitChange)
                        {
                            int value = _repository.ForceSave();

                            if (value > 0)
                            {
                                dbSpecialistAccountItemTravel?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, accountItemTravel?.FirstOrDefault()?.ActionByUser,
                                                                                                        null,
                                                                                                        ValidationType.Delete.ToAuditActionType(),
                                                                                                        SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTravel,
                                                                                                        _mapper.Map<DomainModel.VisitSpecialistAccountItemTravel>(x1),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemTravel);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }


        private IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> GetVisitTSTravel(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                                                       ValidationType validationType)
        {
            IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel = null;
            if (validationType != ValidationType.Add)
            {
                if (accountItemTravel?.Count > 0)
                {
                    var visitTechnicalSpecialistAccountTravelId = accountItemTravel.Select(x => x.VisitTechnicalSpecialistAccountTravelId).Distinct().ToList();
                    dbSpecialistAccountItemTravel = _repository.FindBy(x => visitTechnicalSpecialistAccountTravelId.Contains(x.Id)).ToList();
                }
            }
            return dbSpecialistAccountItemTravel;
        }


        private IList<DomainModel.VisitSpecialistAccountItemTravel> FilterRecord(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                                                    ValidationType filterType)
        {
            IList<DomainModel.VisitSpecialistAccountItemTravel> filteredTSTime = null;

            if (filterType == ValidationType.Add)
                filteredTSTime = accountItemTravel?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredTSTime = accountItemTravel?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredTSTime = accountItemTravel?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredTSTime;
        }


        private bool IsValidPayload(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _techSpecAccountItemTravelValidationService.Validate(JsonConvert.SerializeObject(accountItemTravel), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Visit, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }


        private void FetchMasterData(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                     ref IList<DbModel.Data> masterData)
        {
            if (masterData == null)
            {
                IList<MasterType> types = new List<MasterType>() { MasterType.ExpenseType, MasterType.Currency };
                masterData = _masterService.Get(types);
            }
        }


        private bool IsValidExpenseType(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                         ref IList<DbModel.Data> dbExpenseType,
                                         IList<DbModel.Data> masterData,
                                         ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var dbExpense = masterData.Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType &&
                                                x.Type == ExpenseType.T.ToString()).ToList();

            var expenseTypeNotExists = accountItemTravel.Where(x => !string.IsNullOrEmpty(x.ChargeExpenseType))?.Where(x => !dbExpense.Any(x1 => x1.Name == x.ChargeExpenseType)).ToList();
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


        private bool IsValidCurrency(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
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

                var chargeNotExists = accountItemTravel.Where(x => !string.IsNullOrEmpty(x.ChargeRateCurrency))?.Where(x => !dbChargeCurrency.Any(x1 => x1.Code == x.ChargeRateCurrency)).ToList();
                var payNotExists = accountItemTravel.Where(x => !string.IsNullOrEmpty(x.PayRateCurrency))?.Where(x => !dbPayCurrency.Any(x1 => x1.Code == x.PayRateCurrency)).ToList();
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


        private bool IsValidContractProjectAssignmentVisit(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
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



        private bool IsRecordUpdateCountMatching(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                                IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = accountItemTravel.Where(x => !dbSpecialistAccountItemTravel.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() &&
                                                                                                             x1.Id == x.VisitTechnicalSpecialistAccountTravelId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x.VisitTechnicalSpecialistAccountTravelId, MessageType.VisitTravelUpdateCountMisMatch, x.VisitTechnicalSpecialistAccountTravelId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsVisitTravelExistInDb(IList<long> accountItemTravelIds,
                                                  IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                                                  ref IList<long> visitTravelIdNotExists,
                                                  ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbSpecialistAccountItemTravel == null)
                dbSpecialistAccountItemTravel = new List<DbModel.VisitTechnicalSpecialistAccountItemTravel>();

            var validMessages = validationMessages;

            if (accountItemTravelIds?.Count > 0)
            {
                visitTravelIdNotExists = accountItemTravelIds.Where(x => !dbSpecialistAccountItemTravel.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                visitTravelIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.VisitTravelInvalid, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidForAdd(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                        IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
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

            var visitIds = accountItemTravel.Where(x => x.VisitId > 0).Select(x => (long)x.VisitId).Distinct().ToList();
            var visitTechSpecIds = accountItemTravel.Where(x => x.VisitTechnicalSpecialistId > 0).Select(x => (long)x.VisitTechnicalSpecialistId).Distinct().ToList();
            if (dbExpenseType?.Count > 0)
                dbMasterData = dbExpenseType;
            else
                FetchMasterData(accountItemTravel, ref dbMasterData);

            if (IsValidVisit(visitIds, ref dbVisit, ref messages, new string[] {  "VisitTechnicalSpecialist",
                                                                                                                "Assignment",
                                                                                                                "Assignment.Project",
                                                                                                                "Assignment.Project.Contract"}))
                if (IsValidExpenseType(accountItemTravel, ref dbExpenseType, dbMasterData, ref messages))
                    if (IsValidCurrency(accountItemTravel, dbMasterData, ref messages))
                        if (IsVisitTechnicalSpecialistExistInDb(visitTechSpecIds, dbVisit.ToList().SelectMany(x => x.VisitTechnicalSpecialist).ToList(), ref notExists, ref messages))
                            IsValidContractProjectAssignmentVisit(accountItemTravel, dbVisit, ref messages);

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


        private bool IsRecordValidForUpdate(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                        IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
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

            var visitIds = accountItemTravel.Where(x => x.VisitId > 0).Select(x => (long)x.VisitId).Distinct().ToList();
            var visitTechSpecIds = accountItemTravel.Where(x => x.VisitTechnicalSpecialistId > 0).Select(x => (long)x.VisitTechnicalSpecialistId).Distinct().ToList();
            if (dbExpenseType?.Count > 0)
                dbMasterData = dbExpenseType;
            else
                FetchMasterData(accountItemTravel, ref dbMasterData);

            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(accountItemTravel, dbSpecialistAccountItemTravel, ref messages))
                    if (IsValidVisit(visitIds, ref dbVisit, ref messages, new string[] {  "VisitTechnicalSpecialist",
                                                                                                                        "Assignment",
                                                                                                                        "Assignment.Project",
                                                                                                                        "Assignment.Project.Contract"}))
                        if (IsValidExpenseType(accountItemTravel, ref dbExpenseType, dbMasterData, ref messages))
                            if (IsValidCurrency(accountItemTravel, dbMasterData, ref messages))
                                if (IsVisitTechnicalSpecialistExistInDb(visitTechSpecIds, dbVisit.ToList().SelectMany(x => x.VisitTechnicalSpecialist).ToList(), ref notExists, ref messages))
                                    IsValidContractProjectAssignmentVisit(accountItemTravel, dbVisit, ref messages);
            }

            dbAssignment = dbVisit?.ToList().Select(x => x.Assignment)?.ToList();
            dbProject = dbVisit?.ToList().Select(x => x.Assignment.Project)?.ToList();
            dbContract = dbVisit?.ToList().Select(x => x.Assignment.Project.Contract)?.ToList();


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemTravel> accountItemTravel,
                                                ValidationType validationType,
                                                ref IList<DomainModel.VisitSpecialistAccountItemTravel> filteredAccountItemTravel,
                                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
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
                if (accountItemTravel != null && accountItemTravel.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredAccountItemTravel == null || filteredAccountItemTravel.Count <= 0)
                        filteredAccountItemTravel = FilterRecord(accountItemTravel, validationType);

                    if (filteredAccountItemTravel != null && filteredAccountItemTravel?.Count > 0)
                    {
                        result = IsValidPayload(filteredAccountItemTravel,
                                                validationType,
                                                ref validationMessages);
                        if (filteredAccountItemTravel?.Count > 0 && result)
                        {
                            IList<long> moduleNotExists = null;
                            var visitTSTravelId = filteredAccountItemTravel.Where(x => x.VisitTechnicalSpecialistAccountTravelId.HasValue)
                                                                                         .Select(x => (long)x.VisitTechnicalSpecialistAccountTravelId).Distinct().ToList();

                            if (dbSpecialistAccountItemTravel == null || dbSpecialistAccountItemTravel.Count <= 0)
                                dbSpecialistAccountItemTravel = GetVisitTSTravel(filteredAccountItemTravel, validationType);
                            else
                            {
                                IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbItemTravel = GetVisitTSTravel(filteredAccountItemTravel, validationType);
                                if(dbItemTravel != null && dbItemTravel.Count > 0)
                                    dbSpecialistAccountItemTravel.AddRange(dbItemTravel);
                            }

                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                            {
                                result = IsVisitTravelExistInDb(visitTSTravelId,
                                                                      dbSpecialistAccountItemTravel,
                                                                      ref moduleNotExists,
                                                                      ref validationMessages);

                                if (result && validationType == ValidationType.Update)
                                    result = IsRecordValidForUpdate(filteredAccountItemTravel,
                                                                    dbSpecialistAccountItemTravel,
                                                                    ref dbVisit,
                                                                    ref dbAssignment,
                                                                    ref dbProject,
                                                                    ref dbContract,
                                                                    ref dbExpenseType,
                                                                    ref validationMessages);
                            }
                            else if (validationType == ValidationType.Add)
                                result = IsRecordValidForAdd(filteredAccountItemTravel,
                                                                 dbSpecialistAccountItemTravel,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemTravel);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
    }
}
