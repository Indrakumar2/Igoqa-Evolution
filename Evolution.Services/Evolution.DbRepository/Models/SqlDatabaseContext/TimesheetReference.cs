using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TimesheetReference
    {
        public long Id { get; set; }
        public long TimesheetId { get; set; }
        public int AssignmentReferenceTypeId { get; set; }
        public string ReferenceValue { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public long? Evoid { get; set; }
        public string SyncFlag { get; set; }

        public virtual Data AssignmentReferenceType { get; set; }
        public virtual Timesheet Timesheet { get; set; }
    }
}
