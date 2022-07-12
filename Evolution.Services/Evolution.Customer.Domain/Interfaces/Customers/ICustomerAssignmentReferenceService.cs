using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Customer.Domain.Interfaces.Customers
{
    /// <summary>
    /// This will provide all the functionality related to customer Assignment Reference.
    /// </summary>
    public interface ICustomerAssignmentReferenceService
    {
        /// <summary>
        /// Save customer document
        /// </summary>
        /// <param name="customerCode">Unique Customer code</param>
        /// <param name="customerAssignmentReferences">List of Customer Assignment Reference</param>
        /// <returns>All the Saved Customer Assignment Reference Details</returns>
        Response SaveCustomerAssignmentReference(string customerCode, IList<Models.Customers.CustomerAssignmentReference> customerAssignmentReferences);

        /// <summary>
        /// Modify list of Customer Assignment Reference
        /// </summary>
        /// <param name="customerCode">Unique Customer code</param>
        /// <param name="customerAssignmentReferences">List of customer Assignment Reference which need to update.</param>
        Response ModifyCustomerAssignmentReference(string customerCode, IList<Models.Customers.CustomerAssignmentReference> customerAssignmentReferences);

        /// <summary>
        /// Return all the match search Customer Assignment Reference.
        /// </summary>
        /// <param name="customerCode">Unique Customer code</param>
        /// <param name="searchModel">search model</param>
        /// <returns>List of Matched Search Result</returns>
        Response GetCustomerAssignmentReference(string customerCode, Models.Customers.CustomerAssignmentReference searchModel);

        /// <summary>
        /// Delete all the matched customer assignment references.
        /// </summary>
        /// <param name="customerCode">Unique Customer code</param>
        /// <param name="customerAssignmentReferences">List of customer assignment reference which need to be deleted.</param> 
        Response DeleteCustomerAssignmentReference(string customerCode, IList<Models.Customers.CustomerAssignmentReference> customerAssignmentReferences);

    }
}
