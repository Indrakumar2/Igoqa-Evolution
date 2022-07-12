using System;
using Evolution.Common.Models.Base;

namespace Evolution.Visit.Domain.Models.Visits
{
    /// <summary>
    ///  This will Contain information on SupplierPerformanceType
    /// </summary>
    public class VisitSupplierPerformanceType : BaseModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        [AuditNameAttribute("Visit Supplier Performance Type Id")]
        public long? VisitSupplierPerformanceTypeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [AuditNameAttribute("Visit Id")]
        public long? VisitId { get; set; }
        /// <summary>
        ///Defines SupplierPerformance
        /// </summary>
        /// 
        [AuditNameAttribute("Supplier Performance")]
        public string SupplierPerformance { get; set; }
        /// <summary>
        ///Defines NCR Reference(Score)
        /// </summary>
        /// 

        [AuditNameAttribute("NCR Reference")]
        public string NCRReference { get; set; }
        /// <summary>
        ///Defines NCR Closure Date
        /// </summary>
        /// 

        [AuditNameAttribute("NCR Close Out Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? NCRCloseOutDate { get; set; }
    }
}
