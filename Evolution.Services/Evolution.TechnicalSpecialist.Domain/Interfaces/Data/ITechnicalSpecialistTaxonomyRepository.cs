using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
   public interface ITechnicalSpecialistTaxonomyRepository : IGenericRepository<DbModel.TechnicalSpecialistTaxonomy>
    {
        IList<DbModel.TechnicalSpecialistTaxonomy> Search(DomainModel.TechnicalSpecialistTaxonomyInfo model);

        bool IsTaxonomyHistoryExists(int epin); //D684

        IList<DbModel.TechnicalSpecialistTaxonomy> Get(IList<int> taxonomyIds);

        IList<DbModel.TechnicalSpecialistTaxonomy> GetByPinId(IList<string> pinIds);      
       
    }
}