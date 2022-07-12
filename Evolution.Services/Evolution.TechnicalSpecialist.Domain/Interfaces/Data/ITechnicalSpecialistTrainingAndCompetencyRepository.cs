using Evolution.Common.Enums;
using Evolution.GenericDbRepository.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
    public interface ITechnicalSpecialistTrainingAndCompetencyRepository : IGenericRepository<DbModel.TechnicalSpecialistTrainingAndCompetency>
    {
        IList<DbModel.TechnicalSpecialistTrainingAndCompetency> Get(IList<int> Ids);
        IList<DbModel.TechnicalSpecialistTrainingAndCompetency> Get(IList<string> trainingOrCompetencyNames, CompCertTrainingType trainingAndCompetencyType);
        IList<DbModel.TechnicalSpecialistTrainingAndCompetency> Get(IList<KeyValuePair<string, string>> ePinAndTrainingOrCompetencyNames, CompCertTrainingType trainingAndCompetencyType);
        IList<DbModel.TechnicalSpecialistTrainingAndCompetency> GetByPinId(IList<string> pinIds);

        IList<DbModel.TechnicalSpecialistTrainingAndCompetency> Search(TechnicalSpecialistCompetency searchModel);
        IList<DbModel.TechnicalSpecialistTrainingAndCompetency> Search(TechnicalSpecialistInternalTraining searchModel);
    }
}