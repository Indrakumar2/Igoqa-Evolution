using Evolution.Common.Models.Base;

namespace Evolution.Timesheet.Domain.Models.Timesheets
{
    public class TimesheetInterCompanyDiscounts : BaseModel
    {
        [AuditNameAttribute("Discount Id")]
        public int? TimesheetInterCompanyDiscountId { get; set; }

        [AuditNameAttribute("Discount Type")]
        public string DiscountType { get; set; }

        [AuditNameAttribute("Company Name")]
        public string CompanyName { get; set; }

        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        [AuditNameAttribute("Discount Percentage")]
        public decimal? DiscountPercentage { get; set; }

        [AuditNameAttribute("Description")]
        public string Description { get; set; }

        [AuditNameAttribute("Timesheet Id")]
        public long TimesheetId { get; set; }
    }
}
