using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class AssignmentInterCompanyDiscount
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public string DiscountType { get; set; }
        public int CompanyId { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Assignment Assignment { get; set; }
        public virtual Company Company { get; set; }
        public String  AmendmentReason { get; set; }
}
}
