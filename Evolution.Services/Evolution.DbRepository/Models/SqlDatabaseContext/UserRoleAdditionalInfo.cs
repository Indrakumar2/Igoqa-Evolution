using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class UserRoleAdditionalInfo
    {
        public long Id { get; set; }
        public string UserLogOnName { get; set; }
        public string UserRoleRef { get; set; }
        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }
    }
}
