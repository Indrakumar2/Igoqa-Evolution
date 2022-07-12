using Evolution.Common.Models.Base;

namespace Evolution.Assignment.Domain.Models.Assignments
{
    public class AssignmentAdditionalExpense : BaseModel
    {
        [AuditNameAttribute("Assignment Additional Expense Id ")]
        public int? AssignmentAdditionalExpenseId { get; set; }

        [AuditNameAttribute("Assignment Id ")]
        public int? AssignmentId { get; set; }

        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        [AuditNameAttribute("Company Name ")]
        public string CompanyName { get; set; }

        [AuditNameAttribute("Description")]
        public string Description { get; set; }

        [AuditNameAttribute("Currency Code")]
        public string CurrencyCode { get; set; }

        [AuditNameAttribute("Expense Type ")]
        public string ExpenseType { get; set; }
        
        [AuditNameAttribute("Per Unit Rate")]
        public string PerUnitRate { get; set; }

        [AuditNameAttribute("Total Unit ")]
        public decimal? TotalUnit { get; set; }
        
        [AuditNameAttribute("Is Already Linked ")]
        public bool? IsAlreadyLinked { get; set; }
    }
}