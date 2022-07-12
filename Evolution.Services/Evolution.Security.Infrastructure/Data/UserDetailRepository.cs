using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Security.Domain.Interfaces.Data;
using Evolution.Security.Domain.Models.Security;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Security.Infrastructure.Data
{
    public class UserDetailRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.UserRole>, IUserDetailRepository
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public UserDetailRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<UserDetail> Search(UserInfo model)
        {
            return SearchDetail(model);
        }

        public IList<UserDetail> Search(UserInfo model, string[] excludeUserTypes)
        {
            return SearchDetail(model, null, excludeUserTypes);
        }

        public int DeleteUser(IList<string> logonNames)
        {
            var names = logonNames.Select(x => string.Format("'{0}'", x)).ToList();

            var deleteStatement = string.Format(Utility.GetSqlQuery(SQLModuleType.Security, SQLModuleActionType.Delete),
                                                string.Join(',', names), string.Join(',', names));
            return _dbContext.Database.ExecuteSqlCommand(deleteStatement);
        }

        public List<UserMenuPermissionInfo> GetMenuList(string applicationName, string userLogonName, string companyCode)
        {
           return _dbContext.RoleActivity.Join(_dbContext.UserRole,
                                dbRoleActivity => new { dbRoleActivity.RoleId },
                                dbUserRole => new { dbUserRole.RoleId },
                                (dbRoleActivity, dbUserRole) => new { dbRoleActivity, dbUserRole })
                                .Join(_dbContext.User,
                                dbUserRoleAct => new { dbUserRoleAct.dbUserRole.UserId },
                                dbUser => new { UserId = dbUser.Id },
                       (dbUserRoleAct, dbUser) => new { dbUserRoleAct, dbUser })
                                .Join(_dbContext.ApplicationMenu,
                                dbUserRoleActMen => new { dbUserRoleActMen.dbUserRoleAct.dbRoleActivity.ModuleId },
                                dbApplicationMenu => new { dbApplicationMenu.ModuleId },
                       (dbUserRoleActMen, dbApplicationMenu) => new { dbUserRoleActMen, dbApplicationMenu })
                      .Where(x1 => x1.dbUserRoleActMen.dbUser.SamaccountName == userLogonName && x1.dbUserRoleActMen.dbUserRoleAct.dbUserRole.Company.Code == companyCode
                                   && x1.dbUserRoleActMen.dbUser.Application.Name == applicationName
                                   && x1.dbUserRoleActMen.dbUser.IsActive == true)
                      ?.Select(x => new UserMenuPermissionInfo()
                      {
                          Module = x.dbUserRoleActMen.dbUserRoleAct.dbRoleActivity.Module.Name,
                          MenuName = x.dbApplicationMenu.MenuName,
                          IsVisible = true
                      })?.Distinct()?.OrderBy(x=>x.MenuName).ToList();
        }

        public IList<UserCompanyInfo> GetUserRoleCompany(IList<KeyValuePair<string, string>> appUserLogonNames)
        {
            var userLogonNames = appUserLogonNames.Select(x => x.Value)
                                                     .Where(x1 => !string.IsNullOrEmpty(x1))
                                                     .ToList();
            var applications = appUserLogonNames.Select(x => x.Key)
                                                  .Where(x1 => !string.IsNullOrEmpty(x1))
                                                  .ToList();
            return _dbContext.User.Join(_dbContext.UserType,
                                dbUser => new { UserId = dbUser.Id },
                                dbUserType => new { dbUserType.UserId },
                       (dbUser, dbUserType) => new { dbUser, dbUserType })
                      .Where(x1 => applications.Contains(x1.dbUser.Application.Name) &&
                                   userLogonNames.Contains(x1.dbUser.SamaccountName) &&
                                   x1.dbUserType.IsActive == true)
                      ?.Select(x1 => new UserCompanyInfo()
                      {
                          CompanyId = x1.dbUserType.CompanyId,
                          CompanyCode = x1.dbUserType.Company.Code,
                          CompanyName = x1.dbUserType.Company.Name,
                          UserLogonName = x1.dbUser.SamaccountName
                      })
                      ?.Distinct().OrderBy(x2=>x2.CompanyName)
                      ?.ToList();



        }
        //scrapped on 08Feb2020-PT
        public IList<UserCompanyInfo> GetUserRoleCompany1(IList<KeyValuePair<string, string>> appUserLogonNames)
        {
            var userCompanyInfos = new List<UserCompanyInfo>();
            if (appUserLogonNames?.Count > 0)
            {
                var userLogonNames = appUserLogonNames.Select(x => x.Value)
                                                      .Where(x1 => !string.IsNullOrEmpty(x1))
                                                      .ToList();
                var applications = appUserLogonNames.Select(x => x.Key)
                                                      .Where(x1 => !string.IsNullOrEmpty(x1))
                                                      .ToList();

                var dbUsers = _dbContext.User.Include(x => x.Application)
                                             .Include(x => x.Company)
                                             .Include(x => x.CompanyOffice)
                                             .Include(x => x.UserRole).ThenInclude(x => x.Application)
                                             .Include(x => x.UserRole).ThenInclude(x1 => x1.Company)
                                             .Where(x1 => applications.Contains(x1.Application.Name) &&
                                                        userLogonNames.Contains(x1.SamaccountName))
                                             .ToList();
                if (dbUsers?.Count > 0)
                {
                    var userDefCompany = dbUsers.Select(x2 => new UserCompanyInfo()
                    {
                        CompanyId = x2.CompanyId,
                        CompanyCode = x2.Company.Code,
                        CompanyName = x2.Company.Name,
                        UserLogonName = x2.SamaccountName
                    }).ToList();
      
                                                

                    // commented by abdul : Fetch role company based on usertype InActive status

                    //var userRoleCompany = dbUsers.SelectMany(x => x.UserRole.Select(x2 => new UserCompanyInfo()
                    //{
                    //    CompanyId = x2.CompanyId,
                    //    CompanyCode = x2.Company.Code,
                    //    CompanyName = x2.Company.Name,
                    //    UserLogonName = x.SamaccountName
                    //})).ToList();

                    var userTypeCompany = dbUsers.SelectMany(x => x.UserType.Select(x2 => new UserCompanyInfo()
                    {
                        CompanyId = x2.CompanyId,
                        CompanyCode = x2.Company.Code,
                        CompanyName = x2.Company.Name,
                        UserLogonName = x.SamaccountName
                    })).ToList();
                
                    var userCompany = userDefCompany.Union(userTypeCompany).ToList();

                    var userDefCompCodes = dbUsers.Where(x1 => x1.Company.IsActive == true).Select(x => x.Company.Code).Distinct().ToList();

                    // commented by abdul : Fetch role company based on usertype InActive status
                    //var userRoleCompCodes = dbUsers.SelectMany(x => x.UserRole.Select(x1 => x1.Company.Code)).Distinct().ToList();

                    var userTypeCompCodes = dbUsers.SelectMany(x => x.UserType.Where(x1 => x1.IsActive == true && x1.Company.IsActive == true).Select(x1 => x1.Company.Code)).Distinct().ToList();

                    var compCodes = userDefCompCodes.Union(userTypeCompCodes).Distinct().ToList();
                    foreach (var compCode in compCodes)
                    {
                        if (!userCompanyInfos.Any(x => x.CompanyCode == compCode))
                        {
                            var itemTobeAdd = userCompany.FirstOrDefault(x => x.CompanyCode == compCode);
                            if (itemTobeAdd != null)
                                userCompanyInfos.Add(itemTobeAdd);
                        }
                    }
                }

                //userCompanyInfos = _dbContext.UserRole
                //                             .Include(x => x.Company)
                //                             .Include(x => x.Application)
                //                             .Where(x1 => applications.Contains(x1.Application.Name) &&
                //                                          userLogonNames.Contains(x1.User.SamaccountName))
                //                             .Select(x2 => new UserCompanyInfo()
                //                             {
                //                                 CompanyId = x2.CompanyId,
                //                                 CompanyCode = x2.Company.Code,
                //                                 CompanyName = x2.Company.Name,
                //                                 UserLogonName = x2.User.SamaccountName
                //                             }).ToList();

            }
            return userCompanyInfos.OrderBy(x => x.CompanyName).ToList();
        }

        private IList<UserDetail> SearchDetail(UserInfo model, string companyCode = null, string[] excludeUserTypes = null)
        {
            var dbSearchUser = _mapper.Map<User>(model);
            var whereClause = dbSearchUser.ToExpression(new List<string>()
            {
                nameof(dbSearchUser.EmailConfirmed),
                nameof(dbSearchUser.PhoneNumberConfirmed),
                nameof (dbSearchUser.LockoutEnabled),
                nameof(dbSearchUser.IsPasswordNeedToBeChange),
                nameof(dbSearchUser.IsActive), //Temprorary Coomenting it, need proper solution to handle isActive
                nameof(dbSearchUser.CreatedDate),
                nameof(dbSearchUser.IsShowNewVisit)
            });

            IQueryable<User> users = _dbContext.User.Include(x => x.Application)
                                                    .Include(x => x.Company)
                                                    .Include(x => x.CompanyOffice) 
                                                    .Include(x => x.UserRole).ThenInclude(x => x.Role).ThenInclude(x => x.Application)
                                                    .Include(x => x.UserRole).ThenInclude(x1 => x1.Company)
                                                    .Include(x => x.UserType).ThenInclude(x1 => x1.Company);
            if (whereClause != null)
                users = users.Where(whereClause);

            if (!string.IsNullOrEmpty(companyCode))
                users = users.Where(x => x.Company.Code == companyCode);

            if (excludeUserTypes != null)
                users = users.Where(x => x.UserType.Any(x1=> !excludeUserTypes.Contains(x1.UserTypeName)));

            var userData = users.ToList();
            //var dbApplication = userData.Select(x => x.Application).ToList();
            //var dbCompany = userData.Select(x => x.Company).ToList();
            //var dbCompanyOffice = userData.Where(x => x.CompanyOffice != null)
            //                             .Select(x => x.CompanyOffice).ToList();

            return userData.Select(x => new UserDetail()
            {
                User = ConvertFromDbUser(x),
                CompanyRoles = ConvertFromUserRole(x.UserRole.ToList()),
                CompanyUserTypes= ConvertFromUserType(x.UserType.ToList()) 
            }).ToList();
        }

        private RoleInfo ConvertFromDbRole(Role role)
        {
            IList<Application> dbApplication = new List<Application>() { role.Application };
            return _mapper.Map<RoleInfo>(role, opt =>
            {
                opt.Items["dbApplication"] = dbApplication;
            });
        }

        private UserInfo ConvertFromDbUser(User user)
        {
            IList<Application> dbApplication = new List<Application>() { user.Application };
            IList<Company> dbCompany = new List<Company>() { user.Company };
            IList<CompanyOffice> dbCompanyOffice = new List<CompanyOffice>() { user.CompanyOffice };
            return _mapper.Map<UserInfo>(user, opt =>
            {
                opt.Items["dbApplication"] = dbApplication;
                opt.Items["dbCompany"] = dbCompany;
                opt.Items["dbCompanyOffice"] = dbCompanyOffice;
            });
        }

        private IList<CompanyRole> ConvertFromUserRole(IList<UserRole> dbUserRoles)
        {
            if (dbUserRoles?.Count > 0)
            {
                var groupedRecs = dbUserRoles.GroupBy(x => new { x.CompanyId, x.Company.Code, x.ApplicationId });

                return groupedRecs.Select(x => new CompanyRole()
                {
                    CompanyCode = x.Key.Code,
                    CompanyId = x.Key.CompanyId,
                    Roles = x.Select(x1 => ConvertFromDbRole(x1.Role)).ToList()
                }).ToList();
            }
            else
            {
                return null;
            }
        }

        private IList<CompanyUserType> ConvertFromUserType(IList<DbRepository.Models.SqlDatabaseContext.UserType> dbUserTypes)
        {
            if (dbUserTypes?.Count > 0)
            {
                var groupedRecs = dbUserTypes.GroupBy(x => new { x.CompanyId, x.Company.Code});

                return groupedRecs.Select(x => new CompanyUserType()
                {
                    CompanyCode = x.Key.Code,
                    CompanyId = x.Key.CompanyId,
                    UserTypes = x.Select(x1 => ConvertFromDbUserType(x1)).ToList(),
                   IsActive = x.Select(x1=>ConvertFromDbUserType(x1))?.ToList()?.Where(x2=>x2.IsActive == true)?.Count()>0? true: false
                }).ToList();
            }
            else
            {
                return null;
            }
        }

        private UserTypeInfo ConvertFromDbUserType(DbRepository.Models.SqlDatabaseContext.UserType userType)
        { 
            IList<Company> dbCompany = new List<Company>() { userType.Company };
            IList<User> dbUser = new List<User>() { userType.User };
            return _mapper.Map<UserTypeInfo>(userType, opt =>
            { 
                opt.Items["dbCompany"] = dbCompany;
                opt.Items["dbUser"] = dbUser;
            });
        }
        /*This method is used in Role Company call from Dashboard*/
        public IList<UserPermissionInfo> UserPermission(int companyId, string userLogonName, string moduleName)
        {
            if (moduleName == "Assignment")
            {
                return UserRoleList(companyId, userLogonName, moduleName, "TechSpecialist", "S00010");
            }
            else
            {
                return UserRoleList(companyId, userLogonName, moduleName, "", "");
            }
        }

        /*This method is used in Authorise Filter to validate if the user has correct permissions*/
        public int UserPermission(int companyId, string userLogonName, string moduleName,List<string> activities)
        {
            return _dbContext.UserRole.Join(_dbContext.RoleActivity,
                                          dbUserRole => new { dbUserRole.RoleId },
                                          dbRoleActivity => new { dbRoleActivity.RoleId },
                                          (dbUserRole, dbRoleActivity) => new { dbUserRole, dbRoleActivity })
                                  .Where(x => x.dbUserRole.CompanyId == companyId
                                          && x.dbUserRole.User.SamaccountName == userLogonName
                                          && activities.Contains(x.dbRoleActivity.Activity.Code)
                                          && x.dbRoleActivity.Module.Name == moduleName)
                                   ?.Select(x => x.dbRoleActivity.ActivityId)
                                   ?.Distinct()
                                   ?.Count() ?? 0;
        }

        private IList<UserPermissionInfo> UserRoleList(int companyId, string userLogonName, string moduleName, string techModule, string techRoleCode)
        {
            return _dbContext.UserRole.Join(_dbContext.UserType,
                                         dbUserRole => new { dbUserRole.UserId },
                                         dbUserType => new { dbUserType.UserId },
                                        (dbUserRole, dbUserType) => new { dbUserRole, dbUserType })
                                        .Join(_dbContext.RoleActivity,
                                         dbUserRole => new { dbUserRole.dbUserRole.RoleId },
                                         dbRoleActivity => new { dbRoleActivity.RoleId },
                                        (dbUserRole, dbRoleActivity) => new { dbUserRole, dbRoleActivity })
                                         .Join(_dbContext.Module,
                                         dbRoleActivity => new { dbRoleActivity.dbRoleActivity.ModuleId },
                                         dbModule => new { ModuleId = dbModule.Id },
                                        (dbRoleActivity, dbModule) => new { dbRoleActivity, dbModule })
                                         .Join(_dbContext.Activity,
                                         dbRoleActivity => new { dbRoleActivity.dbRoleActivity.dbRoleActivity.ActivityId },
                                         dbActivity => new { ActivityId = dbActivity.Id },
                                        (dbRoleActivity, dbActivity) => new { dbRoleActivity, dbActivity })
                                        .Join(_dbContext.User,
                                         dbRoleActivity => new { dbRoleActivity.dbRoleActivity.dbRoleActivity.dbUserRole.dbUserRole.UserId },
                                         dbUser => new { UserId = dbUser.Id },
                                        (dbRoleActivity, dbUser) => new { dbRoleActivity, dbUser })

                                         ?.Where(x => x.dbUser.ApplicationId == 1 &&
                                                      x.dbRoleActivity.dbRoleActivity.dbRoleActivity.dbUserRole.dbUserType.CompanyId == companyId
                                                      && x.dbRoleActivity.dbRoleActivity.dbRoleActivity.dbUserRole.dbUserType.IsActive == true
                                                      && x.dbRoleActivity.dbRoleActivity.dbRoleActivity.dbUserRole.dbUserRole.CompanyId == companyId
                                                      && x.dbUser.SamaccountName == userLogonName
                                                      && (x.dbRoleActivity.dbRoleActivity.dbModule.Name == moduleName
                                                      || (x.dbRoleActivity.dbRoleActivity.dbModule.Name == techModule
                                                      && x.dbRoleActivity.dbActivity.Code == techRoleCode)))
                                         ?.Select(x => new UserPermissionInfo()
                                         {
                                             Module = x.dbRoleActivity.dbRoleActivity.dbModule.Name,
                                             Activity = x.dbRoleActivity.dbActivity.Code,
                                             HasPermission = true
                                         }).Distinct()
                                         ?.ToList();
        }
    }
}