using Evolution.Common.Enums;
using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
    public interface ITechnicalSpecialistCertificationAndTrainingRepository : IGenericRepository<DbModel.TechnicalSpecialistCertificationAndTraining>
    {
        IList<DbModel.TechnicalSpecialistCertificationAndTraining> Search(DomainModel.TechnicalSpecialistCertification searchModel);

        IList<DbModel.TechnicalSpecialistCertificationAndTraining> Search(DomainModel.TechnicalSpecialistTraining searchModel);

        IList<DbModel.TechnicalSpecialistCertificationAndTraining> GetByPinId(IList<string> pinIds);

        IList<DbModel.TechnicalSpecialistCertificationAndTraining> Get(IList<int> Ids);
         
        IList<DbModel.TechnicalSpecialistCertificationAndTraining> Get(IList<string> certificationOrTraingNames, CompCertTrainingType certificateOrTrainingType); 

        IList<DbModel.TechnicalSpecialistCertificationAndTraining> Get(IList<KeyValuePair<string, string>> ePinAndCertificationOrTraingNames, CompCertTrainingType certificateOrTrainingType);

        void Update(List<KeyValuePair<DbModel.TechnicalSpecialistCertificationAndTraining, List<KeyValuePair<string, object>>>> technicalSpecialist, params Expression<Func<DbModel.TechnicalSpecialistCertificationAndTraining, object>>[] updatedProperties);
    }


}
