using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class InvoiceItem
    {
        public InvoiceItem()
        {
            InvoiceItemBackup = new HashSet<InvoiceItemBackup>();
        }

        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int LineNumber { get; set; }
        public int AssignmentId { get; set; }
        public string ReferenceValue { get; set; }
        public int NumberOfVisits { get; set; }
        public decimal Net { get; set; }
        public int? SalesTaxId { get; set; }
        public decimal ItemTotal { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public string SalesTaxName { get; set; }
        public string SalesTaxCode { get; set; }
        public decimal? SalesTaxValue { get; set; }
        public decimal? SalesTaxTotal { get; set; }

        public virtual Assignment Assignment { get; set; }
        public virtual Invoice Invoice { get; set; }
        public virtual CompanyTax SalesTax { get; set; }
        public virtual ICollection<InvoiceItemBackup> InvoiceItemBackup { get; set; }
    }
}
