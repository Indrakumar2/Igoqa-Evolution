using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Contract.Domain.Models.Contracts
{
    public class ContractSearch : BaseContract
    {
        [AuditNameAttribute("Contract Future Days")]
        public int? ContractFutureDays { get; set; }

        [AuditNameAttribute(" Contract Type Not In")]
        public string ContractTypeNotIn { get; set; }

        [AuditNameAttribute(" Contract Numbers")]
        public IList<string> ContractNumbers { get; set; }
         
        [AuditNameAttribute("Search Document Type")]
        public string SearchDocumentType { get; set; }

        [AuditNameAttribute(" Document Search Text")]
        public string DocumentSearchText { get; set; }
        
    }
}
