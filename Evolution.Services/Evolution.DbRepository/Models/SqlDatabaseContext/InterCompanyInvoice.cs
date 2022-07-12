using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class InterCompanyInvoice
    {
        public InterCompanyInvoice()
        {
            InterCompanyInvoiceItem = new HashSet<InterCompanyInvoiceItem>();
        }

        public int Id { get; set; }
        public int? BatchId { get; set; }
        public int CustomerInvoiceId { get; set; }
        public int? InterCompanyTransferId { get; set; }
        public string InvoiceNumber { get; set; }
        public int DraftInvoiceNumber { get; set; }
        public string InvoiceType { get; set; }
        public int RaisedByCompanyId { get; set; }
        public int RaisedAgainstCompanyId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Status { get; set; }
        public decimal InvoiceTotal { get; set; }
        public decimal Discount { get; set; }
        public bool IsShowDiscounts { get; set; }
        public bool IsSettledThroughIctms { get; set; }
        public decimal TotalInvoiceAmount { get; set; }
        public int? SalesTaxId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public string Currency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string SalesTaxName { get; set; }
        public decimal? SalesTaxValue { get; set; }
        public string SalesTaxCode { get; set; }
        public decimal? SalesTaxTotal { get; set; }

        public virtual InterCompanyInvoiceBatch Batch { get; set; }
        public virtual Invoice CustomerInvoice { get; set; }
        public virtual InterCompanyTransfer InterCompanyTransfer { get; set; }
        public virtual Company RaisedAgainstCompany { get; set; }
        public virtual Company RaisedByCompany { get; set; }
        public virtual CompanyTax SalesTax { get; set; }
        public virtual ICollection<InterCompanyInvoiceItem> InterCompanyInvoiceItem { get; set; }
    }
}
