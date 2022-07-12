using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TimesheetRejectedCount
    {
        public int Id { get; set; }
        public int TimesheetId { get; set; }
        public string TimesheetStatus { get; set; }
        public int? RejectCount { get; set; }
        public string ModifiedBy { get; set; }
        public byte? UpdateCount { get; set; }
        public DateTime? LastModification { get; set; }
        public string SyncFlag { get; set; }
    }
}
