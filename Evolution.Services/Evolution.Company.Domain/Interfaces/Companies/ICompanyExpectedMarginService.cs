using Evolution.Common.Models.Responses;
using System.Collections.Generic;

namespace Evolution.Company.Domain.Interfaces.Companies
{
    /// <summary>
    /// This will provide all the functionality related to Company Expected Margin.
    /// </summary>
    public interface ICompanyExpectedMarginService
    {

        Response SaveCompanyExpectedMargin(string companyCode, IList<Models.Companies.CompanyExpectedMargin> companyExpectedMargins, bool commitChange = true);

        Response ModifyCompanyExpectedMargin(string companyCode, IList<Models.Companies.CompanyExpectedMargin> companyExpectedMargins, bool commitChange = true);

        Response DeleteCompanyExpectedMargin(string companyCode, IList<Models.Companies.CompanyExpectedMargin> companyExpectedMargins, bool commitChange = true);

        Response GetCompanyExpectedMargin(Models.Companies.CompanyExpectedMargin searchModel);
    }
}
