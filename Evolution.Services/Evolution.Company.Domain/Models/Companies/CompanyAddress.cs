using Evolution.Common.Models.Base;

namespace Evolution.Company.Domain.Models.Companies
{
    public class CompanyAddress : BaseModel
    {
        [AuditNameAttribute("Address Id ")]
        public int AddressId { get; set; }

        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        [AuditNameAttribute("Office Name")]
        public string OfficeName { get; set; }

        [AuditNameAttribute("Full Address")]
        public string FullAddress { get; set; }
        
        public int? CountryId { get; set; } //Added for ITK D1536
        [AuditNameAttribute("Country")]
        public string Country { get; set; }

        public int? StateId { get; set; } //Added for ITK D1536
        [AuditNameAttribute("State")]
        public string State { get; set; }

        public int? CityId { get; set; } //Added for ITK D1536
        [AuditNameAttribute("City")]
        public string City { get; set; }

        [AuditNameAttribute("Post/Zip Code ")]
        public string PostalCode { get; set; }

        [AuditNameAttribute("Account Ref")]
        public string AccountRef { get; set; }
    }
}
