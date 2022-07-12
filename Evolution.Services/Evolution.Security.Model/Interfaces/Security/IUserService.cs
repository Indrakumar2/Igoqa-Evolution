using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using Evolution.Security.Domain.Models.Security;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
namespace Evolution.Security.Domain.Interfaces.Security
{
    public interface IUserService
    {
        Response Get(Models.Security.UserInfo searchModel, string[] excludeUserTypes=null, bool isGetAllUser = false);

        /// <summary>
        /// Filter the user and get dbUsers.
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="dbUsers"></param>
        /// <param name="includes">Pass the child table Navigation Property</param>
        Response Get(Models.Security.UserInfo searchModel, ref IList<DbModel.User> dbUsers, params string[] includes);

        Response GetUser(string companyCode, string userType, bool isActive = true);

        Response Get(string applicationName, IList<string> userNames);

        Response GetByUserType(string companyCode, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false);

        Response GetUserType(string companyCode, IList<string> userTypes); //Added for ITK Defect 908(Ref ALM Document 14-05-2020)

        // Added for Email Notification
        IList<DbModel.UserType> GetUsersByTypeAndCompany(string companyCode, IList<string> userTypes);

        IList<DbModel.User> GetUsersByCompanyAndName(string companyCode, string userName);

        Response GetByUserType(string companyCode, IList<string> userTypes, bool isUserTypeRequired, bool isFilterCompanyActiveCoordinators = false);

        Response GetByUserType(IList<string> loginNames, IList<string> userTypes, bool isFilterCompanyActiveCoordinators=false);

        Response GetUserCompanyRoles(string username, string menuName);

        Response Add(IList<UserInfo> users, ref long? eventId, bool commitChange = true, bool isDbValidationRequire = true);

        Response Get(IList<string> userNames);

        Response Get(IList<string> userNames, bool IsUserTypeRequired);

        Response GetViewAllAssignments(string samAccountName);

        Response Add(IList<UserInfo> users,
                        ref IList<DbModel.User> dbUsers,
                        ref IList<DbModel.Application> dbApplications,
                        ref IList<DbModel.Company> dbCompany,
                        ref long? eventId,
                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response Modify(IList<UserInfo> users,
                        ref IList<DbModel.User> dbUsers,
                        ref IList<DbModel.Application> dbApplications,
                        ref IList<DbModel.Company> dbCompany,
                        ref long? eventId,
                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response Modify(IList<UserInfo> users, ref long? eventId, bool commitChange = true, bool isDbValidationRequire = true);

        Response Delete(IList<UserInfo> userInfo, bool commitChange = true, bool isDbValidationRequire = true);

        Response IsRecordValidForProcess(IList<UserInfo> users, ValidationType validationType, params string[] childTableToBeExcludes);

        Response IsRecordValidForProcess(IList<UserInfo> users,
                                            ValidationType validationType,
                                            ref IList<DbModel.User> dbUsers,
                                            ref IList<DbModel.Application> dbApplications,
                                            ref IList<DbModel.Company> dbCompany,
                                             params string[] childTableToBeExcludes);

        Response IsRecordExistInDb(IList<KeyValuePair<string, string>> appUserNames,
                                   ref IList<DbModel.User> dbUser,
                                   ref IList<string> userNotExists);
    }
}
