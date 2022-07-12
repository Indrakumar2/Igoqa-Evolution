using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Customer.Domain.Interfaces.Customers
{
   public interface ICustomerDetailService
    { 
        /// <summary>
        /// Save all customer details including addresses,contacts,documents,notes,Assignment refferences and company account refferences.
        /// </summary>
        /// <param name="customerDetails">List of customer details</param>
        /// <returns>Returns Response with Code,Messages and Results list.</returns>
        Response SaveCustomerDetails(IList<Models.Customers.CustomerDetail> customerDetails);
    }
}
