using AutoMapper;
using Evolution.Assignment.Domain.Enums;
using Evolution.Common.Enums;
using Evolution.Project.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using DomResolver= Evolution.Automapper.Resolver.MongoSearch;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModels = Evolution.Timesheet.Domain.Models.Timesheets;

namespace Evolution.Timesheet.Core.Mapper
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            #region BaseTimesheet
            CreateMap<DbModels.Timesheet, DomainModels.BaseTimesheet>()
               .ForMember(dest => dest.TimesheetAssignmentId, src => src.MapFrom(x => x.AssignmentId))
               .ForMember(dest => dest.TimesheetAssignmentNumber, src => src.MapFrom(x => x.Assignment.AssignmentNumber))
               .ForMember(dest => dest.AssignmentCreatedDate, src => src.MapFrom(x => x.Assignment.CreatedDate))
               .ForMember(dest => dest.TimesheetContractCompanyCode, src => src.MapFrom(x => x.Assignment.ContractCompany.Code))
               .ForMember(dest => dest.TimesheetContractCompany, src => src.MapFrom(x => x.Assignment.ContractCompany.Name))
               .ForMember(dest => dest.TimesheetContractCoordinator, src => src.MapFrom(x => x.Assignment.ContractCompanyCoordinator.Name))
               .ForMember(dest => dest.TimesheetContractCoordinatorCode, src => src.MapFrom(x => x.Assignment.ContractCompanyCoordinator.SamaccountName))
               .ForMember(dest => dest.TimesheetContractNumber, src => src.MapFrom(x => x.Assignment.Project.Contract.ContractNumber))
               .ForMember(dest => dest.TimesheetCustomerCode, src => src.MapFrom(x => x.Assignment.Project.Contract.Customer.Code))
               .ForMember(dest => dest.TimesheetCustomerName, src => src.MapFrom(x => x.Assignment.Project.Contract.Customer.Name))
               .ForMember(dest => dest.TimesheetOperatingCompany, src => src.MapFrom(x => x.Assignment.OperatingCompany.Name))
               .ForMember(dest => dest.TimesheetOperatingCompanyCode, src => src.MapFrom(x => x.Assignment.OperatingCompany.Code))
               .ForMember(dest => dest.TimesheetOperatingCoordinator, src => src.MapFrom(x => x.Assignment.OperatingCompanyCoordinator.Name))
               .ForMember(dest => dest.TimesheetOperatingCoordinatorCode, src => src.MapFrom(x => x.Assignment.OperatingCompanyCoordinator.SamaccountName))
               .ForMember(dest => dest.TimesheetProjectNumber, src => src.MapFrom(x => x.Assignment.Project.ProjectNumber))
               .ForMember(dest => dest.CustomerProjectName, src => src.MapFrom(x => x.Assignment.Project.CustomerProjectName))
               .ForMember(dest => dest.TimesheetDescription, src => src.MapFrom(x => x.TimesheetDescription))
               .ForMember(dest => dest.TimesheetEndDate, src => src.MapFrom(x => x.ToDate))
               .ForMember(dest => dest.TimesheetStatus, src => src.MapFrom(x => x.TimesheetStatus))
               .ForMember(dest => dest.UnusedReason, src => src.MapFrom(x => x.UnusedReason))
               .ForMember(dest => dest.TimesheetStartDate, src => src.MapFrom(x => x.FromDate))
               .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => Convert.ToInt32(x.UpdateCount)))
               .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
               .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
               .ForMember(dest => dest.TimesheetId, src => src.MapFrom(x => x.Id))
               .ForMember(dest => dest.TimesheetNumber, src => src.MapFrom(x => x.TimesheetNumber))
               .ForMember(dest => dest.AssignmentReference, src => src.MapFrom(x => x.Assignment.AssignmentReference))
               .ForMember(dest => dest.TechSpecialists, src => src.MapFrom(x=> x.TimesheetTechnicalSpecialist))
               .ReverseMap()
               .ForAllOtherMembers(src => src.Ignore());
            #endregion

            #region Timesheet

            CreateMap<DbModels.Timesheet, DomainModels.Timesheet>()
                .ForMember(dest => dest.TimesheetId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.TimesheetAssignmentId, src => src.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.TimesheetAssignmentNumber, src => src.MapFrom(x => x.Assignment.AssignmentNumber))
                .ForMember(dest => dest.TimesheetContractCompanyCode, src => src.MapFrom(x => x.Assignment.ContractCompany.Code))
                .ForMember(dest => dest.TimesheetContractCompany, src => src.MapFrom(x => x.Assignment.ContractCompany.Name))
                .ForMember(dest => dest.TimesheetContractCoordinator, src => src.MapFrom(x => x.Assignment.ContractCompanyCoordinator.Name))
                .ForMember(dest => dest.TimesheetContractCoordinatorCode, src => src.MapFrom(x => x.Assignment.ContractCompanyCoordinator.SamaccountName))
                .ForMember(dest => dest.TimesheetContractNumber, src => src.MapFrom(x => x.Assignment.Project.Contract.ContractNumber))
                .ForMember(dest => dest.TimesheetCustomerCode, src => src.MapFrom(x => x.Assignment.Project.Contract.Customer.Code))
                .ForMember(dest => dest.TimesheetCustomerName, src => src.MapFrom(x => x.Assignment.Project.Contract.Customer.Name))
                .ForMember(dest => dest.TimesheetOperatingCompany, src => src.MapFrom(x => x.Assignment.OperatingCompany.Name))
                .ForMember(dest => dest.TimesheetOperatingCompanyCode, src => src.MapFrom(x => x.Assignment.OperatingCompany.Code))
                .ForMember(dest => dest.TimesheetOperatingCoordinatorCode, src => src.MapFrom(x => x.Assignment.OperatingCompanyCoordinator.SamaccountName))
                .ForMember(dest => dest.TimesheetOperatingCoordinator, src => src.MapFrom(x => x.Assignment.OperatingCompanyCoordinator.Name))
                .ForMember(dest => dest.IsContractHoldingCompanyActive, src => src.MapFrom(x => x.Assignment.ContractCompany.IsActive)) // ITK D - 619
                .ForMember(dest => dest.TimesheetProjectNumber, src => src.MapFrom(x => x.Assignment.Project.ProjectNumber))
                .ForMember(dest => dest.CustomerProjectName, src => src.MapFrom(x => x.Assignment.Project.CustomerProjectName))
                .ForMember(dest => dest.IsApprovedByContractCompany, src => src.MapFrom(x => x.IsApprovedByContractCompany))
                .ForMember(dest => dest.TimesheetCompletedPercentage, src => src.MapFrom(x => x.PercentageCompleted))
                .ForMember(dest => dest.TimesheetDatePeriod, src => src.MapFrom(x => x.DatePeriod))
                .ForMember(dest => dest.TimesheetDescription, src => src.MapFrom(x => x.TimesheetDescription))
                .ForMember(dest => dest.TimesheetExpectedCompleteDate, src => src.MapFrom(x => x.ExpectedCompleteDate))
                .ForMember(dest => dest.TimesheetEndDate, src => src.MapFrom(x => x.ToDate))
                .ForMember(dest => dest.TimesheetNumber, src => src.MapFrom(x => x.TimesheetNumber))
                .ForMember(dest => dest.TimesheetStatus, src => src.MapFrom(x => x.TimesheetStatus))
                .ForMember(dest => dest.UnusedReason, src => src.MapFrom(x => x.UnusedReason))
                .ForMember(dest => dest.TimesheetReference1, src => src.MapFrom(x => x.Reference1))
                .ForMember(dest => dest.TimesheetReference2, src => src.MapFrom(x => x.Reference2))
                .ForMember(dest => dest.TimesheetReference3, src => src.MapFrom(x => x.Reference3))
                .ForMember(dest => dest.TimesheetReviewDate, src => src.MapFrom(x => x.ReviewedDate))
                .ForMember(dest => dest.TimesheetReviewBy, src => src.MapFrom(x => x.ReviewedBy))
                .ForMember(dest => dest.TimesheetStartDate, src => src.MapFrom(x => x.FromDate))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => Convert.ToInt32(x.UpdateCount)))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.AssignmentClientReportingRequirements, src => src.MapFrom(x => x.Assignment.AssignmentMessage
                                                                                              .FirstOrDefault(x1 => x1.AssignmentId == x.AssignmentId
                                                                                                             && x1.MessageTypeId == Convert.ToInt16(AssignmentMessageType.ReportingRequirements)
                                                                                                             && x1.IsActive == true)
                                                                                              .Message))
                 .ForMember(dest => dest.AssignmentProjectBusinessUnit, src => src.MapFrom(x => x.Assignment.Project.ProjectType.Name))
                 .ForMember(dest => dest.ProjectInvoiceInstructionNotes, src =>src.MapFrom(x=>x.Assignment.Project.ProjectMessage
                                                        .FirstOrDefault(m => m.ProjectId == x.Assignment.ProjectId 
                                                            && m.MessageTypeId == Convert.ToInt16(ProjectMessageType.InvoiceNotes)).Message))
                .ForMember(dest => dest.AssignmentProjectWorkFlow, src => src.MapFrom(x => x.Assignment.Project.WorkFlowType))
                .ForMember(dest => dest.AssignmentStatus, src => src.MapFrom(x => x.Assignment.AssignmentStatus))
                .ForMember(dest => dest.TechSpecialists, src => src.MapFrom(x=> x.TimesheetTechnicalSpecialist))
                .ForMember(dest => dest.IsVisitOnPopUp,src => src.MapFrom(x => x.Assignment.Project.IsVisitOnPopUp)) //IGO QC 864 - client pop in timesheet
                .ForMember(dest => dest.EvoId, src => src.MapFrom(x => x.Evoid)) // These needs to be removed once DB sync done
                .ForAllOtherMembers(src => src.Ignore());

            CreateMap<DomainModels.Timesheet, DbModels.Timesheet>()
               .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isTimeId") ? (Convert.ToBoolean(context.Options.Items["isTimeId"]) ? src.TimesheetId : null) : src.TimesheetId)))
               //.ForMember(dest => dest.TimesheetNumber, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("isTimesheetNumber") ? Convert.ToBoolean(context.Options.Items["isTimesheetNumber"]) == true ? src.TimesheetNumber + 1 : src.TimesheetNumber : src.TimesheetNumber))
                 //.ForMember(dest => dest.TimesheetNumber, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Assignment")
                 //                                                                                                 ? (Convert.ToBoolean(context.Options.Items["isTimesheetNumber"])) ? (((List<DbModels.Assignment>)context.Options.Items["Assignment"])?
                 //                                                                                                                                                                         .FirstOrDefault(x => x.Id == src.TimesheetAssignmentId)?
                 //                                                                                                                                                                         .Timesheet?.Count > 0)
                 //                                                                                                                                                                     ? ((List<DbModels.Assignment>)context.Options.Items["Assignment"])?
                 //                                                                                                                                                                         .FirstOrDefault(x => x.Id == src.TimesheetAssignmentId)?
                 //                                                                                                                                                                         .Timesheet?.Max(x => x.TimesheetNumber + 1) : 1
                 //                                                                                                                                                                    : src.TimesheetNumber
                 //                                                                                                 : null))
               .ForMember(dest => dest.TimesheetNumber, src => src.MapFrom(x => x.TimesheetNumber))
               .ForMember(dest => dest.AssignmentId, src => src.MapFrom(x => x.TimesheetAssignmentId))
               .ForMember(dest => dest.TimesheetStatus, src => src.MapFrom(x => x.TimesheetStatus))
               .ForMember(dest => dest.UnusedReason, src => src.MapFrom(x => x.UnusedReason))
               .ForMember(dest => dest.FromDate, src => src.MapFrom(x => x.TimesheetStartDate))
               .ForMember(dest => dest.ToDate, src => src.MapFrom(x => x.TimesheetEndDate))
               .ForMember(dest => dest.ExpectedCompleteDate, src => src.MapFrom(x => x.TimesheetExpectedCompleteDate))
               .ForMember(dest => dest.DatePeriod, src => src.MapFrom(x => x.TimesheetDatePeriod))
               .ForMember(dest => dest.Reference1, src => src.MapFrom(x => x.TimesheetReference1))
               .ForMember(dest => dest.Reference2, src => src.MapFrom(x => x.TimesheetReference2))
               .ForMember(dest => dest.Reference3, src => src.MapFrom(x => x.TimesheetReference3))
               .ForMember(dest => dest.TimesheetDescription, src => src.MapFrom(x => x.TimesheetDescription))
               .ForMember(dest => dest.PercentageCompleted, src => src.MapFrom(x => x.TimesheetCompletedPercentage))
               .ForMember(dest => dest.IsApprovedByContractCompany, src => src.MapFrom(x => x.IsApprovedByContractCompany))
               .ForMember(dest => dest.ReviewedDate, src => src.MapFrom(x => x.TimesheetReviewDate))
               .ForMember(dest => dest.ReviewedBy, src => src.MapFrom(x => x.TimesheetReviewBy))
               .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => Convert.ToInt32(x.UpdateCount)))
               .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
               .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
               .ForAllOtherMembers(src => src.Ignore());

            #endregion

            #region DB Timesheet to Domain Timesheet Search

            CreateMap<DbModels.Timesheet, DomainModels.TimesheetSearch>()
                .ForMember(dest => dest.TimesheetId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.TimesheetNumber, src => src.MapFrom(x => x.TimesheetNumber))
                .ForMember(dest => dest.TimesheetAssignmentId, src => src.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.TimesheetAssignmentNumber, src => src.MapFrom(x => x.Assignment.AssignmentNumber))
                .ForMember(dest => dest.TimesheetContractNumber, src => src.MapFrom(x => x.Assignment.Project.Contract.ContractNumber))
                .ForMember(dest => dest.TimesheetCustomerCode, src => src.MapFrom(x => x.Assignment.Project.Contract.Customer.Code))
                .ForMember(dest => dest.TimesheetCustomerName, src => src.MapFrom(x => x.Assignment.Project.Contract.Customer.Name))
                .ForMember(dest => dest.TimesheetOperatingCompany, src => src.MapFrom(x => x.Assignment.OperatingCompany.Name))
                .ForMember(dest => dest.TimesheetOperatingCompanyCode, src => src.MapFrom(x => x.Assignment.OperatingCompany.Code))
                .ForMember(dest => dest.TimesheetProjectNumber, src => src.MapFrom(x => x.Assignment.Project.ProjectNumber))
                .ForMember(dest => dest.CustomerProjectName, src => src.MapFrom(x => x.Assignment.Project.CustomerProjectName))
                .ForMember(dest => dest.TimesheetDescription, src => src.MapFrom(x => x.TimesheetDescription))
                .ForMember(dest => dest.TimesheetEndDate, src => src.MapFrom(x => x.ToDate))
                .ForMember(dest => dest.TimesheetStatus, src => src.MapFrom(x => x.TimesheetStatus))
                .ForMember(dest => dest.TimesheetStartDate, src => src.MapFrom(x => x.FromDate))
                .ForMember(dest => dest.TimesheetContractHolderCompanyCode, src => src.MapFrom(x => x.Assignment.ContractCompany.Code))
                .ForMember(dest => dest.TimesheetContractHolderCompany, src => src.MapFrom(x => x.Assignment.ContractCompany.Name))
                .ForMember(dest => dest.TimesheetContractHolderCoordinator, src => src.MapFrom(x => x.Assignment.ContractCompanyCoordinator.Name))
                .ForMember(dest => dest.TimesheetContractHolderCoordinatorCode, src => src.MapFrom(x => x.Assignment.ContractCompanyCoordinator.SamaccountName))
                .ForMember(dest => dest.TimesheetOperatingCompanyCoordinator, src => src.MapFrom(x => x.Assignment.OperatingCompanyCoordinator.Name))
                .ForMember(dest => dest.TimesheetOperatingCompanyCoordinatorCode, src => src.MapFrom(x => x.Assignment.OperatingCompanyCoordinator.SamaccountName))
                // .ForMember(dest => dest.TechSpecialists, src => src.MapFrom(x=> x.TimesheetTechnicalSpecialist == null ? new List<DomainModels.TechnicalSpecialist>() : x.TimesheetTechnicalSpecialist.Select(ts => new DomainModels.TechnicalSpecialist
                // {
                //     FirstName = ts.TechnicalSpecialist == null ? null:ts.TechnicalSpecialist.FirstName,
                //     LastName = ts.TechnicalSpecialist == null ? null:ts.TechnicalSpecialist.LastName,
                //     Pin = ts.TechnicalSpecialist == null ? 0 : ts.TechnicalSpecialist.Pin,
                //     TimesheetId = ts.TimesheetId
                // }).ToList()))
                .ForMember(dest => dest.TechSpecialists, src => src.MapFrom(x=> x.TimesheetTechnicalSpecialist))
                .ForAllOtherMembers(src => src.Ignore());

            CreateMap<DbModels.TimesheetTechnicalSpecialist, DomainModels.TechnicalSpecialist>()
               .ForMember(dest => dest.TimesheetId, source => source.MapFrom(x => x.TimesheetId))
               .ForMember(dest => dest.FirstName, source => source.MapFrom(x => x.TechnicalSpecialist.FirstName))
               .ForMember(dest => dest.LastName, source => source.MapFrom(x => x.TechnicalSpecialist.LastName))
               .ForMember(dest => dest.LoginName, source => source.MapFrom(x => x.TechnicalSpecialist.LogInName))
               .ForMember(dest => dest.Pin, source => source.MapFrom(x => x.TechnicalSpecialist.Pin))
               .ForMember(dest => dest.ProfileStatus, source => source.MapFrom(x => x.TechnicalSpecialist.ProfileStatus.Name))
               .ForAllOtherMembers(x => x.Ignore());
            #endregion
                
            #region Timesheet Note

            CreateMap<DbModels.TimesheetNote, DomainModels.TimesheetNote>()
                .ForMember(dest => dest.TimesheetNoteId, source => source.MapFrom(x => x.Id))
                .ForMember(dest => dest.TimesheetId, source => source.MapFrom(x => x.TimesheetId))
                .ForMember(dest => dest.Note, source => source.MapFrom(x => x.Note))
                .ForMember(dest => dest.IsCustomerVisible, source => source.MapFrom(x => x.IsCustomerVisible))
                .ForMember(dest => dest.IsSpecialistVisible, source => source.MapFrom(x => x.IsSpecialistVisible))
                .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
                .ForMember(dest => dest.CreatedOn, source => source.MapFrom(x => x.CreatedDate))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModels.TimesheetNote, DbModels.TimesheetNote>()
           .ForMember(dest => dest.TimesheetId, source => source.MapFrom(x => x.TimesheetId))
           .ForMember(dest => dest.Id, source => source.MapFrom(x => x.TimesheetNoteId))
           .ForMember(dest => dest.Note, source => source.MapFrom(x => x.Note))
           .ForMember(dest => dest.IsCustomerVisible, source => source.MapFrom(x => x.IsCustomerVisible))
           .ForMember(dest => dest.IsSpecialistVisible, source => source.MapFrom(x => x.IsSpecialistVisible))
           .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
           .ForMember(dest => dest.CreatedDate, source => source.MapFrom(x => x.CreatedOn))
           .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
           .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
           .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
           .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Timesheet References
            CreateMap<DbModels.TimesheetReference, DomainModels.TimesheetReferenceType>()
                     .ForMember(dest => dest.TimesheetReferenceId, source => source.MapFrom(x => x.Id))
                     .ForMember(dest => dest.TimesheetId, source => source.MapFrom(x => x.TimesheetId))
                     .ForMember(dest => dest.AssignmentReferenceTypeId, source => source.MapFrom(x => x.AssignmentReferenceTypeId)) //Added for insert AssignmentReferenceType in SkeletonTimesheet
                     .ForMember(dest => dest.ReferenceType, source => source.MapFrom(x => x.AssignmentReferenceType.Name))
                     .ForMember(dest => dest.ReferenceValue, source => source.MapFrom(x => x.ReferenceValue))
                     .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                     .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                     .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                     .ForMember(dest => dest.Evoid, src => src.MapFrom(x => x.Evoid)) // These needs to be removed once DB sync done
                     .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModels.TimesheetReferenceType, DbModels.TimesheetReference>()
               .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isTimesheetId") ? (Convert.ToBoolean(context.Options.Items["isTimesheetId"]) ? src.TimesheetReferenceId : null) : src.TimesheetReferenceId)))
               .ForMember(dest => dest.TimesheetId, source => source.MapFrom(x => x.TimesheetId))
               .ForMember(dest => dest.AssignmentReferenceTypeId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("ReferenceTypes") ? ((List<DbModels.Data>)context.Options.Items["ReferenceTypes"])?.FirstOrDefault(x => x.Name == src.ReferenceType)?.Id : null))
               .ForMember(dest => dest.ReferenceValue, source => source.MapFrom(x => x.ReferenceValue))
               .ForMember(dest => dest.UpdateCount, opt => opt.MapFrom(src => src.UpdateCount))
               .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
               .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
               .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Timesheet Technical Specialists

            CreateMap<DbModels.TimesheetTechnicalSpecialist, DomainModels.TimesheetTechnicalSpecialist>()
                   .ForMember(dest => dest.TimesheetTechnicalSpecialistId, source => source.MapFrom(x => x.Id))
                   .ForMember(dest => dest.TimesheetId, source => source.MapFrom(x => x.TimesheetId))
                   .ForMember(dest => dest.TechnicalSpecialistName, source => source.MapFrom(x => string.Format("{0}, {1}", x.TechnicalSpecialist.LastName, x.TechnicalSpecialist.FirstName)))
                   .ForMember(dest => dest.LoginName, source => source.MapFrom(x => x.TechnicalSpecialist.LogInName))
                   .ForMember(dest => dest.Pin, source => source.MapFrom(x => x.TechnicalSpecialistId))
                   .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                   .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                   .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                   .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModels.TimesheetTechnicalSpecialist, DbModels.TimesheetTechnicalSpecialist>()
               .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isTimesheetTSId") ? (Convert.ToBoolean(context.Options.Items["isTimesheetTSId"]) ? src.TimesheetTechnicalSpecialistId : null) : src.TimesheetTechnicalSpecialistId)))
               .ForMember(dest => dest.TimesheetId, source => source.MapFrom(x => x.TimesheetId))
               .ForMember(dest => dest.TechnicalSpecialistId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("TechSpec") ? ((List<DbModels.TechnicalSpecialist>)context.Options.Items["TechSpec"])?.FirstOrDefault(x => x.Pin == src.Pin)?.Id : null))
               .ForMember(dest => dest.UpdateCount, opt => opt.MapFrom(src => src.UpdateCount))
               .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
               .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
               .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Timesheet Technical Specialist Account Item Time

            CreateMap<DbModels.TimesheetTechnicalSpecialistAccountItemTime, DomainModels.TimesheetSpecialistAccountItemTime>()
                .ForMember(dest => dest.TimesheetTechnicalSpecialistAccountTimeId, source => source.MapFrom(x => x.Id))
                .ForMember(dest => dest.TimesheetId, source => source.MapFrom(x => x.TimesheetId))
                .ForMember(dest => dest.Pin, source => source.MapFrom(x => x.TimesheetTechnicalSpeciallist.TechnicalSpecialistId))
                .ForMember(dest => dest.TimesheetTechnicalSpecialistId, source => source.MapFrom(x => x.TimesheetTechnicalSpeciallistId))
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
               // .ForMember(dest => dest.InvoicingStatus, src => src.MapFrom(x => "N"))  //To be taken from Invoice Status enum when finance module comes into place
               .ForMember(dest => dest.Evoid, src => src.MapFrom(x => x.Evoid)) // These needs to be removed once DB sync done
                .ForMember(dest => dest.ChargeRateId, src => src.MapFrom(x => x.ChargeRateId))
                .ForMember(dest => dest.PayRateId, src => src.MapFrom(x => x.PayRateId))
                .ForMember(dest => dest.SalesTaxId, src => src.MapFrom(x => x.SalesTaxId))
                .ForMember(dest => dest.WithholdingTaxId, src => src.MapFrom(x => x.WithholdingTaxId))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModels.TimesheetSpecialistAccountItemTime, DbModels.TimesheetTechnicalSpecialistAccountItemTime>()
                .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isTimesheetTSTimeId") ? (Convert.ToBoolean(context.Options.Items["isTimesheetTSTimeId"]) ? src.TimesheetTechnicalSpecialistAccountTimeId : null) : src.TimesheetTechnicalSpecialistAccountTimeId)))
                .ForMember(dest => dest.TimesheetId, source => source.MapFrom(x => x.TimesheetId))
                .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.ProjectId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Project") ? ((IList<DbModels.Project>)context.Options.Items["Project"])?.FirstOrDefault(x => x.ProjectNumber == src.ProjectNumber)?.Id : null))
                .ForMember(dest => dest.ContractId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Contract") ? ((IList<DbModels.Contract>)context.Options.Items["Contract"])?.FirstOrDefault(x => x.ContractNumber == src.ContractNumber)?.Id : null))
                .ForMember(dest => dest.ExpenseChargeTypeId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("ExpenseType") ? ((IList<DbModels.Data>)context.Options.Items["ExpenseType"])?.FirstOrDefault(x => x.Name == src.ChargeExpenseType && x.MasterDataTypeId == (int)MasterType.ExpenseType)?.Id : null))
                .ForMember(dest => dest.TimesheetTechnicalSpeciallistId, source => source.MapFrom(x => x.TimesheetTechnicalSpecialistId))
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

            #region Timesheet Technical Specialist Account Item Travel

            CreateMap<DbModels.TimesheetTechnicalSpecialistAccountItemTravel, DomainModels.TimesheetSpecialistAccountItemTravel>()
                .ForMember(dest => dest.TimesheetTechnicalSpecialistAccountTravelId, source => source.MapFrom(x => x.Id))
                .ForMember(dest => dest.TimesheetId, source => source.MapFrom(x => x.TimesheetId))
                .ForMember(dest => dest.Pin, source => source.MapFrom(x => x.TimesheetTechnicalSpecialist.TechnicalSpecialistId))
                .ForMember(dest => dest.TimesheetTechnicalSpecialistId, source => source.MapFrom(x => x.TimesheetTechnicalSpecialistId))
                .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.ChargeExpenseType, source => source.MapFrom(x => x.ChargeExpenseType.Name))
                .ForMember(dest => dest.PayExpenseType, source => source.MapFrom(x => x.PayExpenseType.Name))
                .ForMember(dest => dest.ExpenseDate, source => source.MapFrom(x => x.ExpenceDate))
                .ForMember(dest => dest.ExpenseDescription, source => source.MapFrom(x => x.ExpenseDescription))
                .ForMember(dest => dest.IsInvoicePrintExpenseDescription, source => source.MapFrom(x => x.IsDescriptionPrintedOnInvoice))
                .ForMember(dest => dest.ChargeUnit, source => source.MapFrom(x => x.ChargeTotalUnit))
                .ForMember(dest => dest.ChargeRate, source => source.MapFrom(x => x.ChargeRate))
                .ForMember(dest => dest.ChargeRateCurrency, source => source.MapFrom(x => x.ChargeRateCurrency))
                .ForMember(dest => dest.PayUnit, source => source.MapFrom(x => x.PayTotalUnit))
                .ForMember(dest => dest.PayRate, source => source.MapFrom(x => x.PayRate))
                .ForMember(dest => dest.PayRateCurrency, source => source.MapFrom(x => x.PayRateCurrency))
                .ForMember(dest => dest.ProjectNumber, source => source.MapFrom(x => x.Project.ProjectNumber))
                .ForMember(dest => dest.ContractNumber, source => source.MapFrom(x => x.Contract.ContractNumber))
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
                //.ForMember(dest => dest.InvoicingStatus, src => src.MapFrom(x => "N"))  //To be taken from Invoice Status enum when finance module comes into place
                .ForMember(dest => dest.Evoid, src => src.MapFrom(x => x.Evoid)) // These needs to be removed once DB sync done
                .ForMember(dest => dest.ChargeRateId, src => src.MapFrom(x => x.ChargeRateId))
                .ForMember(dest => dest.PayRateId, src => src.MapFrom(x => x.PayRateId))
                .ForMember(dest => dest.SalesTaxId, src => src.MapFrom(x => x.SalesTaxId))
                .ForMember(dest => dest.WithholdingTaxId, src => src.MapFrom(x => x.WithholdingTaxId))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModels.TimesheetSpecialistAccountItemTravel, DbModels.TimesheetTechnicalSpecialistAccountItemTravel>()
                .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isTimesheetTSTravelId") ? (Convert.ToBoolean(context.Options.Items["isTimesheetTSTravelId"]) ? src.TimesheetTechnicalSpecialistAccountTravelId : null) : src.TimesheetTechnicalSpecialistAccountTravelId)))
                .ForMember(dest => dest.TimesheetId, source => source.MapFrom(x => x.TimesheetId))
                .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.ProjectId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Project") ? ((IList<DbModels.Project>)context.Options.Items["Project"])?.FirstOrDefault(x => x.ProjectNumber == src.ProjectNumber)?.Id : null))
                .ForMember(dest => dest.ContractId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Contract") ? ((IList<DbModels.Contract>)context.Options.Items["Contract"])?.FirstOrDefault(x => x.ContractNumber == src.ContractNumber)?.Id : null))
                .ForMember(dest => dest.ChargeExpenseTypeId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("ExpenseType") ? ((IList<DbModels.Data>)context.Options.Items["ExpenseType"])?.FirstOrDefault(x => x.Name == src.ChargeExpenseType && x.MasterDataTypeId == (int)MasterType.ExpenseType)?.Id : null))
                .ForMember(dest => dest.PayExpenseTypeId,    opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("ExpenseType") ? ((IList<DbModels.Data>)context.Options.Items["ExpenseType"])?.FirstOrDefault(x => x.Name == src.PayExpenseType && x.MasterDataTypeId == (int)MasterType.ExpenseType)?.Id : null))
                .ForMember(dest => dest.TimesheetTechnicalSpecialistId, source => source.MapFrom(x => x.TimesheetTechnicalSpecialistId))
                .ForMember(dest => dest.ExpenceDate, source => source.MapFrom(x => x.ExpenseDate))
                .ForMember(dest => dest.ExpenseDescription, source => source.MapFrom(x => !string.IsNullOrEmpty(x.ExpenseDescription) ? new string(x.ExpenseDescription.Take(50).ToArray()) : x.ExpenseDescription))
                .ForMember(dest => dest.IsDescriptionPrintedOnInvoice, source => source.MapFrom(x => x.IsInvoicePrintExpenseDescription))
                .ForMember(dest => dest.ChargeTotalUnit, source => source.MapFrom(x => x.ChargeUnit))
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

            #region Timesheet Technical Specialist Account Item Expense

            CreateMap<DbModels.TimesheetTechnicalSpecialistAccountItemExpense, DomainModels.TimesheetSpecialistAccountItemExpense>()
                .ForMember(dest => dest.TimesheetTechnicalSpecialistAccountExpenseId, source => source.MapFrom(x => x.Id))
                .ForMember(dest => dest.TimesheetId, source => source.MapFrom(x => x.TimesheetId))
                .ForMember(dest => dest.Pin, source => source.MapFrom(x => x.TimesheetTechnicalSpeciallist.TechnicalSpecialistId))
                .ForMember(dest => dest.TimesheetTechnicalSpecialistId, source => source.MapFrom(x => x.TimesheetTechnicalSpeciallistId))
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
                //.ForMember(dest => dest.InvoicingStatus, src => src.MapFrom(x => "N"))  //To be taken from Invoice Status enum when finance module comes into place
                .ForMember(dest => dest.Evoid, src => src.MapFrom(x => x.Evoid)) // These needs to be removed once DB sync done
                .ForMember(dest => dest.ChargeRateId, src => src.MapFrom(x => x.ChargeRateId))
                .ForMember(dest => dest.PayRateId, src => src.MapFrom(x => x.PayRateId))
                .ForMember(dest => dest.SalesTaxId, src => src.MapFrom(x => x.SalesTaxId))
                .ForMember(dest => dest.WithholdingTaxId, src => src.MapFrom(x => x.WithholdingTaxId))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModels.TimesheetSpecialistAccountItemExpense, DbModels.TimesheetTechnicalSpecialistAccountItemExpense>()
              .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isTimesheetTSExpenseId") ? (Convert.ToBoolean(context.Options.Items["isTimesheetTSExpenseId"]) ? src.TimesheetTechnicalSpecialistAccountExpenseId : null) : src.TimesheetTechnicalSpecialistAccountExpenseId)))
              .ForMember(dest => dest.TimesheetId, source => source.MapFrom(x => x.TimesheetId))
              .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
              .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
              .ForMember(dest => dest.TimesheetTechnicalSpeciallistId, source => source.MapFrom(x => x.TimesheetTechnicalSpecialistId))
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

            #region Timesheet Technical Specialist Account Item Consumable

            CreateMap<DbModels.TimesheetTechnicalSpecialistAccountItemConsumable, DomainModels.TimesheetSpecialistAccountItemConsumable>()
                .ForMember(dest => dest.TimesheetTechnicalSpecialistAccountConsumableId, source => source.MapFrom(x => x.Id))
                .ForMember(dest => dest.TimesheetId, source => source.MapFrom(x => x.TimesheetId))
                .ForMember(dest => dest.TimesheetTechnicalSpecialistId, source => source.MapFrom(x => x.TimesheetTechnicalSpecialistId))
                .ForMember(dest => dest.Pin, source => source.MapFrom(x => x.TimesheetTechnicalSpecialist.TechnicalSpecialistId))
                .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
                .ForMember(dest => dest.ExpenseDate, source => source.MapFrom(x => x.ExpenceDate))
                .ForMember(dest => dest.ChargeExpenseType, source => source.MapFrom(x => x.ChargeExpenseType.Name))
                .ForMember(dest => dest.ChargeDescription, source => source.MapFrom(x => x.ChargeDescription))
                .ForMember(dest => dest.IsInvoicePrintChargeDescription, source => source.MapFrom(x => x.IsDescriptionPrintedOnInvoice))
                .ForMember(dest => dest.ChargeUnit, source => source.MapFrom(x => x.ChargeTotalUnit))
                .ForMember(dest => dest.ChargeRate, source => source.MapFrom(x => x.ChargeRate))
                .ForMember(dest => dest.ChargeRateCurrency, source => source.MapFrom(x => x.ChargeRateCurrency))
                .ForMember(dest => dest.PayType, source => source.MapFrom(x => x.PayExpenseType.Name))
                .ForMember(dest => dest.PayUnit, source => source.MapFrom(x => x.PayTotalUnit))
                .ForMember(dest => dest.PayRate, source => source.MapFrom(x => x.PayRate))
                .ForMember(dest => dest.PayRateCurrency, source => source.MapFrom(x => x.PayRateCurrency))
                .ForMember(dest => dest.PayRateDescription, source => source.MapFrom(x => x.PayRateDescription))
                .ForMember(dest => dest.ProjectNumber, source => source.MapFrom(x => x.Project.ProjectNumber))
                .ForMember(dest => dest.ContractNumber, source => source.MapFrom(x => x.Contract.ContractNumber))
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
               // .ForMember(dest => dest.InvoicingStatus, src => src.MapFrom(x => "N"))  //To be taken from Invoice Status enum when finance module comes into place
               .ForMember(dest => dest.Evoid, src => src.MapFrom(x => x.Evoid)) // These needs to be removed once DB sync done
                .ForMember(dest => dest.ChargeRateId, src => src.MapFrom(x => x.ChargeRateId))
                .ForMember(dest => dest.PayRateId, src => src.MapFrom(x => x.PayRateId))
                .ForMember(dest => dest.SalesTaxId, src => src.MapFrom(x => x.SalesTaxId))
                .ForMember(dest => dest.WithholdingTaxId, src => src.MapFrom(x => x.WithholdingTaxId))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModels.TimesheetSpecialistAccountItemConsumable, DbModels.TimesheetTechnicalSpecialistAccountItemConsumable>()
               .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isTimesheetTSConsumableId") ? (Convert.ToBoolean(context.Options.Items["isTimesheetTSConsumableId"]) ? src.TimesheetTechnicalSpecialistAccountConsumableId : null) : src.TimesheetTechnicalSpecialistAccountConsumableId)))
               .ForMember(dest => dest.TimesheetId, source => source.MapFrom(x => x.TimesheetId))
               .ForMember(dest => dest.AssignmentId, source => source.MapFrom(x => x.AssignmentId))
               .ForMember(dest => dest.TimesheetTechnicalSpecialistId, source => source.MapFrom(x => x.TimesheetTechnicalSpecialistId))
               .ForMember(dest => dest.ProjectId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Project") ? ((IList<DbModels.Project>)context.Options.Items["Project"])?.FirstOrDefault(x => x.ProjectNumber == src.ProjectNumber)?.Id : null))
               .ForMember(dest => dest.ContractId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("Contract") ? ((IList<DbModels.Contract>)context.Options.Items["Contract"])?.FirstOrDefault(x => x.ContractNumber == src.ContractNumber)?.Id : null))
               .ForMember(dest => dest.ChargeExpenseTypeId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("ExpenseType") ? ((IList<DbModels.Data>)context.Options.Items["ExpenseType"])?.FirstOrDefault(x => x.Name == src.ChargeExpenseType && x.MasterDataTypeId == (int)MasterType.ExpenseType)?.Id : null))
               .ForMember(dest => dest.PayExpenseTypeId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("ExpenseType") ? ((IList<DbModels.Data>)context.Options.Items["ExpenseType"])?.FirstOrDefault(x => x.Name == src.PayType && x.MasterDataTypeId == (int)MasterType.ExpenseType)?.Id : null))
               .ForMember(dest => dest.ExpenceDate, source => source.MapFrom(x => x.ExpenseDate))
               .ForMember(dest => dest.ChargeDescription, source => source.MapFrom(x => x.ChargeDescription))
               .ForMember(dest => dest.IsDescriptionPrintedOnInvoice, source => source.MapFrom(x => x.IsInvoicePrintChargeDescription))
               .ForMember(dest => dest.ChargeTotalUnit, source => source.MapFrom(x => x.ChargeUnit))
               .ForMember(dest => dest.ChargeRate, source => source.MapFrom(x => x.ChargeRate))
               .ForMember(dest => dest.ChargeRateCurrency, source => source.MapFrom(x => x.ChargeRateCurrency))
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

            #region Timesheet Document Search
            CreateMap<DomainModels.TimesheetSearch, Document.Domain.Models.Document.EvolutionMongoDocumentSearch>()
                .ForMember(dest => dest.ModuleCode, src => src.ResolveUsing(x => Common.Enums.ModuleCodeType.TIME.ToString()))
                .ForMember(dest => dest.ReferenceCode, src => src.MapFrom(x => x.TimesheetId))
                .ForMember(dest => dest.Text, src => src.ResolveUsing<DomResolver.SearchTextFormatResolver, string>("DocumentSearchText"))
                .ForMember(dest => dest.DocumentTypes, src => src.ResolveUsing(x => string.IsNullOrEmpty(x.SearchDocumentType) ? null : new List<string>() { x.SearchDocumentType }))
                .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Timesheet Inter Company Discounts

            CreateMap<DbModels.TimesheetInterCompanyDiscount, DomainModels.TimesheetInterCompanyDiscounts>()
                .ForMember(dest => dest.TimesheetId, src => src.MapFrom(x => x.TimesheetId))
                .ForMember(dest => dest.DiscountType, src => src.MapFrom(x => x.DiscountType))
                .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Description))
                .ForMember(dest => dest.DiscountPercentage, src => src.MapFrom(x => x.Percentage))
                .ForMember(dest => dest.CompanyCode, src => src.MapFrom(x => x.Company.Code))
                .ForMember(dest => dest.CompanyName, src => src.MapFrom(x => x.Company.Name))
                .ForMember(dest => dest.TimesheetInterCompanyDiscountId, src => src.MapFrom(x => x.Id))
                .ForAllOtherMembers(src => src.Ignore());
            
            #endregion

        }
    }
}
