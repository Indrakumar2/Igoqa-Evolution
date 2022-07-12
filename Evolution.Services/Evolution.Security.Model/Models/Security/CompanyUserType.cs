using System.Collections.Generic;

namespace Evolution.Security.Domain.Models.Security
{
    public class CompanyUserType
    {
        public int? CompanyId { get; set; }

        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        public IList<UserTypeInfo> UserTypes { get; set; }

        [AuditNameAttribute("Active")]
        public bool IsActive { get; set; }
    }
}
