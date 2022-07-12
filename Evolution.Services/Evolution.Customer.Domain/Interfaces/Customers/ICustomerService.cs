using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Customer.Domain.Interfaces.Customers
{
    /// <summary>
    /// This will provide all the functionality related to customer.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Save Customers
        /// </summary>
        /// <param name="customers">List of Customer</param>
        /// <returns>All the Saved Customer Details</returns>
        Response SaveCustomer(IList<Models.Customers.Customer> customers);

        /// <summary>
        /// Modify list of Customers
        /// </summary>
        /// <param name="customers">List of customer which need to update.</param>
        Response ModifyCustomer(IList<Models.Customers.Customer> customers);

        /// <summary>
        /// iConnect integration to amend Customers
        /// </summary>
        /// <param name="customers">List of customer which needs to be amended.</param>
        Response IConnectIntegration(IList<Models.Customers.Customer> customers);

        /// <summary>
        /// Return all the match search customer.
        /// </summary>
        /// <param name="searchModel">search model</param>
        /// <returns>List of Matched Search Result</returns>
        //Response GetCustomer(Models.Customers.CustomerSearch searchModel);
        Task<Response> GetCustomerAsync(Models.Customers.CustomerSearch searchModel);

        Response GetCustomerBasedOnCoordinator(Models.Customers.CustomerSearch searchModel);

        bool IsValidCustomer(IList<string> customerCodes, ref IList<DbModel.Customer> dbCustomers, ref IList<ValidationMessage> messages, params Expression<Func<DbModel.Customer, object>>[] includes);

        Response GetApprovedVistCustomers(int ContractHolderCompanyId, bool isVisit, bool isNDT);

        Response GetUnApprovedVisitCustomers(int ContractHolderCompanyId, int? CoordinatorId, bool isVisit, bool isOperating);

        Response GetVisitTimesheetKPICustomers(int ContractHolderCompanyId, bool isVisit, bool isOperating);
    }
}
