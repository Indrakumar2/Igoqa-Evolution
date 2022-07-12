using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Audience
    {
        public Audience()
        {
            ClientAudience = new HashSet<ClientAudience>();
        }

        public int Id { get; set; }
        public string AudienceCode { get; set; }
        public string AudienceName { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<ClientAudience> ClientAudience { get; set; }
    }
}
