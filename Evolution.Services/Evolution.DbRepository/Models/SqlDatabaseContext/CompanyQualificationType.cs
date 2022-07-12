using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CompanyQualificationType
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int QualificationTypeId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Company Company { get; set; }
        public virtual Data QualificationType { get; set; }
    }
}
