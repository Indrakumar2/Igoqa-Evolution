using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Supplier.Domain.Interfaces.Data
{
    public interface ISupplierRepository : IGenericRepository<DbModel.Supplier>
    {
        // IList<DbModel.Supplier> Search(DomainModel.SupplierSearch searchModel, params Expression<Func<DbModel.Supplier, object>>[] includes);
        IList<DomainModel.Supplier> Search(DomainModel.SupplierSearch searchModel, params Expression<Func<DbModel.Supplier, object>>[] includes);

        IList<DbModel.Supplier> Get(IList<int> supplierIds, params Expression<Func<DbModel.Supplier, object>>[] includes);

        IList<DbModel.Supplier> Get(IList<string> supplierNames, params Expression<Func<DbModel.Supplier, object>>[] includes);

        int DeleteSupplier(int supplierId);
    }
}