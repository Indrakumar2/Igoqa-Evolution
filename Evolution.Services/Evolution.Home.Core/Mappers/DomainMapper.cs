using AutoMapper;
using Evolution.Common.Constants;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomineModel = Evolution.Home.Domain.Models.Homes;

namespace Evolution.Home.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            #region MyTask 

            CreateMap<DbModel.Task, DomineModel.MyTask>()
             .ForMember(dest => dest.MyTaskId, src => src.MapFrom(x => x.Id))
             .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
             .ForMember(dest => dest.Moduletype, src => src.MapFrom(x => x.Moduletype))
             .ForMember(dest => dest.TaskType, src => src.MapFrom(x => x.TaskType))
             .ForMember(dest => dest.TaskRefCode, src => src.MapFrom(x => x.TaskRefCode))
             .ForMember(dest => dest.AssignedBy, src => src.MapFrom(x => x.AssignedBy.SamaccountName))
             .ForMember(dest => dest.AssignedTo, src => src.MapFrom(x => x.AssignedTo.SamaccountName))
             .ForMember(dest => dest.CreatedOn, src => src.MapFrom(x => x.CreatedOn)) 
             .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
             .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
             .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Description))
             .ForMember(dest => dest.CompanyCode, src => src.MapFrom(x => x.Company.Code))//D661 issue1 myTask CR
             .ForMember(dest => dest.CompanyName, src => src.MapFrom(x => x.Company.Name)) //D363 CR Change
             .ForAllOtherMembers(src => src.Ignore());


            CreateMap<DomineModel.MyTask, DbModel.Task>()
             .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.MyTaskId : null) : src.MyTaskId)))
             .ForMember(dest => dest.Moduletype, opt => opt.MapFrom(src => src.Moduletype))
             .ForMember(dest => dest.TaskType, opt => opt.MapFrom(src => src.TaskType))
             .ForMember(dest => dest.TaskRefCode, opt => opt.MapFrom(src => src.TaskRefCode))
             .ForMember(dest => dest.AssignedById, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Users") ? ((List<DbModel.User>)context.Options.Items["Users"])?.FirstOrDefault(x => x.SamaccountName == src.AssignedBy)?.Id : null))
             .ForMember(dest => dest.AssignedToId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Users") ? ((List<DbModel.User>)context.Options.Items["Users"])?.FirstOrDefault(x => x.SamaccountName == src.AssignedTo)?.Id : null))
             .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))  
             .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
             .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
              .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Description))
              .ForMember(dest => dest.CompanyId, opt => opt.ResolveUsing((src,dst,arg3,context) => context.Options.Items.ContainsKey("DbCompanies") ? ((List<DbModel.Company>)context.Options.Items["DbCompanies"])?.FirstOrDefault(x => x.Code == src.CompanyCode)?.Id : null)) //D661 issue1 myTask CR
             .ForAllOtherMembers(opt => opt.Ignore());

            #endregion

            #region DraftandTask

            CreateMap<Evolution.Draft.Domain.Models.Draft, DomineModel.MyTask>()            
             .ForMember(dest => dest.Moduletype, src => src.MapFrom(x => x.Moduletype))
             .ForMember(dest => dest.AssignedBy, src => src.MapFrom(x => x.AssignedBy))
             .ForMember(dest => dest.AssignedTo, src => src.MapFrom(x => x.AssignedTo))
             .ForMember(dest => dest.MyTaskId, src => src.MapFrom(x => x.Id))
             .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
             .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
             .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
             .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
              .ForMember(dest => dest.TaskRefCode, opt => opt.MapFrom(src => src.DraftId))
              .ForMember(dest => dest.TaskType, opt => opt.MapFrom(src => src.DraftType ))
              .ForMember(dest => dest.Description, opt => opt.ResolveUsing<Automapper.Resolver.TechnicalSpecialist.MyTaskResolver,Evolution.Draft.Domain.Models.Draft> (src=> src))  //D-670 
             //.ForMember(dest => dest.Description, opt => opt.MapFrom(src => string.Format("{0}({1})", ((DraftType)Enum.Parse(typeof(DraftType), src.DraftType)).DisplayName(), src.DraftId)))  //D-670 
             .ForMember(dest => dest.CompanyCode, opt => opt.MapFrom(src => src.CompanyCode)) //D363 CR Change
             .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))//D363 CR Change
             .ForAllOtherMembers(src => src.Ignore());
            #endregion

        }
    }
}
