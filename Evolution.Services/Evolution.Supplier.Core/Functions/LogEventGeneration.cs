using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Supplier.Core.Functions
{
    public  class LogEventGeneration
    {
        private readonly IAuditLogger _auditLogger = null;
        private readonly IAppLogger<LogEventGeneration> _logger = null;
        public LogEventGeneration(IAuditLogger auditLogger, IAppLogger<LogEventGeneration> logger)
        {
            this._auditLogger = auditLogger;
            this._logger = logger;
        }

        public long GetEventLogId(long? eventId, SqlAuditActionType type, string actionBy, string searchRef)
        {
            Exception exception = null;
            try
            {
                if (!eventId.HasValue || eventId == 0)
                {
                    var auditResponse = this._auditLogger.LogAuditEvent(SqlAuditModuleType.Supplier, type, actionBy, searchRef);
                    if (auditResponse.Code == ResponseType.Success.ToId())
                        eventId = auditResponse.Result?.Populate<IList<long>>()?.FirstOrDefault();
                    else
                        _logger.LogError(ResponseType.Exception.ToId(), string.Empty, searchRef);
                }
            }
             catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchRef);
            }

            return (long)eventId;
        }
    }
}
