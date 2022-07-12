using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class UserAdditionalInfoToDelete
    {
        public int Id { get; set; }
        public string UserLogonAccount { get; set; }
        public int CompanyId { get; set; }
        public int? CompanyOfficeId { get; set; }

        public virtual Company Company { get; set; }
        public virtual CompanyOffice CompanyOffice { get; set; }
    }
}
