using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Client
    {
        public Client()
        {
            ClientAudience = new HashSet<ClientAudience>();
        }

        public int Id { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public int? AccessTokenExpMins { get; set; }
        public int? RefreshTokenExpMins { get; set; }
        public string SeckretKey { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string TokenIssuer { get; set; }

        public virtual ICollection<ClientAudience> ClientAudience { get; set; }
    }
}
