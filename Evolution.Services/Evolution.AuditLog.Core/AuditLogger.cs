using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.AuditLog.Domain.Models.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.AuditLog.Core
{
    public class AuditLogger : IAuditLogger
    {
        private readonly ISqlAuditLogEventInfoService _auditService = null;
        private readonly IAppLogger<AuditLogger> _logger = null;

        public AuditLogger(ISqlAuditLogEventInfoService service, IAppLogger<AuditLogger> logger)
        {
            this._auditService = service;
            this._logger = logger;
        }

        public Response LogAuditData(SqlAuditModuleType sqlAuditModuleType, SqlAuditModuleType sqlAuditSubModuleType, SqlAuditActionType sqlAuditActionType, string actionBy, string searchReference, object oldValue = null, object newValue = null)
        {
            string newValueJson = null;
            string oldValueJson = null;
            Exception exception = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            try
            {
                if (newValue != null)
                    newValueJson = newValue.Serialize(Common.Enums.SerializationType.JSON, true);

                if (oldValue != null)
                    oldValueJson = oldValue.Serialize(Common.Enums.SerializationType.JSON, true);

                var auditData = new SqlAuditLogDetailInfo()
                {
                    ActionBy = actionBy,
                    ActionType = sqlAuditActionType.DisplayName(),
                    AuditModuleName = sqlAuditModuleType.ToString(),
                    AuditSubModuleName = sqlAuditSubModuleType.ToString(),
                    NewValue = newValueJson,
                    OldValue = oldValueJson,
                    SearchReference = searchReference
                };

                return _auditService.Add(new List<SqlAuditLogDetailInfo>() { auditData }, dbModule);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response LogAuditData(long logEventId, SqlAuditModuleType sqlAuditSubModuleType, object oldValue = null, object newValue = null, IList<DbModel.SqlauditModule> dbModule = null)
        {
            string newValueJson = null;
            string oldValueJson = null;
            Exception exception = null;
            bool IsProcessOldRec=true;
            try
            {
                if (newValue != null)
                {
                    if ((newValue.IsList() || newValue.IsEnumerable()))
                    {
                        var lstNewValue=(IList)newValue;
                        var lstOldValue=(IList)oldValue;
                        IsProcessOldRec=false;
                       
                        for (int cnt=0; cnt< lstNewValue.Count;cnt++)
                        {
                            if(lstOldValue!=null)
                            LogAuditData(logEventId, sqlAuditSubModuleType,lstOldValue[cnt], lstNewValue[cnt], dbModule);
                            else
                            LogAuditData(logEventId, sqlAuditSubModuleType,null,lstNewValue[cnt], dbModule);
                        }
                    }
                    else
                    {
                        newValueJson = newValue.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName");
                    }
                }

                if (oldValue != null && IsProcessOldRec)
                {
                    if ((oldValue.IsList() || oldValue.IsEnumerable()))
                    {
                        var lstNewValue=(IList)newValue;
                        var lstOldValue=(IList)oldValue;
                        try{
                             for (int cnt=0; cnt<= lstOldValue?.Count-1; cnt++)
                            {
                            if(lstOldValue!=null)
                            LogAuditData(logEventId, sqlAuditSubModuleType,lstOldValue?[cnt],lstNewValue?[cnt]);
                            else
                            LogAuditData(logEventId, sqlAuditSubModuleType,lstOldValue?[cnt],null);
                            }
                        }
                        catch(Exception ex)
                        {
                            throw ex;
                        }
                        
                    }
                    else
                    {
                        oldValueJson = oldValue.AuditSerializeAttribute<Object, AuditNameAttribute>("AuditName");
                    }
                }
                if((!string.IsNullOrEmpty(newValueJson)) || (!string.IsNullOrEmpty(oldValueJson)))
                {
                    var auditData = new SqlAuditLogDetailInfo()
                   {
                    LogId = logEventId,
                    AuditSubModuleName = sqlAuditSubModuleType.ToString(),
                    NewValue = newValueJson,
                    OldValue = oldValueJson
                   };

                return _auditService.Add(new List<SqlAuditLogDetailInfo>() { auditData }, dbModule);
                }
 
                
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response LogAuditEvent(SqlAuditModuleType sqlAuditModuleType, SqlAuditActionType sqlAuditActionType, string actionBy, string searchReference, IList<DbModel.SqlauditModule> dbModule = null)
        {
            Exception exception = null;
            try
            {
                var auditEvent = new SqlAuditLogEventInfo()
                {
                    ActionBy = actionBy,
                    ActionType = sqlAuditActionType.ToString(), //Convert.ToChar(sqlAuditActionType.FirstChar()),
                    AuditModuleName = sqlAuditModuleType.ToString(),
                    SearchReference = searchReference
                };

                return _auditService.Add(new List<SqlAuditLogEventInfo>() { auditEvent },dbModule);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }
    }
}
