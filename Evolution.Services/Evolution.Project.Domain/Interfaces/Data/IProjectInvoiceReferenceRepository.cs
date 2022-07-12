using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using DomainModel = Evolution.Project.Domain.Models.Projects;

namespace Evolution.Project.Domain.Interfaces.Data
{
    /// <summary>
    /// Contains the predefined Functionalities for the Project Invoice Reference
    /// </summary>
    public interface IProjectInvoiceReferenceRepository : IGenericRepository<ProjectInvoiceAssignmentReference>
    {
        IList<DomainModel.ProjectInvoiceReference> Search(DomainModel.ProjectInvoiceReference searchModel);
    }
}
