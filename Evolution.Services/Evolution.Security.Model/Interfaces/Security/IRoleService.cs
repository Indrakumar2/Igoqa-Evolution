using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using Evolution.Security.Domain.Models.Security;
using System.Collections.Generic;

namespace Evolution.Security.Domain.Interfaces.Security
{
    public interface IRoleService
    {
        Response Get(Models.Security.RoleInfo searchModel);

        Response Get(string applicationName, IList<string> roleNames);

        Response Add(IList<Models.Security.RoleInfo> roles, ref long? eventId, bool commitChange = true, bool isValidationRequire = true); // Manage Security Audit changes

        Response Add(IList<RoleInfo> roles, ref IList<DbRepository.Models.SqlDatabaseContext.Role> dbRoles, ref long? eventId, bool commitChange = true, bool isDbValidationRequire = true); // Manage Security Audit changes

        Response Modify(IList<Models.Security.RoleInfo> roles, ref long? eventId, bool commitChange = true, bool isValidationRequire = true);

        Response Modify(IList<RoleInfo> roles, ref IList<DbRepository.Models.SqlDatabaseContext.Role> dbRoles, ref long? eventId, bool commitChange = true, bool isDbValidationRequire = true);

        Response Delete(IList<RoleInfo> roleInfo, bool commitChange = true, bool isValidationRequire = true);

        Response IsRecordValidForProcess(IList<RoleInfo> roles, ValidationType validationType);

        Response IsRecordValidForProcess(IList<RoleInfo> roles, ValidationType validationType, ref IList<DbRepository.Models.SqlDatabaseContext.Role> dbRoles);

        Response IsRecordValidForProcess(IList<RoleInfo> roles, ValidationType validationType, IList<DbRepository.Models.SqlDatabaseContext.Role> dbRoles);

        Response IsRecordExistInDb(IList<KeyValuePair<string, string>> appRoleNames,
                                   ref IList<DbRepository.Models.SqlDatabaseContext.Role> dbRole,
                                   ref IList<string> activityNotExists);
    }
}
