using Evolution.Common.Models.Base;

namespace Evolution.Company.Domain.Models.Companies
{
    public class CompanyExpectedMargin : BaseModel
    {
        [AuditNameAttribute("Company Expected Margin Id")]
        public int? CompanyExpectedMarginId { get; set; }

        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        [AuditNameAttribute(" Margin Type")]
        public string MarginType { get; set; }
        
        [AuditNameAttribute(" Minimum Margin")]
        public decimal MinimumMargin { get; set; }
    }
}
