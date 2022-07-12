using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Constants;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.Home.Domain.Interfaces.Data;
using Evolution.Home.Domain.Interfaces.Homes;
using Evolution.Home.Domain.Models.Homes;
using Evolution.Home.Domain.Models.Validations;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Security;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Home.Domain.Models.Homes;

namespace Evolution.Home.Core.Services
{
    public class MyTaskService : IMyTaskService
    {
        private readonly IAppLogger<MyTaskService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IMyTaskRepository _myTaskRepository = null;
        private readonly IUserService _userService = null;
        private readonly IMyTaskValidationService _myTaskValidationService = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        private readonly IAuditLogger _auditLogger = null;
        private readonly ICompanyRepository _companyRepository = null;

        #region Constructor

        public MyTaskService(IAppLogger<MyTaskService> logger,
                            IMapper mapper,
                            JObject messages,
                            IMyTaskRepository myTaskRepository,
                            IUserService userService,
                            IMyTaskValidationService myTaskValidationService,
                            IOptions<AppEnvVariableBaseModel> environment,
                            IAuditLogger auditLogger,
                            ICompanyRepository companyRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _messageDescriptions = messages;
            _myTaskRepository = myTaskRepository;
            _userService = userService;
            _myTaskValidationService = myTaskValidationService;
            _environment = environment.Value;
            _auditLogger = auditLogger;
            _companyRepository = companyRepository;
        }


        #endregion

        #region Public Methods

        #region Add

        public Response Add(IList<Domain.Models.Homes.MyTask> myTasks, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.Task> dbMyTasks = null;
            long? eventId = null;
            return AddMyTasks(myTasks, ref dbMyTasks, ref eventId, commitChange, isDbValidationRequired);
        }

