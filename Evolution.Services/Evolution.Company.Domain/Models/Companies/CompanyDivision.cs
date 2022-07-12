
using Evolution.Common.Models.Base;

namespace Evolution.Company.Domain.Models.Companies
{
    public class CompanyDivision : BaseModel
    {
        [AuditNameAttribute("Company Division Id")]
        public int? CompanyDivisionId { get; set; }

        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        [AuditNameAttribute("Division Name")]
        public string DivisionName { get; set; }
        
        [AuditNameAttribute("Division Accounts Reference")]
        public string DivisionAcReference { get; set; }
    }
}
