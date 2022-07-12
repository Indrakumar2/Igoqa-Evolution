using AutoMapper;
using Evolution.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.ResourceSearch.Domain.Models.ResourceSearch;

namespace Evolution.ResourceSearch.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {

            #region ResourceSearchTechspecInfo

            CreateMap<DbModel.TechnicalSpecialist, DomainModel.ResourceSearchTechSpecInfo>()
                    .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.Pin))
                    .ForMember(dest => dest.FirstName, source => source.MapFrom(x => x.FirstName))
                    .ForMember(dest => dest.LastName, source => source.MapFrom(x => x.LastName))
                    .ForMember(dest => dest.EmploymentType, source => source.MapFrom(x => x.EmploymentType.Name))
                    .ForMember(dest => dest.MobileNumber, source => source.ResolveUsing<Automapper.Resolver.ResourceSearch.TechSpecMobileNumberResolver, ICollection<DbModel.TechnicalSpecialistContact>>("TechnicalSpecialistContact"))
                    .ForMember(dest => dest.Email, source => source.ResolveUsing<Automapper.Resolver.ResourceSearch.TechSpecEmailResolver, ICollection<DbModel.TechnicalSpecialistContact>>("TechnicalSpecialistContact"))
                    .ForMember(dest => dest.ProfileStatus, source => source.MapFrom(x => x.ProfileStatus.Name))
                    .ForMember(dest => dest.SubDivision, source => source.MapFrom(x => x.SubDivision.Name))
                    //.ForMember(dest => dest.TsCertifications, source => source.MapFrom(x => x.TechnicalSpecialistCertificationAndTraining))
                    //.ForMember(dest => dest.TsCommodityEquipmentKnowledgeInfos, source => source.MapFrom(x => x.TechnicalSpecialistCommodityEquipmentKnowledge))
                    //.ForMember(dest => dest.TsLanguageCapabilityInfos, source => source.MapFrom(x => x.TechnicalSpecialistLanguageCapability))
                    .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Resource Search service
            CreateMap<DbModel.ResourceSearch, DomainModel.ResourceSearch>()
                    .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                    .ForMember(dest => dest.SearchAction, source => source.MapFrom(x => x.ActionStatus))
                    .ForMember(dest => dest.SearchType, source => source.MapFrom(x => x.SearchType)) 
                    .ForMember(dest => dest.DispositionType, source => source.MapFrom(x => x.DispositionType))
                    .ForMember(dest => dest.AssignedBy, source => source.MapFrom(x => x.AssignedBy))
                    .ForMember(dest => dest.AssignedTo, source => source.MapFrom(x => x.AssignedTo))
                    .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
                    .ForMember(dest => dest.CustomerCode, source => source.MapFrom(x => x.Customer.Code))
                    .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
                    .ForMember(dest => dest.CompanyCode, source => source.MapFrom(x => x.Company.Code))
                    .ForMember(dest => dest.CategoryName, source => source.MapFrom(x => x.Category.Name))
                    .ForMember(dest => dest.SubCategoryName, source => source.MapFrom(x => x.SubCategory.TaxonomySubCategoryName))
                    .ForMember(dest => dest.ServiceName, source => source.MapFrom(x => x.Service.TaxonomyServiceName))
                    .ForMember(dest => dest.AssignedToOmLognName, source => source.MapFrom(x => x.AssignedToOm))
                    .ForAllOtherMembers(x => x.Ignore());


            CreateMap<DomainModel.ResourceSearch, DbModel.ResourceSearch>()
                  .ForMember(dest => dest.Id, source => source.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
                  .ForMember(dest => dest.ActionStatus, source => source.MapFrom(x => x.SearchAction))
                  .ForMember(dest => dest.SearchType, source => source.MapFrom(x => x.SearchType)) 
                  .ForMember(dest => dest.DispositionType, source => source.MapFrom(x => x.DispositionType))
                  .ForMember(dest => dest.AssignedBy, source => source.MapFrom(x => x.AssignedBy))
                  .ForMember(dest => dest.AssignedTo, source => source.MapFrom(x => x.AssignedTo))
                  .ForMember(dest => dest.AssignedToOm, source => source.MapFrom(x => x.AssignedToOmLognName))
                  .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
                  .ForMember(dest => dest.SerilizableObject, source => source.ResolveUsing<Automapper.Resolver.ResourceSearch.SerilizableObjectResolver, DomainModel.ResourceSearchParameter>("SearchParameter"))
                  .ForMember(dest => dest.SerilizationType, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("serializationType") ? (ctx.Options.Items["serializationType"]).ToString() : string.Empty))
                  .ForMember(dest => dest.CustomerId, source => source.ResolveUsing<Automapper.Resolver.ResourceSearch.CustomerIdResolver, string>("CustomerCode"))
                  .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
                  .ForMember(dest => dest.CompanyId, source => source.ResolveUsing<Automapper.Resolver.ResourceSearch.CompanyIdResolver, string>("CompanyCode"))
                  .ForMember(dest => dest.CategoryId, source => source.ResolveUsing<Automapper.Resolver.ResourceSearch.CategoryIdResolver, string>("CategoryName"))
                  .ForMember(dest => dest.SubCategoryId, source => source.ResolveUsing<Automapper.Resolver.ResourceSearch.SubCategoryIdResolver, string>("SubCategoryName"))
                  .ForMember(dest => dest.ServiceId, source => source.ResolveUsing<Automapper.Resolver.ResourceSearch.ServiceIdResolver, string>("ServiceName"))
                  .ForMember(dest => dest.CreatedOn, source => source.MapFrom(x => x.CreatedOn))
                  .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                  .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                  .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                  .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Base Resource Search service

            CreateMap<DbModel.ResourceSearch, DomainModel.BaseResourceSearch>()
                    .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                    .ForMember(dest => dest.SearchAction, source => source.MapFrom(x => x.ActionStatus))
                    .ForMember(dest => dest.SearchType, source => source.MapFrom(x => x.SearchType)) 
                    .ForMember(dest => dest.DispositionType, source => source.MapFrom(x => x.DispositionType))
                    .ForMember(dest => dest.AssignedBy, source => source.MapFrom(x => x.AssignedBy))
                    .ForMember(dest => dest.AssignedTo, source => source.MapFrom(x => x.AssignedTo))
                    .ForMember(dest => dest.AssignedToOmLognName, source => source.MapFrom(x => x.AssignedToOm))
                    .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
                    .ForMember(dest => dest.CustomerCode, source => source.MapFrom(x => x.Customer.Code))
                    .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
                    .ForMember(dest => dest.CompanyCode, source => source.MapFrom(x => x.Company.Code))
                    .ForMember(dest => dest.CategoryName, source => source.MapFrom(x => x.Category.Name))
                    .ForMember(dest => dest.SubCategoryName, source => source.MapFrom(x => x.SubCategory.TaxonomySubCategoryName))
                    .ForMember(dest => dest.ServiceName, source => source.MapFrom(x => x.Service.TaxonomyServiceName))
                    .ForMember(dest => dest.CreatedOn, source => source.MapFrom(x => x.CreatedOn))
                    .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModel.BaseResourceSearch, DbModel.ResourceSearch>()
                 .ForMember(dest => dest.Id, source => source.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
                 .ForMember(dest => dest.ActionStatus, source => source.MapFrom(x => x.SearchAction))
                 .ForMember(dest => dest.SearchType, source => source.MapFrom(x => x.SearchType)) 
                 .ForMember(dest => dest.DispositionType, source => source.MapFrom(x => x.DispositionType))
                 .ForMember(dest => dest.AssignedBy, source => source.MapFrom(x => x.AssignedBy))
                 .ForMember(dest => dest.AssignedTo, source => source.MapFrom(x => x.AssignedTo))
                 .ForMember(dest => dest.AssignedToOm, source => source.MapFrom(x => x.AssignedToOmLognName))
                 .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
                 .ForMember(dest => dest.CustomerId, source => source.ResolveUsing<Automapper.Resolver.ResourceSearch.CustomerIdResolver, string>("CustomerCode"))
                 .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
                 .ForMember(dest => dest.CompanyId, source => source.ResolveUsing<Automapper.Resolver.ResourceSearch.CompanyIdResolver, string>("CompanyCode"))
                 .ForMember(dest => dest.CategoryId, source => source.ResolveUsing<Automapper.Resolver.ResourceSearch.CategoryIdResolver, string>("CategoryName"))
                 .ForMember(dest => dest.SubCategoryId, source => source.ResolveUsing<Automapper.Resolver.ResourceSearch.SubCategoryIdResolver, string>("SubCategoryName"))
                 .ForMember(dest => dest.ServiceId, source => source.ResolveUsing<Automapper.Resolver.ResourceSearch.ServiceIdResolver, string>("ServiceName"))
                 .ForMember(dest => dest.CreatedOn, source => source.MapFrom(x => x.CreatedOn))
                 .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                 .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                 .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                 .ForAllOtherMembers(x => x.Ignore());


            CreateMap<DomainModel.ResourceSearch, DomainModel.BaseResourceSearch>()
                  .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                  .ForMember(dest => dest.SearchAction, source => source.MapFrom(x => x.SearchAction))
                  .ForMember(dest => dest.SearchType, source => source.MapFrom(x => x.SearchType))
                  .ForMember(dest => dest.Description, source => source.MapFrom(x => x.Description))
                  .ForMember(dest => dest.DispositionType, source => source.MapFrom(x => x.DispositionType))
                  .ForMember(dest => dest.AssignedBy, source => source.MapFrom(x => x.AssignedBy))
                  .ForMember(dest => dest.AssignedTo, source => source.MapFrom(x => x.AssignedTo))
                  .ForMember(dest => dest.AssignedToOmLognName, source => source.MapFrom(x => x.AssignedToOmLognName))
                  .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
                  .ForMember(dest => dest.CustomerCode, source => source.MapFrom(x => x.CustomerCode))
                  .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
                  .ForMember(dest => dest.CompanyCode, source => source.MapFrom(x => x.CompanyCode))
                  .ForMember(dest => dest.CategoryName, source => source.MapFrom(x => x.CategoryName))
                  .ForMember(dest => dest.SubCategoryName, source => source.MapFrom(x => x.SubCategoryName))
                  .ForMember(dest => dest.ServiceName, source => source.MapFrom(x => x.ServiceName))
                  .ForMember(dest => dest.CreatedOn, source => source.MapFrom(x => x.CreatedOn))
                  .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Resource Search Result

            CreateMap<DbModel.ResourceSearch, DomainModel.ResourceSearchResult>()
                    .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                    .ForMember(dest => dest.SearchAction, source => source.MapFrom(x => x.ActionStatus))
                    .ForMember(dest => dest.SearchType, source => source.MapFrom(x => x.SearchType)) 
                    .ForMember(dest => dest.DispositionType, source => source.MapFrom(x => x.DispositionType))
                    .ForMember(dest => dest.AssignedBy, source => source.MapFrom(x => x.AssignedBy))
                    .ForMember(dest => dest.AssignedTo, source => source.MapFrom(x => x.AssignedTo))
                    .ForMember(dest => dest.AssignedToOmLognName, source => source.MapFrom(x => x.AssignedToOm))
                    .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
                    .ForMember(dest => dest.CreatedOn, source => source.MapFrom(x => x.CreatedOn))
                    .ForMember(dest => dest.CustomerCode, source => source.MapFrom(x => x.Customer.Code))
                    .ForMember(dest => dest.CompanyCode, source => source.MapFrom(x => x.Company.Code))
                    .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
                    .ForMember(dest => dest.CategoryName, source => source.MapFrom(x => x.Category.Name))
                    .ForMember(dest => dest.SubCategoryName, source => source.MapFrom(x => x.SubCategory.TaxonomySubCategoryName))
                    .ForMember(dest => dest.ServiceName, source => source.MapFrom(x => x.Service.TaxonomyServiceName))
                    .ForMember(dest => dest.SearchParameter, source => source.MapFrom(x => x.SerilizableObject))
                    .ForMember(dest => dest.OverridenPreferredResources, source => source.MapFrom(x => x.OverrideResource))
                    .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount)) 
                    .ForAllOtherMembers(x => x.Ignore());


            #endregion
             
            #region ARS search assignment info

            CreateMap<DbModel.Assignment, DomainModel.ResourceSearch>()
                  .ForMember(dest => dest.SearchType, source => source.MapFrom(x => ResourceSearchType.ARS.ToString()))
                  .ForMember(dest => dest.CategoryName, source => source.MapFrom(x =>x.AssignmentTaxonomy.FirstOrDefault().TaxonomyService.TaxonomySubCategory.TaxonomyCategory.Name ))
                  .ForMember(dest => dest.SubCategoryName, source => source.MapFrom(x => x.AssignmentTaxonomy.FirstOrDefault().TaxonomyService.TaxonomySubCategory.TaxonomySubCategoryName))
                  .ForMember(dest => dest.ServiceName, source => source.MapFrom(x => x.AssignmentTaxonomy.FirstOrDefault().TaxonomyService.TaxonomyServiceName))
                  .ForMember(dest => dest.CompanyCode, source => source.MapFrom(x => x.OperatingCompany.Code))
                  .ForMember(dest => dest.CustomerCode, source => source.MapFrom(x => x.Project.Contract.Customer.Code))
                  .ForMember(dest => dest.IsTechSpecFromAssignmentTaxonomy, src => src.MapFrom(x => x.AssignmentTechnicalSpecialist==null ? false: x.AssignmentTechnicalSpecialist.SelectMany(x1=>x1.TechnicalSpecialist.TechnicalSpecialistTaxonomy) == null
                                                                                               ? false
                                                                                               : x.AssignmentTechnicalSpecialist.SelectMany(x1 => x1.TechnicalSpecialist.TechnicalSpecialistTaxonomy).Where(x1 => x1.TaxonomyServicesId == x.AssignmentTaxonomy.FirstOrDefault().TaxonomyServiceId).ToList() != null
                                                                                                    ? true : false))

                  .ForMember(dest => dest.SearchParameter, source => source.MapFrom(x => x))
                  .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.Assignment, DomainModel.ResourceSearchParameter>()
                   .ForMember(dest => dest.CustomerName, source => source.MapFrom(x => x.Project.Contract.Customer.Name))
                   .ForMember(dest => dest.CustomerCode, source => source.MapFrom(x => x.Project.Contract.Customer.Code))
                   .ForMember(dest => dest.ContractNumber, source => source.MapFrom(x => x.Project.Contract.ContractNumber))
                   .ForMember(dest => dest.ProjectName, source => source.MapFrom(x => x.Project.CustomerProjectName))
                   .ForMember(dest => dest.ProjectNumber, source => source.MapFrom(x => x.Project.ProjectNumber))
                   .ForMember(dest => dest.AssignmentNumber, source => source.MapFrom(x => x.AssignmentNumber))
                   .ForMember(dest => dest.AssignmentType, source => source.MapFrom(x => x.AssignmentType.Trim()))
                   .ForMember(dest => dest.AssignmentStatus, source => source.MapFrom(x => x.AssignmentStatus.Trim()))
                   .ForMember(dest => dest.WorkFlowType, source => source.MapFrom(x => x.Project.WorkFlowType.Trim()))
                   .ForMember(dest => dest.CHCompanyCode, source => source.MapFrom(x => x.ContractCompany.Code))
                   .ForMember(dest => dest.CHCompanyName, source => source.MapFrom(x => x.ContractCompany.Name))
                   .ForMember(dest => dest.CHCoordinatorLogOnName, source => source.MapFrom(x => x.ContractCompanyCoordinator.SamaccountName))
                   .ForMember(dest => dest.OPCompanyCode, source => source.MapFrom(x => x.OperatingCompany.Code))
                   .ForMember(dest => dest.OPCompanyName, source => source.MapFrom(x => x.OperatingCompany.Name))
                   .ForMember(dest => dest.OPCoordinatorLogOnName, source => source.MapFrom(x => x.OperatingCompanyCoordinator.SamaccountName))
                   .ForMember(dest => dest.Supplier, source => source.MapFrom(x => x.SupplierPurchaseOrder.Supplier.SupplierName))
                   .ForMember(dest => dest.SupplierLocation, source => source.MapFrom(x => x.SupplierPurchaseOrder.Supplier.Address))
                   .ForMember(dest => dest.SupplierPurchaseOrder, source => source.MapFrom(x => x.SupplierPurchaseOrder.SupplierPonumber))
                   .ForMember(dest => dest.MaterialDescription, source => source.MapFrom(x => x.SupplierPurchaseOrder.MaterialDescription))
                   .ForMember(dest => dest.SubSupplierInfos, source => source.MapFrom(x => x.AssignmentSubSupplier))
                  //.ForMember(dest => dest.AssignedResourceInfos, source => source.MapFrom(x => x.AssignmentSubSupplier))
                  .ForMember(dest => dest.CategoryName, source => source.MapFrom(x => x.AssignmentTaxonomy.FirstOrDefault().TaxonomyService.TaxonomySubCategory.TaxonomyCategory.Name))
                  .ForMember(dest => dest.SubCategoryName, source => source.MapFrom(x => x.AssignmentTaxonomy.FirstOrDefault().TaxonomyService.TaxonomySubCategory.TaxonomySubCategoryName))
                  .ForMember(dest => dest.ServiceName, source => source.MapFrom(x => x.AssignmentTaxonomy.FirstOrDefault().TaxonomyService.TaxonomyServiceName))
                   //.AfterMap((src, dest) =>
                   // {
                   //     dest?.AssignedResourceInfos.ToList()?.ForEach(x =>
                   //     {
                   //         x.TaxonomyServiceName = src?.AssignmentTaxonomy?.FirstOrDefault()?.TaxonomyService?.TaxonomyServiceName;
                   //         x.AssignedTechSpec?.ToList()?.ForEach(val =>
                   //         {
                   //             val.IsTechSpecFromAssignmentTaxonomy = (src?.AssignmentTechnicalSpecialist?
                   //                                            .SelectMany(x1 =>x1.TechnicalSpecialist?.TechnicalSpecialistTaxonomy)?
                   //                                            .Where(x2 => x2.TechnicalSpecialist?.Pin == val.Epin &&  x2.TaxonomyServicesId == src?.AssignmentTaxonomy?.FirstOrDefault()?.TaxonomyService?.Id)?
                   //                                            .ToList()?.Count > 0) ? true : false;
                   //         });
                   //     });

                   //     if (src.Visit?.Count > 0)
                   //     {
                   //         var firstVisit = src.Visit?.FirstOrDefault(x => x.FromDate == src.Visit.Min(x1 => x1.FromDate));
                   //         if (firstVisit != null)
                   //         {
                   //             dest.FirstVisitFromDate = firstVisit.FromDate;
                   //             dest.FirstVisitToDate = firstVisit.ToDate;
                   //             dest.FirstVisitStatus = firstVisit.VisitStatus?.Trim();
                   //         }
                   //     }
                   //     else if (src.Timesheet?.Count > 0)
                   //     {
                   //         var firstTimeSheet = src.Timesheet?.FirstOrDefault(x => x.FromDate == src.Timesheet.Min(x1 => x1.FromDate));
                   //         if (firstTimeSheet != null)
                   //         {
                   //             dest.FirstVisitFromDate = firstTimeSheet.FromDate;
                   //             dest.FirstVisitToDate = firstTimeSheet.ToDate;
                   //             dest.FirstVisitStatus = firstTimeSheet.TimesheetStatus?.Trim();
                   //         }
                   //     }
                   // })
                  .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.AssignmentSubSupplier, DomainModel.ResourceSearchSubSupplierInfo>()
                     .ForMember(dest => dest.SubSupplier, source => source.MapFrom(x => x.Supplier.SupplierName))//MS-TS Link CR
                   .ForMember(dest => dest.SubSupplierLocation, source => source.MapFrom(x => x.Supplier.Address))//MS-TS Link CR

                   .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.AssignmentSubSupplier, DomainModel.AssignedResourceInfo>()
              .ForMember(dest => dest.AssignedTechSpec, source => source.ResolveUsing<Automapper.Resolver.ResourceSearch.AssignedResourceAssignedTechSpecResolver, ICollection<DbModel.AssignmentSubSupplierTechnicalSpecialist>>("AssignmentSubSupplierTechnicalSpecialist"))
              .ForMember(dest => dest.SupplierName, source => source.MapFrom(x => x.Supplier.SupplierName))//MS-TS Link CR

              .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region ResourceSearchNote

            CreateMap<DbModel.ResourceSearchNote, DomainModel.ResourceSearchNote>()
               .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
               .ForMember(dest => dest.ResourceSearchId, source => source.MapFrom(x => x.ResourceSearchId))
               .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
               .ForMember(dest => dest.CreatedOn, source => source.MapFrom(x => x.CreatedDate))
               .ForMember(dest => dest.Notes, source => source.MapFrom(x => x.Note))
               .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
               .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
               .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModel.ResourceSearchNote, DbModel.ResourceSearchNote>()
               .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
               .ForMember(dest => dest.ResourceSearchId, source => source.MapFrom(x => x.ResourceSearchId))
               .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
               .ForMember(dest => dest.CreatedDate, source => source.MapFrom(x => x.CreatedOn))
               .ForMember(dest => dest.Note, source => source.MapFrom(x => x.Notes))
               .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
               .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
               .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Overriden Preferred Resource

            CreateMap<DbModel.OverrideResource, DomainModel.OverridenPreferredResource>()
               .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
               .ForMember(dest => dest.ResourceSearchId, source => source.MapFrom(x => x.ResourceSearchId))
               .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
               .ForMember(dest => dest.CreatedDate, source => source.MapFrom(x => x.CreatedDate))
               .ForMember(dest => dest.IsApproved, source => source.MapFrom(x => x.IsApproved))
               .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
               .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
               .ForMember(dest => dest.TechSpecialist, source => source.MapFrom(x => x.TechnicalSpecialist))
               .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.TechnicalSpecialist, DomainModel.BaseResourceSearchTechSpecInfo>()
           .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.Pin))
           .ForMember(dest => dest.LastName, source => source.MapFrom(x => x.LastName))
           .ForMember(dest => dest.FirstName, source => source.MapFrom(x => x.FirstName))
           .ForMember(dest => dest.TechSpecGeoLocation, source => source.Ignore()) 
           .ForAllOtherMembers(x => x.Ignore());


            CreateMap<DomainModel.OverridenPreferredResource, DbModel.OverrideResource>()
             .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
             .ForMember(dest => dest.ResourceSearchId, source => source.MapFrom(x => x.ResourceSearchId))
             .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
             .ForMember(dest => dest.CreatedDate, source => source.MapFrom(x => x.CreatedDate))
             .ForMember(dest => dest.IsApproved, source => source.MapFrom(x => x.IsApproved))
             .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
             .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
             .ForMember(dest => dest.TechnicalSpecialistId, source => source.ResolveUsing<Automapper.Resolver.ResourceSearch.TechSpecIdResolver, DomainModel.BaseResourceSearchTechSpecInfo>("TechSpecialist"))
             .ForAllOtherMembers(x => x.Ignore());

            #endregion
        }
    }
}
