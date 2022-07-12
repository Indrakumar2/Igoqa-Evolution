using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Assignment.Domain.Interfaces.Data
{
    public interface IAssignmentSubSupplerTSRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.AssignmentSubSupplierTechnicalSpecialist>, IDisposable
    {
        IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> Search(DomainModel.AssignmentSubSupplierTechnicalSpecialist model, params Expression<Func<DbModel.AssignmentSubSupplierTechnicalSpecialist, object>>[] includes);
    }
}
