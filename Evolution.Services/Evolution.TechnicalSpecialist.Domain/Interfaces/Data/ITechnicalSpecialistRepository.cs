using Evolution.GenericDbRepository.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
    public interface ITechnicalSpecialistRepository : IGenericRepository<DbModel.TechnicalSpecialist>
    {
        IList<BaseTechnicalSpecialistInfo> Search(string companyCode, string logonName = null, IList<int> ePins = null);
        
        IList<TechnicalSpecialistInfo> Search(BaseTechnicalSpecialistInfo model,
                                              params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes);

        IList<DbModel.TechnicalSpecialist> Search(TechnicalSpecialistInfo model, string[] includes);

        IList<DbModel.TechnicalSpecialist> Search(TechnicalSpecialistInfo model,
                                                  params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes);

        IList<DbModel.TechnicalSpecialist> Get(IList<int> ids,
                                               params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes);

        IList<DbModel.TechnicalSpecialist> Get(IList<string> pins,
                                               params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes);

        IList<TechnicalSpecialistExpiredRecord> GetExpiredRecords();

        //D946 CR Start
        void Update(List<KeyValuePair<DbModel.TechnicalSpecialist, List<KeyValuePair<string, object>>>> technicalSpecialist, params Expression<Func<DbModel.TechnicalSpecialist, object>>[] updatedProperties);
    }
}
