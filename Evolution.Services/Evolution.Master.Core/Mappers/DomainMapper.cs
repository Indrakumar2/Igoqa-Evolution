using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Master.Domain.Models;

namespace Evolution.Master.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            //CreateMap<string, bool>().ConvertUsing(str => str.ToUpper() == "YES");

            CreateMap<DomainModel.MasterData, DbModel.Data>()
                    .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isMasterId") ? (Convert.ToBoolean(context.Options.Items["isMasterId"]) ? src.Id : null) : src.Id)))
                    .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
                   .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                   .ForMember(dest => dest.DisplayName, src => src.MapFrom(x => x.DisplayName))
                   .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Description))
                   .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
                   .ForMember(dest => dest.IsAlchiddenForNewFacility, src => src.MapFrom(x => x.IsAlchiddenForNewFacility))
                   .ForMember(dest => dest.IsEmployed, src => src.MapFrom(x => x.IsEmployed))
                   .ForMember(dest => dest.Type, src => src.MapFrom(x => x.Type))
                   .ForMember(dest => dest.InterCompanyType, src => src.MapFrom(x => x.InterCompanyType))
                   .ForMember(dest => dest.ChargeReference, src => src.MapFrom(x => x.ChargeReference))
                   .ForMember(dest => dest.PayReference, src => src.MapFrom(x => x.PayReference))
                   .ForMember(dest => dest.InvoiceType, src => src.MapFrom(x => x.InvoiceType))
                   .ForMember(dest => dest.Precedence, src => src.MapFrom(x => x.Precedence))
                   .ForMember(dest => dest.Hour, src => src.MapFrom(x => x.Hour))
                   .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                   .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                   .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                   .ForMember(dest => dest.IsArs, src => src.MapFrom(x => x.IsARS))
                   //.ForMember(dest => dest.MasterDataTypeId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("MasterType") ? ((ICollection<DbModel.Data>)context.Options.Items["MasterType"])?.FirstOrDefault(x => x.MasterDataType.Name == src.MasterType)?.Id : null))
                   .ForMember(dest => dest.MasterDataTypeId, src => src.MapFrom(x => x.MasterDataTypeId))
                   .ForAllOtherMembers(src => src.Ignore());



            CreateMap<DbModel.Data, DomainModel.MasterData>()
                    .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                   //  .ForMember(dest => dest.MasterType, src => src.MapFrom(x=>x.MasterDataType.Name))
                   .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
                   .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name.Trim()))
                   .ForMember(dest => dest.DisplayName, src => src.MapFrom(x => x.DisplayName))
                   .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Description))
                   .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
                   .ForMember(dest => dest.IsAlchiddenForNewFacility, src => src.MapFrom(x => x.IsAlchiddenForNewFacility))
                   .ForMember(dest => dest.IsEmployed, src => src.MapFrom(x => x.IsEmployed))
                   .ForMember(dest => dest.Type, src => src.MapFrom(x => x.Type))
                   .ForMember(dest => dest.InterCompanyType, src => src.MapFrom(x => x.InterCompanyType))
                   .ForMember(dest => dest.ChargeReference, src => src.MapFrom(x => x.ChargeReference))
                   .ForMember(dest => dest.PayReference, src => src.MapFrom(x => x.PayReference))
                   .ForMember(dest => dest.InvoiceType, src => src.MapFrom(x => x.InvoiceType))
                   .ForMember(dest => dest.Precedence, src => src.MapFrom(x => x.Precedence))
                   .ForMember(dest => dest.Hour, src => src.MapFrom(x => x.Hour))
                   .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                   .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                   .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                   .ForMember(dest => dest.IsARS, src => src.MapFrom(x => x.IsArs))
                   .ForMember(dest => dest.MasterDataTypeId, src => src.MapFrom(x => x.MasterDataTypeId))
                     .ForAllOtherMembers(src => src.Ignore());


            CreateMap<DomainModel.ModuleDocumentType, DomainModel.MasterData>()
                   .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                   .ReverseMap();

            CreateMap<DbModel.ModuleDocumentType, DomainModel.ModuleDocumentType>()
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.DocumentType.Name))
                .ForMember(dest => dest.ModuleName, src => src.MapFrom(x => x.Module.Name))
                .ForMember(dest => dest.IsTSVisible, src => src.MapFrom(x => x.Tsvisible))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ReverseMap()
                .ForAllOtherMembers(src => src.Ignore());
            #region city

            CreateMap<DbModel.City, DomainModel.City>()
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.State, src => src.MapFrom(x => x.County.Name))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ReverseMap()
                .ForAllOtherMembers(src => src.Ignore());




            CreateMap<DomainModel.City, DbModel.City>()
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.CountyId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("IsCountyId") ? ((IList<DbModel.Data>)context.Options.Items["IsCountyId"])?.FirstOrDefault(x => x.Name == src.State)?.Id : null))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForAllOtherMembers(src => src.Ignore());
            #endregion

            #region country
            CreateMap<DbModel.Country, DomainModel.Country>()
            .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
            .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
            .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
            .ForMember(dest => dest.EUVatPrefix, src => src.MapFrom(x => x.Euvatprefix))
            .ForMember(dest => dest.IsEuMember, src => src.MapFrom(x => x.IsEumember))
            .ForMember(dest => dest.IsGCCMember, src => src.MapFrom(x => x.IsGccmember))
            .ForMember(dest => dest.Region, src => src.MapFrom(x => x.Regon.Name))
            .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
            .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
            .ForAllOtherMembers(src => src.Ignore());

            CreateMap<DomainModel.Country, DbModel.Country>()
            .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
            .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isCountryId") ? (Convert.ToBoolean(context.Options.Items["isCountryId"]) ? src.Id : null) : src.Id)))
            .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
            .ForMember(dest => dest.Euvatprefix, src => src.MapFrom(x => x.EUVatPrefix))
            .ForMember(dest => dest.IsEumember, src => src.MapFrom(x => x.IsEuMember))
            .ForMember(dest => dest.IsGccmember, src => src.MapFrom(x => x.IsGCCMember))
            .ForMember(dest => dest.RegonId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("isRegionId") ? ((List<DbModel.Data>)context.Options.Items["isRegionId"])?.FirstOrDefault(x => x.Name == src.Region)?.Id : null))
            .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
            .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
            .ForAllOtherMembers(src => src.Ignore());

            #endregion country

            #region county
            CreateMap<DbModel.County, DomainModel.State>()
           .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
           .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
           .ForMember(dest => dest.Country, src => src.MapFrom(x => x.Country.Name))
           .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
           .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
           .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
           .ForAllOtherMembers(src => src.Ignore());

            CreateMap<DomainModel.State, DbModel.County>()
           .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
           .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isCountyId") ? (Convert.ToBoolean(context.Options.Items["isCountyId"]) ? src.Id : null) : src.Id)))
           .ForMember(dest => dest.CountryId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("isCountry") ? ((List<DbModel.Country>)context.Options.Items["isCountry"])?.FirstOrDefault(x => x.Name == src.Name)?.Id : null))
           .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
           .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
           .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
           .ForAllOtherMembers(src => src.Ignore());
            #endregion county


            CreateMap<DomainModel.BaseMasterModel, DomainModel.MasterData>()
                    .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                    .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                   .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                   .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                   .ForMember(dest => dest.IsActive,src => src.MapFrom(x=>x.IsActive))   //Changes for Defect 112
                   .ReverseMap();

            CreateMap<DomainModel.TaxType, DbModel.Tax>()
                    .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
                    .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                    .ForMember(dest => dest.Rate, src => src.MapFrom(x => x.Rate))
                    .ForMember(dest => dest.TaxType, src => src.MapFrom(x => x.Type))
                    .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive)) 
                    .ReverseMap()
                    .ForAllOtherMembers(src => src.Ignore());

            CreateMap<DomainModel.Currency, DomainModel.MasterData>()
                .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
                .ReverseMap();



            CreateMap<DomainModel.Region, DomainModel.MasterData>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
                .ReverseMap();



            CreateMap<DomainModel.Region, DomainModel.MasterData>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
                .ReverseMap();


            CreateMap<DomainModel.CostOfSale, DomainModel.MasterData>()
               .ForMember(dest => dest.ChargeReference, src => src.MapFrom(x => x.ChargeReference))
               .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
               .ForMember(dest => dest.Type, src => src.MapFrom(x => x.ChargeType))
               .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
               .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
               .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
               .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Description))
               .ForMember(dest => dest.PayReference, src => src.MapFrom(x => x.PayReference)) //Added for D1089 (Ref ALM 22-05-2020 Doc)
               .ReverseMap();

            CreateMap<DomainModel.AssignmentReferenceType, DomainModel.MasterData>()
               .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
               .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
               .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
               .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
               .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
               .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
               .ReverseMap();

            CreateMap<DomainModel.Salutation, DomainModel.MasterData>()
               .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
               .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
               .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
               .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
               .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
               .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
               .ReverseMap();

            CreateMap<DomainModel.TechnicalSpecialistStampCountryCode, DomainModel.MasterData>()
               .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
               .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
               .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
               .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
               .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
               .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
               .ReverseMap();

            CreateMap<DomainModel.Division, DomainModel.MasterData>()
              .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
              .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
              .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
              .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
              .ReverseMap();

            CreateMap<DomainModel.ExportPrefix, DomainModel.MasterData>()
             .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
             .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
             .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
             .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
             .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
             .ReverseMap();

            CreateMap<DomainModel.PayrollType, DomainModel.MasterData>()
              .ForMember(dest => dest.Id,src => src.MapFrom(x => x.Id))
              .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
              .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
              .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
              .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
              .ReverseMap();

            CreateMap<DbModel.EmailPlaceHolder, DomainModel.EmailPlaceholder>()
              .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
              .ForMember(dest => dest.ModuleName, src => src.MapFrom(x => x.ModuleName))
              .ForMember(dest => dest.DisplayName, src => src.MapFrom(x => x.DisplayName))
              .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
              .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
              .ReverseMap();


            CreateMap<DomainModel.CompanyMarginType, DomainModel.MasterData>()
               .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
               .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
               .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
               .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
               .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
               .ReverseMap();

            CreateMap<DomainModel.InvoicePaymentTerms, DomainModel.MasterData>()
              .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
              .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
              .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
              .ReverseMap();

            CreateMap<DomainModel.StandardChargeSchedule, DomainModel.MasterData>()
             .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
             .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
             .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
             .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
             .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
             .ReverseMap();

            CreateMap<DomainModel.Division, DomainModel.MasterData>()
             .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
             .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
             .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
             .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
             .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
             .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
             .ReverseMap();


            CreateMap<DomainModel.IndustrySector, DomainModel.MasterData>()
                 .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
                .ReverseMap();


            CreateMap<DbModel.Data, DomainModel.Logo>()
                .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
                .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                .ReverseMap();


            CreateMap<DomainModel.ManagedServiceType, DomainModel.MasterData>()
              .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
               .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
               .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
               .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
               .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
               .ReverseMap();

            CreateMap<DomainModel.Equipment, DomainModel.MasterData>()
              .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
              .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
              .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
              .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
              .ReverseMap();

            CreateMap<DomainModel.SubDivision, DomainModel.MasterData>()
              .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
              .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
              .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
              .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
              .ReverseMap();

            CreateMap<DomainModel.ProfileAction, DomainModel.MasterData>()
              .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
              .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
              .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
              .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
              .ReverseMap();

            CreateMap<DomainModel.ProfileStatus, DomainModel.MasterData>()
              .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
              .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
              .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
              .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
              .ReverseMap();

            CreateMap<DomainModel.Commodity, DomainModel.MasterData>()
              .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
              .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
              .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
              .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
              .ReverseMap();

            CreateMap<DomainModel.EmploymentType, DomainModel.MasterData>()
              .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
              .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
              .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
              .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
              .ReverseMap();
            CreateMap<DomainModel.CodeStandard, DomainModel.MasterData>()
             .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
             .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
             .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
             .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
             .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
             .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
             .ReverseMap();
            CreateMap<DomainModel.ComputerKnowledge, DomainModel.MasterData>()
            .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
            .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
            .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
            .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
            .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
            .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
            .ReverseMap();
            CreateMap<DomainModel.Certifications, DomainModel.MasterData>()
           .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
           .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
           .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
           .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
           .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
           .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
           .ReverseMap();
            CreateMap<DomainModel.Trainings, DomainModel.MasterData>()
           .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
           .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
           .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
           .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
           .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
           .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
           .ReverseMap();
            CreateMap<DomainModel.TaxonomyCategory, DomainModel.MasterData>()
           .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
           .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
           .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
           .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
           .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
           .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
           .ReverseMap();

            CreateMap<DomainModel.ProjectType, DomainModel.MasterData>()
              .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
              .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
              .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
              .ForMember(dest => dest.IsARS, src => src.MapFrom(x => x.IsARS))
              .ReverseMap();
            CreateMap<DomainModel.ExpenseType, DomainModel.MasterData>()
             .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
             .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
             .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
             .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
             .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
             .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Description))
             .ReverseMap();

            CreateMap<DomainModel.Language, DomainModel.MasterData>()
           .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
           .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
           .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
           .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
           .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
           .ReverseMap();
            CreateMap<DomainModel.Competency, DomainModel.MasterData>()
           .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
           .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
           .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
           .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
           .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
           .ReverseMap();
            CreateMap<DomainModel.InternalTraining, DomainModel.MasterData>()
          .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
          .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
          .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
          .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
          .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
          .ReverseMap();

            CreateMap<DomainModel.AssignmentStatus, DomainModel.MasterData>()

             .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
             .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
             .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
             .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
             .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
             .ReverseMap();

            CreateMap<DomainModel.AssignmentType, DomainModel.MasterData>()

             .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
             .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
             .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
             .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
             .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
             .ReverseMap();

            CreateMap<DomainModel.VisitStatus, DomainModel.MasterData>()

             .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
            .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
             .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
             .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
            .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
            .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
            .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
            .ForMember(dest => dest.Precedence, src => src.MapFrom(x => x.Precedence))
            .ReverseMap();

            CreateMap<DomainModel.UnusedReason, DomainModel.MasterData>()
             .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
            .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
             //.ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
             .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
            //.ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
            //.ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
            //.ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
            .ForMember(dest => dest.Precedence, src => src.MapFrom(x => x.Precedences))
            .ReverseMap();

            CreateMap<DomainModel.ReviewAndModeration, DomainModel.MasterData>()

           .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
          .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
           .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
           .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
          .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
          .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
          .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))

          .ReverseMap();

            CreateMap<DbModel.TaxonomyBusinessUnit, DomainModel.TaxonomyBusinessUnit>()
            .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
            .ForMember(dest => dest.ProjectTypeId, src => src.MapFrom(x => x.ProjectTypeId))
            .ForMember(dest => dest.ProjectType, src => src.MapFrom(x => x.ProjectType.Name))
           .ForMember(dest => dest.CategoryId, src => src.MapFrom(x => x.CategoryId))
            .ForMember(dest => dest.Category, src => src.MapFrom(x => x.Category.Name))
           .ForMember(dest => dest.Type, src => src.MapFrom(x => x.ProjectType.Type))
           .ForMember(dest => dest.InterCompanyType, src => src.MapFrom(x => x.ProjectType.InterCompanyType))
           .ForMember(dest => dest.InvoiceType, src => src.MapFrom(x => x.ProjectType.InvoiceType))
           .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
           .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
           .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
           .ReverseMap().ForAllOtherMembers(src => src.Ignore());

            CreateMap<DomainModel.DispositionType, DomainModel.MasterData>()
              .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
              .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
              .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
              .ForMember(dest => dest.Type, src => src.MapFrom(x => x.Type))
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
              .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
              .ReverseMap();

            CreateMap<DomainModel.LeaveCategoryType, DomainModel.MasterData>()
           .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
           .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
           .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
           .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
           .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
           .ReverseMap();


            CreateMap<DomainModel.Documentation, DomainModel.MasterData>()
           .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
           .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
           .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Description))
           .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
          .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
          .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
           .ReverseMap();

            // CreateMap<DbModel.AuditSearch, DomainModel.AuditSearch>()
            //     .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
            //     .ForMember(dest => dest.ModuleName, src => src.MapFrom(x => x.Module.Name))
            //     .ForMember(dest => dest.SearchParameterName, src => src.MapFrom(x => x.Search.Name))
            //     .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
            //     .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
            //     .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
            //     .ReverseMap()
            //     .ForAllOtherMembers(src => src.Ignore());

            CreateMap<DomainModel.MasterModuleTypes, DomainModel.MasterData>()
            .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
            .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
            .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
            .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
            .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
            .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
            .ReverseMap();
            CreateMap<DomainModel.SupplierPerformanceType, DomainModel.MasterData>()
                 .ForMember(dest => dest.Code, src => src.MapFrom(x => x.Code))
                 .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
                 .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
                 .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                 .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                 .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                 .ReverseMap();


            #region LanguageRefType
            CreateMap<DbModel.LanguageReferenceType, DomainModel.LanguageReferenceType>()
           .ForMember(dest => dest.LanguageReferenceTypeId, src => src.MapFrom(x => x.Id))
           .ForMember(dest => dest.Language, src => src.MapFrom(x => x.Language))
           .ForMember(dest => dest.ReferenceType, src => src.MapFrom(x => x.ReferenceType.Name))
            .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
           .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
           .ForAllOtherMembers(src => src.Ignore());

            CreateMap<DomainModel.LanguageReferenceType, DbModel.LanguageReferenceType>()
            .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isLanguageRefId") ? (Convert.ToBoolean(context.Options.Items["isLanguageRefId"]) ? src.LanguageReferenceTypeId : null) : src.LanguageReferenceTypeId)))
           .ForMember(dest => dest.LanguageId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("isLanguageId") ? ((List<DbModel.Data>)context.Options.Items["isLanguageId"])?.FirstOrDefault(x => x.Name == src.Language)?.Id : null))
           .ForMember(dest => dest.ReferenceTypeId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("isReferenceTypeId") ? ((List<DbModel.Data>)context.Options.Items["isReferenceTypeId"])?.FirstOrDefault(x => x.Name == src.ReferenceType)?.Id : null))
           .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
           .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
           .ForAllOtherMembers(src => src.Ignore());
            #endregion county


            #region CommodityEquipment
            CreateMap<DbModel.CommodityEquipment, DomainModel.CommodityEquipment>()
           .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
           .ForMember(dest => dest.Commodity, src => src.MapFrom(x => x.Commodity.Name))
           .ForMember(dest => dest.Equipment, src => src.MapFrom(x => x.Equipment.Name))
           .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Equipment.Name))
           .ForMember(dest => dest.CommodityCode, src => src.MapFrom(x => x.CommodityId))
           .ForMember(dest => dest.EquipmentCode, src => src.MapFrom(x => x.EquipmentId))
           .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
           .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
           .ReverseMap()
           .ForAllOtherMembers(src => src.Ignore());
            #endregion county
        }



    }
}
