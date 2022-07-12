using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Supplier.Domain.Interfaces.Data
{
    public interface ISupplierPerfomanceRepository : IGenericRepository<DbModel.VisitSupplierPerformance>
    {
        IList<DomainModel.SupplierPerformanceReport> Search(DomainModel.SupplierPerformanceReportsearch searchModel);
    }
}
