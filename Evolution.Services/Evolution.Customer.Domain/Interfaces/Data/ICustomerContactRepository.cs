using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Customer.Domain.Models;

namespace Evolution.Customer.Domain.Interfaces.Data
{
    public interface ICustomerContactRepository : IGenericRepository<DbModel.CustomerContact>
    {
        IList<DomainModel.Customers.Contact> Search(string customerCode, int? addressId, DomainModel.Customers.Contact searchModel);

        IList<DomainModel.Customers.Contact> SearchContact(string customerCode);
    }
}
