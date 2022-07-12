using System;

namespace Evolution.Common.Models.Budget
{
    public class BudgetAccountItem
    {
        public int ContractId { get; set; }
        public string ContractNumber { get; set; }
        public int? ParentContractId { get; set; }
        public int ProjectId { get; set; }
        public int ProjectNumber { get; set; }
        public int AssignmentId { get; set; }
        public int AssignmentNumber { get; set; }
        public int SupplierPurchaseOrderId { get; set; }
        public string Status { get; set; }
        public decimal ChargeTotalUnit { get; set; }
        public decimal ChargeRate { get; set; }
        public decimal ChargeExchangeRate { get; set; }
        public string ChargeRateCurrency { get; set; }
        public string ExpenseType { get; set; }
        public string ExpenseName { get; set; }
        public string BudgetCurrency { get; set; }
        public DateTime VisitDate { get; set; }

        public Decimal ContractExchangeRate { get; set; }
    }
}
