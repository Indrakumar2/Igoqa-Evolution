using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Domain.Interfaces.Data
{
    public interface IVisitSupplierPerformanceRepository : IGenericRepository<DbModel.VisitSupplierPerformance>, IDisposable
    {
        IList<DomainModel.VisitSupplierPerformanceType> Search(DomainModel.VisitSupplierPerformanceType searchModel);

        int DeleteVisitSupplierPerformanceType(List<DomainModel.VisitSupplierPerformanceType> visitReferences);
    }
}
