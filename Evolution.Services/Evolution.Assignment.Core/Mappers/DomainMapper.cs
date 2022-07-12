using AutoMapper;
using Evolution.Assignment.Core.Resolver;
using Evolution.Assignment.Domain.Enums;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Automapper.Resolver.MongoSearch;
using Evolution.Common.Enums; //MS-TS Link CR
using Evolution.Common.Extensions;
using Evolution.Common.Mappers.Resolvers;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Budget;
using Evolution.Project.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomineModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            #region DB Assignment  to Domain Edit Assignment Search
            CreateMap<DbModel.Assignment, DomineModel.AssignmentEditSearch>()
                 .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.Id))
                 .ForMember(dest => dest.IsAssignmentCompleted, src => src.MapFrom(x => x.IsAssignmentComplete))
                 .ForMember(dest => dest.AssignmentNumber, src => src.MapFrom(x => x.AssignmentNumber))
                 .ForMember(dest => dest.AssignmentReference, src => src.MapFrom(x => x.AssignmentReference))
                 .ForMember(dest => dest.AssignmentCustomerName, src => src.MapFrom(x => x.CustomerAssignmentContact.CustomerAddress.Customer.Name))
                 .ForMember(dest => dest.AssignmentContractHoldingCompanyCode, src => src.MapFrom(x => x.ContractCompany.Code))
                 .ForMember(dest => dest.AssignmentContractHoldingCompany, src => src.MapFrom(x => x.ContractCompany.Name))
                 .ForMember(dest => dest.IsContractHoldingCompanyActive, src => src.MapFrom(x => x.ContractCompany.IsActive)) // ITK D - 619
                 .ForMember(dest => dest.AssignmentCustomerName, src => src.MapFrom(x => x.Project.Contract.Customer.Name))
                 .ForMember(dest => dest.AssignmentOperatingCompanyCode, src => src.MapFrom(x => x.OperatingCompany.Code))
                 .ForMember(dest => dest.AssignmentOperatingCompany, src => src.MapFrom(x => x.OperatingCompany.Name))
                 .ForMember(dest => dest.IsOperatingCompanyActive, src => src.MapFrom(x => x.OperatingCompany.IsActive)) // ITK D - 619
                 .ForMember(dest => dest.AssignmentProjectNumber, src => src.MapFrom(x => x.Project.ProjectNumber))
                 .ForMember(dest => dest.AssignmentStatus, src => src.MapFrom(x => x.AssignmentStatus))
                 .ForMember(dest => dest.AssignmentSupplierPurchaseOrderId, src => src.MapFrom(x => x.SupplierPurchaseOrderId))
                 .ForMember(dest => dest.AssignmentSupplierPurchaseOrderNumber, src => src.MapFrom(x => x.SupplierPurchaseOrder.SupplierPonumber))
                 .ForMember(dest => dest.MaterialDescription, src => src.MapFrom(x => x.SupplierPurchaseOrder.MaterialDescription))
                 .ForMember(dest => dest.AssignmentFirstVisitDate, src => src.MapFrom(x => x.FirstVisitTimesheetStartDate))
                 .ForMember(dest => dest.TechSpecialists, src => src.MapFrom(x => x.AssignmentTechnicalSpecialist))
                 .ForMember(dest => dest.IsInternalAssignment, src => src.MapFrom(x => x.IsInternalAssignment))
                 .ForAllOtherMembers(src => src.Ignore());

            #endregion

            #region DB Assignment  to Domain Assignment Search
            CreateMap<DbModel.Assignment, DomineModel.AssignmentSearch>()
                 .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.Id))
                 .ForMember(dest => dest.AssignmentCompanyAddress, src => src.MapFrom(x => x.AssignmentCompanyAddress.Address))
                 .ForMember(dest => dest.IsAssignmentCompleted, src => src.MapFrom(x => x.IsAssignmentComplete))
                 .ForMember(dest => dest.AssignmentLifecycle, src => src.MapFrom(x => x.AssignmentLifecycle.Name))
                 .ForMember(dest => dest.AssignmentNumber, src => src.MapFrom(x => x.AssignmentNumber))
                 .ForMember(dest => dest.AssignmentReference, src => src.MapFrom(x => x.AssignmentReference))
                 .ForMember(dest => dest.AssignmentCustomerCode, src => src.MapFrom(x => x.CustomerAssignmentContact.CustomerAddress.Customer.Code))
                 .ForMember(dest => dest.AssignmentCustomerName, src => src.MapFrom(x => x.CustomerAssignmentContact.CustomerAddress.Customer.Name))
                 .ForMember(dest => dest.AssignmentType, src => src.MapFrom(x => x.AssignmentType))
                 .ForMember(dest => dest.AssignmentContractHoldingCompany, src => src.MapFrom(x => x.ContractCompany.Name))
                 .ForMember(dest => dest.AssignmentContractHoldingCompanyCode, src => src.MapFrom(x => x.ContractCompany.Code))
                 .ForMember(dest => dest.IsContractHoldingCompanyActive, src => src.MapFrom(x => x.ContractCompany.IsActive)) // ITK D - 619
                 .ForMember(dest => dest.AssignmentContractHoldingCompanyCoordinator, src => src.MapFrom(x => x.ContractCompanyCoordinator.Name))
                 .ForMember(dest => dest.AssignmentContractHoldingCompanyCoordinatorCode, src => src.MapFrom(x => x.ContractCompanyCoordinator.SamaccountName))
                 .ForMember(dest => dest.AssignmentContractNumber, src => src.MapFrom(x => x.Project.Contract.ContractNumber))
                 .ForMember(dest => dest.AssignmentCustomerAssigmentContact, src => src.MapFrom(x => x.CustomerAssignmentContact.ContactName))
                 .ForMember(dest => dest.AssignmentCustomerName, src => src.MapFrom(x => x.Project.Contract.Customer.Name))
                 .ForMember(dest => dest.AssignmentOperatingCompanyCode, src => src.MapFrom(x => x.OperatingCompany.Code))
                 .ForMember(dest => dest.AssignmentOperatingCompany, src => src.MapFrom(x => x.OperatingCompany.Name))
                 .ForMember(dest => dest.IsOperatingCompanyActive, src => src.MapFrom(x => x.OperatingCompany.IsActive)) // ITK D - 619
                 .ForMember(dest => dest.AssignmentHostCompanyCode, src => src.MapFrom(x => x.HostCompany.Code))
                 .ForMember(dest => dest.AssignmentHostCompany, src => src.MapFrom(x => x.HostCompany.Name))
                 .ForMember(dest => dest.AssignmentOperatingCompanyCoordinator, src => src.MapFrom(x => x.OperatingCompanyCoordinator.Name))
                 .ForMember(dest => dest.AssignmentOperatingCompanyCoordinatorCode, src => src.MapFrom(x => x.OperatingCompanyCoordinator.SamaccountName))
                 .ForMember(dest => dest.AssignmentProjectNumber, src => src.MapFrom(x => x.Project.ProjectNumber))
                 .ForMember(dest => dest.AssignmentCustomerProjectNumber, src => src.MapFrom(x => x.Project.CustomerProjectNumber))
                 .ForMember(dest => dest.AssignmentCustomerProjectName, src => src.MapFrom(x => x.Project.CustomerProjectName))
                 .ForMember(dest => dest.AssignmentStatus, src => src.MapFrom(x => x.AssignmentStatus))
                 .ForMember(dest => dest.AssignmentSupplierName, src => src.MapFrom(x => x.SupplierPurchaseOrder.Supplier.SupplierName))
                 .ForMember(dest => dest.AssignmentSupplierPurchaseOrderId, src => src.MapFrom(x => x.SupplierPurchaseOrderId))
                 .ForMember(dest => dest.AssignmentSupplierPurchaseOrderNumber, src => src.MapFrom(x => x.SupplierPurchaseOrder.SupplierPonumber))
                 .ForMember(dest => dest.MaterialDescription, src => src.MapFrom(x => x.SupplierPurchaseOrder.MaterialDescription))
                 .ForMember(dest => dest.AssignmentFirstVisitDate, src => src.MapFrom(x => x.FirstVisitTimesheetStartDate))
                 //.ForMember(dest => dest.AssignmentProjectWorkFlow, src => src.MapFrom(x => x.Project.WorkFlowType))
                 .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => Convert.ToInt32(x.UpdateCount)))
                 .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                 .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                 .ForMember(dest => dest.TechSpecialists, src => src.MapFrom(x => x.AssignmentTechnicalSpecialist))
                 .ForMember(dest => dest.IsInternalAssignment, src => src.MapFrom(x => x.IsInternalAssignment))
                 .ForAllOtherMembers(src => src.Ignore());

            #endregion

            #region Assignment Document Search
            CreateMap<DomineModel.AssignmentSearch, Document.Domain.Models.Document.EvolutionMongoDocumentSearch>()
                .ForMember(dest => dest.ModuleCode, src => src.ResolveUsing(x => Common.Enums.ModuleCodeType.ASGMNT.ToString()))
                .ForMember(dest => dest.ReferenceCode, src => src.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.Text, src => src.ResolveUsing<SearchTextFormatResolver, string>("DocumentSearchText"))
                .ForMember(dest => dest.DocumentTypes, src => src.ResolveUsing(x => string.IsNullOrEmpty(x.SearchDocumentType) ? null : new List<string>() { x.SearchDocumentType }))
                .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Assignment Document Search
            CreateMap<DomineModel.AssignmentEditSearch, Document.Domain.Models.Document.EvolutionMongoDocumentSearch>()
                .ForMember(dest => dest.ModuleCode, src => src.ResolveUsing(x => Common.Enums.ModuleCodeType.ASGMNT.ToString()))
                .ForMember(dest => dest.ReferenceCode, src => src.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.Text, src => src.ResolveUsing<SearchTextFormatResolver, string>("DocumentSearchText"))
                .ForMember(dest => dest.DocumentTypes, src => src.ResolveUsing(x => string.IsNullOrEmpty(x.SearchDocumentType) ? null : new List<string>() { x.SearchDocumentType }))
                .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Domain Assignment Search to DB Assignment  
            CreateMap<DomineModel.AssignmentSearch, DbModel.Assignment>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.AssignmentNumber, src => src.MapFrom(x => x.AssignmentNumber))
                .ForMember(dest => dest.AssignmentCompanyAddressId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentCompanyAddress") ? ((List<DbModel.CustomerAddress>)context.Options.Items["AssignmentCompanyAddress"])?.FirstOrDefault(x => x.Address == src.AssignmentCompanyAddress)?.Id : null))
                .ForMember(dest => dest.IsAssignmentComplete, src => src.MapFrom(x => x.IsAssignmentCompleted))
                .ForMember(dest => dest.AssignmentLifecycleId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentLifeCycle") ? ((List<DbModel.Data>)context.Options.Items["AssignmentLifeCycle"])?.FirstOrDefault(x => x.Name == src.AssignmentLifecycle)?.Id : null))
                .ForMember(dest => dest.AssignmentNumber, src => src.MapFrom(x => x.AssignmentNumber))
                .ForMember(dest => dest.AssignmentReference, src => src.MapFrom(x => x.AssignmentReference))
                .ForMember(dest => dest.CustomerAssignmentContactId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentCustomerContact") ? ((List<DbModel.CustomerContact>)context.Options.Items["AssignmentCustomerContact"])?.FirstOrDefault(x => x.ContactName == src.AssignmentCustomerAssigmentContact)?.Id : null))
                .ForMember(dest => dest.AssignmentType, src => src.MapFrom(x => x.AssignmentType))
                .ForMember(dest => dest.ContractCompanyId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentContractHoldingCompany") ? ((List<DbModel.Contract>)context.Options.Items["AssignmentContractHoldingCompany"])?.FirstOrDefault(x => x.ContractHolderCompany.Code == src.AssignmentContractHoldingCompanyCode)?.Id : null))
                .ForMember(dest => dest.ContractCompanyCoordinatorId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentContractHoldingCoordinator") ? ((List<DbModel.User>)context.Options.Items["AssignmentContractHoldingCoordinator"])?.FirstOrDefault(x => x.Name == src.AssignmentContractHoldingCompanyCoordinator)?.Id : null))
                .ForMember(dest => dest.OperatingCompanyId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentOperatingCompany") ? ((List<DbModel.Company>)context.Options.Items["AssignmentOperatingCompany"])?.FirstOrDefault(x => x.Code == src.AssignmentOperatingCompanyCode)?.Id : null))
                .ForMember(dest => dest.HostCompanyId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentHostCompany") ? ((List<DbModel.Company>)context.Options.Items["AssignmentHostCompany"])?.FirstOrDefault(x => x.Code == src.AssignmentHostCompanyCode)?.Id : null))
                .ForMember(dest => dest.OperatingCompanyCoordinatorId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentOperatingCompanyCoordinator") ? ((List<DbModel.User>)context.Options.Items["AssignmentOperatingCompanyCoordinator"])?.FirstOrDefault(x => x.Name == src.AssignmentOperatingCompanyCoordinator)?.Id : null))
                .ForMember(dest => dest.ProjectId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentProjectNumber") ? ((List<DbModel.Project>)context.Options.Items["AssignmentProjectNumber"])?.FirstOrDefault(x => x.ProjectNumber == src.AssignmentProjectNumber)?.Id : null))
                .ForMember(dest => dest.AssignmentStatus, src => src.MapFrom(x => x.AssignmentStatus))
                .ForMember(dest => dest.SupplierPurchaseOrderId, src => src.MapFrom(x => x.AssignmentSupplierPurchaseOrderId))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => Convert.ToInt32(x.UpdateCount)))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForAllOtherMembers(src => src.Ignore());
            #endregion

            #region AssignmentDashboard

            CreateMap<DbModel.Assignment, DomineModel.AssignmentDashboard>()
                .ForMember(dest => dest.IsAssignmentCompleted, src => src.MapFrom(x => x.IsAssignmentComplete))
                .ForMember(dest => dest.AssignmentCreatedDate, src => src.MapFrom(x => x.CreatedDate))
                .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.AssignmentNumber, src => src.MapFrom(x => x.AssignmentNumber))
                .ForMember(dest => dest.AssignmentReference, src => src.MapFrom(x => x.AssignmentReference))
                .ForMember(dest => dest.AssignmentType, src => src.MapFrom(x => x.AssignmentType))
                .ForMember(dest => dest.AssignmentContractHoldingCompany, src => src.MapFrom(x => x.ContractCompany.Name))
                .ForMember(dest => dest.AssignmentContractHoldingCompanyCode, src => src.MapFrom(x => x.ContractCompany.Code))  //D-1351 Fix
                .ForMember(dest => dest.AssignmentContractHoldingCompanyCoordinator, src => src.MapFrom(x => x.ContractCompanyCoordinator.Name))
                .ForMember(dest => dest.AssignmentContractNumber, src => src.MapFrom(x => x.Project.Contract.ContractNumber))
                .ForMember(dest => dest.AssignmentFirstVisitDate, src => src.MapFrom(x => x.FirstVisitTimesheetStartDate))
                .ForMember(dest => dest.IsAssignmentCustomerFormatReportRequired, src => src.MapFrom(x => x.IsCustomerFormatReportRequired))
                .ForMember(dest => dest.AssignmentOperatingCompany, src => src.MapFrom(x => x.OperatingCompany.Name))
                .ForMember(dest => dest.AssignmentOperatingCompanyCoordinator, src => src.MapFrom(x => x.OperatingCompanyCoordinator.Name))
                .ForMember(dest => dest.AssignmentProjectName, src => src.MapFrom(x => x.Project.CustomerProjectName))
                .ForMember(dest => dest.AssignmentProjectNumber, src => src.MapFrom(x => x.Project.ProjectNumber))
                .ForMember(dest => dest.AssignmentStatus, src => src.MapFrom(x => x.AssignmentStatus))
                .ForMember(dest => dest.AssignmentSupplierName, src => src.MapFrom(x => x.SupplierPurchaseOrder.Supplier.SupplierName))
                .ForMember(dest => dest.AssignmentSupplierPurchaseOrderId, src => src.MapFrom(x => x.SupplierPurchaseOrderId))
                .ForMember(dest => dest.AssignmentSupplierPurchaseOrderNumber, src => src.MapFrom(x => x.SupplierPurchaseOrder.SupplierPonumber))
                .ForMember(dest => dest.AssignmentSupplierPoMaterial, src => src.MapFrom(x => x.SupplierPurchaseOrder.MaterialDescription))
                .ForMember(dest => dest.AssignmentCustomerName, src => src.MapFrom(x => x.Project.Contract.Customer.Name))
                .ForMember(dest => dest.TechSpecialists, src => src.MapFrom(x => x.AssignmentTechnicalSpecialist))
                .ForAllOtherMembers(src => src.Ignore());
            #endregion

            #region AssignmentVisitTimesheet
            CreateMap<DbModel.Assignment, DomineModel.AssignmentVisitTimesheet>()
                 .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.Id))
                 .ForMember(dest => dest.AssignmentNumber, src => src.MapFrom(x => x.AssignmentNumber))
                 .ForMember(dest => dest.AssignmentCustomerProjectName, src => src.MapFrom(x => x.Project.CustomerProjectName))
                 .ForMember(dest => dest.AssignmentCreatedDate, src => src.MapFrom(x => x.CreatedDate))
                 .ForMember(dest => dest.AssignmentSupplierPurchaseOrderId, src => src.MapFrom(x => x.SupplierPurchaseOrder.Id))
                 .ForMember(dest => dest.AssignmentSupplierId, src => src.MapFrom(x => x.SupplierPurchaseOrder.SupplierId))
                 .ForMember(dest => dest.AssignmentSupplierPurchaseOrderNumber, src => src.MapFrom(x => x.SupplierPurchaseOrder.SupplierPonumber))
                 .ForMember(dest => dest.AssignmentContractHoldingCompany, src => src.MapFrom(x => x.ContractCompany.Name))
                 .ForMember(dest => dest.AssignmentContractHoldingCompanyCode, src => src.MapFrom(x => x.ContractCompany.Code))
                 .ForMember(dest => dest.AssignmentContractHoldingCompanyCoordinator, src => src.MapFrom(x => x.ContractCompanyCoordinator.Name))
                 .ForMember(dest => dest.AssignmentContractHoldingCompanyCoordinatorCode, src => src.MapFrom(x => x.ContractCompanyCoordinator.SamaccountName))
                 .ForMember(dest => dest.IsContractHoldingCompanyActive, src => src.MapFrom(x => x.ContractCompany.IsActive)) // ITK D - 619
                 .ForMember(dest => dest.AssignmentContractNumber, src => src.MapFrom(x => x.Project.Contract.ContractNumber))
                 .ForMember(dest => dest.AssignmentCustomerCode, src => src.MapFrom(x => x.Project.Contract.Customer.Code))
                 .ForMember(dest => dest.AssignmentCustomerName, src => src.MapFrom(x => x.Project.Contract.Customer.Name))
                 .ForMember(dest => dest.AssignmentOperatingCompanyCode, src => src.MapFrom(x => x.OperatingCompany.Code))
                 .ForMember(dest => dest.AssignmentOperatingCompany, src => src.MapFrom(x => x.OperatingCompany.Name))
                 .ForMember(dest => dest.AssignmentOperatingCompanyCoordinator, src => src.MapFrom(x => x.OperatingCompanyCoordinator.Name))
                 .ForMember(dest => dest.AssignmentOperatingCompanyCoordinatorCode, src => src.MapFrom(x => x.OperatingCompanyCoordinator.SamaccountName))
                 .ForMember(dest => dest.AssignmentProjectNumber, src => src.MapFrom(x => x.Project.ProjectNumber))
                 .ForMember(dest => dest.AssignmentProjectWorkFlow, src => src.MapFrom(x => x.Project.WorkFlowType))
                 .ForMember(dest => dest.AssignmentProjectBusinessUnit, src => src.MapFrom(x => x.Project.ProjectType.Name))
                 .ForMember(dest => dest.ClientReportingRequirements, src => src.MapFrom(x => x.AssignmentMessage
                 .FirstOrDefault(x1 => x1.AssignmentId == x.Id
                                              && x1.MessageTypeId ==
                                              Convert.ToInt16(AssignmentMessageType.ReportingRequirements)
                                              && x1.IsActive == true).Message))
                 .ForMember(dest => dest.ProjectInvoiceInstructionNotes, src => src.MapFrom(x => x.Project.ProjectMessage
                                                                            .FirstOrDefault(m => m.ProjectId == x.ProjectId
                                                                                                    && m.MessageTypeId == Convert.ToInt16(ProjectMessageType.InvoiceNotes))
                                                                            .Message))
                 .ForMember(dest => dest.IsExtranetSummaryReportVisible, src => src.MapFrom(x => x.Project.IsExtranetSummaryVisibleToClient))
                 .ForAllOtherMembers(src => src.Ignore());
            #endregion

            #region  DB Assignment to Domain Assignment 
            CreateMap<DbModel.Assignment, DomineModel.Assignment>()
                .ForMember(dest => dest.AssignmentCompanyAddress, src => src.MapFrom(x => x.AssignmentCompanyAddress.Address))
                .ForMember(dest => dest.IsAssignmentCompleted, src => src.MapFrom(x => x.IsAssignmentComplete))
                .ForMember(dest => dest.AssignmentCreatedDate, src => src.MapFrom(x => x.CreatedDate))
                .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.AssignmentLifecycle, src => src.MapFrom(x => x.AssignmentLifecycle.Name))
                .ForMember(dest => dest.AssignmentNumber, src => src.MapFrom(x => x.AssignmentNumber))
                .ForMember(dest => dest.AssignmentReference, src => src.MapFrom(x => x.AssignmentReference))
                .ForMember(dest => dest.AssignmentCustomerCode, src => src.MapFrom(x => x.CustomerAssignmentContact.CustomerAddress.Customer.Code))
                .ForMember(dest => dest.AssignmentCustomerName, src => src.MapFrom(x => x.CustomerAssignmentContact.CustomerAddress.Customer.Name))
                .ForMember(dest => dest.AssignmentType, src => src.MapFrom(x => x.AssignmentType))
                .ForMember(dest => dest.AssignmentBudgetHours, src => src.MapFrom(x => x.BudgetHours))
                .ForMember(dest => dest.AssignmentBudgetHoursWarning, src => src.MapFrom(x => x.BudgetHoursWarning))
                .ForMember(dest => dest.AssignmentBudgetValue, src => src.MapFrom(x => x.BudgetValue))
                .ForMember(dest => dest.AssignmentBudgetWarning, src => src.MapFrom(x => x.BudgetWarning))
                .ForMember(dest => dest.AssignmentContractHoldingCompany, src => src.MapFrom(x => x.ContractCompany.Name))
                .ForMember(dest => dest.AssignmentContractHoldingCompanyCode, src => src.MapFrom(x => x.ContractCompany.Code))
                .ForMember(dest => dest.IsContractHoldingCompanyActive, src => src.MapFrom(x => x.ContractCompany.IsActive)) // ITK D - 619
                .ForMember(dest => dest.AssignmentContractHoldingCompanyCoordinator, src => src.MapFrom(x => x.ContractCompanyCoordinator.Name))
                .ForMember(dest => dest.AssignmentContractHoldingCompanyCoordinatorCode, src => src.MapFrom(x => x.ContractCompanyCoordinator.SamaccountName))
                .ForMember(dest => dest.AssignmentContractNumber, src => src.MapFrom(x => x.Project.Contract.ContractNumber))
                .ForMember(dest => dest.AssignmentContractId, src => src.MapFrom(x => x.Project.ContractId))
                .ForMember(dest => dest.AssignmentContractType, src => src.MapFrom(x => x.Project.Contract.ContractType))
                .ForMember(dest => dest.AssignmentParentContractCompany, src => src.MapFrom(x => x.Project.Contract.ParentContract.ContractHolderCompany.Name))
                .ForMember(dest => dest.AssignmentParentContractCompanyCode, src => src.MapFrom(x => x.Project.Contract.ParentContract.ContractHolderCompany.Code))
                .ForMember(dest => dest.AssignmentParentContractDiscount, src => src.MapFrom(x => x.Project.Contract.ParentContractDiscountPercentage))
                .ForMember(dest => dest.AssignmentCustomerAssigmentContact, src => src.MapFrom(x => x.CustomerAssignmentContact.ContactName))
                .ForMember(dest => dest.IsAssignmentCustomerFormatReportRequired, src => src.MapFrom(x => x.IsCustomerFormatReportRequired))
                .ForMember(dest => dest.AssignmentCustomerName, src => src.MapFrom(x => x.Project.Contract.Customer.Name))
                .ForMember(dest => dest.AssignmentOperatingCompanyCode, src => src.MapFrom(x => x.OperatingCompany.Code))
                .ForMember(dest => dest.AssignmentOperatingCompany, src => src.MapFrom(x => x.OperatingCompany.Name))
                .ForMember(dest => dest.IsOperatingCompanyActive, src => src.MapFrom(x => x.OperatingCompany.IsActive)) // ITK D - 619
                .ForMember(dest => dest.AssignmentOperatingCompanyCoordinator, src => src.MapFrom(x => x.OperatingCompanyCoordinator.Name))
                .ForMember(dest => dest.AssignmentOperatingCompanyCoordinatorCode, src => src.MapFrom(x => x.OperatingCompanyCoordinator.SamaccountName))
                .ForMember(dest => dest.AssignmentHostCompanyCode, src => src.MapFrom(x => x.HostCompany.Code))
                .ForMember(dest => dest.AssignmentHostCompany, src => src.MapFrom(x => x.HostCompany.Name))
                .ForMember(dest => dest.AssignmentProjectName, src => src.MapFrom(x => x.Project.CustomerProjectName))
                .ForMember(dest => dest.AssignmentProjectNumber, src => src.MapFrom(x => x.Project.ProjectNumber))
                .ForMember(dest => dest.AssignmentProjectWorkFlow, src => src.MapFrom(x => x.Project.WorkFlowType))
                .ForMember(dest => dest.AssignmentProjectBusinessUnit, src => src.MapFrom(x => x.Project.ProjectType.Name))
                .ForMember(dest => dest.AssignmentCustomerProjectNumber, src => src.MapFrom(x => x.Project.CustomerProjectNumber))
                .ForMember(dest => dest.AssignmentCustomerProjectName, src => src.MapFrom(x => x.Project.CustomerProjectName))
                .ForMember(dest => dest.AssignmentStatus, src => src.MapFrom(x => x.AssignmentStatus))
                .ForMember(dest => dest.AssignmentSupplierName, src => src.MapFrom(x => x.SupplierPurchaseOrder.Supplier.SupplierName))
                .ForMember(dest => dest.AssignmentSupplierId, src => src.MapFrom(x => x.SupplierPurchaseOrder.SupplierId))
                .ForMember(dest => dest.AssignmentSupplierPurchaseOrderId, src => src.MapFrom(x => x.SupplierPurchaseOrderId))
                .ForMember(dest => dest.AssignmentSupplierPurchaseOrderNumber, src => src.MapFrom(x => x.SupplierPurchaseOrder.SupplierPonumber))
                .ForMember(dest => dest.AssignmentSupplierPoMaterial, src => src.MapFrom(x => x.SupplierPurchaseOrder.MaterialDescription))
                .ForMember(dest => dest.AssignmentReviewAndModerationProcess, src => src.MapFrom(x => x.ReviewAndModerationProcess.Name))

                .ForMember(dest => dest.ClientReportingRequirements, src => src.MapFrom(x => x.AssignmentMessage
                        .FirstOrDefault(x1 => x1.AssignmentId == x.Id
                                              && x1.MessageTypeId ==
                                              Convert.ToInt16(AssignmentMessageType.ReportingRequirements)
                                              && x1.IsActive == true)
                .Message))
                .ForMember(dest => dest.WorkLocationCountryId, src => src.MapFrom(x => x.OperationCompanyCountryId)) //Added for ITK D1536
                .ForMember(dest => dest.WorkLocationCountyId, src => src.MapFrom(x => x.OperatingCompanyCountyId)) //Added for ITK D1536
                .ForMember(dest => dest.WorkLocationCityId, src => src.MapFrom(x => x.OperatingCompanyLocationId)) //Added for ITK D1536
                .ForMember(dest => dest.WorkLocationCountry, src => src.MapFrom(x => x.OperationCompanyCountry.Name)) //Added for ITK D1536
                .ForMember(dest => dest.WorkLocationCounty, src => src.MapFrom(x => x.OperatingCompanyCounty.Name)) //Added for ITK D1536
                .ForMember(dest => dest.WorkLocationCity, src => src.MapFrom(x => x.OperatingCompanyLocation.Name)) //Added for ITK D1536
                .ForMember(dest => dest.WorkLocationPinCode, src => src.MapFrom(x => x.OperatingCompanyPinCode))

                .ForMember(dest => dest.VisitFromDate, src => src.MapFrom(x => x.Visit.Count > 0 ? x.Visit.Where(x1 => x1.AssignmentId == x.Id).Select(x1=> new { x1.AssignmentId, x1.FromDate, x1.ToDate, x1.VisitStatus }).GroupBy(t => t.AssignmentId).Select(x1 => x1.Min(x2 => x2.FromDate)).FirstOrDefault() : x.FirstVisitTimesheetStartDate))
                .ForMember(dest => dest.VisitToDate, src => src.MapFrom(x => x.Visit.Count > 0 ? x.Visit.Where(x1 => x1.AssignmentId == x.Id).Select(x1 => new { x1.AssignmentId, x1.FromDate, x1.ToDate, x1.VisitStatus }).GroupBy(t => t.AssignmentId).Select(x1 => x1.Min(x2 => x2.ToDate)).FirstOrDefault() : x.FirstVisitTimesheetEndDate))
                .ForMember(dest => dest.VisitStatus, src => src.MapFrom(x => x.Visit.Count > 0 ? x.Visit.Where(x1 => x1.AssignmentId == x.Id).Select(x1 => new { x1.AssignmentId, x1.FromDate, x1.ToDate, x1.VisitStatus }).GroupBy(t => t.AssignmentId).Select(x2 => x2.FirstOrDefault(x3 => x3.FromDate == x2.Min(x4 => x4.FromDate)).VisitStatus).FirstOrDefault() : x.FirstVisitTimesheetStatus))

                .ForMember(dest => dest.TimesheetFromDate, src => src.MapFrom(x => x.Timesheet.Count > 0 ? x.Timesheet.Where(x1 => x1.AssignmentId == x.Id).Select(x1 => new { x1.AssignmentId, x1.FromDate, x1.ToDate, x1.TimesheetStatus }).GroupBy(t => t.AssignmentId).Select(x1 => x1.Min(x2 => x2.FromDate)).FirstOrDefault() : x.FirstVisitTimesheetStartDate))
                .ForMember(dest => dest.TimesheetToDate, src => src.MapFrom(x => x.Timesheet.Count > 0 ? x.Timesheet.Where(x1 => x1.AssignmentId == x.Id).Select(x1 => new { x1.AssignmentId, x1.FromDate, x1.ToDate, x1.TimesheetStatus }).GroupBy(t => t.AssignmentId).Select(x1 => x1.Min(x2 => x2.ToDate)).FirstOrDefault() : x.FirstVisitTimesheetEndDate))
                .ForMember(dest => dest.TimesheetStatus, src => src.MapFrom(x => x.Timesheet.Count > 0 ? x.Timesheet.Where(x1 => x1.AssignmentId == x.Id).Select(x1 => new { x1.AssignmentId, x1.FromDate, x1.ToDate, x1.TimesheetStatus }).GroupBy(t => t.AssignmentId).Select(x2 => x2.FirstOrDefault(x3 => x3.FromDate == x2.Min(x4 => x4.FromDate)).TimesheetStatus).FirstOrDefault() : x.FirstVisitTimesheetStatus))

                .ForMember(dest => dest.FirstVisitTimesheetStartDate, src => src.MapFrom(x => x.FirstVisitTimesheetStartDate))
                .ForMember(dest => dest.FirstVisitTimesheetEndDate, src => src.MapFrom(x => x.FirstVisitTimesheetEndDate))

                //.ForMember(dest => dest.VisitFromDate, src => src.MapFrom(x => x.SupplierPurchaseOrderId != null && x.SupplierPurchaseOrderId > 0 ? x.FirstVisitTimesheetStartDate : null))
                //.ForMember(dest => dest.VisitToDate, src => src.MapFrom(x => x.SupplierPurchaseOrderId != null && x.SupplierPurchaseOrderId > 0 ? x.FirstVisitTimesheetEndDate : null))
                //.ForMember(dest => dest.VisitStatus, src => src.MapFrom(x => x.SupplierPurchaseOrderId != null && x.SupplierPurchaseOrderId > 0 ? x.FirstVisitTimesheetStatus : null))
                //.ForMember(dest => dest.TimesheetFromDate, src => src.MapFrom(x => x.SupplierPurchaseOrderId == null ? x.FirstVisitTimesheetStartDate : null))
                //.ForMember(dest => dest.TimesheetToDate, src => src.MapFrom(x => x.SupplierPurchaseOrderId == null ? x.FirstVisitTimesheetEndDate : null))
                //.ForMember(dest => dest.TimesheetStatus, src => src.MapFrom(x => x.SupplierPurchaseOrderId == null ? x.FirstVisitTimesheetStatus : null))

                .ForMember(dest => dest.AssignmentBudgetCurrency, src => src.MapFrom(x => x.Project.Contract.BudgetCurrency))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => Convert.ToInt32(x.UpdateCount)))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.ResourceSearchId, src => src.MapFrom(x => x.ResourceSearchId))
                .ForMember(dest => dest.ProjectInvoiceInstructionNotes, src => src.MapFrom(x => x.Project.ProjectMessage
                                                                                              .FirstOrDefault(m => m.ProjectId == x.ProjectId
                                                                                                                     && m.MessageTypeId == Convert.ToInt16(ProjectMessageType.InvoiceNotes))
                                                                                              .Message))
                .ForMember(dest => dest.IsExtranetSummaryReportVisible, src => src.MapFrom(x => x.Project.IsExtranetSummaryVisibleToClient))
                .ForMember(dest => dest.IsVisitOnPopUp, src => src.MapFrom(x => x.Project.IsVisitOnPopUp)) //IGO QC 864 - client pop in visit/timesheet
                .ForMember(dest => dest.IsFirstVisit, src => src.MapFrom(x => x.IsFirstVisit))
                .ForMember(dest => dest.IsOverrideOrPLO, src => src.MapFrom(x => x.IsOverrideOrPlo))
                .ForMember(dest => dest.TechSpecialists, src => src.MapFrom(x => x.AssignmentTechnicalSpecialist))
                .ForAllOtherMembers(src => src.Ignore());
            #endregion

            CreateMap<DbModel.AssignmentTechnicalSpecialist, DomineModel.TechnicalSpecialist>()
             .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
             .ForMember(dest => dest.FirstName, src => src.MapFrom(x => x.TechnicalSpecialist.FirstName))
             .ForMember(dest => dest.LastName, src => src.MapFrom(x => x.TechnicalSpecialist.LastName))
             .ForMember(dest => dest.Pin, src => src.MapFrom(x => x.TechnicalSpecialist.Pin))
             .ForMember(dest => dest.ProfileStatus, src => src.MapFrom(x => x.TechnicalSpecialist.ProfileStatus.Name))
             .ForAllOtherMembers(src => src.Ignore());

            #region Domain Assignment to Db Assignment  
            CreateMap<DomineModel.Assignment, DbModel.Assignment>()
               .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.AssignmentId : null) : src.AssignmentId)))
               .ForMember(dest => dest.AssignmentNumber, opt => opt.ResolveUsing((src, dst, arg3, context) =>
                   context.Options.Items.ContainsKey("isAssignmentNumber") ? Convert.ToBoolean(context.Options.Items["isAssignmentNumber"]) == true ? context.Options.Items["AssignmentNumberSequence"]
                   : src.AssignmentNumber
                   : src.AssignmentNumber)
               )
               //.ForMember(dest => dest.AssignmentNumber, opt => opt.ResolveUsing((src, dst, arg3, context) =>
               //    context.Options.Items.ContainsKey("isAssignmentNumber") ?
               //    Convert.ToBoolean(context.Options.Items["isAssignmentNumber"]) == true ?
               //    ((List<DbModel.NumberSequence>)context.Options.Items["AssignmentNumberSequence"])?.FirstOrDefault(x => x.LastSequenceNumber >= 0)?.LastSequenceNumber
               //    : src.AssignmentNumber
               //    : src.AssignmentNumber)
               //)
               //.ForMember(dest => dest.AssignmentNumber, src => src.MapFrom(x => x.AssignmentNumber))
               .ForMember(dest => dest.AssignmentCompanyAddressId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentCompanyAddress") ? ((List<DbModel.CustomerAddress>)context.Options.Items["AssignmentCompanyAddress"])?.FirstOrDefault(x => x.Address == src.AssignmentCompanyAddress)?.Id : null))
               .ForMember(dest => dest.IsAssignmentComplete, src => src.MapFrom(x => x.IsAssignmentCompleted))
               .ForMember(dest => dest.CreatedDate, src => src.MapFrom(x => x.AssignmentCreatedDate))
               .ForMember(dest => dest.AssignmentLifecycleId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentLifeCycle") ? ((List<DbModel.Data>)context.Options.Items["AssignmentLifeCycle"])?.FirstOrDefault(x => x.Name == src.AssignmentLifecycle)?.Id : null))
               .ForMember(dest => dest.AssignmentReference, src => src.MapFrom(x => x.AssignmentReference))
               .ForMember(dest => dest.AssignmentType, src => src.MapFrom(x => x.AssignmentType))
               .ForMember(dest => dest.BudgetHours, src => src.MapFrom(x => x.AssignmentBudgetHours))
               .ForMember(dest => dest.BudgetHoursWarning, src => src.MapFrom(x => x.AssignmentBudgetHoursWarning))
               .ForMember(dest => dest.BudgetValue, src => src.MapFrom(x => x.AssignmentBudgetValue))
               .ForMember(dest => dest.BudgetWarning, src => src.MapFrom(x => x.AssignmentBudgetWarning))
               .ForMember(dest => dest.ContractCompanyId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentContractHoldingCompany") ? ((IList<DbModel.Contract>)context.Options.Items["AssignmentContractHoldingCompany"])?.FirstOrDefault()?.ContractHolderCompanyId : null))
               .ForMember(dest => dest.ContractCompanyCoordinatorId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentContractHoldingCoordinator") ? ((List<DbModel.User>)context.Options.Items["AssignmentContractHoldingCoordinator"])?.FirstOrDefault(x => x.SamaccountName == src.AssignmentContractHoldingCompanyCoordinatorCode)?.Id : null))
               .ForMember(dest => dest.CustomerAssignmentContactId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentCustomerContact") ? ((List<DbModel.CustomerContact>)context.Options.Items["AssignmentCustomerContact"])?.FirstOrDefault(x => x.ContactName == src.AssignmentCustomerAssigmentContact)?.Id : null))
               .ForMember(dest => dest.IsCustomerFormatReportRequired, src => src.MapFrom(x => x.IsAssignmentCustomerFormatReportRequired))
               .ForMember(dest => dest.OperatingCompanyId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentOperatingCompany") ? ((List<DbModel.Company>)context.Options.Items["AssignmentOperatingCompany"])?.FirstOrDefault(x => x.Code == src.AssignmentOperatingCompanyCode)?.Id : null))
               .ForMember(dest => dest.OperatingCompanyCoordinatorId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentOperatingCompanyCoordinator") ? ((List<DbModel.User>)context.Options.Items["AssignmentOperatingCompanyCoordinator"])?.FirstOrDefault(x => x.SamaccountName == src.AssignmentOperatingCompanyCoordinatorCode)?.Id : null))
               .ForMember(dest => dest.HostCompanyId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentHostCompany") ? ((List<DbModel.Company>)context.Options.Items["AssignmentHostCompany"])?.FirstOrDefault(x => x.Code == src.AssignmentHostCompanyCode)?.Id : null))
               .ForMember(dest => dest.ProjectId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentProjectNumber") ? ((List<DbModel.Project>)context.Options.Items["AssignmentProjectNumber"])?.FirstOrDefault(x => x.ProjectNumber == src.AssignmentProjectNumber)?.Id : null))
               .ForMember(dest => dest.AssignmentStatus, src => src.MapFrom(x => x.AssignmentStatus))
               .ForMember(dest => dest.SupplierPurchaseOrderId, src => src.MapFrom(x => x.AssignmentSupplierPurchaseOrderId))
               .ForMember(dest => dest.ReviewAndModerationProcessId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentReview") ? ((List<DbModel.Data>)context.Options.Items["AssignmentReview"])?.FirstOrDefault(x => x.Name == src.AssignmentReviewAndModerationProcess)?.Id : null))
               .ForMember(dest => dest.AssignmentMessage, src => src.ResolveUsing<AssignmentMessageResolver>())
               .ForMember(dest => dest.OperationCompanyCountryId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentWorkCountry") ? ((List<DbModel.Country>)context.Options.Items["AssignmentWorkCountry"])?.FirstOrDefault(x => x.Id == src.WorkLocationCountryId)?.Id : null))
               .ForMember(dest => dest.OperatingCompanyCountyId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentWorkCounty") ? ((List<DbModel.County>)context.Options.Items["AssignmentWorkCounty"])?.FirstOrDefault(x => x.Id == src.WorkLocationCountyId)?.Id : null))
               .ForMember(dest => dest.OperatingCompanyLocationId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentWorkCity") ? ((List<DbModel.City>)context.Options.Items["AssignmentWorkCity"])?.FirstOrDefault(x => x.Id == src.WorkLocationCityId && x.County.Id == src.WorkLocationCountyId)?.Id : null)) //Changes for state-city issue
               .ForMember(dest => dest.OperatingCompanyPinCode, src => src.MapFrom(x => x.WorkLocationPinCode))
               .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => Convert.ToInt32(x.UpdateCount)))
               .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
               .ForMember(dest => dest.FirstVisitTimesheetStartDate, src => src.MapFrom(x => (x.IsFirstVisit && x.FirstVisitTimesheetStartDate.HasValue) ? x.FirstVisitTimesheetStartDate : (x.VisitFromDate.HasValue ? x.VisitFromDate : x.TimesheetFromDate)))
               .ForMember(dest => dest.FirstVisitTimesheetEndDate, src => src.MapFrom(x => (x.IsFirstVisit && x.FirstVisitTimesheetEndDate.HasValue) ? x.FirstVisitTimesheetEndDate : (x.VisitToDate.HasValue ? x.VisitToDate : x.TimesheetToDate)))
               //.ForMember(dest => dest.FirstVisitTimesheetStatus, src => src.MapFrom(x => x.VisitFromDate.HasValue ? !string.IsNullOrEmpty(x.VisitStatus) ? x.VisitStatus : "T" : !string.IsNullOrEmpty(x.TimesheetStatus) ? x.TimesheetStatus : "N"))
               .ForMember(dest => dest.FirstVisitTimesheetStatus, src => src.MapFrom(x => x.AssignmentProjectWorkFlow.Trim().IsVisitEntry() ? string.IsNullOrEmpty(x.VisitStatus) ? "T" : x.VisitStatus : string.IsNullOrEmpty(x.TimesheetStatus) ? "N" : x.TimesheetStatus))
               .ForMember(dest => dest.IsFirstVisit, src => src.MapFrom(x => x.VisitFromDate.HasValue ? true : x.TimesheetFromDate.HasValue ? true : false))
               .ForMember(dest => dest.IsOverrideOrPlo, src => src.MapFrom(x => x.IsOverrideOrPLO))
               .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
               .ForMember(dest => dest.ResourceSearchId, src => src.MapFrom(x => x.ResourceSearchId))
               .ForMember(dest => dest.IsInternalAssignment, src => src.MapFrom(x => x.IsInternalAssignment))
               .ForAllOtherMembers(src => src.Ignore());

            #endregion

            #region InvoiceInfo to Budget

            CreateMap<InvoicedInfo, Budget>()
                .ForMember(dest => dest.ContractId, src => src.MapFrom(x => x.ContractId))
                .ForMember(dest => dest.ContractNumber, src => src.MapFrom(x => x.ContractNumber))
                .ForMember(dest => dest.ProjectId, src => src.MapFrom(x => x.ProjectId))
                .ForMember(dest => dest.ProjectNumber, src => src.MapFrom(x => x.ProjectNumber))
                .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.AssignmentNumber, src => src.MapFrom(x => x.AssignmentNumber))
                .ForMember(dest => dest.InvoicedToDate, src => src.MapFrom(x => x.InvoicedToDate))
                .ForMember(dest => dest.UninvoicedToDate, src => src.MapFrom(x => x.UninvoicedToDate))
                .ForMember(dest => dest.HoursInvoicedToDate, src => src.MapFrom(x => x.HoursInvoicedToDate))
                .ForMember(dest => dest.HoursUninvoicedToDate, src => src.MapFrom(x => x.HoursUninvoicedToDate))
                .ForAllOtherMembers(src => src.Ignore());

            #endregion

            #region AssignmentContractSchedule 

            CreateMap<DbModel.AssignmentContractSchedule, DomineModel.AssignmentContractRateSchedule>()
             .ForMember(dest => dest.AssignmentContractRateScheduleId, src => src.MapFrom(x => x.Id))
             .ForMember(dest => dest.ContractScheduleId, src => src.MapFrom(x => x.ContractScheduleId))
             .ForMember(dest => dest.ContractScheduleName, src => src.MapFrom(x => x.ContractSchedule.Name))
             .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
             .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
             .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
             .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
             .ForAllOtherMembers(src => src.Ignore());


            CreateMap<DomineModel.AssignmentContractRateSchedule, DbModel.AssignmentContractSchedule>()
             .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.AssignmentContractRateScheduleId : null) : src.AssignmentContractRateScheduleId)))
             .ForMember(dest => dest.ContractScheduleId, opt => opt.MapFrom(src => src.ContractScheduleId))
               .ForMember(dest => dest.AssignmentId, opt => opt.MapFrom(src => src.AssignmentId))
             .ForMember(dest => dest.UpdateCount, opt => opt.MapFrom(src => src.UpdateCount))
             .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
             .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
             .ForAllOtherMembers(opt => opt.Ignore());

            #endregion

            #region AssignmentTechnicalSpecialistSchedule

            CreateMap<DbModel.AssignmentTechnicalSpecialistSchedule, DomineModel.AssignmentTechnicalSpecialistSchedule>()
             .ForMember(dest => dest.AssignmentTechnicalSpecialistScheduleId, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.ContractScheduleId, opt => opt.MapFrom(src => src.ContractChargeScheduleId))
             .ForMember(dest => dest.ContractScheduleName, opt => opt.MapFrom(src => src.ContractChargeSchedule.Name))
             .ForMember(dest => dest.AssignmentTechnicalSpecilaistId, opt => opt.MapFrom(src => src.AssignmentTechnicalSpecialistId))
             //.ForMember(dest => dest.TechnicalSpecialistEpin, opt => opt.MapFrom(src => src.AssignmentTechnicalSpecialist.TechnicalSpecialist.Pin))
             //.ForMember(dest => dest.TechnicalSpecialistName, opt => opt.MapFrom(src => string.Format("{0} {1} {2}", src.AssignmentTechnicalSpecialist.TechnicalSpecialist.LastName, src.AssignmentTechnicalSpecialist.TechnicalSpecialist.MiddleName, src.AssignmentTechnicalSpecialist.TechnicalSpecialist.FirstName)))
             .ForMember(dest => dest.TechnicalSpecialistPayScheduleId, opt => opt.MapFrom(src => src.TechnicalSpecialistPayScheduleId))
             .ForMember(dest => dest.TechnicalSpecialistPayScheduleName, opt => opt.MapFrom(src => src.TechnicalSpecialistPaySchedule.PayScheduleName))
             .ForMember(dest => dest.ScheduleNoteToPrintOnInvoice, opt => opt.MapFrom(src => src.ScheduleNoteToPrintOnInvoice))
             .ForMember(dest => dest.UpdateCount, opt => opt.MapFrom(src => src.UpdateCount))
             .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
             .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
             .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DomineModel.AssignmentTechnicalSpecialistSchedule, DbModel.AssignmentTechnicalSpecialistSchedule>()
            .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.AssignmentTechnicalSpecialistScheduleId : null) : src.AssignmentTechnicalSpecialistScheduleId)))
            .ForMember(dest => dest.ContractChargeScheduleId, opt => opt.MapFrom(src => src.ContractScheduleId))
            .ForMember(dest => dest.AssignmentTechnicalSpecialistId, opt => opt.MapFrom(src => src.AssignmentTechnicalSpecilaistId))
            .ForMember(dest => dest.TechnicalSpecialistPayScheduleId, opt => opt.MapFrom(src => src.TechnicalSpecialistPayScheduleId))
            .ForMember(dest => dest.ScheduleNoteToPrintOnInvoice, opt => opt.MapFrom(src => src.ScheduleNoteToPrintOnInvoice))

            .ForMember(dest => dest.UpdateCount, opt => opt.MapFrom(src => src.UpdateCount))
            .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
            .ForAllOtherMembers(opt => opt.Ignore());

            #endregion

            #region Assignment Additional Expense
            CreateMap<DbModel.AssignmentAdditionalExpense, DomineModel.AssignmentAdditionalExpense>()
                .ForMember(dest => dest.AssignmentAdditionalExpenseId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.CompanyCode, src => src.MapFrom(x => x.Company.Code))
                .ForMember(dest => dest.CompanyName, src => src.MapFrom(x => x.Company.Name))
                .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Description))
                .ForMember(dest => dest.CurrencyCode, src => src.MapFrom(x => x.Currency))
                .ForMember(dest => dest.ExpenseType, src => src.MapFrom(x => x.ExpenseType.Name))
                .ForMember(dest => dest.PerUnitRate, src => src.MapFrom(x => x.Rate))
                .ForMember(dest => dest.TotalUnit, src => src.MapFrom(x => x.TotalUnit))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.IsAlreadyLinked, src => src.MapFrom(x => (x.IsAlreadyLinked==null)?false: x.IsAlreadyLinked)) //For Audit
               .ForAllOtherMembers(src => src.Ignore());

            CreateMap<DomineModel.AssignmentAdditionalExpense, DbModel.AssignmentAdditionalExpense>()
            .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.AssignmentAdditionalExpenseId : null) : src.AssignmentAdditionalExpenseId)))
            .ForMember(dest => dest.AssignmentId, opt => opt.MapFrom(src => src.AssignmentId))
            .ForMember(dest => dest.ExpenseTypeId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("ExpenseTypes") ? ((List<DbModel.Data>)context.Options.Items["ExpenseTypes"])?.FirstOrDefault(x => x.Name == src.ExpenseType)?.Id : null))
            .ForMember(dest => dest.CompanyId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("CompanyCodes") ? ((List<DbModel.Company>)context.Options.Items["CompanyCodes"])?.FirstOrDefault(x => x.Code == src.CompanyCode)?.Id : null))
            .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Description))
            .ForMember(dest => dest.Currency, src => src.MapFrom(x => x.CurrencyCode))
            .ForMember(dest => dest.TotalUnit, src => src.MapFrom(x => x.TotalUnit))
            .ForMember(dest => dest.Rate, src => src.MapFrom(x => x.PerUnitRate))
            .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => Convert.ToInt32(x.UpdateCount)))
            .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
            .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
            .ForMember(dest => dest.IsAlreadyLinked, src => src.MapFrom(x => x.IsAlreadyLinked))
            .ForAllOtherMembers(opt => opt.Ignore());
            #endregion

            #region AssignmentTechnicalSpecialist
            CreateMap<DbModel.AssignmentTechnicalSpecialist, DomineModel.AssignmentTechnicalSpecialist>()
             .ForMember(dest => dest.AssignmentTechnicalSplId, src => src.MapFrom(x => x.Id))
             .ForMember(dest => dest.Epin, src => src.MapFrom(x => x.TechnicalSpecialistId))
             .ForMember(dest => dest.TechnicalSpecialistName, src => src.MapFrom(x => string.Format("{0}, {1}", x.TechnicalSpecialist.LastName, x.TechnicalSpecialist.FirstName))) // ITK D-710
             .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
             .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
             .ForMember(dest => dest.IsSupervisor, src => src.MapFrom(x => x.IsSupervisor))
             .ForMember(dest => dest.ProfileStatus, src => src.MapFrom(x => x.TechnicalSpecialist.ProfileStatus.Name))
             .ForMember(dest => dest.CreatedOn, src => src.MapFrom(x => x.CreatedDate))
             .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
             .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
             .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
             .ForMember(dest => dest.AssignmentTechnicalSpecialistSchedules, src => src.MapFrom(x => x.AssignmentTechnicalSpecialistSchedule))
             .ReverseMap()
             .ForAllOtherMembers(src => src.Ignore());
            #endregion

            #region AssignmentReference
            CreateMap<DbModel.AssignmentReference, DomineModel.AssignmentReferenceType>()
              .ForMember(dest => dest.AssignmentReferenceTypeId, src => src.MapFrom(x => x.Id))
              .ForMember(dest => dest.ReferenceType, src => src.MapFrom(x => x.AssignmentReferenceType.Name))
              .ForMember(dest => dest.ReferenceValue, src => src.MapFrom(x => x.ReferenceValue))
              .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
             .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
             .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
             .ForMember(dest => dest.AssignmentReferenceTypeMasterId, src => src.MapFrom(x => x.AssignmentReferenceTypeId)) //For audit to show ReferenceType
             .ForAllOtherMembers(src => src.Ignore());

            CreateMap<DomineModel.AssignmentReferenceType, DbModel.AssignmentReference>()
             .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.AssignmentReferenceTypeId : null) : src.AssignmentReferenceTypeId)))
             .ForMember(dest => dest.AssignmentId, opt => opt.MapFrom(src => src.AssignmentId))
             .ForMember(dest => dest.AssignmentReferenceTypeId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("ReferenceTypes") ? ((List<DbModel.Data>)context.Options.Items["ReferenceTypes"])?.FirstOrDefault(x => x.Name == src.ReferenceType)?.Id : null))
             .ForMember(dest => dest.ReferenceValue, opt => opt.MapFrom(src => src.ReferenceValue))
             .ForMember(dest => dest.UpdateCount, opt => opt.MapFrom(src => src.UpdateCount))
             .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
             .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
             .ForAllOtherMembers(opt => opt.Ignore());
            #endregion

            #region Assignment Instructions

            CreateMap<DbModel.AssignmentMessage, BaseMessage>()
               .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
               .ForMember(dest => dest.MsgIdentifier, src => src.MapFrom(x => x.Identifier))
               .ForMember(dest => dest.MsgType, src => src.MapFrom(x => x.MessageType.Name))
               .ForMember(dest => dest.MsgText, src => src.MapFrom(x => x.Message))
               .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
               .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
               .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
               .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
               .ReverseMap()
               .ForAllOtherMembers(x => x.Ignore());

            CreateMap<List<DbModel.AssignmentMessage>, DomineModel.AssignmentInstructions>()

                //.ForMember(dest => dest.ClientReportingRequirements, src => src.ResolveUsing(new MessageResolver<DbModel.AssignmentMessage, DomineModel.AssignmentInstructions>(AssignmentMessageType.ReportingRequirements.ToString())))
                .ForMember(dest => dest.TechnicalSpecialistInstructions, src => src.ResolveUsing(new MessageResolver<DbModel.AssignmentMessage, DomineModel.AssignmentInstructions>(AssignmentMessageType.OperationalNotes.ToString())))
                .ForMember(dest => dest.InterCompanyInstructions, src => src.ResolveUsing(new MessageResolver<DbModel.AssignmentMessage, DomineModel.AssignmentInstructions>(AssignmentMessageType.InterCompanyInstructions.ToString())))
                .ReverseMap()
                .ForAllOtherMembers(src => src.Ignore());
            #endregion  

            #region AssignmentInterCompany

            CreateMap<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCompanyDiscount>()
                .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.DiscountType, src => src.MapFrom(x => x.DiscountType))
                .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Description))
                .ForMember(dest => dest.DiscountPercentage, src => src.MapFrom(x => x.Percentage))
                .ForMember(dest => dest.CompanyCode, src => src.MapFrom(x => x.Company.Code))
                .ForMember(dest => dest.CompanyName, src => src.MapFrom(x => x.Company.Name))
                .ForMember(dest => dest.AssignmentInterCompanyDiscountId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.AmendmentReason, src => src.MapFrom(x => x.AmendmentReason))
                .ForAllOtherMembers(src => src.Ignore());

            CreateMap<IList<DbModel.AssignmentInterCompanyDiscount>, DomineModel.AssignmentInterCoDiscountInfo>()
                // For Additional InterCompany Office 1
                .ForMember(dest => dest.AssignmentAdditionalIntercompany1_Code, src => src.ResolveUsing(new AssignmentInterCoDiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyCode), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId))))
                .ForMember(dest => dest.AssignmentAdditionalIntercompany1_Description, src => src.ResolveUsing(new AssignmentInterCoDiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.Description), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId))))
                .ForMember(dest => dest.AssignmentAdditionalIntercompany1_Name, src => src.ResolveUsing(new AssignmentInterCoDiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyName), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId))))
                .ForMember(dest => dest.AssignmentAdditionalIntercompany1_Discount, src => src.ResolveUsing(new DiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId))))
                // For Additional InterCompany Office 2
                .ForMember(dest => dest.AssignmentAdditionalIntercompany2_Code, src => src.ResolveUsing(new AssignmentInterCoDiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyCode), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2))))
                .ForMember(dest => dest.AssignmentAdditionalIntercompany2_Description, src => src.ResolveUsing(new AssignmentInterCoDiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.Description), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2))))
                .ForMember(dest => dest.AssignmentAdditionalIntercompany2_Name, src => src.ResolveUsing(new AssignmentInterCoDiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyName), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2))))
                .ForMember(dest => dest.AssignmentAdditionalIntercompany2_Discount, src => src.ResolveUsing(new DiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2))))
                // For Parent Contract Holding Company
                .ForMember(dest => dest.ParentContractHoldingCompanyCode, src => src.ResolveUsing(new AssignmentInterCoDiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyCode), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.ParentContract))))
                .ForMember(dest => dest.ParentContractHoldingCompanyDescription, src => src.ResolveUsing(new AssignmentInterCoDiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.Description), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.ParentContract))))
                .ForMember(dest => dest.ParentContractHoldingCompanyName, src => src.ResolveUsing(new AssignmentInterCoDiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyName), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.ParentContract))))
                .ForMember(dest => dest.ParentContractHoldingCompanyDiscount, src => src.ResolveUsing(new DiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.ParentContract))))
                // For Contract Holding Company Of Assignment
                .ForMember(dest => dest.AssignmentContractHoldingCompanyCode, src => src.ResolveUsing(new AssignmentInterCoDiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyCode), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.Contract))))
                .ForMember(dest => dest.AssignmentContractHoldingCompanyDescription, src => src.ResolveUsing(new AssignmentInterCoDiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.Description), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.Contract))))
                .ForMember(dest => dest.AssignmentContractHoldingCompanyName, src => src.ResolveUsing(new AssignmentInterCoDiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyName), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.Contract))))
                .ForMember(dest => dest.AssignmentContractHoldingCompanyDiscount, src => src.ResolveUsing(new DiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.Contract))))
                .ForMember(dest => dest.AmendmentReason, src => src.ResolveUsing(new AssignmentInterCoDiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.AmendmentReason), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.Contract))))
                // For Host Company
                .ForMember(dest => dest.AssignmentHostcompanyCode, src => src.ResolveUsing(new AssignmentInterCoDiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyCode), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.OperatingCountryCompany))))
                .ForMember(dest => dest.AssignmentHostcompanyDescription, src => src.ResolveUsing(new AssignmentInterCoDiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.Description), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.OperatingCountryCompany))))
                .ForMember(dest => dest.AssignmentHostcompanyName, src => src.ResolveUsing(new AssignmentInterCoDiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyName), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.OperatingCountryCompany))))
                .ForMember(dest => dest.AssignmentHostcompanyDiscount, src => src.ResolveUsing(new DiscountResolver<DbModel.AssignmentInterCompanyDiscount, DomineModel.AssignmentInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.OperatingCountryCompany))))
                .ForAllOtherMembers(src => src.Ignore());

            #endregion

            #region Assignment Note
            CreateMap<DbModel.AssignmentNote, DomineModel.AssignmentNote>()
               .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.AssignmnetNoteId, src => src.MapFrom(x => x.Id))
               .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => x.CreatedBy))
               .ForMember(dest => dest.CreatedOn, src => src.MapFrom(x => x.CreatedDate))
               .ForMember(dest => dest.Note, src => src.MapFrom(x => x.Note))
               .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
               .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
               .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
               .ReverseMap()
               .ForAllOtherMembers(src => src.Ignore());
            #endregion

            #region AssignmentContributionCalculation
            CreateMap<DbModel.AssignmentContributionCalculation, DomineModel.AssignmentContributionCalculation>()
             .ForMember(dest => dest.AssignmentContCalculationId, src => src.MapFrom(x => x.Id))
             .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
             .ForMember(dest => dest.TotalContributionValue, src => src.MapFrom(x => x.TotalContributionValue))
             .ForMember(dest => dest.ContractHolderPercentage, src => src.MapFrom(x => x.ContractHolderPercentage))
             .ForMember(dest => dest.OperatingCompanyPercentage, src => src.MapFrom(x => x.OperatingCompanyPercentage))
             .ForMember(dest => dest.CountryCompanyPercentage, src => src.MapFrom(x => x.CountryCompanyPercentage))
             .ForMember(dest => dest.ContractHolderValue, src => src.MapFrom(x => x.ContractHolderValue))
             .ForMember(dest => dest.OperatingCompanyValue, src => src.MapFrom(x => x.OperatingCompanyValue))
             .ForMember(dest => dest.CountryCompanyValue, src => src.MapFrom(x => x.CountryCompanyValue))
             .ForMember(dest => dest.MarkupPercentage, src => src.MapFrom(x => x.MarkupPercentage))
             .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
             .ForMember(dest => dest.AssignmentContributionRevenueCosts, src => src.MapFrom(x => x.AssignmentContributionRevenueCost))
             .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
             .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
             .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
             .ForAllOtherMembers(src => src.Ignore());


            CreateMap<DomineModel.AssignmentContributionCalculation, DbModel.AssignmentContributionCalculation>()
          .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.AssignmentContCalculationId : null) : src.AssignmentContCalculationId)))
          .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
          .ForMember(dest => dest.TotalContributionValue, src => src.MapFrom(x => x.TotalContributionValue))
          .ForMember(dest => dest.ContractHolderPercentage, src => src.MapFrom(x => x.ContractHolderPercentage))
          .ForMember(dest => dest.OperatingCompanyPercentage, src => src.MapFrom(x => x.OperatingCompanyPercentage))
          .ForMember(dest => dest.CountryCompanyPercentage, src => src.MapFrom(x => x.CountryCompanyPercentage))
          .ForMember(dest => dest.ContractHolderValue, src => src.MapFrom(x => x.ContractHolderValue))
          .ForMember(dest => dest.OperatingCompanyValue, src => src.MapFrom(x => x.OperatingCompanyValue))
          .ForMember(dest => dest.CountryCompanyValue, src => src.MapFrom(x => x.CountryCompanyValue))
          .ForMember(dest => dest.MarkupPercentage, src => src.MapFrom(x => x.MarkupPercentage))
          .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
          .ForMember(dest => dest.AssignmentContributionRevenueCost, src => src.MapFrom(x => x.AssignmentContributionRevenueCosts))
          .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
          .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
          .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
          .ForAllOtherMembers(src => src.Ignore());
            #endregion

            #region AssignmentContributionRevenueCost 

            CreateMap<DbModel.AssignmentContributionRevenueCost, DomineModel.AssignmentContributionRevenueCost>()
             .ForMember(dest => dest.AssignmentContributionRevenueCostId, src => src.MapFrom(x => x.Id))
             .ForMember(dest => dest.AssignmentContributionCalculationId, src => src.MapFrom(x => x.Id))
             .ForMember(dest => dest.Type, src => src.MapFrom(x => x.SectionType))
             .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Description))
              .ForMember(dest => dest.Value, src => src.MapFrom(x => x.Value))
             .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
             .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
             .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
             .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<DomineModel.AssignmentContributionRevenueCost, DbModel.AssignmentContributionRevenueCost>()
            .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isContributionRevenueCostId") ? (Convert.ToBoolean(context.Options.Items["isContributionRevenueCostId"]) ? src.AssignmentContributionRevenueCostId : null) : src.AssignmentContributionRevenueCostId)))
             .ForMember(dest => dest.AssignmentContributionCalculationId, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isContributionCalculationId") ? (Convert.ToBoolean(context.Options.Items["isContributionCalculationId"]) ? src.AssignmentContributionCalculationId : null) : src.AssignmentContributionCalculationId)))
             .ForMember(dest => dest.AssignmentContributionCalculationId, src => src.MapFrom(x => x.AssignmentContributionCalculationId))
             .ForMember(dest => dest.SectionType, src => src.MapFrom(x => x.Type))
             .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Description))
             .ForMember(dest => dest.Value, src => src.MapFrom(x => x.Value))
             .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
             .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
             .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
             .ForAllOtherMembers(opt => opt.Ignore());

            #endregion

            #region AssignmentSubSupplier

            CreateMap<DbModel.AssignmentSubSupplier, DomineModel.AssignmentSubSupplier>()
            .ForMember(dest => dest.AssignmentSubSupplierId, src => src.MapFrom(x => x.SupplierType == SupplierType.MainSupplier.FirstChar() && x.IsDeleted == false ? x.Id : 0))//MS-TS Link CR
            .ForMember(dest => dest.AssignmentSupplierId, src => src.MapFrom(x => x.Id)) //Added for IGO - D932
            .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
            .ForMember(dest => dest.MainSupplierId, src => src.MapFrom(x => x.SupplierType == SupplierType.MainSupplier.FirstChar() && x.IsDeleted == false ? x.SupplierId : null))//MS-TS Link CR
            .ForMember(dest => dest.MainSupplierName, src => src.MapFrom(x => x.SupplierType == SupplierType.MainSupplier.FirstChar() && x.IsDeleted == false ? x.Supplier.SupplierName : null))
            .ForMember(dest => dest.MainSupplierContactId, src => src.MapFrom(x => x.SupplierType == SupplierType.MainSupplier.FirstChar() && x.IsDeleted == false ? x.SupplierContactId : null))
            .ForMember(dest => dest.MainSupplierContactName, src => src.MapFrom(x => x.SupplierType == SupplierType.MainSupplier.FirstChar() && x.IsDeleted == false ? x.SupplierContact.SupplierContactName : null))
            .ForMember(dest => dest.IsMainSupplierFirstVisit, src => src.MapFrom(x => x.SupplierType == SupplierType.MainSupplier.FirstChar() && x.IsDeleted == false ? x.IsFirstVisit : null)) //MS-TS Link CR  
            .ForMember(dest => dest.SubSupplierId, src => src.MapFrom(x => x.SupplierType == SupplierType.SubSupplier.FirstChar() && x.IsDeleted == false ? x.SupplierId : null))//MS-TS Link CR
            .ForMember(dest => dest.SubSupplierName, src => src.MapFrom(x => x.SupplierType == SupplierType.SubSupplier.FirstChar() && x.IsDeleted == false ? x.Supplier.SupplierName : null)) //MS-TS Link CR
            .ForMember(dest => dest.SubSupplierAddress, src => src.MapFrom(x => x.SupplierType == SupplierType.SubSupplier.FirstChar() && x.IsDeleted == false ? x.Supplier.Address : null))  // Added for D-718
            .ForMember(dest => dest.SubSupplierContactId, src => src.MapFrom(x => x.SupplierType == SupplierType.SubSupplier.FirstChar() && x.IsDeleted == false ? x.SupplierContactId : null)) //MS-TS Link CR
            .ForMember(dest => dest.SubSupplierContactName, src => src.MapFrom(x => x.SupplierType == SupplierType.SubSupplier.FirstChar() && x.IsDeleted == false ? x.SupplierContact.SupplierContactName : null)) //MS-TS Link CR
            .ForMember(dest => dest.IsSubSupplierFirstVisit, src => src.MapFrom(x => x.SupplierType == SupplierType.SubSupplier.FirstChar() && x.IsDeleted == false ? x.IsFirstVisit : null)) //MS-TS Link CR 
            .ForMember(dest => dest.AssignmentSubSupplierIdForSubSupplier, src => src.MapFrom(x => x.SupplierType == SupplierType.SubSupplier.FirstChar() ? x.Id : 0))//MS-TS Link CR
            .ForMember(dest => dest.SupplierType, src => src.MapFrom(x => x.SupplierType))//MS-TS Link CR
            .ForMember(dest => dest.City, src => src.MapFrom(x => x.Supplier.City.Name))  // Added for D-718
            .ForMember(dest => dest.State, src => src.MapFrom(x => x.Supplier.County.Name))  // Added for D-718
            .ForMember(dest => dest.Country, src => src.MapFrom(x => x.Supplier.Country.Name))  // Added for D-718
            .ForMember(dest => dest.ZipCode, src => src.MapFrom(x => x.Supplier.PostalCode))  // Added for D-718
            .ForMember(dest => dest.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
            .ForMember(dest => dest.AssignmentSubSupplierTS, src => src.MapFrom(x => x.AssignmentSubSupplierTechnicalSpecialist))
            .ForMember(dest => dest.IsFirstVisitAssociated, src => src.MapFrom(x => x.AssignmentSubSupplierTechnicalSpecialist != null ? true : false)) // Added for front end convenience
            .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
            .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
            .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
            .ForAllOtherMembers(src => src.Ignore());

            CreateMap<DbModel.AssignmentSubSupplier, DomineModel.AssignmentSubSupplierVisit>()
            .ForMember(dest => dest.AssignmentSubSupplierId, src => src.MapFrom(x => x.SupplierType == SupplierType.MainSupplier.FirstChar() ? x.Id : 0))//MS-TS Link CR
            .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
            .ForMember(dest => dest.MainSupplierId, src => src.MapFrom(x => x.SupplierType == SupplierType.MainSupplier.FirstChar() ? x.SupplierId : null))//MS-TS Link CR
            .ForMember(dest => dest.MainSupplierName, src => src.MapFrom(x => x.SupplierType == SupplierType.MainSupplier.FirstChar() ? x.Supplier.SupplierName : null))
            .ForMember(dest => dest.MainSupplierContactId, src => src.MapFrom(x => x.SupplierType == SupplierType.MainSupplier.FirstChar() ? x.SupplierContactId : null))
            .ForMember(dest => dest.MainSupplierContactName, src => src.MapFrom(x => x.SupplierType == SupplierType.MainSupplier.FirstChar() ? x.SupplierContact.SupplierContactName : null))
            .ForMember(dest => dest.IsMainSupplierFirstVisit, src => src.MapFrom(x => x.SupplierType == SupplierType.MainSupplier.FirstChar() ? x.IsFirstVisit : null)) //MS-TS Link CR  
            .ForMember(dest => dest.SubSupplierId, src => src.MapFrom(x => x.SupplierType == SupplierType.SubSupplier.FirstChar() ? x.SupplierId : null))//MS-TS Link CR
            .ForMember(dest => dest.SubSupplierName, src => src.MapFrom(x => x.SupplierType == SupplierType.SubSupplier.FirstChar() ? x.Supplier.SupplierName : null)) //MS-TS Link CR
            .ForMember(dest => dest.SubSupplierAddress, src => src.MapFrom(x => x.SupplierType == SupplierType.SubSupplier.FirstChar() ? x.Supplier.Address : null))  // Added for D-718
            .ForMember(dest => dest.SubSupplierContactId, src => src.MapFrom(x => x.SupplierType == SupplierType.SubSupplier.FirstChar() ? x.SupplierContactId : null)) //MS-TS Link CR
            .ForMember(dest => dest.SubSupplierContactName, src => src.MapFrom(x => x.SupplierType == SupplierType.SubSupplier.FirstChar() ? x.SupplierContact.SupplierContactName : null)) //MS-TS Link CR
            .ForMember(dest => dest.IsSubSupplierFirstVisit, src => src.MapFrom(x => x.SupplierType == SupplierType.SubSupplier.FirstChar() ? x.IsFirstVisit : null)) //MS-TS Link CR 
            .ForMember(dest => dest.AssignmentSubSupplierIdForSubSupplier, src => src.MapFrom(x => x.SupplierType == SupplierType.SubSupplier.FirstChar() ? x.Id : 0))//MS-TS Link CR
            .ForMember(dest => dest.SupplierType, src => src.MapFrom(x => x.SupplierType))//MS-TS Link CR
            .ForMember(dest => dest.City, src => src.MapFrom(x => x.Supplier.City.Name))  // Added for D-718
            .ForMember(dest => dest.State, src => src.MapFrom(x => x.Supplier.County.Name))  // Added for D-718
            .ForMember(dest => dest.Country, src => src.MapFrom(x => x.Supplier.Country.Name))  // Added for D-718
            .ForMember(dest => dest.ZipCode, src => src.MapFrom(x => x.Supplier.PostalCode))  // Added for D-718
            .ForMember(dest => dest.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
            .ForMember(dest => dest.AssignmentSubSupplierTS, src => src.MapFrom(x => x.AssignmentSubSupplierTechnicalSpecialist))
            .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
            .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
            .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
            .ForAllOtherMembers(src => src.Ignore());

            // CreateMap<DbModel.AssignmentSubSupplier, DomineModel.AssignmentSubSupplier>()
            //.ForMember(dest => dest.AssignmentSubSupplierId, src => src.MapFrom(x => x.Id))
            //.ForMember(dest => dest.SupplierContactId, src => src.MapFrom(x => x.SupplierContactId))
            //.ForMember(dest => dest.SupplierContactName, src => src.MapFrom(x => x.SupplierContact.SupplierContactName))
            //.ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
            //.ForMember(dest => dest.SupplierId, src => src.MapFrom(x => x.SupplierId))//MS-TS Link CR
            //.ForMember(dest => dest.MainSupplierName, src => src.MapFrom(x => x.Assignment.MainSupplierContact.Supplier.SupplierName))
            //.ForMember(dest => dest.IsSubSupplierFirstVisit, src => src.MapFrom(x => x.SupplierType == SupplierType.Subsupplier.FirstChar() ? x.IsFirstVisit : null)) //MS-TS Link CR -
            //.ForMember(dest => dest.IsMainSupplierFirstVisit, src => src.MapFrom(x => x.SupplierType == SupplierType.MainSupplier.FirstChar() ? x.IsFirstVisit : null)) //MS-TS Link CR  
            //.ForMember(dest => dest.SupplierName, src => src.MapFrom(x => x.Supplier.SupplierName)) //MS-TS Link CR
            //.ForMember(dest => dest.SubSupplierSupplierId, src => src.MapFrom(x => x.Supplier.Id))//MS-TS Link CR
            //.ForMember(dest => dest.SupplierType, src => src.MapFrom(x => x.SupplierType))//MS-TS Link CR
            //.ForMember(dest => dest.SubSupplierAddress, src => src.MapFrom(x => x.Supplier.Address))  // Added for D-718
            //.ForMember(dest => dest.City, src => src.MapFrom(x => x.Supplier.City.Name))  // Added for D-718
            //.ForMember(dest => dest.State, src => src.MapFrom(x => x.Supplier.County.Name))  // Added for D-718
            //.ForMember(dest => dest.Country, src => src.MapFrom(x => x.Supplier.Country.Name))  // Added for D-718
            //.ForMember(dest => dest.ZipCode, src => src.MapFrom(x => x.Supplier.PostalCode))  // Added for D-718
            //.ForMember(dest => dest.AssignmentSubSupplierTS, src => src.MapFrom(x => x.AssignmentSubSupplierTechnicalSpecialist))
            //.ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
            //.ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
            //.ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
            //.ForAllOtherMembers(src => src.Ignore());


            CreateMap<DomineModel.AssignmentSubSupplierTS, DbModel.AssignmentSubSupplierTechnicalSpecialist>()
              .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.AssignmentSubSupplierTSId : null) : src.AssignmentSubSupplierTSId)))
              .ForMember(dest => dest.AssignmentSubSupplierId, src => src.MapFrom(x => x.AssignmentSubSupplierId))
              .ForMember(dest => dest.TechnicalSpecialistId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("AssignmentTsId") ? ((List<DbModel.AssignmentTechnicalSpecialist>)context.Options.Items["AssignmentTsId"])?.FirstOrDefault(x => x.TechnicalSpecialistId == src.Epin)?.Id : null))
              .ForMember(dest => dest.TechnicalSpecialistId, src => src.MapFrom(x => x.AssignmentTechnicalSpecialistId))
              .ForMember(dest => dest.IsDeleted, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("IsDeleted") ? Convert.ToBoolean(context.Options.Items["IsDeleted"]) : false))
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
              .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
              .ForAllOtherMembers(src => src.Ignore());


            CreateMap<DomineModel.AssignmentSubSupplier, DbModel.AssignmentSubSupplier>()
             .ForMember(dest => dest.Id, src => src.MapFrom(x => x.AssignmentSubSupplierId))
             .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
             .ForMember(dest => dest.SupplierId, src => src.MapFrom(x => x.MainSupplierId))//MS-TS Link CR
             .ForMember(dest => dest.SupplierType, src => src.MapFrom(x => x.SupplierType))//MS-TS Link CR
             .ForMember(dest => dest.SupplierContactId, src => src.MapFrom(x => x.MainSupplierContactId))
             .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
             .ForMember(dest => dest.AssignmentSubSupplierTechnicalSpecialist, src => src.MapFrom(x => x.AssignmentSubSupplierTS))
             .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
             .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
             .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
             .ForMember(dest => dest.IsDeleted, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("IsDeleted") ? Convert.ToBoolean(context.Options.Items["IsDeleted"]) : false))
            // .ForMember(dest => dest.IsFirstVisit, src => src.MapFrom(x => x.SupplierType == SupplierType.MainSupplier.FirstChar() ? x.IsMainSupplierFirstVisit : x.IsSubSupplierFirstVisit)) //MS-TS Link CR -
             .ForMember(dest => dest.IsFirstVisit, src => src.MapFrom(x => x.IsMainSupplierFirstVisit))
            .ForAllOtherMembers(src => src.Ignore());

            CreateMap<DomineModel.AssignmentSubSupplierVisit, DbModel.AssignmentSubSupplier>()
             .ForMember(dest => dest.Id, src => src.MapFrom(x => x.AssignmentSubSupplierId))
             .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
             .ForMember(dest => dest.SupplierId, src => src.MapFrom(x => x.MainSupplierId))//MS-TS Link CR
             .ForMember(dest => dest.SupplierType, src => src.MapFrom(x => x.SupplierType))//MS-TS Link CR
             .ForMember(dest => dest.SupplierContactId, src => src.MapFrom(x => x.MainSupplierContactId))
             .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
             .ForMember(dest => dest.AssignmentSubSupplierTechnicalSpecialist, src => src.MapFrom(x => x.AssignmentSubSupplierTS))
             .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
             .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
             .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
             .ForMember(dest => dest.IsDeleted, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("IsDeleted") ? Convert.ToBoolean(context.Options.Items["IsDeleted"]) : false))
             .ForMember(dest => dest.IsFirstVisit, src => src.MapFrom(x => x.IsMainSupplierFirstVisit))
            .ForAllOtherMembers(src => src.Ignore());

            #endregion

            #region Db Model to Domain AssignmentSubSupplierTechnicalSpecialist
            CreateMap<DbModel.AssignmentSubSupplierTechnicalSpecialist, DomineModel.AssignmentSubSupplierTS>()
              .ForMember(dest => dest.AssignmentSubSupplierTSId, src => src.MapFrom(x => x.Id))
              .ForMember(dest => dest.AssignmentSubSupplierId, src => src.MapFrom(x => x.AssignmentSubSupplierId))
              .ForMember(dest => dest.Epin, src => src.MapFrom(x => x.TechnicalSpecialist.TechnicalSpecialistId))
              .ForMember(dest => dest.AssignmentTechnicalSpecialistId, src => src.MapFrom(x => x.TechnicalSpecialistId))
              .ForMember(dest => dest.IsDeleted, src => src.MapFrom(x => x.IsDeleted))
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
              .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
               .ForAllOtherMembers(src => src.Ignore());
            #endregion

            #region AssignmentTaxonomy

            CreateMap<DbModel.AssignmentTaxonomy, DomineModel.AssignmentTaxonomy>()
                .ForMember(dest => dest.AssignmentTaxonomyId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
                 .ForMember(dest => dest.TaxonomyCategoryId, src => src.MapFrom(x => x.TaxonomyService.TaxonomySubCategory.TaxonomyCategory.Id))
                 .ForMember(dest => dest.TaxonomySubCategoryId, src => src.MapFrom(x => x.TaxonomyService.TaxonomySubCategory.Id))
                .ForMember(dest => dest.TaxonomyServiceId, src => src.MapFrom(x => x.TaxonomyServiceId))
                .ForMember(dest => dest.TaxonomyService, src => src.MapFrom(x => x.TaxonomyService.TaxonomyServiceName))
                .ForMember(dest => dest.TaxonomySubCategory, src => src.MapFrom(x => x.TaxonomyService.TaxonomySubCategory.TaxonomySubCategoryName))
                .ForMember(dest => dest.TaxonomyCategory, src => src.MapFrom(x => x.TaxonomyService.TaxonomySubCategory.TaxonomyCategory.Name))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForAllOtherMembers(src => src.Ignore());


            CreateMap<DomineModel.AssignmentTaxonomy, DbModel.AssignmentTaxonomy>()
                .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.AssignmentTaxonomyId : null) : src.AssignmentTaxonomyId)))
                .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.TaxonomyServiceId, src => src.MapFrom(x => x.TaxonomyServiceId))
                // .ForMember(dest => dest.TaxonomyServiceId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("TaxonomyService") ? ((List<DbModel.TaxonomyService>)context.Options.Items["TaxonomyService"])?.FirstOrDefault(x => x.TaxonomyServiceName == src.TaxonomyService)?.Id : null))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForAllOtherMembers(src => src.Ignore());


            #endregion

           #region  AssignmentDetail
            CreateMap<DbModel.Assignment, DomineModel.AssignmentDetail>()
               .ForMember(dest => dest.AssignmentInfo, src => src.MapFrom(x => x))
               .ForMember(dest => dest.AssignmentNotes, src => src.MapFrom(x => x.AssignmentNote))
               .ForMember(dest => dest.AssignmentAdditionalExpenses, src => src.MapFrom(x => x.AssignmentAdditionalExpense))
               .ForMember(dest => dest.AssignmentContractSchedules, src => src.MapFrom(x => x.AssignmentContractSchedule))
               .ForMember(dest => dest.AssignmentContributionCalculators, src => src.MapFrom(x => x.AssignmentContributionCalculation))
               .ForMember(dest => dest.AssignmentInterCompanyDiscounts, src => src.MapFrom(x => x.AssignmentInterCompanyDiscount.ToList()))
               .ForMember(dest => dest.AssignmentInstructions, src => src.MapFrom(x => x.AssignmentMessage.ToList()))
               .ForMember(dest => dest.AssignmentReferences, src => src.MapFrom(x => x.AssignmentReferenceNavigation))
               .ForMember(dest => dest.AssignmentSubSuppliers, src => src.MapFrom(x => x.AssignmentSubSupplier))
               .ForMember(dest => dest.AssignmentTaxonomy, src => src.MapFrom(x => x.AssignmentTaxonomy))
               .ForMember(dest => dest.AssignmentTechnicalSpecialists, src => src.MapFrom(x => x.AssignmentTechnicalSpecialist))
               // .ForMember(dest => dest.AssignmentTechnicalSpecialistChargeRates, src => src.MapFrom(x => x.ass))
               .ForAllOtherMembers(x => x.Ignore());

            #endregion
        }

        public class AssignmentMessageResolver : IValueResolver<DomineModel.Assignment, DbModel.Assignment, ICollection<DbModel.AssignmentMessage>>
        {
            private readonly IAssignmentService _assignmentService = null;

            public AssignmentMessageResolver(IAssignmentService assignmentService)
            {
                this._assignmentService = assignmentService;
            }

            public ICollection<DbModel.AssignmentMessage> Resolve(DomineModel.Assignment source, DbModel.Assignment destination, ICollection<DbModel.AssignmentMessage> destMember, ResolutionContext context)
            {
                List<DbModel.AssignmentMessage> dbMessages = destination.AssignmentMessage?.ToList();
                return this._assignmentService.AssignAssignmentMessages(dbMessages, source);
            }
        }

    }
}
