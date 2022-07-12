using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Security.Domain.Models.Security
{
    public class CompanyRole
    {
        public int? CompanyId { get; set; }

        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        public IList<RoleInfo> Roles { get; set; }
    }
}
