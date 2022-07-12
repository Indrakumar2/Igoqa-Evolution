using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialist
    {
        public TechnicalSpecialist()
        {
            AssignmentTechnicalSpecialist = new HashSet<AssignmentTechnicalSpecialist>();
            OverrideResource = new HashSet<OverrideResource>();
            TechnicalSpecialistCalendar = new HashSet<TechnicalSpecialistCalendar>();
            TechnicalSpecialistCertificationAndTraining = new HashSet<TechnicalSpecialistCertificationAndTraining>();
            TechnicalSpecialistCodeAndStandard = new HashSet<TechnicalSpecialistCodeAndStandard>();
            TechnicalSpecialistCommodityEquipmentKnowledge = new HashSet<TechnicalSpecialistCommodityEquipmentKnowledge>();
            TechnicalSpecialistComputerElectronicKnowledge = new HashSet<TechnicalSpecialistComputerElectronicKnowledge>();
            TechnicalSpecialistContact = new HashSet<TechnicalSpecialistContact>();
            TechnicalSpecialistCustomerApproval = new HashSet<TechnicalSpecialistCustomerApproval>();
            TechnicalSpecialistEducationalQualification = new HashSet<TechnicalSpecialistEducationalQualification>();
            TechnicalSpecialistLanguageCapability = new HashSet<TechnicalSpecialistLanguageCapability>();
            TechnicalSpecialistNote = new HashSet<TechnicalSpecialistNote>();
            TechnicalSpecialistPayRate = new HashSet<TechnicalSpecialistPayRate>();
            TechnicalSpecialistPaySchedule = new HashSet<TechnicalSpecialistPaySchedule>();
            TechnicalSpecialistStamp = new HashSet<TechnicalSpecialistStamp>();
            TechnicalSpecialistTaxonomy = new HashSet<TechnicalSpecialistTaxonomy>();
            TechnicalSpecialistTimeOffRequest = new HashSet<TechnicalSpecialistTimeOffRequest>();
            TechnicalSpecialistTrainingAndCompetency = new HashSet<TechnicalSpecialistTrainingAndCompetency>();
            TechnicalSpecialistWorkHistory = new HashSet<TechnicalSpecialistWorkHistory>();
            TimesheetTechnicalSpecialist = new HashSet<TimesheetTechnicalSpecialist>();
            VisitTechnicalSpecialist = new HashSet<VisitTechnicalSpecialist>();
        }

        public int Id { get; set; }
        public int Pin { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string DrivingLicenseNumber { get; set; }
        public string PassportNumber { get; set; }
        public string ModeOfCommunication { get; set; }
        public int CompanyId { get; set; }
        public int SubDivisionId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ProfileStatusId { get; set; }
        public int? EmploymentTypeId { get; set; }
        public bool? IsReviewAndModerationProcess { get; set; }
        public string TaxReference { get; set; }
        public int? CompanyPayrollId { get; set; }
        public string PayrollReference { get; set; }
        public string PayrollNote { get; set; }
        public string ProfessionalAfiliation { get; set; }
        public string ProfessionalSummary { get; set; }
        public string BusinessInformationComment { get; set; }
        public int ProfileActionId { get; set; }
        public bool? IsActive { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public DateTime? DrivingLicenseExpiryDate { get; set; }
        public DateTime? PassportExpiryDate { get; set; }
        public int? PassportCountryOriginId { get; set; }
        public bool? IsEreportingQualified { get; set; }
        public string CreatedBy { get; set; }
        public string LogInName { get; set; }
        public string HomePageComment { get; set; }
        public string ContactComment { get; set; }
        public string ApprovalStatus { get; set; }
        public string Tqmcomment { get; set; }
        public string AssignedToUser { get; set; }
        public string AssignedByUser { get; set; }
        public int? PendingWithId { get; set; }
        public bool? IsTsCredSent { get; set; }
        public int? Userid { get; set; }

        public virtual Company Company { get; set; }
        public virtual CompanyPayroll CompanyPayroll { get; set; }
        public virtual Data EmploymentType { get; set; }
        public virtual Country PassportCountryOrigin { get; set; }
        public virtual User PendingWith { get; set; }
        public virtual Data ProfileAction { get; set; }
        public virtual Data ProfileStatus { get; set; }
        public virtual Data SubDivision { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<AssignmentTechnicalSpecialist> AssignmentTechnicalSpecialist { get; set; }
        public virtual ICollection<OverrideResource> OverrideResource { get; set; }
        public virtual ICollection<TechnicalSpecialistCalendar> TechnicalSpecialistCalendar { get; set; }
        public virtual ICollection<TechnicalSpecialistCertificationAndTraining> TechnicalSpecialistCertificationAndTraining { get; set; }
        public virtual ICollection<TechnicalSpecialistCodeAndStandard> TechnicalSpecialistCodeAndStandard { get; set; }
        public virtual ICollection<TechnicalSpecialistCommodityEquipmentKnowledge> TechnicalSpecialistCommodityEquipmentKnowledge { get; set; }
        public virtual ICollection<TechnicalSpecialistComputerElectronicKnowledge> TechnicalSpecialistComputerElectronicKnowledge { get; set; }
        public virtual ICollection<TechnicalSpecialistContact> TechnicalSpecialistContact { get; set; }
        public virtual ICollection<TechnicalSpecialistCustomerApproval> TechnicalSpecialistCustomerApproval { get; set; }
        public virtual ICollection<TechnicalSpecialistEducationalQualification> TechnicalSpecialistEducationalQualification { get; set; }
        public virtual ICollection<TechnicalSpecialistLanguageCapability> TechnicalSpecialistLanguageCapability { get; set; }
        public virtual ICollection<TechnicalSpecialistNote> TechnicalSpecialistNote { get; set; }
        public virtual ICollection<TechnicalSpecialistPayRate> TechnicalSpecialistPayRate { get; set; }
        public virtual ICollection<TechnicalSpecialistPaySchedule> TechnicalSpecialistPaySchedule { get; set; }
        public virtual ICollection<TechnicalSpecialistStamp> TechnicalSpecialistStamp { get; set; }
        public virtual ICollection<TechnicalSpecialistTaxonomy> TechnicalSpecialistTaxonomy { get; set; }
        public virtual ICollection<TechnicalSpecialistTimeOffRequest> TechnicalSpecialistTimeOffRequest { get; set; }
        public virtual ICollection<TechnicalSpecialistTrainingAndCompetency> TechnicalSpecialistTrainingAndCompetency { get; set; }
        public virtual ICollection<TechnicalSpecialistWorkHistory> TechnicalSpecialistWorkHistory { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialist> TimesheetTechnicalSpecialist { get; set; }
        public virtual ICollection<VisitTechnicalSpecialist> VisitTechnicalSpecialist { get; set; }
    }
}
