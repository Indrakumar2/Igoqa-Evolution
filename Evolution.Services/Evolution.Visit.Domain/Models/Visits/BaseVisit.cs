using System;
using System.Collections.Generic;

namespace Evolution.Visit.Domain.Models.Visits
{
    public class BaseVisit : VisitSearch
    {
        private string _visitJobReferenceNumber { get; set; }
        //[AuditNameAttribute("Visit Id")]
        //public long VisitId { get; set; }
        [AuditNameAttribute("Visit Number")]
        public int VisitNumber { get; set; }
        [AuditNameAttribute("Visit Assignment Number")]
        public int VisitAssignmentNumber { get; set; }
        [AuditNameAttribute("Visit Assignment Id")]
        public int VisitAssignmentId { get; set; }
        [AuditNameAttribute("Contract Company Code")]
        public string VisitContractCompanyCode { get; set; }
        [AuditNameAttribute("Contract Company")]
        public string VisitContractCompany { get; set; }
        [AuditNameAttribute("Visit Contract Coordinator")]
        public string VisitContractCoordinator { get; set; }
        [AuditNameAttribute("Visit Contract Coordinator Code")]
        public string VisitContractCoordinatorCode { get; set; }
        [AuditNameAttribute("Visit Contract Number")]
        public string VisitContractNumber { get; set; }
        [AuditNameAttribute("Visit Customer Code")]
        public string VisitCustomerCode { get; set; }
        [AuditNameAttribute("Visit Customer Contract Number")]
        public string VisitCustomerContractNumber { get; set; }
        [AuditNameAttribute("Visit Customer Name")]
        public string VisitCustomerName { get; set; }
        [AuditNameAttribute("Visit Project Name")]
        public string VisitCustomerProjectName { get; set; }

        public string VisitCustomerProjectNumber { get; set; }
        [AuditNameAttribute("Visit Operating Company Code")]
        public string VisitOperatingCompanyCode { get; set; }
        [AuditNameAttribute("Visit Operating Company")]
        public string VisitOperatingCompany { get; set; }
        [AuditNameAttribute("Visit Operating Company Coordinator")]
        public string VisitOperatingCompanyCoordinator { get; set; }
        [AuditNameAttribute("Visit Operating Company Coordinator Sam Acct Name")]
        public string VisitOperatingCompanyCoordinatorSamAcctName { get; set; }
        [AuditNameAttribute("Visit Operating Company Coordinator Code")]
        public string VisitOperatingCompanyCoordinatorCode { get; set; }
        [AuditNameAttribute("Visit Project Number")]
        public int? VisitProjectNumber { get; set; }
        [AuditNameAttribute("Supplier Name")]
        public string VisitSupplier { get; set; }

        public int VisitSupplierPOId { get; set; }
        [AuditNameAttribute("Visit Supplier PO Number")]
        public string VisitSupplierPONumber { get; set; }

        public IList<TechnicalSpecialist> TechSpecialists { get; set; }
        [AuditNameAttribute("Visit Future Days")]
        public int? VisitFutureDays { get; set; }
        [AuditNameAttribute("Visit Notification Reference")]
        public string VisitNotificationReference { get; set; }
        [AuditNameAttribute("Visit Report Number")]
        public string VisitReportNumber { get; set; }
        [AuditNameAttribute("Visit From Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime VisitStartDate { get; set; }
        [AuditNameAttribute("Visit Status")]
        public string VisitStatus { get; set; }
        [AuditNameAttribute("Unused Reason")]
        public string UnusedReason { get; set; }
        [AuditNameAttribute("Status Description")]
        public string VisitStatusDescription { get; set; }
        [AuditNameAttribute("Final Visit")]
        public string FinalVisit { get; set; }
        [AuditNameAttribute("Is Final Visit")]
        public bool? IsFinalVisit { get; set; }
        //[AuditNameAttribute("Supplier Id")]
        //public int SupplierId { get; set; }
        [AuditNameAttribute("Supplier Location")]
        public string SupplierLocation { get; set; }
        [AuditNameAttribute("Job Reference")]
        public string VisitJobReference
        {
            get
            {
                return _visitJobReferenceNumber = VisitProjectNumber + "-" + VisitAssignmentNumber + "-" + VisitNumber;
            }
        }
        [AuditNameAttribute("Visit Material Description")]
        public string VisitMaterialDescription { get; set; }

        [AuditNameAttribute("Data Report Sent To Customer", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? VisitReportSentToCustomerDate { get; set; }

        public string DocumentApprovalVisitValue { get; set; }

    }

    public class HeaderList
    {
        public string Label { get; set; }
        public string Key { get; set; }
    }

    public class Result
    {
        public IList<HeaderList> Header { get; set; }
        public IList<BaseVisit> BaseVisit { get; set; }
    }
}
