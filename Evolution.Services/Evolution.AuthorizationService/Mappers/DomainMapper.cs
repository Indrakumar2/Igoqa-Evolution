using AutoMapper;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomineModel = Evolution.AuthorizationService.Models;

namespace Evolution.AuthorizationService.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            CreateMap<DbModel.User, DomineModel.User>()
                   .ForMember(dest => dest.UserId, src => src.MapFrom(x => x.Id))
                   .ForMember(dest => dest.Username, src => src.MapFrom(x => x.SamaccountName))
                   .ForMember(dest => dest.DefaultCompanyCode, src => src.MapFrom(x => x.Company.Code))
                   .ForMember(dest => dest.DefaultCompanyId, src => src.MapFrom(x => x.CompanyId))
                   .ForMember(dest => dest.UserType, src => src.ResolveUsing(x => string.Join(",", x.UserType.Where(x1=>x1.CompanyId== x.CompanyId)?.Select(x2=>x2.UserTypeName))))
                   .ForMember(dest => dest.AuthenticationType, src => src.MapFrom(x => x.AuthenticationMode))
                   .ForMember(dest => dest.Application, src => src.MapFrom(x => x.Application.Name))
                   .ForMember(dest => dest.AccessFailedCount, src => src.MapFrom(x => x.AccessFailedCount))
                   .ForMember(dest => dest.DisplayName, src => src.MapFrom(x => x.Name))
                   .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
                   .ForMember(dest => dest.Culture, src => src.MapFrom(x => x.Culture))
                   .ForMember(dest => dest.PasswordHash, src => src.MapFrom(x => x.PasswordHash))
                   .ForMember(dest => dest.IsRoleAssigned, src => src.ResolveUsing(x => x.UserRole != null && x.UserRole.Count>0))
                   .ReverseMap()
                   .ForAllOtherMembers(src => src.Ignore());

            CreateMap<DbModel.RefreshToken, DomineModel.Tokens.RefreshToken>()
                   .ForMember(dest => dest.AccessToken, src => src.MapFrom(x => x.AccessToken))
                   .ForMember(dest => dest.Application, src => src.MapFrom(x => x.Application))
                   .ForMember(dest => dest.RequestedIp, src => src.MapFrom(x => x.RequestedIp))
                   .ForMember(dest => dest.Token, src => src.MapFrom(x => x.Token))
                   .ForMember(dest => dest.TokenExpiretime, src => src.MapFrom(x => x.TokenExpiretime))
                   .ForMember(dest => dest.Username, src => src.MapFrom(x => x.Username))
                   .ReverseMap()
                   .ForAllOtherMembers(src => src.Ignore());
        }
    }
}
