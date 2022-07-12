using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Customer.Domain.Models.Customers;

namespace Evolution.Customer.Domain.Interfaces.Data
{
    
    public interface ICustomerAccountReferenceRepository : IGenericRepository<DbModel.CustomerCompanyAccountReference>
    {
        IList<DomainModel.CustomerCompanyAccountReference> Search(string customerCode, DomainModel.CustomerCompanyAccountReference model);

        Response AddCustomerAccountRefs(string customerCode, IList<DomainModel.CustomerCompanyAccountReference> models);

        Response UpdateCustomerAccountRefs(string customerCode, IList<DomainModel.CustomerCompanyAccountReference> models);

        Response DeleteCustomerAccountRefs(string customerCode, IList<DomainModel.CustomerCompanyAccountReference> models);

        //void Add(int customerId, IList<DomainModel.CustomerCompanyAccountReference> customerAccountReferences, ref IList<DomainModel.CustomerCompanyAccountReference> lstNotInsertedCustomerAccountReferences, ref IList<MessageDetail> lstFailedInsertMessages);

        //void Update(int customerId, IList<DomainModel.CustomerCompanyAccountReference> customerAccountReferences, ref IList<DomainModel.CustomerCompanyAccountReference> FailedToUpdatedCustomerAccountReferences, ref IList<MessageDetail> lstFailedUpdateMessages);


    }
}
