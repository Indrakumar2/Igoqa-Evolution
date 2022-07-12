using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Customer.Domain.Models.Customers
{
    /// <summary>
    /// This will hold information related to Customer Address which will use to display Address information
    /// </summary>
    public class CustomerAddress : BaseCustomer
    {
        [AuditNameAttribute("Address")]
        /// <summary>
        /// Customer Address
        /// </summary>
        public string Address { get; set; }
        [AuditNameAttribute("Country")]
        /// <summary>
        /// Name of country where customer's address belongs.
        /// </summary>
        public string Country { get; set; }
        [AuditNameAttribute("County ")]
        /// <summary>
        /// Name of County where customer's address belongs.
        /// </summary>
        public string County { get; set; }
        [AuditNameAttribute("City")]
        /// <summary>
        /// Name of City where customer's address belongs.
        /// </summary>
        public string City { get; set; }
        [AuditNameAttribute("Postal Code")]
        /// <summary>
        /// PostalCode of customer's address
        /// </summary>
        public string PostalCode { get; set; }
        [AuditNameAttribute("EU VAT Prefix ")]
        /// <summary>
        /// EU Vat Prefix for the customer's address
        /// </summary>
        public string EUVatPrefix { get; set; }
        [AuditNameAttribute("Vat Tax Reg Number")]
        /// <summary>
        /// Vat Tax Registration Number for company's address
        /// </summary>
        public string VatTaxRegNumber { get; set; }

        [AuditNameAttribute("Address Id ")]
        /// <summary>
        ///  ID for company's address
        /// </summary>
        public int AddressId { get; set; }
        [AuditNameAttribute("Customer Code")]
        /// <summary>
        ///  Customer Code to which company's address is linked
        /// </summary>
        public string CustomerCode { get; set; }

        public int? CountryId { get; set; } //Changes for D1536

        public int? StateId { get; set; } //Changes for D1536

        public int? CityId { get; set; } //Changes for D1536
        
        public IList<Contact> Contacts { get; set; }
    }
}
