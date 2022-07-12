using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class AssignmentContributionRevenueCost
    {
        public int Id { get; set; }
        public int AssignmentContributionCalculationId { get; set; }
        public string SectionType { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual AssignmentContributionCalculation AssignmentContributionCalculation { get; set; }
    }
}
