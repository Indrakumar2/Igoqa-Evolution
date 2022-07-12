using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using  Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
    public interface ITechnicalSpecialistEducationalQualificationRepository : IGenericRepository<DbModel.TechnicalSpecialistEducationalQualification>
    {
     
        IList<DbModel.TechnicalSpecialistEducationalQualification> Search(TechnicalSpecialistEducationalQualificationInfo model);

        IList<DbModel.TechnicalSpecialistEducationalQualification> Get(IList<int> EduQulificationIds);

        IList<DbModel.TechnicalSpecialistEducationalQualification> GetByPinId(IList<string> pinIds);

    }
}

