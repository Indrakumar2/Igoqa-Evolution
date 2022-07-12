using Evolution.Common.Models.Base;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistContactInfo : BaseTechnicalSpecialistModel
    {
        [AuditNameAttribute("Technical Specialist Contact Info Id")]
        public int Id { get; set; }
        [AuditNameAttribute("ePin")]
        public int Epin { get; set; }
        [AuditNameAttribute("Address")]
        public string Address { get; set; }

        public int? CountryId { get; set; } // Added For ITK DEf 1536
        [AuditNameAttribute("Country")]
        public string Country { get; set; }

        public int? CountyId { get; set; } // Added For ITK DEf 1536
        [AuditNameAttribute("County")]
        public string County { get; set; }

        public int? CityId { get; set; } // Added For ITK DEf 1536
        [AuditNameAttribute("City")]
        public string City { get; set; }
        [AuditNameAttribute("Post/Zip Code")]
        public string PostalCode { get; set; }
        [AuditNameAttribute("Mobile Number")]
        public string MobileNumber { get; set; }
        [AuditNameAttribute("Telephone Number")]
        public string TelephoneNumber { get; set; }
        [AuditNameAttribute("Emergency Contact Name")]
        public string EmergencyContactName { get; set; }
        [AuditNameAttribute("Email Address")]
        public string EmailAddress { get; set; }
        [AuditNameAttribute("Fax Number")]
        public string FaxNumber { get; set; }
        //[AuditNameAttribute("Display Order")]
        public string DisplayOrder { get; set; }
        [AuditNameAttribute("Contact Type")]
        public string ContactType { get; set; } 

        public bool? IsGeoCordinateSync { get; set; }
    }
}
