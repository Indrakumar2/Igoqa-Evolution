using AutoMapper;
using Resolver=Evolution.Automapper.Resolver;
using System;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Security.Domain.Models.Security;
using System.Collections.Generic;
using System.Linq;
using Evolution.Security.Domain.Models.Security;

namespace Evolution.Security.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            #region UserInfo 
            CreateMap<DbModel.User, DomainModel.UserInfo>()
                        .ForMember(dest => dest.UserId, source => source.MapFrom(x => x.Id))
                        .ForMember(dest => dest.ApplicationName, source => source.MapFrom(x => x.Application.Name))
                        .ForMember(dest => dest.UserName, source => source.MapFrom(x => x.Name))
                        .ForMember(dest => dest.LogonName, source => source.MapFrom(x => x.SamaccountName))
                        .ForMember(dest => dest.Email, source => source.MapFrom(x => x.Email))
                        .ForMember(dest => dest.PhoneNumber, source => source.MapFrom(x => x.PhoneNumber))
                        .ForMember(dest => dest.IsAccountLocked, source => source.MapFrom(x => x.LockoutEnabled))
                        .ForMember(dest => dest.LockoutEndDateUtc, source => source.MapFrom(x => x.LockoutEndDateUtc)) 
                        .ForMember(dest => dest.CompanyCode, source => source.MapFrom(x => x.Company.Code))
                        .ForMember(dest => dest.CompanyName, source => source.MapFrom(x => x.Company.Name))
                        .ForMember(dest => dest.CompanyOfficeName, source => source.MapFrom(x => x.CompanyOffice.OfficeName))
                        .ForMember(dest => dest.AuthenticationMode, source => source.MapFrom(x => x.AuthenticationMode))
                        .ForMember(dest => dest.DefaultCompanyUserType, source => source.ResolveUsing<Resolver.Security.DefaultCompanyUsertypesResolver, ICollection<DbModel.UserType>>("UserType"))
                        .ForMember(dest => dest.SecurityQuestion1, source => source.MapFrom(x => x.SecurityQuestion1))
                        .ForMember(dest => dest.SecurityQuestion1Answer, source => source.MapFrom(x => x.SecurityQuestion1Answer))
                        .ForMember(dest => dest.IsActive, source => source.MapFrom(x => x.IsActive))
                        .ForMember(dest => dest.Password, source => source.MapFrom(x => x.PasswordHash))
                        .ForMember(dest => dest.IsPasswordNeedToBeChange, source => source.MapFrom(x => x.IsPasswordNeedToBeChange))
                        .ForMember(dest => dest.Culture, source => source.MapFrom(x => x.Culture))
                        .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                        .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                        .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                        .ForMember(dest => dest.IsShowNewVisit, source => source.MapFrom(x => x.IsShowNewVisit))
                        .ForMember(dest => dest.ExtranetAccessLevel, source => source.MapFrom(x => x.ExtranetAccessLevel))
                        .ForMember(dest => dest.Comments, source => source.MapFrom(x => x.Comments))
                        .ForMember(dest => dest.CustomerUserProjectNumbers, source => source.MapFrom(x => x.CustomerUserProjectAccess))
                        .ForAllOtherMembers(x => x.Ignore());
            CreateMap<DomainModel.UserInfo, DbModel.User>()
                        .ForMember(dest => dest.Id, source => source.MapFrom(x => x.UserId))
                        .ForMember(dest => dest.ApplicationId, source => source.ResolveUsing<Resolver.Security.ApplicationIdResolver, string>("ApplicationName"))
                        .ForMember(dest => dest.Name, source => source.MapFrom(x => x.UserName))
                        .ForMember(dest => dest.SamaccountName, source => source.MapFrom(x => x.LogonName))
                        .ForMember(dest => dest.Email, source => source.MapFrom(x => x.Email))
                        .ForMember(dest => dest.PhoneNumber, source => source.MapFrom(x => x.PhoneNumber))
                        .ForMember(dest => dest.LockoutEnabled, source => source.MapFrom(x => x.IsAccountLocked))
                        .ForMember(dest => dest.LockoutEndDateUtc, source => source.MapFrom(x => x.LockoutEndDateUtc))
                        .ForMember(dest => dest.CompanyId, source => source.ResolveUsing<Resolver.Company.CompanyIdResolver, string>("CompanyCode"))
                        .ForMember(dest => dest.CompanyOfficeId, source => source.ResolveUsing<Resolver.Company.CompanyOfficeIdResolver, string>("CompanyOfficeName"))
                        //.ForMember(dest => dest.CustomerUserProjectAccess, source => source.ResolveUsing<Resolver.Customer.CustomerProjectIdResolver, IList<CustomerUserProject>>("CustomerUserProjectNumbers"))
                        .ForMember(dest => dest.AuthenticationMode, source => source.MapFrom(x => x.AuthenticationMode))
                        //.ForMember(dest => dest.UserType, source => source.MapFrom(x => x.UserType))
                        .ForMember(dest => dest.SecurityQuestion1, source => source.MapFrom(x => x.SecurityQuestion1))
                        .ForMember(dest => dest.SecurityQuestion1Answer, source => source.MapFrom(x => x.SecurityQuestion1Answer))
                        .ForMember(dest => dest.IsActive, source => source.MapFrom(x => x.IsActive))
                        .ForMember(dest => dest.PasswordHash, source => source.MapFrom(x => x.Password))
                        .ForMember(dest => dest.IsPasswordNeedToBeChange, source => source.MapFrom(x => x.IsPasswordNeedToBeChange))
                        .ForMember(dest => dest.Culture, source => source.MapFrom(x => x.Culture))
                        .ForMember(dest => dest.IsShowNewVisit, source => source.MapFrom(x => x.IsShowNewVisit))
                        .ForMember(dest => dest.ExtranetAccessLevel, source => source.MapFrom(x => x.ExtranetAccessLevel)) 
                        .ForMember(dest => dest.Comments, source => source.MapFrom(x => x.Comments))
                        .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                        .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                        .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region UserInfo 
            CreateMap<DbModel.User, DomainModel.UserInfos>()
                        .ForMember(dest => dest.UserId, source => source.MapFrom(x => x.Id))
                        .ForMember(dest => dest.ApplicationName, source => source.MapFrom(x => x.Application.Name))
                        .ForMember(dest => dest.UserName, source => source.MapFrom(x => x.Name))
                        .ForMember(dest => dest.LogonName, source => source.MapFrom(x => x.SamaccountName))
                        .ForMember(dest => dest.Email, source => source.MapFrom(x => x.Email))
                        .ForMember(dest => dest.PhoneNumber, source => source.MapFrom(x => x.PhoneNumber))
                        .ForMember(dest => dest.IsAccountLocked, source => source.MapFrom(x => x.LockoutEnabled))
                        .ForMember(dest => dest.LockoutEndDateUtc, source => source.MapFrom(x => x.LockoutEndDateUtc))
                        .ForMember(dest => dest.CompanyCode, source => source.MapFrom(x => x.Company.Code))
                        .ForMember(dest => dest.CompanyName, source => source.MapFrom(x => x.Company.Name))
                        .ForMember(dest => dest.CompanyOfficeName, source => source.MapFrom(x => x.CompanyOffice.OfficeName))
                        .ForMember(dest => dest.AuthenticationMode, source => source.MapFrom(x => x.AuthenticationMode))
                        .ForMember(dest => dest.SecurityQuestion1, source => source.MapFrom(x => x.SecurityQuestion1))
                        .ForMember(dest => dest.SecurityQuestion1Answer, source => source.MapFrom(x => x.SecurityQuestion1Answer))
                        .ForMember(dest => dest.IsActive, source => source.MapFrom(x => x.IsActive))
                        .ForMember(dest => dest.Password, source => source.MapFrom(x => x.PasswordHash))
                        .ForMember(dest => dest.IsPasswordNeedToBeChange, source => source.MapFrom(x => x.IsPasswordNeedToBeChange))
                        .ForMember(dest => dest.Culture, source => source.MapFrom(x => x.Culture))
                        .ForMember(dest => dest.CreatedDate, source => source.MapFrom(x => x.CreatedDate))
                        .ForMember(dest => dest.LastLoginDate, source => source.MapFrom(x => x.LastLoginDate))
                        .ForMember(dest => dest.Culture, source => source.MapFrom(x => x.Culture))
                        .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                        .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                        .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                        .ForMember(dest => dest.UserType, source => source.MapFrom(x => x.UserType!=null && x.UserType.Any() ? x.UserType.Select(x1=>x1.UserTypeName) : null ))
                        .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModel.UserInfos, DbModel.User>()
                        .ForMember(dest => dest.Id, source => source.MapFrom(x => x.UserId))
                        .ForMember(dest => dest.ApplicationId, source => source.ResolveUsing<Resolver.Security.ApplicationIdResolver, string>("ApplicationName"))
                        .ForMember(dest => dest.Name, source => source.MapFrom(x => x.UserName))
                        .ForMember(dest => dest.SamaccountName, source => source.MapFrom(x => x.LogonName))
                        .ForMember(dest => dest.Email, source => source.MapFrom(x => x.Email))
                        .ForMember(dest => dest.PhoneNumber, source => source.MapFrom(x => x.PhoneNumber))
                        .ForMember(dest => dest.LockoutEndDateUtc, source => source.MapFrom(x => x.LockoutEndDateUtc))
                        .ForMember(dest => dest.CompanyId, source => source.ResolveUsing<Resolver.Company.CompanyIdResolver, string>("CompanyCode"))
                        .ForMember(dest => dest.CompanyOfficeId, source => source.ResolveUsing<Resolver.Company.CompanyOfficeIdResolver, string>("CompanyOfficeName"))
                        .ForMember(dest => dest.AuthenticationMode, source => source.MapFrom(x => x.AuthenticationMode))
                        //.ForMember(dest => dest.UserType, source => source.MapFrom(x => x.UserType))
                        .ForMember(dest => dest.SecurityQuestion1, source => source.MapFrom(x => x.SecurityQuestion1))
                        .ForMember(dest => dest.SecurityQuestion1Answer, source => source.MapFrom(x => x.SecurityQuestion1Answer))
                        .ForMember(dest => dest.IsActive, source => source.MapFrom(x => x.IsActive))
                        .ForMember(dest => dest.PasswordHash, source => source.MapFrom(x => x.Password))
                        .ForMember(dest => dest.IsPasswordNeedToBeChange, source => source.MapFrom(x => x.IsPasswordNeedToBeChange))
                        .ForMember(dest => dest.Culture, source => source.MapFrom(x => x.Culture))
                        .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                        .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                        .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region UserRoleInfo
            CreateMap<DbModel.UserRole, DomainModel.UserRoleInfo>()
                       .ForMember(dest => dest.ApplicationName, source => source.MapFrom(x => x.User.Application.Name))
                       .ForMember(dest => dest.UserLogonName, source => source.MapFrom(x => x.User.SamaccountName))
                       .ForMember(dest => dest.RoleName, source => source.MapFrom(x => x.Role.Name))
                       .ForMember(dest => dest.CompanyCode, source => source.MapFrom(x => x.Company.Code))
                       .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModel.UserRoleInfo,DbModel.UserRole>()
                      .ForMember(dest => dest.ApplicationId, source => source.ResolveUsing<Resolver.Security.ApplicationIdResolver, string>("ApplicationName"))
                      .ForMember(dest => dest.UserId, source => source.ResolveUsing<Resolver.Security.UserIdResolver, string>("UserLogonName"))
                      .ForMember(dest => dest.RoleId, source => source.ResolveUsing<Resolver.Security.RoleIdResolver, string>("RoleName"))
                      .ForMember(dest => dest.CompanyId, source => source.ResolveUsing<Resolver.Company.CompanyIdResolver, string>("CompanyCode"))
                      .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region UserTypeInfo
            CreateMap<DbModel.UserType, DomainModel.UserTypeInfo>()
                       .ForMember(dest => dest.UserLogonName, source => source.MapFrom(x => x.User.SamaccountName))
                       .ForMember(dest => dest.UserType, source => source.MapFrom(x => x.UserTypeName))
                       .ForMember(dest => dest.IsActive, source => source.MapFrom(X=>X.IsActive))
                       .ForMember(dest => dest.CompanyCode, source => source.MapFrom(x => x.Company.Code))
                       .ForMember(dest => dest.UserName, source => source.MapFrom(x => x.User.Name)) //Added for ITK Defect 908(Ref ALM Document 14-05-2020)
                       .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModel.UserTypeInfo, DbModel.UserType>()
                      .ForMember(dest => dest.UserId, source => source.ResolveUsing<Resolver.Security.UserIdResolver, string>("UserLogonName"))
                      .ForMember(dest => dest.UserTypeName, source => source.MapFrom(x => x.UserType))
                      .ForMember(dest => dest.IsActive, source => source.MapFrom(x=>x.IsActive))
                      .ForMember(dest => dest.CompanyId, source => source.ResolveUsing<Resolver.Company.CompanyIdResolver, string>("CompanyCode"))
                      .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region RoleInfo 
            CreateMap<DomainModel.RoleInfo, DbModel.Role>()
                        .ForMember(dest => dest.ApplicationId,source=> source.ResolveUsing<Resolver.Security.ApplicationIdResolver, string>("ApplicationName"))
                        .ForMember(source => source.Id, dest => dest.MapFrom(x => x.RoleId))
                        .ForMember(source => source.Name, dest => dest.MapFrom(x => x.RoleName))
                        .ForMember(source => source.Description, dest => dest.MapFrom(x => x.Description))
                        .ForMember(source => source.IsAllowedDuringLock, dest => dest.MapFrom(x => x.IsAllowedDuringLock))
                        .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                        .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                        .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                        .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.Role, DomainModel.RoleInfo>()
                        .ForMember(source => source.RoleId, dest => dest.MapFrom(x => x.Id))
                        .ForMember(source => source.RoleName, dest => dest.MapFrom(x => x.Name))
                        .ForMember(source => source.ApplicationName, dest => dest.MapFrom(x => x.Application.Name))
                        .ForMember(source => source.Description, dest => dest.MapFrom(x => x.Description))
                        .ForMember(source => source.IsAllowedDuringLock, dest => dest.MapFrom(x => x.IsAllowedDuringLock))
                        .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
                        .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
                        .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
                        .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region ActivityInfo 
            CreateMap<DomainModel.ActivityInfo, DbModel.Activity>()
                        .ForMember(dest => dest.ApplicationId, source=> source.ResolveUsing<Resolver.Security.ApplicationIdResolver, string>("ApplicationName"))
                        .ForMember(dest => dest.Id, source => source.MapFrom(x => x.ActivityId))
                        .ForMember(dest => dest.Code, source => source.MapFrom(x => x.ActivityCode))
                        .ForMember(dest => dest.Name, source => source.MapFrom(x => x.ActivityName))
                        .ForMember(dest => dest.Description, source => source.MapFrom(x => x.Description))
                        .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                        .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                        .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                        .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.Activity, DomainModel.ActivityInfo>()
                        .ForMember(dest => dest.ApplicationName, source => source.MapFrom(x => x.Application.Name))
                        .ForMember(dest => dest.ActivityId, source => source.MapFrom(x => x.Id))
                        .ForMember(dest => dest.ActivityCode, source => source.MapFrom(x => x.Code))
                        .ForMember(dest => dest.ActivityName, source => source.MapFrom(x => x.Name))
                        .ForMember(dest => dest.Description, source => source.MapFrom(x => x.Description))
                        .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                        .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                        .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                        .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region ModuleInfo 
            CreateMap<DomainModel.ModuleInfo, DbModel.Module>()
                .ForMember(dest => dest.ApplicationId, source => source.ResolveUsing<Resolver.Security.ApplicationIdResolver, string>("ApplicationName"))
                .ForMember(dest => dest.Id, source => source.MapFrom(x => x.ModuleId))
                .ForMember(dest => dest.Name, source => source.MapFrom(x => x.ModuleName))
                .ForMember(dest => dest.Description, source => source.MapFrom(x => x.Description))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.Module, DomainModel.ModuleInfo>()
                      .ForMember(dest => dest.ModuleId, source => source.MapFrom(x => x.Id))
                      .ForMember(dest => dest.ModuleName, source => source.MapFrom(x => x.Name))
                      .ForMember(dest => dest.ApplicationName, source => source.MapFrom(x => x.Application.Name))
                      .ForMember(dest => dest.Description, source => source.MapFrom(x => x.Description))
                      .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region RoleActivityInfo 

            CreateMap<DbModel.RoleActivity, DomainModel.ActivityInfo>()
                      .ForMember(dest => dest.ActivityId, src => src.MapFrom(x => x.ActivityId))
                      .ForMember(dest => dest.ActivityCode, src => src.MapFrom(x => x.Activity.Code))
                      .ForMember(dest => dest.ActivityName, src => src.MapFrom(x => x.Activity.Name))
                      .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Activity.Description))
                      .ForMember(dest => dest.ApplicationName, src => src.MapFrom(x => x.Activity.Application.Name))
                      .ForMember(dest => dest.ModuleId, src => src.MapFrom(x => x.ModuleId))
                      .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Application Menu
            CreateMap<DbModel.ApplicationMenu, DomainModel.ApplicationMenuInfo>()
                      .ForMember(dest => dest.ApplicationName, source => source.MapFrom(x => x.Application.Name))
                      .ForMember(dest => dest.ModuleName, source => source.MapFrom(x => x.Module.Name))
                      .ForMember(dest => dest.MenuName, source => source.MapFrom(x => x.MenuName))
                      .ForMember(dest => dest.IsActive, source => source.MapFrom(x => x.IsActive))
                      .ForMember(dest => dest.ActivityCodes, source => source.MapFrom(x => x.ActivitiesCode))
                      .ReverseMap()
                      .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Application
            CreateMap<DbModel.Application, DomainModel.ApplicationInfo>()
                      .ForMember(dest => dest.ApplicationName, source => source.MapFrom(x => x.Name))
                      .ForMember(dest => dest.Description, source => source.MapFrom(x => x.Description))
                      .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                      .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                      .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                      .ReverseMap()
                      .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region UserCompanyRole

            CreateMap<DbModel.UserRole, DomainModel.UserCompanyRole>()
               .ForMember(dest => dest.CompanyCode, src => src.MapFrom(x => x.Company.Code))
               .ForMember(dest => dest.CompanyName, src => src.MapFrom(x => x.Company.Name))
               .ForMember(dest => dest.RoleName, src => src.MapFrom(x => x.Role.Name))
               .ForMember(dest => dest.RoleActivities , opt => opt.ResolveUsing((src,dst,arg,cntxt) => string.IsNullOrEmpty((string)cntxt.Items["menu"])? src.Role.RoleActivity : src.Role.RoleActivity.Where(x=>x.Module.ApplicationMenu.Any(x2=> x2.MenuName == (string)cntxt.Items["menu"]))))
               .ReverseMap()
               .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.RoleActivity, DomainModel.UserRoleActivity>()
                .ForMember(dest => dest.ActivityCode, src => src.MapFrom(x => x.Activity.Code))
                .ForMember(dest => dest.ActivityName, src => src.MapFrom(x => x.Activity.Name))
                .ForMember(dest => dest.ModuleName, src => src.MapFrom(x => x.Module.Name))
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());

            #endregion

            CreateMap<DbModel.CustomerUserProjectAccess, DomainModel.CustomerUserProject>()
                .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                .ForMember(dest => dest.UserId, source => source.MapFrom(x => x.UserId))
                .ForMember(dest => dest.ProjectNumber, source => source.MapFrom(x => x.ProjectId))
                .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModel.ExtranetUserInfo,DomainModel.UserInfo>() 
                        .ForMember(dest => dest.UserId, source => source.MapFrom(x => x.UserId))
                        .ForMember(dest => dest.ApplicationName, source => source.MapFrom(x => x.ApplicationName))
                        .ForMember(dest => dest.UserName, source => source.MapFrom(x => x.UserName))
                        .ForMember(dest => dest.LogonName, source => source.MapFrom(x => x.LogonName))
                        .ForMember(dest => dest.Email, source => source.MapFrom(x => x.Email))
                        .ForMember(dest => dest.PhoneNumber, source => source.MapFrom(x => x.PhoneNumber))
                        .ForMember(dest => dest.LockoutEndDateUtc, source => source.MapFrom(x => x.LockoutEndDateUtc))
                        .ForMember(dest => dest.CompanyCode, source => source.MapFrom(x => x.CompanyCode))
                        .ForMember(dest => dest.CompanyOfficeName, source => source.MapFrom(x => x.CompanyOfficeName))
                        .ForMember(dest => dest.CustomerUserProjectNumbers, source => source.MapFrom(x => x.CustomerUserProjectNumbers))
                        .ForMember(dest => dest.AuthenticationMode, source => source.MapFrom(x => x.AuthenticationMode))
                        .ForMember(dest => dest.UserType, source => source.MapFrom(x => x.UserType))
                        .ForMember(dest => dest.SecurityQuestion1, source => source.MapFrom(x => x.SecurityQuestion1))
                        .ForMember(dest => dest.SecurityQuestion1Answer, source => source.MapFrom(x => x.SecurityQuestion1Answer))
                        .ForMember(dest => dest.IsActive, source => source.MapFrom(x => x.IsActive))
                        .ForMember(dest => dest.IsAccountLocked, source => source.MapFrom(x => x.IsAccountLocked))
                        .ForMember(dest => dest.IsShowNewVisit, source => source.MapFrom(x => x.IsShowNewVisit))
                        .ForMember(dest => dest.ExtranetAccessLevel, source => source.MapFrom(x => x.ExtranetAccessLevel))
                        .ForMember(dest => dest.Comments, source => source.MapFrom(x => x.Comments))
                        .ForMember(dest => dest.Password, source => source.MapFrom(x => x.Password))
                        .ForMember(dest => dest.IsPasswordNeedToBeChange, source => source.MapFrom(x => x.IsPasswordNeedToBeChange))
                        .ForMember(dest => dest.Culture, source => source.MapFrom(x => x.Culture))
                        .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                        .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                        .ReverseMap()
                        .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModel.ExtranetUserInfo, DbModel.User>()
                        .ForMember(dest => dest.Id, source => source.MapFrom(x => x.UserId))
                        .ForMember(dest => dest.ApplicationId, source => source.ResolveUsing<Resolver.Security.ApplicationIdResolver, string>("ApplicationName"))
                        .ForMember(dest => dest.Name, source => source.MapFrom(x => x.UserName))
                        .ForMember(dest => dest.SamaccountName, source => source.MapFrom(x => x.LogonName))
                        .ForMember(dest => dest.Email, source => source.MapFrom(x => x.Email))
                        .ForMember(dest => dest.PhoneNumber, source => source.MapFrom(x => x.PhoneNumber))
                        .ForMember(dest => dest.LockoutEndDateUtc, source => source.MapFrom(x => x.LockoutEndDateUtc))
                        .ForMember(dest => dest.CompanyId, source => source.ResolveUsing<Resolver.Company.CompanyIdResolver, string>("CompanyCode"))
                        .ForMember(dest => dest.CompanyOfficeId, source => source.ResolveUsing<Resolver.Company.CompanyOfficeIdResolver, string>("CompanyOfficeName"))
                        .ForMember(dest => dest.CustomerUserProjectAccess, source => source.ResolveUsing<Resolver.Customer.CustomerProjectIdResolver, IList<CustomerUserProject>>("CustomerUserProjectNumbers"))
                        .ForMember(dest => dest.AuthenticationMode, source => source.MapFrom(x => x.AuthenticationMode)) 
                        .ForMember(dest => dest.SecurityQuestion1, source => source.MapFrom(x => x.SecurityQuestion1))
                        .ForMember(dest => dest.SecurityQuestion1Answer, source => source.MapFrom(x => x.SecurityQuestion1Answer))
                        .ForMember(dest => dest.IsActive, source => source.MapFrom(x => x.IsActive))
                        .ForMember(dest => dest.LockoutEnabled, source => source.MapFrom(x => x.IsAccountLocked))
                        .ForMember(dest => dest.IsShowNewVisit, source => source.MapFrom(x => x.IsShowNewVisit))
                        .ForMember(dest => dest.ExtranetAccessLevel, source => source.MapFrom(x => x.ExtranetAccessLevel))
                        .ForMember(dest => dest.PasswordHash, source => source.MapFrom(x => x.Password)) 
                        .ForMember(dest => dest.IsPasswordNeedToBeChange, source => source.MapFrom(x => x.IsPasswordNeedToBeChange))
                        .ForMember(dest => dest.Culture, source => source.MapFrom(x => x.Culture))
                        .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                        .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                        .ForMember(dest => dest.Comments, source => source.MapFrom(x => x.Comments))
                        .ForAllOtherMembers(x => x.Ignore());

        }
    }
}