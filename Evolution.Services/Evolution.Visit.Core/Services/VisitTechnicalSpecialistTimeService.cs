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
    public class VisitTechnicalSpecialistTimeService : IVisitTechnicalSpecialistTimeService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<VisitTechnicalSpecialistTimeService> _logger = null;
        private readonly IVisitTechnicalSpecialistTimeRespository _repository = null;
        private readonly IVisitTechnicalSpecialistAccountItemTimeValidationService _techSpecAccountItemTimeValidationService = null;
        private readonly IVisitRepository _visitRepository = null;
        private readonly IMasterService _masterService = null;
        private readonly JObject _messages = null;
        private readonly IAuditSearchService _auditSearchService = null;

        public VisitTechnicalSpecialistTimeService(IAppLogger<VisitTechnicalSpecialistTimeService> logger,
                                              IVisitTechnicalSpecialistTimeRespository visitTimeRepository,
                                              IVisitTechnicalSpecialistAccountItemTimeValidationService validationService,
                                              IVisitRepository visitRepository,
                                              IMasterService masterService,
                                              IMapper mapper,
                                              JObject messages,
                                              IAuditSearchService auditSearchService)
                                             
        {
            _mapper = mapper;
            _logger = logger;
            _repository = visitTimeRepository;
            _visitRepository = visitRepository;
            _masterService = masterService;
            _techSpecAccountItemTimeValidationService = validationService;
            this._messages = messages;
            _auditSearchService = auditSearchService;
        }

        public Response Get(DomainModel.VisitSpecialistAccountItemTime searchModel)
        {
            IList<DomainModel.VisitSpecialistAccountItemTime> result = null;
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

        public async Task<Response> GetAsync(DomainModel.VisitSpecialistAccountItemTime searchModel)
        {
            IList<DomainModel.VisitSpecialistAccountItemTime> result = null;
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

        public Response Add(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
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
            IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes = null;
            if (visitId.HasValue)
                accountItemTimes?.ToList().ForEach(x => { x.VisitId = visitId; });

            return AddTSTime(accountItemTimes,
                                         ref dbSpecialistAccountItemTimes,
                                         ref dbVisit,
                                         ref dbAssignment,
                                         ref dbProject,
                                         ref dbContract,
                                         ref dbExpenseType,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }


        public Response Add(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                            ref IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
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
                accountItemTimes?.ToList().ForEach(x => { x.VisitId = visitId; });

            return AddTSTime(accountItemTimes,
                                         ref dbSpecialistAccountItemTimes,
                                         ref dbVisit,
                                         ref dbAssignment,
                                         ref dbProject,
                                         ref dbContract,
                                         ref dbExpenseType,
                                         dbModule,
                                         commitChange,
                                         isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
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
            IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes = null;
            if (visitId.HasValue)
                accountItemTimes?.ToList().ForEach(x => { x.VisitId = visitId; });

            return this.RemoveTSTime(accountItemTimes,
                                     ref dbSpecialistAccountItemTimes,
                                     ref dbVisit,
                                     ref dbAssignment,
                                     ref dbProject,
                                     ref dbContract,
                                     ref dbExpenseType,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequired);
        }


        public Response Delete(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
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
                accountItemTimes?.ToList().ForEach(x => { x.VisitId = visitId; });
            return this.RemoveTSTime(accountItemTimes,
                                                 ref dbSpecialistAccountItemTimes,
                                                 ref dbVisit,
                                                 ref dbAssignment,
                                                 ref dbProject,
                                                 ref dbContract,
                                                 ref dbExpenseType,
                                                 dbModule,
                                                 commitChange,
                                                 isDbValidationRequired);
        }


        public Response Modify(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                              bool commitChange = true,
                              bool isDbValidationRequired = true,
                              long? visitId = null)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            if (visitId.HasValue)
                accountItemTimes?.ToList().ForEach(x => { x.VisitId = visitId; });

            return UpdateTSTime(accountItemTimes,
                                     ref dbSpecialistAccountItemTimes,
                                     ref dbVisit,
                                     ref dbAssignment,
                                     ref dbProject,
                                     ref dbContract,
                                     ref dbExpenseType,
                                     dbModule,
                                     commitChange,
                                     isDbValidationRequired);
        }



        public Response Modify(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                    ref IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
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
                accountItemTimes?.ToList().ForEach(x => { x.VisitId = visitId; });

            return UpdateTSTime(accountItemTimes,
                                                 ref dbSpecialistAccountItemTimes,
                                                 ref dbVisit,
                                                 ref dbAssignment,
                                                 ref dbProject,
                                                 ref dbContract,
                                                 ref dbExpenseType,
                                                 dbModule,
                                                 commitChange,
                                                 isDbValidationRequired);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                                ValidationType validationType)
        {
            IList<DbModel.Data> dbExpenseType = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.Assignment> dbAssignment = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Contract> dbContract = null;
            IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes = null;
            return IsRecordValidForProcess(accountItemTimes,
                                            validationType,
                                             ref dbSpecialistAccountItemTimes,
                                             ref dbVisit,
                                             ref dbAssignment,
                                             ref dbProject,
                                             ref dbContract,
                                             ref dbExpenseType);

        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                                ValidationType validationType,
                                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType)
        {
            IList<DomainModel.VisitSpecialistAccountItemTime> filteredAccountItemTimes = null;
            return IsRecordValidForProcess(accountItemTimes,
                                             validationType,
                                             ref filteredAccountItemTimes,
                                             ref dbSpecialistAccountItemTimes,
                                             ref dbVisit,
                                             ref dbAssignment,
                                             ref dbProject,
                                             ref dbContract,
                                             ref dbExpenseType);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                                ValidationType validationType,
                                                IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                                                ref IList<DbModel.Visit> dbVisit,
                                                ref IList<DbModel.Assignment> dbAssignment,
                                                ref IList<DbModel.Project> dbProject,
                                                ref IList<DbModel.Contract> dbContract,
                                                ref IList<DbModel.Data> dbExpenseType)
        {
            return IsRecordValidForProcess(accountItemTimes,
                                          validationType,
                                           dbSpecialistAccountItemTimes,
                                           ref dbVisit,
                                           ref dbAssignment,
                                           ref dbProject,
                                           ref dbContract,
                                           ref dbExpenseType);
        }



        private Response AddTSTime(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                    ref IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
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
                var recordToBeAdd = FilterRecord(accountItemTimes, ValidationType.Add);
                eventId = accountItemTimes.FirstOrDefault().EventId;

                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(accountItemTimes,
                                                            ValidationType.Add,
                                                            ref recordToBeAdd,
                                                            ref dbSpecialistAccountItemTimes,
                                                            ref dbVisit,
                                                            ref dbAssignment,
                                                            ref dbProject,
                                                            ref dbContract,
                                                            ref dbExpenseType);
                else if (dbSpecialistAccountItemTimes?.Count <= 0)
                    dbSpecialistAccountItemTimes = GetVisitTSTime(recordToBeAdd, ValidationType.Add);

                if (recordToBeAdd?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    _repository.AutoSave = false;
                    dbExpenseTypes = dbExpenseType;
                    dbProjects = dbProject;
                    dbContracts = dbContract;
                    dbSpecialistAccountItemTimes = _mapper.Map<IList<DbModel.VisitTechnicalSpecialistAccountItemTime>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isVisitTSTimeId"] = false;
                        opt.Items["Project"] = dbProjects;
                        opt.Items["Contract"] = dbContracts;
                        opt.Items["ExpenseType"] = dbExpenseTypes;
                    });
                    _repository.Add(dbSpecialistAccountItemTimes);
                    if (commitChange)
                    {
                      int value =  _repository.ForceSave();
                        if (value > 0)
                        {
                            dbSpecialistAccountItemTimes?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, accountItemTimes?.FirstOrDefault()?.ActionByUser,
                                                                                                     null,
                                                                                                     ValidationType.Add.ToAuditActionType(),
                                                                                                     SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTime,
                                                                                                     null,
                                                                                                     _mapper.Map<DomainModel.VisitSpecialistAccountItemTime>(x1)
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemTimes);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateTSTime(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                      ref IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
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
            IList<DomainModel.VisitSpecialistAccountItemTime> result = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(accountItemTimes, ValidationType.Update);
                eventId = accountItemTimes?.FirstOrDefault()?.EventId;
                if (isDbValidationRequired)
                    valdResponse = IsRecordValidForProcess(accountItemTimes,
                                                            ValidationType.Update,
                                                            ref recordToBeModify,
                                                            ref dbSpecialistAccountItemTimes,
                                                            ref dbVisit,
                                                            ref dbAssignment,
                                                            ref dbProject,
                                                            ref dbContract,
                                                            ref dbExpenseType);
                else if (dbSpecialistAccountItemTimes?.Count <= 0)
                    dbSpecialistAccountItemTimes = GetVisitTSTime(recordToBeModify, ValidationType.Update);
                IList<DomainModel.VisitSpecialistAccountItemTime> domExistingVisitSpecialistAccountItem = new List<DomainModel.VisitSpecialistAccountItemTime>();
                dbSpecialistAccountItemTimes?.ToList()?.ForEach(x =>
                {
                    domExistingVisitSpecialistAccountItem.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.VisitSpecialistAccountItemTime>(x)));
                });

                if (recordToBeModify?.Count > 0 && (valdResponse == null || valdResponse.Code == MessageType.Success.ToId()))
                {
                    if (dbSpecialistAccountItemTimes?.Count > 0)
                    {
                        dbExpenseTypes = dbExpenseType;
                        dbProjects = dbProject;
                        dbContracts = dbContract;
                        dbSpecialistAccountItemTimes.ToList().ForEach(visitTS =>
                        {
                            var visitTSTimeToBeModify = recordToBeModify.FirstOrDefault(x => x.VisitTechnicalSpecialistAccountTimeId == visitTS.Id);
                            _mapper.Map(visitTSTimeToBeModify, visitTS, opt =>
                            {
                                opt.Items["isVisitTSTimeId"] = true;
                                opt.Items["Project"] = dbProjects;
                                opt.Items["Contract"] = dbContracts;
                                opt.Items["ExpenseType"] = dbExpenseTypes;
                            });
                            visitTS.LastModification = DateTime.UtcNow;
                            visitTS.UpdateCount = visitTSTimeToBeModify.UpdateCount.CalculateUpdateCount();
                            visitTS.ModifiedBy = visitTSTimeToBeModify.ModifiedBy;
                        });

                        _repository.AutoSave = false;
                        _repository.Update(dbSpecialistAccountItemTimes);
                        if (commitChange)
                        {
                          int value = _repository.ForceSave();
                            if (value > 0)
                            {
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                       null,
                                                                       ValidationType.Update.ToAuditActionType(),
                                                                       SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTime,
                                                                       domExistingVisitSpecialistAccountItem?.FirstOrDefault(x2 => x2.VisitTechnicalSpecialistAccountTimeId == x1.VisitTechnicalSpecialistAccountTimeId),
                                                                       x1,
                                                                       dbModule
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

        private Response RemoveTSTime(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                      ref IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
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
                var recordToBeDelete = FilterRecord(accountItemTimes, ValidationType.Delete);
                eventId = accountItemTimes?.FirstOrDefault()?.EventId;
                if (isDbValidationRequired)
                    response = IsRecordValidForProcess(accountItemTimes,
                                                       ValidationType.Delete,
                                                       ref recordToBeDelete,
                                                       ref dbSpecialistAccountItemTimes,
                                                       ref dbVisit,
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
                            int value=_repository.ForceSave();
                            if (value > 0)
                            {
                                dbSpecialistAccountItemTimes?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, accountItemTimes?.FirstOrDefault()?.ActionByUser,
                                                                                                          null,
                                                                                                          ValidationType.Delete.ToAuditActionType(),
                                                                                                          SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTime,
                                                                                                          _mapper.Map<DomainModel.VisitSpecialistAccountItemTime>(x1),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemTimes);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }


        private IList<DbModel.VisitTechnicalSpecialistAccountItemTime> GetVisitTSTime(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                                                       ValidationType validationType)
        {
            IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes = null;
            if (validationType != ValidationType.Add)
            {
                if (accountItemTimes?.Count > 0)
                {
                    var visitTechnicalSpecialistAccountTimeId = accountItemTimes.Select(x => x.VisitTechnicalSpecialistAccountTimeId).Distinct().ToList();
                    dbSpecialistAccountItemTimes = _repository.FindBy(x => visitTechnicalSpecialistAccountTimeId.Contains(x.Id)).ToList();
                }
            }
            return dbSpecialistAccountItemTimes;
        }


        private IList<DomainModel.VisitSpecialistAccountItemTime> FilterRecord(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                                                    ValidationType filterType)
        {
            IList<DomainModel.VisitSpecialistAccountItemTime> filteredTSTime = null;

            if (filterType == ValidationType.Add)
                filteredTSTime = accountItemTimes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredTSTime = accountItemTimes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredTSTime = accountItemTimes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredTSTime;
        }


        private bool IsValidPayload(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _techSpecAccountItemTimeValidationService.Validate(JsonConvert.SerializeObject(accountItemTimes), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Visit, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }


        private void FetchMasterData(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                     ref IList<DbModel.Data> masterData)
        {
            if (masterData == null)
            {
                IList<MasterType> types = new List<MasterType>() { MasterType.ExpenseType, MasterType.Currency };
                masterData = _masterService.Get(types);
            }
        }


        private bool IsValidExpenseType(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
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
                messages.Add(_messages, x.ChargeExpenseType, MessageType.VisitTSExpenseTypeInvalid, x.ChargeExpenseType);
            });

            if (dbExpenseType?.Count == 0 || dbExpenseType == null)
                dbExpenseType = dbExpense;

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsValidCurrency(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
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

                var chargeNotExists = accountItemTimes.Where(x => !string.IsNullOrEmpty(x.ChargeRateCurrency))?.Where(x => !dbChargeCurrency.Any(x1 => x1.Code == x.ChargeRateCurrency)).ToList();
                var payNotExists = accountItemTimes.Where(x => !string.IsNullOrEmpty(x.PayRateCurrency))?.Where(x => !dbPayCurrency.Any(x1 => x1.Code == x.PayRateCurrency)).ToList();
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


        private bool IsValidContractProjectAssignmentVisit(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
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



        private bool IsRecordUpdateCountMatching(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                                IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = accountItemTimes?.Where(x => !dbSpecialistAccountItemTimes.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() &&
                                                                                                             x1.Id == x.VisitTechnicalSpecialistAccountTimeId))?.ToList();
            notMatchedRecords?.ForEach(x =>
            {
                messages.Add(_messages, x.VisitTechnicalSpecialistAccountTimeId, MessageType.VisitTSTimeUpdateCountMisMatch, x.VisitTechnicalSpecialistAccountTimeId);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private bool IsVisitTimeExistInDb(IList<long> accountItemTimeIds,
                                                  IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
                                                  ref IList<long> visitTimeIdNotExists,
                                                  ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbSpecialistAccountItemTimes == null)
                dbSpecialistAccountItemTimes = new List<DbModel.VisitTechnicalSpecialistAccountItemTime>();

            var validMessages = validationMessages;

            if (accountItemTimeIds?.Count > 0)
            {
                visitTimeIdNotExists = accountItemTimeIds?.Where(x => !dbSpecialistAccountItemTimes.Select(x1 => x1.Id).Contains(x))?.ToList();
                visitTimeIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.VisitTimeInvalid, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidForAdd(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                        IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
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

            var visitIds = accountItemTimes.Where(x => x.VisitId > 0).Select(x => (long)x.VisitId).Distinct().ToList();
            var visitTechSpecIds = accountItemTimes.Where(x => x.VisitTechnicalSpecialistId > 0).Select(x => (long)x.VisitTechnicalSpecialistId).Distinct().ToList();
            if (dbExpenseType?.Count > 0)
                dbMasterData = dbExpenseType;
            else
            FetchMasterData(accountItemTimes, ref dbMasterData);
            if (IsValidVisit(visitIds, ref dbVisit, ref messages, new string[] {  "VisitTechnicalSpecialist",
                                                                                                                "Assignment",
                                                                                                                "Assignment.Project",
                                                                                                                "Assignment.Project.Contract"}))
                if (IsValidExpenseType(accountItemTimes, ref dbExpenseType, dbMasterData, ref messages))
                    if (IsValidCurrency(accountItemTimes, dbMasterData, ref messages))
                        if (IsVisitTechnicalSpecialistExistInDb(visitTechSpecIds, dbVisit.ToList().SelectMany(x => x.VisitTechnicalSpecialist).ToList(), ref notExists, ref messages))
                            IsValidContractProjectAssignmentVisit(accountItemTimes, dbVisit, ref messages);

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
                var resultVisits = _visitRepository.FetchVisits(visitId,includes);
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

        private bool IsRecordValidForUpdate(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                        IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
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

            var visitIds = accountItemTimes.Where(x => x.VisitId > 0).Select(x => (long)x.VisitId).Distinct().ToList();
            var visitTechSpecIds = accountItemTimes.Where(x => x.VisitTechnicalSpecialistId > 0).Select(x => (long)x.VisitTechnicalSpecialistId).Distinct().ToList();
            if (dbExpenseType?.Count > 0)
                dbMasterData = dbExpenseType;
            else
                FetchMasterData(accountItemTimes, ref dbMasterData);
            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(accountItemTimes, dbSpecialistAccountItemTimes, ref messages))
                    if (IsValidVisit(visitIds, ref dbVisit, ref messages, new string[] {  "VisitTechnicalSpecialist",
                                                                                                                        "Assignment",
                                                                                                                        "Assignment.Project",
                                                                                                                        "Assignment.Project.Contract"}))
                        if (IsValidExpenseType(accountItemTimes, ref dbExpenseType, dbMasterData, ref messages))
                            if (IsValidCurrency(accountItemTimes, dbMasterData, ref messages))
                                if (IsVisitTechnicalSpecialistExistInDb(visitTechSpecIds, dbVisit.ToList().SelectMany(x => x.VisitTechnicalSpecialist).ToList(), ref notExists, ref messages))
                                    IsValidContractProjectAssignmentVisit(accountItemTimes, dbVisit, ref messages);
            }

            dbAssignment = dbVisit?.ToList().Select(x => x.Assignment)?.ToList();
            dbProject = dbVisit?.ToList().Select(x => x.Assignment.Project)?.ToList();
            dbContract = dbVisit?.ToList().Select(x => x.Assignment.Project.Contract)?.ToList();


            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        private Response IsRecordValidForProcess(IList<DomainModel.VisitSpecialistAccountItemTime> accountItemTimes,
                                                ValidationType validationType,
                                                ref IList<DomainModel.VisitSpecialistAccountItemTime> filteredAccountItemTimes,
                                                ref IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbSpecialistAccountItemTimes,
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
                            var visitTSTimeId = filteredAccountItemTimes.Where(x => x.VisitTechnicalSpecialistAccountTimeId.HasValue)
                                                                                         .Select(x => (long)x.VisitTechnicalSpecialistAccountTimeId).Distinct().ToList();

                            if (dbSpecialistAccountItemTimes == null || dbSpecialistAccountItemTimes.Count <= 0)
                                dbSpecialistAccountItemTimes = GetVisitTSTime(filteredAccountItemTimes, validationType);
                            else
                            {
                                IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbItemExpense = GetVisitTSTime(filteredAccountItemTimes, validationType);
                                if(dbItemExpense != null && dbItemExpense.Count > 0)
                                    dbSpecialistAccountItemTimes.AddRange(dbItemExpense);
                            }

                            if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                            {
                                result = IsVisitTimeExistInDb(visitTSTimeId,
                                                                      dbSpecialistAccountItemTimes,
                                                                      ref moduleNotExists,
                                                                      ref validationMessages);

                                if (result && validationType == ValidationType.Update)
                                    result = IsRecordValidForUpdate(filteredAccountItemTimes,
                                                                    dbSpecialistAccountItemTimes,
                                                                    ref dbVisit,
                                                                    ref dbAssignment,
                                                                    ref dbProject,
                                                                    ref dbContract,
                                                                    ref dbExpenseType,
                                                                    ref validationMessages);
                            }
                            else if (validationType == ValidationType.Add)
                                result = IsRecordValidForAdd(filteredAccountItemTimes,
                                                                 dbSpecialistAccountItemTimes,
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), accountItemTimes);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
    }
}
