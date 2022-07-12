using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Security.Domain.Models.Security;

namespace Evolution.Security.Domain.Interfaces.Security
{
    public interface IUserRoleService
    {
        Response Get(DomainModel.UserRoleInfo searchModel);

        Response Get(IList<int> userIds);

        Response Get(DomainModel.UserRoleInfo searchModel, params string[] includes);

        Response IsUserRoleExistInDb(IList<DomainModel.UserRoleInfo> userRoleInfos,
                                        ref IList<DomainModel.UserRoleInfo> userRoleNotExists,
                                        ref IList<DbModel.UserRole> dbUserRoles);

        Response Add(IList<DomainModel.UserRoleInfo> userRoleInfos,
                        ref long? eventId, // Manage Security Audit changes
                        bool commitChange = true, 
                        bool isDbValidationRequire = true);

        Response Add(IList<DomainModel.UserRoleInfo> userRoleInfos,
                        ref IList<DbModel.UserRole> dbUserRoles,
                        ref IList<DbModel.Application> dbApplications,
                        ref IList<DbModel.User> dbUsers,
                        ref IList<DbModel.Role> dbRoles,
                        ref IList<DbModel.Company> dbCompany,
                        ref long? eventId, // Manage Security Audit changes
                        bool commitChange = true,
                        bool isDbValidationRequire = true);


       // Response Modify(IList<DbModel.UserRole> dbUserRoles, string modifiedBy);

        Response Delete(IList<DomainModel.UserRoleInfo> userRoleInfo,
                        ref long? eventId, // Manage Security Audit changes
                        bool commitChange = true, 
                        bool isDbValidationRequire = true);
        
        Response Delete(IList<DomainModel.UserRoleInfo> userRoleInfo,
                        ref IList<DbModel.UserRole> dbUserRoles,
                        ref IList<DbModel.Application> dbApplications,
                        ref IList<DbModel.User> dbUsers,
                        ref IList<DbModel.Role> dbRoles,
                        ref IList<DbModel.Company> dbCompany,
                        ref long? eventId, // Manage Security Audit changes
                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response IsRecordValidForProcess(IList<DomainModel.UserRoleInfo> userRoleInfos, ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.UserRoleInfo> userRoleInfos,
                                            ValidationType validationType,
                                            ref IList<DbModel.UserRole> dbUserRoles,
                                            ref IList<DbModel.Application> dbApplications,
                                            ref IList<DbModel.User> dbUsers,
                                            ref IList<DbModel.Role> dbRoles,
                                            ref IList<DbModel.Company> dbCompany);
    }
}
