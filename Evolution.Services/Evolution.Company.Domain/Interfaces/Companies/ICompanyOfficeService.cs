using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Company.Domain.Interfaces.Companies
{
    public interface ICompanyOfficeService
    {
        Response GetCompanyAddress(Models.Companies.CompanyAddress searchModel);

        Response ModifyCompanyAddress(string companyCode, IList<Models.Companies.CompanyAddress> companyAdress, bool commitChange = true, bool returnResultSet = true);

        Response SaveCompanyAddress(string companyCode, IList<Models.Companies.CompanyAddress> companyAdress, bool commitChange = true, bool returnResultSet = true);

        Response DeleteCompanyAddress(string companyCode, IList<Models.Companies.CompanyAddress> companyAdress, bool commitChange = true, bool returnResultSet = true);

        bool IsValidCompanyAddress(IList<KeyValuePair<string, string>> companyCodeAndOfficeNames,
                                            ref IList<DbModel.CompanyOffice> dbCompanyOffices,
                                            ref IList<ValidationMessage> messages,
                                            params Expression<Func<DbModel.CompanyOffice, object>>[] includes);
    }
}
