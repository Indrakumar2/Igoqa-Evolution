using Evolution.Common.Models.Base;

namespace Evolution.Visit.Domain.Models.Visits
{
    public class VisitInterCompanyDiscounts : BaseModel
    {
        [AuditNameAttribute("Discount Id")]
        public int? VisitInterCompanyDiscountId { get; set; }

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

        [AuditNameAttribute("Visit Id")]
        public long VisitId { get; set; }

        [AuditNameAttribute("Amendment Reaon")]
        public string AmendmentReason { get; set; }

    }
}
