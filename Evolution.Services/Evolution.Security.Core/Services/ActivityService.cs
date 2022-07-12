using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Data;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Interfaces.Validations;
using Evolution.Security.Domain.Models.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Security.Domain.Models.Security;

namespace Evolution.Security.Core.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IAppLogger<ActivityService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly IActivityRepository _repository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IActivityValidationService _validationService = null;
        private readonly IApplicationRepository _applicationRepository = null;

        #region Constructor
        public ActivityService(IMapper mapper,
                             IActivityRepository repository,
                             IApplicationRepository applicationRepository,
                             IAppLogger<ActivityService> logger,
                             IActivityValidationService validationService,
                             JObject messgaes)
        {
            this._applicationRepository = applicationRepository;
            this._repository = repository;
            this._logger = logger;
            this._mapper = mapper;
            this._validationService = validationService;
            this._messageDescriptions = messgaes;
        }
        #endregion

        #region Public Methods

        #region Get
        public Response Get(ActivityInfo searchModel)
        {
            IList<DomainModel.ActivityInfo> result = null;
            Exception exception = null;
            try
            {
                result = this._repository.Search(searchModel)?
                                         .AsQueryable()
                                         .ProjectTo<DomainModel.ActivityInfo>()
                                         .ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(string applicationName, IList<string> activitiesNames)
        {
            IList<DomainModel.ActivityInfo> result = null;
            Exception exception = null;
            try
            {
                result = this.GetActivityByName(activitiesNames?.Select(x => new KeyValuePair<string, string>(applicationName, x)).ToList())
                              .AsQueryable()
                              .ProjectTo<DomainModel.ActivityInfo>()
                              .ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), activitiesNames);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> activitiesIds)
        {
            IList<DomainModel.ActivityInfo> result = null;
            Exception exception = null;
            try
            {
                result = this.GetActivityById(activitiesIds)
                                    .AsQueryable()
                                    .ProjectTo<DomainModel.ActivityInfo>()
                                    .ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), activitiesIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response IsRecordExistInDb(IList<KeyValuePair<string, string>> appActivityNames,
                                          ref IList<DbModel.Activity> dbActivities,
                                          ref IList<string> activitiesNotExists)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (dbActivities == null && appActivityNames?.Count > 0)
                    dbActivities = GetActivityByName(appActivityNames);

                result = IsActivityExistInDb(appActivityNames, dbActivities, ref activitiesNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), appActivityNames);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
        #endregion

        #region Add
        public Response Add(IList<ActivityInfo> activitiess, bool commitChange = true, bool isDbValidationRequire = true)
        {
            IList<DbModel.Activity> dbActivities = null;
            return AddActivity(activitiess, ref dbActivities, commitChange, isDbValidationRequire);
        }

        public Response Add(IList<ActivityInfo> activitiess, ref IList<DbModel.Activity> dbActivities, bool commitChange = true, bool isDbValidationRequire = true)
        {
            return AddActivity(activitiess, ref dbActivities, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Modify
        public Response Modify(IList<ActivityInfo> activitiess, bool commitChange = true, bool isDbValidationRequire = true)
        {
            IList<DbModel.Activity> dbActivities = null;
            return UpdateActivities(activitiess, ref dbActivities, commitChange, isDbValidationRequire);
        }

        public Response Modify(IList<ActivityInfo> activitiess, ref IList<DbModel.Activity> dbActivities, bool commitChange = true, bool isDbValidationRequire = true)
        {
            return UpdateActivities(activitiess, ref dbActivities, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Delete
        public Response Delete(IList<ActivityInfo> activitiesInfo, bool commitChange = true, bool isDbValidationRequire = true)
        {
            return this.RemoveActivity(activitiesInfo, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Record Valid Check
        public Response IsRecordValidForProcess(IList<ActivityInfo> activitiess, ValidationType validationType)
        {
            IList<DbModel.Activity> dbActivities = null;
            return IsRecordValidForProcess(activitiess, validationType, ref dbActivities);
        }

        public Response IsRecordValidForProcess(IList<ActivityInfo> activitiess, ValidationType validationType, ref IList<DbModel.Activity> dbActivities)
        {
            IList<DomainModel.ActivityInfo> filteredActivities = null;
            IList<DbModel.Application> dbApplications = null;
            return this.CheckRecordValidForProcess(activitiess, validationType, ref filteredActivities, ref dbActivities, ref dbApplications);
        }

        public Response IsRecordValidForProcess(IList<ActivityInfo> activitiess, ValidationType validationType, IList<DbModel.Activity> dbActivities)
        {
            return IsRecordValidForProcess(activitiess, validationType, ref dbActivities);
        }
        #endregion

        #endregion

        #region Private Metods
        #region Get
        private IList<DbModel.Activity> GetActivityByName(IList<KeyValuePair<string, string>> appActivityNames)
        {
            IList<DbModel.Activity> dbActivities = null;
            if (appActivityNames?.Count > 0)
            {
                var appNames = appActivityNames.Select(x => x.Key);
                var activitiesNames = appActivityNames.Select(x => x.Value);
                dbActivities = _repository.FindBy(x => activitiesNames.Contains(x.Name) && appNames.Contains(x.Application.Name)).ToList();
            }
            return dbActivities;
        }

        private IList<DbModel.Activity> GetActivityById(IList<int> activitiesIds)
        {
            IList<DbModel.Activity> dbActivities = null;
            if (activitiesIds?.Count > 0)
                dbActivities = _repository.FindBy(x => activitiesIds.Contains((int)x.Id)).ToList();

            return dbActivities;
        }
        #endregion

        #region Add
        private bool IsRecordValidForAdd(IList<DomainModel.ActivityInfo> activitiess,
                                         ref IList<DomainModel.ActivityInfo> filteredActivities,
                                         ref IList<DbRepository.Models.SqlDatabaseContext.Activity> dbActivities,
                                         ref IList<DbRepository.Models.SqlDatabaseContext.Application> dbApplications,
                                         ref IList<ValidationMessage> validationMessages)
        {
            bool result = true;
            if (activitiess != null && activitiess.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredActivities == null || filteredActivities.Count <= 0)
                    filteredActivities = FilterRecord(activitiess, validationType);

                if (filteredActivities?.Count > 0 && IsValidPayload(filteredActivities, validationType, ref validationMessages))
                {
                    //IList<int> activitiesIds = null;
                    IList<string> activitiesNotExists = null;
                    IList<ValidationMessage> messages = new List<ValidationMessage>();
                    this.GetActivityAndApplicationDbInfo(filteredActivities, false, ref dbActivities, ref dbApplications);
                    var applications = activitiess.Select(x => new KeyValuePair<int, string>(0, x.ApplicationName)).Distinct().ToList();
                    if (this.IsApplicationExistInDb(applications, dbApplications, ref validationMessages))
                    {
                        var appActivityNames = activitiess.Select(x => new KeyValuePair<string, string>(x.ApplicationName, x.ActivityName)).Distinct().ToList();
                        var response = (!IsActivityExistInDb(appActivityNames, dbActivities, ref activitiesNotExists, ref messages) && messages.Count == appActivityNames.Count);
                        if (!response)
                        {
                            var mdouleAlreadyExists = appActivityNames.Where(x => !activitiesNotExists.Contains(x.Value)).ToList();
                            mdouleAlreadyExists?.ForEach(x =>
                            {
                                messages.Add(_messageDescriptions, x, MessageType.ActivityAlreadyExist, x);
                            });

                            if (messages.Count > 0)
                            {
                                result = false;
                                validationMessages.AddRange(messages);
                            }
                        }
                    }
                    else
                        result = false;
                }
                else
                    result = false;
            }

            return result;
        }

        private Response AddActivity(IList<ActivityInfo> activitiess, ref IList<DbModel.Activity> dbActivities, bool commitChange = true, bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                Response valdResponse = null;
                IList<DbModel.Application> dbApplications = null;
                IList<ActivityInfo> recordToBeAdd = null;

                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(activitiess, ValidationType.Add, ref recordToBeAdd, ref dbActivities, ref dbApplications);

                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    _repository.AutoSave = false;
                    recordToBeAdd = recordToBeAdd.Select(x => { x.ActivityId = 0; return x; }).ToList();
                    _repository.Add(_mapper.Map<IList<DbModel.Activity>>(recordToBeAdd));
                    if (commitChange)
                    {
                        _repository.ForceSave();
                        dbActivities = _repository.Get(recordToBeAdd.Select(x => x.ActivityName).ToList());
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), activitiess);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }
        #endregion

        #region Modify
        private Response UpdateActivities(IList<ActivityInfo> activitiess, ref IList<DbModel.Activity> dbActivities, bool commitChange = true, bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                Response valdResponse = null;
                IList<DbModel.Application> dbApplications = null;
                var recordToBeModify = FilterRecord(activitiess, ValidationType.Update);

                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(activitiess, ValidationType.Update, ref recordToBeModify, ref dbActivities, ref dbApplications);
                else if ((dbActivities == null || dbActivities?.Count <= 0) && recordToBeModify?.Count > 0)
                    dbActivities = _repository.Get(recordToBeModify?.Select(x => (int)x.ActivityId).ToList());

                if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result) && dbActivities?.Count > 0))
                {
                    dbActivities.ToList().ForEach(dbActivity =>
                    {
                        var activitiesToBeModify = recordToBeModify.FirstOrDefault(x => x.ActivityId == dbActivity.Id);
                        dbActivity.ApplicationId = dbApplications.FirstOrDefault(x => x.Name == activitiesToBeModify.ApplicationName).Id;
                        dbActivity.Code = activitiesToBeModify.ActivityCode;
                        dbActivity.Name = activitiesToBeModify.ActivityName;
                        dbActivity.Description = activitiesToBeModify.Description;
                        dbActivity.LastModification = DateTime.UtcNow;
                        dbActivity.UpdateCount = activitiesToBeModify.UpdateCount.CalculateUpdateCount();
                        dbActivity.ModifiedBy = activitiesToBeModify.ModifiedBy;
                    });
                    _repository.AutoSave = false;
                    _repository.Update(dbActivities);
                    if (commitChange)
                        _repository.ForceSave();
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), activitiess);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private bool IsRecordValidForUpdate(IList<DomainModel.ActivityInfo> activitiess,
                                            ref IList<DomainModel.ActivityInfo> filteredActivities,
                                            ref IList<DbRepository.Models.SqlDatabaseContext.Activity> dbActivities,
                                            ref IList<DbRepository.Models.SqlDatabaseContext.Application> dbApplications,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (activitiess != null && activitiess.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredActivities == null || filteredActivities.Count <= 0)
                    filteredActivities = FilterRecord(activitiess, validationType);

                if (filteredActivities?.Count > 0 && IsValidPayload(filteredActivities, validationType, ref messages))
                {
                    this.GetActivityAndApplicationDbInfo(filteredActivities, true, ref dbActivities, ref dbApplications);
                    IList<int> activitiesIds = filteredActivities.Select(x => (int)x.ActivityId).ToList();
                    if (dbActivities?.Count != activitiesIds?.Count) //Invalid Activity Id found.
                    {
                        var dbActivityByIds = dbActivities;
                        var idNotExists = activitiesIds.Where(id => !dbActivityByIds.Any(activities => activities.Id == id)).ToList();
                        var activitiesList = filteredActivities;
                        idNotExists?.ForEach(x =>
                        {
                            var activities = activitiesList.FirstOrDefault(x1 => x1.ActivityId == x);
                            messages.Add(_messageDescriptions, activities, MessageType.RequestedUpdateActivityNotExists);
                        });
                    }
                    else
                    {
                        var applications = filteredActivities.Select(x => new KeyValuePair<int, string>(0, x.ApplicationName)).Distinct().ToList();
                        if (this.IsApplicationExistInDb(applications, dbApplications, ref messages))
                        {
                            result = IsRecordUpdateCountMatching(filteredActivities, dbActivities, ref messages);
                            if (result)
                                result = this.IsActivityNameUnique(filteredActivities, ref validationMessages);
                        }
                    }
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }

            return result;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.ActivityInfo> activitiess, IList<DbRepository.Models.SqlDatabaseContext.Activity> dbActivities, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = activitiess.Where(x => !dbActivities.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.ActivityId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x.ActivityName, MessageType.ActivityUpdateCountMismatch, x.ActivityName);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsActivityNameUnique(IList<DomainModel.ActivityInfo> filteredActivities, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var activitiesNames = filteredActivities.Select(x => x.ActivityName);
            var appNames = filteredActivities.Select(x => x.ApplicationName);
            var appActivities = _repository.FindBy(x => appNames.Contains(x.Application.Name))
                                        .Join(activitiesNames,
                                              activities => activities.Name,
                                              activitiesName => activitiesName,
                                              (activities, activitiesName) => activities)
                                        .ToList();
            if (appActivities?.Count > 0)
            {
                var activitiesAlreadyExist = filteredActivities.Where(x => !appActivities.Select(x1 => x1.Id)
                                                                 .ToList()
                                                                 .Contains((int)x.ActivityId));
                activitiesAlreadyExist?.ToList().ForEach(x =>
                {
                    messages.Add(_messageDescriptions, x.ActivityName, MessageType.ActivityAlreadyExist, x.ActivityName);
                });
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }
        #endregion

        #region Remove
        private bool IsRecordValidForRemove(IList<DomainModel.ActivityInfo> activitiess,
                                            ref IList<DomainModel.ActivityInfo> filteredActivities,
                                            ref IList<DbRepository.Models.SqlDatabaseContext.Activity> dbActivities,
                                            ref IList<DbRepository.Models.SqlDatabaseContext.Application> dbApplications,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (activitiess != null && activitiess.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredActivities == null || filteredActivities.Count <= 0)
                    filteredActivities = FilterRecord(activitiess, validationType);

                if (filteredActivities?.Count > 0 && IsValidPayload(filteredActivities, validationType, ref validationMessages))
                {
                    this.GetActivityAndApplicationDbInfo(filteredActivities, false, ref dbActivities, ref dbApplications);
                    var applications = filteredActivities.Select(x => new KeyValuePair<int, string>(0, x.ApplicationName)).Distinct().ToList();
                    if (this.IsApplicationExistInDb(applications, dbApplications, ref validationMessages))
                    {
                        IList<string> activitiesNotExists = null;
                        var appActivityNames = filteredActivities.Select(x => new KeyValuePair<string, string>(x.ApplicationName, x.ActivityName)).Distinct().ToList();
                        result = IsActivityExistInDb(appActivityNames, dbActivities, ref activitiesNotExists, ref validationMessages);
                        if (result)
                            result = IsActivityCanBeRemove(dbActivities, ref validationMessages);
                    }
                }
            }
            return result;
        }

        private bool IsActivityCanBeRemove(IList<DbModel.Activity> dbActivities, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            dbActivities?.Where(x => x.IsAnyCollectionPropertyContainValue())
                        .ToList()
                        .ForEach(x =>
                        {
                            messages.Add(_messageDescriptions, x.Name, MessageType.ActivityIsBeingUsed, x.Name);
                        });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }
        #endregion

        #region Common
        private void GetActivityAndApplicationDbInfo(IList<ActivityInfo> filteredActivities,
                                                 bool isActivityInfoById,
                                                 ref IList<DbModel.Activity> dbActivities,
                                                 ref IList<DbModel.Application> dbAppications)
        {
            var activitiesNames = !isActivityInfoById ?
                            filteredActivities.Select(x => new KeyValuePair<string, string>(x.ApplicationName, x.ActivityName))
                                         .Distinct()
                                         .ToList() : null;

            IList<int> activitiesIds = isActivityInfoById ? filteredActivities.Select(x => (int)x.ActivityId).Distinct().ToList() : null;
            IList<string> appNames = filteredActivities.Select(x => x.ApplicationName).Distinct().ToList();
            if (dbActivities == null || dbActivities.Count <= 0)
                dbActivities = isActivityInfoById ? this.GetActivityById(activitiesIds).ToList() : this.GetActivityByName(activitiesNames).ToList();
            if (dbAppications == null || dbAppications.Count <= 0)
                dbAppications = _applicationRepository.Get(appNames).ToList();
        }

        private IList<DomainModel.ActivityInfo> FilterRecord(IList<DomainModel.ActivityInfo> activitiess,
                                                            ValidationType filterType)
        {
            IList<DomainModel.ActivityInfo> filteredActivities = null;

            if (filterType == ValidationType.Add)
                filteredActivities = activitiess?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredActivities = activitiess?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredActivities = activitiess?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredActivities;
        }

        private bool IsActivityExistInDb(IList<KeyValuePair<string, string>> appActivityNames,
                                        IList<DbModel.Activity> dbActivities,
                                        ref IList<string> activitiesNotExists,
                                        ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbActivities == null)
                dbActivities = new List<DbModel.Activity>();

            var validMessages = validationMessages;

            if (appActivityNames?.Count > 0)
            {
                activitiesNotExists = appActivityNames
                                  .Where(x => !dbActivities
                                              .Any(x1 => x1.Application.Name == x.Key &&
                                                         x1.Name == x.Value))
                                  .Select(x => x.Value)
                                  .ToList();

                activitiesNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.ActivityNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private Response RemoveActivity(IList<ActivityInfo> activitiess, bool commitChange, bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DbModel.Activity> dbActivities = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();

                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(activitiess, ValidationType.Delete, ref dbActivities);

                if (!isDbValidationRequire || (response.Code == ResponseType.Success.ToId() &&
                                                Convert.ToBoolean(response.Result) &&
                                                dbActivities?.Count > 0))
                {
                    _repository.AutoSave = false;
                    _repository.Delete(dbActivities);
                    if (commitChange)
                        _repository.ForceSave();
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), activitiess);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<DomainModel.ActivityInfo> activitiess,
                                                    ValidationType validationType,
                                                    ref IList<DomainModel.ActivityInfo> filteredActivities,
                                                    ref IList<DbModel.Activity> dbActivities,
                                                    ref IList<DbModel.Application> dbAppications)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(activitiess, ref filteredActivities, ref dbActivities, ref dbAppications, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(activitiess, ref filteredActivities, ref dbActivities, ref dbAppications, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(activitiess, ref filteredActivities, ref dbActivities, ref dbAppications, ref validationMessages);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), activitiess);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsActivityExistInDb(IList<int> activitiesIds,
                                        IList<DbModel.Activity> dbActivities,
                                        ref IList<int> activitiesNotExists,
                                        ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbActivities == null)
                dbActivities = new List<DbModel.Activity>();

            var validMessages = validationMessages;

            if (activitiesIds?.Count > 0)
            {
                activitiesNotExists = activitiesIds.Where(x => !dbActivities.Select(x1 => (int)x1.Id).ToList().Contains(x)).ToList();
                activitiesNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.ActivityNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsValidPayload(IList<DomainModel.ActivityInfo> activitiess,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(activitiess), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Security, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsApplicationExistInDb(IList<KeyValuePair<int, string>> applications,
                                            IList<DbRepository.Models.SqlDatabaseContext.Application> dbApplications,
                                            ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbApplications == null)
                dbApplications = new List<DbModel.Application>();

            var validMessages = validationMessages;

            if (applications?.Count > 0)
            {
                var appNotExists = applications
                                  .Where(x => dbApplications
                                              .Count(x1 =>
                                              {
                                                  var result = false;
                                                  if (x.Key > 0 && !string.IsNullOrEmpty(x.Value))
                                                      result = (x1.Id == x.Key && x1.Name == x.Value);
                                                  else if (x.Key > 0)
                                                      result = (x1.Id == x.Key);
                                                  else if (!string.IsNullOrEmpty(x.Value))
                                                      result = (x1.Name == x.Value);

                                                  return result;
                                              }) <= 0)
                                  .Select(x => x.Value)
                                  .ToList();

                appNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.ApplicationNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        #endregion
        #endregion
    }
}