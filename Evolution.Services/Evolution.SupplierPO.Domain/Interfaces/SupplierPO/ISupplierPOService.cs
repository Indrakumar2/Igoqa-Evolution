using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using DomainModel= Evolution.SupplierPO.Domain.Models.SupplierPO;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using System.Linq.Expressions;
using Evolution.Common.Models.Filters;
using System.Threading.Tasks;

namespace Evolution.SupplierPO.Domain.Interfaces.SupplierPO
{
    public interface ISupplierPOService
    {

        Task<Response> GetAsync(DomainModel.SupplierPOSearch searchModel);

        Response Get(DomainModel.SupplierPOSearch searchModel, AdditionalFilter filter = null);

        Response Get(IList<int> supplierPOIds);

        Response GetSupplierPO(DomainModel.SupplierPOSearch searchModel);

        Response Add(IList<DomainModel.SupplierPO> supplierPOs,
                            bool commitChange = true,
                            bool isValidationRequire = true);

        Response Add(IList<DomainModel.SupplierPO> supplierPOs,
             IList<DbModel.SqlauditModule> dbModule,
                            ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                            ref IList<DbModel.Supplier> dbSuppliers,
                            ref IList<DbModel.Project> dbProjects,
                            ref long? eventId,
                            bool commitChange = true,
                            bool isDbValidationRequire = true);

        Response Modify(IList<DomainModel.SupplierPO> supplierPOs,
                               bool commitChange = true,
                               bool isValidationRequire = true);

        Response Modify(IList<DomainModel.SupplierPO> supplierPOs,
             IList<DbModel.SqlauditModule> dbModule,
                               ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                               ref IList<DbModel.Supplier> dbSuppliers,
                               ref IList<DbModel.Project> dbProjects,
                               ref long? eventId,
                               bool commitChange = true,
                               bool isDbValidationRequire = true);

        Response Delete(IList<DomainModel.SupplierPO> supplierPOs,
                               bool commitChange = true,
                               bool isValidationRequire = true);

        Response Delete(IList<DomainModel.SupplierPO> supplierPOs,
            IList<DbModel.SqlauditModule> dbModule,
                               ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                               ref IList<DbModel.Supplier> dbSuppliers,
                               ref IList<DbModel.Project> dbProjects,
                               ref long? eventId,
                               bool commitChange = true, 
                               bool isValidationRequire = true);

        Response IsRecordValidForProcess(IList<DomainModel.SupplierPO> supplierPOs,
                                                 ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.SupplierPO> supplierPOs,
                                                ValidationType validationType,
                                                ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                                ref IList<DbModel.Supplier> dbSuppliers,
                                                ref IList<DbModel.Project> dbProjects);

        Response IsRecordValidForProcess(IList<DomainModel.SupplierPO> supplierPOs,
                                                ValidationType validationType,
                                                IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                                IList<DbModel.Supplier> dbSuppliers,
                                                IList<DbModel.Project> dbProjects);

        Response IsRecordExistInDb(IList<int> supplierPOIds,
                                ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                ref IList<ValidationMessage> validationMessages,
                                params Expression<Func<DbModel.SupplierPurchaseOrder, object>>[] includes);

        Response IsRecordExistInDb(IList<int> supplierPOIds,
                                   ref IList<DbModel.SupplierPurchaseOrder> dbSupplierPOs,
                                   ref IList<int> supplierPOIdNotExists,
                                   ref IList<ValidationMessage> validationMessages,
                                   params Expression<Func<DbModel.SupplierPurchaseOrder, object>>[] includes);
    }
}