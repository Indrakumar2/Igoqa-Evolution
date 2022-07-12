using Evolution.Common.Models.Documents;
using Evolution.Document.Domain.Models.Document;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Contract.Domain.Models.Contracts
{
    public class ContractDetail
    {
        public Contract ContractInfo { get; set; }

        public IList<ContractExchangeRate> ContractExchangeRates { get; set; }

        public IList<ContractInvoiceAttachment> ContractInvoiceAttachments { get; set; }

        public IList<ContractInvoiceReferenceType> ContractInvoiceReferences { get; set; } 

        public IList<ContractSchedule> ContractSchedules { get; set; }

        public IList<ContractScheduleRate> ContractScheduleRates { get; set; }
          
        public IList<ContractNote> ContractNotes { get; set; }

        public IList<ModuleDocument> ContractDocuments { get; set; }
    }
}
