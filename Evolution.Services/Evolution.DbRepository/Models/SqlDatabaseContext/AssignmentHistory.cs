using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class AssignmentHistory
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public DateTime AssignmentHistoryDateTime { get; set; }
        public int HistoryItemId { get; set; }
        public string ChangedBy { get; set; }
        public DateTime? LastModification { get; set; }

        public virtual Assignment Assignment { get; set; }
        public virtual Data HistoryItem { get; set; }
    }
}
