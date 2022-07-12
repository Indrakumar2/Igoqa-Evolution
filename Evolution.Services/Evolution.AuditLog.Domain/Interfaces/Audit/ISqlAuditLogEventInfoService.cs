using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DomainModel = Evolution.AuditLog.Domain.Models;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.AuditLog.Domain.Enums;

namespace Evolution.AuditLog.Domain.Interfaces.Audit
{
    public interface ISqlAuditLogEventInfoService
    {
        Response Get(DomainModel.Audit.SqlAuditLogEventInfo searchModel);

        Response Get(IList<string> moduleNames);

        Response Get(DomainModel.Audit.SqlAuditLogEventSearchInfo searchModel);

        Response Add(IList<Models.Audit.SqlAuditLogEventInfo> auditLogInfos, bool commitChange = true);

        Response Add(IList<Models.Audit.SqlAuditLogEventInfo> auditLogInfos, IList<DbModel.SqlauditModule> dbModules = null, bool commitChange = true);

        Response Add(IList<Models.Audit.SqlAuditLogDetailInfo> auditLogInfos, bool commitChange = true);

        Response Add(IList<Models.Audit.SqlAuditLogDetailInfo> auditLogInfos, IList<DbModel.SqlauditModule> dbModules = null, bool commitChange = true);

        Response IsRecordValidForProcess(IList<DomainModel.Audit.SqlAuditLogEventInfo> auditLogEventInfos, ValidationType validationType, ref IList<DbModel.SqlauditModule> dbModules);

        Response GetAll(DomainModel.Audit.SqlAuditLogEventSearchInfo searchModel);
        Response GetAuditEvent(DomainModel.Audit.SqlAuditLogEventSearchInfo searchModel);
        Response GetAuditLog(DomainModel.Audit.SqlAuditLogEventSearchInfo searchModel);

    }
}
