using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class SqlauditLogEvent
    {
        public SqlauditLogEvent()
        {
            SqlauditLogDetail = new HashSet<SqlauditLogDetail>();
        }

        public long Id { get; set; }
        public int SqlAuditModuleId { get; set; }
        public string ActionBy { get; set; }
        public string ActionType { get; set; }
        public DateTime ActionOn { get; set; }
        public string SearchReference { get; set; }

        public virtual SqlauditModule SqlAuditModule { get; set; }
        public virtual ICollection<SqlauditLogDetail> SqlauditLogDetail { get; set; }
    }
}
