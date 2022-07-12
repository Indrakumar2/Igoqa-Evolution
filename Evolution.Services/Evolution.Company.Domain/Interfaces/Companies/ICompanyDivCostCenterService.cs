using Evolution.Common.Models.Responses;
using System.Collections.Generic;

namespace Evolution.Company.Domain.Interfaces.Companies
{
    /// <summary>
    /// This will provide all the functionality related to Company cost centers.
    /// </summary>
    public interface ICompanyDivisionCostCenterService
    {        
        Response SaveCompanyCostCenter(string companyCode, string divisionName, IList<Models.Companies.CompanyDivisionCostCenter> companyCostCenters, bool commitChange = true);
        
        Response ModifyCompanyCostCenter(string companyCode, string divisionName, IList<Models.Companies.CompanyDivisionCostCenter> companyCostCenters, bool commitChange = true);
        
        Response DeleteCompanyCostCenter(string companyCode, string divisionName, IList<Models.Companies.CompanyDivisionCostCenter> companyCostCenters, bool commitChange = true);
        
        Response GetCompanyCostCenter(Models.Companies.CompanyDivisionCostCenter searchModel);
    }
}
