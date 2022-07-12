using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CustomerUserProjectAccess
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public byte? UpdateCount { get; set; }
        public DateTime? LastModification { get; set; }

        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
    }
}
