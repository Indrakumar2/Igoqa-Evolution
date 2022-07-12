using AutoMapper;
using Evolution.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Reports.Domain.Models.Reports;

namespace Evolution.Reports.Core.Mappers
{
   public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            #region CustomerApproval

            CreateMap<DbModel.TechnicalSpecialistCustomerApproval, DomainModel.CustomerApproval>()
                     .ForMember(dest => dest.CustomerApprovalName, src => src.MapFrom(x => x.Customer.Name))
                     .ForMember(dest => dest.EpinId, src => src.MapFrom(x => x.TechnicalSpecialistId))
                     .ForMember(dest => dest.CustomerSapID, src => src.MapFrom(x => x.CustomerSapId))
                     .ForMember(dest => dest.LastName, src => src.MapFrom(x => x.TechnicalSpecialist.LastName))
                     .ForMember(dest => dest.FirstName, src => src.MapFrom(x => x.TechnicalSpecialist.FirstName))
                     .ForMember(dest => dest.EffectiveFrom, src => src.MapFrom(x => x.DateFrom))
                     .ForMember(dest => dest.EffectiveTo, src => src.MapFrom(x => x.DateTo))                                                                                                  
                     .ForMember(dest => dest.ProfileStatus, src => src.MapFrom(x => x.TechnicalSpecialist.ProfileStatus.Name))
                     .ForMember(dest => dest.EmploymentType, src => src.MapFrom(x => x.TechnicalSpecialist.EmploymentType.Name))
                     .ForMember(dest => dest.Company, src => src.MapFrom(x => x.TechnicalSpecialist.Company.Name))
                     .ForMember(dest => dest.Country, src => src.MapFrom(x => x.TechnicalSpecialist.TechnicalSpecialistContact.Select(X1=>X1.Country.Name).FirstOrDefault()))
                     .ForMember(dest => dest.County, src => src.MapFrom(x => x.TechnicalSpecialist.TechnicalSpecialistContact.Select(X1 => X1.County.Name).FirstOrDefault()))
                     .ForMember(dest => dest.City, src => src.MapFrom(x => x.TechnicalSpecialist.TechnicalSpecialistContact.Select(X1 => X1.City.Name).FirstOrDefault()))
                     .ForMember(dest => dest.ZipCode, src => src.MapFrom(x => x.TechnicalSpecialist.TechnicalSpecialistContact.Select(X1=>X1.PostalCode).FirstOrDefault()))
                     .ForMember(dest => dest.Comments, src => src.MapFrom(x => x.Comments))
                     .ForMember(dest => dest.SubDivision, src => src.MapFrom(x => x.TechnicalSpecialist.SubDivision.Name))
                     .ReverseMap()
                     .ForAllOtherMembers(src => src.Ignore());


            CreateMap<DbModel.TechnicalSpecialistCustomerApproval, DomainModel.CustomerApprovalSearch>()

                    .ForMember(dest => dest.FirstName, src => src.MapFrom(x => x.TechnicalSpecialist.FirstName))
                    .ReverseMap()
                    .ForAllOtherMembers(src => src.Ignore());



            #endregion
            #region WonLost
            CreateMap<DbModel.ResourceSearch, DomainModel.WonLost>()

                   .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                   .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => x.CreatedBy))
                   .ForMember(dest => dest.CreatedOn, src => src.MapFrom(x => x.CreatedOn.Date))
                   .ForMember(dest => dest.SearchParameter, src => src.MapFrom(x => x.SerilizableObject))
                   .ForMember(dest => dest.CustomerCode, src => src.MapFrom(x => x.Customer.Code))   
                   .ForMember(dest => dest.CategoryName, src => src.MapFrom(x => x.Category.Name))
                   .ForMember(dest => dest.SubCategoryName, src => src.MapFrom(x => x.SubCategory.TaxonomySubCategoryName))
                   .ForMember(dest => dest.ServiceName, source => source.MapFrom(x => x.Service.TaxonomyServiceName))
                   .ForMember(dest => dest.FromDate, src => src.MapFrom(x => x.CreatedOn.Date))
                   .ForMember(dest => dest.ToDate, source => source.MapFrom(x => x.CreatedOn.Date))
                   .ForMember(dest => dest.CompanyCode, src => src.MapFrom(x => x.Company.Code))
                   .ForMember(dest => dest.AssginedTo, source => source.MapFrom(x => x.AssignedTo))
                   .ForMember(dest => dest.Action, src => src.MapFrom(x => x.ActionStatus))
                   .ForMember(dest => dest.SearchType, src => src.MapFrom(x => x.SearchType))
                   /*.ForMember(dest => dest.Description, src => src.MapFrom(x => x.ResourceSearchNote.OrderByDescending(x1=>x1.Id).FirstOrDefault().Note))*/
                   .ReverseMap()
                   .ForAllOtherMembers(src => src.Ignore());



            CreateMap<DbModel.ResourceSearch, DomainModel.WonLostSearch>()

                   .ForMember(dest => dest.ToDate, src => src.MapFrom(x => x.CreatedOn.Date))
                   .ForMember(dest => dest.FromDate, src => src.MapFrom(x => x.CreatedOn.Date))
                   .ForMember(dest => dest.CompanyCode, src => src.MapFrom(x => x.Company.Code))
                   .ForMember(dest => dest.AssginedTo, source => source.MapFrom(x => x.AssignedTo))
                   .ForMember(dest => dest.Action, src => src.MapFrom(x => x.ActionStatus))
                   .ForMember(dest => dest.SearchType, src => src.MapFrom(x => x.SearchType))
                   .ReverseMap()
                   .ForAllOtherMembers(src => src.Ignore());



            #endregion

        }
    }
}
