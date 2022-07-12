using Evolution.Common.Models.Responses;
using DomainModel = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.SupplierPO.Domain.Interfaces.SupplierPO
{
    public interface ISupplierPODetailService
    {
        Response Add(DomainModel.SupplierPODetail supplierPODetail, bool commitChange = true);

        Response Modify(DomainModel.SupplierPODetail supplierPODetail, bool commitChange = true);

        Response Delete(DomainModel.SupplierPODetail supplierPODetail, bool commitChange = true);
    }
}