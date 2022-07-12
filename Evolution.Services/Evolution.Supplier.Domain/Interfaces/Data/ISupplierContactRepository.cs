using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Supplier.Domain.Interfaces.Data
{
    public interface ISupplierContactRepository: IGenericRepository<DbModel.SupplierContact>
    {
        IList<DbModel.SupplierContact> Search(DomainModel.SupplierContact searchModel, params Expression<Func<DbModel.SupplierContact, object>>[] includes);

        IList<DbModel.SupplierContact> Get(IList<int> supplierIds, params Expression<Func<DbModel.SupplierContact, object>>[] includes);
    }
}
