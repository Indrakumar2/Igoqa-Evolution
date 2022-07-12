using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evolution.Company.Domain.Models.Companies
{
    public class CompanyPayrollPeriod : BaseModel
    {
        [AuditNameAttribute("Company Payroll Period Id")]
        public int? CompanyPayrollPeriodId { get; set; }

        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        [AuditNameAttribute("Company Payroll Id")]
        public int? CompanyPayrollId { get; set; }
        
        [AuditNameAttribute("Payroll Type ")]
        public string PayrollType { get; set; }

        [AuditNameAttribute("Period Name")]
        public string PeriodName { get; set; }

        [AuditNameAttribute("Start Date","dd-MMM-yyyy",AuditNameformatDataType.DateTime)]
        public DateTime StartDate { get; set; }

        [AuditNameAttribute("End Date","dd-MMM-yyyy",AuditNameformatDataType.DateTime)]
        public DateTime EndDate { get; set; }

        [AuditNameAttribute("Period Status")]
        public string PeriodStatus { get; set; }

        [AuditNameAttribute("Active ")]
        public bool? IsActive { get; set; }

    }
}
