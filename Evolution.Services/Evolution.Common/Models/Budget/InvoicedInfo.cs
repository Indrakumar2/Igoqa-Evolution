namespace Evolution.Common.Models.Budget
{
    public class InvoicedInfo
    {
        public int ContractId { get; set; }

        public int? ParentContractId { get; set; }

        public int ProjectId { get; set; }

        public int AssignmentId { get; set; }

        public string ContractNumber { get; set; }

        public int ProjectNumber { get; set; }

        public int AssignmentNumber { get; set; } 

        public decimal InvoicedToDate { get; set; }

        public decimal UninvoicedToDate { get; set; }  

        public decimal HoursInvoicedToDate { get; set; }

        public decimal HoursUninvoicedToDate { get; set; }

    }
}
