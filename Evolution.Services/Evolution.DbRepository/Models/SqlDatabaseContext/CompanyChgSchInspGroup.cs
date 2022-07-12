using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CompanyChgSchInspGroup
    {
        public CompanyChgSchInspGroup()
        {
            CompanyChgSchInspGrpInspectionType = new HashSet<CompanyChgSchInspGrpInspectionType>();
        }

        public int Id { get; set; }
        public int CompanyChargeScheduleId { get; set; }
        public int StandardInspectionGroupId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual CompanyChargeSchedule CompanyChargeSchedule { get; set; }
        public virtual Data StandardInspectionGroup { get; set; }
        public virtual ICollection<CompanyChgSchInspGrpInspectionType> CompanyChgSchInspGrpInspectionType { get; set; }
    }
}
