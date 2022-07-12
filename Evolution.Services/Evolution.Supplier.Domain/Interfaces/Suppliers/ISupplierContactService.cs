using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.SupplierContacts.Domain.Interfaces.Suppliers
{
    public interface ISupplierContactService
    {
        Response Get(DomainModel.SupplierContact searchModel);

        Response Add(IList<DomainModel.SupplierContact> supplierContacts,
                            bool commitChange = true,
                            bool isDbValidationRequire = true);

        Response Add(IList<DomainModel.SupplierContact> supplierContacts,
            IList<DbModel.SqlauditModule> dbModule,
                            ref IList<DbModel.SupplierContact> dbSupplierContacts,
                            ref IList<DbModel.Supplier> dbSuppliers,
                            bool commitChange = true,
                            bool isDbValidationRequire = true);

        Response Modify(IList<DomainModel.SupplierContact> supplierContacts,
                                bool commitChange = true,
                                bool isDbValidationRequire = true);

        Response Modify(IList<DomainModel.SupplierContact> supplierContacts,
            IList<DbModel.SqlauditModule> dbModule,
                                ref IList<DbModel.SupplierContact> dbSupplierContacts,
                                ref IList<DbModel.Supplier> dbSuppliers,
                                bool commitChange = true,
                                bool isDbValidationRequire = true);

        Response Delete(IList<DomainModel.SupplierContact> supplierContacts,
                               bool commitChange = true,
                               bool isDbValidationRequire = true);

        Response Delete(IList<DomainModel.SupplierContact> supplierContacts,
            IList<DbModel.SqlauditModule> dbModule,
                               ref IList<DbModel.SupplierContact> dbSupplierContacts,
                               ref IList<DbModel.Supplier> dbSuppliers,
                               bool commitChange = true,
                               bool isDbValidationRequire = true);

        Response IsRecordValidForProcess(IList<DomainModel.SupplierContact> supplierContacts,
                                              ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.SupplierContact> supplierContacts,
                                                ValidationType validationType,
                                                ref IList<DbModel.SupplierContact> dbSupplierContacts,
                                                ref IList<DbModel.Supplier> dbSuppliers);

        Response IsRecordValidForProcess(IList<DomainModel.SupplierContact> supplierContacts,
                                                 ValidationType validationType,
                                                 IList<DbModel.SupplierContact> dbSupplierContacts,
                                                 IList<DbModel.Supplier> dbSuppliers);
    }
}