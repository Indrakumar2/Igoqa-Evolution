using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Customer.Domain.Models.Customers
{
    /// <summary>
    /// This will hold the information related to Customer and Company Account Reference Relation
    /// </summary>
    public class CustomerCompanyAccountReference : BaseCustomer
    {
        [AuditNameAttribute("Customer Company Account Reference Id ")]
        /// <summary>
        ///  Customer Company Account Reference Id
        /// </summary>
        public int CustomerCompanyAccountReferenceId { get; set; }
        [AuditNameAttribute("Account Reference Value")]
        /// <summary>
        /// Account Reference Value
        /// </summary>
        public string AccountReferenceValue { get; set; }
        [AuditNameAttribute("Company Name")]
        /// <summary>
        /// Account Reference Value
        /// </summary>
        public string CompanyName { get; set; }
         
        /// <summary>
        /// Code of the company
        /// </summary>
        public string CompanyCode { get; set; }
    }

    public class CustomerCompanyAccountReferenceDetail : CustomerCompanyAccountReference
    {
       
        public int CompanyId { get; set; }

        public int CustomerId { get; set; }
    }
}
