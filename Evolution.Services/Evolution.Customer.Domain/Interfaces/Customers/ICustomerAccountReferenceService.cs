using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Customer.Domain.Interfaces.Customers
{
    /// <summary>
    /// This will provide all the functionality related to customer account reference.
    /// </summary>
    public interface ICustomerAccountReferenceService
    {
        /// <summary>
        /// Save Customer Account Reference
        /// </summary>
        /// <param name="customerCode">Unique Customer code</param>
        /// <param name="customerAccountReferences">List of Customer Account Reference</param>
        /// <returns>All the Saved Customer AccountReference Details</returns>
        Response SaveCustomerAccountReference(string customerCode, IList<Models.Customers.CustomerCompanyAccountReference> customerAccountReferences);

        /// <summary>
        /// Modify list of Customer Account Reference
        /// </summary>
        /// <param name="customerCode">Unique Customer code</param>
        /// <param name="customerAccountReferences">List of customer Account Reference which need to update.</param>
        Response ModifyCustomerAccountReference(string customerCode, IList<Models.Customers.CustomerCompanyAccountReference> customerAccountReferences);

        /// <summary>
        /// Return all the match search Customer Account Reference.
        /// </summary>
        /// <param name="customerCode">Unique Customer code</param>
        /// <param name="searchModel">search model</param>
        /// <returns>List of Matched Search Result</returns>
        Response GetCustomerAccountReference(string customerCode, Models.Customers.CustomerCompanyAccountReference searchModel);

        /// <summary>
        /// Delete all the matched customer account reference.
        /// </summary>
        /// <param name="customerCode">Unique Customer code</param>
        /// <param name="customerAccountReferences">List of customer account reference which need to be deleted.</param> 
        Response DeleteCustomerAccountReference(string customerCode, IList<Models.Customers.CustomerCompanyAccountReference> customerAccountReferences);

    }
}
