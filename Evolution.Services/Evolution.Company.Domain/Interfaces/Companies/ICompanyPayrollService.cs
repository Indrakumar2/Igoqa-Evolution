using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Company.Domain.Interfaces.Companies
{
    /// <summary>
    /// This will provide all the functionality related to Company Payroll.
    /// </summary>
    public interface ICompanyPayrollService
    {
        /// <summary>
        /// Save Company Payrolls
        /// </summary>
        /// <param name="companyPayroll">List of Company Payrolls</param>
        /// <returns>All the Saved Company Payroll Details</returns>
        Response SaveCompanyPayroll(string companyCode, IList<Models.Companies.CompanyPayroll> companyPayroll, bool commitChange = true);

        /// <summary>
        /// Modify list of Company Payrolls
        /// </summary>
        /// <param name="companyPayroll">List of Company Payrolls which needs to be updated.</param>
        Response ModifyCompanyPayroll(string companyCode, IList<Models.Companies.CompanyPayroll> companyPayroll, bool commitChange = true);

        /// <summary>
        /// Delete list of Company Payrolls
        /// </summary>
        /// <param name="companyPayroll">List of Company Payrolls which need to be deleted.</param>
        Response DeleteCompanyPayroll(string companyCode, IList<Models.Companies.CompanyPayroll> companyPayroll, bool commitChange = true);

        /// <summary>
        /// Return all the match search Company Payrolls
        /// </summary>
        /// <param name="searchModel">search model</param>
        /// <returns>List of Matched Search Result</returns>
        Response GetCompanyPayroll(Models.Companies.CompanyPayroll searchModel);

        bool IsValidCompanyPayroll(IList<KeyValuePair<string, string>> compPayrollNames,
                                   ref IList<DbModel.CompanyPayroll> dbCompanyPayroll,
                                   ref IList<ValidationMessage> valdMessages,
                                   params Expression<Func<DbModel.CompanyPayroll, object>>[] includes);
    }
}
