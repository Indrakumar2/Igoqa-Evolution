using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Customer.Domain.Interfaces.Customers
{
    /// <summary>
    /// This will provide all the functionality related to customer note.
    /// </summary>
    public interface ICustomerNoteService
    {
        /// <summary>
        /// Save customer notes
        /// </summary>
        /// <param name="customerCode">Unique Customer code</param>
        /// <param name="customerNotes">List of Customer Note</param>
        /// <returns>All the Saved Customer Note Details</returns>
        Response SaveCustomerNote(string customerCode,IList<Models.Customers.CustomerNote> customerNotes);

        /// <summary>
        /// Modify list of Customer Notes
        /// </summary>
        /// <param name="customerCode">Unique Customer code</param>
        /// <param name="customerNotes">List of customer note which need to update.</param>
        Response ModifyCustomerNote(string customerCode, IList<Models.Customers.CustomerNote> customerNotes);

        /// <summary>
        /// Return all the match search customer note.
        /// </summary>
        /// <param name="customerCode">Unique Customer code</param>
        /// <param name="searchModel">search model</param>
        /// <returns>List of Matched Search Result</returns>
        Response GetCustomerNote(string customerCode, Models.Customers.CustomerNote searchModel);

        /// <summary>
        /// Delete all the matched customer notes.
        /// </summary>
        /// <param name="customerCode">Unique Customer code</param>
        /// <param name="customerNotes">List of customer note which need to be deleted.</param> 
        Response DeleteCustomerNote(string customerCode, IList<Models.Customers.CustomerNote> customerNotes);
    }
}
