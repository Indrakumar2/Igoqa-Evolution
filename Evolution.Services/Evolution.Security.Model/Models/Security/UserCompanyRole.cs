using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Security.Domain.Models.Security
{

    public class UserCompanyRole

    {
        public string CompanyCode { get; set; }

        public string CompanyName { get; set; }

        public string RoleName { get; set; }

        public IList<UserRoleActivity> RoleActivities { get; set; }
    }

    public class UserRoleActivity
    {
        public string ActivityCode { get; set; }

        public string ActivityName { get; set; }

        public string ModuleName { get; set; }
    }

}
