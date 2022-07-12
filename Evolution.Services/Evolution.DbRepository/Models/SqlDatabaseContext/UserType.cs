using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class UserType
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserTypeName { get; set; }
        public int CompanyId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsActive { get; set; }

        public virtual Company Company { get; set; }
        public virtual User User { get; set; }
    }
}
