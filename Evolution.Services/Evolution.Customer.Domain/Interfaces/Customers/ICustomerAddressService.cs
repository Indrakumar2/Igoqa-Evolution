using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Customer.Domain.Interfaces.Customers
{
    /// <summary>
    /// This will provide all the functionality related to customer address.
    /// </summary>
   public interface ICustomerAddressService
    {
        /// <summary>
        /// Save customer Address
        /// </summary>
        /// <param name="customerCode">Unique Customer code</param>
        /// <param name="customerAddresses">List of Customer Address</param>
        /// <returns>All the Saved Customer Address Details</returns>
        Response SaveCustomerAddress(string customerCode, IList<Models.Customers.CustomerAddress> customerAddresses);

        /// <summary>
        /// Modify list of Customer Address
        /// </summary>
        /// <param name="customerCode">Unique Customer code</param>
        /// <param name="customerAddresses">List of customer Address which need to update.</param>
        Response ModifyCustomerAddress(string customerCode, IList<Models.Customers.CustomerAddress> customerAddresses);

        /// <summary>
        /// Return all the match search Customer Address.
        /// </summary>
        /// <param name="customerCode">Unique Customer code</param>
        /// <param name="searchModel">search model</param>
        /// <returns>List of Matched Search Result</returns>
        Response GetCustomerAddress(Models.Customers.CustomerAddress searchModel);

        /// <summary>
        /// Delete all the matched customer address.
        /// </summary>
        /// <param name="customerCode">Unique Customer code</param>
        /// <param name="customerAddresses">List of customer address which need to be deleted.</param> 
        Response DeleteCustomerAddress(string customerCode, IList<Models.Customers.CustomerAddress> customerAddresses);

        Response GetCustomerAddress(string customerCode);

    }
}
