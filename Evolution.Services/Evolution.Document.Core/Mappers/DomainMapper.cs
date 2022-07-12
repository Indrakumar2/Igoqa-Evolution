using AutoMapper;
using Evolution.Document.Domain.Models.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Document.Domain.Models.Document;
using Evolution.Common.Enums;

namespace Evolution.Document.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            this.MapModel();
        }

        public void MapModel()
        {
          //  this.MapDocumentApproval();
            this.MogoDocumentToMongoDocumentSearch();
            this.MogoDocumentToMongoDocumentResult();
            this.MapUniqueDocumentToModuleDocument();
            this.MapDocument();

        }

        //public void MapDocumentApproval()
        //{
        //    CreateMap<DbModel.DocumentApproval, DomainModel.DocumentApproval>()
        //                    .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
        //                    .ForMember(dest => dest.DocumentSourceId, src => src.MapFrom(x => x.SourceId))
        //                    .ForMember(dest => dest.DocumentSourceModule, src => src.MapFrom(x => x.SourceModule))
        //                    .ForMember(dest => dest.DocumentTargetId, src => src.MapFrom(x => x.TargetId))
        //                    .ForMember(dest => dest.DocumentTargetModule, src => src.MapFrom(x => x.TargetModule))
        //                    .ForMember(dest => dest.DocumentName, src => src.MapFrom(x => x.DocumentName))
        //                    .ForMember(dest => dest.DocumentType, src => src.MapFrom(x => x.DocumentType))
        //                    .ForMember(dest => dest.DocumentSize, src => src.MapFrom(x => x.DocumentSize))
        //                    .ForMember(dest => dest.DocumentUploadedDate, src => src.MapFrom(x => x.UploadedDate))
        //                    .ForMember(dest => dest.DocumentApprovedDate, src => src.MapFrom(x => x.ApprovedDate))
        //                    .ForMember(dest => dest.DocumentApprovedBy, src => src.MapFrom(x => x.Coordinator.SamaccountName))
        //                    //.ForMember(dest => dest.DocumentUploadedBy, src => src.MapFrom(x => x.UploadedUser.SamaccountName))
        //                    .ForMember(dest => dest.IsSpecialistVisible, src => src.MapFrom(x => x.IsSpecialistVisible))
        //                    .ForMember(dest => dest.IsApproved, src => src.MapFrom(x => x.IsApproved))
        //                    // .ForMember(dest => dest.DocumentUploadId, src => src.MapFrom(x => x.DocumentUploadId))
        //                    //.ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
        //                    .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
        //                    .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => Convert.ToInt32(x.UpdateCount)))
        //                    .ReverseMap()
        //                    .ForAllOtherMembers(src => src.Ignore());
        //}

        public void MapDocument()
        {
            CreateMap<DbModel.Document, ModuleDocument>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.DocumentName, src => src.MapFrom(x => x.DocumentName))
                .ForMember(dest => dest.DocumentType, src => src.MapFrom(x => x.DocumentType))
                .ForMember(dest => dest.DocumentSize, src => src.MapFrom(x => x.Size))
                .ForMember(dest => dest.IsVisibleToTS, src => src.MapFrom(x => x.IsVisibleToTechSpecialist))
                .ForMember(dest => dest.IsVisibleToCustomer, src => src.MapFrom(x => x.IsVisibleToCustomer))
                .ForMember(dest => dest.IsVisibleOutOfCompany, src => src.MapFrom(x => x.IsVisibleToOutsideOfCompany))
                .ForMember(dest => dest.ModuleCode, src => src.MapFrom(x => x.ModuleCode))
                .ForMember(dest => dest.ModuleRefCode, src => src.MapFrom(x => x.ModuleRefCode))
                .ForMember(dest => dest.SubModuleRefCode, src => src.MapFrom(x => x.SubModuleRefCode))
                .ForMember(dest => dest.DocumentUniqueName, src => src.MapFrom(x => x.DocumentUniqueName))
                .ForMember(dest => dest.CreatedOn, src => src.MapFrom(x => x.CreatedDate))
                .ForMember(dest => dest.Status, src => src.MapFrom(x => x.Status.Trim()))
                .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => x.CreatedBy))
                .ForMember(dest => dest.Comments, src => src.MapFrom(x => x.Comments))
                .ForMember(dest => dest.ExpiryDate, src => src.MapFrom(x => x.ExpiryDate))
                .ForMember(dest => dest.DisplayOrder, src => src.MapFrom(x => x.DisplayOrder))
                .ForMember(dest => dest.IsForApproval, src => src.MapFrom(x => x.IsForApproval))
                .ForMember(dest => dest.ApprovalDate, src => src.MapFrom(x => x.ApprovalDate))
                .ForMember(dest => dest.ApprovedBy, src => src.MapFrom(x => x.ApprovedBy))
                .ForMember(dest => dest.DocumentTitle, src => src.MapFrom(x => x.DocumentTitle))
                .ForMember(dest => dest.FilePath, src => src.MapFrom(x => x.FilePath))
                .ForAllOtherMembers(src => src.Ignore());

            CreateMap<ModuleDocument, DbModel.Document>()
              .ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id))
              .ForMember(dest => dest.DocumentName, src => src.MapFrom(x => x.DocumentName))
              .ForMember(dest => dest.DocumentType, src => src.MapFrom(x => x.DocumentType))
              .ForMember(dest => dest.Size, src => src.MapFrom(x => x.DocumentSize))
              .ForMember(dest => dest.IsVisibleToTechSpecialist, src => src.MapFrom(x => x.IsVisibleToTS))
              .ForMember(dest => dest.IsVisibleToCustomer, src => src.MapFrom(x => x.IsVisibleToCustomer))
              .ForMember(dest => dest.IsVisibleToOutsideOfCompany, src => src.MapFrom(x => x.IsVisibleOutOfCompany))
              .ForMember(dest => dest.ModuleCode, src => src.MapFrom(x => x.ModuleCode))
              .ForMember(dest => dest.ModuleRefCode, src => src.MapFrom(x => x.ModuleRefCode))
              .ForMember(dest => dest.SubModuleRefCode, src => src.MapFrom(x => (x.ModuleCode == ModuleCodeType.VST.ToString() || x.ModuleCode == ModuleCodeType.TIME.ToString() ? x.SubModuleRefCode : (x.SubModuleRefCode ?? "0")))) //Remove submodulerefcode filter in express for Visit/Timesheet
              .ForMember(dest => dest.DocumentUniqueName, src => src.MapFrom(x => x.DocumentUniqueName))
              .ForMember(dest => dest.CreatedDate, src => src.MapFrom(x => x.CreatedOn))
              .ForMember(dest => dest.Status, src => src.MapFrom(x => x.Status.Trim()))
              .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => x.CreatedBy))
              .ForMember(dest => dest.Comments, src => src.MapFrom(x => x.Comments))
              .ForMember(dest => dest.ExpiryDate, src => src.MapFrom(x => x.ExpiryDate))
              .ForMember(dest => dest.DisplayOrder, src => src.MapFrom(x => x.DisplayOrder))
              .ForMember(dest => dest.IsForApproval, src => src.MapFrom(x => x.IsForApproval))
              .ForMember(dest => dest.ApprovalDate, src => src.MapFrom(x => x.ApprovalDate))
              .ForMember(dest => dest.ApprovedBy, src => src.MapFrom(x => x.ApprovedBy))
              .ForMember(dest => dest.DocumentTitle, src => src.MapFrom(x => x.DocumentTitle))
              .ForMember(dest => dest.FilePath, src => src.MapFrom(x => x.FilePath))
              .ForAllOtherMembers(src => src.Ignore());

        }

        public void MapUniqueDocumentToModuleDocument()
        {
            CreateMap<DomainModel.DocumentUniqueNameDetail, ModuleDocument>()
                .ForMember(dest => dest.DocumentName, src => src.MapFrom(x => x.DocumentName))
                .ForMember(dest => dest.ModuleCode, src => src.MapFrom(x => x.ModuleCode))
                .ForMember(dest => dest.ModuleRefCode, src => src.MapFrom(x => x.ModuleCodeReference))
                .ForMember(dest => dest.SubModuleRefCode, src => src.MapFrom(x => x.SubModuleCodeReference))
                .ForMember(dest => dest.DocumentUniqueName, src => src.MapFrom(x => x.UniqueName))
                .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => x.RequestedBy))
                .ForMember(dest => dest.DocumentType, src => src.MapFrom(x => x.DocumentType))
                .ForMember(dest => dest.DocumentSize, src => src.MapFrom(x => x.DocumentSize))
                .ReverseMap()
                .ForAllOtherMembers(src => src.Ignore());

        }

        public void MogoDocumentToMongoDocumentSearch()
        {
            CreateMap<DomainModel.EvolutionMongoDocument, EvolutionMongoDocumentSearch>()
                            .ForMember(dest => dest.ModuleCode, src => src.MapFrom(x => x.ModuleCode))
                            .ForMember(dest => dest.DocumentTypes, src => src.MapFrom(x => new List<string>() { x.DocumentType }))
                            .ForMember(dest => dest.ReferenceCode, src => src.MapFrom(x => x.ReferenceCode))
                            .ForMember(dest => dest.SubReferenceCode, src => src.MapFrom(x => x.SubReferenceCode))
                            .ForMember(dest => dest.Text, src => src.MapFrom(x => x.Text))
                            .ForMember(dest => dest.UniqueName, src => src.MapFrom(x => x.UniqueName))
                            .ForAllOtherMembers(src => src.Ignore());

            CreateMap<EvolutionMongoDocumentSearch, DomainModel.EvolutionMongoDocument>()
                            .ForMember(dest => dest.ModuleCode, src => src.MapFrom(x => x.ModuleCode))
                            .ForMember(dest => dest.DocumentType, src => src.MapFrom(x => x.DocumentTypes != null ? x.DocumentTypes.FirstOrDefault() : null))
                            .ForMember(dest => dest.ReferenceCode, src => src.MapFrom(x => x.ReferenceCode))
                            .ForMember(dest => dest.SubReferenceCode, src => src.MapFrom(x => x.SubReferenceCode))
                            .ForMember(dest => dest.Text, src => src.MapFrom(x => x.Text))
                            .ForMember(dest => dest.UniqueName, src => src.MapFrom(x => x.UniqueName))
                            .ForAllOtherMembers(src => src.Ignore());
        }

        public void MogoDocumentToMongoDocumentResult()
        {
            CreateMap<DomainModel.EvolutionMongoDocument, DbRepository.MongoModels.DocumentResult>()
                            .ForMember(dest => dest.DocumentData, src => src.MapFrom(x => System.Text.Encoding.ASCII.GetBytes(x.Text)))
                            .ForAllOtherMembers(src => src.Ignore());
        }
    }
}
