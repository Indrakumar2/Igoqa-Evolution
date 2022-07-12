using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Enums;
using Evolution.Common.Models.Base;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistDraft : BaseTechnicalSpecialistModel
    {
        [AuditNameAttribute("Specialist Id")]
        public int Id { get; set; }
         [AuditNameAttribute("Draft Id")]
        public string DraftId { get; set; }
         [AuditNameAttribute("Module Type")]
        public string Moduletype { get; set; }
         [AuditNameAttribute("Description")]
        public string Description { get; set; }
         [AuditNameAttribute("Serilizable Objects")]
        public string SerilizableObject { get; set; }
         [AuditNameAttribute("Serilization Type")]
        public string SerilizationType { get; set; }
         [AuditNameAttribute("Assigned By")]
        public string AssignedBy { get; set; }
         [AuditNameAttribute("Assigned To")]
        public string AssignedTo { get; set; }
         [AuditNameAttribute("Assigned On", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? AssignedOn { get; set; }
         [AuditNameAttribute("Created By ")]
        public string CreatedBy { get; set; }
         [AuditNameAttribute("Created On", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? CreatedOn { get; set; }
    }
}
