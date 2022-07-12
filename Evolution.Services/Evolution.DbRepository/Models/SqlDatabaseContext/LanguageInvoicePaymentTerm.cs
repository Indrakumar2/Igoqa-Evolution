using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class LanguageInvoicePaymentTerm
    {
        public int Id { get; set; }
        public int InvoicePaymentTermId { get; set; }
        public int LanguageId { get; set; }
        public string Text { get; set; }
        public byte? UpdateCount { get; set; }
        public DateTime? LastModification { get; set; }
        public bool? IsActive { get; set; }

        public virtual Data InvoicePaymentTerm { get; set; }
        public virtual Data Language { get; set; }
    }
}
