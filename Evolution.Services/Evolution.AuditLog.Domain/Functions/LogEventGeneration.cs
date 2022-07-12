using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.AuditLog.Domain.Functions
{
    public  class LogEventGeneration
    {
        private readonly IAuditLogger _auditLogger = null;
        public LogEventGeneration(IAuditLogger auditLogger)
        {
            this._auditLogger = auditLogger;
        }

        public long? GetEventLogId(long? eventId, SqlAuditActionType type, string actionBy, string searchRef, string moduleType,IList<DbModel.SqlauditModule> dbModule=null)
        {
            Exception exception = null;
            try
            {
                if (!eventId.HasValue || eventId == 0)
                {
                    var auditResponse = this._auditLogger.LogAuditEvent(moduleType.ToEnum<SqlAuditModuleType>(), type, actionBy, searchRef, dbModule);
                    if (auditResponse.Code == ResponseType.Success.ToId())
                        eventId = auditResponse.Result?.Populate<IList<long>>()?.FirstOrDefault();
                }
            }
             catch (Exception ex)
            {
                exception = ex;
            }

            return eventId;
        }
    }
}
