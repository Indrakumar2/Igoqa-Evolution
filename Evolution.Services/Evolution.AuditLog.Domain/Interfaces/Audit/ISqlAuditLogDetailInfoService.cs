using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DomainModel = Evolution.AuditLog.Domain.Models;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
namespace Evolution.AuditLog.Domain.Interfaces.Audit
{
    public interface ISqlAuditLogDetailInfoService
    {
        Response Get(DomainModel.Audit.SqlAuditLogDetailInfo searchModel);

        Response Add(IList<DomainModel.Audit.SqlAuditLogDetailInfo> auditLogDetailInfos, ref IList<DbModel.SqlauditModule> dbModules, bool commitChange = true);

        Response IsRecordValidForProcess(IList<DomainModel.Audit.SqlAuditLogDetailInfo> auditLogDetailInfos,
                                            ValidationType validationType,
                                            ref IList<DbModel.SqlauditModule> dbModules);
             
       
    }
}
