using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;

namespace Evolution.Assignment.Domain.Models.Assignments
{
    //public  class AssignmentTechnicalSpecialistSchedule : BaseModel
    //{
    //    public int? AssignmentTechnicalSpecialistScheduleId { get; set; }

    //   // public int? AssignmentId { get; set; }

    //    public int? ContractScheduleId { get; set; }

    //    public string ContractScheduleName { get; set; }

    //    public int? AssignmentTechnicalSpecilaistId { get; set; }

    //    //public int? TechnicalSpecialistEpin { get; set; }

    //    //public string TechnicalSpecialistName { get; set; }

    //    public int? TechnicalSpecialistPayScheduleId { get; set; }

    //    public string TechnicalSpecialistPayScheduleName { get; set; }

    //    public string ScheduleNoteToPrintOnInvoice { get; set; }

    //}

    public class AssignmentTechSpecSchedules
    {
        [AuditNameAttribute("Charge Schedules")]
        public IList<TechnicalSpecialistChargeSchedule> ChargeSchedules { get; set; }
        
        [AuditNameAttribute("Pay Schedules")]
        public IList<TechnicalSpecialistPaySchedule> PaySchedules { get; set; }
    }

    public class TechnicalSpecialistChargeSchedule
    {
        [AuditNameAttribute("Assignment TechnicalSpecialist ScheduleIds")]
        public int? AssignmentTechnicalSpecialistScheduleId { get; set; }

        [AuditNameAttribute("Contract Schedule Id")]
        public int? ContractScheduleId { get; set; }

        [AuditNameAttribute("Contract Schedule Name")]
        public string ContractScheduleName { get; set; }

        [AuditNameAttribute("TechnicalSpecialistId")]
        public int? TechnicalSpecialistId { get; set; }

        [AuditNameAttribute("PIN")]
        public int? Epin { get; set; }

        [AuditNameAttribute("Charge Schedule Currency")]
        public string ChargeScheduleCurrency { get; set; }

        [AuditNameAttribute("Charge Schedule Rates")]
        public IList<ChargeScheduleRates> ChargeScheduleRates { get; set; }

        [AuditNameAttribute("Contract Company Code")]
        public string ContractCompanyCode { get; set; }
    }

    public class TechnicalSpecialistPaySchedule
    {
        [AuditNameAttribute("Assignment Technical Specilaist Id")]
        public int? AssignmentTechnicalSpecialistScheduleId { get; set; }

        [AuditNameAttribute("TechnicalSpecialist PaySchedule Id")]
        public int? TechnicalSpecialistPayScheduleId { get; set; }

        [AuditNameAttribute("TechnicalSpecialist PaySchedule Name")]
        public string TechnicalSpecialistPayScheduleName { get; set; }

        [AuditNameAttribute("TechnicalSpecialistId")]
        public int? TechnicalSpecialistId { get; set; }

        [AuditNameAttribute("PIN")]
        public int? Epin { get; set; }

        [AuditNameAttribute("Pay Schedule Currency")]
        public string PayScheduleCurrency { get; set; }

        [AuditNameAttribute("Pay Schedule Rates")]
        public IList<PayScheduleRates> PayScheduleRates { get; set; }

        [AuditNameAttribute("TechnicalSpecialist Company Code")]
        public string TechnicalSpecialistCompanyCode { get; set; }
    }

    public class ChargeScheduleRates
    {
        [AuditNameAttribute("Assignment Technical Specialistt Id")]
        public int AssignmentTechnicalSpecialistScheduleId { get; set; }
        [AuditNameAttribute("Rate Id")]
        public int RateId { get; set; }

        [AuditNameAttribute("Currency")]
        public string Currency { get; set; }

        [AuditNameAttribute("Charge TypeId")]
        public int? ChargeTypeId { get; set; }

        [AuditNameAttribute("Charge Type")]
        public string ChargeType { get; set; }

        [AuditNameAttribute("Type")]
        public string Type { get; set; }

        [AuditNameAttribute("Charge Rate")]
        public string ChargeRate { get; set; }
        
        [AuditNameAttribute(" Description")]
        public string Description { get; set; }

        [AuditNameAttribute("Effective From","dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime EffectiveFrom { get; set; }

        [AuditNameAttribute(" Effective To","dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? EffectiveTo { get; set; }

        [AuditNameAttribute("ChargeRate Type")]
        public string ChargeRateType { get; set; }

        [AuditNameAttribute("Is Active")]
        public bool? IsActive { get; set; }

        public bool? IsPrintDescriptionOnInvoice { get; set; }

    }

    public class PayScheduleRates
    {
        [AuditNameAttribute("Assignment Technical Specilaist Id")]
        public int AssignmentTechnicalSpecialistScheduleId { get; set; }
        
        [AuditNameAttribute("Rate Id")]
        public int? RateId { get; set; }

        [AuditNameAttribute("Currency")]
        public string Currency { get; set; }

        [AuditNameAttribute("Expense Type Id")]
        public int? ExpenseTypeId { get; set; }

        [AuditNameAttribute("Expense Type")]
        public string ExpenseType { get; set; }

        [AuditNameAttribute("Type")]
        public string Type { get; set; }

        [AuditNameAttribute("Description")]
        public string Description { get; set; }

        [AuditNameAttribute("PayRate")]
        public decimal PayRate { get; set; }

        public string SPayRate { get; set; }

        [AuditNameAttribute("Effective From","dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime EffectiveFrom { get; set; }

        [AuditNameAttribute("Effective To","dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? EffectiveTo { get; set; }

        [AuditNameAttribute("Is Active")]
        public bool IsActive { get; set; }

    }
}
