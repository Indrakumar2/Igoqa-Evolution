using System.Collections.Generic;

namespace Evolution.Security.Domain.Models.Security
{
    public class UserDetail
    {
        public UserInfo User { get; set; } 

        public IList<CompanyRole> CompanyRoles { get; set; }

        public IList<CompanyUserType> CompanyUserTypes { get; set; }
    }
}