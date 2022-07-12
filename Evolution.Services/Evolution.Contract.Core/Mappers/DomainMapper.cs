using AutoMapper;
using Evolution.Common.Models.Budget;
using Evolution.Contract.Domain.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;
using Evolution.Automapper.Resolver.MongoSearch;
using Enum = Evolution.Contract.Domain.Enums;

namespace Evolution.Contract.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            #region ContractSearch

            CreateMap<DbModel.Contract, DomainModel.ContractSearch>()
                  .ForMember(source => source.ContractHoldingCompanyCode, dest => dest.MapFrom(x => x.ContractHolderCompany.Code))
                  .ForMember(source => source.ContractHoldingCompanyName, dest => dest.MapFrom(x => x.ContractHolderCompany.Name))
                  .ForMember(source => source.IsContractHoldingCompanyActive, dest => dest.MapFrom(x => x.ContractHolderCompany.IsActive)) // ITK D - 619
                  .ForMember(source => source.ContractNumber, dest => dest.MapFrom(x => x.ContractNumber))
                  .ForMember(source => source.ContractCustomerName, dest => dest.MapFrom(x => x.Customer.Name))
                  .ForMember(source => source.ContractCustomerCode, dest => dest.MapFrom(x => x.Customer.Code))
                  .ForMember(source => source.CustomerContractNumber, dest => dest.MapFrom(x => x.CustomerContractNumber))
                  .ForMember(source => source.ContractType, dest => dest.MapFrom(x => x.ContractType))
                  .ForMember(source => source.ContractStartDate, dest => dest.MapFrom(x => x.StartDate))
                  .ForMember(source => source.ContractEndDate, dest => dest.MapFrom(x => x.EndDate))
                  .ForMember(source => source.ContractStatus, dest => dest.MapFrom(x => x.Status))
                  //.ForMember(source => source.ContractEndDate, dest => dest.MapFrom(x => x.EndDate <=DateTime.Now.AddDays(ContractFutureDays)))
                  .ReverseMap()
                  .ForAllOtherMembers(x => x.Ignore());

            //Contract Mongo Document Search
            CreateMap<ContractSearch, Document.Domain.Models.Document.EvolutionMongoDocumentSearch>()
                .ForMember(dest => dest.ModuleCode, src => src.ResolveUsing(x => Common.Enums.ModuleCodeType.CNT.ToString()))
                .ForMember(dest => dest.ReferenceCode, src => src.MapFrom(x => x.ContractNumber))
                .ForMember(dest => dest.Text, src => src.ResolveUsing<SearchTextFormatResolver, string>("DocumentSearchText"))
                .ForMember(dest => dest.DocumentTypes, src => src.ResolveUsing(x => string.IsNullOrEmpty(x.SearchDocumentType) ? null : new List<string>() { x.SearchDocumentType }))
                .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Contract

            CreateMap<DbModel.Contract, DomainModel.Contract>()
                  .ForMember(source => source.Id, dest => dest.MapFrom(x => x.Id))
                  .ForMember(source => source.ContractNumber, dest => dest.MapFrom(x => x.ContractNumber))
                  .ForMember(source => source.CustomerContractNumber, dest => dest.MapFrom(x => x.CustomerContractNumber))
                  .ForMember(source => source.ContractHoldingCompanyCode, dest => dest.MapFrom(x => x.ContractHolderCompany.Code))
                  .ForMember(source => source.ContractHoldingCompanyName, dest => dest.MapFrom(x => x.ContractHolderCompany.Name))
                  .ForMember(source => source.IsContractHoldingCompanyActive, dest => dest.MapFrom(x => x.ContractHolderCompany.IsActive)) // ITK D - 619
                  .ForMember(source => source.ContractCustomerName, dest => dest.MapFrom(x => x.Customer.Name))
                  .ForMember(source => source.ContractCustomerCode, dest => dest.MapFrom(x => x.Customer.Code))
                  .ForMember(source => source.CustomerContractNumber, dest => dest.MapFrom(x => x.CustomerContractNumber))
                  .ForMember(source => source.ContractType, dest => dest.MapFrom(x => x.ContractType))
                  .ForMember(source => source.ContractStatus, dest => dest.MapFrom(x => x.Status))
                  .ForMember(source => source.ContractBudgetHours, dest => dest.MapFrom(x => x.BudgetHours))
                  .ForMember(source => source.ContractBudgetMonetaryValue, dest => dest.MapFrom(x => x.Budget))
                  .ForMember(source => source.ContractBudgetMonetaryCurrency, dest => dest.MapFrom(x => x.BudgetCurrency))
                  .ForMember(source => source.ContractBudgetMonetaryWarning, dest => dest.MapFrom(x => x.BudgetWarning))
                  .ForMember(source => source.ContractBudgetHours, dest => dest.MapFrom(x => x.BudgetHours))
                  .ForMember(source => source.ContractBudgetHoursWarning, dest => dest.MapFrom(x => x.BudgetHoursWarning))
                  .ForMember(source => source.IsParentContract, dest => dest.MapFrom(x => x.CompanyOfficeId.HasValue))
                  .ForMember(source => source.ParentCompanyOffice, dest => dest.MapFrom(x => x.CompanyOffice.OfficeName))
                  .ForMember(source => source.IsChildContract, dest => dest.MapFrom(x => x.ParentContractId.HasValue))
                  .ForMember(source => source.ParentContractId, dest => dest.MapFrom(x => x.ParentContractId))
                  .ForMember(source => source.ParentContractNumber, dest => dest.MapFrom(x => x.ParentContract.ContractNumber))
                  .ForMember(source => source.ParentContractDiscount, dest => dest.MapFrom(x => x.ParentContractDiscountPercentage))
                  .ForMember(source => source.ParentCompanyCode, dest => dest.MapFrom(x => x.ParentContract.ContractHolderCompany.Code))
                  .ForMember(source => source.ParentContractHolder, dest => dest.MapFrom(x => x.ParentContract.CompanyOffice.Company.Name))
                  .ForMember(source => source.IsFrameworkContract, dest => dest.MapFrom(x => x.FrameworkCompanyOfficeId.HasValue))
                  .ForMember(source => source.FrameworkCompanyOfficeName, dest => dest.MapFrom(x => x.FrameworkCompanyOffice.OfficeName))
                  .ForMember(source => source.IsRelatedFrameworkContract, dest => dest.MapFrom(x => x.FrameworkContractId.HasValue))
                  .ForMember(source => source.FrameworkContractNumber, dest => dest.MapFrom(x => x.FrameworkContract.ContractNumber))
                  .ForMember(source => source.FrameworkCompanyCode, dest => dest.MapFrom(x => x.FrameworkContract.ContractHolderCompany.Code))
                  .ForMember(source => source.FrameworkContractHolder, dest => dest.MapFrom(x => x.FrameworkContract.FrameworkCompanyOffice.Company.Name))
                  .ForMember(source => source.ContractInvoicingCompanyCode, dest => dest.MapFrom(x => x.InvoicingCompany.Code))
                  .ForMember(source => source.ContractInvoicingCompanyName, dest => dest.MapFrom(x => x.InvoicingCompany.Name)) 
                  .ForMember(source => source.ContractStartDate, dest => dest.MapFrom(x => x.StartDate))
                  .ForMember(source => source.CreatedDate, dest => dest.MapFrom(x=> x.CreatedDate))
                  .ForMember(source => source.ContractEndDate, dest => dest.MapFrom(x => x.EndDate))
                  .ForMember(source => source.IsCRM, dest => dest.MapFrom(x => x.IsCrmstatus))
                  .ForMember(source => source.ContractCRMReference, dest => dest.MapFrom(x => x.Crmreference == null ? null : (Int64?)x.Crmreference))
                  .ForMember(source => source.ContractCRMReason, dest => dest.MapFrom(x => x.Crmreason))
                  .ForMember(source => source.ContractCustomerContact, dest => dest.MapFrom(x => x.DefaultCustomerContractContact.ContactName))
                  .ForMember(source => source.ContractCustomerInvoiceContact, dest => dest.MapFrom(x => x.DefaultCustomerInvoiceContact.ContactName))
                  .ForMember(source => source.ContractCustomerInvoiceAddress, dest => dest.MapFrom(x => x.DefaultCustomerInvoiceAddress.Address))
                  .ForMember(source => source.ContractSalesTax, dest => dest.MapFrom(x => x.DefaultSalesTax.Name))
                  .ForMember(source => source.ContractWithHoldingTax, dest => dest.MapFrom(x => x.DefaultWithholdingTax.Name))
                  .ForMember(source => source.ContractInvoicingCurrency, dest => dest.MapFrom(x => x.DefaultInvoiceCurrency))
                  .ForMember(source => source.ContractInvoiceGrouping, dest => dest.MapFrom(x => x.DefaultInvoiceGrouping))
                  .ForMember(source => source.ContractCustomerContactAddress, dest => dest.MapFrom(x => x.DefaultCustomerContractAddress.Address))
                  .ForMember(source => source.ContractInvoicePaymentTerms, dest => dest.MapFrom(x => x.InvoicePaymentTerms.Name))
                  .ForMember(source => source.IsFixedExchangeRateUsed, dest => dest.MapFrom(x => x.IsUseFixedExchangeRates))
                  .ForMember(source => source.IsParentContractInvoiceUsed, dest => dest.MapFrom(x => x.IsUseInvoiceDetailsFromParentContract))
                  .ForMember(source => source.IsRemittanceText, dest => dest.MapFrom(x => x.IsRemittanceText))
                  .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
                  .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
                  .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
                  .ForMember(source => source.ContractOperationalNote, dest => dest.MapFrom(x => x.ContractMessage.FirstOrDefault(m => m.ContractId == x.Id && m.MessageTypeId == Convert.ToInt16(Enum.ContractMessageType.OperationalNotes) && m.IsActive ==true).Message))
                  .ForMember(source => source.ContractInvoiceInstructionNotes, dest => dest.MapFrom(x => x.ContractMessage.FirstOrDefault(m => m.ContractId == x.Id && m.MessageTypeId == Convert.ToInt16(Enum.ContractMessageType.DefaultInvoiceNotes) && m.IsActive ==true).Message))
                  .ForMember(source => source.ContractInvoiceRemittanceIdentifier, dest => dest.MapFrom(x => x.DefaultRemittanceText.Identifier))
                  .ForMember(source => source.ContractInvoiceFooterIdentifier, dest => dest.MapFrom(x => x.DefaultFooterText.Identifier))
                  .ForMember(source => source.ContractInvoiceFreeText, dest => dest.MapFrom(x => x.ContractMessage.FirstOrDefault(m => m.ContractId == x.Id && m.MessageTypeId == Convert.ToInt16(Enum.ContractMessageType.InvoiceFreeText) && m.IsActive == true).Message))
                  .ForMember(source => source.ContractClientReportingRequirement, dest => dest.MapFrom(x => x.ContractMessage.FirstOrDefault(m => m.ContractId == x.Id && m.MessageTypeId == Convert.ToInt16(Enum.ContractMessageType.ReportingRequirements) && m.IsActive ==true).Message))
                  .ForMember(source => source.ContractConflictOfInterest, dest => dest.MapFrom(x => x.ContractMessage.FirstOrDefault(m => m.ContractId == x.Id && m.MessageTypeId == Convert.ToInt16(Enum.ContractMessageType.ConflictofInterest) && m.IsActive ==true).Message))
                  //.ForMember(dest => dest.ParentContractInvoicingDetails, opt => opt.MapFrom(x => x ))
                  .ReverseMap()
                  .ForAllOtherMembers(x => x.Ignore());

            //def 590
            CreateMap<DbModel.Contract, DomainModel.ParentContractInvoicingDetails>()
                  .ForMember(source => source.ParentContractSalesTax, dest => dest.MapFrom(x => x.ParentContract != null ? x.ParentContract.DefaultSalesTax.Name : null))
                  .ForMember(source => source.ParentContractWithHoldingTax, dest => dest.MapFrom(x => x.ParentContract != null ? x.ParentContract.DefaultWithholdingTax.Name  : null))
                  .ForMember(source => source.ParentContractInvoiceRemittanceIdentifier, dest => dest.MapFrom(x => x.ParentContract != null ? x.ParentContract.DefaultRemittanceText.Identifier  : null))
                  .ForMember(source => source.ParentContractInvoiceFooterIdentifier, dest => dest.MapFrom(x => x.ParentContract != null ? x.ParentContract.DefaultFooterText.Identifier  : null))
                  .ForMember(source => source.IsParentContractInvoiceUsed, dest => dest.MapFrom(x => x.ParentContract.IsUseInvoiceDetailsFromParentContract))
                  .ForAllOtherMembers(x => x.Ignore());


            CreateMap<DbModel.Contract, DomainModel.BaseContract>()
                 .ForMember(source => source.ContractNumber, dest => dest.MapFrom(x => x.ContractNumber))
                 .ForMember(source => source.CustomerContractNumber, dest => dest.MapFrom(x => x.CustomerContractNumber))
                 .ForMember(source => source.ContractHoldingCompanyCode, dest => dest.MapFrom(x => x.ContractHolderCompany.Code))
                 .ForMember(source => source.ContractHoldingCompanyName, dest => dest.MapFrom(x => x.ContractHolderCompany.Name))
                 .ForMember(source => source.ContractCustomerName, dest => dest.MapFrom(x => x.Customer.Name))
                 .ForMember(source => source.ContractCustomerCode, dest => dest.MapFrom(x => x.Customer.Code))
                 .ForMember(source => source.CustomerContractNumber, dest => dest.MapFrom(x => x.CustomerContractNumber))
                 .ForMember(source => source.ContractType, dest => dest.MapFrom(x => x.ContractType))
                 .ForMember(source => source.ContractStatus, dest => dest.MapFrom(x => x.Status))
                 .ForMember(source => source.ContractStartDate, dest => dest.MapFrom(x => x.StartDate))
                 .ForMember(source => source.ContractEndDate, dest => dest.MapFrom(x => x.EndDate))
                 .ForMember(source => source.ContractCRMReference, dest => dest.MapFrom(x => x.Crmreference==null?null:(Int64?)x.Crmreference))
                 .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
                 .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
                 .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
                 .ReverseMap()
                 .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region ContractInvoiceReferenceType

            CreateMap<DbModel.ContractInvoiceReference, DomainModel.ContractInvoiceReferenceType>()
            .ForMember(dest => dest.ContractInvoiceReferenceTypeId, src => src.MapFrom(x => x.Id))
            .ForMember(dest => dest.ContractNumber, src => src.MapFrom(x => x.Contract.ContractNumber))
            .ForMember(dest => dest.DisplayOrder, src => src.MapFrom(x => Convert.ToInt32(x.SortOrder)))
            .ForMember(dest => dest.IsVisibleToAssignment, src => src.MapFrom(x => x.IsAssignment))
            .ForMember(dest => dest.IsVisibleToTimesheet, src => src.MapFrom(x => x.IsTimesheet))
            .ForMember(dest => dest.IsVisibleToVisit, src => src.MapFrom(x => x.IsVisit))
            .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
            .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
            .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
            .ForMember(dest => dest.ReferenceType, src => src.MapFrom(x => x.AssignmentReferenceType.Name))
            .ReverseMap()
            .ForAllOtherMembers(src => src.Ignore());

            #endregion

            #region InvoiceAttachment

            CreateMap<DbModel.ContractInvoiceAttachment, DomainModel.ContractInvoiceAttachment>()
                .ForMember(dest => dest.ContractInvoiceAttachmentId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.ContractNumber, src => src.MapFrom(x => x.Contract.ContractNumber))
                .ForMember(dest => dest.DocumentType, src => src.MapFrom(x => x.DocumentType.Name))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ReverseMap()
                .ForAllOtherMembers(src => src.Ignore());
            #endregion

            #region Contract Exchange Rate

            CreateMap<DbModel.ContractExchangeRate, DomainModel.ContractExchangeRate>()
                 .ForMember(dest => dest.ExchangeRateId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.ContractNumber, src => src.MapFrom(x => x.Contract.ContractNumber))
                .ForMember(dest => dest.EffectiveFrom, src => src.MapFrom(x => x.EffectiveFrom))
                .ForMember(dest => dest.ExchangeRate, src => src.MapFrom(x => x.ExchangeRate))
                .ForMember(dest => dest.FromCurrency, src => src.MapFrom(x => x.CurrencyFrom))
                //.ForMember(dest => dest.IsFixedExchangeRateUsed, src => src.MapFrom(x => x.Contract.IsUseFixedExchangeRates))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.ToCurrency, src => src.MapFrom(x => x.CurrencyTo))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ReverseMap()
                .ForAllOtherMembers(src => src.Ignore());

            #endregion

            #region ContractSchedule
            CreateMap<DbModel.ContractSchedule, DomainModel.ContractSchedule>()
            .ForMember(source =>source.ScheduleId, dest => dest.MapFrom(x=>x.Id))
            .ForMember(source => source.ContractNumber, dest => dest.MapFrom(x => x.Contract.ContractNumber))
            .ForMember(source => source.ScheduleName, dest => dest.MapFrom(x => x.Name))
            .ForMember(source => source.ScheduleNameForInvoicePrint, dest => dest.MapFrom(x => x.ScheduleNoteForInvoice))
            .ForMember(source => source.ChargeCurrency, dest => dest.MapFrom(x => x.Currency))
            /*AddScheduletoRF*/
            .ForMember(source => source.BaseScheduleId, dest => dest.MapFrom(x => x.BaseScheduleId))
            //.ForMember(source => source.BaseScheduleName, dest => dest.MapFrom(x => x.BaseSchedule.Name))
            .ForMember(source => source.CanBeDeleted, dest => dest.MapFrom(x => !(x.AssignmentContractSchedule.Any() || x.AssignmentTechnicalSpecialistSchedule.Any())))
            .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
            .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
            .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
            .ReverseMap()
            .ForAllOtherMembers(x => x.Ignore());
            #endregion
          

            #region ContractRate
            CreateMap<DbModel.ContractRate, DomainModel.ContractScheduleRate>()
            .ForMember(source => source.RateId, dest => dest.MapFrom(x => x.Id))
            .ForMember(source => source.ContractNumber, dest => dest.MapFrom(x => x.ContractSchedule.Contract.ContractNumber))
            .ForMember(source => source.ScheduleName, dest => dest.MapFrom(x => x.ContractSchedule.Name))
            .ForMember(source => source.ScheduleId, dest => dest.MapFrom(x => x.ContractSchedule.Id))
            .ForMember(source => source.ExpenseTypeId, dest => dest.MapFrom(x => x.ExpenseType.Id))
            .ForMember(source => source.ChargeType, dest => dest.MapFrom(x => x.ExpenseType.Name))
            .ForMember(source => source.ChargeValue, dest => dest.MapFrom(x => x.Rate))
            .ForMember(source => source.Description, dest => dest.MapFrom(x => x.Description))
            .ForMember(source => source.IsDescriptionPrintedOnInvoice, dest => dest.MapFrom(x => x.IsPrintDescriptionOnInvoice))
            .ForMember(source => source.StandardValue, dest => dest.MapFrom(x => x.StandardValue))
            .ForMember(source => source.DiscountApplied, dest => dest.MapFrom(x => x.DiscountApplied))
            .ForMember(source => source.Percentage, dest => dest.MapFrom(x => x.Percentage))
            .ForMember(source => source.EffectiveFrom, dest => dest.MapFrom(x => x.FromDate))
            .ForMember(source => source.EffectiveTo, dest => dest.MapFrom(x => x.ToDate))
            .ForMember(source => source.IsActive, dest => dest.MapFrom(x => x.IsActive))
            .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
            .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
            .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
            .ForMember(source => source.Currency, dest => dest.MapFrom(x => x.ContractSchedule.Currency))
            //.ForMember(source => source.BaseScheduleName, dest => dest.MapFrom(x => x.ContractBaseSchedule.Name))
            //.ForMember(source => source.BaseRateId, dest => dest.MapFrom(x => x.ContractBaseRate.Id))
            /*AddScheduletoRF*/
            .ForMember(source => source.BaseScheduleId, dest => dest.MapFrom(x => x.BaseScheduleId))
            .ForMember(source => source.BaseRateId, dest => dest.MapFrom(x => x.BaseRateId))
            .ForMember(source => source.ChargeRateType, dest => dest.MapFrom(x => x.ExpenseType.Type))
    
            .ForMember(source => source.StandardChargeSchedule, dest => dest.MapFrom(x => x.StandardInspectionTypeChargeRate.CompanyChgSchInspGrpInspectionType.CompanyChgSchInspGroup.CompanyChargeSchedule.StandardChargeSchedule.Name))
            .ForMember(source => source.StandardChargeScheduleInspectionGroup, dest => dest.MapFrom(x => x.StandardInspectionTypeChargeRate.CompanyChgSchInspGrpInspectionType.CompanyChgSchInspGroup.StandardInspectionGroup.Name))
            .ForMember(source => source.StandardChargeScheduleInspectionType, dest => dest.MapFrom(x => x.StandardInspectionTypeChargeRate.CompanyChgSchInspGrpInspectionType.StandardInspectionType.Name))
            .ForMember(source => source.StandardInspectionTypeChargeRateId, dest => dest.MapFrom(x => x.StandardInspectionTypeChargeRate.Id))
            
            .ReverseMap()
            .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region ContractDocument

            //CreateMap<DbModel.ContractDocument, DomainModel.ContractDocument>()
            //.ForMember(dest => dest.ContractDocumentId, source => source.MapFrom(x => x.Id))
            //.ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
            // .ForMember(dest => dest.ContractNumber, source => source.MapFrom(x => x.Contract.ContractNumber))
            //.ForMember(dest => dest.DocumentType, src => src.MapFrom(x => x.DocumentType))
            //.ForMember(dest => dest.IsVisibleToTS, src => src.MapFrom(x => x.IsVisibleToTechnicalSpecialist))
            //.ForMember(dest => dest.IsVisibleToCustomer, src => src.MapFrom(x => x.IsVisibleToCustomer))
            //.ForMember(dest => dest.IsOutOfCompanyVisible, src => src.MapFrom(x => x.IsVisibleToOutOfCompany))
            //.ForMember(dest => dest.DocumentSize, src => src.MapFrom(x => x.DocumentSize))
            //.ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
            //.ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
            //.ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
            //.ForMember(dest => dest.UploadedOn, src => src.MapFrom(x => x.CreatedDate))
            //.ForMember(dest => dest.UploadDataId, src => src.MapFrom(x => x.UploadedDataId))
            //.ReverseMap()
            //.ForAllOtherMembers(src => src.Ignore());


            //CreateMap<DbModel.ContractDocument, ModuleDocument>()
            //.ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
            //.ForMember(dest => dest.Name, src => src.MapFrom(x => x.Name))
            // .ForMember(dest => dest.ModuleRefCode, source => source.MapFrom(x => x.Contract.ContractNumber))
            //.ForMember(dest => dest.DocumentType, src => src.MapFrom(x => x.DocumentType))
            //.ForMember(dest => dest.IsVisibleToTS, src => src.MapFrom(x => x.IsVisibleToTechnicalSpecialist))
            //.ForMember(dest => dest.IsVisibleToCustomer, src => src.MapFrom(x => x.IsVisibleToCustomer))
            //.ForMember(dest => dest.IsOutOfCompanyVisible, src => src.MapFrom(x => x.IsVisibleToOutOfCompany))
            //.ForMember(dest => dest.DocumentSize, src => src.MapFrom(x => x.DocumentSize))
            //.ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
            //.ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
            //.ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
            //.ForMember(dest => dest.UploadedOn, src => src.MapFrom(x => x.CreatedDate))
            //.ForMember(dest => dest.UploadDataId, src => src.MapFrom(x => x.UploadedDataId))
            //.ReverseMap()
            //.ForAllOtherMembers(src => src.Ignore());

            //CreateMap<DbModel.ContractDocument, Evolution.DbRepository.MongoModels.ContractDocument>()
            //  .ForMember(dest => dest.ContractNumber, source => source.MapFrom(x => x.Contract.ContractNumber))
            //  .ForMember(dest => dest.DocumentName, source => source.MapFrom(x => x.Name))
            //  .ForMember(dest => dest.UploadGuidID, source => source.MapFrom(x => x.UploadedDataId))
            //  .ReverseMap()
            //  .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Contract Note
            CreateMap<DbModel.ContractNote, DomainModel.ContractNote>()
              .ForMember(dest => dest.ContractNoteId, source => source.MapFrom(x => x.Id))
              .ForMember(dest => dest.ContractNumber, source => source.MapFrom(x => x.Contract.ContractNumber))
              .ForMember(dest => dest.CreatedOn, source => source.MapFrom(x => x.CreatedDate))
              .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
              .ForMember(dest => dest.Notes, source => source.MapFrom(x => x.Note))
              .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
              .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
              .ReverseMap()
              .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region COntract Account Item
            CreateMap<DbModel.GetAccountItemDetailResult, BudgetAccountItem>()
                .ForMember(dest => dest.ContractId, src => src.MapFrom(x => x.ContractId))
                .ForMember(dest => dest.ContractNumber, src => src.MapFrom(x => x.ContractNumber))
                .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.AssignmentNumber, src => src.MapFrom(x => x.AssignmentNumber))
                .ForMember(dest => dest.BudgetCurrency, src => src.MapFrom(x => x.BudgetCurrency))
                .ForMember(dest => dest.ChargeExchangeRate, src => src.MapFrom(x => x.ChargeExchangeRate))
                .ForMember(dest => dest.ChargeRate, src => src.MapFrom(x => x.ChargeRate))
                .ForMember(dest => dest.ChargeRateCurrency, src => src.MapFrom(x => x.ChargeRateCurrency))
                .ForMember(dest => dest.ChargeTotalUnit, src => src.MapFrom(x => x.ChargeTotalUnit))
                .ForMember(dest => dest.ExpenseName, src => src.MapFrom(x => x.ExpenseName))
                .ForMember(dest => dest.ExpenseType, src => src.MapFrom(x => x.ExpenseType))
                .ForMember(dest => dest.ProjectId, src => src.MapFrom(x => x.ProjectId))
                .ForMember(dest => dest.ProjectNumber, src => src.MapFrom(x => x.ProjectNumber))
                .ForMember(dest => dest.Status, src => src.MapFrom(x => x.Status))
                .ForMember(dest => dest.VisitDate, src => src.MapFrom(x => x.VisitDate))
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());
            #endregion
        }
    }
}
