using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Company.Domain.Interfaces.Companies
{
    public interface ICompanyService
    {        
        Response SaveCompany(IList<Models.Companies.Company> companies, bool commitChange = true);

        Response SaveCompany(IList<Models.Companies.Company> companies,ref IList<DbModel.Company> dbCompanies,bool commitChange = true);
        
        Response ModifyCompany(IList<Models.Companies.Company> companies, bool commitChange = true);

        Response ModifyCompany(IList<Models.Companies.Company> companies,ref IList<Models.Companies.CompanyDetail> exsistingCompany,ref IList<DbModel.Company> dbCompany, bool commitChange = true);

        Task<Response> GetCompanyAsync(Models.Companies.CompanySearch searchModel);

        bool IsValidCompany(IList<string> companyCodes, ref IList<DbModel.Company> dbCompanies, ref IList<ValidationMessage> messages, params Expression<Func<DbModel.Company, object>>[] includes);

        bool IsValidCompany(IList<string> companyCodes,
                                  ref IList<DbModel.Company> dbCompanies,
                                  ref IList<ValidationMessage> messages,
                                  string[] includes);

        bool IsValidCompanyById(IList<int?> companyCodes, ref IList<DbModel.Company> dbCompanies, ref IList<ValidationMessage> messages, params Expression<Func<DbModel.Company, object>>[] includes);

        Response GetCompanyList(Models.Companies.CompanySearch searchModel);
    }
}
