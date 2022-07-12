using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class BaseTechnicalSpecialistInfo : BaseModel
    {
        [AuditNameAttribute("ePin")]
        public int Epin { get; set; }

        [AuditNameAttribute("ePin Text")]
        public string ePinString { get; set; }

        //[AuditNameAttribute("Technical Specialist Id")]
        //public int Id { get; set; }

        [AuditNameAttribute("First Name")]
        public string FirstName { get; set; }

        [AuditNameAttribute("Middle Name")]
        public string MiddleName { get; set; }

        [AuditNameAttribute("Last Name")]
        public string LastName { get; set; }

        //public string FullName => string.Format("{0},{1},{2}", LastName, MiddleName, FirstName);
        [AuditNameAttribute("Employment Type")]
        public string EmploymentType { get; set; }

        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        [AuditNameAttribute("Company Name")]
        public string CompanyName { get; set; }

        [AuditNameAttribute("Profile Status")]
        public string ProfileStatus { get; set; }

        [AuditNameAttribute("Start Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? StartDate { get; set; }

        [AuditNameAttribute("End Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? EndDate { get; set; }

        [AuditNameAttribute("Log On Name ")]
        public string LogonName { get; set; }

        [AuditNameAttribute("Contact Comment")]
        public string ContactComment { get; set; }

        [AuditNameAttribute("Approval Status")]
        public string ApprovalStatus { get; set; }

        [AuditNameAttribute("Sub Division Name")]  /** Added for Hot Fixes on NDT */
        public string SubDivisionName { get; set; }

        public string PendingWithUser { get; set; } //Added for D946 CR
        
        [AuditNameAttribute("Pending With User")] 
        public string PendingWithUserLogOnName { get; set; } // IGOQC D855 
    }

    public class SearchTechnicalSpecialist : BaseTechnicalSpecialistInfo
    {
        [AuditNameAttribute("Full Address")]
        public string FullAddress { get; set; }

        public int? CountryId { get; set; } //Added for ITK D1536
        [AuditNameAttribute("Country")]
        public string Country { get; set; }

        public int? CountyId { get; set; } //Added for ITK D1536
        [AuditNameAttribute("County")]
        public string County { get; set; }

        public int? CityId { get; set; } //Added for ITK D1536
        [AuditNameAttribute("City")]
        public string City { get; set; }

        [AuditNameAttribute("Post/Zip Code")]
        public string PinCode { get; set; }

        [AuditNameAttribute("Technical Discipline")]
        public string TechnicalDiscipline { get; set; }

        [AuditNameAttribute("Category")]
        public string Category { get; set; }

        [AuditNameAttribute("Sub Category")]
        public string SubCategory { get; set; }

        [AuditNameAttribute("Service")]
        public string Service { get; set; }

        //[AuditNameAttribute("Logon Name")]
        //public string LogonName { get; set; }

        [AuditNameAttribute("Search Document Type")]
        public string SearchDocumentType { get; set; }

        [AuditNameAttribute(" Document Search Text")]
        public string DocumentSearchText { get; set; } 

    }

    public class TechnicalSpecialistSearchResult : BaseTechnicalSpecialistInfo
    {
        [AuditNameAttribute("TechnicalSpecialist Contact")]
        public IList<TechnicalSpecialistContactInfo> TechnicalSpecialistContact { get; set; }
    }
}
