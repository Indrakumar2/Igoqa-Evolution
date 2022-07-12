using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using Evolution.Security.Domain.Models.Security;
using System.Collections.Generic;

namespace Evolution.Security.Domain.Interfaces.Security
{ 
    public interface IModuleService
    {
        Response Get(Models.Security.ModuleInfo searchModel);

        Response Get(string applicationName, IList<string> moduleNames);

        Response Add(IList<Models.Security.ModuleInfo> modules, bool commitChange = true,bool isValidationRequire=true);

        Response Add(IList<ModuleInfo> modules, ref IList<DbRepository.Models.SqlDatabaseContext.Module> dbModules, bool commitChange = true, bool isDbValidationRequire = true);

        Response Modify(IList<Models.Security.ModuleInfo> modules, bool commitChange = true, bool isValidationRequire = true);

        Response Modify(IList<ModuleInfo> modules, ref IList<DbRepository.Models.SqlDatabaseContext.Module> dbModules, bool commitChange = true, bool isDbValidationRequire = true);

        Response Delete(IList<ModuleInfo> moduleInfo, bool commitChange = true, bool isValidationRequire = true);

        Response IsRecordValidForProcess(IList<ModuleInfo> modules, ValidationType validationType);

        Response IsRecordValidForProcess(IList<ModuleInfo> modules, ValidationType validationType,ref IList<DbRepository.Models.SqlDatabaseContext.Module> dbModules);

        Response IsRecordValidForProcess(IList<ModuleInfo> modules, ValidationType validationType,IList<DbRepository.Models.SqlDatabaseContext.Module> dbModules);
    }
}