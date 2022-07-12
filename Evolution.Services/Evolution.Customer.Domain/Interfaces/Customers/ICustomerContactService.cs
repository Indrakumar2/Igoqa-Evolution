using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Customer.Domain.Interfaces.Customers
{
   public  interface ICustomerContactService
    {
        /// <summary>
        /// Save customer Contact
        /// </summary> 
        /// <param name="addressId">Unique Customer address Id</param>
        /// <param name="customerContacts">List of Customer Contact</param>
        /// <returns>All the Saved Customer Contact Details</returns>
        Response SaveCustomerContact(string customerCode,int addressId, IList<Models.Customers.Contact> customerContacts);

        /// <summary>
        /// Modify list of Customer Contacts
        /// </summary> 
        /// <param name="addressId">Unique Customer address Id</param>
        /// <param name="customerContacts">List of customer Contact which need to update.</param>
        Response ModifyCustomerContact(string customerCode,int addressId, IList<Models.Customers.Contact> customerContacts);

        /// <summary>
        /// Return all the match search customer Contact.
        /// </summary> 
        /// <param name="addressId">Unique Customer address Id</param>
        /// <param name="searchModel">search model</param>
        /// <returns>List of Matched Search Result</returns>
        Response GetCustomerContact(string customerCode,int? addressId,Models.Customers.Contact customerContact);

        /// <summary>
        /// Delete all the matched customer Contacts.
        /// </summary> 
        /// <param name="addressId">Unique Customer address Id</param>
        /// <param name="customerContacts">List of customer Contact which need to be deleted.</param> 
        Response DeleteCustomerContact(string customerCode,int addressId, IList<Models.Customers.Contact> customerContacts);

        Response GetCustomerContact(string customerCode);


    }
}
