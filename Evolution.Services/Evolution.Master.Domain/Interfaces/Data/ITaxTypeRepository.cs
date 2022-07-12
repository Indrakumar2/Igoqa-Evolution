using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Master.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Interfaces.Data
{
    public interface ITaxTypeRepository : IGenericRepository<Tax>
    {
        IList<Tax> Search(Tax search);
    }
}
