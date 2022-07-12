using AutoMapper;
using Evolution.Automapper.Resolver.MongoSearch;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            #region Technicalspecialist
            CreateMap<DomainModel.TechnicalSpecialistInfo, DbModel.TechnicalSpecialist>()
                        .ForMember(dest => dest.BusinessInformationComment, source => source.MapFrom(x => x.BusinessInformationComment))
                        .ForMember(dest => dest.CompanyId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbCompanies") ? ((List<DbModel.Company>)ctx.Options.Items["DbCompanies"])?.FirstOrDefault(comp => comp.Code == src.CompanyCode)?.Id : 0))
                        .ForMember(dest => dest.CompanyPayrollId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbCompPayrolls") ? ((List<DbModel.CompanyPayroll>)ctx.Options.Items["DbCompPayrolls"])?.FirstOrDefault(cp => cp.Name == src.CompanyPayrollName)?.Id : 0))//Defect 45 Changes
                        .ForMember(dest => dest.PassportCountryOriginId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbCountries") ? ((List<DbModel.Country>)ctx.Options.Items["DbCountries"])?.FirstOrDefault(cp => cp.Name == src.PassportCountryName)?.Id : 0))
                        .ForMember(dest => dest.DateOfBirth, source => source.MapFrom(x => x.DateOfBirth))
                        .ForMember(dest => dest.DrivingLicenseNumber, source => source.MapFrom(x => x.DrivingLicenseNo))
                        .ForMember(dest => dest.DrivingLicenseExpiryDate, source => source.MapFrom(x => x.DrivingLicenseExpiryDate))
                        .ForMember(dest => dest.EndDate, source => source.MapFrom(x => x.EndDate))
                        .ForMember(dest => dest.EmploymentTypeId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbEmploymentTypes") ? ((List<DbModel.Data>)ctx.Options.Items["DbEmploymentTypes"])?.FirstOrDefault(cp => cp.Name == src.EmploymentType)?.Id : 0))
                        .ForMember(dest => dest.FirstName, source => source.MapFrom(x => x.FirstName))
                        .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                        .ForMember(dest => dest.IsReviewAndModerationProcess, source => source.MapFrom(x => x.ReviewAndModerationProcessName))
                        .ForMember(dest => dest.IsActive, source => source.MapFrom(x => x.IsActive))
                        .ForMember(dest => dest.HomePageComment, source => source.MapFrom(x => x.HomePageComment))
                        .ForMember(dest => dest.IsEreportingQualified, source => source.MapFrom(x => x.IsEReportingQualified ?? false ))
                        .ForMember(dest => dest.LastName, source => source.MapFrom(x => x.LastName))
                        .ForMember(dest => dest.MiddleName, source => source.MapFrom(x => x.MiddleName))
                        .ForMember(dest => dest.ModeOfCommunication, source => source.MapFrom(x => x.ModeOfCommunication))
                        .ForMember(dest => dest.PassportNumber, source => source.MapFrom(x => x.PassportNo))
                        .ForMember(dest => dest.PassportExpiryDate, source => source.MapFrom(x => x.PassportExpiryDate))
                        .ForMember(dest => dest.PayrollReference, source => source.MapFrom(x => x.PayrollReference))
                        .ForMember(dest => dest.PayrollNote, source => source.MapFrom(x => x.PayrollNotes))
                        .ForMember(dest => dest.ProfessionalAfiliation, source => source.MapFrom(x => x.ProfessionalAfiliation))
                        .ForMember(dest => dest.ProfessionalSummary, source => source.MapFrom(x => x.ProfessionalSummary))
                        .ForMember(dest => dest.ProfileActionId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbActions") ? ((List<DbModel.Data>)ctx.Options.Items["DbActions"])?.FirstOrDefault(act => act.Name == src.ProfileAction)?.Id : 0))
                        .ForMember(dest => dest.ProfileStatusId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbStatuses") ? ((List<DbModel.Data>)ctx.Options.Items["DbStatuses"])?.FirstOrDefault(act => act.Name == src.ProfileStatus)?.Id : 0))
                        .ForMember(dest => dest.Pin, source => source.MapFrom(x => x.Epin))
                        .ForMember(dest => dest.Salutation, source => source.MapFrom(x => x.Salutation))
                        .ForMember(dest => dest.StartDate, source => source.MapFrom(x => x.StartDate))
                        .ForMember(dest => dest.SubDivisionId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbSubDivisions") ? ((List<DbModel.Data>)ctx.Options.Items["DbSubDivisions"])?.FirstOrDefault(subDiv => subDiv.Name == src.SubDivisionName)?.Id : 0))
                        .ForMember(dest => dest.TaxReference, source => source.MapFrom(x => x.TaxReference))
                        .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                        .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
                        .ForMember(dest => dest.LogInName, source => source.MapFrom(x => x.LogonName))
                        .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                         .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                         .ForMember(dest => dest.ContactComment, source => source.MapFrom(x => x.ContactComment))
                         .ForMember(dest => dest.ApprovalStatus, source => source.MapFrom(x => x.ApprovalStatus))
                         .ForMember(dest => dest.Tqmcomment, source => source.MapFrom(x => x.TqmComment))
                         .ForMember(dest => dest.AssignedToUser, source => source.MapFrom(x => x.AssignedToUser)) //Added for D946 CR
                         .ForMember(dest => dest.AssignedByUser, source => source.MapFrom(x => x.AssignedByUser)) //Added for D946 CR
                         .ForMember(dest => dest.IsTsCredSent, source => source.MapFrom(x => x.IsTsCredSent)) //Added for D978 SLNO 1
                        .ForMember(dest => dest.PendingWithId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbUser") ? ((List<DbModel.User>)ctx.Options.Items["DbUser"])?.FirstOrDefault(x => x.SamaccountName == src.PendingWithUser)?.Id : 0)) //Added for D946 CR
                        .ForMember(dest => dest.Userid, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbLoginUser") ? ((List<DbModel.User>)ctx.Options.Items["DbLoginUser"])?.FirstOrDefault(x => x.Id == src.UserId)?.Id : 0)) 
                        .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.TechnicalSpecialist, DomainModel.TechnicalSpecialistInfo>()
                        .ForMember(dest => dest.BusinessInformationComment, source => source.MapFrom(x => x.BusinessInformationComment))
                        .ForMember(dest => dest.CompanyCode, source => source.MapFrom(x => x.Company.Code))
                        .ForMember(dest => dest.CompanyName, source => source.MapFrom(x => x.Company.Name))
                        .ForMember(dest => dest.CompanyPayrollName, source => source.MapFrom(x => x.CompanyPayroll.Name)) //Defect 45 Changes
                        .ForMember(dest => dest.DateOfBirth, source => source.MapFrom(x => x.DateOfBirth))
                        .ForMember(dest => dest.DrivingLicenseNo, source => source.MapFrom(x => x.DrivingLicenseNumber))
                        .ForMember(dest => dest.DrivingLicenseExpiryDate, source => source.MapFrom(x => x.DrivingLicenseExpiryDate))
                        .ForMember(dest => dest.EmploymentType, source => source.MapFrom(x => x.EmploymentType.Name))
                        .ForMember(dest => dest.EndDate, source => source.MapFrom(x => x.EndDate))
                        .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.Pin))
                        .ForMember(dest => dest.FirstName, source => source.MapFrom(x => x.FirstName))
                        .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                        .ForMember(dest => dest.IsActive, source => source.MapFrom(x => x.IsActive))
                        .ForMember(dest => dest.HomePageComment, source => source.MapFrom(x => x.HomePageComment))
                        .ForMember(dest => dest.ContactComment, source => source.MapFrom(x => x.ContactComment))
                        .ForMember(dest => dest.ApprovalStatus, source => source.MapFrom(x => x.ApprovalStatus))
                        .ForMember(dest => dest.IsEReportingQualified, source => source.MapFrom(x => x.IsEreportingQualified))
                        .ForMember(dest => dest.LastName, source => source.MapFrom(x => x.LastName))
                        .ForMember(dest => dest.MiddleName, source => source.MapFrom(x => x.MiddleName))
                        .ForMember(dest => dest.ModeOfCommunication, source => source.MapFrom(x => x.ModeOfCommunication))
                        .ForMember(dest => dest.PassportNo, source => source.MapFrom(x => x.PassportNumber))
                        //.ForMember(dest => dest.PassportCountryCode, source => source.MapFrom(x => x.PassportCountryOrigin.Code))
                        .ForMember(dest => dest.PassportCountryName, source => source.MapFrom(x => x.PassportCountryOrigin.Name))
                        .ForMember(dest => dest.PassportExpiryDate, source => source.MapFrom(x => x.PassportExpiryDate))
                        .ForMember(dest => dest.PayrollReference, source => source.MapFrom(x => x.PayrollReference))
                        .ForMember(dest => dest.PayrollNotes, source => source.MapFrom(x => x.PayrollNote))
                        .ForMember(dest => dest.ProfessionalAfiliation, source => source.MapFrom(x => x.ProfessionalAfiliation))
                        .ForMember(dest => dest.ProfessionalSummary, source => source.MapFrom(x => x.ProfessionalSummary))
                        .ForMember(dest => dest.ProfileStatus, source => source.MapFrom(x => x.ProfileStatus.Name))
                        .ForMember(dest => dest.ProfileAction, source => source.MapFrom(x => x.ProfileAction.Name))
                        .ForMember(dest => dest.PrevProfileAction, source => source.MapFrom(x => x.ProfileAction.Name))
                        .ForMember(dest => dest.ReviewAndModerationProcessName, source => source.MapFrom(x => x.IsReviewAndModerationProcess))
                        .ForMember(dest => dest.Salutation, source => source.MapFrom(x => x.Salutation))
                        .ForMember(dest => dest.StartDate, source => source.MapFrom(x => x.StartDate))
                        .ForMember(dest => dest.SubDivisionName, source => source.MapFrom(x => x.SubDivision.Name))
                        .ForMember(dest => dest.TaxReference, source => source.MapFrom(x => x.TaxReference))
                        .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                        .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
                         .ForMember(dest => dest.LogonName, source => source.MapFrom(x => x.LogInName))
                        .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                        .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                        .ForMember(dest => dest.TqmComment, source => source.MapFrom(x => x.Tqmcomment))
                        .ForMember(dest => dest.AssignedToUser, source => source.MapFrom(x => x.AssignedToUser))//Added for D946 CR
                        .ForMember(dest => dest.AssignedByUser, source => source.MapFrom(x => x.AssignedByUser))//Added for D946 CR
                        .ForMember(dest => dest.PendingWithUser, source => source.MapFrom(x => x.PendingWith.Name))//Added for D946 CR
                        .ForMember(dest => dest.IsTsCredSent, source => source.MapFrom(x => x.IsTsCredSent)) //Added for D978 SLNO 1
                        .ForMember(dest => dest.PendingWithUserLogOnName, source => source.MapFrom(x => x.PendingWith.SamaccountName))// IGOQC D855  
                        .ForMember(dest => dest.UserId, source => source.MapFrom(x => x.User.Id))
                        .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Base Technicalspecialist
            CreateMap<DomainModel.BaseTechnicalSpecialistInfo, DbModel.TechnicalSpecialist>()
                       .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                       .ForMember(dest => dest.Pin, source => source.MapFrom(x => x.Epin))
                       .ForMember(dest => dest.FirstName, source => source.MapFrom(x => x.FirstName))
                       .ForMember(dest => dest.MiddleName, source => source.MapFrom(x => x.MiddleName))
                       .ForMember(dest => dest.LastName, source => source.MapFrom(x => x.LastName))
                       .ForMember(dest => dest.StartDate, source => source.MapFrom(x => x.StartDate))
                       .ForMember(dest => dest.EndDate, source => source.MapFrom(x => x.EndDate))
                       .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                       .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                       .ForMember(dest => dest.ApprovalStatus, source => source.MapFrom(x => x.ApprovalStatus)) 
                       .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.TechnicalSpecialist, DomainModel.BaseTechnicalSpecialistInfo>()
                       .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                       .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.Pin))
                       .ForMember(dest => dest.FirstName, source => source.MapFrom(x => x.FirstName))
                       .ForMember(dest => dest.MiddleName, source => source.MapFrom(x => x.MiddleName))
                       .ForMember(dest => dest.LastName, source => source.MapFrom(x => x.LastName))
                       .ForMember(dest => dest.StartDate, source => source.MapFrom(x => x.StartDate))
                       .ForMember(dest => dest.EndDate, source => source.MapFrom(x => x.EndDate))
                       .ForMember(dest => dest.CompanyCode, source => source.MapFrom(x => x.Company.Code))
                       .ForMember(dest => dest.CompanyName, source => source.MapFrom(x => x.Company.Name))
                       .ForMember(dest => dest.ProfileStatus, source => source.MapFrom(x => x.ProfileStatus.Name))
                       .ForMember(dest => dest.EmploymentType, source => source.MapFrom(x => x.EmploymentType.Name))
                       .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                       .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                       .ForMember(dest => dest.ApprovalStatus, source => source.MapFrom(x => x.ApprovalStatus))
                       .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModel.TechnicalSpecialistInfo, DomainModel.BaseTechnicalSpecialistInfo>()
                      .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                      .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.Epin))
                      .ForMember(dest => dest.FirstName, source => source.MapFrom(x => x.FirstName))
                      .ForMember(dest => dest.MiddleName, source => source.MapFrom(x => x.MiddleName))
                      .ForMember(dest => dest.LastName, source => source.MapFrom(x => x.LastName))
                      .ForMember(dest => dest.CompanyCode, source => source.MapFrom(x => x.CompanyCode))
                      .ForMember(dest => dest.CompanyName, source => source.MapFrom(x => x.CompanyName))
                      .ForMember(dest => dest.StartDate, source => source.MapFrom(x => x.StartDate))
                      .ForMember(dest => dest.EndDate, source => source.MapFrom(x => x.EndDate))
                      .ForMember(dest => dest.ProfileStatus, source => source.MapFrom(x => x.ProfileStatus))
                      .ForMember(dest => dest.EmploymentType, source => source.MapFrom(x => x.EmploymentType))
                      .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                      .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                      .ForMember(dest => dest.ApprovalStatus, source => source.MapFrom(x => x.ApprovalStatus))
                      .ReverseMap()
                      .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Search Technicalspecialist

            CreateMap<DomainModel.TechnicalSpecialistInfo, DomainModel.TechnicalSpecialistSearchResult>()
                     .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                     .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.Epin))
                     .ForMember(dest => dest.FirstName, source => source.MapFrom(x => x.FirstName))
                     .ForMember(dest => dest.MiddleName, source => source.MapFrom(x => x.MiddleName))
                     .ForMember(dest => dest.LastName, source => source.MapFrom(x => x.LastName))
                     .ForMember(dest => dest.CompanyCode, source => source.MapFrom(x => x.CompanyCode))
                     .ForMember(dest => dest.CompanyName, source => source.MapFrom(x => x.CompanyName))
                     .ForMember(dest => dest.StartDate, source => source.MapFrom(x => x.StartDate))
                     .ForMember(dest => dest.EndDate, source => source.MapFrom(x => x.EndDate))
                     .ForMember(dest => dest.ProfileStatus, source => source.MapFrom(x => x.ProfileStatus))
                     .ForMember(dest => dest.EmploymentType, source => source.MapFrom(x => x.EmploymentType))
                     .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                     .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                     .ForMember(dest => dest.ApprovalStatus, source => source.MapFrom(x => x.ApprovalStatus))
                      .ForMember(dest => dest.PendingWithUser, source => source.MapFrom(x => x.PendingWithUser))//Added for D946 CR
                     .ReverseMap()
                     .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.TechnicalSpecialist, DomainModel.TechnicalSpecialistSearchResult>()
                      .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                      .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.Pin))
                      .ForMember(dest => dest.FirstName, source => source.MapFrom(x => x.FirstName))
                      .ForMember(dest => dest.MiddleName, source => source.MapFrom(x => x.MiddleName))
                      .ForMember(dest => dest.LastName, source => source.MapFrom(x => x.LastName))
                      .ForMember(dest => dest.StartDate, source => source.MapFrom(x => x.StartDate))
                      .ForMember(dest => dest.EndDate, source => source.MapFrom(x => x.EndDate))
                      .ForMember(dest => dest.CompanyCode, source => source.MapFrom(x => x.Company.Code))
                      .ForMember(dest => dest.CompanyName, source => source.MapFrom(x => x.Company.Name))
                      .ForMember(dest => dest.ProfileStatus, source => source.MapFrom(x => x.ProfileStatus.Name))
                      .ForMember(dest => dest.EmploymentType, source => source.MapFrom(x => x.EmploymentType.Name))
                      .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                      .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                      .ForMember(dest => dest.ContactComment, source => source.MapFrom(x => x.ContactComment))
                      .ForMember(dest => dest.ApprovalStatus, source => source.MapFrom(x => x.ApprovalStatus))
                      .ForMember(dest => dest.TechnicalSpecialistContact, source => source.MapFrom(x => x.TechnicalSpecialistContact))
                      .ForMember(dest => dest.SubDivisionName, source => source.MapFrom(x => x.SubDivision.Name))  /** Changes for Hot Fixes on NDT */
                      .ForMember(dest => dest.PendingWithUser, source => source.MapFrom(x => x.PendingWith.Name))//Added for D946 CR
                      .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModel.SearchTechnicalSpecialist, DomainModel.TechnicalSpecialistInfo>()
                .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.Epin))
                .ForMember(dest => dest.FirstName, source => source.MapFrom(x => x.FirstName))
                .ForMember(dest => dest.MiddleName, source => source.MapFrom(x => x.MiddleName))
                .ForMember(dest => dest.LastName, source => source.MapFrom(x => x.LastName))
                .ForMember(dest => dest.CompanyCode, source => source.MapFrom(x => x.CompanyCode))
                .ForMember(dest => dest.CompanyName, source => source.MapFrom(x => x.CompanyName))
                .ForMember(dest => dest.StartDate, source => source.MapFrom(x => x.StartDate))
                .ForMember(dest => dest.EndDate, source => source.MapFrom(x => x.EndDate))
                .ForMember(dest => dest.ProfileStatus, source => source.MapFrom(x => x.ProfileStatus))
                .ForMember(dest => dest.EmploymentType, source => source.MapFrom(x => x.EmploymentType))
                .ForMember(dest => dest.LogonName, source => source.MapFrom(x => x.LogonName))
                .ForMember(dest => dest.ApprovalStatus, source => source.MapFrom(x => x.ApprovalStatus))
                .ForMember(dest => dest.PendingWithUser, source => source.MapFrom(x => x.PendingWithUser))//Added for D946 CR
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region TechnicalspecialistStamp
            CreateMap<DomainModel.TechnicalSpecialistStampInfo, DbModel.TechnicalSpecialistStamp>()
                        .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
                        .ForMember(dest => dest.TechnicalSpecialistId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbTechSpecialist") ? ((List<DbModel.TechnicalSpecialist>)ctx.Options.Items["DbTechSpecialist"])?.FirstOrDefault(act => act.Pin == src.Epin)?.Id : 0))
                        .ForMember(dest => dest.CountryId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DBTsStampCountryCodes") ? ((List<DbModel.Data>)ctx.Options.Items["DBTsStampCountryCodes"])?.FirstOrDefault(act => act.Name == src.CountryName)?.Id : 0))
                        .ForMember(dest => dest.IsSoftStamp, source => source.MapFrom(x => x.IsSoftStamp))
                        .ForMember(dest => dest.StampNumber, source => source.MapFrom(x => x.StampNumber))
                        .ForMember(dest => dest.IssuedDate, source => source.MapFrom(x => x.IssuedDate))
                        .ForMember(dest => dest.ReturnDate, source => source.MapFrom(x => x.ReturnedDate))
                        .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                        .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                        .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                        .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.TechnicalSpecialistStamp, DomainModel.TechnicalSpecialistStampInfo>()
                        .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                        .ForMember(dest => dest.IsSoftStamp, source => source.MapFrom(x => x.IsSoftStamp))
                        .ForMember(dest => dest.StampNumber, source => source.MapFrom(x => x.StampNumber))
                        .ForMember(dest => dest.IssuedDate, source => source.MapFrom(x => x.IssuedDate))
                        .ForMember(dest => dest.ReturnedDate, source => source.MapFrom(x => x.ReturnDate))
                        .ForMember(dest => dest.CountryCode, source => source.MapFrom(x => x.Country.Code))
                        .ForMember(dest => dest.CountryName, source => source.MapFrom(x => x.Country.Name))
                        .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.TechnicalSpecialist.Pin))
                        .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                        .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                        .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                        .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Educational
            CreateMap<DomainModel.TechnicalSpecialistEducationalQualificationInfo, DbModel.TechnicalSpecialistEducationalQualification>()
           .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                     .ForMember(dest => dest.TechnicalSpecialistId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbTechSpecialist") ? ((List<DbModel.TechnicalSpecialist>)ctx.Options.Items["DbTechSpecialist"])?.FirstOrDefault(act => act.Pin == src.Epin)?.Id : 0))
                .ForMember(dest => dest.DisplayOrder, source => source.MapFrom(x => x.DisplayOrder))
               .ForMember(dest => dest.Address, source => source.MapFrom(x => x.Address))
               .ForMember(dest => dest.Institution, source => source.MapFrom(x => x.Institution))
                      //.ForMember(dest => dest.City, source => source.MapFrom(x => x.CityName))
                      //  .ForMember(dest => dest.Country.Name, source => source.MapFrom(x => x.CountyName))
                      //    .ForMember(dest => dest.County.Name, source => source.MapFrom(x => x.CountyName))
                      .ForMember(dest => dest.Qualification, source => source.MapFrom(x => x.Qualification))
                       .ForMember(dest => dest.Percentage, source => source.MapFrom(x => x.Percentage))
                        .ForMember(dest => dest.DateFrom, source => source.MapFrom(x => x.FromDate))
                         .ForMember(dest => dest.DateTo, source => source.MapFrom(x => x.ToDate))
             .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
             .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
             .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
             .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.TechnicalSpecialistEducationalQualification, DomainModel.TechnicalSpecialistEducationalQualificationInfo>()
              .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
              .ForMember(dest => dest.DisplayOrder, source => source.MapFrom(x => x.DisplayOrder))
              .ForMember(dest => dest.Address, source => source.MapFrom(x => x.Address))
           .ForMember(dest => dest.Institution, source => source.MapFrom(x => x.Institution))
           .ForMember(dest => dest.FromDate, source => source.MapFrom(x => x.DateFrom))
            .ForMember(dest => dest.ToDate, source => source.MapFrom(x => x.DateTo))

                       //.ForMember(dest => dest.CityName, source => source.MapFrom(x => x.City.Name))
                       //  .ForMember(dest => dest.CountyName, source => source.MapFrom(x => x.County.Name))
                       //    .ForMember(dest => dest.CountyName, source => source.MapFrom(x => x.County.Name))
                       .ForMember(dest => dest.Qualification, source => source.MapFrom(x => x.Qualification))
                        .ForMember(dest => dest.Percentage, source => source.MapFrom(x => x.Percentage))
              .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.TechnicalSpecialist.Pin))
              .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
              .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
              .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
              .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Language
            CreateMap<TechnicalSpecialistLanguageCapabilityInfo, DbModel.TechnicalSpecialistLanguageCapability>()
                        .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
                        .ForMember(dest => dest.TechnicalSpecialistId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbTechnicalspecialists") ? ((List<DbModel.TechnicalSpecialist>)ctx.Options.Items["DbTechnicalspecialists"])?.FirstOrDefault(act => act.Pin == src.Epin)?.Id : 0))
                    .ForMember(dest => dest.LanguageId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbLanguage") ? ((List<DbModel.Data>)ctx.Options.Items["DbLanguage"])?.FirstOrDefault(act => act.Name == src.Language)?.Id : 0))
                        .ForMember(dest => dest.SpeakingCapabilityLevel, source => source.MapFrom(x => x.SpeakingCapabilityLevel))
                         .ForMember(dest => dest.WritingCapabilityLevel, source => source.MapFrom(x => x.WritingCapabilityLevel))
                          .ForMember(dest => dest.ComprehensionCapabilityLevel, source => source.MapFrom(x => x.ComprehensionCapabilityLevel))
                       .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                       .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                       .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                       .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.TechnicalSpecialistLanguageCapability, TechnicalSpecialistLanguageCapabilityInfo>()
                        .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                         .ForMember(dest => dest.SpeakingCapabilityLevel, source => source.MapFrom(x => x.SpeakingCapabilityLevel))
                         .ForMember(dest => dest.WritingCapabilityLevel, source => source.MapFrom(x => x.WritingCapabilityLevel))
                          .ForMember(dest => dest.ComprehensionCapabilityLevel, source => source.MapFrom(x => x.ComprehensionCapabilityLevel))
                       .ForMember(dest => dest.Language, source => source.MapFrom(x => x.Language.Name))
                        .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.TechnicalSpecialist.Pin))
                        .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                        .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                        .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                        .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region WorkHistory

            CreateMap<TechnicalSpecialistWorkHistoryInfo, DbModel.TechnicalSpecialistWorkHistory>()
                     .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
                      .ForMember(dest => dest.TechnicalSpecialistId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbTechnicalspecialists") ? ((List<DbModel.TechnicalSpecialist>)ctx.Options.Items["DbTechnicalspecialists"])?.FirstOrDefault(act => act.Pin == src.Epin)?.Id : 0))
                     .ForMember(dest => dest.ClientName, source => source.MapFrom(x => x.ClientName))
                      .ForMember(dest => dest.JobDescription, source => source.MapFrom(x => x.Description))
                      .ForMember(dest => dest.JobTitle, source => source.MapFrom(x => x.JobTitle))
                     .ForMember(dest => dest.ProjectName, source => source.MapFrom(x => x.ProjectName))
                     .ForMember(dest => dest.JobResponsibility, source => source.MapFrom(x => x.Responsibility))
                     .ForMember(dest => dest.IsCurrentCompany, source => source.MapFrom(x => x.IsCurrentCompany))
                      .ForMember(dest => dest.DateFrom, source => source.MapFrom(x => x.FromDate))
                     .ForMember(dest => dest.DateTo, source => source.MapFrom(x => x.ToDate))
                      .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                     .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                     .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                     .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.TechnicalSpecialistWorkHistory, TechnicalSpecialistWorkHistoryInfo>()
                         .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                           .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.TechnicalSpecialist.Pin))
                     .ForMember(dest => dest.ClientName, source => source.MapFrom(x => x.ClientName))
                      .ForMember(dest => dest.Description, source => source.MapFrom(x => x.JobDescription))
                      .ForMember(dest => dest.JobTitle, source => source.MapFrom(x => x.JobTitle))
                     .ForMember(dest => dest.ProjectName, source => source.MapFrom(x => x.ProjectName))
                     .ForMember(dest => dest.Responsibility, source => source.MapFrom(x => x.JobResponsibility))
                     .ForMember(dest => dest.IsCurrentCompany, source => source.MapFrom(x => x.IsCurrentCompany))
                      .ForMember(dest => dest.FromDate, source => source.MapFrom(x => x.DateFrom))
                     .ForMember(dest => dest.ToDate, source => source.MapFrom(x => x.DateTo))
                      .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                     .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                     .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                        .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region CustomerApproval


            CreateMap<TechnicalSpecialistCustomerApprovalInfo, DbModel.TechnicalSpecialistCustomerApproval>()
                   .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
                    .ForMember(dest => dest.TechnicalSpecialistId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbTechnicalspecialists") ? ((List<DbModel.TechnicalSpecialist>)ctx.Options.Items["DbTechnicalspecialists"])?.FirstOrDefault(act => act.Pin == src.Epin)?.Id : 0))
                    .ForMember(dest => dest.CustomerId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbCustomerCode") ? ((List<DbModel.TechnicalSpecialistCustomers>)ctx.Options.Items["DbCustomerCode"])?.FirstOrDefault(act => act.Code == src.CustomerCode)?.Id : 0))
                    .ForMember(dest => dest.CustomerSapId, source => source.MapFrom(x => x.CustomerSap))
                    .ForMember(dest => dest.CustomerCommodityCodesId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbCustomerCommodity") ? ((List<DbModel.CustomerCommodity>)ctx.Options.Items["DbCustomerCommodity"])?.Where(act => act.Commodity?.Name == src.CustCodes)?.FirstOrDefault()?.CommodityId : null))
                    .ForMember(dest => dest.Comments, source => source.MapFrom(x => x.Comments))
                    .ForMember(dest => dest.DateFrom, source => source.MapFrom(x => x.FromDate))
                    .ForMember(dest => dest.DateTo, source => source.MapFrom(x => x.ToDate))
                    .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                    .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                    .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                    .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.TechnicalSpecialistCustomerApproval, TechnicalSpecialistCustomerApprovalInfo>()
                         .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                         .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.TechnicalSpecialist.Pin))
                         .ForMember(dest => dest.CustomerName, source => source.MapFrom(x => x.Customer.Name))
                         //.ForMember(dest => dest.CustCodes, source => source.MapFrom(x => x.CustomerCommodity.Commodity.Name)) 

                         .ForMember(dest => dest.CustCodes, source => source.MapFrom(x => x.CustomerCommodityCodes.Name))
                         //.CustomerCommodity.Where(x1=>x1.CustomerId == x.CustomerId).FirstOrDefault().Commodity.Name))
                         .ForMember(dest => dest.CustomerCode, source => source.MapFrom(x => x.Customer.Code))
                         .ForMember(dest => dest.CustomerSap, source => source.MapFrom(x => x.CustomerSapId))
                         .ForMember(dest => dest.Comments, source => source.MapFrom(x => x.Comments))
                         .ForMember(dest => dest.FromDate, source => source.MapFrom(x => x.DateFrom))
                         .ForMember(dest => dest.ToDate, source => source.MapFrom(x => x.DateTo))
                         .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                         .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                         .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                         .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Contact
            CreateMap<DbModel.TechnicalSpecialistContact, DomainModel.TechnicalSpecialistContactInfo>()
               .ForMember(source => source.Epin, dest => dest.MapFrom(x => x.TechnicalSpecialist.Id))
                .ForMember(source => source.Address, dest => dest.MapFrom(x => x.Address))
                .ForMember(source => source.City, dest => dest.MapFrom(x => x.City.Name))
                .ForMember(source => source.Country, dest => dest.MapFrom(x => x.Country.Name))
                .ForMember(source => source.County, dest => dest.MapFrom(x => x.County.Name))
                .ForMember(source => source.CityId, dest => dest.MapFrom(x => x.City.Id)) //Added for ITK D1536
                .ForMember(source => source.CountryId, dest => dest.MapFrom(x => x.Country.Id)) //Added for ITK D1536
                .ForMember(source => source.CountyId, dest => dest.MapFrom(x => x.County.Id)) //Added for ITK D1536
                .ForMember(source => source.EmergencyContactName, dest => dest.MapFrom(x => x.EmergencyContactName))
                .ForMember(source => source.EmailAddress, dest => dest.MapFrom(x => x.EmailAddress))
                .ForMember(source => source.FaxNumber, dest => dest.MapFrom(x => x.FaxNumber))
                .ForMember(source => source.PostalCode, dest => dest.MapFrom(x => x.PostalCode))
                .ForMember(source => source.TelephoneNumber, dest => dest.MapFrom(x => x.TelephoneNumber))
                .ForMember(source => source.MobileNumber, dest => dest.MapFrom(x => x.MobileNumber))
                .ForMember(source => source.ContactType, dest => dest.MapFrom(x => x.ContactType))
                .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
                .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
                .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
                .ForMember(source => source.IsGeoCordinateSync, dest => dest.MapFrom(x => x.IsGeoCordinateSync))
               .ReverseMap()
               .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Computer Electronic Knowledge


            CreateMap<DomainModel.TechnicalSpecialistComputerElectronicKnowledgeInfo, DbModel.TechnicalSpecialistComputerElectronicKnowledge>()
                       .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
                       .ForMember(dest => dest.TechnicalSpecialistId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbTechnicalspecialists") ? ((List<DbModel.TechnicalSpecialist>)ctx.Options.Items["DbTechnicalspecialists"])?.FirstOrDefault(act => act.Pin == src.Epin)?.Id : 0))
                       .ForMember(dest => dest.ComputerKnowledgeId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbComputerKnowledges") ? ((List<DbModel.Data>)ctx.Options.Items["DbComputerKnowledges"])?.FirstOrDefault(act => act.Name == src.ComputerKnowledge)?.Id : 0))
                       .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                       .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                       .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                       .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.TechnicalSpecialistComputerElectronicKnowledge, DomainModel.TechnicalSpecialistComputerElectronicKnowledgeInfo>()
                        .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                       .ForMember(dest => dest.ComputerKnowledge, source => source.MapFrom(x => x.ComputerKnowledge.Name))
                        .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.TechnicalSpecialist.Pin))
                        .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                        .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                        .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                        .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Code And Standard
            CreateMap<DomainModel.TechnicalSpecialistCodeAndStandardinfo, DbModel.TechnicalSpecialistCodeAndStandard>()
                       .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
                       .ForMember(dest => dest.TechnicalSpecialistId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbTechnicalspecialists") ? ((List<DbModel.TechnicalSpecialist>)ctx.Options.Items["DbTechnicalspecialists"])?.FirstOrDefault(act => act.Pin == src.Epin)?.Id : 0))
                       .ForMember(dest => dest.CodeStandardId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbCodeAndStandards") ? ((List<DbModel.Data>)ctx.Options.Items["DbCodeAndStandards"])?.FirstOrDefault(act => act.Name == src.CodeStandardName)?.Id : 0))
                       .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                       .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                       .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                       .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.TechnicalSpecialistCodeAndStandard, DomainModel.TechnicalSpecialistCodeAndStandardinfo>()
                        .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                       .ForMember(dest => dest.CodeStandardName, source => source.MapFrom(x => x.CodeStandard.Name))
                        .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.TechnicalSpecialist.Pin))
                        .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                        .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                        .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                        .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Commodity Equipment Knowledge.

            CreateMap<DomainModel.TechnicalSpecialistCommodityEquipmentKnowledgeInfo, DbModel.TechnicalSpecialistCommodityEquipmentKnowledge>()
                       .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
                       .ForMember(dest => dest.TechnicalSpecialistId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbTechSpecialist") ? ((List<DbModel.TechnicalSpecialist>)ctx.Options.Items["DbTechSpecialist"])?.FirstOrDefault(act => act.Pin == src.Epin)?.Id : 0))
                       .ForMember(dest => dest.CommodityId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DBCommodities") ? ((List<DbModel.Data>)ctx.Options.Items["DBCommodities"])?.FirstOrDefault(act => act.Name == src.Commodity)?.Id : 0))
                       .ForMember(dest => dest.EquipmentKnowledgeId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DBEquipments") ? ((List<DbModel.Data>)ctx.Options.Items["DBEquipments"])?.FirstOrDefault(act => act.Name == src.EquipmentKnowledge)?.Id : 0))
                       .ForMember(dest => dest.EquipmentKnowledgeLevel, source => source.MapFrom(x => x.EquipmentKnowledgeLevel))
                       .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                       .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                       .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                       .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge, DomainModel.TechnicalSpecialistCommodityEquipmentKnowledgeInfo>()
                        .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
                       .ForMember(dest => dest.Commodity, source => source.MapFrom(x => x.Commodity.Name))
                        .ForMember(dest => dest.EquipmentKnowledge, source => source.MapFrom(x => x.EquipmentKnowledge.Name))
                        .ForMember(dest => dest.EquipmentKnowledgeLevel, source => source.MapFrom(x => x.EquipmentKnowledgeLevel))
                        .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.TechnicalSpecialist.Pin))
                        .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
                        .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
                        .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
                        .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region TechnicalspecialistTaxonomy
            CreateMap<DomainModel.TechnicalSpecialistTaxonomyInfo, DbModel.TechnicalSpecialistTaxonomy>()
               .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
               .ForMember(dest => dest.TechnicalSpecialistId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DbTechSpecialist") ? ((List<DbModel.TechnicalSpecialist>)ctx.Options.Items["DbTechSpecialist"])?.FirstOrDefault(act => act.Pin == src.Epin)?.Id : 0))
               .ForMember(dest => dest.TaxonomyCategoryId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DBCategories") ? ((List<DbModel.Data>)ctx.Options.Items["DBCategories"])?.FirstOrDefault(act => act.Name == src.TaxonomyCategoryName)?.Id : 0))
               .ForMember(dest => dest.TaxonomySubCategoryId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DBSubCategories") ? ((List<DbModel.TaxonomySubCategory>)ctx.Options.Items["DBSubCategories"])?.FirstOrDefault(act => act.TaxonomySubCategoryName == src.TaxonomySubCategoryName && act.TaxonomyCategory.Name == src.TaxonomyCategoryName)?.Id : 0))
               .ForMember(dest => dest.TaxonomyServicesId, source => source.ResolveUsing((src, dst, arg3, ctx) => ctx.Options.Items.ContainsKey("DBServices") ? ((List<DbModel.TaxonomyService>)ctx.Options.Items["DBServices"])?.FirstOrDefault(act => act.TaxonomyServiceName == src.TaxonomyServices && act.TaxonomySubCategory.TaxonomySubCategoryName == src.TaxonomySubCategoryName)?.Id : 0)) //D897 (Ref 11-04-2020 ALM Doc)
               .ForMember(dest => dest.ApprovalStatus, source => source.MapFrom(x => x.ApprovalStatus))
               .ForMember(dest => dest.TaxonomyStatus, source => source.MapFrom(x => x.TaxonomyStatus))
               .ForMember(dest => dest.Interview, source => source.MapFrom(x => x.Interview))
               .ForMember(dest => dest.Comments, source => source.MapFrom(x => x.Comments))
               .ForMember(dest => dest.FromDate, source => source.MapFrom(x => x.FromDate))
               .ForMember(dest => dest.ToDate, source => source.MapFrom(x => x.ToDate))
               .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
               .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
               .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
               .ForMember(dest => dest.ApprovedBy, source => source.MapFrom(x => x.ApprovedBy))
               .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.TechnicalSpecialistTaxonomy, DomainModel.TechnicalSpecialistTaxonomyInfo>()
              .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
              .ForMember(dest => dest.ApprovalStatus, source => source.MapFrom(x => x.ApprovalStatus))
              .ForMember(dest => dest.TaxonomyStatus, source => source.MapFrom(x => x.TaxonomyStatus))
              .ForMember(dest => dest.Interview, source => source.MapFrom(x => x.Interview))
              .ForMember(dest => dest.Comments, source => source.MapFrom(x => x.Comments))
              .ForMember(dest => dest.FromDate, source => source.MapFrom(x => x.FromDate))
              .ForMember(dest => dest.TaxonomyCategoryName, source => source.MapFrom(x => x.TaxonomyCategory.Name))
              .ForMember(dest => dest.TaxonomySubCategoryName, source => source.MapFrom(x => x.TaxonomySubCategory.TaxonomySubCategoryName))
              .ForMember(dest => dest.TaxonomyServices, source => source.MapFrom(x => x.TaxonomyServices.TaxonomyServiceName))
              .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.TechnicalSpecialist.Pin))
              .ForMember(dest => dest.ToDate, source => source.MapFrom(x => x.ToDate))
              .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
              .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
              .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.ApprovedBy, source => source.MapFrom(x => x.ApprovedBy))
              .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.TechnicalSpecialistTaxonomyHistory, DbModel.TechnicalSpecialistTaxonomy>()
                 .ForMember(dest => dest.ApprovalStatus, src => src.MapFrom(x => x.ApprovalStatus))
                 .ForMember(dest => dest.Comments, src => src.MapFrom(x => x.Comments))
                 .ForMember(dest => dest.DisplayOrder, src => src.MapFrom(x => x.DisplayOrder))
                 .ForMember(dest => dest.TechnicalSpecialistId, src => src.MapFrom(x => x.TechnicalSpecialistId))
                 .ForMember(dest => dest.FromDate, src => src.MapFrom(x => x.FromDate))
                 .ForMember(dest => dest.Id, src => src.MapFrom(x => x.TaxonomyId))
                 .ForMember(dest => dest.Interview, src => src.MapFrom(x => x.Interview))
                 .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                 .ForMember(dest => dest.TaxonomyCategoryId, src=>src.MapFrom(x=>x.TaxonomyCategoryId))
                .ForMember(dest => dest.TaxonomySubCategoryId, src => src.MapFrom(x => x.TaxonomySubCategoryId))
                .ForMember(dest => dest.TaxonomyServicesId, src => src.MapFrom(x => x.TaxonomyServicesId))
                .ForMember(dest => dest.ToDate, src => src.MapFrom(x => x.ToDate))
                .ForMember(dest => dest.ApprovedBy, src => src.MapFrom(x => x.ApprovedBy))
                .ReverseMap()
                .ForAllOtherMembers(src => src.Ignore());
               #endregion

            #region TSPaySchedule
            CreateMap<DomainModel.TechnicalSpecialistPayScheduleInfo, DbModel.TechnicalSpecialistPaySchedule>()
               .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
               .ForMember(dest => dest.TechnicalSpecialistId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("DbTechSpecialist") ? ((List<DbModel.TechnicalSpecialist>)context.Options.Items["DbTechSpecialist"])?.FirstOrDefault(x => x.Pin == src.Epin)?.Id : null))
               .ForMember(dest => dest.DisplayOrder, source => source.MapFrom(x => x.DisplayOrder))
               .ForMember(dest => dest.PayScheduleName, source => source.MapFrom(x => x.PayScheduleName))
               .ForMember(dest => dest.PayScheduleNote, source => source.MapFrom(x => x.PayScheduleNote))
               .ForMember(dest => dest.PayCurrency, source => source.MapFrom(x => x.PayCurrency))
               .ForMember(dest => dest.IsActive, source => source.MapFrom(x => x.IsActive))
               .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
               .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
               .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
               .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.TechnicalSpecialistPaySchedule, DomainModel.TechnicalSpecialistPayScheduleInfo>()
              .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
              .ForMember(dest => dest.DisplayOrder, source => source.MapFrom(x => x.DisplayOrder))
              .ForMember(dest => dest.PayScheduleName, source => source.MapFrom(x => x.PayScheduleName))
              .ForMember(dest => dest.PayScheduleNote, source => source.MapFrom(x => x.PayScheduleNote))
              .ForMember(dest => dest.PayCurrency, source => source.MapFrom(x => x.PayCurrency))
              .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.TechnicalSpecialist.Pin))
              .ForMember(dest => dest.IsActive, source => source.MapFrom(x => x.IsActive))
              .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
              .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
              .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
              .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region TS PayRate
            CreateMap<DomainModel.TechnicalSpecialistPayRateInfo, DbModel.TechnicalSpecialistPayRate>()
            .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
            .ForMember(dest => dest.TechnicalSpecialistId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("DbTechSpecialist") ? ((List<DbModel.TechnicalSpecialist>)context.Options.Items["DbTechSpecialist"])?.FirstOrDefault(x => x.Pin == src.Epin)?.Id : null))
            .ForMember(dest => dest.ExpenseTypeId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("DbExpenseType") ? ((List<DbModel.Data>)context.Options.Items["DbExpenseType"])?.FirstOrDefault(x => x.Name == src.ExpenseType)?.Id : null))
            .ForMember(dest => dest.PayScheduleId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("DbTSPaySchedule") ? ((List<DbModel.TechnicalSpecialistPaySchedule>)context.Options.Items["DbTSPaySchedule"])?.FirstOrDefault(x => x.Id == src.PayScheduleId)?.Id : null))
            .ForMember(dest => dest.Rate, source => source.MapFrom(x => x.Rate))
            .ForMember(dest => dest.Description, source => source.MapFrom(x => x.Description))
            .ForMember(dest => dest.FromDate, source => source.MapFrom(x => x.EffectiveFrom))
            .ForMember(dest => dest.ToDate, source => source.MapFrom(x => x.EffectiveTo))
            .ForMember(dest => dest.IsDefaultPayRate, source => source.MapFrom(x => x.IsDefaultPayRate))
            .ForMember(dest => dest.IsHideOnTsExtranet, source => source.MapFrom(x => x.IsHideOnTsExtranet))
            .ForMember(dest => dest.IsActive, source => source.MapFrom(x => x.IsActive))
            .ForMember(dest => dest.DisplayOrder, source => source.MapFrom(x => x.DisplayOrder))
            .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
            .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
            .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
            .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DbModel.TechnicalSpecialistPayRate, DomainModel.TechnicalSpecialistPayRateInfo>()
             .ForMember(dest => dest.Id, source => source.MapFrom(x => x.Id))
             .ForMember(dest => dest.Rate, source => source.MapFrom(x => x.Rate))
             .ForMember(dest => dest.Description, source => source.MapFrom(x => x.Description))
             .ForMember(dest => dest.EffectiveFrom, source => source.MapFrom(x => x.FromDate))
             .ForMember(dest => dest.EffectiveTo, source => source.MapFrom(x => x.ToDate))
             .ForMember(dest => dest.IsDefaultPayRate, source => source.MapFrom(x => x.IsDefaultPayRate))
             .ForMember(dest => dest.IsHideOnTsExtranet, source => source.MapFrom(x => x.IsHideOnTsExtranet))
             .ForMember(dest => dest.Epin, source => source.MapFrom(x => x.TechnicalSpecialist.Id))
             .ForMember(dest => dest.PayScheduleId, source => source.MapFrom(x => x.PayScheduleId))
             .ForMember(dest => dest.PayScheduleName, source => source.MapFrom(x => x.PaySchedule.PayScheduleName))
             .ForMember(dest => dest.PayScheduleCurrency, source => source.MapFrom(x => x.PaySchedule.PayCurrency))
             .ForMember(dest => dest.ExpenseType, source => source.MapFrom(x => x.ExpenseType.Name))
             .ForMember(dest => dest.IsActive, source => source.MapFrom(x => x.IsActive))
             .ForMember(dest => dest.DisplayOrder, source => source.MapFrom(x => x.DisplayOrder))
             .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
             .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
             .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
             .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Training

            CreateMap<DbModel.TechnicalSpecialistCertificationAndTraining, DomainModel.TechnicalSpecialistTraining>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Epin, opt => opt.MapFrom(src => src.TechnicalSpecialist.Pin))
            .ForMember(dest => dest.TrainingName, opt => opt.MapFrom(src => src.CertificationAndTraining.Name))
            .ForMember(dest => dest.ILearnID, opt => opt.MapFrom(src => src.CertificationAndTraining.Id))
            .ForMember(dest => dest.TrainingRefId, opt => opt.MapFrom(src => src.CertificationAndTrainingRefId))
            .ForMember(dest => dest.EffeciveDate, opt => opt.MapFrom(src => src.EffeciveDate))
            .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
            .ForMember(dest => dest.VerificationType, opt => opt.MapFrom(src => src.VerificationType))
            .ForMember(dest => dest.VerificationStatus, opt => opt.MapFrom(src => src.VerificationStatus))
            .ForMember(dest => dest.VerificationType, opt => opt.MapFrom(src => src.VerificationType))
            .ForMember(dest => dest.VerificationDate, opt => opt.MapFrom(src => src.VerificationDate))
            .ForMember(dest => dest.IsExternal, opt => opt.MapFrom(src => src.IsExternal))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.VerifiedBy, opt => opt.MapFrom(src => src.VerifiedBy.SamaccountName))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
            .ForMember(dest => dest.DispalyOrder, opt => opt.MapFrom(src => src.DispalyOrder))
            .ForMember(dest => dest.RecordType, opt => opt.MapFrom(src => src.RecordType))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
            .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
            .ForMember(dest => dest.UpdateCount, opt => opt.MapFrom(src => src.UpdateCount))
            .ForMember(dest => dest.IsILearn, opt => opt.MapFrom(src => src.IsIlearn == null || src.IsIlearn == false ? false : true))
            .ForAllOtherMembers(src => src.Ignore());


            CreateMap<TechnicalSpecialistTraining, DbModel.TechnicalSpecialistCertificationAndTraining>()
            .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
            .ForMember(dest => dest.TechnicalSpecialistId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("DbTechSpecialist") ? ((List<DbModel.TechnicalSpecialist>)context.Options.Items["DbTechSpecialist"])?.FirstOrDefault(x => x.Pin == src.Epin)?.Id : null))
            .ForMember(dest => dest.CertificationAndTrainingId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("DbTrainingTypes") ? ((List<DbModel.Data>)context.Options.Items["DbTrainingTypes"])?.FirstOrDefault(x => x.Id == src.ILearnID)?.Id : null))
            .ForMember(dest => dest.VerifiedById, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("DbVarifiedByUser") ? ((List<DbModel.User>)context.Options.Items["DbVarifiedByUser"])?.FirstOrDefault(x => x.SamaccountName == src.VerifiedBy)?.Id : null))
            .ForMember(dest => dest.CertificationAndTrainingRefId, opt => opt.MapFrom(src => src.TrainingRefId))
            .ForMember(dest => dest.EffeciveDate, opt => opt.MapFrom(src => src.EffeciveDate))
            .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
            .ForMember(dest => dest.VerificationType, opt => opt.MapFrom(src => src.VerificationType))
            .ForMember(dest => dest.VerificationStatus, opt => opt.MapFrom(src => src.VerificationStatus))
            .ForMember(dest => dest.VerificationType, opt => opt.MapFrom(src => src.VerificationType))
            .ForMember(dest => dest.VerificationDate, opt => opt.MapFrom(src => src.VerificationDate))
            .ForMember(dest => dest.IsExternal, opt => opt.MapFrom(src => src.IsExternal))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
            .ForMember(dest => dest.DispalyOrder, opt => opt.MapFrom(src => src.DispalyOrder))
            .ForMember(dest => dest.RecordType, opt => opt.MapFrom(src => CompCertTrainingType.Tr.ToString()))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
            .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
            .ForMember(dest => dest.UpdateCount, opt => opt.MapFrom(src => src.UpdateCount))
            .ForAllOtherMembers(src => src.Ignore());



            #endregion

            #region Certification

            CreateMap<DbModel.TechnicalSpecialistCertificationAndTraining, TechnicalSpecialistCertification>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Epin, opt => opt.MapFrom(src => src.TechnicalSpecialist.Pin))
            .ForMember(dest => dest.CertificationName, opt => opt.MapFrom(src => src.CertificationAndTraining.Name))
            .ForMember(dest => dest.ILearnID, opt => opt.MapFrom(src => src.CertificationAndTraining.Id))
            .ForMember(dest => dest.EffeciveDate, opt => opt.MapFrom(src => src.EffeciveDate))
            .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
            .ForMember(dest => dest.VerificationType, opt => opt.MapFrom(src => src.VerificationType))
            .ForMember(dest => dest.VerificationStatus, opt => opt.MapFrom(src => src.VerificationStatus))
            .ForMember(dest => dest.VerificationType, opt => opt.MapFrom(src => src.VerificationType))
            .ForMember(dest => dest.VerificationDate, opt => opt.MapFrom(src => src.VerificationDate))
            .ForMember(dest => dest.IsExternal, opt => opt.MapFrom(src => src.IsExternal))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.VerifiedBy, opt => opt.MapFrom(src => src.VerifiedBy.SamaccountName))
            .ForMember(dest => dest.CertificateRefId, opt => opt.MapFrom(src => src.CertificationAndTrainingRefId))
            .ForMember(dest => dest.DispalyOrder, opt => opt.MapFrom(src => src.DispalyOrder))
            .ForMember(dest => dest.RecordType, opt => opt.MapFrom(src => CompCertTrainingType.Ce.ToString()))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
            .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
            .ForMember(dest => dest.UpdateCount, opt => opt.MapFrom(src => src.UpdateCount))
            .ForMember(dest => dest.IsILearn, opt => opt.MapFrom(src => src.IsIlearn == null || src.IsIlearn == false ? false : true))
            .ForAllOtherMembers(src => src.Ignore());


            CreateMap<TechnicalSpecialistCertification, DbModel.TechnicalSpecialistCertificationAndTraining>()
           .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
           .ForMember(dest => dest.TechnicalSpecialistId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("DbTechSpecialist") ? ((List<DbModel.TechnicalSpecialist>)context.Options.Items["DbTechSpecialist"])?.FirstOrDefault(x => x.Pin == src.Epin)?.Id : null))
           .ForMember(dest => dest.CertificationAndTrainingId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("DbCertificationTypes") ? ((List<DbModel.Data>)context.Options.Items["DbCertificationTypes"])?.FirstOrDefault(x => x.Id == src.ILearnID)?.Id : null))
           .ForMember(dest => dest.VerifiedById, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("DbVarifiedByUser") ? ((List<DbModel.User>)context.Options.Items["DbVarifiedByUser"])?.FirstOrDefault(x => x.SamaccountName == src.VerifiedBy)?.Id : null))
           .ForMember(dest => dest.EffeciveDate, opt => opt.MapFrom(src => src.EffeciveDate))
           .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
           .ForMember(dest => dest.VerificationType, opt => opt.MapFrom(src => src.VerificationType))
           .ForMember(dest => dest.VerificationStatus, opt => opt.MapFrom(src => src.VerificationStatus))
           .ForMember(dest => dest.VerificationType, opt => opt.MapFrom(src => src.VerificationType))
           .ForMember(dest => dest.VerificationDate, opt => opt.MapFrom(src => src.VerificationDate))
           .ForMember(dest => dest.IsExternal, opt => opt.MapFrom(src => src.IsExternal))
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
           .ForMember(dest => dest.CertificationAndTrainingRefId, opt => opt.MapFrom(src => src.CertificateRefId))
           .ForMember(dest => dest.DispalyOrder, opt => opt.MapFrom(src => src.DispalyOrder))
           .ForMember(dest => dest.RecordType, opt => opt.MapFrom(src => src.RecordType))
           .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
           .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
           .ForMember(dest => dest.UpdateCount, opt => opt.MapFrom(src => src.UpdateCount))
           .ForAllOtherMembers(src => src.Ignore());

            #endregion

            #region TechnicalSpecialistTrainingAndCompetencyType

            CreateMap<DbModel.TechnicalSpecialistTrainingAndCompetencyType, DomainModel.TechnicalSpecialistInternalTrainingAndCompetencyType>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.Id))
           .ForMember(dest => dest.TechnicalSpecialistInternalTrainingAndCompetencyId, opt => opt.MapFrom(x => x.TechnicalSpecialistTrainingAndCompetencyId))
           .ForMember(dest => dest.TypeName, opt => opt.MapFrom(x => x.TrainingOrCompetencyData.Name))
           .ForMember(dest => dest.TypeId, opt => opt.MapFrom(x => x.TrainingOrCompetencyData.Id))
           .ForAllOtherMembers(x => x.Ignore());

            CreateMap<DomainModel.TechnicalSpecialistInternalTrainingAndCompetencyType, DbModel.TechnicalSpecialistTrainingAndCompetencyType>()
       .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
            .ForMember(dest => dest.TrainingOrCompetencyDataId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("DBMasterTrainingAndCompetency") ? ((List<DbModel.Data>)context.Options.Items["DBMasterTrainingAndCompetency"])?.FirstOrDefault(x => x.Id == src.TypeId)?.Id : null))
            .ForMember(dest => dest.TechnicalSpecialistTrainingAndCompetencyId, opt => opt.MapFrom(x => x.TechnicalSpecialistInternalTrainingAndCompetencyId))
       .ForAllOtherMembers(x => x.Ignore());

            #endregion 

            #region InternalTraining


            CreateMap<DbModel.TechnicalSpecialistTrainingAndCompetency, DomainModel.TechnicalSpecialistInternalTraining>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.Epin, opt => opt.MapFrom(src => src.TechnicalSpecialist.Pin))
           .ForMember(dest => dest.TrainingDate, opt => opt.MapFrom(src => src.TrainingDate))
           .ForMember(dest => dest.Expiry, opt => opt.MapFrom(src => src.Expiry))
           .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
           .ForMember(dest => dest.Competency, opt => opt.MapFrom(src => src.Competency))
           .ForMember(dest => dest.RecordType, opt => opt.MapFrom(src => src.RecordType))
           .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder))
           .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
           .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
           .ForMember(dest => dest.UpdateCount, opt => opt.MapFrom(src => src.UpdateCount))
           .ForMember(dest => dest.TechnicalSpecialistInternalTrainingTypes, opt => opt.MapFrom(src => src.TechnicalSpecialistTrainingAndCompetencyType))
           .ForMember(dest => dest.IsILearn, opt => opt.MapFrom(src => src.IsIlearn == null || src.IsIlearn == false ? false : true))
           .ForAllOtherMembers(src => src.Ignore());

            CreateMap<DomainModel.TechnicalSpecialistInternalTraining, DbModel.TechnicalSpecialistTrainingAndCompetency>()
            .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
            .ForMember(dest => dest.TechnicalSpecialistId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("DbTechSpecialist") ? ((List<DbModel.TechnicalSpecialist>)context.Options.Items["DbTechSpecialist"])?.FirstOrDefault(x => x.Pin == src.Epin)?.Id : null))
            .ForMember(dest => dest.TrainingDate, opt => opt.MapFrom(src => src.TrainingDate))
            .ForMember(dest => dest.Expiry, opt => opt.MapFrom(src => src.Expiry))
            .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
            .ForMember(dest => dest.Competency, opt => opt.MapFrom(src => src.Competency))
            .ForMember(dest => dest.RecordType, opt => opt.MapFrom(src => CompCertTrainingType.IT.ToString()))
            .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
            .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
            .ForMember(dest => dest.UpdateCount, opt => opt.MapFrom(src => src.UpdateCount))
            .ForAllOtherMembers(src => src.Ignore());

            #endregion

            #region Competency

            CreateMap<DbModel.TechnicalSpecialistTrainingAndCompetency, DomainModel.TechnicalSpecialistCompetency>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Epin, opt => opt.MapFrom(src => src.TechnicalSpecialist.Pin))
            .ForMember(dest => dest.EffectiveDate, opt => opt.MapFrom(src => src.TrainingDate))
            .ForMember(dest => dest.Expiry, opt => opt.MapFrom(src => src.Expiry))
            .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
            .ForMember(dest => dest.Competency, opt => opt.MapFrom(src => src.Competency))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
            .ForMember(dest => dest.RecordType, opt => opt.MapFrom(src => src.RecordType))
            .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
            .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
            .ForMember(dest => dest.UpdateCount, opt => opt.MapFrom(src => src.UpdateCount))
            .ForMember(dest => dest.TechnicalSpecialistCompetencyTypes, opt => opt.MapFrom(src => src.TechnicalSpecialistTrainingAndCompetencyType))
             .ForMember(dest => dest.IsILearn, opt => opt.MapFrom(src => src.IsIlearn == null || src.IsIlearn == false ? false : true)) // added for ILearn
            .ForAllOtherMembers(src => src.Ignore());

            CreateMap<DomainModel.TechnicalSpecialistCompetency, DbModel.TechnicalSpecialistTrainingAndCompetency>()
            .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
            .ForMember(dest => dest.TechnicalSpecialistId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("DbTechSpecialist") ? ((List<DbModel.TechnicalSpecialist>)context.Options.Items["DbTechSpecialist"])?.FirstOrDefault(x => x.Pin == src.Epin)?.Id : null))
            .ForMember(dest => dest.TrainingDate, opt => opt.MapFrom(src => src.EffectiveDate))
            .ForMember(dest => dest.Expiry, opt => opt.MapFrom(src => src.Expiry))
            .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
            .ForMember(dest => dest.Competency, opt => opt.MapFrom(src => src.Competency))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
            .ForMember(dest => dest.RecordType, opt => opt.MapFrom(src => CompCertTrainingType.Co.ToString()))
            .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
            .ForMember(dest => dest.LastModification, opt => opt.MapFrom(src => src.LastModification))
            .ForMember(dest => dest.UpdateCount, opt => opt.MapFrom(src => src.UpdateCount))
            .ForAllOtherMembers(src => src.Ignore());

            #endregion

            #region TechnicalSpecialistNote
            CreateMap<DbModel.TechnicalSpecialistNote, TechnicalSpecialistNoteInfo>()
              .ForMember(source => source.Id, dest => dest.MapFrom(x => x.Id))
              .ForMember(source => source.Epin, dest => dest.MapFrom(x => x.TechnicalSpecialist.Id))
              .ForMember(source => source.CreatedBy, dest => dest.MapFrom(x => x.CreatedBy))
              .ForMember(source => source.CreatedDate, dest => dest.MapFrom(x => x.CreatedDate))
              .ForMember(source => source.Note, dest => dest.MapFrom(x => x.Note))
              .ForMember(source => source.RecordRefId, dest => dest.MapFrom(x => x.RecordRefId))
              .ForMember(source => source.RecordType, dest => dest.MapFrom(x => x.RecordType))
              .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
              .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
              .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
              .ForAllOtherMembers(x => x.Ignore());

            CreateMap<TechnicalSpecialistNoteInfo, DbModel.TechnicalSpecialistNote>()
            .ForMember(source => source.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.Id : 0) : src.Id)))
            .ForMember(source => source.TechnicalSpecialistId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("DbTechSpecialist") ? ((List<DbModel.TechnicalSpecialist>)context.Options.Items["DbTechSpecialist"])?.FirstOrDefault(x => x.Pin == src.Epin)?.Id : null))
            .ForMember(source => source.CreatedBy, dest => dest.MapFrom(x => x.CreatedBy))
            .ForMember(source => source.CreatedDate, dest => dest.MapFrom(x => x.CreatedDate))
            .ForMember(source => source.Note, dest => dest.MapFrom(x => x.Note))
            .ForMember(source => source.RecordRefId, dest => dest.MapFrom(x => x.RecordRefId))
            .ForMember(source => source.RecordType, dest => dest.MapFrom(x => x.RecordType))
            .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
            .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
            .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
            .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region TechnicalSpecialistDraft
            CreateMap<DbModel.Draft, DomainModel.TechnicalSpecialistDraft>()
              .ForMember(source => source.Id, dest => dest.MapFrom(x => x.Id))
              .ForMember(source => source.DraftId, dest => dest.MapFrom(x => x.DraftId))
              .ForMember(source => source.Description, dest => dest.MapFrom(x => x.Description))
              .ForMember(source => source.Moduletype, dest => dest.MapFrom(x => x.Moduletype))
              .ForMember(source => source.SerilizableObject, dest => dest.MapFrom(x => x.SerilizableObject))
              .ForMember(source => source.SerilizationType, dest => dest.MapFrom(x => x.SerilizationType))
              .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
              .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
              .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
              .ForMember(source => source.AssignedBy, dest => dest.MapFrom(x => x.AssignedBy))
               .ForMember(source => source.AssignedTo, dest => dest.MapFrom(x => x.AssignedTo))
              .ForMember(source => source.AssignedOn, dest => dest.MapFrom(x => x.AssignedOn))
              .ForMember(source => source.CreatedBy, dest => dest.MapFrom(x => x.CreatedBy))
              .ForMember(source => source.CreatedOn, dest => dest.MapFrom(x => x.CreatedTo))
              .ReverseMap()
              .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region TechnicalSpecialistTimeOffRequest
            CreateMap<DbModel.TechnicalSpecialistTimeOffRequest, TechnicalSpecialistTimeOffRequest>()
            .ForMember(source => source.TechnicalSpecialistTimeOffRequestId, dest => dest.MapFrom(x => x.Id))
             .ForMember(source => source.TechnicalSpecialistId, dest => dest.MapFrom(x => x.TechnicalSpecialistId))
               .ForMember(source => source.Epin, dest => dest.MapFrom(x => x.TechnicalSpecialist.Pin))
              .ForMember(source => source.ResourceName, dest => dest.MapFrom(x => string.Format("{0} {1} {2}", x.TechnicalSpecialist.LastName, x.TechnicalSpecialist.MiddleName, x.TechnicalSpecialist.FirstName)))
              .ForMember(source => source.LeaveCategoryType, dest => dest.MapFrom(x => x.LeaveType.Name))
               .ForMember(source => source.TimeOffFrom, dest => dest.MapFrom(x => x.FromDate))
              .ForMember(source => source.TimeOffThrough, dest => dest.MapFrom(x => x.ToDate))
               .ForMember(source => source.Comments, dest => dest.MapFrom(x => x.Comments))
             .ForMember(source => source.ApprovedBy, dest => dest.MapFrom(x => x.ApprovedBy))
             .ForMember(source => source.RequestedOn, dest => dest.MapFrom(x => x.RequestedOn))
             .ForMember(source => source.RequestedBy, dest => dest.MapFrom(x => x.RequestedBy))
              .ForMember(source => source.ApprovalStatus, dest => dest.MapFrom(x => x.ApprovalStatus))
             .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
            .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
              .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
             .ForAllOtherMembers(x => x.Ignore());

            CreateMap<TechnicalSpecialistTimeOffRequest, DbModel.TechnicalSpecialistTimeOffRequest>()
                .ForMember(dest => dest.Id, opt => opt.ResolveUsing((src, dst, arg3, context) => (context.Options.Items.ContainsKey("isAssignId") ? (Convert.ToBoolean(context.Options.Items["isAssignId"]) ? src.TechnicalSpecialistTimeOffRequestId : 0) : src.TechnicalSpecialistTimeOffRequestId)))
                .ForMember(dest => dest.TechnicalSpecialistId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("TechnicalSpecialistId") ? ((List<DbModel.TechnicalSpecialist>)context.Options.Items["TechnicalSpecialistId"])?.FirstOrDefault(x => x.Pin == src.Epin)?.Id : null))
                .ForMember(dest => dest.LeaveTypeId, opt => opt.ResolveUsing((src, dst, arg3, context) => context.Options.Items.ContainsKey("LeaveCategoryType") ? ((List<DbModel.Data>)context.Options.Items["LeaveCategoryType"])?.FirstOrDefault(x => x.Name == src.LeaveCategoryType)?.Id : null))
                .ForMember(dest => dest.FromDate, src => src.MapFrom(x => x.TimeOffFrom))
                .ForMember(dest => dest.ToDate, src => src.MapFrom(x => x.TimeOffThrough))
                .ForMember(dest => dest.Comments, src => src.MapFrom(x => x.Comments))
                .ForMember(dest => dest.ApprovedBy, src => src.MapFrom(x => x.ApprovedBy))
                 .ForMember(dest => dest.RequestedBy, src => src.MapFrom(x => x.RequestedBy))
                  .ForMember(dest => dest.RequestedOn, src => src.MapFrom(x => x.RequestedOn))
                .ForMember(dest => dest.ApprovalStatus, src => src.MapFrom(x => x.ApprovalStatus))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForAllOtherMembers(src => src.Ignore());
            #endregion

            #region TechnicalSpecialistCalendar

                CreateMap<DbModel.TechnicalSpecialistCalendar, TechnicalSpecialistCalendar>()
                .ForMember(source => source.Id, dest => dest.MapFrom(x => x.Id))
                .ForMember(source => source.TechnicalSpecialistId, dest => dest.MapFrom(x => x.TechnicalSpecialistId))
                .ForMember(source => source.TechnicalSpecialistName, dest => dest.MapFrom(x => x.TechnicalSpecialist.FirstName))
                .ForMember(source => source.CompanyId, dest => dest.MapFrom(x => x.CompanyId))
                .ForMember(source => source.CalendarRefCode, dest => dest.MapFrom(x => x.CalendarRefCode))
                .ForMember(source => source.CalendarType, dest => dest.MapFrom(x => x.CalendarType))
                .ForMember(source => source.CalendarStatus, dest => dest.MapFrom(x => x.CalendarStatus))
                .ForMember(source => source.CreatedBy, dest => dest.MapFrom(x => x.CreatedBy))
                .ForMember(source => source.EndDateTime, dest => dest.MapFrom(x => x.EndDateTime))
                .ForMember(source => source.IsActive, dest => dest.MapFrom(x => x.IsActive))
                .ForMember(source => source.StartDateTime, dest => dest.MapFrom(x => x.StartDateTime))
                .ForMember(source => source.JobReferenceNumber, dest => dest.ResolveUsing<JobReferenceResolver>())


                // .ForMember(source => source.JobReferenceNumber, dest => dest.ResolveUsing((src, dst, arg3, context) => src.CalendarType == "VISIT" ?
                //                    Convert.ToString(((List<DbModel.Visit>)context.Options.Items["Visit"])?.FirstOrDefault(x => x.Id == src.CalendarRefCode)?.Assignment?.ProjectId)
                //                     : src.CalendarStatus)
                //)


                //.ForMember(source => source.JobReferenceNumber, dest => dest.ResolveUsing((src, dst, arg3, context) => src.CalendarType == "VISIT" ?
                //                    ((List<DbModel.Visit>)context.Options.Items["Visit"])?.FirstOrDefault(x => x.Id == src.CalendarRefCode).Assignment.ProjectId + "-" +
                //                    ((List<DbModel.Visit>)context.Options.Items["Visit"])?.FirstOrDefault(x => x.Id == src.CalendarRefCode).Assignment.AssignmentNumber + "-" +
                //                    ((List<DbModel.Visit>)context.Options.Items["Visit"])?.FirstOrDefault(x => x.Id == src.CalendarRefCode).VisitNumber :
                //                    src.CalendarType == "TIMESHEET" ?
                //                    ((List<DbModel.Timesheet>)context.Options.Items["Timesheet"])?.FirstOrDefault(x => x.Id == src.CalendarRefCode).Assignment.ProjectId + "-" +
                //                    ((List<DbModel.Timesheet>)context.Options.Items["Timesheet"])?.FirstOrDefault(x => x.Id == src.CalendarRefCode).Assignment.AssignmentNumber + "-" +
                //                    ((List<DbModel.Timesheet>)context.Options.Items["Timesheet"])?.FirstOrDefault(x => x.Id == src.CalendarRefCode).TimesheetNumber : src.CalendarStatus)
                //)

                .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
                .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
                .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
                .ForMember(source => source.Description, dest => dest.MapFrom(x => x.Description))
                .ForAllOtherMembers(x => x.Ignore());

                CreateMap<TechnicalSpecialistCalendar,DbModel.TechnicalSpecialistCalendar>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.TechnicalSpecialistId, src => src.MapFrom(x => x.TechnicalSpecialistId))
                .ForMember(dest => dest.CompanyId, src => src.ResolveUsing<Automapper.Resolver.Company.CompanyIdResolver, string>("CompanyCode"))
                .ForMember(dest => dest.CalendarRefCode, src => src.MapFrom(x => x.CalendarRefCode))
                .ForMember(dest => dest.CalendarType, src => src.MapFrom(x => x.CalendarType))
                .ForMember(dest => dest.CalendarStatus, src => src.MapFrom(x => x.CalendarStatus))
                .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => x.CreatedBy))
                .ForMember(dest => dest.EndDateTime, src => src.MapFrom(x => x.EndDateTime))
                .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.IsActive))
                .ForMember(dest => dest.StartDateTime, src => src.MapFrom(x => x.StartDateTime))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.Description, src => src.MapFrom(x => x.Description))
                .ForAllOtherMembers(x => x.Ignore());
             
            CreateMap<DbModel.TechnicalSpecialistTimeOffRequest, DbModel.TechnicalSpecialistCalendar>() 
                .ForMember(dest => dest.TechnicalSpecialistId, src => src.MapFrom(x => x.TechnicalSpecialistId))
                .ForMember(dest => dest.CompanyId, src => src.ResolveUsing<Automapper.Resolver.Company.TsCompanyIdResolver, int>("TechnicalSpecialistId"))
                .ForMember(dest => dest.CalendarRefCode, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.CalendarType, src => src.MapFrom(x => CalendarType.PTO.ToString()))
                .ForMember(dest => dest.CalendarStatus, src => src.MapFrom(x => EnumExtension.DisplayName(CalendarStatus.PTO)))
                .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => x.RequestedBy))
                .ForMember(dest => dest.EndDateTime, src => src.MapFrom(x => x.ToDate))
                .ForMember(dest => dest.IsActive, src => src.MapFrom(x => true))
                .ForMember(dest => dest.StartDateTime, src => src.MapFrom(x => x.FromDate))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForAllOtherMembers(x => x.Ignore());
              
                CreateMap<IList<TechnicalSpecialistCalendar>,TechnicalSpecialistCalendarView>() 
                .ForMember(dest => dest.Resources, src => src.ResolveUsing<Automapper.Resolver.TechnicalSpecialist.TsCalendarResourceResolver, IList<TechnicalSpecialistCalendar>>(x=>x))
                .ForMember(dest => dest.Events, src => src.ResolveUsing<Automapper.Resolver.TechnicalSpecialist.TsCalendarEventsResolver, IList<TechnicalSpecialistCalendar>>(x=>x))
                .ForAllOtherMembers(x => x.Ignore());

            #endregion

            //TechnicalSpecialist Mongo Document Search
            CreateMap<SearchTechnicalSpecialist, Document.Domain.Models.Document.EvolutionMongoDocumentSearch>()
                .ForMember(dest => dest.ModuleCode, src => src.ResolveUsing(x => Common.Enums.ModuleCodeType.TS.ToString()))
                .ForMember(dest => dest.ReferenceCode, src => src.MapFrom(x => x.ePinString))
                .ForMember(dest => dest.Text, src => src.ResolveUsing<SearchTextFormatResolver, string>("DocumentSearchText"))
                .ForMember(dest => dest.DocumentTypes, src => src.ResolveUsing(x => string.IsNullOrEmpty(x.SearchDocumentType) ? null : new List<string>() { x.SearchDocumentType }))
                .ForAllOtherMembers(x => x.Ignore());

        }
        public class JobReferenceResolver : IValueResolver<DbModel.TechnicalSpecialistCalendar,TechnicalSpecialistCalendar, string>
        {
            private readonly ITechnicalSpecialistCalendarService _techCalendarService = null;

            public JobReferenceResolver(ITechnicalSpecialistCalendarService techCalendarService)
            {
                _techCalendarService = techCalendarService;
            }

            public string Resolve(DbModel.TechnicalSpecialistCalendar source, TechnicalSpecialistCalendar destination, string destMember, ResolutionContext context)
            {
                return _techCalendarService.GetJobReference(source.CalendarType, (long)source.CalendarRefCode);
            }
        }
    }
}
