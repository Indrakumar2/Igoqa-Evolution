using System.Collections.Generic;
using Evolution.Common.Models.Responses;
using Evolution.AuditLog.Domain.Enums;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.AuditLog.Domain.Interfaces.Audit
{
    public interface IAuditSearchService
    {
        Response GetModuleAndSearchType(string module);

        Response AuditLog(object Models,
                                ref long? eventId,
                                string actionByUser,
                                string ModuleId,
                                SqlAuditActionType sqlAuditActionType,
                                SqlAuditModuleType sqlAuditModuleType,
                                object oldData,
                                object newData,
                                IList<DbModel.SqlauditModule> dbModules = null);

        IList<DbModel.SqlauditModule> GetAuditModule(IList<string> moduleList);
    }
}