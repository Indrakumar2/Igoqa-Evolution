using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TimesheetHistory
    {
        public int Id { get; set; }
        public long TimesheetId { get; set; }
        public DateTime TimesheetHistoryDateTime { get; set; }
        public int HistoryItemId { get; set; }
        public string ChangedBy { get; set; }
        public DateTime LastModification { get; set; }
        public string SyncFlag { get; set; }

        public virtual Data HistoryItem { get; set; }
        public virtual Timesheet Timesheet { get; set; }
    }
}
