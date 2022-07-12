using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class InterCompanyTransfer
    {
        public InterCompanyTransfer()
        {
            InterCompanyInvoice = new HashSet<InterCompanyInvoice>();
        }

        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string InterCompanyTransferType { get; set; }
        public DateTime TransferDate { get; set; }
        public int FromCompanyId { get; set; }
        public int FromCompanyMiiwaref { get; set; }
        public int ToCompanyId { get; set; }
        public int ToCompanyMiiwaref { get; set; }
        public string ToCompanyAccountRef { get; set; }
        public DateTime WorkFromDate { get; set; }
        public DateTime WorkToDate { get; set; }
        public string Currency { get; set; }
        public decimal? TransferValue { get; set; }
        public decimal? TotalValue { get; set; }
        public string NativeCurrency { get; set; }
        public decimal? NativeTransferValue { get; set; }
        public decimal? NativeTotalValue { get; set; }
        public bool InterCompanyInvoiceRequired { get; set; }
        public string InterCompanyInvoiceStatus { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Company FromCompany { get; set; }
        public virtual Invoice Invoice { get; set; }
        public virtual Company ToCompany { get; set; }
        public virtual ICollection<InterCompanyInvoice> InterCompanyInvoice { get; set; }
    }
}
