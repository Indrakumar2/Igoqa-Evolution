using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Project.Domain.Interfaces.Data
{
    public interface IProjectMessageRepository : IGenericRepository<DbModel.ProjectMessage>
    {
    }
}
