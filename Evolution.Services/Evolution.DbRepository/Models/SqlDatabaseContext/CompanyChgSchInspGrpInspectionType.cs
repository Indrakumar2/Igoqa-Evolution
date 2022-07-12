using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CompanyChgSchInspGrpInspectionType
    {
        public CompanyChgSchInspGrpInspectionType()
        {
            CompanyInspectionTypeChargeRate = new HashSet<CompanyInspectionTypeChargeRate>();
        }

        public int Id { get; set; }
        public int StandardInspectionTypeId { get; set; }
        public int CompanyChgSchInspGroupId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual CompanyChgSchInspGroup CompanyChgSchInspGroup { get; set; }
        public virtual Data StandardInspectionType { get; set; }
        public virtual ICollection<CompanyInspectionTypeChargeRate> CompanyInspectionTypeChargeRate { get; set; }
    }
}
