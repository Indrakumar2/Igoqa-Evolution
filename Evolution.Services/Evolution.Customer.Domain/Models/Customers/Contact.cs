using Evolution.Security.Domain.Models.Security;

namespace Evolution.Customer.Domain.Models.Customers
{
    public class Contact : BaseCustomer
    { 
        [AuditName("Contact Id")]
        public int ContactId { get; set; }

        [AuditName("Customer Address Id ")]
        public string ContactAddress { get; set; } //Changes for IGO - D905

        public int CustomerAddressId { get; set; }

        [AuditName("Salutation")] 
        public string Salutation { get; set; }

        [AuditName("Position ")] 
        public string Position { get; set; }

        [AuditName("Contact Person Name")] 
        public string ContactPersonName { get; set; }

        [AuditName("Landline")] 
        public string Landline { get; set; }

        [AuditName("Fax ")] 
        public string Fax { get; set; }

        [AuditName("Mobile ")]
        public string Mobile { get; set; }

        [AuditName("Email")] 
        public string Email { get; set; }

        [AuditName("Other Detail")] 
        public string OtherDetail { get; set; }

        [AuditName("Logon Name")]
        public string LogonName { get; set; }

        [AuditName("Is Portal User")]
        public bool IsPortalUser { get; set; }

        public ExtranetUserInfo UserInfo { get; set; }
    }
 
}
