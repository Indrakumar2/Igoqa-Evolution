using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class ProjectInvoiceAssignmentReference
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int AssignmentReferenceTypeId { get; set; }
        public byte SortOrder { get; set; }
        public bool IsAssignment { get; set; }
        public bool IsVisit { get; set; }
        public bool IsTimesheet { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Data AssignmentReferenceType { get; set; }
        public virtual Project Project { get; set; }
    }
}
