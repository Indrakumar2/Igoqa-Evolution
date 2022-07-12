using Evolution.Common.Models.Responses;
using Evolution.Project.Domain.Models.Projects;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;

namespace Evolution.Project.Domain.Interfaces.Projects
{
    public interface IProjectClientNotificationService
    {
        Response GetProjectClientNotifications(ProjectClientNotification searchModel);

        Response SaveProjectClientNotifications(int projectNumber, IList<ProjectClientNotification> projectClientNotifications, bool commitChange = true, int? projectId = null, bool isResultSetRequired = false);
        Response SaveProjectClientNotifications(int projectNumber, IList<ProjectClientNotification> projectClientNotifications, IList<DbModel.Project> dbProject, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, int? projectId = null, bool isResultSetRequired = false);
        Response ModifyProjectClientNotifications(int projectNumber, IList<ProjectClientNotification> projectClientNotifications, bool commitChange = true, int? projectId = null, bool isResultSetRequired = false);
        Response ModifyProjectClientNotifications(int projectNumber, IList<ProjectClientNotification> projectClientNotifications, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, int? projectId = null, bool isResultSetRequired = false);
        Response DeleteProjectClientNotifications(int projectNumber, IList<ProjectClientNotification> projectClientNotifications, bool commitChange = true, int? projectId = null, bool isResultSetRequired = false);
        Response DeleteProjectClientNotifications(int projectNumber, IList<ProjectClientNotification> projectClientNotifications, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, int? projectId = null, bool isResultSetRequired = false);
    }
}
