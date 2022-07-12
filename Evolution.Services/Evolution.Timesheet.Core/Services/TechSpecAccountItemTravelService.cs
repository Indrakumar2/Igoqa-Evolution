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
    public class TechSpecAccountItemTravelService : ITechSpecAccountItemTravelService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<TechSpecAccountItemTravelService> _logger = null;
        private readonly ITechSpecAccountItemTravelRepository _repository = null;
        private readonly ITechSpecItemTravelValidationService _techSpecAccountItemTravelValidationService = null;
        private readonly ITimesheetRepository _timesheetRepository = null;
        private readonly IMasterService _masterService = null;
        private readonly JObject _messages = null;
         private readonly IAuditSearchService _auditSearchService = null;

        public TechSpecAccountItemTravelService(IAppLogger<TechSpecAccountItemTravelService> logger,
                                                ITechSpecAccountItemTravelRepository timesheetTravelRepository,
                                                ITechSpecItemTravelValidationService techSpecAccountItemTravelValidationService,
                                                ITimesheetRepository timesheetRepository,
                                                IMasterService masterService,
                                                IMapper mapper,
                                                JObject messages,
                                               IAuditSearchService auditSearchService)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = timesheetTravelRepository;
            _techSpecAccountItemTravelValidationService = techSpecAccountItemTravelValidationService;
            _masterService = masterService;
            _timesheetRepository = timesheetRepository;
            this._messages = messages;
            _auditSearchService = auditSearchService;
        }

        #region Public Methods
        public Response Get(DomainModel.TimesheetSpecialistAccountItemTravel searchModel)
        {
            IList<DomainModel.TimesheetSpecialistAccountItemTravel> result = null;
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

        public async Task<Response> GetAsync(DomainModel.TimesheetSpecialistAccountItemTravel searchModel)
        {
            IList<DomainModel.TimesheetSpecialistAccountItemTravel> result = null;
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

        public Response Add(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
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
           IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel = null;
            if (timesheetId.HasValue)
                accountItemTravel?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return AddTSTravel(accountItemTravel,
                                         ref dbSpecialistAccountItemTravel,
                                         ref dbTimesheet,
                                         ref dbAssignment,
                                         ref dbProject,
                                         ref dbContract,
                                         ref dbExpenseType,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }


        public Response Add(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                            ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
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
                accountItemTravel?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return AddTSTravel(accountItemTravel,
                                         ref dbSpecialistAccountItemTravel,
                                         ref dbTimesheet,
                                         ref dbAssignment,
                                         ref dbProject,
                                         ref dbContract,
                                         ref dbExpenseType,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                  bool commitChange = true,
                                  bool isDbValidationRequired = true,
                                  long? timesheetId = null)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            if (timesheetId.HasValue)
                accountItemTravel?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return RemoveTSTravel(accountItemTravel,
                                     ref dbSpecialistAccountItemTravel,
                                     ref dbTimesheet,
                                     ref dbAssignment,
                                     ref dbProject,
                                     ref dbContract,
                                     ref dbExpenseType,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
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
                accountItemTravel?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });
            return RemoveTSTravel(accountItemTravel,
                                                 ref dbSpecialistAccountItemTravel,
                                                 ref dbTimesheet,
                                                 ref dbAssignment,
                                                 ref dbProject,
                                                 ref dbContract,
                                                 ref dbExpenseType,
                                                 dbModule,
                                                 commitChange,
                                                 isDbValidationRequired);
        }


        public Response Modify(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
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
           IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel = null;
            if (timesheetId.HasValue)
                accountItemTravel?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return UpdateTSTravel(accountItemTravel,
                                     ref dbSpecialistAccountItemTravel,
                                     ref dbTimesheet,
                                     ref dbAssignment,
                                     ref dbProject,
                                     ref dbContract,
                                     ref dbExpenseType,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequired);
        }



        public Response Modify(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                    ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
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
                accountItemTravel?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });

            return UpdateTSTravel(accountItemTravel,
                                                 ref dbSpecialistAccountItemTravel,
                                                 ref dbTimesheet,
                                                 ref dbAssignment,
                                                 ref dbProject,
                                                 ref dbContract,
                                                 ref dbExpenseType,
                                                 dbModule,
                                                 commitChange,
                                                 isDbValidationRequired);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                                ValidationType validationType)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel = null;
            return IsRecordValidForProcess(accountItemTravel,
                                            validationType,
                                             ref dbSpecialistAccountItemTravel,
                                             ref dbTimesheet,
                                             ref dbAssignment,
                                             ref dbProject,
                                             ref dbContract,
                                             ref dbExpenseType);

        }

        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                                ValidationType validationType,
                                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType)
        {
            IList<DomainModel.TimesheetSpecialistAccountItemTravel> filteredAccountItemTravel = null;
            return IsRecordValidForProcess(accountItemTravel,
                                             validationType,
                                             ref filteredAccountItemTravel,
                                             ref dbSpecialistAccountItemTravel,
                                             ref dbTimesheet,
                                             ref dbAssignment,
                                             ref dbProject,
                                             ref dbContract,
                                             ref dbExpenseType);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                                ValidationType validationType,
                                                IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                                                ref IList<DbModel.Timesheet> dbTimesheet,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType)
        {
            return IsRecordValidForProcess(accountItemTravel,
                                          validationType,
                                           dbSpecialistAccountItemTravel,
                                           ref dbTimesheet,
                                           ref dbAssignment,
                                           ref dbProject,
                                           ref dbContract,
                                           ref dbExpenseType);
        }



        private Response AddTSTravel(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                    ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
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
                var recordToBeAdd = FilterRecord(accountItemTravel, ValidationType.Add);
                eventId = accountItemTravel?.FirstOrDefault()?.EventId;

                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(accountItemTravel,
                                                            ValidationType.Add,
                                                            ref recordToBeAdd,
                                                            ref dbSpecialistAccountItemTravel,
                                                            ref dbTimesheet,
                                                            ref dbAssignment,
                                                            ref dbProject,
                                                            ref dbContract,
                                                            ref dbExpenseType);
                else if (dbSpecialistAccountItemTravel?.Count <= 0)
                    dbSpecialistAccountItemTravel = GetTimesheetTSTravel(recordToBeAdd, ValidationType.Add);

                if (recordToBeAdd?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    _repository.AutoSave = false;
                    dbExpenseTypes = dbExpenseType;
                    dbProjects = dbProject;
                    dbContracts = dbContract;

                    dbSpecialistAccountItemTravel = _mapper.Map<IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isTimesheetTSTravelId"] = false;
                        opt.Items["Project"] = dbProjects;
                        opt.Items["Contract"] = dbContracts;
                        opt.Items["ExpenseType"] = dbExpenseTypes;
                    });
                    _repository.Add(dbSpecialistAccountItemTravel);
                    if (commitChange)
                    {
                       int value= _repository.ForceSave();
                        if (value > 0)
                        {
                            dbSpecialistAccountItemTravel?.ToList().ForEach(x1 => _auditSearchService.AuditLog(recordToBeAdd.FirstOrDefault(), ref eventId, accountItemTravel?.FirstOrDefault()?.ActionByUser, null,
                                                                                                      ValidationType.Add.ToAuditActionType(),
                                                                                                      SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemTravel, // D - 795
                                                                                                       null,
                                                                                                      _mapper.Map<DomainModel.TimesheetSpecialistAccountItemTravel>(x1),
                                                                                                      dbModule
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemTravel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateTSTravel(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                      ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
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
                var recordToBeModify = FilterRecord(accountItemTravel, ValidationType.Update);
                eventId = accountItemTravel?.FirstOrDefault().EventId;

                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(accountItemTravel,
                                                            ValidationType.Update,
                                                            ref recordToBeModify,
                                                            ref dbSpecialistAccountItemTravel,
                                                            ref dbTimesheet,
                                                            ref dbAssignment,
                                                            ref dbProject,
                                                            ref dbContract,
                                                            ref dbExpenseType);
                else if (dbSpecialistAccountItemTravel?.Count <= 0)
                    dbSpecialistAccountItemTravel = GetTimesheetTSTravel(recordToBeModify, ValidationType.Update);

                if (recordToBeModify?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    if (dbSpecialistAccountItemTravel?.Count > 0)
                    {
                        dbExpenseTypes = dbExpenseType;
                        dbProjects = dbProject;
                        dbContracts = dbContract;
                        IList<DomainModel.TimesheetSpecialistAccountItemTravel> domExistingTimeSheetAccountItemTravel = new List<DomainModel.TimesheetSpecialistAccountItemTravel>();
                        dbSpecialistAccountItemTravel?.ToList()?.ForEach(x =>
                        {
                            domExistingTimeSheetAccountItemTravel.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.TimesheetSpecialistAccountItemTravel>(x)));
                        });

                        dbSpecialistAccountItemTravel.ToList().ForEach(timesheetTS =>
                        {
                            var timesheetTSTravelToBeModify = recordToBeModify.FirstOrDefault(x => x.TimesheetTechnicalSpecialistAccountTravelId == timesheetTS.Id);
                            if(timesheetTSTravelToBeModify != null){
                            _mapper.Map(timesheetTSTravelToBeModify, timesheetTS, opt =>
                            {
                                opt.Items["isTimesheetTSTravelId"] = true;
                                opt.Items["Project"] = dbProjects;
                                opt.Items["Contract"] = dbContracts;
                                opt.Items["ExpenseType"] = dbExpenseTypes;
                            });
                            timesheetTS.LastModification = DateTime.UtcNow;
                            timesheetTS.UpdateCount = timesheetTSTravelToBeModify.UpdateCount.CalculateUpdateCount();
                            timesheetTS.ModifiedBy = timesheetTSTravelToBeModify.ModifiedBy;
                            }
                        });

                        _repository.AutoSave = false;
                        _repository.Update(dbSpecialistAccountItemTravel);
                        if (commitChange)
                        {
                           int value= _repository.ForceSave();
                            if (value > 0)
                            {
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser, null,
                                                                                                      ValidationType.Update.ToAuditActionType(), SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemTravel,
                                                                                                      domExistingTimeSheetAccountItemTravel?.FirstOrDefault(x2 => x2.TimesheetTechnicalSpecialistAccountTravelId == x1.TimesheetTechnicalSpecialistAccountTravelId),
                                                                                                      x1,dbModule));
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

        private Response RemoveTSTravel(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                      ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
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
                var recordToBeDelete = FilterRecord(accountItemTravel, ValidationType.Delete);
                eventId = accountItemTravel?.FirstOrDefault()?.EventId;

                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(accountItemTravel,
                                                       ValidationType.Delete,
                                                       ref recordToBeDelete,
                                                       ref dbSpecialistAccountItemTravel,
                                                       ref dbTimesheet,
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
                                dbSpecialistAccountItemTravel?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, accountItemTravel?.FirstOrDefault()?.ActionByUser, null,
                                                                                                        ValidationType.Delete.ToAuditActionType(),
                                                                                                        SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemTravel,
                                                                                                         _mapper.Map<DomainModel.TimesheetSpecialistAccountItemTravel>(x1),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemTravel);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }
        #endregion

        #region Private Methods

        private IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> GetTimesheetTSTravel(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                                                       ValidationType validationType)
        {
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel = null;
            if (validationType != ValidationType.Add)
            {
                if (accountItemTravel?.Count > 0)
                {
                    var timesheetTechnicalSpecialistAccountTravelId = accountItemTravel.Select(x => x.TimesheetTechnicalSpecialistAccountTravelId).Distinct().ToList();
                    dbSpecialistAccountItemTravel = _repository.FindBy(x => timesheetTechnicalSpecialistAccountTravelId.Contains(x.Id)).ToList();
                }
            }
            return dbSpecialistAccountItemTravel;
        }


        private IList<DomainModel.TimesheetSpecialistAccountItemTravel> FilterRecord(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                                                    ValidationType filterType)
        {
            IList<DomainModel.TimesheetSpecialistAccountItemTravel> filteredTSTime = null;

            if (filterType == ValidationType.Add)
                filteredTSTime = accountItemTravel?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredTSTime = accountItemTravel?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredTSTime = accountItemTravel?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredTSTime;
        }


        private bool IsValidPayload(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _techSpecAccountItemTravelValidationService.Validate(JsonConvert.SerializeObject(accountItemTravel), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Timesheet, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }


        private void FetchMasterData(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                     ref IList<DbModel.Data> masterData)
        {
            if (masterData == null)
            {
                IList<MasterType> types = new List<MasterType>() { MasterType.ExpenseType, MasterType.Currency };
                masterData = _masterService.Get(types);
            }
        }


        private bool IsValidExpenseType(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
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
                messages.Add(_messages, x.ChargeExpenseType, MessageType.TimesheetTSExpenseTypeInvalid, x.ChargeExpenseType);
            });

            if (dbExpenseType?.Count == 0 || dbExpenseType == null)
                dbExpenseType = dbExpense;

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsValidCurrency(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                         IList<DbModel.Data> masterData,
                                         ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (masterData != null)
            {
                var chargeCureency = accountItemTravel.Where(x => !string.IsNullOrEmpty(x.ChargeRateCurrency))?.Select(x => x.ChargeRateCurrency).ToList();
                var payCurrency = accountItemTravel.Where(x => !string.IsNullOrEmpty(x.PayRateCurrency))?.Select(x => x.PayRateCurrency).ToList();
                if (chargeCureency?.Count > 0 && payCurrency?.Count > 0)
                {
                    var dbChargeCurrency = masterData.Where(x => x.MasterDataTypeId == (int)MasterType.Currency).ToList();
                    var dbPayCurrency = masterData.Where(x => x.MasterDataTypeId == (int)MasterType.Currency).ToList();

                    var chargeNotExists = accountItemTravel.Where(x => !string.IsNullOrEmpty(x.ChargeRateCurrency))?.Where(x => !dbChargeCurrency.Any(x1 => x1.Code == x.ChargeRateCurrency)).ToList();
                    var payNotExists = accountItemTravel.Where(x => !string.IsNullOrEmpty(x.PayRateCurrency))?.Where(x => !dbPayCurrency.Any(x1 => x1.Code == x.PayRateCurrency)).ToList();
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

        private bool IsUniqueTimesheetTechSpecialistTravel(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                                           IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                                                           ValidationType validationType,
                                                           ref IList<ValidationMessage> validationMessages)
        {
            IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> travelExists = null;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            travelExists = _repository.IsUniqueTimesheetTSTravel(accountItemTravel, dbSpecialistAccountItemTravel, validationType);
            travelExists?.ToList().ForEach(x =>
            {
                messages.Add(_messages, x.TimesheetTechnicalSpecialistId, MessageType.TimesheetTSDuplicateRecord, x.TimesheetTechnicalSpecialistId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count > 0;
        }


        private bool IsValidContractProjectAssignmentTimesheet(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
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



        private bool IsRecordUpdateCountMatching(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                                IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = accountItemTravel.Where(x => !dbSpecialistAccountItemTravel.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() &&
                                                                                                             x1.Id == x.TimesheetTechnicalSpecialistAccountTravelId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x.TimesheetTechnicalSpecialistAccountTravelId, MessageType.TimesheetTravelUpdateCountMisMatch, x.TimesheetTechnicalSpecialistAccountTravelId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsTimesheetTravelExistInDb(IList<long> accountItemTravelIds,
                                                  IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
                                                  ref IList<long> timesheetTravelIdNotExists,
                                                  ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbSpecialistAccountItemTravel == null)
                dbSpecialistAccountItemTravel = new List<DbModel.TimesheetTechnicalSpecialistAccountItemTravel>();

            var validMessages = validationMessages;

            if (accountItemTravelIds?.Count > 0)
            {
                timesheetTravelIdNotExists = accountItemTravelIds.Where(x => !dbSpecialistAccountItemTravel.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                timesheetTravelIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TimesheetTravelInvalid, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidForAdd(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                        IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
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

            var timesheetIds = accountItemTravel.Where(x => x.TimesheetId > 0).Select(x => (long)x.TimesheetId).Distinct().ToList();
            var timesheetTechSpecIds = accountItemTravel.Where(x => x.TimesheetTechnicalSpecialistId > 0).Select(x => (long)x.TimesheetTechnicalSpecialistId).Distinct().ToList();
            FetchMasterData(accountItemTravel, ref dbMasterData);

            if (IsValidTimesheet(timesheetIds, ref dbTimesheet, ref messages, new string[] {  "TimesheetTechnicalSpecialist",
                                                                                                                "Assignment",
                                                                                                                "Assignment.Project",
                                                                                                                "Assignment.Project.Contract"}))
                if (IsValidExpenseType(accountItemTravel, ref dbExpenseType, dbMasterData, ref messages))
                    if (IsValidCurrency(accountItemTravel, dbMasterData, ref messages))
                        if (IsTimesheetTechnicalSpecialistExistInDb(timesheetTechSpecIds, dbTimesheet.ToList().SelectMany(x => x.TimesheetTechnicalSpecialist).ToList(), ref notExists, ref messages))
                            IsValidContractProjectAssignmentTimesheet(accountItemTravel, dbTimesheet, ref messages);
            //IsUniqueTimesheetTechSpecialistTravel(accountItemTravel, dbSpecialistAccountItemTravel, ValidationType.Add, ref validationMessages);

            dbAssignment = dbTimesheet?.ToList().Select(x => x.Assignment)?.ToList();
            dbProject = dbTimesheet?.ToList().Select(x => x.Assignment.Project)?.ToList();
            dbContract = dbTimesheet?.ToList().Select(x => x.Assignment.Project.Contract)?.ToList();


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsRecordValidForUpdate(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                        IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
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

            var timesheetIds = accountItemTravel.Where(x => x.TimesheetId > 0).Select(x => (long)x.TimesheetId).Distinct().ToList();
            var timesheetTechSpecIds = accountItemTravel.Where(x => x.TimesheetTechnicalSpecialistId > 0).Select(x => (long)x.TimesheetTechnicalSpecialistId).Distinct().ToList();
            FetchMasterData(accountItemTravel, ref dbMasterData);
            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(accountItemTravel, dbSpecialistAccountItemTravel, ref messages))
                    if (IsValidTimesheet(timesheetIds, ref dbTimesheet, ref messages, new string[] {  "TimesheetTechnicalSpecialist",
                                                                                                                        "Assignment",
                                                                                                                        "Assignment.Project",
                                                                                                                        "Assignment.Project.Contract"}))
                        if (IsValidExpenseType(accountItemTravel, ref dbExpenseType, dbMasterData, ref messages))
                            if (IsValidCurrency(accountItemTravel, dbMasterData, ref messages))
                                if (IsTimesheetTechnicalSpecialistExistInDb(timesheetTechSpecIds, dbTimesheet.ToList().SelectMany(x => x.TimesheetTechnicalSpecialist).ToList(), ref notExists, ref messages))
                                    IsValidContractProjectAssignmentTimesheet(accountItemTravel, dbTimesheet, ref messages);
                // IsUniqueTimesheetTechSpecialistTravel(accountItemTravel, dbSpecialistAccountItemTravel, ValidationType.Update, ref validationMessages);
            }

            dbAssignment = dbTimesheet?.ToList().Select(x => x.Assignment)?.ToList();
            dbProject = dbTimesheet?.ToList().Select(x => x.Assignment.Project)?.ToList();
            dbContract = dbTimesheet?.ToList().Select(x => x.Assignment.Project.Contract)?.ToList();


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private Response IsRecordValidForProcess(IList<DomainModel.TimesheetSpecialistAccountItemTravel> accountItemTravel,
                                                ValidationType validationType,
                                                ref IList<DomainModel.TimesheetSpecialistAccountItemTravel> filteredAccountItemTravel,
                                                ref IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbSpecialistAccountItemTravel,
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
                            var timesheetTSTravelId = filteredAccountItemTravel.Where(x => x.TimesheetTechnicalSpecialistAccountTravelId.HasValue)
                                                                                         .Select(x => (long)x.TimesheetTechnicalSpecialistAccountTravelId).Distinct().ToList();

                            if (dbSpecialistAccountItemTravel == null || dbSpecialistAccountItemTravel.Count <= 0)
                                dbSpecialistAccountItemTravel = GetTimesheetTSTravel(filteredAccountItemTravel, validationType);

                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                            {
                                result = IsTimesheetTravelExistInDb(timesheetTSTravelId,
                                                                      dbSpecialistAccountItemTravel,
                                                                      ref moduleNotExists,
                                                                      ref validationMessages);

                                if (result && validationType == ValidationType.Update)
                                    result = IsRecordValidForUpdate(filteredAccountItemTravel,
                                                                    dbSpecialistAccountItemTravel,
                                                                    ref dbTimesheet,
                                                                    ref dbAssignment,
                                                                    ref dbProject,
                                                                    ref dbContract,
                                                                    ref dbExpenseType,
                                                                    ref validationMessages);
                            }
                            else if (validationType == ValidationType.Add)
                                result = IsRecordValidForAdd(filteredAccountItemTravel,
                                                                 dbSpecialistAccountItemTravel,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemTravel);
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
