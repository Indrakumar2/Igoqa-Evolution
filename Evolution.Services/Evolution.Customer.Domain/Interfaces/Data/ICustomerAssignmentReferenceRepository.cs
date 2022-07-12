using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Customer.Domain.Models;

namespace Evolution.Customer.Domain.Interfaces.Data
{
     
    public interface ICustomerAssignmentReferenceRepository : IGenericRepository<DbModel.CustomerAssignmentReferenceType>
    {
        IList<DomainModel.Customers.CustomerAssignmentReference> Search(string customerCode, DomainModel.Customers.CustomerAssignmentReference SearchModel);
    }
}
