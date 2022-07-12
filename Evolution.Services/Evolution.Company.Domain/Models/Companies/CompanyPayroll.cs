using Evolution.Common.Models.Base;

namespace Evolution.Company.Domain.Models.Companies
{
    public class CompanyPayroll : BaseModel
    {
        [AuditNameAttribute("Company Payroll Id")]
        public int? CompanyPayrollId { get; set; }

        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        [AuditNameAttribute("Payroll Type")]
        public string PayrollType { get; set; }

        [AuditNameAttribute("Currency")]
        public string Currency { get; set; }
        
        [AuditNameAttribute("Export Prefix")]
        public string ExportPrefix { get; set; }

    }
}
