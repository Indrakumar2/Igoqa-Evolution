using Evolution.Common.Models.Base;

namespace Evolution.Assignment.Domain.Models.Assignments
{
    public class AssignmentInterCompanyDiscount : BaseModel
    {
        [AuditNameAttribute("Inter Company Discount Id")]
        public int? AssignmentInterCompanyDiscountId { get; set; }

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

        [AuditNameAttribute("Assignment Id")]
         public int AssignmentId { get; set; }

        public string AmendmentReason { get; set; }
    }
}