        public Response Add(IList<Domain.Models.Homes.MyTask> myTasks, ref IList<DbRepository.Models.SqlDatabaseContext.Task> dbMyTasks, ref long? eventId, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return AddMyTasks(myTasks, ref dbMyTasks, ref eventId, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Delete

        public Response Delete(IList<Domain.Models.Homes.MyTask> myTasks, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return RemoveMyTasks(myTasks, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Get

        public Response Get(Domain.Models.Homes.MyTask searchModel)
        {
            IList<DomainModel.MyTask> result = null;
            Exception exception = null;
            try
            {
                result = _myTaskRepository.Search(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> moduleType, IList<string> assignedTo, IList<string> assignedBy = null, IList<string> taskRefCode = null)
        {
            IList<DomainModel.MyTask> result = null;
            Exception exception = null;
            try
            {
                result = _myTaskRepository.Search(moduleType, assignedTo, assignedBy, taskRefCode);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(string taskType, IList<string> taskRefCode)
        {
            IList<DomainModel.MyTask> result = null;
            Exception exception = null;
            try
            {
                result = _myTaskRepository.Search(taskType, taskRefCode: taskRefCode);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> moduleType, IList<string> taskRefCode)
        {
            IList<DomainModel.MyTask> result = null;
            Exception exception = null;
            try
            {
                result = _myTaskRepository.Search(moduleType, null, null, taskRefCode);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        #endregion

        #region Modify

        public Response Modify(IList<MyTask> myTasks, bool commitChange = true, bool isDbValidationRequired = true)
        {
            IList<DbModel.Task> dbMyTasks = null;
            return UpdateMyTasks(myTasks, ref dbMyTasks, commitChange, isDbValidationRequired);
        }

        public Response Modify(IList<Domain.Models.Homes.MyTask> myTasks, ref IList<DbRepository.Models.SqlDatabaseContext.Task> dbMyTasks, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return UpdateMyTasks(myTasks, ref dbMyTasks, commitChange, isDbValidationRequired);
        }

        public Response ModifyResourceSearchOnReassign(IList<Home.Domain.Models.Homes.MyTask> myTasks, bool commitChange = true)
        {
            return ModifyResourceSearch(myTasks);
        }

        #endregion

        #region Validation Check

        public Response IsRecordValidForProcess(IList<Domain.Models.Homes.MyTask> myTasks, ValidationType validationType)
        {
            IList<DbModel.Task> dbMyTasks = null;
            return IsRecordValidForProcess(myTasks, validationType, ref dbMyTasks);
        }

        public Response IsRecordValidForProcess(IList<Domain.Models.Homes.MyTask> myTasks, ValidationType validationType, ref IList<DbRepository.Models.SqlDatabaseContext.Task> dbMyTasks)
        {
            IList<MyTask> filteredMyTasks = null;
            IList<DbModel.User> dbUsers = null;
            return IsRecordValidForProcess(myTasks, validationType, ref filteredMyTasks, ref dbMyTasks, ref dbUsers);
        }

        public Response IsRecordValidForProcess(IList<Domain.Models.Homes.MyTask> myTasks, ValidationType validationType, IList<DbRepository.Models.SqlDatabaseContext.Task> dbMyTasks)
        {
            return IsRecordValidForProcess(myTasks, validationType, ref dbMyTasks);
        }

        #endregion

        #endregion

        #region Private Methods

        private Response AddMyTasks(IList<MyTask> myTasks, ref IList<DbModel.Task> dbMyTasks, ref long? eventId, bool commitChange = true, bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<DbModel.User> dbUsers = null;
            IList<DbModel.Company> dbCompanies = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                Response valdResponse = null;
                var recordToBeAdd = FilterRecord(myTasks, ValidationType.Add);

                if (dbMyTasks == null)
                    dbMyTasks = GetMyTasks(recordToBeAdd);
                //D661 issue1 myTask CR
                if (dbCompanies == null)
                    dbCompanies = _companyRepository?.FindBy(x => myTasks.Select(x1 => x1.CompanyCode).Contains(x.Code)).ToList();

                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(myTasks, ValidationType.Add, ref recordToBeAdd, ref dbMyTasks, ref dbUsers);

                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    _myTaskRepository.AutoSave = false;
                   var dbData = _mapper.Map<IList<DbModel.Task>>(recordToBeAdd, opt =>
                     {
                         opt.Items["isAssignId"] = false;
                         opt.Items["Users"] = dbUsers;
                         opt.Items["DbCompanies"] = dbCompanies;//D661 issue1 myTask CR
                     });
                    _myTaskRepository.Add(dbData);
                    if (commitChange)
                    {
                        int value = _myTaskRepository.ForceSave();
                        if (value > 0)
                            dbData?.ToList().ForEach(x => AuditLog(recordToBeAdd.FirstOrDefault(),
                                                                                  ValidationType.Add.ToAuditActionType(),
                                                                                  SqlAuditModuleType.MyTask,
                                                                                  null,
                                                                                  _mapper.Map<DomainModel.MyTask>(x)));
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), myTasks);
            }
            finally
            {
                _myTaskRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateMyTasks(IList<MyTask> myTasks, ref IList<DbModel.Task> dbMyTasks, bool commitChange = true, bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<MyTask> result = null;
            IList<DbModel.User> dbUsers = null;
            IList<ValidationMessage> validationMessages = null;
            bool isTaskDeleted = false;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(myTasks, ValidationType.Update);
                int? pendingWithId = null;
                if (dbMyTasks == null)
                {
                    if (isDbValidationRequire) //Added for D946
                        valdResponse = IsRecordValidForProcessUpdate(myTasks, ValidationType.Update, ref recordToBeModify, ref dbMyTasks, ref dbUsers);
                    //Added for D761 CR - Starts
                   
                    myTasks.ToList().ForEach(taskData =>
                        {
                            int techSpecId = Convert.ToInt32(taskData.TaskRefCode);
                            if(taskData.TaskType == TechnicalSpecialistConstants.Task_Type_Resource_To_Update_Profile)
                            {
                                 //pendingWithId = dbUsers?.FirstOrDefault(x => x.SamaccountName == taskData.AssignedTo).Id; //UAT 07-08Dec20 Doc Ref: Resource #2 Issue

                                var myTaskToBeDeleted = _myTaskRepository.FindBy(x => x.Id == taskData.Id).ToList();
                                if (myTaskToBeDeleted.Any())
                                {
                                    _myTaskRepository.Delete(myTaskToBeDeleted);// To delete task from RC myTasks list by Reassigning task
                                    isTaskDeleted = true;
                                }
                            }     
                        });
                    //Added for D761 CR - Ends
                    dbMyTasks = GetMyTasksUpdate(recordToBeModify);
                }

                if(isTaskDeleted == false)
                {
                    if (isDbValidationRequire)
                        valdResponse = IsRecordValidForProcessUpdate(myTasks, ValidationType.Update, ref recordToBeModify, ref dbMyTasks, ref dbUsers);
                   
                    if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result) && dbMyTasks?.Count > 0))
                    {
                        IList<MyTask> domExistingMyTask = new List<MyTask>();
                        dbMyTasks.ToList().ForEach(x =>
                        {
                            domExistingMyTask.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.MyTask>(x)));
                        });
                        dbMyTasks.ToList().ForEach(dbTask =>
                        {
                            var taskToBeModify = recordToBeModify?.FirstOrDefault(x => x.Id.Value == dbTask.Id);
                            dbTask.AssignedById = dbUsers?.FirstOrDefault(x => x.SamaccountName == taskToBeModify.AssignedBy).Id;
                            dbTask.AssignedToId = dbUsers?.FirstOrDefault(x => x.SamaccountName == taskToBeModify.AssignedTo).Id;
                            dbTask.LastModification = DateTime.UtcNow;
                            dbTask.ModifiedBy = taskToBeModify.AssignedBy;
                            pendingWithId = dbTask.AssignedToId;

                        });
                        _myTaskRepository.UpdateTechSpec(myTasks, pendingWithId);//TO Update AssignedTo,AssignedBy,Pendingwith while ReAssign
                        _myTaskRepository.AutoSave = false;
                        _myTaskRepository.Update(dbMyTasks);
                        if (commitChange)
                        {
                            int value = _myTaskRepository.ForceSave();
                            if (value > 0)
                                recordToBeModify?.ToList().ForEach(x => AuditLog(x,
                                                                   ValidationType.Update.ToAuditActionType(),
                                                                   SqlAuditModuleType.MyTask,
                                                                   domExistingMyTask?.FirstOrDefault(x1 => x1.MyTaskId == x.MyTaskId),
                                                                   x));
                        }
                    }
                    else
                        return valdResponse;
                } else {
                     _myTaskRepository.UpdateTechSpec(myTasks, pendingWithId);// To update TS ProfileAction if Resource Update Profile task Reassigned by RC ,---Also TO Update AssignedTo,AssignedBy,Pendingwith while ReAssign
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), myTasks);
            }
            finally
            {
                _myTaskRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response RemoveMyTasks(IList<MyTask> myTasks, bool commitChange, bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.Task> dbMyTasks = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                Response response = null;

                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(myTasks, ValidationType.Delete, ref dbMyTasks);

                if (!isDbValidationRequire || (Convert.ToBoolean(response.Result) && dbMyTasks?.Count > 0))
                {
                    _myTaskRepository.AutoSave = false;
                    _myTaskRepository.Delete(dbMyTasks);
                    if (commitChange)
                    {
                        int value = _myTaskRepository.ForceSave();
                        if (value > 0)
                            myTasks?.ToList().ForEach(x => AuditLog(x,
                                                                    ValidationType.Delete.ToAuditActionType(),
                                                                    SqlAuditModuleType.MyTask,
                                                                    x,
                                                                    null));
                    }
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), myTasks);
            }
            finally
            {
                _myTaskRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private IList<MyTask> FilterRecord(IList<MyTask> myTasks, ValidationType filterType)
        {
            IList<MyTask> filteredMyTasks = null;

            if (filterType == ValidationType.Add)
                filteredMyTasks = myTasks?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredMyTasks = myTasks?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredMyTasks = myTasks?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredMyTasks;
        }

        private IList<DbModel.Task> GetMyTasks(IList<MyTask> myTasks)
        {
            IList<DbModel.Task> dbMyTasks = null;
            var myTaskIds = myTasks?.Where(x => x.MyTaskId.HasValue)?.Select(x => x.MyTaskId)?.Distinct()?.ToList();
            if (myTaskIds?.Count > 0)
            {
                dbMyTasks = _myTaskRepository.FindBy(x => myTaskIds.Contains(x.Id)).ToList();
            }

            return dbMyTasks;
        }

        private IList<DbModel.Task> GetMyTasksUpdate(IList<MyTask> myTasks)
        {
            IList<DbModel.Task> dbMyTasks = null;
            var myTaskIds = myTasks?.Where(x => x.MyTaskId.HasValue)?.Select(x => x.Id)?.Distinct()?.ToList();
            if (myTaskIds?.Count > 0)
            {
                dbMyTasks = _myTaskRepository.FindBy(x => myTaskIds.Contains(x.Id)).ToList();
            }

            return dbMyTasks;
        }

        private Response IsRecordValidForProcess(IList<MyTask> myTasks,
                                ValidationType validationType,
                                ref IList<MyTask> filteredMyTasks,
                                ref IList<DbModel.Task> dbMyTasks,
                                ref IList<DbModel.User> dbUsers)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (myTasks != null && myTasks.Count > 0)
                {
                    validationMessages = new List<ValidationMessage>();

                    if (filteredMyTasks == null || filteredMyTasks.Count <= 0)
                        filteredMyTasks = FilterRecord(myTasks, validationType);

                    result = IsValidPayload(filteredMyTasks, validationType, ref validationMessages);
                    if (filteredMyTasks?.Count > 0 && result)
                    {
                        IList<int> moduleNotExists = null;

                        if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                        {
                            var myTaskIds = filteredMyTasks.Where(x => x.MyTaskId.HasValue).Select(x => x.MyTaskId.Value).Distinct().ToList();

                            if (dbMyTasks == null || dbMyTasks?.Count == 0)
                            {
                                dbMyTasks = GetMyTasks(filteredMyTasks);
                            }

                            result = IsMyTaskExistInDb(myTaskIds, dbMyTasks, ref moduleNotExists, ref validationMessages);

                            if (result && validationType == ValidationType.Update)
                                result = IsRecordValidaForUpdate(filteredMyTasks, dbMyTasks, ref dbUsers, ref validationMessages);
                        }
                        else if (validationType == ValidationType.Add)
                            result = IsRecordValidaForAdd(filteredMyTasks, dbMyTasks, ref dbUsers, ref validationMessages);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), myTasks);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response IsRecordValidForProcessUpdate(IList<MyTask> myTasks,
                                ValidationType validationType,
                                ref IList<MyTask> filteredMyTasks,
                                ref IList<DbModel.Task> dbMyTasks,
                                ref IList<DbModel.User> dbUsers)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (myTasks != null && myTasks.Count > 0)
                {
                    validationMessages = new List<ValidationMessage>();

                    if (filteredMyTasks == null || filteredMyTasks.Count <= 0)
                        filteredMyTasks = FilterRecord(myTasks, validationType);

                    result = IsValidPayload(filteredMyTasks, validationType, ref validationMessages);
                    if (filteredMyTasks?.Count > 0 && result)
                    {
                        IList<int> moduleNotExists = null;

                        if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                        {
                            var myTaskIds = filteredMyTasks.Where(x => x.MyTaskId.HasValue).Select(x => x.Id.Value).Distinct().ToList();

                            if (dbMyTasks == null || dbMyTasks?.Count == 0)
                            {
                                dbMyTasks = GetMyTasksUpdate(filteredMyTasks);
                            }

                            result = IsMyTaskExistInDb(myTaskIds, dbMyTasks, ref moduleNotExists, ref validationMessages);

                            if (result && validationType == ValidationType.Update)
                                result = IsRecordValidaForUpdate(filteredMyTasks, dbMyTasks, ref dbUsers, ref validationMessages);
                        }
                        else if (validationType == ValidationType.Add)
                            result = IsRecordValidaForAdd(filteredMyTasks, dbMyTasks, ref dbUsers, ref validationMessages);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), myTasks);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsValidPayload(IList<MyTask> myTasks,
                    ValidationType validationType,
                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _myTaskValidationService.Validate(JsonConvert.SerializeObject(myTasks), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Assignment, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsMyTaskExistInDb(IList<int> myTaskIds,
                        IList<DbModel.Task> dbMyTasks,
                        ref IList<int> myTaskIdsNotExists,
                        ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbMyTasks == null)
                dbMyTasks = new List<DbModel.Task>();

            var validMessages = validationMessages;

            if (myTaskIds?.Count > 0)
            {
                myTaskIdsNotExists = myTaskIds.Where(x => !dbMyTasks.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                myTaskIdsNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.MyTaskNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidaForAdd(IList<DomainModel.MyTask> myTasks, IList<DbModel.Task> dbMyTasks, ref IList<DbModel.User> dbUsers, ref IList<ValidationMessage> validationMessages)
        {
            bool result = true;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var myTaskIds = myTasks.Where(x => x.MyTaskId.HasValue).Select(x => x.MyTaskId.Value).Distinct().ToList();

            // 
            //var result = (!IsMyTaskExistInDb(myTaskIds, dbMyTasks, ref myTaskNotExists, ref messages) &&
            //              messages.Count == myTaskIds.Count);
            //if (!result)
            //{
            //    var myTaskAlreadyExists = myTaskIds.Where(x => !myTaskNotExists.Contains(x)).ToList();
            //    myTaskAlreadyExists?.ForEach(x =>
            //    {
            //        messages.Add(_messageDescriptions, x, MessageType.MyTaskAlreadyExist, x);
            //    });
            //}

            if (messages?.Count == 0)
            {
                IList<string> userNotExists = null;
                var userNames = myTasks.Select(x => x.AssignedBy).Union(myTasks.Select(x => x.AssignedTo)).Distinct().ToList();
                result = Convert.ToBoolean(_userService.IsRecordExistInDb(userNames.Select(x =>
                                                                            new KeyValuePair<string, string>(_environment.SecurityAppName, x)).ToList(),
                                                                            ref dbUsers,
                                                                            ref userNotExists).Result);
            }
            result = IsTSTaskUnique(myTasks, ref validationMessages);

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return result;
        }
        private bool IsTSTaskUnique(IList<DomainModel.MyTask> myTasks,
                                         ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var tsTask = myTasks.Select(x => new { x.TaskRefCode, x.TaskType, x.MyTaskId });
            var dbTsTask = _myTaskRepository.FindBy(x => tsTask.Any(x1 => x1.TaskRefCode == x.TaskRefCode && x1.TaskType == x.TaskType
                                              && x1.MyTaskId != x.Id)).ToList();
            if (dbTsTask?.Count > 0)
            {
                var tsTaskAlreadyExist = myTasks.Where(x => dbTsTask.Any(x1 => x.TaskRefCode == x1.TaskRefCode
                                                                   && x.TaskType == x1.TaskType));
                tsTaskAlreadyExist?.ToList().ForEach(x =>
                {
                    messages.Add(_messageDescriptions, x, MessageType.MyTaskAlreadyExist, x.TaskRefCode);
                });
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsRecordValidaForUpdate(IList<DomainModel.MyTask> myTasks, IList<DbModel.Task> dbMyTasks, ref IList<DbModel.User> dbUsers, ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            myTasks.Select(x => x.Id.Value).ToList()
                   .ForEach(x1 =>
                   {
                       var isExist = dbMyTasks.Any(x2 => x2.Id == x1);
                       if (!isExist)
                           messages.Add(_messageDescriptions, x1, MessageType.MyTaskNotExists, x1);
                   });

            if (messages?.Count == 0)
            {
                IList<string> userNotExists = null;
                var userNames = myTasks.Select(x => x.AssignedBy).Union(myTasks.Select(x => x.AssignedTo)).Distinct();
                _userService.IsRecordExistInDb(userNames.Select(x => new KeyValuePair<string, string>(_environment.SecurityAppName, x)).ToList(), ref dbUsers, ref userNotExists);
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        #endregion
        private Response AuditLog(DomainModel.MyTask task,
                                SqlAuditActionType sqlAuditActionType,
                                SqlAuditModuleType sqlAuditModuleType,
                                object oldData,
                                object newData)
        {
            LogEventGeneration logEventGeneration = new LogEventGeneration(_auditLogger);
            Exception exception = null;
            long? eventId = 0;
            if (task != null && !string.IsNullOrEmpty(task.ActionByUser))
            {
                string actionBy = task.ActionByUser;
                string searchRef = task.MyTaskId != null ? task.MyTaskId.ToString() : task.TaskRefCode;
                if (task.EventId > 0)
                    eventId = task.EventId;
                else
                    eventId = logEventGeneration.GetEventLogId(eventId,
                                                                  sqlAuditActionType,
                                                                  actionBy,
                                                                  searchRef,
                                                                  SqlAuditModuleType.MyTask.ToString());

                return _auditLogger.LogAuditData((long)eventId, sqlAuditModuleType, oldData, newData);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ModifyResourceSearch(IList<Home.Domain.Models.Homes.MyTask> myTasks, bool commitChange = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (myTasks?.Count > 0)
                {
                    // Filter my task to be modified
                    var mytaskTobeModified = myTasks?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                    var resourceSearchIds = mytaskTobeModified?.Where(x => x.TaskRefCode != null)?.Select(x => Convert.ToInt32(x.TaskRefCode)).ToList();
                    if (resourceSearchIds?.Count > 0)
                    {
                        var resourceSearchData = _myTaskRepository.GetResourceSearchByIds(resourceSearchIds);

                        resourceSearchData?.ToList().ForEach(x =>
                        {
                            var taskToBeModify = mytaskTobeModified?.FirstOrDefault(x1 => Convert.ToInt32(x1.TaskRefCode) == x.Id);
                            if (taskToBeModify.TaskType == ResourceSearchConstants.Task_Type_Override_Approval_Request.ToString())
                                x.AssignedToOm = taskToBeModify.AssignedTo;
                            if (taskToBeModify.TaskType == ResourceSearchConstants.Task_Type_OM_Approve_Reject_Resource.ToString() ||
                                taskToBeModify.TaskType == ResourceSearchConstants.Task_Type_PLO_No_Match_GRM.ToString() ||
                                taskToBeModify.TaskType == ResourceSearchConstants.Task_Type_PLO_Search_And_Save_Resources)
                                x.AssignedBy = taskToBeModify.AssignedTo;

                        });
                        // Repository update call to modify the assigned by and assigned to OM
                        _myTaskRepository.UpdateResourceSearchOnReassign(resourceSearchData);
                        if (commitChange)
                        {
                            _myTaskRepository.ForceSave();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), myTasks);
            }
            finally
            {
                _myTaskRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

    }
}
