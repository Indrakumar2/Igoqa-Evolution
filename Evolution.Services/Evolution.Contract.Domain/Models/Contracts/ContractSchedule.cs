using Evolution.Common.Models.Base;

namespace Evolution.Contract.Domain.Models.Contracts
{
    public class ContractSchedule : BaseModel
    {
        [AuditNameAttribute("Schedule Id")]
        public int ScheduleId { get; set; }

        [AuditNameAttribute(" Contract Number")]
        public string ContractNumber { get; set; }

        [AuditNameAttribute(" Schedule Name")]
        public string ScheduleName { get; set; }

        [AuditNameAttribute("Schedule Name to Print on Invoice")]
        public string ScheduleNameForInvoicePrint { get; set; }

        [AuditNameAttribute("Charge Currency")]
        public string ChargeCurrency { get; set; }
        
      
        public string BaseScheduleName { get; set; }
        /*AddScheduletoRF*/
       
        public int? BaseScheduleId { get; set; }

        public bool CanBeDeleted { get; set; }
    }
}
