using AutoMapper;
using Evolution.AuditLog.Core.Mappers.Resolvers;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.AuditLog.Domain.Models.Audit;

namespace Evolution.AuditLog.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            #region SQLAudit Log Detail 
            CreateMap<DbModel.SqlauditLogDetail, DomainModel.SqlAuditLogDetailInfo>()
                        .ForMember(dest => dest.ActionBy, source => source.MapFrom(x => x.SqlAuditLog.ActionBy))
                        .ForMember(dest => dest.ActionOn, source => source.MapFrom(x => x.SqlAuditLog.ActionOn))
                        .ForMember(dest => dest.ActionType, source => source.MapFrom(x => x.SqlAuditLog.ActionType))
                        .ForMember(dest => dest.AuditModuleName, source => source.MapFrom(x => x.SqlAuditLog.SqlAuditModule.ModuleName))
                        .ForMember(dest => dest.AuditSubModuleName, source => source.MapFrom(x => x.SqlAuditSubModule.ModuleName))
                        .ForMember(dest => dest.LogId, source => source.MapFrom(x => x.SqlAuditLogId))
                        .ForMember(dest => dest.NewValue, source => source.MapFrom(x => x.NewValue))
                        .ForMember(dest => dest.OldValue, source => source.MapFrom(x => x.OldValue))
                        .ForMember(dest => dest.SearchReference, source => source.MapFrom(x => x.SqlAuditLog.SearchReference))
                        .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModel.SqlAuditLogDetailInfo, DbModel.SqlauditLogDetail>()
                        .ForMember(dest => dest.SqlAuditLogId, source => source.MapFrom(x => x.LogId))
                        .ForMember(dest => dest.SqlAuditSubModuleId, source => source.ResolveUsing<SqlauditModuleResolver, string>("AuditSubModuleName"))
                        .ForMember(dest => dest.NewValue, source => source.MapFrom(x => x.NewValue))
                        .ForMember(dest => dest.OldValue, source => source.MapFrom(x => x.OldValue))
                        .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region SQLAudit Log Event 
            CreateMap<DbModel.SqlauditLogEvent, DomainModel.SqlAuditLogDetailInfo>()
                        .ForMember(dest => dest.ActionBy, source => source.MapFrom(x => x.ActionBy))
                        .ForMember(dest => dest.ActionOn, source => source.MapFrom(x => x.ActionOn))
                        .ForMember(dest => dest.ActionType, source => source.MapFrom(x => x.ActionType))
                        .ForMember(dest => dest.AuditModuleName, source => source.MapFrom(x => x.SqlAuditModule.ModuleName))
                        .ForMember(dest => dest.SearchReference, source => source.MapFrom(x => x.SearchReference))
                        .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModel.SqlAuditLogDetailInfo, DbModel.SqlauditLogEvent>()
                        .ForMember(dest => dest.Id, source => source.MapFrom(x => x.LogId))
                        .ForMember(dest => dest.ActionBy, source => source.MapFrom(x => x.ActionBy))
                        .ForMember(dest => dest.ActionOn, source => source.MapFrom(x => x.ActionOn))
                        .ForMember(dest => dest.ActionType, source => source.MapFrom(x => x.ActionType))
                        .ForMember(dest => dest.SqlAuditModuleId, source => source.ResolveUsing<SqlauditModuleResolver, string>("AuditModuleName"))
                        .ForMember(dest => dest.SearchReference, source => source.MapFrom(x => x.SearchReference))
                        .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.SqlauditLogEvent, DomainModel.SqlAuditLogEventInfo>()
                       .ForMember(dest => dest.LogId, source => source.MapFrom(x => x.Id))
                       .ForMember(dest => dest.ActionBy, source => source.MapFrom(x => x.ActionBy))
                       .ForMember(dest => dest.ActionOn, source => source.MapFrom(x => x.ActionOn))
                       .ForMember(dest => dest.ActionType, source => source.MapFrom(x => x.ActionType))
                       .ForMember(dest => dest.AuditModuleName, source => source.MapFrom(x => x.SqlAuditModule.ModuleName))
                       .ForMember(dest => dest.SearchReference, source => source.MapFrom(x => x.SearchReference))
                       .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModel.SqlAuditLogEventInfo, DbModel.SqlauditLogEvent>()
                        .ForMember(dest => dest.ActionBy, source => source.MapFrom(x => x.ActionBy))
                        .ForMember(dest => dest.ActionOn, source => source.MapFrom(x => x.ActionOn))
                        .ForMember(dest => dest.ActionType, source => source.MapFrom(x => x.ActionType))
                        .ForMember(dest => dest.SqlAuditModuleId, source => source.ResolveUsing<SqlauditModuleResolver, string>("AuditModuleName"))
                        .ForMember(dest => dest.SearchReference, source => source.MapFrom(x => x.SearchReference))
                        .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModel.SqlAuditLogEventInfo, DomainModel.SqlAuditLogDetailInfo>()
                       .ForMember(dest => dest.LogId, source => source.MapFrom(x => x.LogId))
                       .ForMember(dest => dest.ActionBy, source => source.MapFrom(x => x.ActionBy))
                       .ForMember(dest => dest.ActionOn, source => source.MapFrom(x => x.ActionOn))
                       .ForMember(dest => dest.ActionType, source => source.MapFrom(x => x.ActionType))
                       .ForMember(dest => dest.AuditSubModuleName, source => source.MapFrom(x => x.AuditModuleName))
                       .ForMember(dest => dest.SearchReference, source => source.MapFrom(x => x.SearchReference))
                       .ReverseMap()
                       .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModel.SqlAuditLogEventSearchInfo, DbModel.SqlauditLogEvent>()
                        .ForMember(dest => dest.ActionBy, source => source.MapFrom(x => x.ActionBy))
                        .ForMember(dest => dest.ActionType, source => source.MapFrom(x => x.ActionType))
                        .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModel.SqlAuditLogEventSearchInfo, DomainModel.SqlAuditLogDetailInfo>()
                    .ForMember(dest => dest.LogId, source => source.MapFrom(x => x.LogId))
                    .ForMember(dest => dest.ActionBy, source => source.MapFrom(x => x.ActionBy))
                    .ForMember(dest => dest.ActionType, source => source.MapFrom(x => x.ActionType))
                   .ForMember(dest => dest.SearchReference, source => source.MapFrom(x => x.SearchReference))
                      .ForAllOtherMembers(x => x.Ignore());           


            CreateMap<DbModel.AuditSearch, DomainModel.AuditSearch>()
            .ForMember(dest => dest.AuditSearchId, src => src.MapFrom(x => x.Id))
            .ForMember(dest => dest.Module, src => src.MapFrom(x => x.Module.ModuleName))
            .ForMember(dest => dest.SearchName, src => src.MapFrom(x => x.SearchName))
            .ForMember(dest => dest.ModuleId, src => src.MapFrom(x => x.ModuleId))
            .ForMember(dest => dest.ModuleName, src => src.MapFrom(x => x.ModuleName))
            .ForMember(dest => dest.DispalyName, src => src.MapFrom(x => x.DisplayName))
            .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
            .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
            .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
            .ReverseMap()
            .ForAllOtherMembers(x => x.Ignore());

            #endregion
        }
    }
}
