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
    public class TechSpecAccountItemTimeService : ITechSpecAccountItemTimeService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<TechSpecAccountItemTimeService> _logger = null;
        private readonly ITechSpecAccountItemTimeRepository _repository = null;
        private readonly ITechSpecItemTimeValidationService _techSpecAccountItemTimeValidationService = null;
        private readonly ITimesheetRepository _timesheetRepository = null;
        private readonly IMasterService _masterService = null;
        private readonly JObject _messages = null;
        private readonly IAuditSearchService _auditSearchService = null;

        public TechSpecAccountItemTimeService(IAppLogger<TechSpecAccountItemTimeService> logger,
                                              ITechSpecAccountItemTimeRepository timesheetTimeRepository,
                                              ITechSpecItemTimeValidationService validationService,
                                              ITimesheetRepository timesheetRepository,
                                              IMasterService masterService,
                                              IMapper mapper,
                                              JObject messages,
                                              IAuditSearchService auditSearchService
                                             )
        {
            _mapper = mapper;
            _logger = logger;
            _repository = timesheetTimeRepository;
            _timesheetRepository = timesheetRepository;
            _masterService = masterService;
            _techSpecAccountItemTimeValidationService = validationService;
            this._messages = messages;
            _auditSearchService = auditSearchService;
        }

        #region Public Methods
        public Response Get(DomainModel.TimesheetSpecialistAccountItemTime searchModel)
        {
            IList<DomainModel.TimesheetSpecialistAccountItemTime> result = null;
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

        public async Task<Response> GetAsync(DomainModel.TimesheetSpecialistAccountItemTime searchModel)
        {
            IList<DomainModel.TimesheetSpecialistAccountItemTime> result = null;
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

        public Response Add(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
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
           IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes = null;
            if (timesheetId.HasValue)
                accountItemTimes?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return AddTSTime(accountItemTimes,
                                         ref dbSpecialistAccountItemTimes,
                                         ref dbTimesheet,
                                         ref dbAssignment,
                                         ref dbProject,
                                         ref dbContract,
                                         ref dbExpenseType,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }


        public Response Add(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                            ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
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
                accountItemTimes?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return AddTSTime(accountItemTimes,
                                         ref dbSpecialistAccountItemTimes,
                                         ref dbTimesheet,
                                         ref dbAssignment,
                                         ref dbProject,
                                         ref dbContract,
                                         ref dbExpenseType,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
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
           IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes = null;
            if (timesheetId.HasValue)
                accountItemTimes?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return this.RemoveTSTime(accountItemTimes,
                                     ref dbSpecialistAccountItemTimes,
                                     ref dbTimesheet,
                                     ref dbAssignment,
                                     ref dbProject,
                                     ref dbContract,
                                     ref dbExpenseType,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
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
                accountItemTimes?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });
            return this.RemoveTSTime(accountItemTimes,
                                                 ref dbSpecialistAccountItemTimes,
                                                 ref dbTimesheet,
                                                 ref dbAssignment,
                                                 ref dbProject,
                                                 ref dbContract,
                                                 ref dbExpenseType,
                                                 dbModule,
                                                 commitChange,
                                                 isDbValidationRequired);
        }


        public Response Modify(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
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
           IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes = null;
            if (timesheetId.HasValue)
                accountItemTimes?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return UpdateTSTime(accountItemTimes,
                                     ref dbSpecialistAccountItemTimes,
                                     ref dbTimesheet,
                                     ref dbAssignment,
                                     ref dbProject,
                                     ref dbContract,
                                     ref dbExpenseType,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequired);
        }



        public Response Modify(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                    ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
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
                accountItemTimes?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return UpdateTSTime(accountItemTimes,
                                                 ref dbSpecialistAccountItemTimes,
                                                 ref dbTimesheet,
                                                 ref dbAssignment,
                                                 ref dbProject,
                                                 ref dbContract,
                                                 ref dbExpenseType,
                                                 dbModule,
                                                 commitChange,
                                                 isDbValidationRequired);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                                ValidationType validationType)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes = null;
            return IsRecordValidForProcess(accountItemTimes,
                                            validationType,
                                             ref dbSpecialistAccountItemTimes,
                                             ref dbTimesheet,
                                             ref dbAssignment,
                                             ref dbProject,
                                             ref dbContract,
                                             ref dbExpenseType);

        }

        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                                ValidationType validationType,
                                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType)
        {
            IList<DomainModel.TimesheetSpecialistAccountItemTime> filteredAccountItemTimes = null;
            return IsRecordValidForProcess(accountItemTimes,
                                             validationType,
                                             ref filteredAccountItemTimes,
                                             ref dbSpecialistAccountItemTimes,
                                             ref dbTimesheet,
                                             ref dbAssignment,
                                             ref dbProject,
                                             ref dbContract,
                                             ref dbExpenseType);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                                ValidationType validationType,
                                                IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType)
        {
            return IsRecordValidForProcess(accountItemTimes,
                                          validationType,
                                           dbSpecialistAccountItemTimes,
                                           ref dbTimesheet,
                                           ref dbAssignment,
                                           ref dbProject,
                                           ref dbContract,
                                           ref dbExpenseType);
        }

        #endregion

        #region Private Methods
        private Response AddTSTime(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                    ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
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
                var recordToBeAdd = FilterRecord(accountItemTimes, ValidationType.Add);
                eventId = accountItemTimes.FirstOrDefault().EventId;

                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(accountItemTimes,
                                                            ValidationType.Add,
                                                            ref recordToBeAdd,
                                                            ref dbSpecialistAccountItemTimes,
                                                            ref dbTimesheet,
                                                            ref dbAssignment,
                                                            ref dbProject,
                                                            ref dbContract,
                                                            ref dbExpenseType);
                else if (dbSpecialistAccountItemTimes?.Count <= 0)
                    dbSpecialistAccountItemTimes = GetTimesheetTSTime(recordToBeAdd, ValidationType.Add);

                if (recordToBeAdd?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    _repository.AutoSave = false;
                    dbExpenseTypes = dbExpenseType;
                    dbProjects = dbProject;
                    dbContracts = dbContract;
                    dbSpecialistAccountItemTimes = _mapper.Map<IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isTimesheetTSTimeId"] = false;
                        opt.Items["Project"] = dbProjects;
                        opt.Items["Contract"] = dbContracts;
                        opt.Items["ExpenseType"] = dbExpenseTypes;
                    });

                    _repository.Add(dbSpecialistAccountItemTimes);
                    if (commitChange)
                    {
                       int value = _repository.ForceSave();
                        if (value > 0)
                        {
                            dbSpecialistAccountItemTimes?.ToList().ForEach(x1 => _auditSearchService.AuditLog(recordToBeAdd.FirstOrDefault(), ref eventId, accountItemTimes?.FirstOrDefault()?.ActionByUser,
                                                                                                     null,
                                                                                                     ValidationType.Add.ToAuditActionType(),
                                                                                                     SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemTime,
                                                                                                     null,
                                                                                                     _mapper.Map<DomainModel.TimesheetSpecialistAccountItemTime>(x1)
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemTimes);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateTSTime(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                      ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
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
            IList<DomainModel.TimesheetSpecialistAccountItemTime> result = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(accountItemTimes, ValidationType.Update);
                eventId = accountItemTimes.FirstOrDefault().EventId;
                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(accountItemTimes,
                                                            ValidationType.Update,
                                                            ref recordToBeModify,
                                                            ref dbSpecialistAccountItemTimes,
                                                            ref dbTimesheet,
                                                            ref dbAssignment,
                                                            ref dbProject,
                                                            ref dbContract,
                                                            ref dbExpenseType);
                else if (dbSpecialistAccountItemTimes?.Count <= 0)
                    dbSpecialistAccountItemTimes = GetTimesheetTSTime(recordToBeModify, ValidationType.Update);

                if (recordToBeModify?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    if (dbSpecialistAccountItemTimes?.Count > 0)
                    {
                        dbExpenseTypes = dbExpenseType;
                        dbProjects = dbProject;
                        dbContracts = dbContract;
                        IList<DomainModel.TimesheetSpecialistAccountItemTime> domExistingTimesheetSpecialistAccountItem = new List<DomainModel.TimesheetSpecialistAccountItemTime>();
                        dbSpecialistAccountItemTimes?.ToList()?.ForEach(x =>
                        {
                            domExistingTimesheetSpecialistAccountItem.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.TimesheetSpecialistAccountItemTime>(x)));
                        });

                        dbSpecialistAccountItemTimes.ToList().ForEach(timesheetTS =>
                        {
                            var timesheetTSTimeToBeModify = recordToBeModify.FirstOrDefault(x => x.TimesheetTechnicalSpecialistAccountTimeId == timesheetTS.Id);
                            if(timesheetTSTimeToBeModify != null){
                            _mapper.Map(timesheetTSTimeToBeModify, timesheetTS, opt =>
                            {
                                opt.Items["isTimesheetTSTimeId"] = true;
                                opt.Items["Project"] = dbProjects;
                                opt.Items["Contract"] = dbContracts;
                                opt.Items["ExpenseType"] = dbExpenseTypes;
                            });
                            timesheetTS.LastModification = DateTime.UtcNow;
                            timesheetTS.UpdateCount = timesheetTSTimeToBeModify.UpdateCount.CalculateUpdateCount();
                            timesheetTS.ModifiedBy = timesheetTSTimeToBeModify.ModifiedBy;
                            }
                        });

                        _repository.AutoSave = false;
                        _repository.Update(dbSpecialistAccountItemTimes);
                        if (commitChange)
                        {
                           int value= _repository.ForceSave();
                            if (value > 0)
                            {
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                        null,
                                                                       ValidationType.Update.ToAuditActionType(),
                                                                        SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemTime,
                                                                       domExistingTimesheetSpecialistAccountItem?.FirstOrDefault(x2 => x2.TimesheetTechnicalSpecialistAccountTimeId == x1.TimesheetTechnicalSpecialistAccountTimeId),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemTimes);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveTSTime(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                      ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
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
                var recordToBeDelete = FilterRecord(accountItemTimes, ValidationType.Delete);
                eventId = accountItemTimes?.FirstOrDefault()?.EventId;

                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(accountItemTimes,
                                                       ValidationType.Delete,
                                                       ref recordToBeDelete,
                                                       ref dbSpecialistAccountItemTimes,
                                                       ref dbTimesheet,
                                                       ref dbAssignment,
                                                       ref dbProject,
                                                       ref dbContract,
                                                       ref dbExpenseType);

                if (recordToBeDelete?.Count > 0 && (response == null || response.Code == MessageType.Success.ToId()))
                {
                    if (dbSpecialistAccountItemTimes?.Count > 0)
                    {
                        _repository.AutoSave = false;
                        _repository.Delete(dbSpecialistAccountItemTimes);
                        if (commitChange)
                        {
                           int value= _repository.ForceSave();
                            dbSpecialistAccountItemTimes?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, accountItemTimes?.FirstOrDefault()?.ActionByUser, null,
                                                                                                  ValidationType.Delete.ToAuditActionType(),
                                                                                                  SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemTime,
                                                                                                  _mapper.Map<DomainModel.TimesheetSpecialistAccountItemTime>(x1), null,dbModule));
                         }
                    }
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemTimes);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }


        private IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> GetTimesheetTSTime(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                                                       ValidationType validationType)
        {
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes = null;
            if (validationType != ValidationType.Add)
            {
                if (accountItemTimes?.Count > 0)
                {
                    var timesheetTechnicalSpecialistAccountTimeId = accountItemTimes.Select(x => x.TimesheetTechnicalSpecialistAccountTimeId).Distinct().ToList();
                    dbSpecialistAccountItemTimes = _repository.FindBy(x => timesheetTechnicalSpecialistAccountTimeId.Contains(x.Id)).ToList();
                }
            }
            return dbSpecialistAccountItemTimes;
        }


        private IList<DomainModel.TimesheetSpecialistAccountItemTime> FilterRecord(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                                                    ValidationType filterType)
        {
            IList<DomainModel.TimesheetSpecialistAccountItemTime> filteredTSTime = null;

            if (filterType == ValidationType.Add)
                filteredTSTime = accountItemTimes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredTSTime = accountItemTimes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredTSTime = accountItemTimes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredTSTime;
        }


        private bool IsValidPayload(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _techSpecAccountItemTimeValidationService.Validate(JsonConvert.SerializeObject(accountItemTimes), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Timesheet, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }


        private void FetchMasterData(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                     ref IList<DbModel.Data> masterData)
        {
            if (masterData == null)
            {
                IList<MasterType> types = new List<MasterType>() { MasterType.ExpenseType, MasterType.Currency };
                masterData = _masterService.Get(types);
            }
        }


        private bool IsValidExpenseType(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                         ref IList<DbModel.Data> dbExpenseType,
                                         IList<DbModel.Data> masterData,
                                         ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var dbExpense = masterData.Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType &&
                                                x.Type == ExpenseType.R.ToString()).ToList();

            var expenseTypeNotExists = accountItemTimes.Where(x => !string.IsNullOrEmpty(x.ChargeExpenseType))?.Where(x => !dbExpense.Any(x1 => x1.Name == x.ChargeExpenseType)).ToList();
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


        private bool IsValidCurrency(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                         IList<DbModel.Data> masterData,
                                         ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (masterData != null)
            {
                var chargeCureency = accountItemTimes.Where(x => !string.IsNullOrEmpty(x.ChargeRateCurrency))?.Select(x => x.ChargeRateCurrency).ToList();
                var payCurrency = accountItemTimes.Where(x => !string.IsNullOrEmpty(x.PayRateCurrency))?.Select(x => x.PayRateCurrency).ToList();
                if (chargeCureency?.Count > 0 && payCurrency?.Count > 0)
                {
                    var dbChargeCurrency = masterData.Where(x => x.MasterDataTypeId == (int)MasterType.Currency).ToList();
                    var dbPayCurrency = masterData.Where(x => x.MasterDataTypeId == (int)MasterType.Currency).ToList();

                    var chargeNotExists = accountItemTimes.Where(x => !string.IsNullOrEmpty(x.ChargeRateCurrency))?.Where(x => !dbChargeCurrency.Any(x1 => x1.Code == x.ChargeRateCurrency)).ToList();
                    var payNotExists = accountItemTimes.Where(x => !string.IsNullOrEmpty(x.PayRateCurrency))?.Where(x => !dbPayCurrency.Any(x1 => x1.Code == x.PayRateCurrency)).ToList();
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

        private bool IsUniqueTimesheetTechSpecialistTimes(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                                IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                                                ValidationType validationType,
                                                ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> timeExists = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            timeExists = _repository.IsUniqueTimesheetTSTime(accountItemTimes, dbSpecialistAccountItemTimes, validationType);
            timeExists?.ToList().ForEach(x =>
            {
                messages.Add(_messages, x.TimesheetTechnicalSpeciallistId, MessageType.TimesheetTSDuplicateRecord, x.TimesheetTechnicalSpeciallistId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count > 0;
        }

        private bool IsValidContractProjectAssignmentTimesheet(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
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



        private bool IsRecordUpdateCountMatching(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                                IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = accountItemTimes.Where(x => !dbSpecialistAccountItemTimes.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() &&
                                                                                                             x1.Id == x.TimesheetTechnicalSpecialistAccountTimeId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x.TimesheetTechnicalSpecialistAccountTimeId, MessageType.TimesheetTSTimeUpdateCountMisMatch, x.TimesheetTechnicalSpecialistAccountTimeId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsTimesheetTimeExistInDb(IList<long> accountItemTimeIds,
                                                  IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                                                  ref IList<long> timesheetTimeIdNotExists,
                                                  ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbSpecialistAccountItemTimes == null)
                dbSpecialistAccountItemTimes = new List<DbModel.TimesheetTechnicalSpecialistAccountItemTime>();

            var validMessages = validationMessages;

            if (accountItemTimeIds?.Count > 0)
            {
                timesheetTimeIdNotExists = accountItemTimeIds.Where(x => !dbSpecialistAccountItemTimes.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                timesheetTimeIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TimesheetTimeInvalid, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidForAdd(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                        IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
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

            var timesheetIds = accountItemTimes.Where(x => x.TimesheetId > 0).Select(x => (long)x.TimesheetId).Distinct().ToList();
            var timesheetTechSpecIds = accountItemTimes.Where(x => x.TimesheetTechnicalSpecialistId > 0).Select(x => (long)x.TimesheetTechnicalSpecialistId).Distinct().ToList();
            FetchMasterData(accountItemTimes, ref dbMasterData);

            if (IsValidTimesheet(timesheetIds, ref dbTimesheet, ref messages, new string[] {  "TimesheetTechnicalSpecialist",
                                                                                                                "Assignment",
                                                                                                                "Assignment.Project",
                                                                                                                "Assignment.Project.Contract"}))
                if (IsValidExpenseType(accountItemTimes, ref dbExpenseType, dbMasterData, ref messages))
                    if (IsValidCurrency(accountItemTimes, dbMasterData, ref messages))
                        if (IsTimesheetTechnicalSpecialistExistInDb(timesheetTechSpecIds, dbTimesheet.ToList().SelectMany(x => x.TimesheetTechnicalSpecialist).ToList(), ref notExists, ref messages))
                            IsValidContractProjectAssignmentTimesheet(accountItemTimes, dbTimesheet, ref messages);
            //IsUniqueTimesheetTechSpecialistTimes(accountItemTimes, dbSpecialistAccountItemTimes, ValidationType.Add, ref validationMessages);

            dbAssignment = dbTimesheet?.ToList().Select(x => x.Assignment)?.ToList();
            dbProject = dbTimesheet?.ToList().Select(x => x.Assignment.Project)?.ToList();
            dbContract = dbTimesheet?.ToList().Select(x => x.Assignment.Project.Contract)?.ToList();


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsRecordValidForUpdate(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                        IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
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

            var timesheetIds = accountItemTimes.Where(x => x.TimesheetId > 0).Select(x => (long)x.TimesheetId).Distinct().ToList();
            var timesheetTechSpecIds = accountItemTimes.Where(x => x.TimesheetTechnicalSpecialistId > 0).Select(x => (long)x.TimesheetTechnicalSpecialistId).Distinct().ToList();
            FetchMasterData(accountItemTimes, ref dbMasterData);
            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(accountItemTimes, dbSpecialistAccountItemTimes, ref messages))
                    if (IsValidTimesheet(timesheetIds, ref dbTimesheet, ref messages, new string[] {  "TimesheetTechnicalSpecialist",
                                                                                                                        "Assignment",
                                                                                                                        "Assignment.Project",
                                                                                                                        "Assignment.Project.Contract"}))
                        if (IsValidExpenseType(accountItemTimes, ref dbExpenseType, dbMasterData, ref messages))
                            if (IsValidCurrency(accountItemTimes, dbMasterData, ref messages))
                                if (IsTimesheetTechnicalSpecialistExistInDb(timesheetTechSpecIds, dbTimesheet.ToList().SelectMany(x => x.TimesheetTechnicalSpecialist).ToList(), ref notExists, ref messages))
                                    IsValidContractProjectAssignmentTimesheet(accountItemTimes, dbTimesheet, ref messages);
                // IsUniqueTimesheetTechSpecialistTimes(accountItemTimes, dbSpecialistAccountItemTimes, ValidationType.Update, ref validationMessages);
            }

            dbAssignment = dbTimesheet?.ToList().Select(x => x.Assignment)?.ToList();
            dbProject = dbTimesheet?.ToList().Select(x => x.Assignment.Project)?.ToList();
            dbContract = dbTimesheet?.ToList().Select(x => x.Assignment.Project.Contract)?.ToList();


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemTime> accountItemTimes,
                                                ValidationType validationType,
                                                ref IList<DomainModel.TimesheetSpecialistAccountItemTime> filteredAccountItemTimes,
                                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
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
                if (accountItemTimes != null && accountItemTimes.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredAccountItemTimes == null || filteredAccountItemTimes.Count <= 0)
                        filteredAccountItemTimes = FilterRecord(accountItemTimes, validationType);

                    if (filteredAccountItemTimes != null && filteredAccountItemTimes?.Count > 0)
                    {
                        result = IsValidPayload(filteredAccountItemTimes,
                                                validationType,
                                                ref validationMessages);
                        if (filteredAccountItemTimes?.Count > 0 && result)
                        {
                            IList<long> moduleNotExists = null;
                            var timesheetTSTimeId = filteredAccountItemTimes.Where(x => x.TimesheetTechnicalSpecialistAccountTimeId.HasValue)
                                                                                         .Select(x => (long)x.TimesheetTechnicalSpecialistAccountTimeId).Distinct().ToList();

                            if (dbSpecialistAccountItemTimes == null || dbSpecialistAccountItemTimes.Count <= 0)
                                dbSpecialistAccountItemTimes = GetTimesheetTSTime(filteredAccountItemTimes, validationType);

                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                            {
                                result = IsTimesheetTimeExistInDb(timesheetTSTimeId,
                                                                      dbSpecialistAccountItemTimes,
                                                                      ref moduleNotExists,
                                                                      ref validationMessages);

                                if (result && validationType == ValidationType.Update)
                                    result = IsRecordValidForUpdate(filteredAccountItemTimes,
                                                                    dbSpecialistAccountItemTimes,
                                                                    ref dbTimesheet,
                                                                    ref dbAssignment,
                                                                    ref dbProject,
                                                                    ref dbContract,
                                                                    ref dbExpenseType,
                                                                    ref validationMessages);
                            }
                            else if (validationType == ValidationType.Add)
                                result = IsRecordValidForAdd(filteredAccountItemTimes,
                                                                 dbSpecialistAccountItemTimes,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemTimes);
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
