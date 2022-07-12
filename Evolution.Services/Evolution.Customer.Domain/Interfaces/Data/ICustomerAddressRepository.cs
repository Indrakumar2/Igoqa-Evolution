using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;  
using DomainModel = Evolution.Customer.Domain.Models;


namespace Evolution.Customer.Domain.Interfaces.Data
{
    
    public interface ICustomerAddressRepository : IGenericRepository<DbModel.CustomerAddress>
    {
        IList<DomainModel.Customers.CustomerAddress> Search(DomainModel.Customers.CustomerAddress model);

        IList<DomainModel.Customers.CustomerAddress> SearchAddress(string customerCode);
    }
}
