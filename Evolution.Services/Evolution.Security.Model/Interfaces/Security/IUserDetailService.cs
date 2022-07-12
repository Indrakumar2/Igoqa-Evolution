using Evolution.Common.Models.Responses;
using System.Collections.Generic;

namespace Evolution.Security.Domain.Interfaces.Security
{
    public interface IUserDetailService
    {
        Response Get(Models.Security.UserInfo searchModel);

        Response Get(Models.Security.UserInfo searchModel, string[] excludeUserTypes);

        Response Add(IList<Models.Security.UserDetail> userDetails);

        Response Modify(IList<Models.Security.UserDetail> userDetails);

        Response Delete(IList<Models.Security.UserInfo> userInfos);

        Response ChangeUserStatus(string applicationName, string userSamaAccount, bool newStatus);

        Response GetUserMenu(string applicationName, string userLogonName, string companyCode);

        Response GetUserPermission(string applicationName, string userLogonName, int companyId, string moduleName);

        Response GetUserPermission(string applicationName, string userLogonName, string companyCode);

        Response GetUserRoleCompany(IList<KeyValuePair<string, string>> appUserLogonNames);

        Response GetUserType(string userLogonName, string companyCode);

        bool GetUserPermission(int companyId, string userLogonName, string moduleName, List<string> activities);
    }
}
