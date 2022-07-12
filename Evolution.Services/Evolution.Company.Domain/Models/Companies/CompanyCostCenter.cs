using Evolution.Common.Models.Base;

namespace Evolution.Company.Domain.Models.Companies
{
    public class CompanyDivisionCostCenter : BaseModel
    {
        [AuditNameAttribute("Company Division Cost Center Id")]
        public int? CompanyDivisionCostCenterId { get; set; }

        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        [AuditNameAttribute("Company Division Id")]
        public int? CompanyDivisionId { get; set; }

        [AuditNameAttribute("Division")]
        public string Division { get; set; }

        [AuditNameAttribute("Cost Center Code ")]
        public string CostCenterCode { get; set; }

        [AuditNameAttribute("Cost Center Name")]
        public string CostCenterName { get; set; }
    }
}
