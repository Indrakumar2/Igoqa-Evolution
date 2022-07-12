using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class ClientAudience
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int AudienceId { get; set; }
        public bool IsActive { get; set; }

        public virtual Audience Audience { get; set; }
        public virtual Client Client { get; set; }
    }
}
