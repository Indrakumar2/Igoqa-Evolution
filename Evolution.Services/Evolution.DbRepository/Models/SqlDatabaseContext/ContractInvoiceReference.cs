using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class ContractInvoiceReference
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public int AssignmentReferenceTypeId { get; set; }
        public byte SortOrder { get; set; }
        public DateTime? LastModification { get; set; }
        public bool? IsAssignment { get; set; }
        public bool? IsVisit { get; set; }
        public bool? IsTimesheet { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Data AssignmentReferenceType { get; set; }
        public virtual Contract Contract { get; set; }
    }
}
