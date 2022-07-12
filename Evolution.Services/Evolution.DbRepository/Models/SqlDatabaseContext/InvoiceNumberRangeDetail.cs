using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class InvoiceNumberRangeDetail
    {
        public int Id { get; set; }
        public int InvoiceNumberRangeId { get; set; }
        public string RangeType { get; set; }
        public int? FirstNumber { get; set; }
        public int? NextNumber { get; set; }
        public int? LastNumber { get; set; }
        public bool? IsInvoiceRangeChecked { get; set; }
        public bool? IsCreditNoteRangeChecked { get; set; }
        public bool? IsInterCoInvoiceRangeChecked { get; set; }
        public bool? IsInterCoCreditNoterangeChecked { get; set; }
        public bool? IsOverrideAllowed { get; set; }
        public bool? IsInterCoInvoiceAutoCommit { get; set; }
        public bool? IsInterCoCreditNoteAutoCommit { get; set; }
        public bool? IsAlternateLanguageInvoicesAllowed { get; set; }
        public string Language { get; set; }
        public byte? UpdateCount { get; set; }
        public DateTime? LastModification { get; set; }

        public virtual InvoiceNumberRange InvoiceNumberRange { get; set; }
    }
}
