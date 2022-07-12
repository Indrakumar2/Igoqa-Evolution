using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CompanyChargeSchedule
    {
        public CompanyChargeSchedule()
        {
            CompanyChgSchInspGroup = new HashSet<CompanyChgSchInspGroup>();
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int StandardChargeScheduleId { get; set; }
        public string Currency { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Company Company { get; set; }
        public virtual Data StandardChargeSchedule { get; set; }
        public virtual ICollection<CompanyChgSchInspGroup> CompanyChgSchInspGroup { get; set; }
    }
}
