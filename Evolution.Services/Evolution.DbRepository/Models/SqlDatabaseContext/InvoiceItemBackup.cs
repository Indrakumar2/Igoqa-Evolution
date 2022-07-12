using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class InvoiceItemBackup
    {
        public int Id { get; set; }
        public int InvoiceItemId { get; set; }
        public int SpecialistAccountItemId { get; set; }
        public string Currency { get; set; }
        public decimal Units { get; set; }
        public decimal Rate { get; set; }
        public decimal Net { get; set; }
        public int? SalesTaxId { get; set; }
        public decimal ItemTotal { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal ConvertedNet { get; set; }
        public decimal ConvertedSalesTax { get; set; }
        public decimal ConvertedItemTotal { get; set; }
        public DateTime LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public string SalesTaxName { get; set; }
        public string SalesTaxCode { get; set; }
        public decimal? SalesTaxValue { get; set; }
        public decimal? SalesTaxTotal { get; set; }
        public string ItemType { get; set; }

        public virtual InvoiceItem InvoiceItem { get; set; }
        public virtual CompanyTax SalesTax { get; set; }
    }
}
