using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Interfaces;
using Dom=Evolution.AuditLog.Domain.Models.Audit;
using System.Collections.Generic;
using System.Linq;
using Evolution.AuditLog.Domain.Models.Audit;

namespace Evolution.AuditLog.Domain.Interfaces.Data
{
    public interface ISqlAuditLogDetailRepository : IGenericRepository<SqlauditLogDetail>
    {
        IList<SqlauditLogDetail> Search(Dom.SqlAuditLogDetailInfo model);

        IList<SqlauditLogDetail> Get(IList<long> auditLogIds);
        
        IList<SqlauditLogDetail> Get(SqlAuditLogEventSearchInfo model);
    }
}
