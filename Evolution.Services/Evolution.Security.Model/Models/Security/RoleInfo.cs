using Evolution.Common.Models.Base;
using System.Collections.Generic;

namespace Evolution.Security.Domain.Models.Security
{
    public class RoleInfo : BaseModel
    {
        public int? RoleId { get; set; }

        [AuditNameAttribute("Application Name")]
        public string ApplicationName { get; set; }

        [AuditNameAttribute("User Role")]
        public string RoleName { get; set; }

        [AuditNameAttribute("User Role Description")]
        public string Description { get; set; }

        [AuditNameAttribute("Allow access during lock out")]
        public bool? IsAllowedDuringLock { get; set; }
    }
}