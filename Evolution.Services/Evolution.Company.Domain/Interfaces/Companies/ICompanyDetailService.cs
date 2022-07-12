using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Company.Domain.Interfaces.Companies
{
    public interface ICompanyDetailService
    {
        Response SaveCompanyDetail(IList<Models.Companies.CompanyDetail> companyDetail);
    }
}
