using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class VisitSupplierPerformance
    {
        public long Id { get; set; }
        public long VisitId { get; set; }
        public string PerformanceType { get; set; }
        public string Score { get; set; }
        public DateTime? NcrcloseOutDate { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Visit Visit { get; set; }
    }
}
