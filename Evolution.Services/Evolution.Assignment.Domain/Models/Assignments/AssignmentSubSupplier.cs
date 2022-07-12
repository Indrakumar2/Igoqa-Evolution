using Evolution.Common.Models.Base;
using System.Collections.Generic;

namespace Evolution.Assignment.Domain.Models.Assignments
{
    public class AssignmentSubSupplier : BaseModel
    {
        
        public int? AssignmentSubSupplierId { get; set; }

        [AuditNameAttribute("Assignment Id")]
        public int? AssignmentId { get; set; }

        [AuditNameAttribute("Main Supplier Id")]
        public int? MainSupplierId { get; set; }

        [AuditNameAttribute("Main Supplier Name")]
        public string MainSupplierName { get; set; }

        [AuditNameAttribute("Main Supplier ContactId")]
        public int? MainSupplierContactId { get; set; }

        [AuditNameAttribute("Main Supplier Contact Name")]
        public string MainSupplierContactName { get; set; }

        [AuditNameAttribute("Is Main Supplier First Visit ")]
        public bool? IsMainSupplierFirstVisit { get; set; }

        public int? AssignmentSubSupplierIdForSubSupplier { get; set; } //MS-TS Link CR

        [AuditNameAttribute("Assignment Sub Supplier Id")]
        public int? AssignmentSupplierId { get; set; } //Added for IGO - D932

        [AuditNameAttribute("Sub-Supplier Id ")]
        public int? SubSupplierId { get; set; }//MS-TS Link CR

        [AuditNameAttribute("Sub-Supplier Name ")]
        public string SubSupplierName { get; set; }

        [AuditNameAttribute("Sub-Supplier Contact Id ")]
        public int? SubSupplierContactId { get; set; }

        [AuditNameAttribute("Sub-Supplier Contact Name ")]
        public string SubSupplierContactName { get; set; }

        [AuditNameAttribute("Sub Supplier First Visit")]
        public bool? IsSubSupplierFirstVisit { get; set; }

        [AuditNameAttribute("Supplier Type")]
        public string SupplierType { get; set; }//MS-TS Link CR

        public string SubSupplierAddress { get; set; }

        public bool IsDeleted { get; set; }

        public bool? IsFirstVisitAssociated { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public List<AssignmentSubSupplierTS> AssignmentSubSupplierTS { get; set; }

    }

    public class AssignmentSubSupplierTS : BaseModel
    {
        [AuditNameAttribute("Assignment SubSupplier TS Id")]
        public int? AssignmentSubSupplierTSId { get; set; }

        [AuditNameAttribute("Assignment SubSupplier Id")]
        public int? AssignmentSubSupplierId { get; set; }

        [AuditNameAttribute("ePin")]
        public int Epin { get; set; }

        [AuditNameAttribute("Assignment TechnicalSpecialist Id")]
        public int? AssignmentTechnicalSpecialistId { get; set; }

        [AuditNameAttribute("IsAssigned To This SubSupplier")]
        public bool? IsAssignedToThisSubSupplier { get; set; } //MS-TS Link CR

        public bool? IsDeleted { get; set; }
    }

    public class AssignmentSubSupplierVisit : BaseModel
    {
        [AuditNameAttribute("Assignment Sub Supplier Id")]
        public int? AssignmentSubSupplierId { get; set; }

        [AuditNameAttribute("Assignment Id")]
        public int? AssignmentId { get; set; }

        [AuditNameAttribute("Main Supplier Id")]
        public int? MainSupplierId { get; set; }

        [AuditNameAttribute("Main Supplier Name")]
        public string MainSupplierName { get; set; }

        [AuditNameAttribute("Main Supplier ContactId")]
        public int? MainSupplierContactId { get; set; }

        [AuditNameAttribute("Main Supplier Contact Name")]
        public string MainSupplierContactName { get; set; }

        [AuditNameAttribute("Main Supplier First Visit")]
        public bool? IsMainSupplierFirstVisit { get; set; }

        public int? AssignmentSubSupplierIdForSubSupplier { get; set; } //MS-TS Link CR

        [AuditNameAttribute("Sub Supplier Id")]
        public int? SubSupplierId { get; set; }//MS-TS Link CR

        [AuditNameAttribute("Sub Supplier Name")]
        public string SubSupplierName { get; set; }

        [AuditNameAttribute("Supplier Contact Id")]
        public int? SubSupplierContactId { get; set; }

        [AuditNameAttribute("Supplier Contact Name")]
        public string SubSupplierContactName { get; set; }

        [AuditNameAttribute("Is Sub-Supplier First Visit ")]
        public bool? IsSubSupplierFirstVisit { get; set; }

        [AuditNameAttribute("Supplier Type")]
        public string SupplierType { get; set; }//MS-TS Link CR

        public string SubSupplierAddress { get; set; }

        public bool IsDeleted { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public List<AssignmentSubSupplierTS> AssignmentSubSupplierTS { get; set; }

    }
}
