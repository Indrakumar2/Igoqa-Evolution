namespace Evolution.Web.Gateway.Models
{
    public class ContractAggregatorResponse
    {
        public object ContractInfo { get; set; }

        public object ContractExchangeRates { get; set; }

        public object ContractInvoiceAttachments { get; set; }

        public object ContractInvoiceReferences { get; set; }

        public object ContractSchedules { get; set; }

        public object ContractScheduleRates { get; set; }
        
        public object ContractNotes { get; set; }

        public object ContractDocuments { get; set; }


    }
}
