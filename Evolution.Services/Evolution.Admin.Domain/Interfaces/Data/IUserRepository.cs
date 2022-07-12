using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Admin.Domain.Models.Admins;

namespace Evolution.Admin.Domain.Interfaces.Data
{
    public interface IUserRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.User>
    {
        IList<DomainModel.User> Search(DomainModel.User searchModel);

        IList<DomainModel.User> GetUsers(IList<string> companyCodes);

        IList<DomainModel.User> GetUsers(IList<int> companyIds);

        void GetUsers(IList<string> companyCodes, ref IList<DbModels.User> dbUser);

        IList<DomainModel.User> Get(string companyCode, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false);

        IList<DomainModel.User> Get(IList<string> companyCode, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false);

        IList<DomainModel.User> GetUsers(string loggedInUser, bool isVisit, bool isOperating);
    }
}
