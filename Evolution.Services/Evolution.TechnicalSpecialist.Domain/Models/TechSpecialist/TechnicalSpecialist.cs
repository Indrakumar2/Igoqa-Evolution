using Evolution.Document.Domain.Models.Document;
using System;
using System.Collections.Generic;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistInfo : BaseTechnicalSpecialistInfo
    {
        [AuditNameAttribute("Salutation")]
        public string Salutation { get; set; }
        [AuditNameAttribute("Driving License No")]
        public string DrivingLicenseNo { get; set; }
        [AuditNameAttribute("Passport No")]
        public string PassportNo { get; set; }
        [AuditNameAttribute("Date Of Birth", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? DateOfBirth { get; set; }
        [AuditNameAttribute("Mode Of Communication")]
        public string ModeOfCommunication { get; set; }
        //[AuditNameAttribute("Sub Division Name")]   /** commented for Hot Fixes on NDT */
        //public string SubDivisionName { get; set; }
        [AuditNameAttribute("Is Review And Moderation Process")]
        public bool? ReviewAndModerationProcessName { get; set; }
        [AuditNameAttribute("Tax Reference")]
        public string TaxReference { get; set; }
        [AuditNameAttribute("Payroll Reference")]
        public string PayrollReference { get; set; }
        [AuditNameAttribute("Company Payroll Name")]
        public string CompanyPayrollName { get; set; }
        [AuditNameAttribute("Payroll Notes")]
        public string PayrollNotes { get; set; }
        [AuditNameAttribute("Professional Afiliation")]
        public string ProfessionalAfiliation { get; set; }
        [AuditNameAttribute("Professional Summary")]
        public string ProfessionalSummary { get; set; }
        [AuditNameAttribute("Business Information Comment")]
        public string BusinessInformationComment { get; set; }
        [AuditNameAttribute("Profile Action")]
        public string ProfileAction { get; set; }

        public string PrevProfileAction { get; set; }
        [AuditNameAttribute("Is Active")]
        public bool? IsActive { get; set; }
        [AuditNameAttribute("Driving License Expiry Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? DrivingLicenseExpiryDate { get; set; }
        [AuditNameAttribute("Passport Expiry Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? PassportExpiryDate { get; set; }

        //public string PassportCountryCode { get; set; }
        [AuditNameAttribute("Passport Country Name")]
        public string PassportCountryName { get; set; }

        public bool IsDraft { get; set; }
        [AuditNameAttribute("Is EReporting Qualified")]
        public bool? IsEReportingQualified { get; set; }
        
        public string DraftId { get; set; }
          
        public string DraftType { get; set; }

        [AuditNameAttribute("Assigned To User ")]
        public string AssignedToUser { get; set; }
        [AuditNameAttribute("Assigned By User")]
        public string AssignedByUser { get; set; }
        [AuditNameAttribute("Created By")]
        public string CreatedBy { get; set; }
        [AuditNameAttribute("Document Name")]
        public string DocumentName { get; set; }
        public IList<ModuleDocument> ProfessionalAfiliationDocuments { get; set; }
       
        public int MyTaskId { get; set; }
        [AuditNameAttribute("TS Home Page Comment")]
        public string HomePageComment { get; set; }


        //  public string LogonName { get; set; }
        [AuditNameAttribute("Password")]
        public string Password { get; set; }
        [AuditNameAttribute("Security Question")]
        public string SecurityQuestion { get; set; }
        [AuditNameAttribute("Security Answer")]
        public string SecurityAnswer { get; set; }
        [AuditNameAttribute("Is Locked Out ")]
        public bool IsLockOut { get; set; }
        [AuditNameAttribute("Enable Login")]
        public bool? IsEnableLogin { get; set; }

        [AuditNameAttribute("TQM Comment")]
        public string TqmComment { get; set; }

        public bool? IsTsCredSent { get; set; }
         
        public IList<string> Epins { get; set; }

        public int UserId { get; set; } //D1256 
    }


}
