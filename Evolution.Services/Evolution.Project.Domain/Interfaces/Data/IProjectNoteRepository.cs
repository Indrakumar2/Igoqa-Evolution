using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System;
using System.Collections.Generic;
using System.Text;
using DomainModel=Evolution.Project.Domain.Models.Projects;

namespace Evolution.Project.Domain.Interfaces.Data
{
   
    public interface IProjectNoteRepository : IGenericRepository<ProjectNote>
    {
        IList<DomainModel.ProjectNote> Search(DomainModel.ProjectNote searchModel);

    }
}
