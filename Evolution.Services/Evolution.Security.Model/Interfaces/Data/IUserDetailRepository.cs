using Evolution.GenericDbRepository.Interfaces;
using Evolution.Security.Domain.Models.Security;
using System.Collections.Generic;

namespace Evolution.Security.Domain.Interfaces.Data
{
    public interface IUserDetailRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.UserRole>
    {
        IList<UserDetail> Search(UserInfo model);

        IList<UserDetail> Search(UserInfo model, string[] excludeUserTypes);

        int DeleteUser(IList<string> userLogonNames);

        IList<UserCompanyInfo> GetUserRoleCompany(IList<KeyValuePair<string, string>> userLogonNames);

        IList<UserPermissionInfo> UserPermission(int companyId, string userLogonName, string moduleName);

        List<UserMenuPermissionInfo> GetMenuList(string applicationName, string userLogonName, string companyCode);

        int UserPermission(int companyId, string userLogonName, string moduleName, List<string> activities);
    }
}