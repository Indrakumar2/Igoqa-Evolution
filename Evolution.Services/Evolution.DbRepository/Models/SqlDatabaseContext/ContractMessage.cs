using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class ContractMessage
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public string Identifier { get; set; }
        public int MessageTypeId { get; set; }
        public string Message { get; set; }
        public bool? IsDefaultMessage { get; set; }
        public DateTime? LastModification { get; set; }
        public bool? IsActive { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Contract Contract { get; set; }
        public virtual ContractMessageType MessageType { get; set; }
    }
}
