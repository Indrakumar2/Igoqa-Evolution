using Evolution.Common.Models.Base;

namespace Evolution.Assignment.Domain.Models.Assignments
{
    // To be taken out
    public class AssignmentSubSupplierTechnicalSpecialist : BaseModel
    {
        [AuditNameAttribute("Assignment SubSupplier TechnicalSpecialist Id")]
        public int? AssignmentSubSupplierTechnicalSpecialistId { get; set; }

        [AuditNameAttribute("Assignment Id")]
        public int? AssignmentId { get; set; }

        [AuditNameAttribute("Sub Supplier Id")]
        public int SubSupplierId { get; set; }

        [AuditNameAttribute("Supplier Name")]
        public string SupplierName { get; set; }

        [AuditNameAttribute("PIN")]
        public int? Epin { get; set; }
        
        [AuditNameAttribute("TechnicalSpecialist Name")]
        public string TechnicalSpecialistName { get; set; }
    }



}
