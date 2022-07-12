using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CustomerAssignmentReferenceType
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int AssignmentReferenceId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Data AssignmentReference { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
