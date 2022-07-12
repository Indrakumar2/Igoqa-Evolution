using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class InterCompanyInvoiceItem
    {
        public InterCompanyInvoiceItem()
        {
            InterCompanyInvoiceItemBackup = new HashSet<InterCompanyInvoiceItemBackup>();
        }

        public int Id { get; set; }
        public int InterCompanyInvoiceId { get; set; }
        public int LineNumber { get; set; }
        public int AssignmentId { get; set; }
        public string ReferenceValue { get; set; }
        public int NumberOfVisits { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal ItemTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal? Contribution { get; set; }
        public decimal TotalInterCompanyInvoiceAmount { get; set; }
        public int? SalesTaxId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public string SalesTaxName { get; set; }
        public decimal? SalesTaxValue { get; set; }
        public string SalesTaxCode { get; set; }
        public decimal? SalesTaxTotal { get; set; }

        public virtual Assignment Assignment { get; set; }
        public virtual InterCompanyInvoice InterCompanyInvoice { get; set; }
        public virtual CompanyTax SalesTax { get; set; }
        public virtual ICollection<InterCompanyInvoiceItemBackup> InterCompanyInvoiceItemBackup { get; set; }
    }
}
