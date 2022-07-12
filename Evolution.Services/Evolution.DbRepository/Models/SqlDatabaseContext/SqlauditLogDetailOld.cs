using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class SqlauditLogDetailOld
    {
        public long Id { get; set; }
        public long SqlAuditLogId { get; set; }
        public int SqlAuditSubModuleId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public virtual SqlauditLogEventOld SqlAuditLog { get; set; }
        public virtual SqlauditModuleOld SqlAuditSubModule { get; set; }
    }
}
