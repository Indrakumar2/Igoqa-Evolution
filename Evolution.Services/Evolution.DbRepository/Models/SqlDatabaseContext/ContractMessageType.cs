using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class ContractMessageType
    {
        public ContractMessageType()
        {
            ContractMessage = new HashSet<ContractMessage>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<ContractMessage> ContractMessage { get; set; }
    }
}
