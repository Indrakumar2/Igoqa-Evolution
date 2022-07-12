using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModels = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.SupplierPO.Domain.Interfaces.Data
{
    public interface ISupplierPOSubSupplierRepository : IGenericRepository<DbModels.SupplierPurchaseOrderSubSupplier>
    {
        //IList<DomainModels.SupplierPOSubSupplier> Search(int supplierPoId, DomainModels.SupplierPOSubSupplier searchModel);
        IList<DbModels.SupplierPurchaseOrderSubSupplier> Search(DomainModels.SupplierPOSubSupplier model,
                                                       params Expression<Func<DbModels.SupplierPurchaseOrderSubSupplier, object>>[] includes);

        bool IsValidSupplierContact(int supplierPoId, List<int?> supplierIds, List<int?> supplierContactIds);
    }
}
