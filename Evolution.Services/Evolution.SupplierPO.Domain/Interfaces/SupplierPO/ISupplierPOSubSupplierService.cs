using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DomainModels = Evolution.SupplierPO.Domain.Models.SupplierPO;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Models.Messages;
using Evolution.Common.Enums;
using System.Linq.Expressions;
using System;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.SupplierPO.Domain.Models.SupplierPO;


namespace Evolution.SupplierPO.Domain.Interfaces.SupplierPO
{
    public interface ISupplierPOSubSupplierService
    {        
        Response Get(DomainModels.SupplierPOSubSupplier searchModel);

        Response Add(int supplierPoId,
                     IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                     bool commitChange = true,
                     bool isDbValildationRequired = true);

        Response Add(int supplierPoId,
                            IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                            IList<DbModel.SqlauditModule> dbModule,
                            ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                            ref IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                            ref IList<DbModels.Supplier> dbSuppliers,
                            bool commitChange = true,
                            bool isDbValidationRequired = true);

        Response Modify(int supplierPoId,
                               IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                               bool commitChange = true,
                               bool isDbvalidationRequired = true);

        Response Modify(int supplierPoId,
                               IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                               IList<DbModel.SqlauditModule> dbModule,
                               ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                               ref IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                               ref IList<DbModels.Supplier> dbSuppliers,
                               bool commitChange = true,
                               bool isDbvalidationRequired = true);

        Response Modify(int supplierPoId,
                             IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                             IList<DomainModel.SupplierPOSubSupplier> subSupplierlist,
                             IList<DbModel.SqlauditModule> dbModule,
                             ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                             ref IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                             ref IList<DbModels.Supplier> dbSuppliers,
                             bool commitChange = true,
                             bool isDbvalidationRequired = true);

        Response Delete(int supplierPoId,
                               IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                               bool commitChange = true,
                               bool isDbvalidationRequired = true);

        Response Delete(int supplierPoId,
                             IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                             IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                             IList<DbModel.SqlauditModule> dbModule,
                             ref IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                             ref IList<DbModels.Supplier> dbSuppliers,
                             bool commitChange = true,
                             bool isDbvalidationRequired = true);

        Response IsRecordValidForProcess(int supplierPoId,
                                                IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                                                ValidationType validationType);

        Response IsRecordValidForProcess(int supplierPoId,
                                               IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                                               ValidationType validationType,
                                               ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                               ref IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                                               ref IList<DbModels.Supplier> dbSuppliers);

        Response IsRecordValidForProcess(int supplierPoId,
                                                IList<DomainModels.SupplierPOSubSupplier> subSuppliers,
                                                ValidationType validationType,
                                                IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                                IList<DbModels.SupplierPurchaseOrder> dbSupplierPos,
                                                IList<DbModels.Supplier> dbSuppliers);

        bool IsValidSubSupplier(IList<int> subSupplierId, 
                                ref IList<DbModels.Assignment> dbAssignments, 
                                ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                                ref IList<DbModels.Supplier> dbSuppliers,
                                ref IList<ValidationMessage> messages, 
                                params Expression<Func<DbModels.SupplierPurchaseOrderSubSupplier, object>>[] includes);

        bool IsValidSubSupplierID(IList<int> subSupplierId,
                        ref IList<DbModels.Assignment> dbAssignments,
                        ref IList<DbModels.SupplierPurchaseOrderSubSupplier> dbSubSuppliers,
                        ref IList<DbModels.Supplier> dbSuppliers,
                        ref IList<ValidationMessage> messages,
                        params Expression<Func<DbModels.SupplierPurchaseOrderSubSupplier, object>>[] includes);

        //bool IsValidSupplier(IList<int> supplierIds,
        //                    ref IList<DbModels.Supplier> dbSupplier,
        //                    ref IList<ValidationMessage> messages);


    }
}