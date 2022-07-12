using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using Evolution.Security.Domain.Models.Security;
using System.Collections.Generic;

namespace Evolution.Security.Domain.Interfaces.Security
{

    public interface IActivityService
    {
        Response Get(Models.Security.ActivityInfo searchModel);

        Response Get(string applicationName, IList<string> activityNames);

        Response Add(IList<Models.Security.ActivityInfo> activities, bool commitChange = true, bool isValidationRequire = true);

        Response Add(IList<ActivityInfo> activities, ref IList<DbRepository.Models.SqlDatabaseContext.Activity> dbActivities, bool commitChange = true, bool isDbValidationRequire = true);

        Response Modify(IList<Models.Security.ActivityInfo> activities, bool commitChange = true, bool isValidationRequire = true);

        Response Modify(IList<ActivityInfo> activities, ref IList<DbRepository.Models.SqlDatabaseContext.Activity> dbActivities, bool commitChange = true, bool isDbValidationRequire = true);

        Response Delete(IList<ActivityInfo> activityInfo, bool commitChange = true, bool isValidationRequire = true);

        Response IsRecordValidForProcess(IList<ActivityInfo> activities, ValidationType validationType);

        Response IsRecordValidForProcess(IList<ActivityInfo> activities, ValidationType validationType, ref IList<DbRepository.Models.SqlDatabaseContext.Activity> dbActivities);

        Response IsRecordValidForProcess(IList<ActivityInfo> activities, ValidationType validationType, IList<DbRepository.Models.SqlDatabaseContext.Activity> dbActivities);

        Response IsRecordExistInDb(IList<KeyValuePair<string, string>> appActivityNames,
                                   ref IList<DbRepository.Models.SqlDatabaseContext.Activity> dbActivity,
                                   ref IList<string> activityNotExists);
    }
}