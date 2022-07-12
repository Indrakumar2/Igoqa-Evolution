using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Project.Domain.Models.Projects;

namespace Evolution.Project.Domain.Interfaces.Projects
{
    public interface IProjectDocumentService
    {
        Response GetContractProjectDocuments(string contractId);

        Response GetCustomerProjectDocuments(int? customerId);
    }
}