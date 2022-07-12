using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using Evolution.Security.Model.Models.Security;
using DomainModel = Evolution.Security.Domain.Models;

namespace Evolution.Security.Domain.Interfaces.Data
{
    public interface IUserRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.User>
    {
        IList<DomainModel.Security.UserInfo> Search(DomainModel.Security.UserInfo model, string[] excludeUserTypes=null, bool isGetAllUser = false);

        IList<DomainModel.Security.UserInfo> GetUser(string companyCode, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false);

        IList<User> Get(IList<int> ids);

        IList<User> Get(IList<string> names);

        IList<User> Get(string companyCode, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false);

        IList<User> Get(IList<int> companyIds, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false);

        IList<User> Get(string companyCode, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false, bool isUserTypeRequired = false);

        IList<User> Get(IList<string> loginNames, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false, bool isUserTypeRequired = false);

        IList<DomainModel.Security.UserTypeInfo> Get(string companyCode, IList<string> userTypes); /** To Resolve the Profiler Query Execution we Changed DBModel to Domain Model on 12-02-2021*/ //Added for ITK Defect 908(Ref ALM Document 14-05-2020)

        //Added for Email Notification
        IList<UserType> GetUserByType(string companyCode, IList<string> userTypes);

        //Added for Email expiry Notification
        IList<UserType> GetUserByType(IList<int> companyIds, IList<string> userTypes);

        IList<User> GetUserByCompanyAndName(string companyCode, string userName);

        IList<User> Get(string companyCode, string samAccountName, bool isFilterCompanyActiveCoordinators = false);

        List<ViewAllRights> GetViewAllAssignments(string samAccountName);

        IList<DomainModel.Security.UserCompanyRole> GetUserCompanyRoles(string userName, string MenuName);
    }
}