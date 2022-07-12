using AutoMapper;
using Evolution.Admin.Domain.Enums;
using Evolution.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Admin.Domain.Models.Admins;

namespace Evolution.Admin.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            CreateMap<DbModel.User, DomainModel.User>()
                  .ForMember(dest => dest.Username, src => src.MapFrom(x => x.SamaccountName))
                  .ForMember(dest => dest.CompanyCode, src => src.MapFrom(x => x.Company.Code))
                  .ForMember(dest => dest.DisplayName, src => src.MapFrom(x => x.Name))
                  .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
                  .ForMember(dest => dest.Culture, src => src.MapFrom(x => x.Culture))
                  .ForMember(dest => dest.Email, src => src.MapFrom(x => x.Email))
                  .ForMember(dest => dest.CreatedDate, src => src.MapFrom(x => x.CreatedDate))
                  .ForMember(dest => dest.LastLoginDate, src => src.MapFrom(x => x.LastLoginDate))
                  .ForMember(dest => dest.CompanyName, src => src.MapFrom(x => x.Company.Name))
                  .ForMember(dest => dest.CompanyOffice, src => src.MapFrom(x => x.CompanyOffice.OfficeName))
                  .ForMember(dest => dest.userId, src => src.MapFrom(x => x.Id))
                  .ForMember(dest => dest.UserType, src => src.Ignore())
                  .ForMember(dest => dest.Status, opt => opt.ResolveUsing((src,dst,arg3,context)=> context.Options.Items.ContainsKey("compCodes")? GetStatus(src, (IList<string>)context.Options.Items["compCodes"]): null))
                  .ReverseMap()
                  .ForAllOtherMembers(src => src.Ignore());


            CreateMap<DbModel.Announcement, DomainModel.Announcement>()
                 .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                 .ForMember(dest => dest.Header, src => src.MapFrom(x => x.Header))
                 .ForMember(dest => dest.Text, src => src.MapFrom(x => x.Text))
                 .ForMember(dest => dest.DisplayTill, src => src.MapFrom(x => x.DisplayTill))
                 .ForMember(dest => dest.TextColour, src => src.MapFrom(x => x.TextColour))
                 .ForMember(dest => dest.BackgroundColour, src => src.MapFrom(x => x.BackgroundColour))
                 .ForMember(dest => dest.IsEvolutionLocked, src => src.MapFrom(x => x.IsEvolutionLocked))
                 .ForMember(dest => dest.EvolutionLockMessage, src => src.MapFrom(x => x.EvolutionLockMessage))
                 .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                 .ReverseMap()
                 .ForAllOtherMembers(src => src.Ignore());

            CreateMap<DbModel.BatchProcess, DomainModel.Batches>()
                  .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                  .ForMember(dest => dest.ParamID, src => src.MapFrom(x => x.ParamId))
                  .ForMember(dest => dest.ProcessStatus, src => src.MapFrom(x => x.ProcessStatus))
                  .ForMember(dest => dest.BatchID, src => src.MapFrom(x => x.BatchId))
                  .ForMember(dest => dest.ErrorMessage, src => src.MapFrom(x => x.ErrorMessage))
                  .ForMember(dest => dest.UpdatedDate, src => src.MapFrom(x => x.UpdatedDate))
                  .ForMember(dest => dest.CreatedDate, src => src.MapFrom(x => x.CreatedDate))
                  .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => x.CreatedBy))
                  .ForMember(dest => dest.ReportFileName, src => src.MapFrom(x => x.ReportFileName))
                  .ForMember(dest => dest.ReportFilePath, src => src.MapFrom(x => x.ReportFilePath))
                  .ForMember(dest => dest.ReportParam, src => src.MapFrom(x => x.ReportParam))
                  .ForMember(dest => dest.ReportType, src => src.MapFrom(x => x.ReportType))
                  .ForMember(dest => dest.FileExtension, src => src.MapFrom(x => x.FileExtension))
                  .ForMember(dest => dest.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
                  .ReverseMap()
                  .ForAllOtherMembers(src => src.Ignore());
        }

        private string GetStatus(DbModel.User user, IList<string> companyCodes)
        { 
            var userType = user?.UserType?.Where(x => companyCodes.Contains(x.Company.Code)); 
            // ITK DEF 126 fixes
            return (user.IsActive == true && userType.Any(x1 => x1.IsActive == true && x1.UserTypeName == EnumExtension.DisplayName(UserType.MICoordinator))) ? null
               : (user.IsActive == true && userType.Any(x1 => x1.IsActive == true) && userType.All(x1 => x1.UserTypeName != EnumExtension.DisplayName(UserType.MICoordinator))) ? EnumExtension.DisplayName(MICoordinatorStatus.DONOTUSE)
               : (user.IsActive == true && userType.Any(x1 => x1.IsActive == false && x1.UserTypeName == EnumExtension.DisplayName(UserType.MICoordinator))) ? EnumExtension.DisplayName(MICoordinatorStatus.InActive)
               : (user.IsActive == true && !(userType.Any(x1 => x1.IsActive == false && x1.UserTypeName == EnumExtension.DisplayName(UserType.MICoordinator)))) ? EnumExtension.DisplayName(MICoordinatorStatus.InActive)
               : (user.IsActive == false) ? EnumExtension.DisplayName(MICoordinatorStatus.InActive) : null;
        }
    }
}
