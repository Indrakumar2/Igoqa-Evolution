using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Security.Domain.Interfaces.Data;
using Evolution.Security.Model.Models.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Security.Domain.Models;
using Evolution.Common.Constants;

namespace Evolution.Security.Infrastructure.Data
{
    public class UserRepository : GenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.User>, IUserRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public UserRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        //public IList<User> Search(DomainModel.Security.UserInfo model, string[] excludeUserTypes, bool isGetAllUser = false)
        //{
        //    Expression<Func<User, bool>> whereClause = null;

        //    if (model != null)
        //    {
        //        var dbSearchModel = this._mapper.Map<DbModel.User>(model);
        //        IList<string> excludeProps = new List<string>()
        //        {
        //            nameof(dbSearchModel.LockoutEnabled),
        //            nameof(dbSearchModel.IsPasswordNeedToBeChange),
        //            nameof(dbSearchModel.IsActive),
        //            nameof(dbSearchModel.EmailConfirmed),
        //            nameof(dbSearchModel.PhoneNumberConfirmed),
        //            nameof(dbSearchModel.CreatedDate),
        //            nameof(dbSearchModel.IsShowNewVisit)
        //        };
        //        if (!string.IsNullOrEmpty(model.UserType) && model.UserType == TechnicalSpecialistConstants.User_Type_TM)//Added for D651 issue8
        //        {
        //            excludeProps.Add(nameof(dbSearchModel.CompanyId));
        //        }
        //        whereClause = dbSearchModel.ToExpression(excludeProps);
        //    }

        //    IQueryable<User> dbUsers = this._dbContext.User
        //                                              .Include(x => x.Application)
        //                                              .Include(x => x.Company)
        //                                              .Include(x => x.CompanyOffice)
        //                                              .Include(x => x.UserType)
        //                                              .Include(x => x.CustomerUserProjectAccess).ThenInclude(x=>x.Project);
        //    if (excludeUserTypes != null)
        //        dbUsers = dbUsers.Where(x => x.UserType.Any(x1 => !excludeUserTypes.Contains(x1.UserTypeName)));

        //    if (model.IsActive != false && !isGetAllUser)
        //        dbUsers = dbUsers.Where(x => x.IsActive == model.IsActive);

        //    if (!string.IsNullOrEmpty(model.UserType))
        //        dbUsers = dbUsers.Where(x => x.UserType.Any(x1 => x1.UserTypeName == model.UserType));
        //    if (!string.IsNullOrEmpty(model.CompanyCode))//Added for D651 issue8
        //        dbUsers = dbUsers.Where(x => x.UserType.Any(x1 => x1.Company.Code == model.CompanyCode));

        //    if (!string.IsNullOrEmpty(model.UserType) && !string.IsNullOrEmpty(model.CompanyCode)) //changes for D655 issue 13(f) 
        //        dbUsers = dbUsers.Where(x => x.UserType.Any(x1 => x1.UserTypeName == model.UserType && x1.Company.Code == model.CompanyCode));

        //    if (whereClause != null)
        //        return dbUsers.Where(whereClause).ToList();
        //    else
        //        return dbUsers.ToList();
        //}


