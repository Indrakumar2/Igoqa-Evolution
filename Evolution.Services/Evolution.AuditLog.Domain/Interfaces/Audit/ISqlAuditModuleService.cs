using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Models.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Models.Responses;
using Evolution.AuditLog.Domain.Enums;

namespace Evolution.AuditLog.Domain.Interfaces.Audit
{
    public interface ISqlAuditModuleService
    {
        bool IsValidModule(IList<string> moduleNames, ref IList<SqlauditModule> dbModules, ref IList<string> moduleNameNotExists, ref IList<ValidationMessage> validationMessages);
        //Response AuditLog(object Models,
        //                        ref long? eventId,
        //                         string actionByUser,
        //                         string ModuleId,
        //                        SqlAuditActionType sqlAuditActionType,
        //                        SqlAuditModuleType sqlAuditModuleType,
        //                         object oldData,
        //                         object newData);
    }
}
