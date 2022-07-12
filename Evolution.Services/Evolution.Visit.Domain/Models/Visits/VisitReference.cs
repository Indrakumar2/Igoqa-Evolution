using Evolution.Common.Models.Base;

namespace Evolution.Visit.Domain.Models.Visits
{
    public class VisitReference : BaseModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        [AuditNameAttribute("Visit Reference Id")]
        public long? VisitReferenceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [AuditNameAttribute("Visit Id")]
        public long? VisitId { get; set; }
        /// <summary>
        ///Defines Assignment Reference Type
        /// </summary>
        /// 
        [AuditNameAttribute("Reference Type")]
        public string ReferenceType { get; set; }

        /// <summary>
        ///Defines the reference value
        /// </summary>
        /// 
        [AuditNameAttribute("Reference Value")]
        public string ReferenceValue { get; set; }

        [AuditNameAttribute("Evo Id")]
        public long? Evoid { get; set; } // These needs to be removed once DB sync done


        public int AssignmentReferenceTypeId { get; set; }

    }
}
