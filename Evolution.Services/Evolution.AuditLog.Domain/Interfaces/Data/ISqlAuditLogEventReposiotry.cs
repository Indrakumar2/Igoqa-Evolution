using Evolution.AuditLog.Domain.Models.Audit;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using DomainModel = Evolution.AuditLog.Domain.Models;
namespace Evolution.AuditLog.Domain.Interfaces.Data
{
    public interface ISqlAuditLogEventReposiotry : IGenericRepository<SqlauditLogEvent>
    {
        IList<SqlauditLogEvent> Get(DomainModel.Audit.SqlAuditLogEventInfo searchModel);
        IList<SqlauditLogEvent> Get(IList<string> moduleNames);
        IQueryable<SqlauditLogEvent> Get(DomainModel.Audit.SqlAuditLogEventSearchInfo searchModel);
        List<SqlAuditLogDetailInfo> GetAuditData(SqlAuditLogEventSearchInfo model);
        List<SqlAuditLogDetailInfo> GetAuditEvent(SqlAuditLogEventSearchInfo model);
        List<SqlAuditLogDetailInfo> GetAuditLog(SqlAuditLogEventSearchInfo model);
    }
}
