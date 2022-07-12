using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class AssignmentMessage
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public string Identifier { get; set; }
        public int MessageTypeId { get; set; }
        public string Message { get; set; }
        public DateTime? LastModification { get; set; }
        public bool? IsActive { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Assignment Assignment { get; set; }
        public virtual AssignmentMessageType MessageType { get; set; }
    }
}
