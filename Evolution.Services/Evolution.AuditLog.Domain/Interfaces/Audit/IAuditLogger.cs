using Evolution.AuditLog.Domain.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.AuditLog.Domain.Interfaces.Audit
{
    public interface IAuditLogger
    {
        Response LogAuditData(long logEventId, SqlAuditModuleType sqlAuditSubModuleType,object oldValue = null, object newValue = null, IList<DbModel.SqlauditModule> dbModule = null);

        Response LogAuditData(SqlAuditModuleType sqlAuditModuleType, SqlAuditModuleType sqlAuditSubModuleType, SqlAuditActionType sqlAuditActionType, string actionBy, string searchReference, object oldValue = null, object newValue = null);

        Response LogAuditEvent(SqlAuditModuleType sqlAuditModuleType, SqlAuditActionType sqlAuditActionType, string actionBy, string searchReference, IList<DbModel.SqlauditModule> dbModule = null);
    }
}
