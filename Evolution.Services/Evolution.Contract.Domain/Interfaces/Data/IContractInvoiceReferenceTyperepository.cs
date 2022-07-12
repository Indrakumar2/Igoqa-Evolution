using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.Contract.Domain.Interfaces.Data
{
    /// <summary>
    /// TODO : Replace string to ContractInvoiceReference DB Model
    /// </summary>
    public interface IContractInvoiceReferenceTypeRepository: IGenericRepository<DbModel.ContractInvoiceReference>
    {
        IList<DomainModel.ContractInvoiceReferenceType> Search(DomainModel.ContractInvoiceReferenceType searchModel);

        int DeleteInvoiceReference(List<int> referenceTypeIds);

        int DeleteInvoiceReference(List<DbModel.ContractInvoiceReference> contractInvoiceReferences);

        int DeleteInvoiceReference(List<DomainModel.ContractInvoiceReferenceType> contractInvoiceReferenceTypes);

    }
}
