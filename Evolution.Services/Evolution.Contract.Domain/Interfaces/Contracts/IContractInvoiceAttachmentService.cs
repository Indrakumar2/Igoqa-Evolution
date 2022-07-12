using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Contract.Domain.Interfaces.Contracts
{
    public  interface IContractInvoiceAttachmentService
    {
        /// <summary>
        /// Save ContractinvoiceAttachments
        /// </summary>
        /// <param name="contractInvoiceAttachments">List of ContractinvoiceAttachments</param>
        /// <returns>All the Saved ContractinvoiceAttachments Details</returns>
        Response SaveContractInvoiceAttachment(string contractNumber, IList<DomainModel.ContractInvoiceAttachment> contractInvoiceAttachments, bool commitChange = true, bool isResultSetRequired = false);

        Response SaveContractInvoiceAttachment(string contractNumber, IList<DomainModel.ContractInvoiceAttachment> contractInvoiceAttachments, IList<DbModel.Contract> dbContracts, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false);
        /// <summary>
        /// Modify list of ContractinvoiceAttachments
        /// </summary>
        /// <param name="contractInvoiceAttachments">List of ContractinvoiceAttachments which need to update.</param>
        Response ModifyContractInvoiceAttachment(string contractNumber, IList<DomainModel.ContractInvoiceAttachment> contractInvoiceAttachments, bool commitChange = true, bool isResultSetRequired = false);

        Response ModifyContractInvoiceAttachment(string contractNumber, IList<DomainModel.ContractInvoiceAttachment> contractInvoiceAttachments, IList<DbModel.Contract> dbContracts, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false);
        /// <summary>
        /// Return all the match search ContractinvoiceAttachments.
        /// </summary>
        /// <param name="searchModel">search model</param>
        /// <returns>List of Matched Search Result</returns>
        Response GetContractInvoiceAttachment(DomainModel.ContractInvoiceAttachment searchModel);

        Response DeleteContractInvoiceAttachment(string contractNumber, IList<DomainModel.ContractInvoiceAttachment> deleteModel,  bool commitChange = true, bool isResultSetRequired = false);

        Response DeleteContractInvoiceAttachment(string contractNumber, IList<DomainModel.ContractInvoiceAttachment> deleteModel, IList<DbModel.Contract> dbContracts, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false);
    }
}
