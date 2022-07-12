using Evolution.Common.Models.Base;
using System;

namespace Evolution.Contract.Domain.Models.Contracts
{
    public class ContractScheduleRate : BaseModel
    {
        [AuditNameAttribute("Rate Id")]
        public int RateId { get; set; }

        [AuditNameAttribute(" Contract Number")]
        public string ContractNumber { get; set; }

        [AuditNameAttribute("Schedule Name")]
        public string ScheduleName { get; set; }

        [AuditNameAttribute("Currency")]
        public string Currency { get; set; }

        [AuditNameAttribute("Charge Type")]
        public string ChargeType { get; set; }

        [AuditNameAttribute("Standard Value")]
        public decimal StandardValue { get; set; }

        [AuditNameAttribute(" Charge Value")]
        public string ChargeValue { get; set; }

        [AuditNameAttribute("Description")]
        public string Description { get; set; }
        
        [AuditNameAttribute("Discount Applied")]
        public string DiscountApplied { get; set; }

        [AuditNameAttribute("Percentage")]
        public decimal Percentage { get; set; }

        [AuditNameAttribute("Description Printed On Invoice")]
        public bool? IsDescriptionPrintedOnInvoice { get; set; }

        [AuditNameAttribute("Effective From","dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime EffectiveFrom { get; set; }

        [AuditNameAttribute("Effective To","dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? EffectiveTo { get; set; }

        [AuditNameAttribute("Standard Charge Schedule")]
        public string StandardChargeSchedule { get; set; }

        [AuditNameAttribute("Standard Charge Schedule Inspection Group")]
        public string StandardChargeScheduleInspectionGroup { get; set; }

        [AuditNameAttribute("Standard Charge Schedule Inspection Type")]
        public string StandardChargeScheduleInspectionType { get; set; }

        [AuditNameAttribute("Standard Inspection Type Charge RateId")]
        public int? StandardInspectionTypeChargeRateId { get; set; }

        [AuditNameAttribute("Base Schedule Name")]
        public string BaseScheduleName { get; set; }
        
        public int? BaseRateId { get; set; }

        [AuditNameAttribute("Charge Rate Type")]
        public string ChargeRateType { get; set; }
        
        [AuditNameAttribute("Active")]
        public bool? IsActive { get; set; }
        /*AddScheduletoRF*/
        
        public int? BaseScheduleId { get; set; }

        public int ScheduleId { get; set; }

        public int ExpenseTypeId { get; set; }

    }
}
