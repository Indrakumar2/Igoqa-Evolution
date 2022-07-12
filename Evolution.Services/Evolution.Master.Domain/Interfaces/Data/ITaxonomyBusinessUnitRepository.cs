using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Master.Domain.Models;

namespace Evolution.Master.Domain.Interfaces.Data
{
   public interface ITaxonomyBusinessUnitRepository :IGenericRepository<DbModel.TaxonomyBusinessUnit>
    {
        IList<DomainModel.TaxonomyBusinessUnit> Search(DomainModel.TaxonomyBusinessUnit Search);

    }
}
