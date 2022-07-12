using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Security.Domain.Models.Security
{
    public class UserRoleInfo : BaseModel
    {
        [AuditNameAttribute("Application Name")]
        public string ApplicationName { get; set; }

        [AuditNameAttribute("User Logon Name")]
        public string UserLogonName { get; set; }

        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        [AuditNameAttribute("User Role")]
        public string RoleName { get; set; }

    }
}
