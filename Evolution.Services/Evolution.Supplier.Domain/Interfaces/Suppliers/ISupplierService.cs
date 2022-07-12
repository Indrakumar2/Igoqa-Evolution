using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Supplier.Domain.Interfaces.Suppliers
{
    public interface ISupplierService
    {
        Task<Response> Get(DomainModel.SupplierSearch searchModel);

        Response Get(IList<string> supplierNames);

        Response Get(IList<int> supplierIds);

        Response Add(IList<DomainModel.Supplier> suppliers,
                     bool commitChange = true,
                     bool isDbValidationRequire = true);

        Response Add(IList<DomainModel.Supplier> suppliers,
            IList<DbModel.SqlauditModule> dbModule,
                     ref IList<DbModel.Supplier> dbSuppliers,
                     ref IList<DbModel.Country> dbCountries,
                     ref IList<DbModel.County> dbCounties,
                     ref IList<DbModel.City> dbCities,
                     ref long? eventId,
                     bool commitChange = true,
                     bool isDbValidationRequire = true);

        Response Modify(IList<DomainModel.Supplier> suppliers,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Modify(IList<DomainModel.Supplier> suppliers,
            IList<DbModel.SqlauditModule> dbModule,
                        ref IList<DbModel.Supplier> dbSuppliers,
                        ref IList<DbModel.Country> dbCountries,
                        ref IList<DbModel.County> dbCounties,
                        ref IList<DbModel.City> dbCities,
                        ref long? eventId,
                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response Delete(IList<DomainModel.Supplier> suppliers,
                              bool commitChange = true,
                              bool isDbValidationRequire = true);

        Response Delete(IList<DomainModel.Supplier> suppliers,
            IList<DbModel.SqlauditModule> dbModule,
                               ref IList<DbModel.Supplier> dbSuppliers,
                               ref long? eventId,
                               bool commitChange = true,
                               bool isDbValidationRequire = true);

        Response IsRecordValidForProcess(IList<DomainModel.Supplier> suppliers,
                                         ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.Supplier> suppliers,
                                         ValidationType validationType,
                                         ref IList<DbModel.Supplier> DbSuppliers,
                                         ref IList<DbModel.Country> dbCountries,
                                         ref IList<DbModel.County> dbCounties,
                                         ref IList<DbModel.City> dbCities);

        Response IsRecordValidForProcess(IList<DomainModel.Supplier> suppliers,
                                         ValidationType validationType,
                                         IList<DbModel.Supplier> DbSuppliers,
                                         IList<DbModel.Country> dbCountries,
                                         IList<DbModel.County> dbCounties,
                                         IList<DbModel.City> dbCities);

        Response IsRecordExistInDb(IList<int> supplierIds,
                                   ref IList<DbModel.Supplier> dbSuppliers,
                                   ref IList<ValidationMessage> validationMessages,
                                   params Expression<Func<DbModel.Supplier, object>>[] includes);

        Response IsRecordExistInDb(IList<int> supplierIds,
                                   ref IList<DbModel.Supplier> dbSuppliers,
                                   ref IList<int> supplierIdNotExists,
                                   ref IList<ValidationMessage> validationMessages,
                                   params Expression<Func<DbModel.Supplier, object>>[] includes);

    }
}