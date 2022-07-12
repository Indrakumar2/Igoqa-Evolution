using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class VisitHistory
    {
        public int Id { get; set; }
        public long VisitId { get; set; }
        public DateTime VisitHistoryDateTime { get; set; }
        public int HistoryItemId { get; set; }
        public string ChangedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public string SyncFlag { get; set; }

        public virtual Data HistoryItem { get; set; }
        public virtual Visit Visit { get; set; }
    }
}
