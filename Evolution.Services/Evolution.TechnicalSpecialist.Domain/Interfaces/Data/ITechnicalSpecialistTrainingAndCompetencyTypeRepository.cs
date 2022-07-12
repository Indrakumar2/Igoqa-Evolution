using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
    public interface ITechnicalSpecialistTrainingAndCompetencyTypeRepository : IGenericRepository<DbModel.TechnicalSpecialistTrainingAndCompetencyType>
    {
        IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> Search(DomainModel.TechnicalSpecialistInternalTrainingAndCompetencyType model);

        IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> Get(IList<int> typeIds); 

        IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> Get(IList<string> typeName); 

        IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> Get(IList<KeyValuePair<int, string>> trainingOrCompetencyIdAndTypeName);
    }
}