using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class SqlauditModuleOld
    {
        public SqlauditModuleOld()
        {
            AuditSearchOld = new HashSet<AuditSearchOld>();
            SqlauditLogDetailOld = new HashSet<SqlauditLogDetailOld>();
            SqlauditLogEventOld = new HashSet<SqlauditLogEventOld>();
        }

        public int Id { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
        public bool? IsSearchEnabled { get; set; }

        public virtual ICollection<AuditSearchOld> AuditSearchOld { get; set; }
        public virtual ICollection<SqlauditLogDetailOld> SqlauditLogDetailOld { get; set; }
        public virtual ICollection<SqlauditLogEventOld> SqlauditLogEventOld { get; set; }
    }
}
