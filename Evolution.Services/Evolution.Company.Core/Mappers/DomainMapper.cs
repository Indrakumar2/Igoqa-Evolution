using AutoMapper;
using Evolution.Automapper.Resolver.MongoSearch;
using Evolution.Common.Extensions;
using Evolution.Common.Mappers.Resolvers;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Documents;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.Company.Domain.Models.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Company.Domain.Models.Companies;
using Enum = Evolution.Company.Domain.Enums;

namespace Evolution.Company.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            #region Company 
            CreateMap<DbModel.Company, DomainModel.Company>()
                       .ForMember(source => source.CompanyCode, dest => dest.MapFrom(x => x.Code))
                       .ForMember(source => source.CompanyName, dest => dest.MapFrom(x => x.Name))
                       .ForMember(source => source.InvoiceName, dest => dest.MapFrom(x => x.InvoiceCompanyName))
                       .ForMember(source => source.Currency, dest => dest.MapFrom(x => x.NativeCurrency))
                       .ForMember(source => source.SalesTaxDescription, dest => dest.MapFrom(x => x.SalesTaxDescription))
                       .ForMember(source => source.WithholdingTaxDescription, dest => dest.MapFrom(x => x.WithholdingTaxDescription))
                       .ForMember(source => source.InterCompanyExpenseAccRef, dest => dest.MapFrom(x => x.InterCompanyExpenseAccRef))
                       .ForMember(source => source.InterCompanyRateAccRef, dest => dest.MapFrom(x => x.InterCompanyRateAccRef))
                       .ForMember(source => source.InterCompanyRoyaltyAccRef, dest => dest.MapFrom(x => x.InterCompanyRoyaltyAccRef))
                       .ForMember(source => source.CompanyMiiwaid, dest => dest.MapFrom(x => x.CompanyMiiwaid))
                       .ForMember(source => source.OperatingCountry, dest => dest.MapFrom(x => x.OperatingCountry))
                       .ForMember(source => source.OperatingCountryName ,dest =>dest.MapFrom(x=>x.OperatingCountryNavigation !=null ? x.OperatingCountryNavigation.Name :null)) //IGO QC D-906
                       .ForMember(source => source.IsActive, dest => dest.MapFrom(x => x.IsActive))
                       .ForMember(source => source.IsUseIctms, dest => dest.MapFrom(x => x.IsUseIctms))
                       .ForMember(source => source.IsFullUse, dest => dest.MapFrom(x => x.IsFullUse))
                       .ForMember(source => source.Region, dest => dest.MapFrom(x => x.Region))
                       .ForMember(source => source.IsCOSEmailOverrideAllow, dest => dest.MapFrom(x => x.IsCostOfSalesEmailOverrideAllow))
                       .ForMember(source => source.AvgTSHourlyCost, dest => dest.MapFrom(x => x.AverageTshourlycost))
                       .ForMember(source => source.VatTaxRegNo, dest => dest.MapFrom(x => x.VatTaxRegistrationNo))
                       .ForMember(source => source.EUVatPrefix, dest => dest.MapFrom(x => x.Euvatprefix))
                       .ForMember(source => source.IARegion, dest => dest.MapFrom(x => x.Iaregion))
                       .ForMember(source => source.CognosNumber, dest => dest.MapFrom(x => x.CognosNumber))
                       .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
                       .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
                       .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
                       .ForMember(source => source.LogoText, dest => dest.MapFrom(x => x.Logo != null ? x.Logo.Name : null))
                       .ForMember(source => source.ResourceOutsideDistance, dest => dest.MapFrom(x => x.ResourceOutsideDistance))
                       .ForMember(source => source.VATRegulationTextWithinEC, dest => dest.MapFrom(x => x.VatregTextWithinEc))
                       .ForMember(source => source.VATRegulationTextOutsideEC, dest => dest.MapFrom(x => x.VatregTextOutsideEc))
                       .ForMember(source => source.CompanyMiiwaref, dest => dest.MapFrom(x => x.CompanyMiiwaref))
                       .ForMember(source => source.GfsBu, dest => dest.MapFrom(x => x.GfsBu))
                       .ForMember(source => source.GfsCoa,dest => dest.MapFrom(x => x.GfsCoa))
                      //.ReverseMap()
                      .ForAllOtherMembers(x => x.Ignore());
            CreateMap<DomainModel.Company, DbModel.Company>()
                      .ForMember(source => source.Code, dest => dest.MapFrom(x => x.CompanyCode))
                      .ForMember(source => source.Name, dest => dest.MapFrom(x => x.CompanyName))
                      .ForMember(source => source.InvoiceCompanyName, dest => dest.MapFrom(x => x.InvoiceName))
                      .ForMember(source => source.NativeCurrency, dest => dest.MapFrom(x => x.Currency))
                      .ForMember(source => source.SalesTaxDescription, dest => dest.MapFrom(x => x.SalesTaxDescription))
                      .ForMember(source => source.WithholdingTaxDescription, dest => dest.MapFrom(x => x.WithholdingTaxDescription))
                      .ForMember(source => source.InterCompanyExpenseAccRef, dest => dest.MapFrom(x => x.InterCompanyExpenseAccRef))
                      .ForMember(source => source.InterCompanyRateAccRef, dest => dest.MapFrom(x => x.InterCompanyRateAccRef))
                      .ForMember(source => source.InterCompanyRoyaltyAccRef, dest => dest.MapFrom(x => x.InterCompanyRoyaltyAccRef))
                      .ForMember(source => source.CompanyMiiwaid, dest => dest.MapFrom(x => x.CompanyMiiwaid))
                      .ForMember(source => source.OperatingCountry, dest => dest.MapFrom(x => x.OperatingCountry))
                      .ForMember(source => source.IsActive, dest => dest.MapFrom(x => x.IsActive))
                      .ForMember(source => source.IsUseIctms, dest => dest.MapFrom(x => x.IsUseIctms))
                      .ForMember(source => source.IsFullUse, dest => dest.MapFrom(x => x.IsFullUse))
                      .ForMember(source => source.Region, dest => dest.MapFrom(x => x.Region))
                      .ForMember(source => source.IsCostOfSalesEmailOverrideAllow, dest => dest.MapFrom(x => x.IsCOSEmailOverrideAllow))
                      .ForMember(source => source.AverageTshourlycost, dest => dest.MapFrom(x => x.AvgTSHourlyCost))
                      .ForMember(source => source.VatTaxRegistrationNo, dest => dest.MapFrom(x => x.VatTaxRegNo))
                      .ForMember(source => source.Euvatprefix, dest => dest.MapFrom(x => x.EUVatPrefix))
                      .ForMember(source => source.Iaregion, dest => dest.MapFrom(x => x.IARegion))
                      .ForMember(source => source.CognosNumber, dest => dest.MapFrom(x => x.CognosNumber))
                      .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
                      .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
                      .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
                      //.ForMember(source => source.Logo.Name, dest => dest.MapFrom(x => x.LogoText))
                      .ForMember(source => source.ResourceOutsideDistance, dest => dest.MapFrom(x => x.ResourceOutsideDistance))
                      .ForMember(source => source.VatregTextWithinEc, dest => dest.MapFrom(x =>  x.VATRegulationTextWithinEC))
                      .ForMember(source => source.VatregTextOutsideEc, dest => dest.MapFrom(x => x.VATRegulationTextOutsideEC))
                      .ForMember(source => source.CompanyMiiwaref, dest => dest.MapFrom(x => x.CompanyMiiwaref))
                      .ForMember(source => source.GfsCoa, dest => dest.MapFrom(x => x.GfsCoa))
                      .ForMember(source => source.GfsBu, dest => dest.MapFrom(x => x.GfsBu))
                     //.ReverseMap()
                     .ForAllOtherMembers(x => x.Ignore());

                      

            //Company Mongo Document Search
            CreateMap<CompanySearch, Document.Domain.Models.Document.EvolutionMongoDocumentSearch>()
                .ForMember(dest => dest.ModuleCode, src => src.ResolveUsing(x => Common.Enums.ModuleCodeType.COMP.ToString()))
                .ForMember(dest => dest.ReferenceCode, src => src.MapFrom(x => x.CompanyCode))
                .ForMember(dest => dest.Text, src => src.ResolveUsing<SearchTextFormatResolver, string>("DocumentSearchText"))
                .ForMember(dest => dest.DocumentTypes, src => src.ResolveUsing(x => string.IsNullOrEmpty(x.SearchDocumentType) ? null : new List<string>() { x.SearchDocumentType }))
                .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Company Tax
            CreateMap<DbModel.CompanyTax, DomainModel.CompanyTax>()
                .ForMember(dest => dest.CompanyTaxId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.CompanyCode, src => src.MapFrom(x => x.Company.Code))
                .ForMember(dest => dest.TaxCode, src => src.MapFrom(x=>x.Code))
                .ForMember(dest => dest.TaxName, src => src.MapFrom(x=>x.Name))
                .ForMember(dest => dest.TaxRate, src => src.MapFrom(x=>x.Rate))
                .ForMember(dest => dest.TaxType, src => src.MapFrom(x=>x.TaxType))
                .ForMember(dest => dest.IsActive, src => src.MapFrom(x=>x.IsActive))
                .ForMember(dest => dest.IsIcInv, src => src.MapFrom(x=>x.IsIcinv))
                .ForMember(dest => dest.LastModification, dest => dest.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Company Payroll
            CreateMap<DbModel.CompanyPayroll, DomainModel.CompanyPayroll>()
             .ForMember(dest => dest.CompanyPayrollId, src => src.MapFrom(x => x.Id))
             .ForMember(dest => dest.CompanyCode, src => src.MapFrom(x => x.Company.Code))
             .ForMember(dest => dest.PayrollType, src => src.MapFrom(x => x.Name)) //Defect 45 Changes
             .ForMember(dest => dest.Currency, src => src.MapFrom(x => x.Currency))
             .ForMember(dest => dest.ExportPrefix, src => src.MapFrom(x => x.ExportPrefix))
             .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
             .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
             .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
             .ReverseMap()
             .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Company Payroll Period            
            CreateMap<DbModel.CompanyPayrollPeriod, DomainModel.CompanyPayrollPeriod>()
                .ForMember(dest => dest.CompanyPayrollPeriodId, dest => dest.MapFrom(x => x.Id))
                .ForMember(dest => dest.CompanyCode, dest => dest.MapFrom(x => x.CompanyPayroll.Company.Code))
                .ForMember(dest => dest.CompanyPayrollId, dest => dest.MapFrom(x => x.CompanyPayroll.Id))
                .ForMember(dest => dest.PayrollType, dest => dest.MapFrom(x => x.CompanyPayroll.Name))//Defect 45 Changes
                // .ForMember(dest => dest.PayrollExportPrefix, dest => dest.MapFrom(x => x.CompanyPayroll.PayrollType.PayrollExportPrefix))
                .ForMember(dest => dest.PeriodName, dest => dest.MapFrom(x => x.PeriodName))
                .ForMember(dest => dest.PeriodStatus, dest => dest.MapFrom(x => x.PeriodStatus))
                .ForMember(dest => dest.IsActive, dest => dest.MapFrom(x => x.IsActive))
                .ForMember(dest => dest.StartDate, dest => dest.MapFrom(x => x.StartDate))
                .ForMember(dest => dest.EndDate, dest => dest.MapFrom(x => x.EndDate))
                .ForMember(dest => dest.LastModification, dest => dest.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());
            #endregion
            
            #region Company Division
            CreateMap<DbModel.CompanyDivision, DomainModel.CompanyDivision>()
                .ForMember(source => source.CompanyDivisionId, dest => dest.MapFrom(x => x.Id))
                .ForMember(source => source.DivisionName, dest => dest.MapFrom(x => x.Division.Name))
                .ForMember(source => source.CompanyCode, dest => dest.MapFrom(x => x.Company.Code))
                .ForMember(source => source.DivisionAcReference, dest => dest.MapFrom(x => x.AccountReference))
                .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
                .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
                .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
                .ReverseMap()
               .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Company Cost Center
            CreateMap<DbModel.CompanyDivisionCostCenter, DomainModel.CompanyDivisionCostCenter>()
                .ForMember(source => source.CompanyDivisionCostCenterId, dest => dest.MapFrom(x => x.Id))
                .ForMember(source => source.CompanyCode, dest => dest.MapFrom(x => x.CompanyDivision.Company.Code))
                .ForMember(source => source.CompanyDivisionId, dest => dest.MapFrom(x => x.CompanyDivision.Id))
                .ForMember(source => source.Division, dest => dest.MapFrom(x => x.CompanyDivision.Division.Name))
                .ForMember(source => source.CostCenterCode, dest => dest.MapFrom(x => x.Code))
                .ForMember(source => source.CostCenterName, dest => dest.MapFrom(x => x.Name))
                .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
                .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
                .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Company Expected Margin
            CreateMap<DbModel.CompanyExpectedMargin, DomainModel.CompanyExpectedMargin>()
                .ForMember(source => source.CompanyExpectedMarginId, dest => dest.MapFrom(x => x.Id))
                .ForMember(source => source.CompanyCode, dest => dest.MapFrom(x => x.Company.Code))
                .ForMember(source => source.MarginType, dest => dest.MapFrom(x => x.MarginType.Name))
                .ForMember(source => source.MinimumMargin, dest => dest.MapFrom(x => x.MinimumMargin))
                .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
                .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
                .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Company Note
            CreateMap<DbModel.CompanyNote, DomainModel.CompanyNote>()
                .ForMember(dest => dest.CompanyNoteId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.CompanyCode, src => src.MapFrom(x => x.Company.Code))
                .ForMember(dest => dest.Notes, src => src.MapFrom(x => x.Note))
                .ForMember(dest => dest.CreatedOn, src => src.MapFrom(x => x.CreatedDate))
                .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => x.CreatedBy))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
               

                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Company Office
            CreateMap<DbModel.CompanyOffice, DomainModel.CompanyAddress>()
                .ForMember(dest => dest.CompanyCode, src => src.MapFrom(x => x.Company.Code))
                .ForMember(dest => dest.Country, src => src.MapFrom(x => x.Country.Name))
                .ForMember(dest => dest.State, src => src.MapFrom(x => x.County.Name))
                .ForMember(dest => dest.City, src => src.MapFrom(x => x.City.Name))
                .ForMember(dest => dest.CountryId, src => src.MapFrom(x => x.Country.Id)) //Added for ITK D1536
                .ForMember(dest => dest.StateId, src => src.MapFrom(x => x.County.Id)) //Added for ITK D1536
                .ForMember(dest => dest.CityId, src => src.MapFrom(x => x.City.Id)) //Added for ITK D1536
                .ForMember(dest => dest.FullAddress, src => src.MapFrom(x => x.Address))
                .ForMember(dest => dest.PostalCode, src => src.MapFrom(x => x.PostalCode))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.AccountRef, src => src.MapFrom(x => x.AccountRef))
                .ForMember(dest => dest.OfficeName, src => src.MapFrom(x => x.OfficeName))
                .ForMember(dest => dest.AddressId, src => src.MapFrom(x => x.Id))
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Company Qualification
            CreateMap<DbModel.CompanyQualificationType, DomainModel.CompanyQualification>()
                .ForMember(dest => dest.CompanyQualificationId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.CompanyCode, src => src.MapFrom(x => x.Company.Code))
                .ForMember(dest => dest.Qualification, src => src.MapFrom(x => x.QualificationType.Name))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Company Message
            CreateMap<DbModel.CompanyMessage, BaseMessage>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
              .ForMember(dest => dest.MsgIdentifier, src => src.MapFrom(x => x.Identifier))
              .ForMember(dest => dest.MsgType, src => src.MapFrom(x => x.MessageType.Name))
              .ForMember(dest => dest.MsgText, src => src.MapFrom(x => x.Message))
              .ForMember(dest => dest.IsDefaultMsg, src => src.MapFrom(x => x.IsDefaultMessage))
              .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
              .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
              .ReverseMap()
              .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.CompanyMessage, DomainModel.CompanyMessage>()
               .ForMember(dest => dest.MsgIdentifier, src => src.MapFrom(x => x.Identifier))
               .ForMember(dest => dest.MsgType, src => src.MapFrom(x => x.MessageType.Name))
               .ForMember(dest => dest.MsgText, src => src.MapFrom(x => x.Message))
               .ForMember(dest => dest.IsDefaultMsg, src => src.MapFrom(x => x.IsDefaultMessage))
               .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
               .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
               .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
               .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
               .ReverseMap()
               .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Company Invoice
            CreateMap<List<DbModel.CompanyMessage>, DomainModel.CompanyInvoice>()
                .ForMember(dest => dest.InvoiceDraftText, src => src.ResolveUsing(new MessageResolver<DbModel.CompanyMessage, DomainModel.CompanyInvoice>(Enum.CompanyMessageType.InvoiceDraftText.ToString())))
                .ForMember(dest => dest.InvoiceHeader, src => src.ResolveUsing(new MessageResolver<DbModel.CompanyMessage, DomainModel.CompanyInvoice>(Enum.CompanyMessageType.InvoiceHeader.ToString())))
                .ForMember(dest => dest.InvoiceDescriptionText, src => src.ResolveUsing(new MessageResolver<DbModel.CompanyMessage, DomainModel.CompanyInvoice>(Enum.CompanyMessageType.InvoiceDescriptionText.ToString())))
                .ForMember(dest => dest.InvoiceInterCompDescription, src => src.ResolveUsing(new MessageResolver<DbModel.CompanyMessage, DomainModel.CompanyInvoice>(Enum.CompanyMessageType.InterCompanyDescription.ToString())))
                .ForMember(dest => dest.InvoiceInterCompText, src => src.ResolveUsing(new MessageResolver<DbModel.CompanyMessage, DomainModel.CompanyInvoice>(Enum.CompanyMessageType.InterCompanyText.ToString())))
                .ForMember(dest => dest.InvoiceInterCompDraftText, src => src.ResolveUsing(new MessageResolver<DbModel.CompanyMessage, DomainModel.CompanyInvoice>(Enum.CompanyMessageType.InterCompanyDraftText.ToString())))
                .ForMember(dest => dest.InvoiceSummarryText, src => src.ResolveUsing(new MessageResolver<DbModel.CompanyMessage, DomainModel.CompanyInvoice>(Enum.CompanyMessageType.FreeText.ToString())))
                .ForMember(dest => dest.InvoiceRemittances, src => src.ResolveUsing(new MessagesResolver<DbModel.CompanyMessage, DomainModel.CompanyInvoice, DomainModel.CompanyMessage>(Enum.CompanyMessageType.InvoiceRemittanceText.ToString())))
                .ForMember(dest => dest.InvoiceFooters, src => src.ResolveUsing(new MessagesResolver<DbModel.CompanyMessage, DomainModel.CompanyInvoice, DomainModel.CompanyMessage>(Enum.CompanyMessageType.InvoiceFooterText.ToString())))
                .ForMember(dest => dest.TechSpecialistExtranetComment, src => src.ResolveUsing(new MessageResolver<DbModel.CompanyMessage, DomainModel.CompanyInvoice>(Enum.CompanyMessageType.TechSpecialistExtranetComment.ToString())))
                .ForMember(dest => dest.CustomerExtranetComment, src => src.ResolveUsing(new MessageResolver<DbModel.CompanyMessage, DomainModel.CompanyInvoice>(Enum.CompanyMessageType.CustomerExtranetComment.ToString())))
                .ForMember(dest => dest.ReverseChargeDisclaimer, src => src.ResolveUsing(new MessageResolver<DbModel.CompanyMessage, DomainModel.CompanyInvoice>(Enum.CompanyMessageType.ReverseChargeDisclaimer.ToString())))
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Company Invoice
            CreateMap<DomainModel.CompanyInvoice, DomainModel.CompanyPartialInvoice>()
                .ForMember(dest => dest.InvoiceDraftText, dest => dest.MapFrom(x => x.InvoiceDraftText))
                .ForMember(dest => dest.InvoiceHeader, dest => dest.MapFrom(x => x.InvoiceHeader))
                .ForMember(dest => dest.InvoiceDescriptionText, dest => dest.MapFrom(x => x.InvoiceDescriptionText))
                .ForMember(dest => dest.InvoiceInterCompDescription, dest => dest.MapFrom(x => x.InvoiceInterCompDescription))
                .ForMember(dest => dest.InvoiceInterCompText, dest => dest.MapFrom(x => x.InvoiceInterCompText))
                .ForMember(dest => dest.InvoiceInterCompDraftText, dest => dest.MapFrom(x => x.InvoiceInterCompDraftText))
                .ForMember(dest => dest.InvoiceSummarryText, dest => dest.MapFrom(x => x.InvoiceSummarryText))
                .ForMember(dest => dest.TechSpecialistExtranetComment, dest => dest.MapFrom(x => x.TechSpecialistExtranetComment))
                .ForMember(dest => dest.CustomerExtranetComment, dest => dest.MapFrom(x => x.CustomerExtranetComment))
                .ForMember(dest => dest.ReverseChargeDisclaimer, dest => dest.MapFrom(x => x.ReverseChargeDisclaimer))
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Company Email Templates
            CreateMap<List<DbModel.CompanyMessage>, DomainModel.CompanyEmailTemplate>()
                .ForMember(dest => dest.CustomerReportingNotificationEmailText, src => src.ResolveUsing(new MessageResolver<DbModel.CompanyMessage, DomainModel.CompanyEmailTemplate>(Enum.CompanyMessageType.EmailCustomerReportingNotification.ToString())))
                .ForMember(dest => dest.CustomerDirectReportingEmailText, src => src.ResolveUsing(new MessageResolver<DbModel.CompanyMessage, DomainModel.CompanyEmailTemplate>(Enum.CompanyMessageType.EmailCustomerDirectReporting.ToString())))
                .ForMember(dest => dest.RejectVisitTimesheetEmailText, src => src.ResolveUsing(new MessageResolver<DbModel.CompanyMessage, DomainModel.CompanyEmailTemplate>(Enum.CompanyMessageType.EmailRejectedVisit.ToString())))
                .ForMember(dest => dest.VisitCompletedCoordinatorEmailText, src => src.ResolveUsing(new MessageResolver<DbModel.CompanyMessage, DomainModel.CompanyEmailTemplate>(Enum.CompanyMessageType.EmailVisitCompletedToCoordinator.ToString())))
                .ForMember(dest => dest.InterCompanyOperatingCoordinatorEmail, src => src.ResolveUsing(new MessageResolver<DbModel.CompanyMessage, DomainModel.CompanyEmailTemplate>(Enum.CompanyMessageType.EmailInterCompanyAssignmentToCoordinator.ToString())))
                .ForMember(dest => dest.InterCompanyDiscountAmendmentReasonEmail, src => src.ResolveUsing(new MessageResolver<DbModel.CompanyMessage, DomainModel.CompanyEmailTemplate>(Enum.CompanyMessageType.EmailInterCompanyDiscountAmendmentReason.ToString())))
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region CompanyDetail
           

                  CreateMap<DbModel.Company, DomainModel.CompanyDetail>()
                .ForMember(dest => dest.CompanyInfo, src => src.MapFrom(x => x))
                .ForMember(dest => dest.CompanyDivisions, src => src.MapFrom(x => x.CompanyDivision))
                .ForMember(dest => dest.CompanyPayrolls, src => src.MapFrom(x => x.CompanyPayroll))
                .ForMember(dest => dest.CompanyExpectedMargins, src => src.MapFrom(x => x.CompanyExpectedMargin))
                .ForMember(dest => dest.CompanyNotes, src => src.MapFrom(x => x.CompanyNote))
                .ForMember(dest => dest.CompanyOffices, src => src.MapFrom(x => x.CompanyOffice))
                .ForMember(dest => dest.CompanyInvoiceInfo, src => src.MapFrom(x => x.CompanyMessage.ToList()))
                .ForMember(dest => dest.CompanyEmailTemplates, src => src.MapFrom(x => x.CompanyMessage.ToList()))
                .ForMember(dest => dest.CompanyTaxes, src => src.MapFrom(x => x.CompanyTax))
                .ForMember(dest => dest.CompanyDivisionCostCenters, src => src.MapFrom(x => x.CompanyDivision.SelectMany(x1=>x1.CompanyDivisionCostCenter)))
                 .ForMember(dest => dest.CompanyPayrollPeriods, src => src.MapFrom(x => x.CompanyPayroll.SelectMany(x1=>x1.CompanyPayrollPeriod)))
                  .ForAllOtherMembers(x => x.Ignore());
            #endregion
        }
    }
}



