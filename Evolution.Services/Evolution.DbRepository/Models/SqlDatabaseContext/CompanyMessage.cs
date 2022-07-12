using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CompanyMessage
    {
        public CompanyMessage()
        {
            ContractDefaultFooterText = new HashSet<Contract>();
            ContractDefaultRemittanceText = new HashSet<Contract>();
            ProjectInvoiceFooterText = new HashSet<Project>();
            ProjectInvoiceRemittanceText = new HashSet<Project>();
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Identifier { get; set; }
        public int MessageTypeId { get; set; }
        public string Message { get; set; }
        public bool? IsDefaultMessage { get; set; }
        public DateTime? LastModification { get; set; }
        public bool? IsActive { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public int? Evo1Id { get; set; }

        public virtual Company Company { get; set; }
        public virtual CompanyMessageType MessageType { get; set; }
        public virtual ICollection<Contract> ContractDefaultFooterText { get; set; }
        public virtual ICollection<Contract> ContractDefaultRemittanceText { get; set; }
        public virtual ICollection<Project> ProjectInvoiceFooterText { get; set; }
        public virtual ICollection<Project> ProjectInvoiceRemittanceText { get; set; }
    }
}
