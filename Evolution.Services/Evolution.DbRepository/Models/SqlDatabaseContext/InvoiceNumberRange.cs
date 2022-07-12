using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class InvoiceNumberRange
    {
        public InvoiceNumberRange()
        {
            InvoiceNumberRangeDetail = new HashSet<InvoiceNumberRangeDetail>();
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int NextDraftNumber { get; set; }
        public string Notes { get; set; }
        public string Language { get; set; }
        public byte? UpdateCount { get; set; }
        public DateTime? LastModification { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<InvoiceNumberRangeDetail> InvoiceNumberRangeDetail { get; set; }
    }
}
