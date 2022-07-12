using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Supplier.Domain.Interfaces.Suppliers
{
    public interface ISupplierNoteService
    {
        Response Get(DomainModel.SupplierNote searchModel);

        Response Add(IList<DomainModel.SupplierNote> supplierNotes,
                          bool commitChange = true,
                          bool isDbValidationRequire = true);

        Response Add(IList<DomainModel.SupplierNote> supplierNotes,
            IList<DbModel.SqlauditModule> dbModule,
                             ref IList<DbModel.SupplierNote> dbSupplierNotes,
                             ref IList<DbModel.Supplier> dbSuppliers,
                             bool commitChange = true,
                             bool isDbValidationRequire = true);

        Response Delete(IList<DomainModel.SupplierNote> supplierNotes,
                               bool commitChange = true,
                               bool isDbValidationRequired = true,
                               int? assignmentIds = null);

        Response Delete(IList<DomainModel.SupplierNote> supplierNotes,
            IList<DbModel.SqlauditModule> dbModule,
                               ref IList<DbModel.SupplierNote> dbSupplierNotes,
                               ref IList<DbModel.Supplier> dbSupplier,
                                bool commitChange = true,
                               bool isDbValidationRequired = true,
                               int? assignmentIds = null);
    //D661 issue 8 Start
        Response Update(IList<DomainModel.SupplierNote> supplierNotes,
            IList<DbModel.SqlauditModule> dbModule,
                             ref IList<DbModel.SupplierNote> dbSupplierNotes,
                             ref IList<DbModel.Supplier> dbSupplier,
                              bool commitChange = true,
                             bool isDbValidationRequired = true);
    //D661 issue 8 End
        Response IsRecordValidForProcess(IList<DomainModel.SupplierNote> supplierNotes,
                                                     ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.SupplierNote> supplierNotes,
                                                ValidationType validationType,
                                                ref IList<DbModel.SupplierNote> dbSupplierNotes,
                                                ref IList<DbModel.Supplier> dbSuppliers);

        Response IsRecordValidForProcess(IList<DomainModel.SupplierNote> supplierNotes,
                                                ValidationType validationType,
                                                IList<DbModel.SupplierNote> dbSupplierNotes,
                                                IList<DbModel.Supplier> dbSuppliers);
    }
}