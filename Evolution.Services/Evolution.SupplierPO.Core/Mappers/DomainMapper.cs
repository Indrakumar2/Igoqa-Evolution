using AutoMapper;
using Evolution.Automapper.Resolver.MongoSearch;
using Evolution.Common.Extensions;
using Evolution.SupplierPO.Domain.Models.SupplierPO;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModels = Evolution.SupplierPO.Domain.Models.SupplierPO;

namespace Evolution.SupplierPO.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            string delimiter = ",";
            #region Base Supplier PO
            CreateMap<DbModel.SupplierPurchaseOrder, DomainModels.BaseSupplierPO>()
               .ForMember(src => src.SupplierPOCompletedDate, dest => dest.MapFrom(x => x.CompletionDate))
               .ForMember(src => src.SupplierPOContractNumber, dest => dest.MapFrom(x => x.Project.Contract.ContractNumber))
               .ForMember(src => src.SupplierPOCustomerCode, dest => dest.MapFrom(x => x.Project.Contract.Customer.Code))
               .ForMember(src => src.SupplierPOCustomerName, dest => dest.MapFrom(x => x.Project.Contract.Customer.Name))
               .ForMember(src => src.SupplierPOCustomerProjectName, dest => dest.MapFrom(x => x.Project.CustomerProjectName))
               .ForMember(src => src.SupplierPOCustomerProjectNumber, dest => dest.MapFrom(x => x.Project.CustomerProjectNumber))
               .ForMember(src => src.SupplierPODeliveryDate, dest => dest.MapFrom(x => x.DeliveryDate))
               .ForMember(src => src.SupplierPOId, dest => dest.MapFrom(x => x.Id))
               .ForMember(src => src.SupplierPOMainSupplierId, dest => dest.MapFrom(x => x.SupplierId))
               .ForMember(src => src.SupplierPOMainSupplierName, dest => dest.MapFrom(x => x.Supplier.SupplierName))
               .ForMember(src => src.SupplierPONumber, dest => dest.MapFrom(x => x.SupplierPonumber))
               .ForMember(src => src.SupplierPOProjectNumber, dest => dest.MapFrom(x => x.Project.ProjectNumber))
               .ForMember(src => src.SupplierPOMaterialDescription, dest => dest.MapFrom(x => x.MaterialDescription))
               .ForMember(src => src.SupplierPOStatus, dest => dest.MapFrom(x => x.Status))
               .ForMember(src => src.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
               .ForMember(src => src.LastModification, dest => dest.MapFrom(x => x.LastModification))
               .ForMember(src => src.SupplierPOCompanyName, dest => dest.MapFrom(x=>x.Project.Contract.ContractHolderCompany.Name))
               .ForMember(src => src.SupplierPOCompanyCode, dest => dest.MapFrom(x => x.Project.Contract.ContractHolderCompany.Code))
               .ForMember(src => src.SupplierPOCompanyId,dest  => dest.MapFrom(x=> x.Project.Contract.ContractHolderCompanyId))
               .ForMember(src => src.IsSupplierPOCompanyActive, dest => dest.MapFrom(x => x.Project.Contract.ContractHolderCompany.IsActive)) // D - 619
                .ForMember(src => src.SupplierPOSubSupplierName, dest => dest.MapFrom(x => x.SupplierPurchaseOrderSubSupplier.HasItems()
                                                                                     ? string.Join(delimiter, x.SupplierPurchaseOrderSubSupplier.Select(x1 => x1.Supplier.SupplierName).ToList())
                                                                                     : string.Empty))

              
               .ForMember(src => src.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
               .ReverseMap()
               .ForAllOtherMembers(dest => dest.Ignore());

            CreateMap<DomainModels.BaseSupplierPO, DbModel.SupplierPurchaseOrder>()
                .ForMember(dest => dest.CompletionDate, src => src.MapFrom(x => x.SupplierPOCompletedDate))
                .ForMember(dest => dest.DeliveryDate, src => src.MapFrom(x => x.SupplierPODeliveryDate))
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.SupplierPOId))
                .ForMember(dest => dest.SupplierId, src => src.MapFrom(x => x.SupplierPOMainSupplierId))
                .ForMember(dest => dest.SupplierPonumber, src => src.MapFrom(x => x.SupplierPONumber))
                .ForMember(dest => dest.MaterialDescription, src => src.MapFrom(x => x.SupplierPOMaterialDescription))
                .ForMember(dest => dest.Status, src => src.MapFrom(x => x.SupplierPOStatus))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForAllOtherMembers(src => src.Ignore());

            //Supplier PO Mongo Document Search
            CreateMap<SupplierPOSearch, Document.Domain.Models.Document.EvolutionMongoDocumentSearch>()
                .ForMember(dest => dest.ModuleCode, src => src.ResolveUsing(x => Common.Enums.ModuleCodeType.SUPPO.ToString()))
                .ForMember(dest => dest.ReferenceCode, src => src.MapFrom(x => x.SupplierPONumber))
                .ForMember(dest => dest.Text, src => src.ResolveUsing<SearchTextFormatResolver, string>("DocumentSearchText"))
                .ForMember(dest => dest.DocumentTypes, src => src.ResolveUsing(x => string.IsNullOrEmpty(x.SearchDocumentType) ? null : new List<string>() { x.SearchDocumentType }))
                .ForAllOtherMembers(x => x.Ignore());
            #endregion

            #region SupplierPO
            CreateMap<DbModel.SupplierPurchaseOrder, DomainModels.SupplierPO>()
               .ForMember(src => src.SupplierPOBudget, dest => dest.MapFrom(x => x.BudgetValue))
               .ForMember(src => src.SupplierPOBudgetHours, dest => dest.MapFrom(x => x.BudgetHoursUnit))
               .ForMember(src => src.SupplierPOBudgetWarning, dest => dest.MapFrom(x => x.BudgetWarning))
               .ForMember(src=>src.SupplierPOBudgetHoursWarning,dest=>dest.MapFrom(x=>x.BudgetHoursUnitWarning))
               .ForMember(src => src.SupplierPOCompletedDate, dest => dest.MapFrom(x => x.CompletionDate))
               .ForMember(src => src.SupplierPOContractNumber, dest => dest.MapFrom(x => x.Project.Contract.ContractNumber))
               .ForMember(src => src.SupplierPOCustomerCode, dest => dest.MapFrom(x => x.Project.Contract.Customer.Code))
               .ForMember(src => src.SupplierPOCustomerName, dest => dest.MapFrom(x => x.Project.Contract.Customer.Name))
               .ForMember(src => src.SupplierPOCustomerProjectName, dest => dest.MapFrom(x => x.Project.CustomerProjectName))
               .ForMember(src => src.SupplierPOCustomerProjectNumber, dest => dest.MapFrom(x => x.Project.CustomerProjectNumber))
               .ForMember(src => src.SupplierPODeliveryDate, dest => dest.MapFrom(x => x.DeliveryDate))
               .ForMember(src => src.SupplierPOId, dest => dest.MapFrom(x => x.Id))
               .ForMember(src => src.SupplierPOMainSupplierId, dest => dest.MapFrom(x => x.SupplierId))
               .ForMember(src => src.SupplierPOMainSupplierName, dest => dest.MapFrom(x => x.Supplier.SupplierName))
               .ForMember(src => src.SupplierPOMainSupplierAddress, dest => dest.MapFrom(x => x.Supplier.Address))
               .ForMember(src => src.City, dest => dest.MapFrom(x => x.Supplier.City.Name)) // Added for D-718
               .ForMember(src => src.State, dest => dest.MapFrom(x => x.Supplier.County.Name)) // Added for D-718
               .ForMember(src => src.Country, dest => dest.MapFrom(x => x.Supplier.Country.Name)) // Added for D-718
               .ForMember(src => src.ZipCode, dest => dest.MapFrom(x => x.Supplier.PostalCode)) // Added for D-718
               .ForMember(src => src.SupplierPOCurrency, dest => dest.MapFrom(x => x.Project.InvoiceCurrency))
               .ForMember(src => src.SupplierPONumber, dest => dest.MapFrom(x => x.SupplierPonumber))
               .ForMember(src => src.SupplierPOProjectNumber, dest => dest.MapFrom(x => x.Project.ProjectNumber))
               .ForMember(src => src.SupplierPOMaterialDescription, dest => dest.MapFrom(x => x.MaterialDescription))
               .ForMember(src => src.SupplierPOSubSupplierName, dest => dest.MapFrom(x => x.SupplierPurchaseOrderSubSupplier.Select(x1 => x1.Supplier.SupplierName).ToList() == null
                                                                                    ? string.Empty : string.Join(delimiter, x.SupplierPurchaseOrderSubSupplier.Select(x1 => x1.Supplier.SupplierName).ToList())))
               //.ForMember(src => src.SupplierPOSubSupplierName, dest => dest.ResolveUsing<SubSupplierNameResolver>())
               .ForMember(src => src.SupplierPOStatus, dest => dest.MapFrom(x => x.Status))
               .ForMember(src => src.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
               .ForMember(src => src.LastModification, dest => dest.MapFrom(x => x.LastModification))
               .ForMember(src=>src.SupplierPOCompanyCode, dest =>dest.MapFrom(x=>x.Project.Contract.ContractHolderCompany.Code))
               .ForMember(src => src.SupplierPOCompanyName, dest => dest.MapFrom(x => x.Project.Contract.ContractHolderCompany.Name))
               .ForMember(src => src.IsSupplierPOCompanyActive, dest => dest.MapFrom(x => x.Project.Contract.ContractHolderCompany.IsActive)) // D - 619
               .ForMember(src => src.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
               .ReverseMap()
               .ForAllOtherMembers(dest => dest.Ignore());

            #endregion

            #region SupplierPO Sub Supplier

            CreateMap<DbModel.SupplierPurchaseOrderSubSupplier, DomainModels.SupplierPOSubSupplier>()
                .ForMember(src => src.SupplierPONumber, dest => dest.MapFrom(x => x.SupplierPurchaseOrder.SupplierPonumber))
                .ForMember(src => src.SupplierId, dest => dest.MapFrom(x => x.SupplierId))
                .ForMember(src => src.SubSupplierId, dest => dest.MapFrom(x => x.Id))
                .ForMember(src => src.SupplierPOId, dest => dest.MapFrom(x => x.SupplierPurchaseOrderId))
                .ForMember(src => src.SubSupplierName, dest => dest.MapFrom(x => x.Supplier.SupplierName))
                .ForMember(src => src.SubSupplierAddress, dest => dest.MapFrom(x => x.Supplier.Address))
                .ForMember(src => src.Country, dest => dest.MapFrom(x => x.Supplier.Country.Name))
                .ForMember(src => src.State, dest => dest.MapFrom(x => x.Supplier.County.Name))
                .ForMember(src => src.City, dest => dest.MapFrom(x => x.Supplier.City.Name))
                .ForMember(src => src.PostalCode, dest => dest.MapFrom(x => x.Supplier.PostalCode))

                .ForMember(src => src.LastModification, dest => dest.MapFrom(x => x.LastModification))
                .ForMember(src => src.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
                .ForMember(src => src.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
                .ReverseMap()
                .ForAllOtherMembers(dest => dest.Ignore());

            #endregion

            #region SupplierPO Note
            CreateMap<DbModel.SupplierPurchaseOrderNote, DomainModels.SupplierPONote>()
                .ForMember(src => src.SupplierPOId, dest => dest.MapFrom(x => x.SupplierPurchaseOrderId))
                .ForMember(src => src.SupplierPONoteId, dest => dest.MapFrom(x => x.Id))
                .ForMember(src => src.Notes, dest => dest.MapFrom(x => x.Note))
                .ForMember(src => src.CreatedBy, dest => dest.MapFrom(x => x.CreatedBy))
                .ForMember(src => src.CreatedOn, dest => dest.MapFrom(x => x.CreatedDate))
                .ForMember(src => src.LastModification, dest => dest.MapFrom(x => x.LastModification))
                .ForMember(src => src.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
                .ForMember(src => src.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
                .ForMember(src => src.SupplierPONumber, dest => dest.MapFrom(x => x.SupplierPurchaseOrder.SupplierPonumber))
                .ReverseMap()
                .ForAllOtherMembers(dest => dest.Ignore());
            #endregion

            #region SuplierPO Document
            //CreateMap<DbModel.SupplierPurchaseOrderDocument, DomainModels.SupplierPODocument>()
            //    .ForMember(src => src.SupplierPOId, dest => dest.MapFrom(x => x.SupplierPurchaseOrder.Id))
            //    .ForMember(src => src.SupplierPODocumentId, dest => dest.MapFrom(x => x.Id))
            //    .ForMember(src => src.DocumentName, dest => dest.MapFrom(x => x.Name))
            //    .ForMember(src => src.DocumentType, dest => dest.MapFrom(x => x.DocumentType))
            //    .ForMember(src => src.DocumentSize, dest => dest.MapFrom(x => x.DocumentSize))
            //    .ForMember(src => src.IsVisibleToCustomer, dest => dest.MapFrom(x => x.IsVisibleToCustomer))
            //    .ForMember(src => src.IsVisibleToTS, dest => dest.MapFrom(x => x.IsVisibleToTechnicalSpecialist))
            //    //.ForMember(src => src.UploadedOn, dest => dest.MapFrom(x => x.UploadedOn))
            //    .ForMember(src => src.LastModification, dest => dest.MapFrom(x => x.LastModification))
            //    .ForMember(src => src.ModifiedBy, dest => dest.MapFrom(x => x.ModifiedBy))
            //    .ForMember(src => src.UpdateCount, dest => dest.MapFrom(x => x.UpdateCount))
            //    .ForMember(src => src.DocumentUniqueName, dest => dest.MapFrom(x => x.UploadedDataId))
            //    .ForMember(src => src.SupplierPONumber, dest => dest.MapFrom(x => x.SupplierPurchaseOrder.SupplierPonumber))
            //    .ReverseMap()
            //.ForAllOtherMembers(dest => dest.Ignore());
            #endregion
        }


    }
    public class SubSupplierNameResolver : IValueResolver<DbModel.SupplierPurchaseOrder, DomainModels.SupplierPO, string>
    {
        public SubSupplierNameResolver()
        {
        }

        public string Resolve(DbModel.SupplierPurchaseOrder source, DomainModels.SupplierPO destination, string destMember, ResolutionContext context)
        {
            string delimiter = ",";
            var subSupplierNames = source?.SupplierPurchaseOrderSubSupplier?.Select(x => x.Supplier.SupplierName).ToList();
            if (subSupplierNames != null)
                return subSupplierNames?.Count() > 0 ? string.Join(delimiter, subSupplierNames.ToArray()) : string.Empty;
            else
                return string.Empty;
        }
    }

}
