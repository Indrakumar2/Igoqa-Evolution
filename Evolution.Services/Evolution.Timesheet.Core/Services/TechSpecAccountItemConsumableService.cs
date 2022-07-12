using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Timesheet.Domain.Interfaces.Data;
using Evolution.Timesheet.Domain.Interfaces.Timesheets;
using Evolution.Timesheet.Domain.Interfaces.Validations;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Linq;
using Evolution.Common.Models.Messages;
using Newtonsoft.Json;
using Evolution.Timesheet.Domain.Enum;
using System.Threading.Tasks;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Extensions;
using System.Transactions;

namespace Evolution.Timesheet.Core.Services
{
    public class TechSpecAccountItemConsumableService : ITechSpecAccountItemConsumableService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<TechSpecAccountItemConsumableService> _logger = null;
        private readonly ITechSpecAccountItemConsumableRepository _repository = null;
        private readonly ITechSpecItemConsumableValidationService _techSpecAccountItemConsumablesValidationService = null;
        private readonly ITimesheetRepository _timesheetRepository = null;
        private readonly IMasterService _masterService = null;
        private readonly JObject _messages = null;
        private readonly IAuditSearchService _auditSearchService = null;

        public TechSpecAccountItemConsumableService(IAppLogger<TechSpecAccountItemConsumableService> logger,
                                                    ITechSpecAccountItemConsumableRepository timesheetConsumableRepository,
                                                    ITechSpecItemConsumableValidationService techSpecAccountItemConsumablesValidationService,
                                                    ITimesheetRepository timesheetRepository,
                                                    IMasterService masterService,
                                                    IMapper mapper,
                                                    JObject messages,
                                                   IAuditSearchService auditSearchService)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = timesheetConsumableRepository;
            _techSpecAccountItemConsumablesValidationService = techSpecAccountItemConsumablesValidationService;
            _timesheetRepository = timesheetRepository;
            _masterService = masterService;
            this._messages = messages;
            this._auditSearchService = auditSearchService;
        }

        #region Public Methods
        public Response Get(DomainModel.TimesheetSpecialistAccountItemConsumable searchModel)
        {
            IList<DomainModel.TimesheetSpecialistAccountItemConsumable> result = null;
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

        public async Task<Response> GetAsync(DomainModel.TimesheetSpecialistAccountItemConsumable searchModel)
        {
            IList<DomainModel.TimesheetSpecialistAccountItemConsumable> result = null;
            Exception exception = null;
            try
            {
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

        public Response Add(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                          bool commitChange = true,
                          bool isDbValidationRequired = true,
                          long? timesheetId = null)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            if (timesheetId.HasValue)
                accountItemConsumables?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return AddTSConsumable(accountItemConsumables,
                                         ref dbSpecialistAccountItemConsumable,
                                         ref dbTimesheet,
                                         ref dbAssignment,
                                         ref dbProject,
                                         ref dbContract,
                                         ref dbExpenseType,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }


        public Response Add(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                            ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                            ref IList<DbModel.Timesheet> dbTimesheet,
                            ref IList<DbModel.Assignment> dbAssignment,
                            ref IList<DbModel.Project> dbProject,
                            ref IList<DbModel.Contract> dbContract,
                            ref IList<DbModel.Data> dbExpenseType,
                            IList<DbModel.SqlauditModule> dbModule,
                            bool commitChange = true,
                            bool isDbValidationRequired = true,
                            long? timesheetId = null)
        {
            if (timesheetId.HasValue)
                accountItemConsumables?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return AddTSConsumable(accountItemConsumables,
                                         ref dbSpecialistAccountItemConsumable,
                                         ref dbTimesheet,
                                         ref dbAssignment,
                                         ref dbProject,
                                         ref dbContract,
                                         ref dbExpenseType,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                  bool commitChange = true,
                                  bool isDbValidationRequired = true,
                                  long? timesheetId = null)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.SqlauditModule> dbModule = null;
           IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable = null;
            if (timesheetId.HasValue)
                accountItemConsumables?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return RemoveTSConsumable(accountItemConsumables,
                                     ref dbSpecialistAccountItemConsumable,
                                     ref dbTimesheet,
                                     ref dbAssignment,
                                     ref dbProject,
                                     ref dbContract,
                                     ref dbExpenseType,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                ref IList<DbModel.Timesheet> dbTimesheet,
                                ref IList<DbModel.Assignment> dbAssignment,
                                ref IList<DbModel.Project> dbProject,
                                ref IList<DbModel.Contract> dbContract,
                                ref IList<DbModel.Data> dbExpenseType,
                                IList<DbModel.SqlauditModule> dbModule,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                long? timesheetId = null)
        {
            if (timesheetId.HasValue)
                accountItemConsumables?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });
            return RemoveTSConsumable(accountItemConsumables,
                                                 ref dbSpecialistAccountItemConsumable,
                                                 ref dbTimesheet,
                                                 ref dbAssignment,
                                                 ref dbProject,
                                                 ref dbContract,
                                                 ref dbExpenseType,
                                                 dbModule,
                                                 commitChange,
                                                 isDbValidationRequired);
        }


        public Response Modify(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                              bool commitChange = true,
                              bool isDbValidationRequired = true,
                              long? timesheetId = null)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.SqlauditModule> dbModule = null;
           IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable = null;
            if (timesheetId.HasValue)
                accountItemConsumables?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return UpdateTSConsumable(accountItemConsumables,
                                     ref dbSpecialistAccountItemConsumable,
                                     ref dbTimesheet,
                                     ref dbAssignment,
                                     ref dbProject,
                                     ref dbContract,
                                     ref dbExpenseType,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequired);
        }



        public Response Modify(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                    ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                    ref IList<DbModel.Timesheet> dbTimesheet,
                                    ref IList<DbModel.Assignment> dbAssignment,
                                    ref IList<DbModel.Project> dbProject,
                                    ref IList<DbModel.Contract> dbContract,
                                    ref IList<DbModel.Data> dbExpenseType,
                                    IList<DbModel.SqlauditModule> dbModule,
                                    bool commitChange = true,
                                    bool isDbValidationRequired = true,
                                    long? timesheetId = null)
        {
            if (timesheetId.HasValue)
                accountItemConsumables?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return UpdateTSConsumable(accountItemConsumables,
                                                 ref dbSpecialistAccountItemConsumable,
                                                 ref dbTimesheet,
                                                 ref dbAssignment,
                                                 ref dbProject,
                                                 ref dbContract,
                                                 ref dbExpenseType,
                                                 dbModule,
                                                 commitChange,
                                                 isDbValidationRequired);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                                ValidationType validationType)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable = null;
            return IsRecordValidForProcess(accountItemConsumables,
                                            validationType,
                                             ref dbSpecialistAccountItemConsumable,
                                             ref dbTimesheet,
                                             ref dbAssignment,
                                             ref dbProject,
                                             ref dbContract,
                                             ref dbExpenseType);

        }

        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                                ValidationType validationType,
                                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType)
        {
            IList<DomainModel.TimesheetSpecialistAccountItemConsumable> filteredAccountItemConsumable = null;
            return IsRecordValidForProcess(accountItemConsumables,
                                             validationType,
                                             ref filteredAccountItemConsumable,
                                             ref dbSpecialistAccountItemConsumable,
                                             ref dbTimesheet,
                                             ref dbAssignment,
                                             ref dbProject,
                                             ref dbContract,
                                             ref dbExpenseType);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                                ValidationType validationType,
                                                 IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType)
        {
            return IsRecordValidForProcess(accountItemConsumables,
                                          validationType,
                                           dbSpecialistAccountItemConsumable,
                                           ref dbTimesheet,
                                           ref dbAssignment,
                                           ref dbProject,
                                           ref dbContract,
                                           ref dbExpenseType);
        }
        #endregion

        #region Private Methods
        private Response AddTSConsumable(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                    ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                    ref IList<DbModel.Timesheet> dbTimesheet,
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
                eventId = accountItemConsumables?.FirstOrDefault().EventId;
                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(accountItemConsumables,
                                                            ValidationType.Add,
                                                            ref recordToBeAdd,
                                                            ref dbSpecialistAccountItemConsumable,
                                                            ref dbTimesheet,
                                                            ref dbAssignment,
                                                            ref dbProject,
                                                            ref dbContract,
                                                            ref dbExpenseType);
                else if (dbSpecialistAccountItemConsumable?.Count <= 0)
                    dbSpecialistAccountItemConsumable = GetTimesheetTSConsumable(recordToBeAdd, ValidationType.Add);

                if (recordToBeAdd?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    _repository.AutoSave = false;
                    dbExpenseTypes = dbExpenseType;
                    dbProjects = dbProject;
                    dbContracts = dbContract;

                    dbSpecialistAccountItemConsumable = _mapper.Map<IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isTimesheetTSConsumableId"] = false;
                        opt.Items["Project"] = dbProjects;
                        opt.Items["Contract"] = dbContracts;
                        opt.Items["ExpenseType"] = dbExpenseTypes;
                    });

                    _repository.Add(dbSpecialistAccountItemConsumable);
                    if (commitChange)
                    {
                       int value = _repository.ForceSave();
                        if (value > 0)
                        {
                            dbSpecialistAccountItemConsumable?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, accountItemConsumables.FirstOrDefault().ActionByUser,
                                                                                                     null,
                                                                                                     ValidationType.Add.ToAuditActionType(),
                                                                                                     SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemConsumable,
                                                                                                     null,
                                                                                                     _mapper.Map<DomainModel.TimesheetSpecialistAccountItemConsumable>(x1)
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemConsumables);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }


        private Response UpdateTSConsumable(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                      ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                      ref IList<DbModel.Timesheet> dbTimesheet,
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
            IList<DomainModel.TimesheetSpecialistAccountItemTravel> result = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(accountItemConsumables, ValidationType.Update);

                eventId = accountItemConsumables.FirstOrDefault().EventId;
                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(accountItemConsumables,
                                                            ValidationType.Update,
                                                            ref recordToBeModify,
                                                            ref dbSpecialistAccountItemConsumable,
                                                            ref dbTimesheet,
                                                            ref dbAssignment,
                                                            ref dbProject,
                                                            ref dbContract,
                                                            ref dbExpenseType);
                else if (dbSpecialistAccountItemConsumable?.Count <= 0)
                    dbSpecialistAccountItemConsumable = GetTimesheetTSConsumable(recordToBeModify, ValidationType.Update);

                if (recordToBeModify?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    if (dbSpecialistAccountItemConsumable?.Count > 0)
                    {
                        dbExpenseTypes = dbExpenseType;
                        dbProjects = dbProject;
                        dbContracts = dbContract;
                        IList<DomainModel.TimesheetSpecialistAccountItemConsumable> domExistingTimeSheetAccItemConsumable = new List<DomainModel.TimesheetSpecialistAccountItemConsumable>();
                        dbSpecialistAccountItemConsumable?.ToList()?.ForEach(x =>
                        {
                            domExistingTimeSheetAccItemConsumable.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.TimesheetSpecialistAccountItemConsumable>(x)));
                        });
                        dbSpecialistAccountItemConsumable.ToList().ForEach(timesheetTS =>
                        {
                            var timesheetTSConsumableToBeModify = recordToBeModify.FirstOrDefault(x => x.TimesheetTechnicalSpecialistAccountConsumableId == timesheetTS.Id);
                            if(timesheetTSConsumableToBeModify != null){
                            _mapper.Map(timesheetTSConsumableToBeModify, timesheetTS, opt =>
                            {
                                opt.Items["isTimesheetTSConsumableId"] = true;
                                opt.Items["Project"] = dbProjects;
                                opt.Items["Contract"] = dbContracts;
                                opt.Items["ExpenseType"] = dbExpenseTypes;
                            });
                            timesheetTS.LastModification = DateTime.UtcNow;
                            timesheetTS.UpdateCount = timesheetTSConsumableToBeModify.UpdateCount.CalculateUpdateCount();
                            timesheetTS.ModifiedBy = timesheetTSConsumableToBeModify.ModifiedBy;
                            }
                        });

                        _repository.AutoSave = false;
                        _repository.Update(dbSpecialistAccountItemConsumable);
                        if (commitChange)
                        {
                            int value=_repository.ForceSave();
                            if (value > 0)
                            {
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                        null,
                                                                        ValidationType.Update.ToAuditActionType(),
                                                                        SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTravel,
                                                                        domExistingTimeSheetAccItemConsumable?.FirstOrDefault(x2 => x2.TimesheetTechnicalSpecialistAccountConsumableId == x1.TimesheetTechnicalSpecialistAccountConsumableId),
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

        private Response RemoveTSConsumable(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                      ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                      ref IList<DbModel.Timesheet> dbTimesheet,
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
                eventId = accountItemConsumables.FirstOrDefault().EventId;


                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(accountItemConsumables,
                                                       ValidationType.Delete,
                                                       ref recordToBeDelete,
                                                       ref dbSpecialistAccountItemConsumable,
                                                       ref dbTimesheet,
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
                           int value= _repository.ForceSave();
                            if (value > 0)
                            {
                                dbSpecialistAccountItemConsumable?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, accountItemConsumables?.FirstOrDefault()?.ActionByUser,
                                                                                                            null,
                                                                                                            ValidationType.Delete.ToAuditActionType(),
                                                                                                            SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemConsumable,
                                                                                                            _mapper.Map<DomainModel.TimesheetSpecialistAccountItemConsumable>(x1),
                                                                                                            null,dbModule
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemConsumables);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }


        private IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> GetTimesheetTSConsumable(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                                                       ValidationType validationType)
        {
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable = null;
            if (validationType != ValidationType.Add)
            {
                if (accountItemConsumables?.Count > 0)
                {
                    var timesheetTechnicalSpecialistAccountConsumableId = accountItemConsumables.Select(x => x.TimesheetTechnicalSpecialistAccountConsumableId).Distinct().ToList();
                    dbSpecialistAccountItemConsumable = _repository.FindBy(x => timesheetTechnicalSpecialistAccountConsumableId.Contains(x.Id)).ToList();
                }
            }
            return dbSpecialistAccountItemConsumable;
        }


        private IList<DomainModel.TimesheetSpecialistAccountItemConsumable> FilterRecord(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                                                    ValidationType filterType)
        {
            IList<DomainModel.TimesheetSpecialistAccountItemConsumable> filteredTSConsumable = null;

            if (filterType == ValidationType.Add)
                filteredTSConsumable = accountItemConsumables?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredTSConsumable = accountItemConsumables?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredTSConsumable = accountItemConsumables?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredTSConsumable;
        }


        private bool IsValidPayload(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _techSpecAccountItemConsumablesValidationService.Validate(JsonConvert.SerializeObject(accountItemConsumables), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Timesheet, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }


        private void FetchMasterData(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                     ref IList<DbModel.Data> masterData)
        {
            if (masterData == null)
            {
                IList<MasterType> types = new List<MasterType>() { MasterType.ExpenseType, MasterType.Currency };
                masterData = _masterService.Get(types);
            }
        }


        private bool IsValidExpenseType(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
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
                messages.Add(_messages, x.ChargeExpenseType, MessageType.TimesheetTSExpenseTypeInvalid, x.ChargeExpenseType);
            });

            if (dbExpenseType?.Count == 0 || dbExpenseType == null)
                dbExpenseType = dbExpense;

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsValidCurrency(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                         IList<DbModel.Data> masterData,
                                         ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (masterData != null)
            {
                var chargeCureency = accountItemConsumables.Where(x => !string.IsNullOrEmpty(x.ChargeRateCurrency))?.Select(x => x.ChargeRateCurrency).ToList();
                var payCurrency = accountItemConsumables.Where(x => !string.IsNullOrEmpty(x.PayRateCurrency))?.Select(x => x.PayRateCurrency).ToList();
                if (chargeCureency?.Count > 0 && payCurrency?.Count > 0)
                {
                    var dbChargeCurrency = masterData.Where(x => x.MasterDataTypeId == (int)MasterType.Currency).ToList();
                    var dbPayCurrency = masterData.Where(x => x.MasterDataTypeId == (int)MasterType.Currency).ToList();

                    var chargeNotExists = accountItemConsumables.Where(x=>!string.IsNullOrEmpty(x.ChargeRateCurrency))?.Where(x => !dbChargeCurrency.Any(x1 => x1.Code == x.ChargeRateCurrency)).ToList();
                    var payNotExists = accountItemConsumables.Where(x => !string.IsNullOrEmpty(x.PayRateCurrency))?.Where(x => !dbPayCurrency.Any(x1 => x1.Code == x.PayRateCurrency)).ToList();
                    chargeNotExists.ToList().ForEach(x =>
                    {
                        messages.Add(_messages, x.ChargeExpenseType, MessageType.TimesheetTSChargeCurrencyInvalid, x.ChargeExpenseType);
                    });

                    payNotExists.ToList().ForEach(x =>
                    {
                        messages.Add(_messages, x.PayRateCurrency, MessageType.TimesheetTSPayCurrencyInvalid, x.PayRateCurrency);
                    });
                }
            }


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsValidContractProjectAssignmentTimesheet(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                                               IList<DbModel.Timesheet> dbTimesheet,
                                                               ref IList<ValidationMessage> validationMessages)
        {

            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTimesheet != null)
            {
                var dbContract = dbTimesheet?.ToList()?.Select(x => x.Assignment?.Project?.Contract).ToList();
                if (dbContract == null || dbContract.Count == 0)
                    messages.Add(_messages, null, MessageType.TimesheetTSTimesheetContractInvalid, null);

                if (messages.Count > 0)
                    validationMessages.AddRange(messages);
            }

            return messages?.Count <= 0;
        }

        private bool IsUniqueTimesheetTechSpecialistConsumables(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                                 IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                                 ValidationType validationType,
                                                 ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> consumablesNotExists = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            consumablesNotExists = _repository.IsUniqueTimesheetTSConsumables(accountItemConsumables, dbSpecialistAccountItemConsumable, validationType);
            consumablesNotExists?.ToList().ForEach(x =>
            {
                messages.Add(_messages, x.TimesheetTechnicalSpecialistId, MessageType.TimesheetTSDuplicateRecord, x.TimesheetTechnicalSpecialistId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count > 0;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                                 IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = accountItemConsumables.Where(x => !dbSpecialistAccountItemConsumable.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() &&
                                                                                                             x1.Id == x.TimesheetTechnicalSpecialistAccountConsumableId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x.TimesheetTechnicalSpecialistAccountConsumableId, MessageType.TimesheetConsumableUpdateCountMisMatch, x.TimesheetTechnicalSpecialistAccountConsumableId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsTimesheetConsumablesExistInDb(IList<long> accountItemTravelIds,
                                                   IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                                  ref IList<long> timesheetTravelIdNotExists,
                                                  ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbSpecialistAccountItemConsumable == null)
                dbSpecialistAccountItemConsumable = new List<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable>();

            var validMessages = validationMessages;

            if (accountItemTravelIds?.Count > 0)
            {
                timesheetTravelIdNotExists = accountItemTravelIds.Where(x => !dbSpecialistAccountItemConsumable.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                timesheetTravelIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TimesheetTravelInvalid, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidForAdd(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                        IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                        ref IList<DbModel.Timesheet> dbTimesheet,
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

            if (dbExpenseType != null)
                dbMasterData = dbExpenseType;

            var timesheetIds = accountItemConsumables.Where(x => x.TimesheetId > 0).Select(x => (long)x.TimesheetId).Distinct().ToList();
            var timesheetTechSpecIds = accountItemConsumables.Where(x => x.TimesheetTechnicalSpecialistId > 0).Select(x => (long)x.TimesheetTechnicalSpecialistId).Distinct().ToList();
            
            FetchMasterData(accountItemConsumables, ref dbMasterData);

            if (IsValidTimesheet(timesheetIds, ref dbTimesheet, ref messages, new string[] {  "TimesheetTechnicalSpecialist",
                                                                                                                "Assignment",
                                                                                                                "Assignment.Project",
                                                                                                                "Assignment.Project.Contract"}))
                if (IsValidExpenseType(accountItemConsumables, ref dbExpenseType, dbMasterData, ref messages))
                    if (IsValidCurrency(accountItemConsumables, dbMasterData, ref messages))
                        if (IsTimesheetTechnicalSpecialistExistInDb(timesheetTechSpecIds, dbTimesheet.ToList().SelectMany(x => x.TimesheetTechnicalSpecialist).ToList(), ref notExists, ref messages))
                            IsValidContractProjectAssignmentTimesheet(accountItemConsumables, dbTimesheet, ref messages);
            //IsUniqueTimesheetTechSpecialistConsumables(accountItemConsumables, dbSpecialistAccountItemConsumable, ValidationType.Add, ref validationMessages);

            dbAssignment = dbTimesheet?.ToList().Select(x => x.Assignment)?.ToList();
            dbProject = dbTimesheet?.ToList().Select(x => x.Assignment.Project)?.ToList();
            dbContract = dbTimesheet?.ToList().Select(x => x.Assignment.Project.Contract)?.ToList();


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsRecordValidForUpdate(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                         IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                        ref IList<DbModel.Timesheet> dbTimesheet,
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
            if (dbExpenseType != null)
                dbMasterData = dbExpenseType;

            var timesheetIds = accountItemConsumables.Where(x => x.TimesheetId > 0).Select(x => (long)x.TimesheetId).Distinct().ToList();
            var timesheetTechSpecIds = accountItemConsumables.Where(x => x.TimesheetTechnicalSpecialistId > 0).Select(x => (long)x.TimesheetTechnicalSpecialistId).Distinct().ToList();

            FetchMasterData(accountItemConsumables, ref dbMasterData);
            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(accountItemConsumables, dbSpecialistAccountItemConsumable, ref messages))
                    if (IsValidTimesheet(timesheetIds, ref dbTimesheet, ref messages, new string[] {  "TimesheetTechnicalSpecialist",
                                                                                                                        "Assignment",
                                                                                                                        "Assignment.Project",
                                                                                                                        "Assignment.Project.Contract"}))
                        if (IsValidExpenseType(accountItemConsumables, ref dbExpenseType, dbMasterData, ref messages))
                            if (IsValidCurrency(accountItemConsumables, dbMasterData, ref messages))
                                if (IsTimesheetTechnicalSpecialistExistInDb(timesheetTechSpecIds, dbTimesheet.ToList().SelectMany(x => x.TimesheetTechnicalSpecialist).ToList(), ref notExists, ref messages))
                                    IsValidContractProjectAssignmentTimesheet(accountItemConsumables, dbTimesheet, ref messages);
                //IsUniqueTimesheetTechSpecialistConsumables(accountItemConsumables, dbSpecialistAccountItemConsumable, ValidationType.Update, ref validationMessages);
            }

            dbAssignment = dbTimesheet?.ToList().Select(x => x.Assignment)?.ToList();
            dbProject = dbTimesheet?.ToList().Select(x => x.Assignment.Project)?.ToList();
            dbContract = dbTimesheet?.ToList().Select(x => x.Assignment.Project.Contract)?.ToList();


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> accountItemConsumables,
                                                ValidationType validationType,
                                                ref IList<DomainModel.TimesheetSpecialistAccountItemConsumable> filteredAccountItemConsumables,
                                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbSpecialistAccountItemConsumable,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
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
                            var timesheetTSConsumableId = filteredAccountItemConsumables.Where(x => x.TimesheetTechnicalSpecialistAccountConsumableId.HasValue)
                                                                                         .Select(x => (long)x.TimesheetTechnicalSpecialistAccountConsumableId).Distinct().ToList();

                            if (dbSpecialistAccountItemConsumable == null || dbSpecialistAccountItemConsumable.Count <= 0)
                                dbSpecialistAccountItemConsumable = GetTimesheetTSConsumable(filteredAccountItemConsumables, validationType);

                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                            {
                                result = IsTimesheetConsumablesExistInDb(timesheetTSConsumableId,
                                                                      dbSpecialistAccountItemConsumable,
                                                                      ref moduleNotExists,
                                                                      ref validationMessages);

                                if (result && validationType == ValidationType.Update)
                                    result = IsRecordValidForUpdate(filteredAccountItemConsumables,
                                                                    dbSpecialistAccountItemConsumable,
                                                                    ref dbTimesheet,
                                                                    ref dbAssignment,
                                                                    ref dbProject,
                                                                    ref dbContract,
                                                                    ref dbExpenseType,
                                                                    ref validationMessages);
                            }
                            else if (validationType == ValidationType.Add)
                                result = IsRecordValidForAdd(filteredAccountItemConsumables,
                                                                 dbSpecialistAccountItemConsumable,
                                                                 ref dbTimesheet,
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

        private bool IsValidTimesheet(IList<long> timesheetId,
                                    ref IList<DbModel.Timesheet> dbTimesheets,
                                    ref IList<ValidationMessage> messages,
                                    params string[] includes)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (dbTimesheets == null)
            {

                var resultTimesheets = _timesheetRepository.FetchTimesheets(timesheetId, includes);
                var timesheetNotExists = timesheetId?.Where(x => !resultTimesheets.Any(x2 => x2.Id == x))?.ToList();
                timesheetNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.TimesheetNotExists.ToId();
                    message.Add(_messages, x, MessageType.TimesheetNotExists, x);
                });
                dbTimesheets = resultTimesheets;
                messages = message;
            }

            return messages?.Count <= 0;
        }

        private bool IsTimesheetTechnicalSpecialistExistInDb(IList<long> timesheetTechnicalSpecialistIds,
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

        #endregion
    }
}
