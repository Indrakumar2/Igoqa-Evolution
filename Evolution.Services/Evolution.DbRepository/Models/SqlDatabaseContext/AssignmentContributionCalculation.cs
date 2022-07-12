using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class AssignmentContributionCalculation
    {
        public AssignmentContributionCalculation()
        {
            AssignmentContributionRevenueCost = new HashSet<AssignmentContributionRevenueCost>();
        }

        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public decimal TotalContributionValue { get; set; }
        public decimal ContractHolderPercentage { get; set; }
        public decimal? OperatingCompanyPercentage { get; set; }
        public decimal? CountryCompanyPercentage { get; set; }
        public decimal ContractHolderValue { get; set; }
        public decimal? OperatingCompanyValue { get; set; }
        public decimal? CountryCompanyValue { get; set; }
        public decimal? MarkupPercentage { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Assignment Assignment { get; set; }
        public virtual ICollection<AssignmentContributionRevenueCost> AssignmentContributionRevenueCost { get; set; }
    }
}
