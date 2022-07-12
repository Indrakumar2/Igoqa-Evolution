using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class SqlauditModule
    {
        public SqlauditModule()
        {
            AuditSearch = new HashSet<AuditSearch>();
            SqlauditLogDetail = new HashSet<SqlauditLogDetail>();
            SqlauditLogEvent = new HashSet<SqlauditLogEvent>();
        }

        public int Id { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public bool? IsSearchEnabled { get; set; }

        public virtual ICollection<AuditSearch> AuditSearch { get; set; }
        public virtual ICollection<SqlauditLogDetail> SqlauditLogDetail { get; set; }
        public virtual ICollection<SqlauditLogEvent> SqlauditLogEvent { get; set; }
    }
}
