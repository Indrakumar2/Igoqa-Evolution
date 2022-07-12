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
using Evolution.Timesheet.Domain.Enum;
using Evolution.Timesheet.Domain.Interfaces.Data;
using Evolution.Timesheet.Domain.Interfaces.Timesheets;
using Evolution.Timesheet.Domain.Interfaces.Validations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;

namespace Evolution.Timesheet.Core.Services
{
    public class TechSpecAccountItemExpenseService : ITechSpecAccountItemExpenseService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<TechSpecAccountItemExpenseService> _logger = null;
        private readonly ITechSpecAccountItemExpenseRepository _repository = null;
        private readonly ITechSpecItemExpenseValidationService _techSpecAccountItemExpenseValidationService = null;
        private readonly ITimesheetRepository _timesheetRepository = null;
        private readonly IMasterService _masterService = null;
        private readonly JObject _messages = null;
        private readonly IAuditSearchService _auditSearchService = null;

        public TechSpecAccountItemExpenseService(IAppLogger<TechSpecAccountItemExpenseService> logger,
                                                 ITechSpecAccountItemExpenseRepository timesheetExpenceRepository,
                                                 ITechSpecItemExpenseValidationService techSpecAccountItemExpenseValidationService,
                                                 ITimesheetRepository timesheetRepository,
                                                 IMasterService masterService,
                                                 IMapper mapper,
                                                 JObject messages,
                                                 IAuditSearchService auditSearchService)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = timesheetExpenceRepository;
            _techSpecAccountItemExpenseValidationService = techSpecAccountItemExpenseValidationService;
            _timesheetRepository = timesheetRepository;
            _masterService = masterService;
            this._messages = messages;
            _auditSearchService = auditSearchService;
        }

        #region Public Methods
        public Response Get(DomainModel.TimesheetSpecialistAccountItemExpense searchModel)
        {
            IList<DomainModel.TimesheetSpecialistAccountItemExpense> result = null;
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

        public async Task<Response> GetAsync(DomainModel.TimesheetSpecialistAccountItemExpense searchModel)
        {
            IList<DomainModel.TimesheetSpecialistAccountItemExpense> result = null;
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

        public Response Add(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                          bool commitChange = true,
                          bool isDbValidationRequired = true,
                          long? timesheetId = null)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.SqlauditModule> dbModule=null;
           IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense = null;
            if (timesheetId.HasValue)
                accountItemExpense?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return AddTSTravel(accountItemExpense,
                                         ref dbSpecialistAccountItemExpense,
                                         ref dbTimesheet,
                                         ref dbAssignment,
                                         ref dbProject,
                                         ref dbContract,
                                         ref dbExpenseType,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }


        public Response Add(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                            ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
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
                accountItemExpense?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return AddTSTravel(accountItemExpense,
                                         ref dbSpecialistAccountItemExpense,
                                         ref dbTimesheet,
                                         ref dbAssignment,
                                         ref dbProject,
                                         ref dbContract,
                                         ref dbExpenseType,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
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
           IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense = null;
            if (timesheetId.HasValue)
                accountItemExpense?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return RemoveTSTravel(accountItemExpense,
                                     ref dbSpecialistAccountItemExpense,
                                     ref dbTimesheet,
                                     ref dbAssignment,
                                     ref dbProject,
                                     ref dbContract,
                                     ref dbExpenseType,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
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
                accountItemExpense?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });
            return RemoveTSTravel(accountItemExpense,
                                                 ref dbSpecialistAccountItemExpense,
                                                 ref dbTimesheet,
                                                 ref dbAssignment,
                                                 ref dbProject,
                                                 ref dbContract,
                                                 ref dbExpenseType,
                                                 dbModule,
                                                 commitChange,
                                                 isDbValidationRequired);
        }


        public Response Modify(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
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
           IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense = null;
            if (timesheetId.HasValue)
                accountItemExpense?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return UpdateTSTravel(accountItemExpense,
                                     ref dbSpecialistAccountItemExpense,
                                     ref dbTimesheet,
                                     ref dbAssignment,
                                     ref dbProject,
                                     ref dbContract,
                                     ref dbExpenseType,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequired);
        }



        public Response Modify(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                    ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
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
                accountItemExpense?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return UpdateTSTravel(accountItemExpense,
                                                 ref dbSpecialistAccountItemExpense,
                                                 ref dbTimesheet,
                                                 ref dbAssignment,
                                                 ref dbProject,
                                                 ref dbContract,
                                                 ref dbExpenseType,
                                                 dbModule,
                                                 commitChange,
                                                 isDbValidationRequired);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                                ValidationType validationType)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense = null;
            return IsRecordValidForProcess(accountItemExpense,
                                            validationType,
                                             ref dbSpecialistAccountItemExpense,
                                             ref dbTimesheet,
                                             ref dbAssignment,
                                             ref dbProject,
                                             ref dbContract,
                                             ref dbExpenseType);

        }

        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                                ValidationType validationType,
                                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType)
        {
            IList<DomainModel.TimesheetSpecialistAccountItemExpense> filteredAccountItemTravel = null;
            return IsRecordValidForProcess(accountItemExpense,
                                             validationType,
                                             ref filteredAccountItemTravel,
                                             ref dbSpecialistAccountItemExpense,
                                             ref dbTimesheet,
                                             ref dbAssignment,
                                             ref dbProject,
                                             ref dbContract,
                                             ref dbExpenseType);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                                ValidationType validationType,
                                                IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType)
        {
            return IsRecordValidForProcess(accountItemExpense,
                                          validationType,
                                           dbSpecialistAccountItemExpense,
                                           ref dbTimesheet,
                                           ref dbAssignment,
                                           ref dbProject,
                                           ref dbContract,
                                           ref dbExpenseType);
        }
        #endregion

        #region Private methods
        private Response AddTSTravel(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                    ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
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
            long? eventId = null;
            try
            {
                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(accountItemExpense, ValidationType.Add);
                eventId = accountItemExpense?.FirstOrDefault().EventId;

                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(accountItemExpense,
                                                            ValidationType.Add,
                                                            ref recordToBeAdd,
                                                            ref dbSpecialistAccountItemExpense,
                                                            ref dbTimesheet,
                                                            ref dbAssignment,
                                                            ref dbProject,
                                                            ref dbContract,
                                                            ref dbExpenseType);
                else if (dbSpecialistAccountItemExpense?.Count <= 0)
                    dbSpecialistAccountItemExpense = GetTimesheetTSExpense(recordToBeAdd, ValidationType.Add);

                if (recordToBeAdd?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    _repository.AutoSave = false;
                    dbExpenseTypes = dbExpenseType;
                    dbProjects = dbProject;
                    dbContracts = dbContract;
                    dbSpecialistAccountItemExpense = _mapper.Map<IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isTimesheetTSExpenseId"] = false;
                        opt.Items["Project"] = dbProjects;
                        opt.Items["Contract"] = dbContracts;
                        opt.Items["ExpenseType"] = dbExpenseTypes;
                    });

                    _repository.Add(dbSpecialistAccountItemExpense);
                    if (commitChange)
                    {
                       int value= _repository.ForceSave();
                        if (value > 0)
                        {
                            dbSpecialistAccountItemExpense?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, accountItemExpense.FirstOrDefault().ActionByUser,
                                                                                                     null,
                                                                                                     ValidationType.Add.ToAuditActionType(),
                                                                                                     SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemExpense,
                                                                                                     null,
                                                                                                     _mapper.Map<DomainModel.TimesheetSpecialistAccountItemExpense>(x1)
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

        private Response UpdateTSTravel(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                      ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
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
            IList<DomainModel.TimesheetSpecialistAccountItemExpense> result = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(accountItemExpense, ValidationType.Update);
                eventId = accountItemExpense.FirstOrDefault().EventId;
                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(accountItemExpense,
                                                            ValidationType.Update,
                                                            ref recordToBeModify,
                                                            ref dbSpecialistAccountItemExpense,
                                                            ref dbTimesheet,
                                                            ref dbAssignment,
                                                            ref dbProject,
                                                            ref dbContract,
                                                            ref dbExpenseType);
                else if (dbSpecialistAccountItemExpense?.Count <= 0)
                    dbSpecialistAccountItemExpense = GetTimesheetTSExpense(recordToBeModify, ValidationType.Update);

                if (recordToBeModify?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    if (dbSpecialistAccountItemExpense?.Count > 0)
                    {
                        dbExpenseTypes = dbExpenseType;
                        dbProjects = dbProject;
                        dbContracts = dbContract;

                        IList<DomainModel.TimesheetSpecialistAccountItemExpense> domExistingTimeSheetAccountExpense = new List<DomainModel.TimesheetSpecialistAccountItemExpense>();
                        dbSpecialistAccountItemExpense?.ToList()?.ForEach(x =>
                        {
                            domExistingTimeSheetAccountExpense.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.TimesheetSpecialistAccountItemExpense>(x)));
                        });
                        dbSpecialistAccountItemExpense.ToList().ForEach(timesheetTS =>
                        {
                            var timesheetTSExpenseToBeModify = recordToBeModify.FirstOrDefault(x => x.TimesheetTechnicalSpecialistAccountExpenseId == timesheetTS.Id);
                            if(timesheetTSExpenseToBeModify != null){
                            _mapper.Map(timesheetTSExpenseToBeModify, timesheetTS, opt =>
                            {
                                opt.Items["isTimesheetTSExpenseId"] = true;
                                opt.Items["Project"] = dbProjects;
                                opt.Items["Contract"] = dbContracts;
                                opt.Items["ExpenseType"] = dbExpenseTypes;
                            });
                            timesheetTS.LastModification = DateTime.UtcNow;
                            timesheetTS.UpdateCount = timesheetTSExpenseToBeModify.UpdateCount.CalculateUpdateCount();
                            timesheetTS.ModifiedBy = timesheetTSExpenseToBeModify.ModifiedBy;
                            }
                        });

                        _repository.AutoSave = false;
                        _repository.Update(dbSpecialistAccountItemExpense);
                        if (commitChange)
                        {
                          int value=  _repository.ForceSave();
                            if (value > 0)
                            {
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                        null,
                                                                       ValidationType.Update.ToAuditActionType(),
                                                                       SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemExpense,
                                                                       domExistingTimeSheetAccountExpense?.FirstOrDefault(x2 => x2.TimesheetTechnicalSpecialistAccountExpenseId == x1.TimesheetTechnicalSpecialistAccountExpenseId),
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

        private Response RemoveTSTravel(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                      ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
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
                var recordToBeDelete = FilterRecord(accountItemExpense, ValidationType.Delete);
                eventId = accountItemExpense.FirstOrDefault().EventId;

                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(accountItemExpense,
                                                       ValidationType.Delete,
                                                       ref recordToBeDelete,
                                                       ref dbSpecialistAccountItemExpense,
                                                       ref dbTimesheet,
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
                             int value =_repository.ForceSave();
                            if (value > 0)
                            {
                                dbSpecialistAccountItemExpense?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, accountItemExpense?.FirstOrDefault()?.ActionByUser,
                                                                                                            null,
                                                                                                            ValidationType.Delete.ToAuditActionType(),
                                                                                                            SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemExpense,
                                                                                                            _mapper.Map<DomainModel.TimesheetSpecialistAccountItemExpense>(x1),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemExpense);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }


        private IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> GetTimesheetTSExpense(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemTravel,
                                                                       ValidationType validationType)
        {
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense = null;
            if (validationType != ValidationType.Add)
            {
                if (accountItemTravel?.Count > 0)
                {
                    var timesheetTechnicalSpecialistAccountExpenseId = accountItemTravel.Select(x => x.TimesheetTechnicalSpecialistAccountExpenseId).Distinct().ToList();
                    dbSpecialistAccountItemExpense = _repository.FindBy(x => timesheetTechnicalSpecialistAccountExpenseId.Contains(x.Id)).ToList();
                }
            }
            return dbSpecialistAccountItemExpense;
        }


        private IList<DomainModel.TimesheetSpecialistAccountItemExpense> FilterRecord(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemTravel,
                                                                    ValidationType filterType)
        {
            IList<DomainModel.TimesheetSpecialistAccountItemExpense> filteredTSExpense = null;

            if (filterType == ValidationType.Add)
                filteredTSExpense = accountItemTravel?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredTSExpense = accountItemTravel?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredTSExpense = accountItemTravel?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredTSExpense;
        }


        private bool IsValidPayload(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemTravel,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _techSpecAccountItemExpenseValidationService.Validate(JsonConvert.SerializeObject(accountItemTravel), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Timesheet, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }


        private void FetchMasterData(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                     ref IList<DbModel.Data> masterData)
        {
            if (masterData == null)
            {
                IList<MasterType> types = new List<MasterType>() { MasterType.ExpenseType, MasterType.Currency };
                masterData = _masterService.Get(types);
            }
        }


        private bool IsValidExpenseType(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
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
                messages.Add(_messages, x.ChargeExpenseType, MessageType.TimesheetTSExpenseTypeInvalid, x.ChargeExpenseType);
            });

            if (dbExpenseType?.Count == 0 || dbExpenseType == null)
                dbExpenseType = dbExpense;

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsValidCurrency(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                         IList<DbModel.Data> masterData,
                                         ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (masterData != null)
            {
                var chargeCureency = accountItemExpense.Where(x => !string.IsNullOrEmpty(x.ChargeRateCurrency))?.Select(x => x.ChargeRateCurrency).ToList();
                var payCurrency = accountItemExpense.Where(x=>!string.IsNullOrEmpty(x.PayRateCurrency))?.Select(x => x.PayRateCurrency).ToList();
                if (chargeCureency?.Count>0  && payCurrency?.Count>0)
                {
                    var dbChargeCurrency = masterData.Where(x => x.MasterDataTypeId == (int)MasterType.Currency).ToList();
                    var dbPayCurrency = masterData.Where(x => x.MasterDataTypeId == (int)MasterType.Currency).ToList();

                    var chargeNotExists = accountItemExpense.Where(x => !string.IsNullOrEmpty(x.ChargeRateCurrency))?.Where(x => !dbChargeCurrency.Any(x1 => x1.Code == x.ChargeRateCurrency)).ToList();
                    var payNotExists = accountItemExpense.Where(x => !string.IsNullOrEmpty(x.PayRateCurrency))?.Where(x => !dbPayCurrency.Any(x1 => x1.Code == x.PayRateCurrency)).ToList();
                    chargeNotExists.ToList().ForEach(x =>
                    {
                        messages.Add(_messages, x.ChargeRateCurrency, MessageType.TimesheetTSChargeCurrencyInvalid, x.ChargeRateCurrency);
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

        private bool IsUniqueTimesheetTechSpecialistExpense(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                                            IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                                                            ValidationType validationType,
                                                            ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> expenseExists = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            expenseExists = _repository.IsUniqueTimesheetTechSpecialistExpense(accountItemExpense, dbSpecialistAccountItemExpense, validationType);
            expenseExists?.ToList().ForEach(x =>
            {
                messages.Add(_messages, x.TimesheetTechnicalSpeciallistId, MessageType.TimesheetTSDuplicateRecord, x.TimesheetTechnicalSpeciallistId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count > 0;
        }


        private bool IsValidContractProjectAssignmentTimesheet(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
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
                    messages.Add(_messages, null, MessageType.TimesheetTSTimesheetContractInvalid);

                if (messages.Count > 0)
                    validationMessages.AddRange(messages);
            }

            return messages?.Count <= 0;
        }



        private bool IsRecordUpdateCountMatching(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                                IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = accountItemExpense.Where(x => !dbSpecialistAccountItemExpense.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() &&
                                                                                                             x1.Id == x.TimesheetTechnicalSpecialistAccountExpenseId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x.TimesheetTechnicalSpecialistAccountExpenseId, MessageType.TimesheetExpenseUpdateCountMisMatch, x.TimesheetTechnicalSpecialistAccountExpenseId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsTimesheetExpenseExistInDb(IList<long> accountItemExpenseIds,
                                                  IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
                                                  ref IList<long> timesheetExpenseIdNotExists,
                                                  ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbSpecialistAccountItemExpense == null)
                dbSpecialistAccountItemExpense = new List<DbModel.TimesheetTechnicalSpecialistAccountItemExpense>();

            var validMessages = validationMessages;

            if (accountItemExpenseIds?.Count > 0)
            {
                timesheetExpenseIdNotExists = accountItemExpenseIds.Where(x => !dbSpecialistAccountItemExpense.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                timesheetExpenseIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TimesheetExpenseInvalid, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidForAdd(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                        IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
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

            var timesheetIds = accountItemExpense.Where(x => x.TimesheetId > 0).Select(x => (long)x.TimesheetId).Distinct().ToList();
            var timesheetTechSpecIds = accountItemExpense.Where(x => x.TimesheetTechnicalSpecialistId > 0).Select(x => (long)x.TimesheetTechnicalSpecialistId).Distinct().ToList();
            FetchMasterData(accountItemExpense, ref dbMasterData);

            if (IsValidTimesheet(timesheetIds, ref dbTimesheet, ref messages, new string[] {  "TimesheetTechnicalSpecialist",
                                                                                                                "Assignment",
                                                                                                                "Assignment.Project",
                                                                                                                "Assignment.Project.Contract"}))
                if (IsValidExpenseType(accountItemExpense, ref dbExpenseType, dbMasterData, ref messages))
                    if (IsValidCurrency(accountItemExpense, dbMasterData, ref messages))
                        if (IsTimesheetTechnicalSpecialistExistInDb(timesheetTechSpecIds, dbTimesheet.ToList().SelectMany(x => x.TimesheetTechnicalSpecialist).ToList(), ref notExists, ref messages))
                            IsValidContractProjectAssignmentTimesheet(accountItemExpense, dbTimesheet, ref messages);
            //IsUniqueTimesheetTechSpecialistExpense(accountItemExpense, dbSpecialistAccountItemExpense, ValidationType.Add, ref validationMessages);

            dbAssignment = dbTimesheet?.ToList().Select(x => x.Assignment)?.ToList();
            dbProject = dbTimesheet?.ToList().Select(x => x.Assignment.Project)?.ToList();
            dbContract = dbTimesheet?.ToList().Select(x => x.Assignment.Project.Contract)?.ToList();


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsRecordValidForUpdate(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                        IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
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

            var timesheetIds = accountItemExpense.Where(x => x.TimesheetId > 0).Select(x => (long)x.TimesheetId).Distinct().ToList();
            var timesheetTechSpecIds = accountItemExpense.Where(x => x.TimesheetTechnicalSpecialistId > 0).Select(x => (long)x.TimesheetTechnicalSpecialistId).Distinct().ToList();
            FetchMasterData(accountItemExpense, ref dbMasterData);
            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(accountItemExpense, dbSpecialistAccountItemExpense, ref messages))
                    if (IsValidTimesheet(timesheetIds, ref dbTimesheet, ref messages, new string[] {  "TimesheetTechnicalSpecialist",
                                                                                                                        "Assignment",
                                                                                                                        "Assignment.Project",
                                                                                                                        "Assignment.Project.Contract"}))
                        if (IsValidExpenseType(accountItemExpense, ref dbExpenseType, dbMasterData, ref messages))
                            if (IsValidCurrency(accountItemExpense, dbMasterData, ref messages))
                                if (IsTimesheetTechnicalSpecialistExistInDb(timesheetTechSpecIds, dbTimesheet.ToList().SelectMany(x => x.TimesheetTechnicalSpecialist).ToList(), ref notExists, ref messages))
                                    IsValidContractProjectAssignmentTimesheet(accountItemExpense, dbTimesheet, ref messages);
                //IsUniqueTimesheetTechSpecialistExpense(accountItemExpense, dbSpecialistAccountItemExpense, ValidationType.Update, ref validationMessages);
            }

            dbAssignment = dbTimesheet?.ToList().Select(x => x.Assignment)?.ToList();
            dbProject = dbTimesheet?.ToList().Select(x => x.Assignment.Project)?.ToList();
            dbContract = dbTimesheet?.ToList().Select(x => x.Assignment.Project.Contract)?.ToList();


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemExpense> accountItemExpense,
                                                ValidationType validationType,
                                                ref IList<DomainModel.TimesheetSpecialistAccountItemExpense> filteredAccountItemExpense,
                                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbSpecialistAccountItemExpense,
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
                            var timesheetTSExpenseId = filteredAccountItemExpense.Where(x => x.TimesheetTechnicalSpecialistAccountExpenseId.HasValue)
                                                                                         .Select(x => (long)x.TimesheetTechnicalSpecialistAccountExpenseId).Distinct().ToList();

                            if (dbSpecialistAccountItemExpense == null || dbSpecialistAccountItemExpense.Count <= 0)
                                dbSpecialistAccountItemExpense = GetTimesheetTSExpense(filteredAccountItemExpense, validationType);

                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                            {
                                result = IsTimesheetExpenseExistInDb(timesheetTSExpenseId,
                                                                      dbSpecialistAccountItemExpense,
                                                                      ref moduleNotExists,
                                                                      ref validationMessages);

                                if (result && validationType == ValidationType.Update)
                                    result = IsRecordValidForUpdate(filteredAccountItemExpense,
                                                                    dbSpecialistAccountItemExpense,
                                                                    ref dbTimesheet,
                                                                    ref dbAssignment,
                                                                    ref dbProject,
                                                                    ref dbContract,
                                                                    ref dbExpenseType,
                                                                    ref validationMessages);
                            }
                            else if (validationType == ValidationType.Add)
                                result = IsRecordValidForAdd(filteredAccountItemExpense,
                                                                 dbSpecialistAccountItemExpense,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemExpense);
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
