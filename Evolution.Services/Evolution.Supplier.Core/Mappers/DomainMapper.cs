using AutoMapper;
using Evolution.Automapper.Resolver.MongoSearch;
using Evolution.Common.Models.Budget;
using Evolution.Supplier.Domain.Models.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.Supplier.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            #region Supplier

            CreateMap<DbModel.Supplier, DomainModel.Supplier>()
                  .ForMember(source => source.SupplierId, dest => dest.MapFrom(x => x.Id))
                  .ForMember(source => source.SupplierName, dest => dest.MapFrom(x => x.SupplierName))
                  .ForMember(source => source.SupplierAddress, dest => dest.MapFrom(x => x.Address))
                  .ForMember(source => source.Country, dest => dest.MapFrom(x => x.Country.Name))
                  .ForMember(source => source.State, dest => dest.MapFrom(x => x.County.Name))
                  .ForMember(source => source.City, dest => dest.MapFrom(x => x.City.Name))
                  .ForMember(source => source.CountryId ,dest => dest.MapFrom(x=>x.Country.Id))  //Added for D-1076
                  .ForMember(source => source.StateId, dest => dest.MapFrom(x => x.County.Id))   //Added for D-1076
                  .ForMember(source => source.CityId, dest => dest.MapFrom(x => x.City.Id))      //Added for D-1076
                  .ForMember(source => source.PostalCode, dest => dest.MapFrom(x => x.PostalCode))
                  .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
                  .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
                  .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
                  .ForMember(source => source.ActionByUser, dest => dest.MapFrom(x => x.ModifiedBy))
                  .ReverseMap()
                  .ForAllOtherMembers(x => x.Ignore());

            //Supplier Mongo Document Search
            CreateMap<SupplierSearch, Document.Domain.Models.Document.EvolutionMongoDocumentSearch>()
                .ForMember(dest => dest.ModuleCode, src => src.ResolveUsing(x => Common.Enums.ModuleCodeType.SUP.ToString()))
                .ForMember(dest => dest.ReferenceCode, src => src.MapFrom(x => x.SupplierId == null ? null : x.SupplierId.ToString()))
                .ForMember(dest => dest.Text, src => src.ResolveUsing<SearchTextFormatResolver, string>("DocumentSearchText"))
                .ForMember(dest => dest.DocumentTypes, src => src.ResolveUsing(x => string.IsNullOrEmpty(x.SearchDocumentType) ? null : new List<string>() { x.SearchDocumentType }))
                .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region SupplierContact

            CreateMap<DbModel.SupplierContact, DomainModel.SupplierContact>()
                  .ForMember(source => source.SupplierContactId, dest => dest.MapFrom(x => x.Id))
                  .ForMember(source => source.SupplierId, dest => dest.MapFrom(x => x.SupplierId))
                  .ForMember(source => source.SupplierName, dest => dest.MapFrom(x => x.Supplier.SupplierName))
                  .ForMember(source => source.SupplierContactName, dest => dest.MapFrom(x => x.SupplierContactName))
                  .ForMember(source => source.SupplierTelephoneNumber, dest => dest.MapFrom(x => x.TelephoneNumber))
                  .ForMember(source => source.SupplierFaxNumber, dest => dest.MapFrom(x => x.FaxNumber))
                  .ForMember(source => source.SupplierMobileNumber, dest => dest.MapFrom(x => x.MobileNumber))
                  .ForMember(source => source.SupplierEmail, dest => dest.MapFrom(x => x.EmailId))
                  .ForMember(source => source.OtherContactDetails, dest => dest.MapFrom(x => x.OtherContactDetails))
                  .ForMember(source => source.LastModification, dest => dest.MapFrom(x => x.LastModification))
                  .ForMember(source => source.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
                  .ForMember(source => source.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
                  .ForMember(source => source.ActionByUser, dest => dest.MapFrom(x => x.ModifiedBy))
                  .ReverseMap()
                  .ForAllOtherMembers(x => x.Ignore());

            #endregion

            #region Supplier Note
            CreateMap<DbModel.SupplierNote, DomainModel.SupplierNote>()
              .ForMember(dest => dest.SupplierNoteId, source => source.MapFrom(x => x.Id))
              .ForMember(dest => dest.SupplierId, source => source.MapFrom(x => x.SupplierId))
              .ForMember(dest => dest.SupplierName, source => source.MapFrom(x => x.Supplier.SupplierName))
              .ForMember(dest => dest.CreatedOn, source => source.MapFrom(x => x.CreatedDate))
              .ForMember(dest => dest.CreatedBy, source => source.MapFrom(x => x.CreatedBy))
              .ForMember(dest => dest.Notes, source => source.MapFrom(x => x.Note))
              .ForMember(dest => dest.LastModification, source => source.MapFrom(x => x.LastModification))
              .ForMember(dest => dest.UpdateCount, source => source.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.ModifiedBy, source => source.MapFrom(x => x.ModifiedBy))
              .ForMember(source => source.ActionByUser, dest => dest.MapFrom(x => x.ModifiedBy))
              .ReverseMap()
              .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region Supplier performance Report

            string delimiter = ",";
            CreateMap<DbModel.VisitSupplierPerformance, DomainModel.SupplierPerformanceReport>()
                .ForMember(dest => dest.City, src => src.MapFrom(x => x.Visit.Supplier.City.Name))
                .ForMember(dest => dest.State, src => src.MapFrom(x => x.Visit.Supplier.City.County.Name))
                .ForMember(dest => dest.Country, src => src.MapFrom(x => x.Visit.Supplier.City.County.Country.Name))
                .ForMember(dest => dest.ContractHoldingCompanyCode, src => src.MapFrom(x => x.Visit.Assignment.ContractCompany.Code))
                .ForMember(dest => dest.ContractHoldingCompanyName, src => src.MapFrom(x => x.Visit.Assignment.ContractCompany.Name))
                .ForMember(dest => dest.ContractNumber, src => src.MapFrom(x => x.Visit.Assignment.Project.Contract.ContractNumber))
                .ForMember(dest => dest.CustomerCode, src => src.MapFrom(x => x.Visit.Assignment.Project.Contract.Customer.Code))
                .ForMember(dest => dest.CustomerName, src => src.MapFrom(x => x.Visit.Assignment.Project.Contract.Customer.Name))
                .ForMember(dest => dest.OperatingCompanyCode, src => src.MapFrom(x => x.Visit.Assignment.OperatingCompany.Code))
                .ForMember(dest => dest.OperatingCompanyName, src => src.MapFrom(x => x.Visit.Assignment.OperatingCompany.Name))
                .ForMember(dest => dest.ProjectNumber, src => src.MapFrom(x => x.Visit.Assignment.Project.ProjectNumber))
                .ForMember(dest => dest.SupplierName, src => src.MapFrom(x => x.Visit.Supplier.SupplierName))
                .ForMember(dest => dest.SupplierPoNumber, src => src.MapFrom(x => x.Visit.Supplier.SupplierPurchaseOrder.Select(x1 => x1.SupplierPonumber).ToList() == null ? string.Empty
                                                                                    : string.Join(delimiter, x.Visit.Supplier.SupplierPurchaseOrder.Select(x2 => x2.SupplierPonumber).ToArray())))

                .ForMember(dest => dest.supplierId, src => src.MapFrom(x => x.Visit.SupplierId))
                .ForMember(dest => dest.VisitDate, src => src.MapFrom(x => x.Visit.FromDate))
                .ForMember(dest => dest.VisitNumber, src => src.MapFrom(x => x.Visit.VisitNumber))
                .ForMember(dest => dest.VisitReportNumber, src => src.MapFrom(x => x.Visit.ReportNumber))
                .ForMember(dest => dest.SupplierPerformanceType, src => src.MapFrom(x => x.PerformanceType))
                .ForMember(dest => dest.NCRReference, src => src.MapFrom(x => x.Score))
                .ForMember(dest => dest.NcrClosureOutDate, src => src.MapFrom(x => x.NcrcloseOutDate))
                .ForMember(dest => dest.AssignmentNumber, src => src.MapFrom(x => x.Visit.Assignment.AssignmentNumber))
                .ForMember(dest => dest.TechnicalSpecialistsName, src => src.MapFrom(x => string.Join(delimiter, x.Visit.VisitTechnicalSpecialist.Select(x1 => x1.TechnicalSpecialist.LastName + " ," + x1.TechnicalSpecialist.FirstName + " (" + x1.TechnicalSpecialist.Pin + ")").ToArray())))
                .ReverseMap()
                .ForAllOtherMembers(src => src.Ignore());


            CreateMap<DbModel.VisitSupplierPerformance, DomainModel.SupplierPerformanceReportsearch>()
                .ForMember(dest => dest.ContractHoldingCompanyCode, src => src.MapFrom(x => x.Visit.Assignment.ContractCompany.Code))
                .ForMember(dest => dest.ContractHoldingCompanyName, src => src.MapFrom(x => x.Visit.Assignment.ContractCompany.Name))
                .ForMember(dest => dest.ContractNumber, src => src.MapFrom(x => x.Visit.Assignment.Project.Contract.ContractNumber))
                .ForMember(dest => dest.CustomerCode, src => src.MapFrom(x => x.Visit.Assignment.Project.Contract.Customer.Code))
                .ForMember(dest => dest.CustomerName, src => src.MapFrom(x => x.Visit.Assignment.Project.Contract.Customer.Name))
                .ForMember(dest => dest.OperatingCompanyCode, src => src.MapFrom(x => x.Visit.Assignment.OperatingCompany.Code))
                .ForMember(dest => dest.OperatingCompanyName, src => src.MapFrom(x => x.Visit.Assignment.OperatingCompany.Name))
                .ForMember(dest => dest.ProjectNumber, src => src.MapFrom(x => x.Visit.Assignment.Project.ProjectNumber))
                .ForMember(dest => dest.SupplierName, src => src.MapFrom(x => x.Visit.Supplier.SupplierName))
                .ForMember(dest => dest.AssignmentNumber, src => src.MapFrom(x => x.Visit.Assignment.AssignmentNumber))
                .ForMember(dest => dest.SupplierPoNumber, src => src.MapFrom(x => x.Visit.Supplier.SupplierPurchaseOrder.Select(x1 => x1.SupplierPonumber)))

                .ForMember(dest => dest.supplierId, src => src.MapFrom(x => x.Visit.SupplierId))
                .ReverseMap()
                .ForAllOtherMembers(src => src.Ignore());



            #endregion

        }
    }
}
