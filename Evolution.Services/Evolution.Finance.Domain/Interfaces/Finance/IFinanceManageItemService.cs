using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Finance.Domain.Interfaces.Data
{
    public interface IFinanceManageItemService
    {
        // Response SaveProjects(IList<Models.Projects.Project> projects, ref long? eventId, bool commitChange = true);

        // Task<Response> GetProjects(Models.Projects.ProjectSearch searchModel, AdditionalFilter filter = null);

        // Task<Response> SearchProjects(Models.Projects.ProjectSearch searchModel);

        // Response ModifyProjects(IList<Models.Projects.Project> projects, ref long? eventId, bool commitChange = true);

        // Response DeleteProjects(IList<Models.Projects.Project> projects, ref long? eventId, bool commitChange = true);

        // Response ProjectValidForDeletion(IList<Models.Projects.Project> projects, ref IList<DbModel.Project> dbProjects);

    }
}
