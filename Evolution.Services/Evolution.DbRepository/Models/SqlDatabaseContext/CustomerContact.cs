using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CustomerContact
    {
        public CustomerContact()
        {
            Assignment = new HashSet<Assignment>();
            ContractDefaultCustomerContractContact = new HashSet<Contract>();
            ContractDefaultCustomerInvoiceContact = new HashSet<Contract>();
            ProjectClientNotification = new HashSet<ProjectClientNotification>();
            ProjectCustomerContact = new HashSet<Project>();
            ProjectCustomerProjectContact = new HashSet<Project>();
        }

        public int Id { get; set; }
        public int CustomerAddressId { get; set; }
        public string Salutation { get; set; }
        public string Position { get; set; }
        public string ContactName { get; set; }
        public string TelephoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string OtherContactDetails { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public string LoginName { get; set; }

        public virtual CustomerAddress CustomerAddress { get; set; }
        public virtual ICollection<Assignment> Assignment { get; set; }
        public virtual ICollection<Contract> ContractDefaultCustomerContractContact { get; set; }
        public virtual ICollection<Contract> ContractDefaultCustomerInvoiceContact { get; set; }
        public virtual ICollection<ProjectClientNotification> ProjectClientNotification { get; set; }
        public virtual ICollection<Project> ProjectCustomerContact { get; set; }
        public virtual ICollection<Project> ProjectCustomerProjectContact { get; set; }
    }
}
