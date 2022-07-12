using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Visit
    {
        public Visit()
        {
            VisitHistory = new HashSet<VisitHistory>();
            VisitInterCompanyDiscount = new HashSet<VisitInterCompanyDiscount>();
            VisitNote = new HashSet<VisitNote>();
            VisitReference = new HashSet<VisitReference>();
            VisitSupplierPerformance = new HashSet<VisitSupplierPerformance>();
            VisitTechnicalSpecialist = new HashSet<VisitTechnicalSpecialist>();
            VisitTechnicalSpecialistAccountItemConsumable = new HashSet<VisitTechnicalSpecialistAccountItemConsumable>();
            VisitTechnicalSpecialistAccountItemExpense = new HashSet<VisitTechnicalSpecialistAccountItemExpense>();
            VisitTechnicalSpecialistAccountItemTime = new HashSet<VisitTechnicalSpecialistAccountItemTime>();
            VisitTechnicalSpecialistAccountItemTravel = new HashSet<VisitTechnicalSpecialistAccountItemTravel>();
        }

        public long Id { get; set; }
        public int? SupplierId { get; set; }
        public int AssignmentId { get; set; }
        public int VisitNumber { get; set; }
        public string VisitStatus { get; set; }
        public string ReportNumber { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string DatePeriod { get; set; }
        public DateTime? ExpectedCompleteDate { get; set; }
        public DateTime? ReportSentToCustomerDate { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Reference3 { get; set; }
        public string NotificationReference { get; set; }
        public int? PercentageCompleted { get; set; }
        public bool? IsApprovedByContractCompany { get; set; }
        public bool? IsSkeltonVisit { get; set; }
        public long? ParentVisitId { get; set; }
        public bool? IsFinalVisit { get; set; }
        public string RejectionReason { get; set; }
        public string ClientReviewStatus { get; set; }
        public string ReviewedBy { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public long? Evoid { get; set; }
        public DateTime? NextVisitDate { get; set; }
        public bool? IsTransferredToInvoice { get; set; }
        public string NextVisitStatus { get; set; }
        public string VisitCreationOrigin { get; set; }
        public string SummaryOfReport { get; set; }
        public string ExtranetUpdate { get; set; }
        public string VisitReject { get; set; }
        public string ModeOfCreation { get; set; }
        public string InsertedFrom { get; set; }
        public DateTime? NextVisitDateTo { get; set; }
        public string ResultofInvestigation { get; set; }
        public string SyncFlag { get; set; }
        public string UnusedReason { get; set; }

        public virtual Assignment Assignment { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<VisitHistory> VisitHistory { get; set; }
        public virtual ICollection<VisitInterCompanyDiscount> VisitInterCompanyDiscount { get; set; }
        public virtual ICollection<VisitNote> VisitNote { get; set; }
        public virtual ICollection<VisitReference> VisitReference { get; set; }
        public virtual ICollection<VisitSupplierPerformance> VisitSupplierPerformance { get; set; }
        public virtual ICollection<VisitTechnicalSpecialist> VisitTechnicalSpecialist { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemConsumable> VisitTechnicalSpecialistAccountItemConsumable { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemExpense> VisitTechnicalSpecialistAccountItemExpense { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTime> VisitTechnicalSpecialistAccountItemTime { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTravel> VisitTechnicalSpecialistAccountItemTravel { get; set; }
    }
}
