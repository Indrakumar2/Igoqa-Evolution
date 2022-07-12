using Evolution.Common.Models.Responses;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Supplier.Domain.Interfaces.Suppliers
{
    public interface ISupplierDetailService
    {
        Response Add(DomainModel.SupplierDetail supplierDetail, bool commitChange = true);

        Response Modify(DomainModel.SupplierDetail supplierDetail, bool commitChange = true);

        Response Delete(DomainModel.SupplierDetail supplierDetail, bool commitChange = true);
    }
}
