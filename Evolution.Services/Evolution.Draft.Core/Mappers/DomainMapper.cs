using AutoMapper;
using System;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Draft.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Draft.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {  
            #region Draft

            CreateMap<DbModel.Draft, DomainModel.Draft>()
              .ForMember(source => source.Id, dest => dest.MapFrom(x => x.Id))
              .ForMember(source => source.DraftId, dest => dest.MapFrom(x => x.DraftId))
              .ForMember(source => source.Description, dest => dest.MapFrom(x => x.Description))
              .ForMember(source => source.DraftType, dest => dest.MapFrom(x => x.Description))
              .ForMember(source => source.Moduletype, dest => dest.MapFrom(x => x.Moduletype))
              .ForMember(source => source.SerilizableObject, dest => dest.MapFrom(x => x.SerilizableObject))
              .ForMember(source => source.SerilizationType, dest => dest.MapFrom(x => x.SerilizationType))
              .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
              .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
              .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
              .ForMember(source => source.AssignedBy, dest => dest.MapFrom(x => x.AssignedBy))
              .ForMember(source => source.AssignedTo, dest => dest.MapFrom(x => x.AssignedTo))
              .ForMember(source => source.AssignedOn, dest => dest.MapFrom(x => x.AssignedOn))
              .ForMember(source => source.CreatedBy, dest => dest.MapFrom(x => x.CreatedBy))
              .ForMember(source => source.CreatedOn, dest => dest.MapFrom(x => x.CreatedTo))
                .ForMember(dest => dest.CompanyCode, src => src.MapFrom(x => x.Company.Code))//D661 issue1 myTask CR
                .ForMember(dest => dest.CompanyName, src => src.MapFrom(x => x.Company.Name)) //D363 CR Change
              .ForAllOtherMembers(x => x.Ignore());


            CreateMap<DomainModel.Draft, DbModel.Draft>()
            .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
            .ForMember(dest => dest.DraftId, opt => opt.MapFrom(src => src.DraftId))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => string.Format("{0} {1}", src.Description, src.DraftType).Trim()))
            .ForMember(dest => dest.Moduletype, opt => opt.MapFrom(src =>  src.Moduletype))
            .ForMember(dest => dest.SerilizableObject, opt => opt.MapFrom(src => src.SerilizableObject))
            .ForMember(dest => dest.SerilizationType, opt => opt.MapFrom(src => src.SerilizationType))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
            .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
            .ForMember(dest => dest.UpdateCount, opt => opt.MapFrom(src => src.UpdateCount))
            .ForMember(dest => dest.AssignedBy, opt => opt.MapFrom(src => src.AssignedBy))
            .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedTo))
            .ForMember(dest => dest.AssignedOn, opt => opt.MapFrom(src => src.AssignedOn))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.CreatedTo, opt => opt.MapFrom(src => src.CreatedOn))
            .ForMember(dest => dest.CompanyId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("DbCompanies") ? ((List<DbModel.Company>)context.Options.Items["DbCompanies"])?.FirstOrDefault(x => x.Code == src.CompanyCode)?.Id : null)) //D661 issue1 myTask CR
            .ForAllOtherMembers(src => src.Ignore());

            #endregion 
        }
    }
}
