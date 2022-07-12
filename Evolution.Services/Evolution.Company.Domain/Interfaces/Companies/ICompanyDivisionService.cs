using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Company.Domain.Interfaces.Companies
{
    public interface ICompanyDivisionService
    { 
        Response SaveCompanyDivision(string companyCode,IList<Models.Companies.CompanyDivision> companyDivisions, bool commitChange = true);
        
        Response ModifyCompanyDivision(string companyCode, IList<Models.Companies.CompanyDivision> companyDivisions, bool commitChange = true);
        
        Response DeleteCompanyDivision(string companyCode, IList<Models.Companies.CompanyDivision> companyDivisions, bool commitChange = true);
        
        Response GetCompanyDivision(Models.Companies.CompanyDivision searchModel);
    }
}
