using Evolution.Security.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Models.Responses;
using Evolution.Security.Domain.Models.Security;
using Evolution.Logging.Interfaces;
using AutoMapper;
using Evolution.Security.Domain.Interfaces.Data;
using Newtonsoft.Json.Linq;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using System.Linq;
using Evolution.Common.Helpers;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Transactions;
using Evolution.Common.Models.Messages;

namespace Evolution.Security.Core.Services
{
    public class ModuleDetailService : IModuleDetailService
    {
        private readonly IAppLogger<ModuleDetailService> _logger = null;
        private readonly IModuleService _moduleService = null;
        private readonly IActivityService _activityService = null;
        private readonly IMapper _mapper = null;
        private readonly IModuleDetailRepository _repository = null;
        private readonly JObject _messages = null;

        #region Constructor
        public ModuleDetailService(IMapper mapper,
                                    IModuleDetailRepository repository,
                                    IAppLogger<ModuleDetailService> logger,
                                    IModuleService moduleService,
                                    IActivityService activityService,
                                    JObject messages)
        {
            this._repository = repository;
            this._moduleService = moduleService;
            this._activityService = activityService;
            this._logger = logger;
            this._mapper = mapper;
            this._messages = messages;
        }
        #endregion

        #region Public Methods
        public Response Add(IList<ModuleDetail> moduleDetails)
        {
            IList<DbRepository.Models.SqlDatabaseContext.Module> dbModules = null;
            IList<DbRepository.Models.SqlDatabaseContext.Activity> dbActivities = null;
            IList<ModuleInfo> modules = null;
            IList<ActivityInfo> activities = null;
            Response response = null;
            Exception exception = null;
            try
            {
                //Checking, Whether provided module and activity exists in DB or not
                response = Validate(moduleDetails, ValidationType.Add, ref modules, ref activities, ref dbModules, ref dbActivities);
                if (response.Code != ResponseType.Success.ToId() || (bool)response.Result == false)
                    return response;

                if (dbModules?.Count <= 0)
                {
                    //To-Do: Will create helper method get TransactionScope instance based on requirement
                    using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                    //using (var tranScope = new TransactionScope())
                    {
                        response = _moduleService.Add(modules, ref dbModules, true, false);
                        if (response.Code != ResponseType.Success.ToId())
                            return response;

                        MapModuleActivity(moduleDetails, dbModules, dbActivities);

                        _repository.ForceSave();

                        tranScope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), moduleDetails);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response Delete(IList<ModuleDetail> moduleDetails)
        {
            return _moduleService.Delete(moduleDetails.Select(x => x.Module).ToList());
        }

        public Response Get(ModuleInfo searchModel)
        {
            IList<ModuleDetail> result = null;
            Exception exception = null;
            try
            {
                result = this._repository.Search(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Modify(IList<ModuleDetail> moduleDetails)
        {
            IList<DbRepository.Models.SqlDatabaseContext.Module> dbModules = null;
            IList<DbRepository.Models.SqlDatabaseContext.Activity> dbActivities = null;
            IList<ModuleInfo> modules = null;
            IList<ActivityInfo> activities = null;
            Response response = null;
            Exception exception = null;
            try
            {
                //Checking, Whether provided module and activity exists in DB or not
                response = Validate(moduleDetails, ValidationType.Update, ref modules, ref activities, ref dbModules, ref dbActivities);
                if (response.Code != ResponseType.Success.ToId() || (bool)response.Result == false)
                    return response;

                if (dbModules?.Count > 0)
                {
                    //To-Do: Will create helper method get TransactionScope instance based on requirement
                    using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                    //using (var tranScope = new TransactionScope())
                    {
                        _repository.AutoSave = false;
                        response = _moduleService.Modify(modules, ref dbModules, true, false);
                        if (response.Code != ResponseType.Success.ToId())
                            return response;

                        MapModuleActivity(moduleDetails, dbModules, dbActivities);
                        DeleteModuleActivity(moduleDetails);

                        _repository.ForceSave();
                        tranScope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), moduleDetails);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }
        #endregion

        #region Private Methods
        private Response Validate(IList<ModuleDetail> moduleDetails,
                                    ValidationType type,
                                    ref IList<ModuleInfo> modules,
                                    ref IList<ActivityInfo> activities,
                                    ref IList<DbRepository.Models.SqlDatabaseContext.Module> dbModules,
                                    ref IList<DbRepository.Models.SqlDatabaseContext.Activity> dbActivities)
        {
            Response response = null;
            string status = Utility.GetRecordStatus(type).FirstChar();
            modules = moduleDetails?.Where(x => (x.Module.RecordStatus == status) == true)
                                    .Select(x => x.Module).ToList();
            response = _moduleService.IsRecordValidForProcess(modules, type, ref dbModules);
            if (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))
            {
                var newStatus = Utility.GetRecordStatus(ValidationType.Add).FirstChar();
                var deleteStatus = Utility.GetRecordStatus(ValidationType.Delete).FirstChar();
                activities = moduleDetails?.SelectMany(x2 => x2.Activities)
                                           .Where(x1 => x1.RecordStatus == newStatus ||
                                                        x1.RecordStatus == deleteStatus)
                                           .ToList();

                IList<string> activityNotExists = null;
                var appActivityies = activities.Select(x => new KeyValuePair<string, string>(x.ApplicationName, x.ActivityName)).ToList();
                response = _activityService.IsRecordExistInDb(appActivityies, ref dbActivities, ref activityNotExists);
            }
            return response;
        }

        private void MapModuleActivity(IList<ModuleDetail> moduleDetail,
                                        IList<Module> dbModules,
                                        IList<Activity> dbActivities)
        {
            IList<ModuleActivity> moduleActivities = new List<ModuleActivity>();
            if (dbModules?.Count > 0 && dbActivities?.Count > 0)
            {
                dbModules.ToList().ForEach(x =>
                {
                    var activities = moduleDetail.Where(x1 => x1.Module.ModuleName.Trim() == x.Name.Trim())
                                                 .SelectMany(x2 => x2.Activities
                                                                     .Where(x3 => x3.RecordStatus.IsRecordStatusNew())
                                                                     .ToList())
                                                 .Join(dbActivities,
                                                        outer => outer.ActivityName,
                                                        inner => inner.Name,
                                                        (outer, inner) => new ModuleActivity()
                                                        {
                                                            MouduleId = x.Id,
                                                            ActivityId = inner.Id
                                                        }).ToList();
                    moduleActivities.AddRange<ModuleActivity>(activities);
                });
            }

            if (moduleActivities.Count > 0)
                _repository.Add(moduleActivities);
        }

        private void DeleteModuleActivity(IList<ModuleDetail> moduleDetail)
        {
            var moduleActivities = moduleDetail
                                   .SelectMany(x => x.Activities
                                                     .Where(x2 => x2.RecordStatus.IsRecordStatusDeleted())
                                                     .Select(x1 => new ModuleActivity()
                                                     {
                                                         ActivityId = x1.ActivityId,
                                                         MouduleId = x.Module.ModuleId
                                                     }).ToList())
                                   .ToList();
            var moduleIds = moduleActivities?.Select(x => x.MouduleId).ToList();
            var activityIds = moduleActivities?.Select(x => x.ActivityId).ToList();
            if (moduleIds?.Count > 0)
            {
                var moduleActvityByModuleId = _repository.FindBy(x => moduleIds.Contains(x.MouduleId));
                if (activityIds?.Count > 0)
                    moduleActvityByModuleId = moduleActvityByModuleId.Where(x => activityIds.Contains(x.ActivityId));

                var dbModuleActivities = moduleActvityByModuleId.ToList();
                if (dbModuleActivities?.Count > 0)
                    _repository.Delete(dbModuleActivities);
            }
        }
        #endregion
    }
}
