using Evolution.Common.Models.Responses;
using Evolution.Project.Domain.Models.Projects;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Project.Domain.Interfaces.Projects
{
    public interface IProjectInvoiceAttachmentService
    {
        Response GetProjectInvoiceAttachments(ProjectInvoiceAttachment searchModel);

        Response SaveProjectInvoiceAttachments(int projectNumber,IList<ProjectInvoiceAttachment> invoiceAttachmentTypes, bool commitChange = true, bool isResultSetRequired = false);

        Response ModifyProjectInvoiceAttachments(int projectNumber, IList<ProjectInvoiceAttachment> invoiceAttachmentTypes, bool commitChange = true, bool isResultSetRequired = false);

        Response DeleteProjectInvoiceAttachments(int projectNumber, IList<ProjectInvoiceAttachment> invoiceAttachmentTypes, bool commitChange = true, bool isResultSetRequired = false);

        Response SaveProjectInvoiceAttachments(int projectNumber, IList<ProjectInvoiceAttachment> invoiceAttachmentTypes, IList<DbModel.Project> dbProjects, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false);

        Response ModifyProjectInvoiceAttachments(int projectNumber, IList<ProjectInvoiceAttachment> invoiceAttachmentTypes, IList<DbModel.Project> dbProjects, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false);

        Response DeleteProjectInvoiceAttachments(int projectNumber, IList<ProjectInvoiceAttachment> invoiceAttachmentTypes, IList<DbModel.Project> dbProjects, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false);

    }
}
