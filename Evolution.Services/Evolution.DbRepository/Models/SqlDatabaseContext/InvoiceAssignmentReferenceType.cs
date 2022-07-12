using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class InvoiceAssignmentReferenceType
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int SortOrder { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public int? InvoiceReferenceTypeId { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual Data InvoiceReferenceType { get; set; }
    }
}
