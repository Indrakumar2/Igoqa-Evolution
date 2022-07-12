using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;


namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
    public interface ITechnicalSpecialistTaxonomyHistoryRepository :IGenericRepository<DbModel.TechnicalSpecialistTaxonomyHistory>
    {

    }
}
