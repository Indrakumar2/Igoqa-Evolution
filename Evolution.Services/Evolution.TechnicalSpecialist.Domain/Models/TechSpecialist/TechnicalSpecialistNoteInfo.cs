using Evolution.Common.Models.Base;
using System;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistNoteInfo : BaseTechnicalSpecialistModel
    {
        [AuditNameAttribute("Technical Specialist Note Info Id")]
        public long Id { get; set; }
        [AuditNameAttribute("ePin")]
        public int Epin { get; set; }
        [AuditNameAttribute("Record Type")]
        public string RecordType { get; set; }
        [AuditNameAttribute("Record Ref Id")]
        public int? RecordRefId { get; set; }
        [AuditNameAttribute("Created Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        [AuditNameAttribute("Created By")]
        public string CreatedBy { get; set; }
        [AuditNameAttribute("Note")]
        public string Note { get; set; }

    }
}
