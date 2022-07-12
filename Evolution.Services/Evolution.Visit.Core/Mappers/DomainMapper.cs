using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Evolution.Assignment.Domain.Enums;
using Evolution.Project.Domain.Enums;
using Evolution.Visit.Core.Resolver;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModels = Evolution.Visit.Domain.Models.Visits;
using DomResolver = Evolution.Automapper.Resolver.MongoSearch;

namespace Evolution.Visit.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            #region DB Visit  to Domain Visit Search

            CreateMap<DbModels.Visit, DomainModels.VisitSearch>()
                .ForMember(dest => dest.VisitId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.CustomerName, src => src.MapFrom(x => x.Assignment.Project.Contract.Customer.Name))
                .ForMember(dest => dest.AssignmentNubmer, src => src.MapFrom(x => x.Assignment.AssignmentNumber))
                .ForMember(dest => dest.ContractHoldingCompanyCode, src => src.MapFrom(x => x.Assignment.ContractCompany.Code))
                .ForMember(dest => dest.ContractHoldingCompanyName, src => src.MapFrom(x => x.Assignment.ContractCompany.Name))
                .ForMember(dest => dest.ReportNumber, src => src.MapFrom(x => x.ReportNumber))
                .ForMember(dest => dest.CHCoordinatorCode, src => src.MapFrom(x => x.Assignment.ContractCompanyCoordinator.SamaccountName))
                .ForMember(dest => dest.CHCoordinatorName, src => src.MapFrom(x => x.Assignment.ContractCompanyCoordinator.Name))
                .ForMember(dest => dest.CustomerContractNumber, src => src.MapFrom(x => x.Assignment.Project.Contract.CustomerContractNumber))
                .ForMember(dest => dest.FromDate, src => src.MapFrom(x => x.FromDate))
                .ForMember(dest => dest.OperatingCompanyCode, src => src.MapFrom(x => x.Assignment.OperatingCompany.Code))
                .ForMember(dest => dest.OperatingCompanyName, src => src.MapFrom(x => x.Assignment.OperatingCompany.Name))
                .ForMember(dest => dest.ProjectNumber, src => src.MapFrom(x => x.Assignment.Project.ProjectNumber))
                .ForMember(dest => dest.ToDate, src => src.MapFrom(x => x.ToDate))
                .ForMember(dest => dest.OCCoordinatorCode, src => src.MapFrom(x => x.Assignment.OperatingCompanyCoordinator.SamaccountName))
                .ForMember(dest => dest.OCCoordinatorName, src => src.MapFrom(x => x.Assignment.OperatingCompanyCoordinator.Name))
                .ForMember(dest => dest.CustomerProjectName, src => src.MapFrom(x => x.Assignment.Project.CustomerProjectName))
                .ForMember(dest => dest.SupplierPONumber, src => src.MapFrom(x => x.Assignment.SupplierPurchaseOrder.SupplierPonumber))
                .ForMember(dest => dest.SupplierSubSupplier, src => src.MapFrom(x => x.Supplier.SupplierName))
                .ForMember(dest => dest.NotificationReference, src => src.MapFrom(x => x.NotificationReference))
                .ForMember(dest => dest.WorkflowType, src => src.MapFrom(x => x.Assignment.Project.WorkFlowType))
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Base Visit
            CreateMap<DbModels.Visit, DomainModels.BaseVisit>()
                .ForMember(dest => dest.VisitContractCompany, src => src.MapFrom(x => x.Assignment.ContractCompany.Name))
                .ForMember(dest => dest.VisitAssignmentNumber, src => src.MapFrom(x => x.Assignment.AssignmentNumber))
                .ForMember(dest => dest.VisitNumber, src => src.MapFrom(x => x.VisitNumber))
                .ForMember(dest => dest.VisitContractCompanyCode, src => src.MapFrom(x => x.Assignment.ContractCompany.Code))
                .ForMember(dest => dest.VisitContractCoordinator, src => src.MapFrom(x => x.Assignment.ContractCompanyCoordinator.Name))
                .ForMember(dest => dest.VisitContractNumber, src => src.MapFrom(x => x.Assignment.Project.Contract.ContractNumber))
                .ForMember(dest => dest.VisitCustomerCode, src => src.MapFrom(x => x.Assignment.Project.Contract.Customer.Code))
                .ForMember(dest => dest.VisitCustomerContractNumber, src => src.MapFrom(x => x.Assignment.Project.Contract.CustomerContractNumber))
                .ForMember(dest => dest.VisitCustomerName, src => src.MapFrom(x => x.Assignment.Project.Contract.Customer.Name))
                .ForMember(dest => dest.VisitCustomerProjectName, src => src.MapFrom(x => x.Assignment.Project.CustomerProjectName))
                .ForMember(dest => dest.FinalVisit, src => src.MapFrom(x => x.IsFinalVisit.ToYesNo()))
                .ForMember(dest => dest.IsFinalVisit, src => src.MapFrom(x => x.IsFinalVisit))
                .ForMember(dest => dest.VisitOperatingCompany, src => src.MapFrom(x => x.Assignment.OperatingCompany.Name))
                .ForMember(dest => dest.VisitOperatingCompanyCoordinator, src => src.MapFrom(x => x.Assignment.OperatingCompanyCoordinator.Name))
                .ForMember(dest => dest.VisitOperatingCompanyCode, src => src.MapFrom(x => x.Assignment.OperatingCompany.Code))
                .ForMember(dest => dest.VisitProjectNumber, src => src.MapFrom(x => x.Assignment.Project.ProjectNumber))
                .ForMember(dest => dest.VisitSupplier, src => src.MapFrom(x => x.Supplier.SupplierName))
                .ForMember(dest => dest.VisitSupplierPONumber, src => src.MapFrom(x => x.Assignment.SupplierPurchaseOrder.SupplierPonumber))
                .ForMember(dest => dest.VisitSupplierPOId, src => src.MapFrom(x => x.Assignment.SupplierPurchaseOrder.Id))
                .ForMember(dest => dest.VisitNotificationReference, src => src.MapFrom(x => x.NotificationReference))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => Convert.ToInt32(x.UpdateCount)))
                .ForMember(dest => dest.VisitReportNumber, src => src.MapFrom(x => x.ReportNumber))
                .ForMember(dest => dest.VisitStartDate, src => src.MapFrom(x => x.FromDate))
                .ForMember(dest => dest.VisitStatus, src => src.MapFrom(x => x.VisitStatus))
                .ForMember(dest => dest.UnusedReason, src => src.MapFrom(x => x.UnusedReason))
                .ForMember(dest => dest.VisitAssignmentId, src => src.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.VisitId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.SupplierId, src => src.MapFrom(x => x.SupplierId))
                .ForMember(dest => dest.VisitCustomerProjectNumber, src => src.MapFrom(x => x.Assignment.Project.CustomerProjectNumber))
                .ForMember(dest => dest.VisitMaterialDescription, src => src.MapFrom(x => x.Assignment.SupplierPurchaseOrder.MaterialDescription))
                .ForMember(dest => dest.VisitReportSentToCustomerDate, src => src.MapFrom(x => x.ReportSentToCustomerDate))
                .ForMember(dest => dest.TechSpecialists, src => src.MapFrom(x => x.VisitTechnicalSpecialist))
                .ReverseMap()
                .ForAllOtherMembers(src => src.Ignore());

            #endregion

            #region Visit
            CreateMap<DbModels.Visit, DomainModels.Visit>()
                .ForMember(dest => dest.VisitId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.VisitAssignmentId, src => src.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.VisitAssignmentNumber, src => src.MapFrom(x => x.Assignment.AssignmentNumber))
                .ForMember(dest => dest.VisitAssignmentCreatedDate, src => src.MapFrom(x => x.Assignment.CreatedDate))
                .ForMember(dest => dest.VisitAssignmentReference, src => src.MapFrom(x => x.Assignment.AssignmentReference))
                .ForMember(dest => dest.VisitContractCompany, src => src.MapFrom(x => x.Assignment.ContractCompany.Name))
                .ForMember(dest => dest.VisitContractCompanyCode, src => src.MapFrom(x => x.Assignment.ContractCompany.Code))
                .ForMember(dest => dest.VisitContractCoordinator, src => src.MapFrom(x => x.Assignment.ContractCompanyCoordinator.Name))
                .ForMember(dest => dest.VisitContractCoordinatorCode, src => src.MapFrom(x => x.Assignment.ContractCompanyCoordinator.SamaccountName))
                .ForMember(dest => dest.VisitContractNumber, src => src.MapFrom(x => x.Assignment.Project.Contract.ContractNumber))
                .ForMember(dest => dest.VisitCustomerCode, src => src.MapFrom(x => x.Assignment.Project.Contract.Customer.Code))
                .ForMember(dest => dest.VisitCustomerContractNumber, src => src.MapFrom(x => x.Assignment.Project.Contract.CustomerContractNumber))
                .ForMember(dest => dest.VisitCustomerName, src => src.MapFrom(x => x.Assignment.Project.Contract.Customer.Name))
                .ForMember(dest => dest.VisitCustomerProjectName, src => src.MapFrom(x => x.Assignment.Project.CustomerProjectName))
                .ForMember(dest => dest.VisitCustomerProjectNumber, src => src.MapFrom(x => x.Assignment.Project.CustomerProjectNumber))
                .ForMember(dest => dest.FinalVisit, src => src.MapFrom(x => x.IsFinalVisit.ToYesNo()))
                .ForMember(dest => dest.IsFinalVisit, src => src.MapFrom(x => x.IsFinalVisit))
                .ForMember(dest => dest.VisitMaterialDescription, src => src.MapFrom(x => x.Assignment.SupplierPurchaseOrder.MaterialDescription))
                .ForMember(dest => dest.VisitOperatingCompany, src => src.MapFrom(x => x.Assignment.OperatingCompany.Name))
                .ForMember(dest => dest.VisitOperatingCompanyCoordinator, src => src.MapFrom(x => x.Assignment.OperatingCompanyCoordinator.Name))
                .ForMember(dest => dest.VisitOperatingCompanyCoordinatorCode, src => src.MapFrom(x => x.Assignment.OperatingCompanyCoordinator.SamaccountName))
                .ForMember(dest => dest.VisitOperatingCompanyCode, src => src.MapFrom(x => x.Assignment.OperatingCompany.Code))
                .ForMember(dest => dest.VisitProjectNumber, src => src.MapFrom(x => x.Assignment.Project.ProjectNumber))
                .ForMember(dest => dest.SkeltonVisit, src => src.MapFrom(x => x.IsSkeltonVisit.ToYesNo()))
                .ForMember(dest => dest.VisitSupplier, src => src.MapFrom(x => x.Supplier.SupplierName))
                .ForMember(dest => dest.SupplierLocation, src => src.MapFrom(x => x.Supplier.Address))
                .ForMember(dest => dest.VisitSupplierPONumber, src => src.MapFrom(x => x.Assignment.SupplierPurchaseOrder.SupplierPonumber))
                .ForMember(dest => dest.VisitSupplierPOId, src => src.MapFrom(x => x.Assignment.SupplierPurchaseOrder.Id))
                .ForMember(dest => dest.VisitApprovedByContractCompany, src => src.MapFrom(x => x.IsApprovedByContractCompany))
                .ForMember(dest => dest.IsContractHoldingCompanyActive, src => src.MapFrom(x => x.Assignment.ContractCompany.IsActive)) // ITK D - 619
                .ForMember(dest => dest.VisitCompletedPercentage, src => src.MapFrom(x => x.PercentageCompleted))
                .ForMember(dest => dest.VisitDatePeriod, src => src.MapFrom(x => x.DatePeriod))
                .ForMember(dest => dest.VisitEndDate, src => src.MapFrom(x => x.ToDate))
                .ForMember(dest => dest.VisitExpectedCompleteDate, src => src.MapFrom(x => x.ExpectedCompleteDate))
                .ForMember(dest => dest.VisitNotificationReference, src => src.MapFrom(x => x.NotificationReference))
                .ForMember(dest => dest.VisitNumber, src => src.MapFrom(x => x.VisitNumber))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => Convert.ToInt32(x.UpdateCount)))
                .ForMember(dest => dest.VisitReference1, src => src.MapFrom(x => x.Reference1))
                .ForMember(dest => dest.VisitReference2, src => src.MapFrom(x => x.Reference2))
                .ForMember(dest => dest.VisitReference3, src => src.MapFrom(x => x.Reference3))
                .ForMember(dest => dest.VisitRejectionReason, src => src.MapFrom(x => x.RejectionReason))
                .ForMember(dest => dest.VisitReportNumber, src => src.MapFrom(x => x.ReportNumber))
                .ForMember(dest => dest.VisitReportSentToCustomerDate, src => src.MapFrom(x => x.ReportSentToCustomerDate))
                .ForMember(dest => dest.VisitReviewDateByClient, src => src.MapFrom(x => x.ReviewedDate))
                .ForMember(dest => dest.VisitReviewStatusByClient, src => src.MapFrom(x => x.ClientReviewStatus))
                .ForMember(dest => dest.VisitStartDate, src => src.MapFrom(x => x.FromDate))
                .ForMember(dest => dest.VisitStatus, src => src.MapFrom(x => x.VisitStatus))
                .ForMember(dest =>dest.UnusedReason, src => src.MapFrom(x =>x.UnusedReason))
                .ForMember(dest => dest.AssignmentClientReportingRequirements, src => src.MapFrom(x => x.Assignment.AssignmentMessage
                                                                                              .FirstOrDefault(x1 => x1.Assignment.Id == x.Assignment.Id
                                                                                                             && x1.MessageTypeId == Convert.ToInt16(AssignmentMessageType.ReportingRequirements)
                                                                                                             && x1.IsActive == true)
                                                                                              .Message))
                .ForMember(dest => dest.SupplierId, src => src.MapFrom(x => x.SupplierId))
                .ForMember(dest => dest.AssignmentProjectBusinessUnit, src => src.MapFrom(x => x.Assignment.Project.ProjectType.Name))
                .ForMember(dest => dest.WorkflowType, src => src.MapFrom(x => x.Assignment.Project.WorkFlowType))
                .ForMember(dest => dest.ProjectInvoiceInstructionNotes, src =>src.MapFrom(x=>x.Assignment.Project.ProjectMessage
                                                        .FirstOrDefault(m => m.ProjectId == x.Assignment.ProjectId
                                                            && m.MessageTypeId == Convert.ToInt16(ProjectMessageType.InvoiceNotes)).Message))
                .ForMember(dest => dest.SummaryOfReport, src=>src.MapFrom(x => x.SummaryOfReport))
                .ForMember(dest => dest.IsExtranetSummaryReportVisible,src => src.MapFrom(x => x.Assignment.Project.IsExtranetSummaryVisibleToClient))
                .ForMember(dest => dest.IsVisitOnPopUp,src => src.MapFrom(x => x.Assignment.Project.IsVisitOnPopUp)) //IGO QC 864 - client pop in visit
                .ForMember(dest => dest.TechSpecialists, src => src.MapFrom(x=> x.VisitTechnicalSpecialist))
                .ForMember(dest => dest.Evoid, src => src.MapFrom(x => x.Evoid)) // These needs to be removed once DB sync done                                                            
                .ForAllOtherMembers(src => src.Ignore());
                
                CreateMap<DbModels.Visit, DomainModels.VisitSearchResults>()
                .ForMember(dest => dest.VisitId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.VisitAssignmentId, src => src.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.VisitAssignmentNumber, src => src.MapFrom(x => x.Assignment.AssignmentNumber))
                .ForMember(dest => dest.VisitAssignmentCreatedDate, src => src.MapFrom(x => x.Assignment.CreatedDate))
                .ForMember(dest => dest.VisitAssignmentReference, src => src.MapFrom(x => x.Assignment.AssignmentReference))
                .ForMember(dest => dest.VisitContractCompany, src => src.MapFrom(x => x.Assignment.ContractCompany.Name))
                .ForMember(dest => dest.VisitContractCompanyCode, src => src.MapFrom(x => x.Assignment.ContractCompany.Code))
                .ForMember(dest => dest.VisitContractCoordinator, src => src.MapFrom(x => x.Assignment.ContractCompanyCoordinator.Name))
                .ForMember(dest => dest.VisitContractCoordinatorCode, src => src.MapFrom(x => x.Assignment.ContractCompanyCoordinator.SamaccountName))
                .ForMember(dest => dest.VisitContractNumber, src => src.MapFrom(x => x.Assignment.Project.Contract.ContractNumber))
                .ForMember(dest => dest.VisitCustomerCode, src => src.MapFrom(x => x.Assignment.Project.Contract.Customer.Code))
                .ForMember(dest => dest.VisitCustomerContractNumber, src => src.MapFrom(x => x.Assignment.Project.Contract.CustomerContractNumber))
                .ForMember(dest => dest.VisitCustomerName, src => src.MapFrom(x => x.Assignment.Project.Contract.Customer.Name))
                .ForMember(dest => dest.VisitCustomerProjectName, src => src.MapFrom(x => x.Assignment.Project.CustomerProjectName))
                .ForMember(dest => dest.VisitCustomerProjectNumber, src => src.MapFrom(x => x.Assignment.Project.CustomerProjectNumber))
                .ForMember(dest => dest.FinalVisit, src => src.MapFrom(x => x.IsFinalVisit.ToYesNo()))
                // .ForMember(dest => dest.IsFinalVisit, src => src.MapFrom(x => x.IsFinalVisit))
                .ForMember(dest => dest.VisitMaterialDescription, src => src.MapFrom(x => x.Assignment.SupplierPurchaseOrder.MaterialDescription))
                .ForMember(dest => dest.VisitOperatingCompany, src => src.MapFrom(x => x.Assignment.OperatingCompany.Name))
                .ForMember(dest => dest.VisitOperatingCompanyCoordinator, src => src.MapFrom(x => x.Assignment.OperatingCompanyCoordinator.Name))
                .ForMember(dest => dest.VisitOperatingCompanyCoordinatorCode, src => src.MapFrom(x => x.Assignment.OperatingCompanyCoordinator.SamaccountName))
                .ForMember(dest => dest.VisitOperatingCompanyCode, src => src.MapFrom(x => x.Assignment.OperatingCompany.Code))
                .ForMember(dest => dest.VisitProjectNumber, src => src.MapFrom(x => x.Assignment.Project.ProjectNumber))
                // .ForMember(dest => dest.SkeltonVisit, src => src.MapFrom(x => x.IsSkeltonVisit.ToYesNo()))
                .ForMember(dest => dest.VisitSupplier, src => src.MapFrom(x => x.Supplier.SupplierName))
                .ForMember(dest => dest.SupplierLocation, src => src.MapFrom(x => x.Supplier.Address))
                .ForMember(dest => dest.VisitSupplierPONumber, src => src.MapFrom(x => x.Assignment.SupplierPurchaseOrder.SupplierPonumber))
                .ForMember(dest => dest.VisitSupplierPOId, src => src.MapFrom(x => x.Assignment.SupplierPurchaseOrder.Id))
                // .ForMember(dest => dest.VisitApprovedByContractCompany, src => src.MapFrom(x => x.IsApprovedByContractCompany))
                // .ForMember(dest => dest.VisitCompletedPercentage, src => src.MapFrom(x => x.PercentageCompleted))
                // .ForMember(dest => dest.VisitDatePeriod, src => src.MapFrom(x => x.DatePeriod))
                .ForMember(dest => dest.VisitEndDate, src => src.MapFrom(x => x.ToDate))
                .ForMember(dest => dest.VisitExpectedCompleteDate, src => src.MapFrom(x => x.ExpectedCompleteDate))
                .ForMember(dest => dest.VisitNotificationReference, src => src.MapFrom(x => x.NotificationReference))
                .ForMember(dest => dest.VisitNumber, src => src.MapFrom(x => x.VisitNumber))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => Convert.ToInt32(x.UpdateCount)))
                // .ForMember(dest => dest.VisitReference1, src => src.MapFrom(x => x.Reference1))
                // .ForMember(dest => dest.VisitReference2, src => src.MapFrom(x => x.Reference2))
                // .ForMember(dest => dest.VisitReference3, src => src.MapFrom(x => x.Reference3))
                // .ForMember(dest => dest.VisitRejectionReason, src => src.MapFrom(x => x.RejectionReason))
                .ForMember(dest => dest.VisitReportNumber, src => src.MapFrom(x => x.ReportNumber))
                .ForMember(dest => dest.VisitReportSentToCustomerDate, src => src.MapFrom(x => x.ReportSentToCustomerDate))
                // .ForMember(dest => dest.VisitReviewDateByClient, src => src.MapFrom(x => x.ReviewedDate))
                // .ForMember(dest => dest.VisitReviewStatusByClient, src => src.MapFrom(x => x.ClientReviewStatus))
                .ForMember(dest => dest.VisitStartDate, src => src.MapFrom(x => x.FromDate))
                .ForMember(dest => dest.VisitStatus, src => src.MapFrom(x => x.VisitStatus))
                .ForMember(dest => dest.UnusedReason, src => src.MapFrom(x => x.UnusedReason))
                .ForMember(dest => dest.SupplierId, src => src.MapFrom(x => x.SupplierId))
                .ForMember(dest => dest.AssignmentProjectBusinessUnit, src => src.MapFrom(x => x.Assignment.Project.ProjectType.Name))
                .ForMember(dest => dest.WorkflowType, src => src.MapFrom(x => x.Assignment.Project.WorkFlowType))
                // .ForMember(dest => dest.IsExtranetSummaryReportVisible,src => src.MapFrom(x => x.Assignment.Project.IsExtranetSummaryVisibleToClient))
                .ForMember(dest => dest.TechSpecialists, src => src.MapFrom(x=> x.VisitTechnicalSpecialist))                                                            
                .ForAllOtherMembers(src => src.Ignore());

            CreateMap<DbModels.VisitTechnicalSpecialist, DomainModels.TechnicalSpecialist>()
               .ForMember(dest => dest.VisitId, source => source.MapFrom(x => x.VisitId))
               .ForMember(dest => dest.FirstName, source => source.MapFrom(x => x.TechnicalSpecialist.FirstName))
               .ForMember(dest => dest.LastName, source => source.MapFrom(x => x.TechnicalSpecialist.LastName))
               .ForMember(dest => dest.LoginName, source => source.MapFrom(x => x.TechnicalSpecialist.LogInName))
               .ForMember(dest => dest.Pin, source => source.MapFrom(x => x.TechnicalSpecialist.Pin))
               .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModels.Visit, DbModels.Visit>()
                .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isVisitId") ? (Convert.ToBoolean(context.Options.Items["isVisitId"]) ? src.VisitId : 0) : src.VisitId)))
                .ForMember(dest => dest.VisitNumber, src => src.MapFrom(x => x.VisitNumber))

                //.ForMember(dest => dest.VisitNumber, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Assignment")
                //                                                                                                ? (Convert.ToBoolean(context.Options.Items["isVisitNumber"])) ? (((List<DbModels.Assignment>)context.Options.Items["Assignment"])?
                //                                                                                                                                                                        .FirstOrDefault(x => x.Id == src.VisitAssignmentId)
                //                                                                                                                                                                        .Visit.Count > 0)
                //                                                                                                                                                                    ? ((List<DbModels.Assignment>)context.Options.Items["Assignment"])?
                //                                                                                                                                                                        .FirstOrDefault(x => x.Id == src.VisitAssignmentId)
                //                                                                                                                                                                        .Visit.Max(x => x.VisitNumber + 1) : 1
                //                                                                                                                                                                   : src.VisitNumber
                //                                                                                                : null))
                .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.VisitAssignmentId))
                .ForMember(dest => dest.IsFinalVisit, src => src.MapFrom(x => x.IsFinalVisit ?? false))
                .ForMember(dest => dest.PercentageCompleted, src => src.MapFrom(x => x.VisitCompletedPercentage))
                .ForMember(dest => dest.DatePeriod, src => src.MapFrom(x => x.VisitDatePeriod))
                .ForMember(dest => dest.ToDate, src => src.MapFrom(x => x.VisitEndDate))
                .ForMember(dest => dest.ExpectedCompleteDate, src => src.MapFrom(x => x.VisitExpectedCompleteDate))
                .ForMember(dest => dest.NotificationReference, src => src.MapFrom(x => x.VisitNotificationReference))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => Convert.ToInt32(x.UpdateCount)))
                .ForMember(dest => dest.Reference1, src => src.MapFrom(x => x.VisitReference1))
                .ForMember(dest => dest.Reference2, src => src.MapFrom(x => x.VisitReference2))
                .ForMember(dest => dest.Reference3, src => src.MapFrom(x => x.VisitReference3))
                .ForMember(dest => dest.RejectionReason, src => src.MapFrom(x => x.VisitRejectionReason))
                .ForMember(dest => dest.ReportNumber, src => src.MapFrom(x => x.VisitReportNumber))
                .ForMember(dest => dest.ReportSentToCustomerDate, src => src.MapFrom(x => x.VisitReportSentToCustomerDate))
                .ForMember(dest => dest.ReviewedDate, src => src.MapFrom(x => x.VisitReviewDateByClient))
                .ForMember(dest => dest.ClientReviewStatus, src => src.MapFrom(x => x.VisitReviewStatusByClient))
                .ForMember(dest => dest.FromDate, src => src.MapFrom(x => x.VisitStartDate))
                .ForMember(dest => dest.VisitStatus, src => src.MapFrom(x => x.VisitStatus))
                .ForMember(dest => dest.UnusedReason, src => src.MapFrom(x => x.UnusedReason))
                .ForMember(dest => dest.SupplierId, src => src.MapFrom(x => x.SupplierId))
                .ForMember(dest => dest.IsApprovedByContractCompany, src => src.MapFrom(x => x.VisitApprovedByContractCompany))
                .ForMember(dest => dest.ReviewedDate, src => src.MapFrom(x => x.VisitReviewDateByClient))
                .ForMember(dest => dest.ReviewedBy, src => src.MapFrom(x => x.VisitReviewedByClient))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.SummaryOfReport, src=> src.MapFrom(x =>x.SummaryOfReport))
                //.ForMember(dest => dest.Evoid, src => src.MapFrom(x => x.Evoid==null?0: x.Evoid)) //Added for audit
                .ForAllOtherMembers(src => src.Ignore());

            #endregion

            #region Visit Document

            CreateMap<DbModels.VisitDocument, DomainModels.VisitDocuments>()
                .ForMember(dest => dest.VisitDocumentId, source => source.MapFrom(x => x.Id))
                .ForMember(dest => dest.VisitId, source => source.MapFrom(x => x.VisitId))
                .ForMember(dest => dest.Name, source => source.MapFrom(x => x.Name))
                .ForMember(dest => dest.DocumentType, source => source.MapFrom(x => x.DocumentType))
                .ForMember(dest => dest.VisibleToCustomer, source => source.MapFrom(x => x.IsVisibleToCustomer))
                .ForMember(dest => dest.VisibleToTS, source => source.MapFrom(x => x.IsVisibleToTechnicalSpecialist))
                .ForMember(dest => dest.DocumentSize, source => source.MapFrom(x => x.DocumentSize))
                .ForMember(dest => dest.UploadedOn, source => source.MapFrom(x => x.UploadedOn))
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Visit Inter Company Discounts

            CreateMap<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCompanyDiscounts>()
                .ForMember(dest => dest.VisitId, src => src.MapFrom(x => x.VisitId))
                .ForMember(dest => dest.DiscountType, src => src.MapFrom(x => x.DiscountType))
                .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Description))
                .ForMember(dest => dest.DiscountPercentage, src => src.MapFrom(x => x.Percentage))
                .ForMember(dest => dest.CompanyCode, src => src.MapFrom(x => x.Company.Code))
                .ForMember(dest => dest.CompanyName, src => src.MapFrom(x => x.Company.Name))
                .ForMember(dest => dest.VisitInterCompanyDiscountId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.AmendmentReason, src => src.MapFrom(x => x.AmendmentReason))
                .ForAllOtherMembers(src => src.Ignore());

            CreateMap<IList<DbModels.VisitInterCompanyDiscount>, DomainModels.VisitInterCoDiscountInfo>()
                // For Additional InterCompany Office 1
                .ForMember(dest => dest.AssignmentAdditionalIntercompany1_Code, src => src.ResolveUsing(new VisitInterCoDiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyCode), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId))))
                .ForMember(dest => dest.AssignmentAdditionalIntercompany1_Description, src => src.ResolveUsing(new VisitInterCoDiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.Description), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId))))
                .ForMember(dest => dest.AssignmentAdditionalIntercompany1_Name, src => src.ResolveUsing(new VisitInterCoDiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyName), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId))))
                .ForMember(dest => dest.AssignmentAdditionalIntercompany1_Discount, src => src.ResolveUsing(new DiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId))))
                // For Additional InterCompany Office 2
                .ForMember(dest => dest.AssignmentAdditionalIntercompany2_Code, src => src.ResolveUsing(new VisitInterCoDiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyCode), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2))))
                .ForMember(dest => dest.AssignmentAdditionalIntercompany2_Description, src => src.ResolveUsing(new VisitInterCoDiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.Description), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2))))
                .ForMember(dest => dest.AssignmentAdditionalIntercompany2_Name, src => src.ResolveUsing(new VisitInterCoDiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyName), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2))))
                .ForMember(dest => dest.AssignmentAdditionalIntercompany2_Discount, src => src.ResolveUsing(new DiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2))))
                // For Parent Contract Holding Company
                .ForMember(dest => dest.ParentContractHoldingCompanyCode, src => src.ResolveUsing(new VisitInterCoDiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyCode), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.ParentContract))))
                .ForMember(dest => dest.ParentContractHoldingCompanyDescription, src => src.ResolveUsing(new VisitInterCoDiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.Description), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.ParentContract))))
                .ForMember(dest => dest.ParentContractHoldingCompanyName, src => src.ResolveUsing(new VisitInterCoDiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyName), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.ParentContract))))
                .ForMember(dest => dest.ParentContractHoldingCompanyDiscount, src => src.ResolveUsing(new DiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.ParentContract))))
                // For Contract Holding Company Of Assignment
                .ForMember(dest => dest.AssignmentContractHoldingCompanyCode, src => src.ResolveUsing(new VisitInterCoDiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyCode), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.Contract))))
                .ForMember(dest => dest.AssignmentContractHoldingCompanyDescription, src => src.ResolveUsing(new VisitInterCoDiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.Description), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.Contract))))
                .ForMember(dest => dest.AssignmentContractHoldingCompanyName, src => src.ResolveUsing(new VisitInterCoDiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyName), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.Contract))))
                .ForMember(dest => dest.AssignmentContractHoldingCompanyDiscount, src => src.ResolveUsing(new DiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.Contract))))
                .ForMember(dest => dest.AmendmentReason, src => src.ResolveUsing(new VisitInterCoDiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.AmendmentReason), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.Contract))))

                // For Host Company
                .ForMember(dest => dest.AssignmentHostcompanyCode, src => src.ResolveUsing(new VisitInterCoDiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyCode), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.OperatingCountryCompany))))
                .ForMember(dest => dest.AssignmentHostcompanyDescription, src => src.ResolveUsing(new VisitInterCoDiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.Description), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.OperatingCountryCompany))))
                .ForMember(dest => dest.AssignmentHostcompanyName, src => src.ResolveUsing(new VisitInterCoDiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCo.CompanyName), EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.OperatingCountryCompany))))
                .ForMember(dest => dest.AssignmentHostcompanyDiscount, src => src.ResolveUsing(new DiscountResolver<DbModels.VisitInterCompanyDiscount, DomainModels.VisitInterCoDiscountInfo>(EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.OperatingCountryCompany))))
                .ForAllOtherMembers(src => src.Ignore());

            #endregion

            #region Visit Notes

            CreateMap<DbModels.VisitNote, DomainModels.VisitNote>()
                .ForMember(dest => dest.VisitNoteId, source => source.MapFrom(x => x.Id))
                .ForMember(dest => dest.VisitId, source => source.MapFrom(x => x.VisitId))
                .ForMember(dest => dest.Note, source => source.MapFrom(x => x.Note))
                .ForMember(dest => dest.VisibleToCustomer, source => source.MapFrom(x => x.IsCustomerVisible))
                .ForMember(dest => dest.VisibleToSpecialist, source => source.MapFrom(x => x.IsSpecialistVisible))
                .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
                .ForMember(dest => dest.CreatedOn, source => source.MapFrom(x => x.CreatedDate))
                .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => Convert.ToInt32(x.UpdateCount)))
                .ForAllOtherMembers(x => x.Ignore());


            CreateMap<DomainModels.VisitNote, DbModels.VisitNote>()
           .ForMember(dest => dest.Id, source => source.MapFrom(x => x.VisitNoteId))
           .ForMember(dest => dest.VisitId, source => source.MapFrom(x => x.VisitId))
           .ForMember(dest => dest.Note, source => source.MapFrom(x => x.Note))
           .ForMember(dest => dest.IsCustomerVisible, source => source.MapFrom(x => x.VisibleToCustomer))
           .ForMember(dest => dest.IsSpecialistVisible, source => source.MapFrom(x => x.VisibleToSpecialist))
           .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
           .ForMember(dest => dest.CreatedDate, source => source.MapFrom(x => x.CreatedOn))
           .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
           .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
           .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
           .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Visit References

            CreateMap<DbModels.VisitReference, DomainModels.VisitReference>()
                     .ForMember(dest => dest.VisitReferenceId, source => source.MapFrom(x => x.Id))
                     .ForMember(dest => dest.VisitId, source => source.MapFrom(x => x.VisitId))
                     .ForMember(dest => dest.AssignmentReferenceTypeId, source => source.MapFrom(x => x.AssignmentReferenceTypeId)) //Added for insert AssignmentReferenceType in SkeletonVisit
                     .ForMember(dest => dest.ReferenceType, source => source.MapFrom(x => x.AssignmentReferenceType.Name))
                     .ForMember(dest => dest.ReferenceValue, source => source.MapFrom(x => x.ReferenceValue))
                     .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                     .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                     .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                     .ForMember(dest => dest.Evoid, src => src.MapFrom(x => x.Evoid)) // These needs to be removed once DB sync done
                     .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModels.VisitReference, DbModels.VisitReference>()
               .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isVisitId") ? (Convert.ToBoolean(context.Options.Items["isVisitId"]) ? src.VisitReferenceId : null) : src.VisitReferenceId)))
               .ForMember(dest => dest.VisitId, source => source.MapFrom(x => x.VisitId))
               .ForMember(dest => dest.AssignmentReferenceTypeId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("ReferenceTypes") ? ((List<DbModels.Data>)context.Options.Items["ReferenceTypes"])?.FirstOrDefault(x => x.Name == src.ReferenceType)?.Id : null))
               .ForMember(dest => dest.ReferenceValue, source => source.MapFrom(x => x.ReferenceValue))
               .ForMember(dest => dest.UpdateCount, opt => opt.MapFrom(src => src.UpdateCount))
               .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
               .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
               .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Visit Supplier Performance

            CreateMap<DbModels.VisitSupplierPerformance, DomainModels.VisitSupplierPerformanceType>()
                .ForMember(dest => dest.VisitSupplierPerformanceTypeId, source => source.MapFrom(x => x.Id))
                .ForMember(dest => dest.VisitId, source => source.MapFrom(x => x.VisitId))
                .ForMember(dest => dest.SupplierPerformance, source => source.MapFrom(x => x.PerformanceType))
                .ForMember(dest => dest.NCRReference, source => source.MapFrom(x => x.Score))
                .ForMember(dest => dest.NCRCloseOutDate, source => source.MapFrom(x => x.NcrcloseOutDate))
                .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => Convert.ToInt32(x.UpdateCount)))
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Visit Technical Specialists

            CreateMap<DbModels.VisitTechnicalSpecialist, DomainModels.VisitTechnicalSpecialist>()
                .ForMember(dest => dest.VisitTechnicalSpecialistId, source => source.MapFrom(x => x.Id))
                .ForMember(dest => dest.VisitId, source => source.MapFrom(x => x.VisitId))                
                //.ForMember(dest => dest.TechnicalSpecialistName, source => source.MapFrom(x => x.TechnicalSpecialist.FirstName))
                .ForMember(dest => dest.TechnicalSpecialistName, source => source.MapFrom(x => string.Format("{0}, {1}", x.TechnicalSpecialist.LastName, x.TechnicalSpecialist.FirstName)))
                .ForMember(dest => dest.LoginName, source => source.MapFrom(x => x.TechnicalSpecialist.LogInName))
                .ForMember(dest => dest.FullName, source => source.MapFrom(x => string.Format("{0}, {1} ({2})", x.TechnicalSpecialist.LastName, x.TechnicalSpecialist.FirstName, x.TechnicalSpecialist.Pin)))
                //.ForMember(dest => dest.GrossMargin, source => source.MapFrom(x => x.GrossMargin))
                .ForMember(dest => dest.Pin, source => source.MapFrom(x => x.TechnicalSpecialistId))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModels.VisitTechnicalSpecialist, DbModels.VisitTechnicalSpecialist>()
                .ForMember(dest => dest.Id, source => source.MapFrom(x => x.VisitTechnicalSpecialistId))
                .ForMember(dest => dest.VisitId, source => source.MapFrom(x => x.VisitId))
                .ForMember(dest => dest.TechnicalSpecialistId, source => source.MapFrom(x => x.Pin))
                //.ForMember(dest => dest.GrossMargin, source => source.MapFrom(x => x.GrossMargin))
                .ForMember(dest => dest.UpdateCount, opt => opt.MapFrom(src => src.UpdateCount))
                .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Visit Technical Specialist Account Item Consumable

            CreateMap<DbModels.VisitTechnicalSpecialistAccountItemConsumable, DomainModels.VisitSpecialistAccountItemConsumable>()
                .ForMember(dest => dest.VisitId, source => source.MapFrom(x => x.VisitId))
                .ForMember(dest => dest.VisitTechnicalSpecialistAccountConsumableId, source => source.MapFrom(x => x.Id))
                .ForMember(dest => dest.VisitTechnicalSpecialistId, source => source.MapFrom(x => x.VisitTechnicalSpecialistId))
                .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.ExpenseDate, source => source.MapFrom(x => x.ExpenceDate))
                .ForMember(dest => dest.ChargeExpenseType, source => source.MapFrom(x => x.ChargeExpenseType.Name))
                .ForMember(dest => dest.ChargeDescription, source => source.MapFrom(x => x.ChargeDescription))
                .ForMember(dest => dest.IsInvoicePrintChargeDescription, source => source.MapFrom(x => x.IsDescriptionPrintedOnInvoice))
                .ForMember(dest => dest.ChargeTotalUnit, source => source.MapFrom(x => x.ChargeTotalUnit))
                .ForMember(dest => dest.ChargeRate, source => source.MapFrom(x => x.ChargeRate))
                .ForMember(dest => dest.ChargeRateCurrency, source => source.MapFrom(x => x.ChargeRateCurrency))
                .ForMember(dest => dest.PayExpenseType, source => source.MapFrom(x => x.PayExpenseType.Name))
                .ForMember(dest => dest.PayUnit, source => source.MapFrom(x => x.PayTotalUnit))
                .ForMember(dest => dest.PayRate, source => source.MapFrom(x => x.PayRate))
                .ForMember(dest => dest.PayRateCurrency, source => source.MapFrom(x => x.PayRateCurrency))
                .ForMember(dest => dest.PayRateDescription, source => source.MapFrom(x => x.PayRateDescription))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.Pin, source => source.MapFrom(x => x.VisitTechnicalSpecialist.TechnicalSpecialistId))
                .ForMember(dest => dest.ProjectNumber, source => source.MapFrom(x => x.Project.ProjectNumber))
                .ForMember(dest => dest.ContractNumber, source => source.MapFrom(x => x.Contract.ContractNumber))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.InvoiceId, src => src.MapFrom(x => x.InvoiceId))
                .ForMember(dest => dest.InvoicingStatus, src => src.MapFrom(x => x.InvoicingStatus))
                .ForMember(dest => dest.CostofSalesBatchId, src => src.MapFrom(x => x.CostofSalesBatchId))
                .ForMember(dest => dest.CostofSalesStatus, src => src.MapFrom(x => x.CostofSalesStatus))
                .ForMember(dest => dest.UnPaidStatus, src => src.MapFrom(x => x.UnpaidStatus.Name))
                .ForMember(dest => dest.UnPaidStatusReason, src => src.MapFrom(x => x.UnpaidReason.Resason))
                .ForMember(dest => dest.ModeOfCreation, src => src.MapFrom(x => x.ModeOfCreation))
                .ForMember(dest => dest.ChargeRateId, src => src.MapFrom(x => x.ChargeRateId))
                .ForMember(dest => dest.PayRateId, src => src.MapFrom(x => x.PayRateId))
                .ForMember(dest => dest.SalesTaxId, src => src.MapFrom(x => x.SalesTaxId))
                .ForMember(dest => dest.WithholdingTaxId, src => src.MapFrom(x => x.WithholdingTaxId))
                // .ForMember(dest => dest.InvoicingStatus, src => src.MapFrom(x => "N"))  //To be taken from Invoice Status enum when finance module comes into place
                .ForMember(dest => dest.Evoid, src => src.MapFrom(x => x.Evoid)) // These needs to be removed once DB sync done
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModels.VisitSpecialistAccountItemConsumable, DbModels.VisitTechnicalSpecialistAccountItemConsumable>()
                .ForMember(dest => dest.VisitId, source => source.MapFrom(x => x.VisitId))
                .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isVisitTSConsumableId") ? (Convert.ToBoolean(context.Options.Items["isVisitTSConsumableId"]) ? src.VisitTechnicalSpecialistAccountConsumableId : null) : src.VisitTechnicalSpecialistAccountConsumableId)))               
                .ForMember(dest => dest.VisitTechnicalSpecialistId, source => source.MapFrom(x => x.VisitTechnicalSpecialistId))
                .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.ProjectId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Project") ? ((IList<DbModels.Project>)context.Options.Items["Project"])?.FirstOrDefault(x => x.ProjectNumber == src.ProjectNumber)?.Id : null))
                .ForMember(dest => dest.ContractId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Contract") ? ((IList<DbModels.Contract>)context.Options.Items["Contract"])?.FirstOrDefault(x => x.ContractNumber == src.ContractNumber)?.Id : null))
                .ForMember(dest => dest.ExpenceDate, source => source.MapFrom(x => x.ExpenseDate))               
                .ForMember(dest => dest.ChargeExpenseTypeId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("ExpenseType") ? ((IList<DbModels.Data>)context.Options.Items["ExpenseType"])?.FirstOrDefault(x => x.Name == src.ChargeExpenseType && x.MasterDataTypeId == (int)MasterType.ExpenseType)?.Id : null))
                .ForMember(dest => dest.ChargeDescription, source => source.MapFrom(x => x.ChargeDescription))
                .ForMember(dest => dest.IsDescriptionPrintedOnInvoice, source => source.MapFrom(x => x.IsInvoicePrintChargeDescription))
                .ForMember(dest => dest.ChargeTotalUnit, source => source.MapFrom(x => x.ChargeTotalUnit))
                .ForMember(dest => dest.ChargeRate, source => source.MapFrom(x => x.ChargeRate))
                .ForMember(dest => dest.ChargeRateCurrency, source => source.MapFrom(x => x.ChargeRateCurrency))               
                .ForMember(dest => dest.PayExpenseTypeId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("ExpenseType") ? ((IList<DbModels.Data>)context.Options.Items["ExpenseType"])?.FirstOrDefault(x => x.Name == src.PayExpenseType && x.MasterDataTypeId == (int)MasterType.ExpenseType)?.Id : null))
                .ForMember(dest => dest.PayTotalUnit, source => source.MapFrom(x => x.PayUnit))
                .ForMember(dest => dest.PayRate, source => source.MapFrom(x => x.PayRate))
                .ForMember(dest => dest.PayRateCurrency, source => source.MapFrom(x => x.PayRateCurrency))
                .ForMember(dest => dest.PayRateDescription, source => source.MapFrom(x => x.PayRateDescription))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.InvoicingStatus, src => src.MapFrom(x => string.IsNullOrEmpty(x.InvoicingStatus) ? "N" : x.InvoicingStatus))  //To be taken from Invoice Status enum when finance module comes into place
                .ForMember(dest => dest.SalesTaxId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Project") ? ((IList<DbModels.Project>)context.Options.Items["Project"])?.FirstOrDefault(x => x.ProjectNumber == src.ProjectNumber)?.InvoiceSalesTaxId : null)) //D959
                .ForMember(dest => dest.WithholdingTaxId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Project") ? ((IList<DbModels.Project>)context.Options.Items["Project"])?.FirstOrDefault(x => x.ProjectNumber == src.ProjectNumber)?.InvoiceWithholdingTaxId : null)) //D959
                .ForMember(dest => dest.ChargeRateId, src => src.MapFrom(x => x.ChargeRateId))
                .ForMember(dest => dest.PayRateId, src => src.MapFrom(x => x.PayRateId))
                .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Visit Technical Specialist Account Item Expense

            CreateMap<DbModels.VisitTechnicalSpecialistAccountItemExpense, DomainModels.VisitSpecialistAccountItemExpense>()
                .ForMember(dest => dest.VisitTechnicalSpecialistAccountExpenseId, source => source.MapFrom(x => x.Id))
                .ForMember(dest => dest.VisitId, source => source.MapFrom(x => x.VisitId))
                .ForMember(dest => dest.VisitTechnicalSpecialistId, source => source.MapFrom(x => x.VisitTechnicalSpeciallistId))
                .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.ChargeExpenseType, source => source.MapFrom(x => x.ExpenseChargeType.Name))
                .ForMember(dest => dest.ExpenseDate, source => source.MapFrom(x => x.ExpenseDate))
                .ForMember(dest => dest.ExpenseDescription, source => source.MapFrom(x => x.ExpenseDescription))
                .ForMember(dest => dest.Currency, source => source.MapFrom(x => x.ExpenceCurrency))
                .ForMember(dest => dest.IsContractHolderExpense, source => source.MapFrom(x => x.IsContractHolderExpense))
                .ForMember(dest => dest.ChargeUnit, source => source.MapFrom(x => x.ChargeTotalUnit))
                .ForMember(dest => dest.ChargeRate, source => source.MapFrom(x => x.ChargeRate))
                .ForMember(dest => dest.ChargeRateExchange, source => source.MapFrom(x => x.ChargeExchangeRate))
                .ForMember(dest => dest.ChargeRateCurrency, source => source.MapFrom(x => x.ChargeRateCurrency))
                .ForMember(dest => dest.PayUnit, source => source.MapFrom(x => x.PayTotalUnit))
                .ForMember(dest => dest.PayRateTax, source => source.MapFrom(x => x.PayRateTax))
                .ForMember(dest => dest.PayRate, source => source.MapFrom(x => x.PayRate))
                .ForMember(dest => dest.PayRateCurrency, source => source.MapFrom(x => x.PayRateCurrency))
                .ForMember(dest => dest.PayRateExchange, source => source.MapFrom(x => x.PayExchangeRate))
                .ForMember(dest => dest.ProjectNumber, source => source.MapFrom(x => x.Project.ProjectNumber))
                .ForMember(dest => dest.ContractNumber, source => source.MapFrom(x => x.Contract.ContractNumber))
                .ForMember(dest => dest.Pin, source => source.MapFrom(x => x.VisitTechnicalSpeciallist.TechnicalSpecialistId))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.InvoiceId, src => src.MapFrom(x => x.InvoiceId))
                .ForMember(dest => dest.InvoicingStatus, src => src.MapFrom(x => x.InvoicingStatus))
                .ForMember(dest => dest.CostofSalesBatchId, src => src.MapFrom(x => x.CostofSalesBatchId))
                .ForMember(dest => dest.CostofSalesStatus, src => src.MapFrom(x => x.CostofSalesStatus))
                .ForMember(dest => dest.UnPaidStatus, src => src.MapFrom(x => x.UnpaidStatus.Name))
                .ForMember(dest => dest.UnPaidStatusReason, src => src.MapFrom(x => x.UnpaidReason.Resason))
                .ForMember(dest => dest.ModeOfCreation, src => src.MapFrom(x => x.ModeOfCreation))
                .ForMember(dest => dest.ChargeRateId, src => src.MapFrom(x => x.ChargeRateId))
                .ForMember(dest => dest.PayRateId, src => src.MapFrom(x => x.PayRateId))
                .ForMember(dest => dest.SalesTaxId, src => src.MapFrom(x => x.SalesTaxId))
                .ForMember(dest => dest.WithholdingTaxId, src => src.MapFrom(x => x.WithholdingTaxId))
               // .ForMember(dest => dest.InvoicingStatus, src => src.MapFrom(x => "N"))  //To be taken from Invoice Status enum when finance module comes into place
               .ForMember(dest => dest.Evoid, src => src.MapFrom(x => x.Evoid)) // These needs to be removed once DB sync done
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModels.VisitSpecialistAccountItemExpense, DbModels.VisitTechnicalSpecialistAccountItemExpense>()
              .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isVisitTSExpenseId") ? (Convert.ToBoolean(context.Options.Items["isVisitTSExpenseId"]) ? src.VisitTechnicalSpecialistAccountExpenseId : null) : src.VisitTechnicalSpecialistAccountExpenseId)))
              .ForMember(dest => dest.VisitId, source => source.MapFrom(x => x.VisitId))
              .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
              .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
              .ForMember(dest => dest.VisitTechnicalSpeciallistId, source => source.MapFrom(x => x.VisitTechnicalSpecialistId))
              .ForMember(dest => dest.ProjectId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Project") ? ((IList<DbModels.Project>)context.Options.Items["Project"])?.FirstOrDefault(x => x.ProjectNumber == src.ProjectNumber)?.Id : null))
              .ForMember(dest => dest.ContractId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Contract") ? ((IList<DbModels.Contract>)context.Options.Items["Contract"])?.FirstOrDefault(x => x.ContractNumber == src.ContractNumber)?.Id : null))
              .ForMember(dest => dest.ExpenseChargeTypeId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("ExpenseType") ? ((IList<DbModels.Data>)context.Options.Items["ExpenseType"])?.FirstOrDefault(x => x.Name == src.ChargeExpenseType && x.MasterDataTypeId == (int)MasterType.ExpenseType)?.Id : null))
              .ForMember(dest => dest.ExpenseDate, source => source.MapFrom(x => x.ExpenseDate))
              .ForMember(dest => dest.ExpenseDescription, source => source.MapFrom(x => !string.IsNullOrEmpty(x.ExpenseDescription) ? new string(x.ExpenseDescription.Take(50).ToArray()) : x.ExpenseDescription))
              .ForMember(dest => dest.ExpenceCurrency, source => source.MapFrom(x => x.Currency))
              .ForMember(dest => dest.IsContractHolderExpense, source => source.MapFrom(x => x.IsContractHolderExpense))
              .ForMember(dest => dest.ChargeTotalUnit, source => source.MapFrom(x => x.ChargeUnit))
              .ForMember(dest => dest.ChargeRate, source => source.MapFrom(x => x.ChargeRate))
              .ForMember(dest => dest.ChargeExchangeRate, source => source.MapFrom(x => x.ChargeRateExchange))
              .ForMember(dest => dest.ChargeRateCurrency, source => source.MapFrom(x => x.ChargeRateCurrency))
              .ForMember(dest => dest.PayTotalUnit, source => source.MapFrom(x => x.PayUnit))
              .ForMember(dest => dest.PayRateTax, source => source.MapFrom(x => x.PayRateTax))
              .ForMember(dest => dest.PayRate, source => source.MapFrom(x => x.PayRate))
              .ForMember(dest => dest.PayRateCurrency, source => source.MapFrom(x => x.PayRateCurrency))
              .ForMember(dest => dest.PayExchangeRate, source => source.MapFrom(x => x.PayRateExchange))
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
              .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
              .ForMember(dest => dest.InvoicingStatus, src => src.MapFrom(x => string.IsNullOrEmpty(x.InvoicingStatus) ? "N" : x.InvoicingStatus))  //To be taken from Invoice Status enum when finance module comes into place
              .ForMember(dest => dest.SalesTaxId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Project") ? ((IList<DbModels.Project>)context.Options.Items["Project"])?.FirstOrDefault(x => x.ProjectNumber == src.ProjectNumber)?.InvoiceSalesTaxId : null)) //D959
              .ForMember(dest => dest.WithholdingTaxId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Project") ? ((IList<DbModels.Project>)context.Options.Items["Project"])?.FirstOrDefault(x => x.ProjectNumber == src.ProjectNumber)?.InvoiceWithholdingTaxId : null)) //D959
              .ForMember(dest => dest.ChargeRateId, src => src.MapFrom(x => x.ChargeRateId))
              .ForMember(dest => dest.PayRateId, src => src.MapFrom(x => x.PayRateId))
              .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Visit Technical Specialist Account Item Time

            CreateMap<DbModels.VisitTechnicalSpecialistAccountItemTime, DomainModels.VisitSpecialistAccountItemTime>()
                .ForMember(dest => dest.VisitTechnicalSpecialistAccountTimeId, source => source.MapFrom(x => x.Id))
                .ForMember(dest => dest.VisitId, source => source.MapFrom(x => x.VisitId))
                .ForMember(dest => dest.VisitTechnicalSpecialistId, source => source.MapFrom(x => x.VisitTechnicalSpeciallistId))
                .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.ChargeExpenseType, source => source.MapFrom(x => x.ExpenseChargeType.Name))
                .ForMember(dest => dest.ExpenseDate, source => source.MapFrom(x => x.ExpenseDate))
                .ForMember(dest => dest.ExpenseDescription, source => source.MapFrom(x => x.TimeDescription))
                .ForMember(dest => dest.IsInvoicePrintExpenseDescription, source => source.MapFrom(x => x.IsTimeDescriptionPrinted))
                .ForMember(dest => dest.ChargeWorkUnit, source => source.MapFrom(x => x.ChargeWorkUnit))
                .ForMember(dest => dest.ChargeTravelUnit, source => source.MapFrom(x => x.ChargeTravelUnit))
                .ForMember(dest => dest.ChargeWaitUnit, source => source.MapFrom(x => x.ChargeWaitUnit))
                .ForMember(dest => dest.ChargeReportUnit, source => source.MapFrom(x => x.ChargeReportUnit))
                .ForMember(dest => dest.ChargeTotalUnit, source => source.MapFrom(x => x.ChargeTotalUnit))
                .ForMember(dest => dest.ChargeRate, source => source.MapFrom(x => x.ChargeRate))
                .ForMember(dest => dest.ChargeRateCurrency, source => source.MapFrom(x => x.ChargeRateCurrency))
                .ForMember(dest => dest.ChargeRateDescription, source => source.MapFrom(x => x.ChargeRateDescription))
                .ForMember(dest => dest.IsInvoicePrintPayRateDescrition, source => source.MapFrom(x => x.IsDescriptionPrintedOnInvoice))
                .ForMember(dest => dest.PayUnit, source => source.MapFrom(x => x.PayTotalUnit))
                .ForMember(dest => dest.PayRate, source => source.MapFrom(x => x.PayRate))
                .ForMember(dest => dest.PayRateCurrency, source => source.MapFrom(x => x.PayRateCurrency))
                .ForMember(dest => dest.PayRateDescription, source => source.MapFrom(x => x.PayRateDescription))
                .ForMember(dest => dest.ProjectNumber, source => source.MapFrom(x => x.Project.ProjectNumber))
                .ForMember(dest => dest.ContractNumber, source => source.MapFrom(x => x.Contract.ContractNumber))
                .ForMember(dest => dest.Pin, source => source.MapFrom(x => x.VisitTechnicalSpeciallist.TechnicalSpecialistId))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.InvoiceId, src => src.MapFrom(x => x.InvoiceId))
                .ForMember(dest => dest.InvoicingStatus, src => src.MapFrom(x => x.InvoicingStatus))
                .ForMember(dest => dest.CostofSalesBatchId, src => src.MapFrom(x => x.CostofSalesBatchId))
                .ForMember(dest => dest.CostofSalesStatus, src => src.MapFrom(x => x.CostofSalesStatus))
                .ForMember(dest => dest.UnPaidStatus, src => src.MapFrom(x => x.UnpaidStatus.Name))
                .ForMember(dest => dest.UnPaidStatusReason, src => src.MapFrom(x => x.UnpaidReason.Resason))
                .ForMember(dest => dest.ModeOfCreation, src => src.MapFrom(x => x.ModeOfCreation))
                .ForMember(dest => dest.ChargeRateId, src => src.MapFrom(x => x.ChargeRateId))
                .ForMember(dest => dest.PayRateId, src => src.MapFrom(x => x.PayRateId))
                .ForMember(dest => dest.SalesTaxId, src => src.MapFrom(x => x.SalesTaxId))
                .ForMember(dest => dest.WithholdingTaxId, src => src.MapFrom(x => x.WithholdingTaxId))
               // .ForMember(dest => dest.InvoicingStatus, src => src.MapFrom(x => "N"))  //To be taken from Invoice Status enum when finance module comes into place
               .ForMember(dest => dest.Evoid, src => src.MapFrom(x => x.Evoid)) // These needs to be removed once DB sync done
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModels.VisitSpecialistAccountItemTime, DbModels.VisitTechnicalSpecialistAccountItemTime>()
                .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isVisitTSTimeId") ? (Convert.ToBoolean(context.Options.Items["isVisitTSTimeId"]) ? src.VisitTechnicalSpecialistAccountTimeId : null) : src.VisitTechnicalSpecialistAccountTimeId)))
                .ForMember(dest => dest.VisitId, source => source.MapFrom(x => x.VisitId))
                .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.ProjectId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Project") ? ((IList<DbModels.Project>)context.Options.Items["Project"])?.FirstOrDefault(x => x.ProjectNumber == src.ProjectNumber)?.Id : null))
                .ForMember(dest => dest.ContractId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Contract") ? ((IList<DbModels.Contract>)context.Options.Items["Contract"])?.FirstOrDefault(x => x.ContractNumber == src.ContractNumber)?.Id : null))
                .ForMember(dest => dest.ExpenseChargeTypeId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("ExpenseType") ? ((IList<DbModels.Data>)context.Options.Items["ExpenseType"])?.FirstOrDefault(x => x.Name == src.ChargeExpenseType && x.MasterDataTypeId == (int)MasterType.ExpenseType)?.Id : null))
                .ForMember(dest => dest.VisitTechnicalSpeciallistId, source => source.MapFrom(x => x.VisitTechnicalSpecialistId))
                .ForMember(dest => dest.ExpenseDate, source => source.MapFrom(x => x.ExpenseDate))
                .ForMember(dest => dest.TimeDescription, source => source.MapFrom(x => x.ExpenseDescription))
                .ForMember(dest => dest.IsTimeDescriptionPrinted, source => source.MapFrom(x => x.IsInvoicePrintExpenseDescription))
                .ForMember(dest => dest.ChargeWorkUnit, source => source.MapFrom(x => x.ChargeWorkUnit))
                .ForMember(dest => dest.ChargeTravelUnit, source => source.MapFrom(x => x.ChargeTravelUnit))
                .ForMember(dest => dest.ChargeWaitUnit, source => source.MapFrom(x => x.ChargeWaitUnit))
                .ForMember(dest => dest.ChargeReportUnit, source => source.MapFrom(x => x.ChargeReportUnit))
                .ForMember(dest => dest.ChargeTotalUnit, source => source.MapFrom(x => x.ChargeTotalUnit))
                .ForMember(dest => dest.ChargeRate, source => source.MapFrom(x => x.ChargeRate))
                .ForMember(dest => dest.ChargeRateCurrency, source => source.MapFrom(x => x.ChargeRateCurrency))
                .ForMember(dest => dest.ChargeRateDescription, source => source.MapFrom(x => x.ChargeRateDescription))
                .ForMember(dest => dest.IsDescriptionPrintedOnInvoice, source => source.MapFrom(x => x.IsInvoicePrintPayRateDescrition))
                .ForMember(dest => dest.PayTotalUnit, source => source.MapFrom(x => x.PayUnit))
                .ForMember(dest => dest.PayRate, source => source.MapFrom(x => x.PayRate))
                .ForMember(dest => dest.PayRateCurrency, source => source.MapFrom(x => x.PayRateCurrency))
                .ForMember(dest => dest.PayRateDescription, source => source.MapFrom(x => x.PayRateDescription))
                 .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                 .ForMember(dest => dest.InvoicingStatus, src => src.MapFrom(x => string.IsNullOrEmpty(x.InvoicingStatus) ? "N" : x.InvoicingStatus))  //To be taken from Invoice Status enum when finance module comes into place
                .ForMember(dest => dest.SalesTaxId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Project") ? ((IList<DbModels.Project>)context.Options.Items["Project"])?.FirstOrDefault(x => x.ProjectNumber == src.ProjectNumber)?.InvoiceSalesTaxId : null)) //D959
                .ForMember(dest => dest.WithholdingTaxId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Project") ? ((IList<DbModels.Project>)context.Options.Items["Project"])?.FirstOrDefault(x => x.ProjectNumber == src.ProjectNumber)?.InvoiceWithholdingTaxId : null)) //D959                
                .ForMember(dest => dest.ChargeRateId, src => src.MapFrom(x => x.ChargeRateId))
                .ForMember(dest => dest.PayRateId, src => src.MapFrom(x => x.PayRateId))
                .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Visit Technical Specialist Account Item Travel

            CreateMap<DbModels.VisitTechnicalSpecialistAccountItemTravel, DomainModels.VisitSpecialistAccountItemTravel>()
                .ForMember(dest => dest.VisitTechnicalSpecialistAccountTravelId, source => source.MapFrom(x => x.Id))
                .ForMember(dest => dest.VisitId, source => source.MapFrom(x => x.VisitId))
                .ForMember(dest => dest.VisitTechnicalSpecialistId, source => source.MapFrom(x => x.VisitTechnicalSpecialistId))
                .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.ChargeExpenseType, source => source.MapFrom(x => x.ChargeExpenseType.Name))
                .ForMember(dest => dest.ExpenseDate, source => source.MapFrom(x => x.ExpenceDate))
                .ForMember(dest => dest.ExpenseDescription, source => source.MapFrom(x => x.ExpenseDescription))
                .ForMember(dest => dest.IsInvoicePrintExpenseDescription, source => source.MapFrom(x => x.IsDescriptionPrintedOnInvoice))
                .ForMember(dest => dest.ChargeTotalUnit, source => source.MapFrom(x => x.ChargeTotalUnit))
                .ForMember(dest => dest.ChargeRate, source => source.MapFrom(x => x.ChargeRate))
                .ForMember(dest => dest.ChargeRateCurrency, source => source.MapFrom(x => x.ChargeRateCurrency))
                .ForMember(dest => dest.PayExpenseType, source => source.MapFrom(x => x.PayExpenseType.Name))
                .ForMember(dest => dest.PayUnit, source => source.MapFrom(x => x.PayTotalUnit))
                .ForMember(dest => dest.PayRate, source => source.MapFrom(x => x.PayRate))
                .ForMember(dest => dest.PayRateCurrency, source => source.MapFrom(x => x.PayRateCurrency))
                .ForMember(dest => dest.ProjectNumber, source => source.MapFrom(x => x.Project.ProjectNumber))
                .ForMember(dest => dest.ContractNumber, source => source.MapFrom(x => x.Contract.ContractNumber))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.Pin, source => source.MapFrom(x => x.VisitTechnicalSpecialist.TechnicalSpecialistId))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.InvoiceId, src => src.MapFrom(x => x.InvoiceId))
                .ForMember(dest => dest.InvoicingStatus, src => src.MapFrom(x => x.InvoicingStatus))
                .ForMember(dest => dest.CostofSalesBatchId, src => src.MapFrom(x => x.CostofSalesBatchId))
                .ForMember(dest => dest.CostofSalesStatus, src => src.MapFrom(x => x.CostofSalesStatus))
                .ForMember(dest => dest.UnPaidStatus, src => src.MapFrom(x => x.UnpaidStatus.Name))
                .ForMember(dest => dest.UnPaidStatusReason, src => src.MapFrom(x => x.UnpaidReason.Resason))
                .ForMember(dest => dest.ModeOfCreation, src => src.MapFrom(x => x.ModeOfCreation))
                .ForMember(dest => dest.ChargeRateId, src => src.MapFrom(x => x.ChargeRateId))
                .ForMember(dest => dest.PayRateId, src => src.MapFrom(x => x.PayRateId))
                .ForMember(dest => dest.SalesTaxId, src => src.MapFrom(x => x.SalesTaxId))
                .ForMember(dest => dest.WithholdingTaxId, src => src.MapFrom(x => x.WithholdingTaxId))
               // .ForMember(dest => dest.InvoicingStatus, src => src.MapFrom(x => "N"))  //To be taken from Invoice Status enum when finance module comes into place
               .ForMember(dest => dest.Evoid, src => src.MapFrom(x => x.Evoid)) // These needs to be removed once DB sync done
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModels.VisitSpecialistAccountItemTravel, DbModels.VisitTechnicalSpecialistAccountItemTravel>()
                .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isVisitTSTravelId") ? (Convert.ToBoolean(context.Options.Items["isVisitTSTravelId"]) ? src.VisitTechnicalSpecialistAccountTravelId : null) : src.VisitTechnicalSpecialistAccountTravelId)))
                .ForMember(dest => dest.VisitId, source => source.MapFrom(x => x.VisitId))
                .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.ProjectId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Project") ? ((IList<DbModels.Project>)context.Options.Items["Project"])?.FirstOrDefault(x => x.ProjectNumber == src.ProjectNumber)?.Id : null))
                .ForMember(dest => dest.ContractId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Contract") ? ((IList<DbModels.Contract>)context.Options.Items["Contract"])?.FirstOrDefault(x => x.ContractNumber == src.ContractNumber)?.Id : null))
                .ForMember(dest => dest.ChargeExpenseTypeId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("ExpenseType") ? ((IList<DbModels.Data>)context.Options.Items["ExpenseType"])?.FirstOrDefault(x => x.Name == src.ChargeExpenseType && x.MasterDataTypeId == (int)MasterType.ExpenseType)?.Id : null))
                .ForMember(dest => dest.VisitTechnicalSpecialistId, source => source.MapFrom(x => x.VisitTechnicalSpecialistId))
                .ForMember(dest => dest.ExpenceDate, source => source.MapFrom(x => x.ExpenseDate))
                .ForMember(dest => dest.ExpenseDescription, source => source.MapFrom(x => !string.IsNullOrEmpty(x.ExpenseDescription) ? new string(x.ExpenseDescription.Take(50).ToArray()) : x.ExpenseDescription))
                .ForMember(dest => dest.IsDescriptionPrintedOnInvoice, source => source.MapFrom(x => x.IsInvoicePrintExpenseDescription))
                .ForMember(dest => dest.PayExpenseTypeId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("ExpenseType") ? ((IList<DbModels.Data>)context.Options.Items["ExpenseType"])?.FirstOrDefault(x => x.Name == src.PayExpenseType && x.MasterDataTypeId == (int)MasterType.ExpenseType)?.Id : null))
                .ForMember(dest => dest.ChargeTotalUnit, source => source.MapFrom(x => x.ChargeTotalUnit))
                .ForMember(dest => dest.ChargeRate, source => source.MapFrom(x => x.ChargeRate))
                .ForMember(dest => dest.ChargeRateCurrency, source => source.MapFrom(x => x.ChargeRateCurrency))
                .ForMember(dest => dest.PayTotalUnit, source => source.MapFrom(x => x.PayUnit))
                .ForMember(dest => dest.PayRate, source => source.MapFrom(x => x.PayRate))
                .ForMember(dest => dest.PayRateCurrency, source => source.MapFrom(x => x.PayRateCurrency))
                 .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                 .ForMember(dest => dest.InvoicingStatus, src => src.MapFrom(x => string.IsNullOrEmpty(x.InvoicingStatus) ? "N" : x.InvoicingStatus))  //To be taken from Invoice Status enum when finance module comes into place
                .ForMember(dest => dest.SalesTaxId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Project") ? ((IList<DbModels.Project>)context.Options.Items["Project"])?.FirstOrDefault(x => x.ProjectNumber == src.ProjectNumber)?.InvoiceSalesTaxId : null)) //D959
                .ForMember(dest => dest.WithholdingTaxId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Project") ? ((IList<DbModels.Project>)context.Options.Items["Project"])?.FirstOrDefault(x => x.ProjectNumber == src.ProjectNumber)?.InvoiceWithholdingTaxId : null)) //D959
                .ForMember(dest => dest.ChargeRateId, src => src.MapFrom(x => x.ChargeRateId))
                .ForMember(dest => dest.PayRateId, src => src.MapFrom(x => x.PayRateId))
                .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Visit Document Search
            CreateMap<DomainModels.VisitSearch, Document.Domain.Models.Document.EvolutionMongoDocumentSearch>()
                .ForMember(dest => dest.ModuleCode, src => src.ResolveUsing(x => Common.Enums.ModuleCodeType.VST.ToString()))
                .ForMember(dest => dest.ReferenceCode, src => src.MapFrom(x => x.VisitId))
                .ForMember(dest => dest.Text, src => src.ResolveUsing<DomResolver.SearchTextFormatResolver, string>("DocumentSearchText"))
                .ForMember(dest => dest.DocumentTypes, src => src.ResolveUsing(x => string.IsNullOrEmpty(x.SearchDocumentType) ? null : new List<string>() { x.SearchDocumentType }))
                .ForAllOtherMembers(x => x.Ignore());
            #endregion
        }
    }
}