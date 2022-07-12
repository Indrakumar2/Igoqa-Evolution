using Evolution.GenericDbRepository.Interfaces;
using Evolution.SupplierPO.Domain.Models.SupplierPO;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.SupplierPO.Domain.Interfaces.Data
{
    public interface ISupplierPORepository : IGenericRepository<DbModel.SupplierPurchaseOrder>
    {
        List<DomainModel.SupplierPO> Search(DomainModel.SupplierPOSearch searchModel);

        List<T> Search<T>(DomainModel.SupplierPOSearch searchModel);

        List<DomainModel.SupplierPO> SearchSupplierPO(DomainModel.SupplierPOSearch searchModel);

        int DeleteSupplierPO(int supplierPOId);

        Result SearchSupplierPO<T>(DomainModel.SupplierPOSearch searchModel);
    }
}
