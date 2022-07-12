using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.Contract.Domain.Interfaces.Data
{
    /// <summary>
    /// TODO : Replace string to ContractInvoiceAttachment DB Model
    /// </summary>
    public interface IContractInvoiceAttachmentRepository: IGenericRepository<DbModel.ContractInvoiceAttachment>
    {
        IList<DomainModel.ContractInvoiceAttachment> Search(DomainModel.ContractInvoiceAttachment searchModel);

        int DeleteInvoiceAttachment(List<int> attachmentIds);

        int DeleteInvoiceAttachment(List<DbModel.ContractInvoiceAttachment> contractInvoiceAttachments);

        int DeleteInvoiceAttachment(List<DomainModel.ContractInvoiceAttachment> contractInvoiceAttachments);
    }
}
