using Evolution.Common.Models.Notes;

namespace Evolution.Contract.Domain.Models.Contracts
{
    public class ContractNote : Note
    {
        [AuditNameAttribute("Contract Note Id")]
        public int ContractNoteId { get; set; }

        [AuditNameAttribute("Contract Number")]
        public string ContractNumber { get; set; }
        
    }
}
