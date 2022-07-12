using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Contract.Domain.Interfaces.Contracts
{
    public  interface IContractInvoiceReferenceTypeService
    { 
        Response SaveContractInvoiceReferenceType(string contractNumber, IList<Models.Contracts.ContractInvoiceReferenceType> contractInvoiceReferenceTypes, bool commitChange = true, bool isResultSetRequired = false);

        Response SaveContractInvoiceReferenceType(string contractNumber, IList<Models.Contracts.ContractInvoiceReferenceType> contractInvoiceReferenceTypes, IList<DbModel.Contract> dbContract, IList<DbModel.Data> dbContractRef, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false);

        Response ModifyContractInvoiceReferenceType(string contractNumber, IList<Models.Contracts.ContractInvoiceReferenceType> contractInvoiceReferenceTypes, bool commitChange = true, bool isResultSetRequired = false);

        Response ModifyContractInvoiceReferenceType(string contractNumber, IList<Models.Contracts.ContractInvoiceReferenceType> contractInvoiceReferenceTypes, IList<DbModel.Contract> dbContract, IList<DbModel.Data> dbContractRef, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false);

        Response GetContractInvoiceReferenceType(Models.Contracts.ContractInvoiceReferenceType searchModel);

        Response DeleteContractInvoiceReferenceType(string contractNumber, IList<Models.Contracts.ContractInvoiceReferenceType> deleteModel,bool commitChange = true, bool isResultSetRequired = false);
        Response DeleteContractInvoiceReferenceType(string contractNumber, IList<Models.Contracts.ContractInvoiceReferenceType> deleteModel, IList<DbModel.Contract> dbContract, IList<DbModel.SqlauditModule> dbModule,bool commitChange = true,  bool isResultSetRequired = false);
    }
}
