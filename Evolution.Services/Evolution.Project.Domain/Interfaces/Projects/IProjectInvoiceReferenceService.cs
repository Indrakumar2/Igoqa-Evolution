using Evolution.Common.Models.Responses;
using Evolution.Project.Domain.Models.Projects;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Project.Domain.Interfaces.Projects
{
    public interface IProjectInvoiceReferenceService
    {
        Response GetProjectInvoiceReferences(ProjectInvoiceReference searchModel);
         
        Response SaveProjectInvoiceReferences(int projectNumber, IList<ProjectInvoiceReference> projectInvoiceReferences, bool commitChange = true, bool isResultSetRequired = false);
         
        Response ModifyProjectInvoiceReferences(int projectNumber, IList<ProjectInvoiceReference> projectInvoiceReferences, bool commitChange = true, bool isResultSetRequired = false);
         
        Response DeleteProjectInvoiceReferences(int projectNumber, IList<ProjectInvoiceReference> projectInvoiceReferences, bool commitChange = true, bool isResultSetRequired = false);

        Response SaveProjectInvoiceReferences(int projectNumber, IList<ProjectInvoiceReference> projectInvoiceReferences, IList<DbModel.Project> dbProjects, IList<DbModel.Data> dbReference, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false);

        Response ModifyProjectInvoiceReferences(int projectNumber, IList<ProjectInvoiceReference> projectInvoiceReferences, IList<DbModel.Project> dbProjects, IList<DbModel.Data> dbReference, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false);

        Response DeleteProjectInvoiceReferences(int projectNumber, IList<ProjectInvoiceReference> projectInvoiceReferences, IList<DbModel.Project> dbProjects, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false);

    }
}
