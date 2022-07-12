using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CustomerAddress
    {
        public CustomerAddress()
        {
            Assignment = new HashSet<Assignment>();
            ContractDefaultCustomerContractAddress = new HashSet<Contract>();
            ContractDefaultCustomerInvoiceAddress = new HashSet<Contract>();
            CustomerContact = new HashSet<CustomerContact>();
            ProjectCustomerInvoiceAddress = new HashSet<Project>();
            ProjectCustomerProjectAddress = new HashSet<Project>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Address { get; set; }
        public int? CityId { get; set; }
        public string PostalCode { get; set; }
        public string Euvatprefix { get; set; }
        public string VatTaxRegistrationNo { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual City City { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Assignment> Assignment { get; set; }
        public virtual ICollection<Contract> ContractDefaultCustomerContractAddress { get; set; }
        public virtual ICollection<Contract> ContractDefaultCustomerInvoiceAddress { get; set; }
        public virtual ICollection<CustomerContact> CustomerContact { get; set; }
        public virtual ICollection<Project> ProjectCustomerInvoiceAddress { get; set; }
        public virtual ICollection<Project> ProjectCustomerProjectAddress { get; set; }
    }
}
