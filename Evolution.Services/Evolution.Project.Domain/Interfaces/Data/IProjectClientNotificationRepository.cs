using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using DomainModel = Evolution.Project.Domain.Models.Projects;

namespace Evolution.Project.Domain.Interfaces.Data
{
    public interface IProjectClientNotificationRepository : IGenericRepository<ProjectClientNotification>
    {
        IList<DomainModel.ProjectClientNotification> Search(DomainModel.ProjectClientNotification searchModel);
    }
}
