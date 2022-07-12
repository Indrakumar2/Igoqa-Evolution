using DomModel=Evolution.Admin.Domain.Models.Admins;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Admin.Domain.Interfaces.Admins
{
    public interface IUserService
    {
        Response GeUser(string userName);
        Response GeUser(DomModel.User searchModel);

        Response GetMICoordinators(IList<string> ContractHoldingCompanyCodes, IList<int> ContractHoldingCompanyIds = null);

        Response GetByUserType(string companyCode, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false);

        Response GetByUserType(IList<string> companyCode, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false);

        Response GetReportCoordniators(string loggedInCompany, bool isVisit, bool isOperating);
    }
}
