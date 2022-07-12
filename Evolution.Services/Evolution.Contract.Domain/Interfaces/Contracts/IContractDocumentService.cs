using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;

namespace Evolution.Contract.Domain.Interfaces.Contracts
{
    public interface IContractDocumentService
    {
        Response GetCustomerContractDocuments(int? customerId);
    }
}