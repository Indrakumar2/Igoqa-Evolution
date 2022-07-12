using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int? CompanyId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public int ApplicationId { get; set; }

        public virtual Application Application { get; set; }
        public virtual Company Company { get; set; }
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
