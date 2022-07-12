using System.Collections.Generic;

namespace Evolution.Customer.Domain.Models.Customers
{
    /// <summary>
    /// This will contain customer information.
    /// </summary>
    public class Customer : BaseCustomer
    {
        [AuditNameAttribute("Customer Code")]
        public string CustomerCode { get; set; }

        [AuditNameAttribute("Customer Name")]
        public string CustomerName { get; set; }

        [AuditNameAttribute("Parent Company Name")]
        public string ParentCompanyName { get; set; }

        [AuditNameAttribute("MIIWA Id")]
        public int MIIWAId { get; set; }

        [AuditNameAttribute("MIIWA Parent Id")]
        public int MIIWAParentId { get; set; }

        [AuditNameAttribute("Active")]
        public string Active { get; set; }


        //added for Reports
        public int? CustomerId { get; set; }
        public int OperatingCompanyCoordinatorId { get; set; }
        public int InvoicingCompanyID { get; set; }
        public int ContractCompanyCoordinatorId { get; set; }
    }

    public class CustomerSearch : Customer
    {
        [AuditNameAttribute("Customer Codes")]
        public IList<string> CustomerCodes { get; set; }

        [AuditNameAttribute("Operating Country")]
        public string OperatingCountry { get; set; }

        [AuditNameAttribute("Search Document Type")]
        public string SearchDocumentType { get; set; }

        [AuditNameAttribute("Document Search Text")]
        public string DocumentSearchText { get; set; }

        [AuditNameAttribute("CoordinatorId")]
        public int CoordinatorId { get; set; }

        public int? Evolution1Id { get; set; }

        public int? MasterDataTypeId { get; set; }

        [AuditNameAttribute("OperatingCompanyId")]
        public int OperatingCompanyId { get; set; }

        [AuditNameAttribute("ContractHolderCompanyId")]
        public int ContractHolderCompanyId { get; set; }
    }
}
