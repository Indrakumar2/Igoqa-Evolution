using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class SqlauditLogDetail
    {
        public long Id { get; set; }
        public long SqlAuditLogId { get; set; }
        public int SqlAuditSubModuleId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public virtual SqlauditLogEvent SqlAuditLog { get; set; }
        public virtual SqlauditModule SqlAuditSubModule { get; set; }
    }
}
