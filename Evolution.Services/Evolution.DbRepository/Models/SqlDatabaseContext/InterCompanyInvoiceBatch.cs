using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class InterCompanyInvoiceBatch
    {
        public InterCompanyInvoiceBatch()
        {
            InterCompanyInvoice = new HashSet<InterCompanyInvoice>();
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string BatchReference { get; set; }
        public string BatchStatus { get; set; }
        public DateTime? LastModification { get; set; }
        public string ModifiedBy { get; set; }
        public byte? UpdateCount { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<InterCompanyInvoice> InterCompanyInvoice { get; set; }
    }
}
