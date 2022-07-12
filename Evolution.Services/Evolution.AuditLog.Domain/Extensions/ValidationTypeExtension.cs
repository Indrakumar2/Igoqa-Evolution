﻿using Evolution.AuditLog.Domain.Enums;
using Evolution.Common.Enums;

namespace Evolution.AuditLog.Domain.Extensions
{
    public static class ValidationTypeExtension
    {
        public static SqlAuditActionType ToAuditActionType(this ValidationType source)
        {
            SqlAuditActionType sqlAuditActionType=SqlAuditActionType.N;
            if (source == ValidationType.Add)
                sqlAuditActionType = SqlAuditActionType.I;
            else if (source == ValidationType.Update)
                sqlAuditActionType = SqlAuditActionType.M;
            else if (source == ValidationType.Delete)
                sqlAuditActionType = SqlAuditActionType.D;

            return sqlAuditActionType;
        }
    }
}
