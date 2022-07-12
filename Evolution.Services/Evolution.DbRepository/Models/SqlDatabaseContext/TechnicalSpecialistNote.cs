using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistNote
    {
        public long Id { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public string RecordType { get; set; }
        public int? RecordRefId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Note { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
    }
}
