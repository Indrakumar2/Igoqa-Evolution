using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Data;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Models.Security;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Evolution.Security.Core.Services
{
    public class RoleDetailService : IRoleDetailService
    {
        private readonly IAppLogger<RoleDetailService> _logger = null;
        private readonly IRoleService _roleService = null;
        private readonly IActivityService _activityService = null;
        private readonly IMapper _mapper = null;
        private readonly IRoleDetailRepository _repository = null;
        //private readonly IUserRoleService _userRoleservice = null;
        private readonly JObject _messages = null;
        private readonly IAuditSearchService _auditSearchService = null;

        #region Constructor
        public RoleDetailService(IMapper mapper,
                                    IRoleDetailRepository repository,
                                    IAppLogger<RoleDetailService> logger,
                                    IRoleService roleService,
                                    IActivityService activityService,
                                    // IUserRoleService userRoleservice,
                                    JObject messages,
                                    IAuditSearchService auditSearchService)
        {
            this._repository = repository;
            this._roleService = roleService;
            this._activityService = activityService;
            // this._userRoleservice = userRoleservice;
            this._logger = logger;
            this._mapper = mapper;
            this._messages = messages;
            _auditSearchService = auditSearchService;
        }
        #endregion

        #region Public Methods
        public Response Add(IList<RoleDetail> roleDetails)
        {
            IList<DbRepository.Models.SqlDatabaseContext.Role> dbRoles = null;
            IList<DbRepository.Models.SqlDatabaseContext.Activity> dbActivities = null;
            IList<RoleInfo> roles = null;
            IList<ActivityInfo> activities = null;
            Response response = null;
            Exception exception = null;
            long? eventId = null; // Manage Security Audit changes
            try
            {
                //Checking, Whether provided role and activity exists in DB or not
                response = Validate(roleDetails, ValidationType.Add, ref roles, ref activities, ref dbRoles, ref dbActivities);
                if (response.Code != ResponseType.Success.ToId() || (bool)response.Result == false)
                    return response;

                if (dbRoles?.Count <= 0)
                {
                    //To-Do: Will create helper method get TransactionScope instance based on requirement
                    using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                    //using (var tranScope = new TransactionScope())
                    {
                        response = _roleService.Add(roles, ref dbRoles, ref eventId, true, true); // Manage Security Audit changes
                        if (response.Code != ResponseType.Success.ToId())
                            return response;
                        //if (response.Code == ResponseType.Success.ToId())
                        //{
                        //    var roleAllowedDuringLock = roles.Where(x => x.IsAllowedDuringLock == true).ToList();
                        //    roleAllowedDuringLock?.ForEach(x => {
                        //        var userRoles = dbRoles.SelectMany(x1 => x1.UserRole.Where(x2 => x2.RoleId == x.RoleId))?.ToList();
                        //        if (userRoles != null)
                        //            _userRoleservice.Modify(userRoles, roleDetails.ToList().FirstOrDefault().Role.ActionByUser);
                        //    });
                        //}
                        MapRoleActivity(roleDetails, dbRoles, dbActivities, ref eventId);
                        _repository.ForceSave();
                        tranScope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), roleDetails);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response Delete(IList<RoleDetail> roleDetails)
        {
            return _roleService.Delete(roleDetails.Select(x => x.Role).ToList());
        }

        public Response Get(RoleInfo searchModel)
        {
            IList<RoleDetail> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
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

        public Response Get(IList<string> roleNames)
        {
            IList<RoleDetail> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = this._repository.Search(roleNames);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), roleNames);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Modify(IList<RoleDetail> roleDetails)
        {
            IList<DbRepository.Models.SqlDatabaseContext.Role> dbRoles = null;
            IList<DbRepository.Models.SqlDatabaseContext.Activity> dbActivities = null;
            IList<RoleInfo> roles = null;
            IList<ActivityInfo> activities = null;
            Response response = null;
            Exception exception = null;
            long? eventId = null;
            try
            {
                //Checking, Whether provided role and activity exists in DB or not
                response = Validate(roleDetails, ValidationType.Update, ref roles, ref activities, ref dbRoles, ref dbActivities);
                if (response.Code != ResponseType.Success.ToId() || (bool)response.Result == false)
                    return response;

                if (dbRoles?.Count > 0)
                {
                    //To-Do: Will create helper method get TransactionScope instance based on requirement
                    using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                    //using (var tranScope = new TransactionScope())
                    {
                        _repository.AutoSave = false;
                        response = _roleService.Modify(roles, ref dbRoles, ref eventId, true, true);
                        if (response.Code != ResponseType.Success.ToId())
                            return response;
                        //if (response.Code == ResponseType.Success.ToId())
                        //{
                        //    var roleAllowedDuringLock = roles.Where(x => x.IsAllowedDuringLock == true).ToList();
                        //    roleAllowedDuringLock?.ForEach(x => {
                        //        var userRoles = dbRoles.SelectMany(x1 => x1.UserRole.Where(x2 => x2.RoleId == x.RoleId))?.ToList();
                        //        if (userRoles != null)
                        //            _userRoleservice.Modify(userRoles, roleDetails.ToList().FirstOrDefault().Role.ActionByUser);
                        //    });
                        //}

                        MapRoleActivity(roleDetails, dbRoles, dbActivities, ref eventId);
                        DeleteRoleActivity(roleDetails, dbActivities, ref eventId);

                        _repository.ForceSave();
                        tranScope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), roleDetails);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }
        #endregion

        #region Private Methods
        private Response Validate(IList<RoleDetail> roleDetails,
                                    ValidationType type,
                                    ref IList<RoleInfo> roles,
                                    ref IList<ActivityInfo> activities,
                                    ref IList<DbRepository.Models.SqlDatabaseContext.Role> dbRoles,
                                    ref IList<DbRepository.Models.SqlDatabaseContext.Activity> dbActivities)
        {
            Response response = null;
            string status = Utility.GetRecordStatus(type).FirstChar();
            roles = roleDetails?.Where(x => (x.Role.RecordStatus == status) == true)
                                    .Select(x => x.Role).ToList();
            response = _roleService.IsRecordValidForProcess(roles, type, ref dbRoles);
            if (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result))
            {
                var newStatus = Utility.GetRecordStatus(ValidationType.Add).FirstChar();
                var deleteStatus = Utility.GetRecordStatus(ValidationType.Delete).FirstChar();
                activities = roleDetails?.SelectMany(x2 => x2.Modules.SelectMany(x3 => x3.Activities))
                                           .Where(x1 => x1.RecordStatus == newStatus ||
                                                        x1.RecordStatus == deleteStatus)
                                           .ToList();

                IList<string> activityNotExists = null;
                var appActivities = activities.Select(x => new KeyValuePair<string, string>(x.ApplicationName, x.ActivityName)).ToList();
                response = _activityService.IsRecordExistInDb(appActivities, ref dbActivities, ref activityNotExists);
            }
            return response;
        }

        private void MapRoleActivity(IList<RoleDetail> roleDetail,
                                        IList<Role> dbRoles,
                                        IList<Activity> dbActivities,
                                        ref long? eventId)
        {
            IList<RoleActivity> roleActivities = new List<RoleActivity>();
            IList<ActivityInfo> domRoleActivity = new List<ActivityInfo>();
            IList<RoleInfo> roles = new List<RoleInfo>();
            long? eventID = 0;
            if (eventId != null)
                eventID = eventId;
            if (dbRoles?.Count > 0 && dbActivities?.Count > 0)
            {
                dbRoles.ToList().ForEach(x =>
                {
                    roles = roleDetail?.Select(r => r.Role).ToList();
                    var activities = roleDetail.Where(x1 => x1.Role.RoleName.Trim() == x.Name.Trim())
                                               .SelectMany(x2 => x2.Modules.SelectMany(x3 => x3.Activities.Select(x4 => new { x4.RecordStatus, x3.Module.ModuleId, x4.ActivityId, Activity = x4 }))
                                                                   .Where(x5 => x5.RecordStatus.IsRecordStatusNew())
                                                                   .ToList())
                                                .Join(dbActivities,
                                                        outer => outer.ActivityId,
                                                        inner => inner.Id,
                                                        (outer, inner) => new RoleActivity()
                                                        {
                                                            RoleId = (int)x.Id,
                                                            ActivityId = (int)inner.Id,
                                                            Activity = inner,
                                                            ModuleId = (int)outer.ModuleId
                                                        }).ToList();

                    roleActivities.AddRange<RoleActivity>(activities);
                });
                roleActivities?.ToList().ForEach(x =>
                {
                    domRoleActivity.Add(ObjectExtension.Clone(_mapper.Map<ActivityInfo>(x)));
                });
            }

            if (roleActivities.Count > 0)
            {
                _repository.Add(roleActivities);
                domRoleActivity?.ToList().ForEach(x => _auditSearchService.AuditLog(x, ref eventID, roles?.FirstOrDefault()?.ActionByUser,
                                                                                    null,
                                                                                    ValidationType.Add.ToAuditActionType(),
                                                                                    SqlAuditModuleType.SystemRole,
                                                                                    null,
                                                                                    x));
                eventId = eventID;
            }

        }

        private void DeleteRoleActivity(IList<RoleDetail> roleDetail, IList<Activity> dbActivities, ref long? eventId)
        {
            if (dbActivities != null)
            {
                IList<ActivityInfo> domRoleActivity = new List<ActivityInfo>();
                IList<RoleInfo> roles = new List<RoleInfo>();
                long? eventID = 0;
                if (eventId != null)
                    eventID = eventId;
                roles = roleDetail?.Select(r => r.Role).ToList();
                var roleActivities = roleDetail
                                       .SelectMany(x => x.Modules.SelectMany(X3 => X3.Activities.Select(x4 => new { x4.RecordStatus, X3.Module.ModuleId, Activity = x4 }))
                                                         .Where(x2 => x2.RecordStatus.IsRecordStatusDeleted())
                                                         .ToList()
                                                         .Join(dbActivities,
                                                            outer => outer.Activity.ActivityId,
                                                            inner => inner.Id,
                                                            (outer, inner) => new RoleActivity()
                                                            {
                                                                //Id=
                                                                ActivityId = (int)inner.Id,
                                                                RoleId = (int)x.Role.RoleId,
                                                                ModuleId = (int)outer.ModuleId,
                                                                Activity = inner
                                                            }).ToList());

                var roleIds = roleActivities?.Select(x => x.RoleId).Distinct().ToList();
                var activityIds = roleActivities?.Select(x => x.ActivityId).ToList();
                var moduleIds = roleActivities?.Select(x => x.ModuleId).ToList();

                if (roleIds?.Count > 0)
                {
                    var roleActvityByRoleId = _repository.FindBy(x => roleIds.Contains(x.RoleId));
                    if (activityIds?.Count > 0)

                        roleActvityByRoleId = roleActvityByRoleId.Join(roleActivities.ToList(),
                                                dbRole => new { dbRole.ModuleId, dbRole.ActivityId },
                                                dbRoleActivity => new { dbRoleActivity.ModuleId, dbRoleActivity.ActivityId },
                                               (dbRole, dbRoleActivity) => new { dbRole, dbRoleActivity })
                                                .Select(x => x.dbRole);
                    var dbRoleActivities = roleActvityByRoleId.ToList();

                    dbRoleActivities?.ToList().ForEach(x =>
                    {
                        domRoleActivity.Add(ObjectExtension.Clone(_mapper.Map<ActivityInfo>(x)));
                    });
                    if (dbRoleActivities?.Count > 0)
                    {
                        _repository.Delete(dbRoleActivities);
                        domRoleActivity?.ToList().ForEach(x => _auditSearchService.AuditLog(x, ref eventID, roles?.FirstOrDefault()?.ActionByUser,
                                                                        null,
                                                                        ValidationType.Delete.ToAuditActionType(),
                                                                        SqlAuditModuleType.SystemRole,
                                                                        x,
                                                                        null));
                        eventId = eventID;
                    }
                }
            }


        }

        #endregion
    }
}
