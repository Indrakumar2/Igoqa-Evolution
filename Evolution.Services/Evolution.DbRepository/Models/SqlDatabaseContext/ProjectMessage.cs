using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class ProjectMessage
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Identifier { get; set; }
        public int MessageTypeId { get; set; }
        public string Message { get; set; }
        public bool? IsDefaultMessage { get; set; }
        public DateTime? LastModification { get; set; }
        public bool? IsActive { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ProjectMessageType MessageType { get; set; }
        public virtual Project Project { get; set; }
    }
}
