using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.AuditLog.Domain.Models.Audit
{
    public class SqlAuditLogEventBaseInfo 
    {
        public long? LogId { get; set; }
        public long? AuditEventId { get; set; }
        public int? AuditModuleId { get; set; }
        public string AuditModuleName { get; set; }
        public string SelectType { get; set; }
        public string ActionBy { get; set; }
        public string ActionType { get; set; }
        public string SearchReference { get; set; }
    }

    public class SqlAuditLogEventInfo : SqlAuditLogEventBaseInfo
    {
        public DateTime? ActionOn { get; set; }
    }

    public class SqlAuditLogDetailInfo : SqlAuditLogEventInfo
    {
        public string AuditSubModuleName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string DiffValue { get; set; }
    }

    public class SqlAuditLogEventSearchInfo : SqlAuditLogEventBaseInfo
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
