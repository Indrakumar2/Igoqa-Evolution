using Evolution.Common.Models.Base;
using System.Collections.Generic;

namespace Evolution.Assignment.Domain.Models.Assignments
{
    public class AssignmentContributionCalculation : BaseModel
    {
        [AuditNameAttribute("Assignment Contract Calculation Id")]
        public int? AssignmentContCalculationId { get; set; }

        [AuditNameAttribute("Assignment Id")]
        public int? AssignmentId { get; set; 
        }
        [AuditNameAttribute("Total Contribution Value")]
        public decimal TotalContributionValue { get; set; }

        [AuditNameAttribute("Contract Holder Percentage")]
        public decimal ContractHolderPercentage { get; set; }

        [AuditNameAttribute("Operating Company Percentage")]
        public decimal? OperatingCompanyPercentage { get; set; }


        [AuditNameAttribute("Country Company Percentage")]
        public decimal? CountryCompanyPercentage { get; set; }

        [AuditNameAttribute("Contract Holder Value ")]
        public decimal ContractHolderValue { get; set; }

        [AuditNameAttribute("Operating Company Value")]
        public decimal? OperatingCompanyValue { get; set; }

        [AuditNameAttribute("Country Company Value")]
        public decimal? CountryCompanyValue { get; set; }

        [AuditNameAttribute("Markup Percentage ")]
        public decimal? MarkupPercentage { get; set; }

        
        public IList<AssignmentContributionRevenueCost> AssignmentContributionRevenueCosts { get; set; }
    }
    public class AssignmentContributionRevenueCost : BaseModel
    {
        [AuditNameAttribute("Contribution Revenue Cost Id ")]
        public int? AssignmentContributionRevenueCostId { get; set; }

        [AuditNameAttribute("Contribution Calculation Id")]
        public int? AssignmentContributionCalculationId { get; set; }

        [AuditNameAttribute("Type")]
        public string Type { get; set; }

        [AuditNameAttribute("Value")]
        public decimal? Value { get; set; }
        
        [AuditNameAttribute("Description")]
        public string Description { get; set; }
    }
}
