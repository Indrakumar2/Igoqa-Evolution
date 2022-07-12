using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
   public interface ITechnicalSpecialistTimeOffRequestRepository : IGenericRepository<DbModel.TechnicalSpecialistTimeOffRequest>

    {
        IList<DomainModel.TechnicalSpecialistTimeOffRequest> Search(DomainModel.TechnicalSpecialistTimeOffRequest model, params Expression<Func<DbModel.TechnicalSpecialistTimeOffRequest, object>>[] includes);

    }
}
