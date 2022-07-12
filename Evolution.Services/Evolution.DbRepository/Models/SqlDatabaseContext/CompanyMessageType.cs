using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CompanyMessageType
    {
        public CompanyMessageType()
        {
            CompanyMessage = new HashSet<CompanyMessage>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<CompanyMessage> CompanyMessage { get; set; }
    }
}
