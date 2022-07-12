using AutoMapper;
using Evolution.Project.Domain.Enums;
using Evolution.Project.Domain.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Project.Domain.Models.Projects;

namespace Evolution.Project.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            #region ProjectSearch

            CreateMap<DbModel.Project, DomainModel.ProjectSearch>()
                  .ForMember(source => source.ContractNumber, dest => dest.MapFrom(x => x.Contract.ContractNumber))
                  .ForMember(source => source.CustomerContractNumber, dest => dest.MapFrom(x => x.Contract.CustomerContractNumber))
                  .ForMember(source => source.ProjectNumber, dest => dest.MapFrom(x => x.ProjectNumber))
                  .ForMember(source => source.CustomerProjectNumber, dest => dest.MapFrom(x => x.CustomerProjectNumber))
                  .ForMember(source => source.CustomerProjectName, dest => dest.MapFrom(x => x.CustomerProjectName))
                  .ForMember(source => source.ContractHoldingCompanyCode, dest => dest.MapFrom(x => x.Contract.ContractHolderCompany.Code))
                  .ForMember(source => source.ContractHoldingCompanyName, dest => dest.MapFrom(x => x.Contract.ContractHolderCompany.Name))
                  .ForMember(source => source.IsContractHoldingCompanyActive, dest => dest.MapFrom(x => x.Contract.ContractHolderCompany.IsActive)) // ITK D - 619
                  .ForMember(source => source.ContractCustomerCode, dest => dest.MapFrom(x => x.Contract.Customer.Code))
                  .ForMember(source => source.ContractCustomerName, dest => dest.MapFrom(x => x.Contract.Customer.Name))
                  .ForMember(source => source.CompanyOffice, dest => dest.MapFrom(x => x.CompanyOffice.OfficeName))
                  .ForMember(source => source.CompanyDivision, dest => dest.MapFrom(x => x.CompanyDivision.Division.Name))
                  .ForMember(source => source.ProjectStartDate, dest => dest.MapFrom(x => x.StartDate))
                  .ForMember(source => source.ProjectEndDate, dest => dest.MapFrom(x => x.EndDate))
                  .ForMember(source => source.ContractType, dest => dest.MapFrom(x => x.Contract.ContractType))
                  .ForMember(source => source.ProjectType, dest => dest.MapFrom(x => x.ProjectType.Name))
                  .ForMember(source => source.ProjectStatus, dest => dest.MapFrom(x => x.Status))
                  .ForMember(source => source.ProjectCoordinatorName, dest => dest.MapFrom(x => x.Coordinator.Name))
                  .ForMember(source => source.ProjectCoordinatorCode, dest => dest.MapFrom(x=>x.Coordinator.SamaccountName)) //IGO QC D-900 Issue 1
                  .ForMember(source => source.IsEReportProjectMapped, dest => dest.MapFrom(x => x.IsEreportProjectMapped))
                  .ReverseMap()
                  .ForAllOtherMembers(x => x.Ignore());


            //Contract Mongo Document Search
            CreateMap<ProjectSearch, Document.Domain.Models.Document.EvolutionMongoDocumentSearch>()
                .ForMember(dest => dest.ModuleCode, src => src.ResolveUsing(x => Common.Enums.ModuleCodeType.PRJ.ToString()))
                .ForMember(dest => dest.ReferenceCode, src => src.MapFrom(x => x.ProjectNumber))
                .ForMember(dest => dest.Text, src => src.ResolveUsing<Automapper.Resolver.MongoSearch.SearchTextFormatResolver, string>("DocumentSearchText"))
                .ForMember(dest => dest.DocumentTypes, src => src.ResolveUsing(x => string.IsNullOrEmpty(x.SearchDocumentType) ? null : new List<string>() { x.SearchDocumentType }))
                .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Project

            CreateMap<DbModel.Project, DomainModel.Project>()
                  .ForMember(source => source.ContractNumber, dest => dest.MapFrom(x => x.Contract.ContractNumber))
                  .ForMember(source => source.CustomerContractNumber, dest => dest.MapFrom(x => x.Contract.CustomerContractNumber))
                  .ForMember(source => source.ProjectNumber, dest => dest.MapFrom(x => x.ProjectNumber))
                  .ForMember(source => source.CustomerProjectNumber, dest => dest.MapFrom(x => x.CustomerProjectNumber))
                  .ForMember(source => source.CustomerProjectName, dest => dest.MapFrom(x => x.CustomerProjectName))
                  .ForMember(source => source.ContractHoldingCompanyCode, dest => dest.MapFrom(x => x.Contract.ContractHolderCompany.Code))
                  .ForMember(source => source.ContractHoldingCompanyName, dest => dest.MapFrom(x => x.Contract.ContractHolderCompany.Name))
                  .ForMember(source => source.IsContractHoldingCompanyActive, dest => dest.MapFrom(x => x.Contract.ContractHolderCompany.IsActive)) // ITK D - 619
                  .ForMember(source => source.ContractCustomerCode, dest => dest.MapFrom(x => x.Contract.Customer.Code))
                  .ForMember(source => source.ContractCustomerName, dest => dest.MapFrom(x => x.Contract.Customer.Name))
                  .ForMember(source => source.ProjectStartDate, dest => dest.MapFrom(x => x.StartDate))
                  .ForMember(source => source.ProjectEndDate, dest => dest.MapFrom(x => x.EndDate))
                  .ForMember(source => source.ContractType, dest => dest.MapFrom(x => x.Contract.ContractType))
                   .ForMember(source => source.ProjectType, dest => dest.MapFrom(x => x.ProjectType.Name))
                  .ForMember(source => source.ProjectStatus, dest => dest.MapFrom(x => x.Status))
                  .ForMember(source => source.VatRegistrationNumber, dest => dest.MapFrom(x => x.CustomerInvoiceAddress.VatTaxRegistrationNo.Trim()))
                  .ForMember(source => source.EUVATPrefix, dest => dest.MapFrom(x => x.CustomerInvoiceAddress.Euvatprefix.Trim()))
                  .ForMember(source => source.IsVisitOnPopUp, dest => dest.MapFrom(x => x.IsVisitOnPopUp))
                  .ForMember(source => source.IsProjectForNewFacility, dest => dest.MapFrom(x => x.IsNewFacility))
                  .ForMember(source => source.IsExtranetSummaryVisibleToClient, dest => dest.MapFrom(x => x.IsExtranetSummaryVisibleToClient))
                  .ForMember(source => source.CompanyOffice, dest => dest.MapFrom(x => x.CompanyOffice.OfficeName))
                  .ForMember(source => source.CompanyDivision, dest => dest.MapFrom(x => x.CompanyDivision.Division.Name))
                  .ForMember(source => source.CompanyCostCenterCode, dest => dest.MapFrom(x => x.CompanyanyDivCostCentre.Code))
                  .ForMember(source => source.CompanyCostCenterName, dest => dest.MapFrom(x => x.CompanyanyDivCostCentre.Name))
                  .ForMember(source => source.WorkFlowType, dest => dest.MapFrom(x => x.WorkFlowType.Trim()))
                  .ForMember(source => source.IndustrySector, dest => dest.MapFrom(x => x.IndustrySector))
                  .ForMember(source => source.IsManagedServices, dest => dest.MapFrom(x => x.IsManagedServices))
                  .ForMember(source => source.ManagedServiceType, dest => dest.MapFrom(x => x.ManagedServicesType))
                  .ForMember(source => source.ProjectCoordinatorName, dest => dest.MapFrom(x => x.Coordinator.Name))
                  .ForMember(source => source.ProjectCoordinatorCode, dest => dest.MapFrom(x=> x.Coordinator.SamaccountName ))//IGO QC D-900 Issue 1
                  .ForMember(source => source.ManagedServiceCoordinatorName, dest => dest.MapFrom(x => x.ManagedServicesCoordinator.Name))
                  .ForMember(source => source.ManagedServiceCoordinatorCode,dest => dest.MapFrom(x=>x.ManagedServicesCoordinator.SamaccountName))//IGO QC D-900 Issue 1
                  .ForMember(source => source.ManagedServiceTypeName, dest => dest.MapFrom(x => x.ManagedServicesTypeNavigation.Name))  //Added for D-1035 

                  .ForMember(source => source.ProjectBudgetValue, dest => dest.MapFrom(x => x.Budget))
                  .ForMember(source => source.ProjectBudgetWarning, dest => dest.MapFrom(x => x.BudgetWarning))
                  .ForMember(source => source.ProjectBudgetHoursUnit, dest => dest.MapFrom(x => x.BudgetHours))
                  .ForMember(source => source.ProjectBudgetHoursWarning, dest => dest.MapFrom(x => x.BudgetHoursWarning))
                  .ForMember(source => source.ProjectBudgetCurrency, dest => dest.MapFrom(x => x.Contract.BudgetCurrency))

                  .ForMember(source => source.IsEReportProjectMapped, dest => dest.MapFrom(x => x.IsEreportProjectMapped))
                  .ForMember(source => source.CreationDate, dest => dest.MapFrom(x => x.CreationDate))
                  .ForMember(source => source.LogoText, dest => dest.MapFrom(x => x.Logo.Name))
                  .ForMember(source => source.IsRemittanectext, dest => dest.MapFrom(x => x.IsRemittanceText))

                  .ForMember(source => source.ProjectCustomerContact, dest => dest.MapFrom(x => x.CustomerProjectContact.ContactName))
                  .ForMember(source => source.ProjectCustomerInvoiceContact, dest => dest.MapFrom(x => x.CustomerContact.ContactName))
                  .ForMember(source => source.ProjectCustomerInvoiceAddress, dest => dest.MapFrom(x => x.CustomerInvoiceAddress.Address))
                  .ForMember(source => source.ProjectCustomerContactAddress, dest => dest.MapFrom(x => x.CustomerProjectAddress.Address))
                  .ForMember(source => source.ProjectSalesTax, dest => dest.MapFrom(x => x.InvoiceSalesTax.Name))
                  .ForMember(source => source.ProjectWithHoldingTax, dest => dest.MapFrom(x => x.InvoiceWithholdingTax.Name))
                  
                  .ForMember(source => source.ProjectInvoicingCurrency, dest => dest.MapFrom(x => x.InvoiceCurrency))
                  .ForMember(source => source.ProjectInvoiceGrouping, dest => dest.MapFrom(x => x.InvoiceGrouping))
                  .ForMember(source => source.CustomerDirectReportingEmailAddress, dest => dest.MapFrom(x => x.CustomerDirectReportingEmailAddress))

                  .ForMember(source => source.ProjectInvoicePaymentTerms, dest => dest.MapFrom(x => x.InvoicePaymentTerms.Name))
                  .ForMember(source => source.ProjectAssignmentOperationNotes, dest => dest.MapFrom(x => x.ProjectMessage.FirstOrDefault(m => m.ProjectId == x.Id && m.MessageTypeId == Convert.ToInt16(ProjectMessageType.OperationalNotes)).Message))
                  .ForMember(source => source.ProjectInvoiceInstructionNotes, dest => dest.MapFrom(x => x.ProjectMessage.FirstOrDefault(m => m.ProjectId == x.Id && m.MessageTypeId == Convert.ToInt16(ProjectMessageType.InvoiceNotes)).Message))
                  .ForMember(source => source.ProjectInvoiceRemittanceIdentifier, dest => dest.MapFrom(x => x.InvoiceRemittanceText.Identifier))
                  .ForMember(source => source.ProjectInvoiceFooterIdentifier, dest => dest.MapFrom(x => x.InvoiceFooterText.Identifier))
                  .ForMember(source => source.ProjectInvoiceFreeText, dest => dest.MapFrom(x => x.ProjectMessage.FirstOrDefault(m => m.ProjectId == x.Id && m.MessageTypeId == Convert.ToInt16(ProjectMessageType.InvoiceFreeText)).Message))
                  .ForMember(source => source.ProjectClientReportingRequirement, dest => dest.MapFrom(x => x.ProjectMessage.FirstOrDefault(m => m.ProjectId == x.Id && m.MessageTypeId == Convert.ToInt16(ProjectMessageType.ReportingRequirements)).Message))

                  //Added for Assignment
                  .ForMember(dest => dest.AssignmentParentContractCompany, src => src.MapFrom(x => x.Contract.ParentContract.ContractHolderCompany.Name))
                  .ForMember(dest => dest.AssignmentParentContractCompanyCode, src => src.MapFrom(x => x.Contract.ParentContract.ContractHolderCompany.Code))
                  .ForMember(dest => dest.AssignmentParentContractDiscount, src => src.MapFrom(x => x.Contract.ParentContractDiscountPercentage))

                  .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
                  .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
                  .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
                  .ReverseMap()
                  .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region ProjectNote
            CreateMap<DbModel.ProjectNote, DomainModel.ProjectNote>()
                .ForMember(dest => dest.ProjectNoteId, source => source.MapFrom(x => x.Id))
                .ForMember(dest => dest.ProjectNumber, source => source.MapFrom(x => x.Project.ProjectNumber))
                .ForMember(dest => dest.Notes, source => source.MapFrom(x => x.Note))
                 .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
                  .ForMember(dest => dest.CreatedOn, source => source.MapFrom(x => x.CreatedDate))
                   .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                    .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                     .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                     .ReverseMap()
                     .ForAllOtherMembers(x => x.Ignore());


            #endregion

            #region ProjectDocument
            //CreateMap<DbModel.ProjectDocument, DomainModel.ProjectDocument>()
            //    .ForMember(dest=>dest.ProjectDocumentId,source=>source.MapFrom(x=>x.Id))
            //     .ForMember(dest => dest.ProjectNumber, source => source.MapFrom(x => x.Project.ProjectNumber))
            //      .ForMember(dest => dest.DocumentType, source => source.MapFrom(x => x.DocumentType))
            //       .ForMember(dest => dest.DocumentSize, source => source.MapFrom(x => x.DocumentSize))
            //        .ForMember(dest => dest.IsVisibleToCustomer, source => source.MapFrom(x => x.IsVisibleToCustomer))
            //         .ForMember(dest => dest.IsVisibleToTS, source => source.MapFrom(x => x.IsVisibleToTechnicalSpecialist))
            //          .ForMember(dest => dest.Name, source => source.MapFrom(x => x.Name))
            //           .ForMember(dest => dest.UploadDataId, source => source.MapFrom(x => x.UploadedDataId))
            //            .ForMember(dest => dest.UploadedOn, source => source.MapFrom(x => x.CreatedDate))
            //             .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
            //              .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
            //               .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
            //              .ReverseMap()
            //         .ForAllOtherMembers(x => x.Ignore());


            //CreateMap<DbModel.ProjectDocument, ModuleDocument>()
            //.ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
            // .ForMember(dest => dest.ModuleRefCode, source => source.MapFrom(x => x.Project.ProjectNumber))
            //  .ForMember(dest => dest.DocumentType, source => source.MapFrom(x => x.DocumentType))
            //   .ForMember(dest => dest.DocumentSize, source => source.MapFrom(x => x.DocumentSize))
            //    .ForMember(dest => dest.IsVisibleToCustomer, source => source.MapFrom(x => x.IsVisibleToCustomer))
            //     .ForMember(dest => dest.IsVisibleToTS, source => source.MapFrom(x => x.IsVisibleToTechnicalSpecialist))
            //      .ForMember(dest => dest.Name, source => source.MapFrom(x => x.Name))
            //       .ForMember(dest => dest.UploadDataId, source => source.MapFrom(x => x.UploadedDataId))
            //        .ForMember(dest => dest.UploadedOn, source => source.MapFrom(x => x.CreatedDate))
            //         .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
            //          .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
            //           .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
            //          .ReverseMap()
            //     .ForAllOtherMembers(x => x.Ignore());

           // CreateMap<DomainModel.ProjectDocument, ModuleDocument>()
           //.ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
           //.ForMember(dest => dest.Id, source => source.MapFrom(x => x.ProjectDocumentId))
           // .ForMember(dest => dest.ModuleRefCode, source => source.MapFrom(x => x.ProjectNumber))
           //  .ForMember(dest => dest.DocumentType, source => source.MapFrom(x => x.DocumentType))
           //   .ForMember(dest => dest.DocumentSize, source => source.MapFrom(x => x.DocumentSize))
           //    .ForMember(dest => dest.IsVisibleToCustomer, source => source.MapFrom(x => x.IsVisibleToCustomer))
           //     .ForMember(dest => dest.IsVisibleToTS, source => source.MapFrom(x => x.IsVisibleToTS))
           //      .ForMember(dest => dest.Name, source => source.MapFrom(x => x.Name))
           //       .ForMember(dest => dest.UploadDataId, source => source.MapFrom(x => x.UploadDataId))
           //        .ForMember(dest => dest.UploadedOn, source => source.MapFrom(x => x.UploadedOn))
           //         .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
           //          .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
           //           .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
           //          .ReverseMap()
           //     .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region ProjectInvoiceAttachment

            CreateMap<DbModel.ProjectInvoiceAttachment, DomainModel.ProjectInvoiceAttachment>()
               .ForMember(dest => dest.ProjectInvoiceAttachmentId, src => src.MapFrom(x => x.Id))
               .ForMember(dest => dest.ProjectNumber, src => src.MapFrom(x => x.Project.ProjectNumber))
               .ForMember(dest => dest.DocumentType, src => src.MapFrom(x => x.DocumentType.Name))
               .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
               .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
               .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
               .ReverseMap()
               .ForAllOtherMembers(src => src.Ignore());

            #endregion

            #region ProjectInvoiceAssignmnetReference

            CreateMap<DbModel.ProjectInvoiceAssignmentReference, DomainModel.ProjectInvoiceReference>()
          .ForMember(dest => dest.ProjectInvoiceReferenceTypeId, src => src.MapFrom(x => x.Id))
          .ForMember(dest => dest.ProjectNumber, src => src.MapFrom(x => x.Project.ProjectNumber))
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

            #region ProjectClientNotification

           CreateMap<DbModel.ProjectClientNotification, DomainModel.ProjectClientNotification>()
          .ForMember(dest => dest.ProjectClientNotificationId, src => src.MapFrom(x => x.Id))
          .ForMember(dest => dest.ProjectNumber, src => src.MapFrom(x => x.Project.ProjectNumber))
          .ForMember(dest => dest.CustomerContact, src => src.MapFrom(x => x.CustomerContact.ContactName))
          .ForMember(dest => dest.EmailAddress, src => src.MapFrom(x => x.CustomerContact.EmailAddress))
          .ForMember(dest => dest.IsSendInspectionReleaseNotesNotification, src => src.MapFrom(x => x.SendInspectionReleaseNotesNotification))
          .ForMember(dest => dest.IsSendFlashReportingNotification, src => src.MapFrom(x => x.SendFlashReportingNotification))
          .ForMember(dest => dest.IsSendNCRReportingNotification, src => src.MapFrom(x => x.SendNcrreportingNotification))
          .ForMember(dest => dest.IsSendCustomerReportingNotification, src => src.MapFrom(x => x.SendCustomerReportingNotification))
          .ForMember(dest => dest.IsSendCustomerDirectReportingNotification, src => src.MapFrom(x => x.SendCustomerDirectReportingNotification))
          .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
          .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
          .ReverseMap()
          .ForAllOtherMembers(src => src.Ignore());

            #endregion

             
        }
    }
}
