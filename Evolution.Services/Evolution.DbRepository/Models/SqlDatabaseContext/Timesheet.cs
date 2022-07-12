using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Timesheet
    {
        public Timesheet()
        {
            TimesheetHistory = new HashSet<TimesheetHistory>();
            TimesheetInterCompanyDiscount = new HashSet<TimesheetInterCompanyDiscount>();
            TimesheetNote = new HashSet<TimesheetNote>();
            TimesheetReference = new HashSet<TimesheetReference>();
            TimesheetTechnicalSpecialist = new HashSet<TimesheetTechnicalSpecialist>();
            TimesheetTechnicalSpecialistAccountItemConsumable = new HashSet<TimesheetTechnicalSpecialistAccountItemConsumable>();
            TimesheetTechnicalSpecialistAccountItemExpense = new HashSet<TimesheetTechnicalSpecialistAccountItemExpense>();
            TimesheetTechnicalSpecialistAccountItemTime = new HashSet<TimesheetTechnicalSpecialistAccountItemTime>();
            TimesheetTechnicalSpecialistAccountItemTravel = new HashSet<TimesheetTechnicalSpecialistAccountItemTravel>();
        }

        public long Id { get; set; }
        public int AssignmentId { get; set; }
        public int TimesheetNumber { get; set; }
        public string TimesheetStatus { get; set; }
        public string UnusedReason { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string DatePeriod { get; set; }
        public DateTime? ExpectedCompleteDate { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Reference3 { get; set; }
        public string TimesheetDescription { get; set; }
        public int? PercentageCompleted { get; set; }
        public bool? IsApprovedByContractCompany { get; set; }
        public string ReviewedBy { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsSkeletonTimesheet { get; set; }
        public long? Evoid { get; set; }
        public DateTime? NextTimesheetDate { get; set; }
        public bool? IsTransferredToInvoice { get; set; }
        public long? ParentTiemsheetId { get; set; }
        public string NextTimesheetStatus { get; set; }
        public string VisitCreationOrigin { get; set; }
        public string SummaryOfReport { get; set; }
        public string ExtranetUpdate { get; set; }
        public DateTime? SendToCustomer { get; set; }
        public string NotificationReference { get; set; }
        public string TimesheetReject { get; set; }
        public string ModeOfCreation { get; set; }
        public string InsertedFrom { get; set; }
        public DateTime? NextVisitDateTo { get; set; }
        public string ResultofInvestigation { get; set; }
        public string ClientReviewStatus { get; set; }
        public string ReasonForRejection { get; set; }
        public string SyncFlag { get; set; }

        public virtual Assignment Assignment { get; set; }
        public virtual ICollection<TimesheetHistory> TimesheetHistory { get; set; }
        public virtual ICollection<TimesheetInterCompanyDiscount> TimesheetInterCompanyDiscount { get; set; }
        public virtual ICollection<TimesheetNote> TimesheetNote { get; set; }
        public virtual ICollection<TimesheetReference> TimesheetReference { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialist> TimesheetTechnicalSpecialist { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemConsumable> TimesheetTechnicalSpecialistAccountItemConsumable { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemExpense> TimesheetTechnicalSpecialistAccountItemExpense { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTime> TimesheetTechnicalSpecialistAccountItemTime { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTravel> TimesheetTechnicalSpecialistAccountItemTravel { get; set; }
    }
}
