using Evolution.Common.Models.Base;
using System;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistPayRateInfo : BaseTechnicalSpecialistModel
    {
        [AuditNameAttribute("Technical Specialist Pay Rate Info Id ")]
        public int? Id { get; set; }
        [AuditNameAttribute("ePin")]
        public int? Epin { get; set; }
        [AuditNameAttribute("Pay Schedule Id")]
        public int? PayScheduleId { get; set; }
        [AuditNameAttribute("Pay Schedule Name")]
        public string PayScheduleName { get; set; }
        [AuditNameAttribute("Pay Schedule Currency")]
        public string PayScheduleCurrency { get; set; }
        [AuditNameAttribute("Expense Type")]
        public string ExpenseType { get; set; }
        [AuditNameAttribute("Description")]
        public string Description { get; set; }
        [AuditNameAttribute("Rate")]
        public decimal Rate { get; set; }
        [AuditNameAttribute("Is Default Pay Rate")]
        public bool IsDefaultPayRate { get; set; }
        [AuditNameAttribute("Hide On Ts Portal")]
        public bool IsHideOnTsExtranet { get; set; }
        [AuditNameAttribute("Effective From", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime EffectiveFrom { get; set; }
        [AuditNameAttribute("Effective To", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? EffectiveTo { get; set; }
        [AuditNameAttribute("Is Active")]
        public bool? IsActive { get; set; }
        //[AuditNameAttribute("Display Order")]
        public int DisplayOrder { get; set; }

    }
}
