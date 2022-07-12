using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Company.Domain.Interfaces.Companies
{
    /// <summary>
    /// This will provide all the functionality related to Company Payroll.
    /// </summary>
    public interface ICompanyTaxService
    {
        /// <summary>
        /// Save Company Tax
        /// </summary>
        /// <param name="companyTax">List of Company Tax</param>
        /// <returns>All the Saved Company Tax Details</returns>
        Response SaveCompanyTax(string companyCode, IList<Models.Companies.CompanyTax> companyTax, bool commitChange = true);

        /// <summary>
        /// Modify list of Company Tax
        /// </summary>
        /// <param name="companyTax">List of Company Tax which needs to be updated.</param>
        Response ModifyCompanyTax(string companyCode, IList<Models.Companies.CompanyTax> companyTax, bool commitChange = true);

        /// <summary>
        /// Delete list of Company Tax
        /// </summary>
        /// <param name="companyTax">List of Company Tax which needs to be deleted.</param>
        Response DeleteCompanyTax(string companyCode, IList<Models.Companies.CompanyTax> companyTax, bool commitChange = true);

        /// <summary>
        /// Return all the match search Company Tax
        /// </summary>
        /// <param name="searchModel">search model</param>
        /// <returns>List of Matched Search Result</returns>
        Response GetCompanyTax(Models.Companies.CompanyTax searchModel);
    }
}
