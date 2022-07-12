using System;
using Evolution.Common.Models.Base;

namespace Evolution.Visit.Domain.Models.Visits
{
    /// <summary>
    ///  This contains the information on visit notes
    /// </summary>
    public class VisitNote : BaseModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        [AuditNameAttribute("Visit Note Id")]
        public int? VisitNoteId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [AuditNameAttribute("Visit Id")]
        public long? VisitId { get; set; }
        /// <summary>
        ///Defines Note
        /// </summary>
        /// 
        [AuditNameAttribute("Note")]
        public string Note { get; set; }
        /// <summary>
        ///Defines who created the note
        /// </summary>
        /// 
        [AuditNameAttribute("CreatedBy")]
        public string CreatedBy { get; set; }
        /// <summary>
        ///Defines when it is created
        /// </summary>
        /// 
        [AuditNameAttribute("Created On", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        ///Defines whether visible to customer
        /// </summary>
        /// 
        [AuditNameAttribute("Visible To Customer")]
        public bool? VisibleToCustomer { get; set; }
        /// <summary>
        ///Defines whether visible to specilaist
        /// </summary>
        /// 
        [AuditNameAttribute("Visible To Specialist")]
        public bool? VisibleToSpecialist { get; set; }

        public string UserDisplayName { get; set; }
    }
}
