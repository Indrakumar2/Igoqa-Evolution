using System;
using System.Collections.Generic;
using System.Text;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Domain.Models.Visits
{
    public class VisitValidationData
    {
        [AuditNameAttribute("First Visit Id")]
        public long? FirstVisitId { get; set; }
        [AuditNameAttribute("Last Visit Id")]
        public long? LastVisitId { get; set; }
        [AuditNameAttribute("Last Visit Number")]
         public int? LastVisitNumber { get; set; }
        [AuditNameAttribute("Final Visit Id")]
        public long? FinalVisitId {get;set;}
        [AuditNameAttribute("Has Final Visit")]
        public bool HasFinalVisit { get; set; }
        [AuditNameAttribute("Has TBA Status Visit")]
        public bool HasTBAStatusVisit {get;set;}
        [AuditNameAttribute("Awaiting Approval Visit Id")]
        public long AwaitingApprovalVisitId {get;set;}
        public bool HasRateUnitFinalVisit { get; set; }
        public List<DomainModel.BaseVisit> SupplierPerformanceNCRDate {get;set;}
        public List<DomainModel.BaseVisit> VisitAssignmentDates {get;set;}
        public List<DomainModel.BaseVisit> Visits {get;set;}
        public IList<DomainModel.VisitTechnicalSpecialist> TechnicalSpecialists { get; set; }
    }
}
