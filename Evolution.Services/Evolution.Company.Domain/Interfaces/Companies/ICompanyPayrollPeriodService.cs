using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Company.Domain.Interfaces.Companies
{
    /// <summary>
    /// This will provide all the functionality related to Company Payroll Period.
    /// </summary>
    public interface ICompanyPayrollPeriodService
    {
        /// <summary>
        /// Save Company Payroll Period
        /// </summary>
        /// <param name="companyPayrollPeriods">List of Company Payroll Period</param>
        /// <returns>All the Saved Company Note Details</returns>
        Response SaveCompanyPayrollPeriod(string companyCode, string payrollType,int? payrollId,IList<Models.Companies.CompanyPayrollPeriod> companyPayrollPeriods, bool commitChange = true);

        /// <summary>
        /// Modify list of Company Payroll Period
        /// </summary>
        /// <param name="companyPayrollPeriods">List of Company Payroll Period which need to update.</param>
        Response ModifyCompanyPayrollPeriod(string companyCode, string payrollType, int? payrollId, IList<Models.Companies.CompanyPayrollPeriod> companyPayrollPeriods, bool commitChange = true);

        /// <summary>
        /// Delete list of Company Payroll Period
        /// </summary>
        /// <param name="companyPayrollPeriods">List of Company Payroll Period which need to delete.</param>
        Response DeleteCompanyPayrollPeriod(string companyCode, string payrollType,int?payrollId, IList<Models.Companies.CompanyPayrollPeriod> companyPayrollPeriods, bool commitChange = true);

        Response GetCompanyPayrollPeriod(Models.Companies.CompanyPayrollPeriod searchModel);
    }
}
