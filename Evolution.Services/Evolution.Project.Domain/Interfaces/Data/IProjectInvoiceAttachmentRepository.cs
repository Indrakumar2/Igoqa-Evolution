using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using DomainModel = Evolution.Project.Domain.Models.Projects;

namespace Evolution.Project.Domain.Interfaces.Data
{
    /// <summary>
    /// Contains the predefined functionality for Project Invoice Attachment
    /// </summary>
    public interface IProjectInvoiceAttachmentRepository : IGenericRepository<ProjectInvoiceAttachment>
    {
        IList<DomainModel.ProjectInvoiceAttachment> Search(DomainModel.ProjectInvoiceAttachment searchModel);
    }
}
