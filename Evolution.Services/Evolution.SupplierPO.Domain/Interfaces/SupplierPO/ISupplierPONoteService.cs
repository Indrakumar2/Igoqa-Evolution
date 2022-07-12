using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using DomainModel = Evolution.SupplierPO.Domain.Models.SupplierPO;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using System.Linq.Expressions;

namespace Evolution.SupplierPO.Domain.Interfaces.SupplierPO
{
    public interface ISupplierPONoteService
    {
        Response Get(DomainModel.SupplierPONote searchModel);

        Response Add(IList<DomainModel.SupplierPONote> supplierPONotes,
                            bool commitChange = true,
                            bool isDbValidationRequire = true);

        Response Add(IList<DomainModel.SupplierPONote> supplierPONotes,
            IList<DbModel.SqlauditModule> dbModule,
                              ref IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONotes,
                              ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                              bool commitChange = true,
                              bool isDbValidationRequire = true);

        Response Delete(IList<DomainModel.SupplierPONote> supplierPONotes,
                             bool commitChange = true,
                             bool isDbValidationRequired = true,
                             int? supplierPos = null);

        Response Delete(IList<DomainModel.SupplierPONote> supplierNotes,
                               IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONotes,
                               IList<DbModel.SqlauditModule> dbModule,
                               ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                bool commitChange = true,
                               bool isDbValidationRequired = true);
       //D661 issue 8 Start
        Response Update(IList<DomainModel.SupplierPONote> supplierPONotes,
            IList<DbModel.SqlauditModule> dbModule,
                               ref IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONotes,
                               ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                               bool commitChange = true,
                               bool isDbValidationRequired = true);
       //D661 issue 8 End
        Response IsRecordValidForProcess(IList<DomainModel.SupplierPONote> supplierPONotes,
                                                ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.SupplierPONote> supplierPONotes,
                                                ValidationType validationType,
                                                ref IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONotes,
                                                ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs);

        Response IsRecordValidForProcess(IList<DomainModel.SupplierPONote> supplierPONotes,
                                                ValidationType validationType,
                                                IList<DbModel.SupplierPurchaseOrderNote> dbSupplierPONotes,
                                                IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs);
    }
}