        public IList<DomainModel.Security.UserInfo> Search(DomainModel.Security.UserInfo model, string[] excludeUserTypes, bool isGetAllUser = false)
        {
            Expression<Func<User, bool>> whereClause = null;
            IList<DomainModel.Security.UserInfo> userInfo = null;
            if (model != null)
            {
                var dbSearchModel = this._mapper.Map<DbModel.User>(model);
                IList<string> excludeProps = new List<string>()
                {
                    nameof(dbSearchModel.LockoutEnabled),
                    nameof(dbSearchModel.IsPasswordNeedToBeChange),
                    nameof(dbSearchModel.IsActive),
                    nameof(dbSearchModel.EmailConfirmed),
                    nameof(dbSearchModel.PhoneNumberConfirmed),
                    nameof(dbSearchModel.CreatedDate),
                    nameof(dbSearchModel.IsShowNewVisit)
                };
                if (!string.IsNullOrEmpty(model.UserType) && (model.UserType == TechnicalSpecialistConstants.User_Type_TM || model.UserType == TechnicalSpecialistConstants.User_Type_RC))//Added for D651 issue8 //RC Check Condition Added For 1279 (Page No.11 Issue) Ref ALM Doc:20-08-2020
                {
                    excludeProps.Add(nameof(dbSearchModel.CompanyId));
                }
                whereClause = dbSearchModel.ToExpression(excludeProps);
            }

            IQueryable<User> dbUsers = this._dbContext.User
                                                      .Include(x => x.Application)
                                                      .Include(x => x.Company)
                                                      .Include(x => x.CompanyOffice)
                                                      .Include(x => x.UserType)
                                                      .Include(x => x.CustomerUserProjectAccess).ThenInclude(x => x.Project);
            if (excludeUserTypes != null)
                dbUsers = dbUsers.Where(x => x.UserType.Any(x1 => !excludeUserTypes.Contains(x1.UserTypeName)));

            if (model.IsActive != false && !isGetAllUser)
                dbUsers = dbUsers.Where(x => x.IsActive == model.IsActive);

            if (!string.IsNullOrEmpty(model.UserType))
                dbUsers = dbUsers.Where(x => x.UserType.Any(x1 => x1.UserTypeName == model.UserType));
            if (!string.IsNullOrEmpty(model.CompanyCode))//Added for D651 issue8
                dbUsers = dbUsers.Where(x => x.UserType.Any(x1 => x1.Company.Code == model.CompanyCode && x1.IsActive == true));  //IGOQC def854 #2

            if (!string.IsNullOrEmpty(model.UserType) && !string.IsNullOrEmpty(model.CompanyCode)) //changes for D655 issue 13(f) 
                dbUsers = dbUsers.Where(x => x.UserType.Any(x1 => x1.UserTypeName == model.UserType && x1.Company.Code == model.CompanyCode && x1.IsActive == true)); //IGOQC def854 #2


            var dbUserListQuery = (whereClause != null) ? dbUsers.Where(whereClause) : dbUsers;
            var userList = dbUserListQuery.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                SamaccountName = x.SamaccountName,
                Email = x.Email,
                CompanyId = x.Company.Id,
                CompanyCode = x.Company.Code,
                CompanyName = x.Company.Name,
                CompanyOfficeName = x.CompanyOffice.OfficeName,
                IsActive = x.IsActive ?? false,
                UserTypes = x.UserType == null ? null : x.UserType.ToList()
            }).ToList();

            userInfo = userList.Select(x => new DomainModel.Security.UserInfo()
            {
                UserId = x.Id,
                UserName = x.Name,
                LogonName = x.SamaccountName,
                Email = x.Email,
                CompanyCode = x.CompanyCode,
                CompanyName = x.CompanyName,
                CompanyOfficeName = x.CompanyOfficeName,
                IsActive = x.IsActive,
                DefaultCompanyUserType = x.UserTypes?.Where(x1 => x1.CompanyId == x.CompanyId && x1.UserId == x.Id)?.Select(x1 => x1.UserTypeName)?.ToList()
            }).ToList();

            return userInfo.OrderBy(user => user.UserName).ToList();
        }

        public IList<User> Get(IList<int> ids)
        {
            if (ids?.Count > 0)
            {
                ids = ids.ToList();
                return this._dbContext.User.Where(x => ids.Contains((int)x.Id)).ToList();
            }
            else
                return null;
        }

        public IList<User> Get(IList<string> names)
        {
            if (names?.Count > 0)
            {
                names = names.ToList();
                return this._dbContext.User.Where(x => names.Contains(x.SamaccountName)).ToList();
            }
            else
                return null;
        }

        /* note- View all Assignment is hard coded assuming it will not change - As discussed with Sumit, Company should be only taken from UserRole 
         Modified on -08Aug2020- after getting an understanding that we need to check via UserType from IGO
             */
        public List<ViewAllRights> GetViewAllAssignments(string samAccountName)
        {
            var activityCodes = new List<string> { AssignmentConstants.ASSIGNMENT_VIEW_ALL, VisitTimesheetConstants.VISIT_VIEW_ALL, VisitTimesheetConstants.TIMESHEET_VIEW_ALL };
            var result = !string.IsNullOrEmpty(samAccountName) ? _dbContext.RoleActivity
                                   .Join(_dbContext.Module,
                                                         ra => new { ModuleId = ra.ModuleId },
                                                         mod => new { ModuleId = mod.Id },
                                                         (ra, mod) => new { ra, mod })
                                   .Join(_dbContext.Activity,
                                                         ra1 => new { ActivityId = ra1.ra.ActivityId },
                                                         ac => new { ActivityId = ac.Id },
                                                         (ra1, ac) => new { ra1, ac })
                                   .Join(_dbContext.UserRole,
                                                         ra2 => new { RoleId = ra2.ra1.ra.RoleId },
                                                         ur => new { RoleId = ur.RoleId },
                                                         (ra2, ur) => new { ra2, ur })
                                   .Join(_dbContext.User,
                                                         ra3 => new { UserId = ra3.ur.UserId },
                                                         u => new { UserId = u.Id },
                                                         (ra3, u) => new { ra3, u })
                                   .Join(_dbContext.UserType.Include("Company"),
                                                        ra4 => new { UserId = ra4.u.Id, userRoleCompanyId = (int)ra4.ra3.ur.CompanyId },
                                                        ut => new { UserId = ut.UserId, userRoleCompanyId = ut.CompanyId },
                                                        (ra4, ut) => new { ra4, ut })

                             .Where(x => x.ra4.u.IsActive == true && x.ut.IsActive == true && x.ut.Company.IsActive == true
                                && activityCodes.Contains(x.ra4.ra3.ra2.ac.Code) && x.ra4.u.SamaccountName == samAccountName)
                             .Select(x => new { x.ra4.ra3.ra2.ac.Code, x.ut.CompanyId })?.Distinct()?
                             .ToList() : null;
            List<ViewAllRights> viewAllRights = new List<ViewAllRights>();
            if (result?.Count > 0)
            {
                var assignmentRights = result.Where(x => x.Code == AssignmentConstants.ASSIGNMENT_VIEW_ALL).Select(x => x.CompanyId).Distinct().ToList();
                if (assignmentRights?.Count > 0)
                {
                    viewAllRights.Add(new ViewAllRights()
                    {
                        ModuleName = AssignmentConstants.ASSGINMENT,
                        ViewAllCompanies = assignmentRights
                    });
                }
                var visitRights = result.Where(x => x.Code == VisitTimesheetConstants.VISIT_VIEW_ALL).Select(x => x.CompanyId).Distinct().ToList();
                if (visitRights?.Count > 0)
                {
                    viewAllRights.Add(new ViewAllRights()
                    {
                        ModuleName = VisitTimesheetConstants.VISIT,
                        ViewAllCompanies = visitRights
                    });
                }
                var timesheetRights = result.Where(x => x.Code == VisitTimesheetConstants.TIMESHEET_VIEW_ALL).Select(x => x.CompanyId).Distinct().ToList();
                if (timesheetRights?.Count > 0)
                {
                    viewAllRights.Add(new ViewAllRights()
                    {
                        ModuleName = VisitTimesheetConstants.TIMESHEET_MODULE,
                        ViewAllCompanies = timesheetRights
                    });
                }
            }
            return viewAllRights;
        }

        public IList<User> Get(string companyCode, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false)
        {
            return _dbContext.User.Include("Application").Include("Company").Include("CompanyOffice").Join(_dbContext.UserType,
                                        usr => new { id = usr.Id },
                                        usrTy => new { id = usrTy.UserId },
                                        (usr, usrType) => new { usr, usrType })
                             .Where(x => x.usrType.Company.Code == companyCode
                                         && userTypes.Contains(x.usrType.UserTypeName)
                                         && (isFilterCompanyActiveCoordinators && x.usr.IsActive == true)
                ).Select(x => x.usr).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();

        }

        public IList<User> Get(IList<int> companyIds, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false)
        {
            return _dbContext.User.Include("Application").Join(_dbContext.UserType,
                                                   usr => new { id = usr.Id },
                                                   usrTy => new { id = usrTy.UserId },
                                                   (usr, usrType) => new { usr, usrType })
                                        .Where(x => companyIds.Contains(x.usrType.CompanyId)
                                                    && userTypes.Contains(x.usrType.UserTypeName)  
                                                    && (isFilterCompanyActiveCoordinators && x.usr.IsActive == true && x.usrType.IsActive == true)
                           ).Select(x => x.usr).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();
        }

        public IList<User> Get(string companyCode, string samAccountName, bool isFilterCompanyActiveCoordinators = false)
        {
            return _dbContext.User.Include("Application").Include("Company").Include("CompanyOffice").Join(_dbContext.UserType,
                                       usr => new { id = usr.Id },
                                       usrTy => new { id = usrTy.UserId },
                                       (usr, usrType) => new { usr, usrType })
                            .Where(x => x.usr.SamaccountName == samAccountName && x.usrType.Company.Code == companyCode
                                        && (isFilterCompanyActiveCoordinators && x.usr.IsActive == true)
               ).Select(x => x.usr).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();
        }

        public IList<User> Get(string companyCode, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false, bool isUserTypeRequired = false)
        {
            var user = _dbContext.User.Include("Application").Include("Company").Include("CompanyOffice");
            if (isUserTypeRequired)
            {
                user = user.Include("UserType");
            }
            return user.Join(_dbContext.UserType,
                                        usr => new { id = usr.Id },
                                        usrTy => new { id = usrTy.UserId },
                                        (usr, usrType) => new { usr, usrType })
                             .Where(x => x.usrType.Company.Code == companyCode
                                         && userTypes.Contains(x.usrType.UserTypeName)
                                         && (isFilterCompanyActiveCoordinators && x.usr.IsActive == true)
                ).Select(x => x.usr).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();

        }

        public IList<DomainModel.Security.UserInfo> GetUser(string companyCode, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false)
        {
            var user = _dbContext.User;
            var userInfo = user.Join(_dbContext.UserType,
                                        usr => new { id = usr.Id },
                                        usrTy => new { id = usrTy.UserId },
                                        (usr, usrType) => new { usr, usrType })
                             .Where(x => x.usrType.Company.Code == companyCode
                                         && userTypes.Contains(x.usrType.UserTypeName)
                                         && (isFilterCompanyActiveCoordinators && x.usr.IsActive == true)
                ).Select(x => new DomainModel.Security.UserInfo { LogonName = x.usr.SamaccountName , UserName = x.usr.Name }).Distinct().ToList();

            return userInfo?.OrderBy(x => x.UserName)?.ToList();

        }

        public IList<User> Get(IList<string> loginNames, IList<string> userTypes, bool isFilterCompanyActiveCoordinators = false, bool isUserTypeRequired = false)
        {
            var user = _dbContext.User.Include("Application").Include("Company").Include("CompanyOffice");
            if (isUserTypeRequired)
            {
                user = user.Include("UserType");
            }
            return user.Join(_dbContext.UserType,
                                        usr => new { id = usr.Id },
                                        usrTy => new { id = usrTy.UserId },
                                        (usr, usrType) => new { usr, usrType })
                             .Where(x => loginNames.Contains(x.usr.SamaccountName)
                                        && userTypes.Contains(x.usrType.UserTypeName)
                                        && (isFilterCompanyActiveCoordinators && x.usr.IsActive == true)
                ).Select(x => x.usr).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();

        }

        /**
         * Get Users based on userType and companyCode
         * Using UserType Table Itself
         * Added for ITK Defect 908(Ref ALM Document 14-05-2020) */
        public IList<DomainModel.Security.UserTypeInfo> Get(string companyCode, IList<string> userTypes)
        {
            /** To Resolve the Profiler Query Execution we Commented UserType Get Query and added a Join Query on 12-02-2021*/

            //return _dbContext.UserType.Where(x => x.User.IsActive == true && x.Company.Code == companyCode
            //                                   && userTypes.Contains(x.UserTypeName) && x.IsActive == true && (bool)x.User.IsActive
            //                                   )?.ToList(); //IGO-QC def 854 #3 //ITK D908 #issue1 (09-07-2020)

            return _dbContext.UserType.Join(_dbContext.User,
                                    usrTy => new { id = usrTy.UserId },
                                        usr => new { id = usr.Id },
                                        (usrType, usr) => new { usrType, usr })
                             .Where(x => x.usrType.Company.Code == companyCode
                                         && userTypes.Contains(x.usrType.UserTypeName)
                                         && x.usr.IsActive == true
                ).Select(x => new DomainModel.Security.UserTypeInfo
                {
                    UserLogonName = x.usr.SamaccountName,
                    CompanyCode = x.usrType.Company.Code,
                    UserType = x.usrType.UserTypeName,
                    IsActive = (bool)x.usrType.IsActive,
                    UserName = x.usr.Name
                })?.Distinct()?.ToList();
        }
        /** Added for ITK Defect 908(Ref ALM Document 14-05-2020) */

        // Added For Email Notification
        public IList<UserType> GetUserByType(string companyCode, IList<string> userTypes)
        {
            var userTypeData = _dbContext.UserType.Include("User").Where(x => x.Company.Code == companyCode
                                                                        && userTypes.Contains(x.UserTypeName)
                                                                        && x.IsActive == true
                                                                        && x.User.IsActive == true).ToList();
            return userTypeData;
        }

        // Added For Email expiry Notification
        public IList<UserType> GetUserByType(IList<int> companyIds, IList<string> userTypes)
        {
            var userTypeData = _dbContext.UserType.Include("User").Where(x => companyIds.Contains(x.CompanyId)
                                                                        && userTypes.Contains(x.UserTypeName)
                                                                        && x.IsActive == true
                                                                        && x.User.IsActive == true).ToList();
            return userTypeData;
        }

        public IList<User> GetUserByCompanyAndName(string companyCode, string userName)
        {
            var userTypeData = _dbContext.UserType?.Include("User").Where(x => x.Company.Code == companyCode
                                                                        && x.User.Name == userName
                                                                        && x.IsActive == true
                                                                        && x.User.IsActive == true)?.ToList();
            var resultUsers = userTypeData?.Select(x => x.User)?.ToList();
            return resultUsers;
        }

        public IList<DomainModel.Security.UserCompanyRole> GetUserCompanyRoles(string userName, string menuName)
        {
            var result = _dbContext.UserRole.Where(x => x.User.SamaccountName == userName
                                               && x.Role.RoleActivity.Any(x1 => x1.Module.ApplicationMenu.Any(x2 => x2.MenuName == menuName))
                                               ).ToList();
            return _mapper.Map<IList<DomainModel.Security.UserCompanyRole>>(result, opt => opt.Items["menu"] = menuName);
        }
    }
}