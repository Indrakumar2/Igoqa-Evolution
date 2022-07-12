using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class SqlauditLogEventOld
    {
        public SqlauditLogEventOld()
        {
            SqlauditLogDetailOld = new HashSet<SqlauditLogDetailOld>();
        }

        public long Id { get; set; }
        public int SqlAuditModuleId { get; set; }
        public string ActionBy { get; set; }
        public string ActionType { get; set; }
        public DateTime ActionOn { get; set; }
        public string SearchReference { get; set; }

        public virtual SqlauditModuleOld SqlAuditModule { get; set; }
        public virtual ICollection<SqlauditLogDetailOld> SqlauditLogDetailOld { get; set; }
    }
}
