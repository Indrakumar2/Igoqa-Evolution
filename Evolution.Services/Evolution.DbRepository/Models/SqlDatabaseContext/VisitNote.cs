using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class VisitNote
    {
        public int Id { get; set; }
        public long VisitId { get; set; }
        public bool? IsCustomerVisible { get; set; }
        public bool? IsSpecialistVisible { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Note { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public long? Evoid { get; set; }
        public string SyncFlag { get; set; }

        public virtual Visit Visit { get; set; }
    }
}
