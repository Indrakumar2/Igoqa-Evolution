using AutoMapper;
using Evolution.Automapper.Resolver.MongoSearch;
using Evolution.Common.Extensions;
using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.Customer.Domain.Models.Customers;
using System;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Customer.Domain.Models.Customers;

namespace Evolution.Customer.Core.Mappers
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            CreateMap<string, bool>().ConvertUsing(str => str.ToUpper() == "YES");

            CreateMap<DomainModel.Customer, DbModel.Customer>()
                   .ForMember(dest => dest.Id, src => src.MapFrom(x => 0))
                   .ForMember(dest => dest.Code, src => src.MapFrom(x => x.CustomerCode))
                   .ForMember(dest => dest.Name, src => src.MapFrom(x => x.CustomerName))
                   .ForMember(dest => dest.ParentName, src => src.MapFrom(x => x.ParentCompanyName))
                   .ForMember(dest => dest.Miiwaid, src => src.MapFrom(x => x.MIIWAId))
                   .ForMember(dest => dest.MiiwaparentId, src => src.MapFrom(x => x.MIIWAParentId))
                   .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.Active))
                   .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                   .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                   .ForMember(dest => dest.LastModification, src => src.MapFrom(x => String.IsNullOrEmpty(x.RecordStatus) ? x.LastModifiedOn : DateTime.UtcNow))
                   .ForMember(dest => dest.Contract, src => src.Ignore())
                   .ForMember(dest => dest.CustomerAddress, src => src.Ignore())
                   .ForMember(dest => dest.CustomerAssignmentReferenceType, src => src.Ignore())
                   .ForMember(dest => dest.CustomerCompanyAccountReference, src => src.Ignore())
                   .ForMember(dest => dest.CustomerNote, src => src.Ignore());

            CreateMap<DbModel.Customer, DomainModel.Customer>()
                .ForMember(dest => dest.CustomerCode, src => src.MapFrom(x => x.Code))
                .ForMember(dest => dest.CustomerId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.CustomerName, src => src.MapFrom(x => x.Name))
                .ForMember(dest => dest.ParentCompanyName, src => src.MapFrom(x => x.ParentName))
                .ForMember(dest => dest.MIIWAId, src => src.MapFrom(x => x.Miiwaid))
                .ForMember(dest => dest.MIIWAParentId, src => src.MapFrom(x => x.MiiwaparentId))
                .ForMember(dest => dest.Active, src => src.MapFrom(x => x.IsActive ? "Yes" : "No"));

            CreateMap<DomainModel.CustomerAddress, DbModel.CustomerAddress>()
                   .ForMember(dest => dest.Id, src => src.MapFrom(x => x.RecordStatus.IsRecordStatusNew() ? 0 : x.AddressId))
                   .ForMember(dest => dest.CustomerId, src => src.Ignore())
                   //.ForMember(dest => dest.Address, src => src.MapFrom(x => x.Address.Trim().Replace("\n","").Replace("\t","")))
                   .ForMember(dest => dest.Address, src => src.MapFrom(x => x.Address.Trim()))
                   .ForMember(dest => dest.CityId, src => src.ResolveUsing<CityIdResolver>())
                   .ForMember(dest => dest.PostalCode, src => src.MapFrom(x => x.PostalCode))
                   .ForMember(dest => dest.Euvatprefix, src => src.MapFrom(x => x.EUVatPrefix))
                   .ForMember(dest => dest.VatTaxRegistrationNo, src => src.MapFrom(x => x.VatTaxRegNumber))
                   .ForMember(dest => dest.LastModification, src => src.MapFrom(x => String.IsNullOrEmpty(x.RecordStatus) ? x.LastModifiedOn : DateTime.UtcNow))
                   .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                   .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                   .ForMember(dest => dest.CustomerContact, src => src.MapFrom(x => x.Contacts))
                   .ForMember(dest => dest.City, src => src.Ignore())
                   .ForMember(dest => dest.Customer, src => src.Ignore());

            CreateMap<DbModel.CustomerAddress, DomainModel.CustomerAddress>()
                   .ForMember(dest => dest.CustomerCode, dest => dest.MapFrom(x => x.Customer.Code))
                  .ForMember(dest => dest.AddressId, src => src.MapFrom(x => x.Id))
                   .ForMember(dest => dest.Address, src => src.MapFrom(x => x.Address.Trim().Replace("\n", "").Replace("\t", "")))
                   .ForMember(dest => dest.Country, src => src.MapFrom(x => x.City.County.Country.Name))
                   .ForMember(dest => dest.County, src => src.MapFrom(x => x.City.County.Name))
                   .ForMember(dest => dest.City, src => src.MapFrom(x => x.City.Name))
                   .ForMember(dest => dest.CountryId, src => src.MapFrom(x => x.City.County.CountryId)) //Changes for D1536
                   .ForMember(dest => dest.StateId, src => src.MapFrom(x => x.City.CountyId)) //Changes for D1536
                   .ForMember(dest => dest.CityId, src => src.MapFrom(x => x.City.Id)) //Changes for D1536
                   .ForMember(dest => dest.PostalCode, src => src.MapFrom(x => x.PostalCode))
                   .ForMember(dest => dest.EUVatPrefix, src => src.MapFrom(x => x.Euvatprefix))
                   .ForMember(dest => dest.VatTaxRegNumber, src => src.MapFrom(x => x.VatTaxRegistrationNo))
                   .ForMember(dest => dest.LastModifiedOn, src => src.MapFrom(x => x.LastModification))
                   .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                   .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                    .ForMember(dest => dest.Contacts, src => src.MapFrom(x => x.CustomerContact));


            CreateMap<DbModel.CustomerAssignmentReferenceType, DomainModel.CustomerAssignmentReference>()
                .ForMember(dest => dest.CustomerAssignmentReferenceId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.AssignmentRefType, src => src.MapFrom(x => x.AssignmentReference.Name))
                .ForMember(dest => dest.LastModifiedOn, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.RecordStatus, src => src.Ignore())
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount));

            CreateMap<DomainModel.CustomerAssignmentReference, DbModel.CustomerAssignmentReferenceType>()
                  .ForMember(dest => dest.Id, src => src.MapFrom(x => x.RecordStatus.IsRecordStatusNew() ? 0 : x.CustomerAssignmentReferenceId))
                  .ForMember(dest => dest.CustomerId, src => src.Ignore())
                  .ForMember(dest => dest.AssignmentReferenceId, src => src.ResolveUsing<AssignmentReferenceTypeResolver>())
                  .ForMember(dest => dest.LastModification, src => src.MapFrom(x => String.IsNullOrEmpty(x.RecordStatus) ? x.LastModifiedOn : DateTime.UtcNow))
                  .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                  .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                  .ForMember(dest => dest.AssignmentReference, src => src.Ignore())
                  .ForMember(dest => dest.Customer, src => src.Ignore());

            CreateMap<DomainModel.CustomerCompanyAccountReferenceDetail, DbModel.CustomerCompanyAccountReference>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.RecordStatus.IsRecordStatusNew() ? 0 : x.CustomerCompanyAccountReferenceId))
                .ForMember(dest => dest.CustomerId, src => src.MapFrom(x => x.CustomerId))
                .ForMember(dest => dest.CompanyId, src => src.MapFrom(x => x.CompanyId))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => String.IsNullOrEmpty(x.RecordStatus) ? x.LastModifiedOn : DateTime.UtcNow))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.AccountReference, src => src.MapFrom(x => x.AccountReferenceValue));

            CreateMap<DbModel.CustomerCompanyAccountReference, DomainModel.CustomerCompanyAccountReference>()
              .ForMember(dest => dest.CustomerCompanyAccountReferenceId, src => src.MapFrom(x => x.Id))
              .ForMember(dest => dest.AccountReferenceValue, src => src.MapFrom(x => x.AccountReference))
              .ForMember(dest => dest.CompanyCode, src => src.MapFrom(x => x.Company.Code))
              .ForMember(dest => dest.CompanyName, src => src.MapFrom(x => x.Company.Name))
              .ForMember(dest => dest.LastModifiedOn, src => src.MapFrom(x => x.LastModification))
              .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
              .ForMember(dest => dest.RecordStatus, src => src.Ignore())
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount));

            CreateMap<DomainModel.CustomerCompanyAccountReference, DbModel.CustomerCompanyAccountReference>()
              .ForMember(dest => dest.AccountReference, src => src.MapFrom(x => x.AccountReferenceValue))
              .ForMember(dest => dest.Id, src => src.MapFrom(x => x.RecordStatus.IsRecordStatusNew() ? 0 : x.CustomerCompanyAccountReferenceId))
              .ForMember(dest => dest.CustomerId, src => src.Ignore())
              .ForMember(dest => dest.CompanyId, src => src.ResolveUsing<CompanyIdResolver>())
              .ForMember(dest => dest.LastModification, src => src.MapFrom(x => String.IsNullOrEmpty(x.RecordStatus) ? x.LastModifiedOn : DateTime.UtcNow))
              .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
              .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
              .ForMember(dest => dest.Customer, src => src.Ignore());

            CreateMap<DbModel.CustomerContact, DomainModel.Contact>()
                .ForMember(dest => dest.ContactId, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.CustomerAddressId, src => src.MapFrom(x => x.CustomerAddressId))
                .ForMember(dest => dest.ContactAddress, src => src.MapFrom(x => x.CustomerAddress.Address))
                .ForMember(dest => dest.Salutation, src => src.MapFrom(x => x.Salutation))
                .ForMember(dest => dest.Position, src => src.MapFrom(x => x.Position))
                .ForMember(dest => dest.ContactPersonName, src => src.MapFrom(x => x.ContactName))
                .ForMember(dest => dest.Landline, src => src.MapFrom(x => x.TelephoneNumber))
                .ForMember(dest => dest.Fax, src => src.MapFrom(x => x.FaxNumber))
                .ForMember(dest => dest.Mobile, src => src.MapFrom(x => x.MobileNumber))
                .ForMember(dest => dest.Email, src => src.MapFrom(x => x.EmailAddress))
                .ForMember(dest => dest.OtherDetail, src => src.MapFrom(x => x.OtherContactDetails))
                .ForMember(dest => dest.LastModifiedOn, src => src.MapFrom(x => x.LastModification))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.LogonName, src => src.MapFrom(x => x.LoginName));

            CreateMap<DomainModel.Contact, DbModel.CustomerContact>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.RecordStatus.IsRecordStatusNew() ? 0 : x.ContactId))
                .ForMember(dest => dest.CustomerAddressId, src => src.MapFrom(x => x.CustomerAddressId))
                .ForMember(dest => dest.Salutation, src => src.MapFrom(x => x.Salutation))
                .ForMember(dest => dest.Position, src => src.MapFrom(x => x.Position))
                .ForMember(dest => dest.ContactName, src => src.MapFrom(x => x.ContactPersonName))
                .ForMember(dest => dest.TelephoneNumber, src => src.MapFrom(x => x.Landline))
                .ForMember(dest => dest.FaxNumber, src => src.MapFrom(x => x.Fax))
                .ForMember(dest => dest.MobileNumber, src => src.MapFrom(x => x.Mobile))
                .ForMember(dest => dest.EmailAddress, src => src.MapFrom(x => x.Email))
                .ForMember(dest => dest.OtherContactDetails, src => src.MapFrom(x => x.OtherDetail))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => String.IsNullOrEmpty(x.RecordStatus) ? x.LastModifiedOn : DateTime.UtcNow))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.LoginName, src => src.MapFrom(x => x.LogonName))
                .ForMember(dest => dest.Assignment, src => src.Ignore())
                .ForMember(dest => dest.ContractDefaultCustomerContractContact, src => src.Ignore())
                .ForMember(dest => dest.ContractDefaultCustomerInvoiceContact, src => src.Ignore())
                .ForMember(dest => dest.ProjectClientNotification, src => src.Ignore())
                .ForMember(dest => dest.ProjectCustomerContact, src => src.Ignore())
                .ForMember(dest => dest.ProjectCustomerProjectContact, src => src.Ignore());

            CreateMap<DbModel.CustomerNote, DomainModel.CustomerNote>()
                  .ForMember(dest => dest.Note, src => src.MapFrom(x => x.Note))
                  .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => x.CreatedBy))
                  .ForMember(dest => dest.CreatedOn, src => src.MapFrom(x => x.CreatedDate))
                  .ForMember(dest => dest.LastModifiedOn, src => src.MapFrom(x => x.LastModification))
                  .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                  .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                  .ForMember(dest => dest.CustomerNoteId, src => src.MapFrom(x => x.Id))
                  .ReverseMap();

            CreateMap<DomainModel.CustomerNote, DbModel.CustomerNote>()
                .ForMember(dest => dest.Note, src => src.MapFrom(x => x.Note))
                .ForMember(dest => dest.CreatedBy, src => src.MapFrom(x => x.CreatedBy))
                .ForMember(dest => dest.CreatedDate, src => src.MapFrom(x => x.CreatedOn))
                .ForMember(dest => dest.LastModification, src => src.MapFrom(x => String.IsNullOrEmpty(x.RecordStatus) ? x.LastModifiedOn : DateTime.UtcNow))
                .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.UpdateCount))
                .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.ModifiedBy))
                .ForMember(dest => dest.Id, src => src.MapFrom(x => x.RecordStatus.IsRecordStatusNew() ? 0 : x.CustomerNoteId))
                .ForMember(dest => dest.Customer, src => src.Ignore());

            CreateMap<DbModel.Customer, DomainModel.CustomerDetail>()
                .ForMember(dest => dest.Detail, src => src.MapFrom(x => x))
                .ForMember(dest => dest.Addresses, src => src.MapFrom(x => x.CustomerAddress))
                .ForMember(dest => dest.AssignmentReferences, src => src.MapFrom(x => x.CustomerAssignmentReferenceType))
                .ForMember(dest => dest.AccountReferences, src => src.MapFrom(x => x.CustomerCompanyAccountReference))
               .ForMember(dest => dest.Notes, src => src.MapFrom(x => x.CustomerNote));
                



            CreateMap<DomainModel.CustomerDetail, DbModel.Customer>()
                   .ForMember(dest => dest.Code, src => src.MapFrom(x => string.Format("{0}{1}", x.Detail.CustomerName.Substring(0, 2).ToUpper(), x.Detail.MIIWAId.ToString("D5"))))
                   .ForMember(dest => dest.Name, src => src.MapFrom(x => x.Detail.CustomerName))
                   .ForMember(dest => dest.ParentName, src => src.MapFrom(x => x.Detail.ParentCompanyName))
                   .ForMember(dest => dest.Miiwaid, src => src.MapFrom(x => x.Detail.MIIWAId))
                   .ForMember(dest => dest.MiiwaparentId, src => src.MapFrom(x => x.Detail.MIIWAParentId))
                   .ForMember(dest => dest.IsActive, src => src.MapFrom(x => x.Detail.Active))
                   .ForMember(dest => dest.UpdateCount, src => src.MapFrom(x => x.Detail.UpdateCount))
                   .ForMember(dest => dest.ModifiedBy, src => src.MapFrom(x => x.Detail.ModifiedBy))
                   .ForMember(dest => dest.LastModification, src => src.MapFrom(x => DateTime.UtcNow))
                   .ForMember(dest => dest.Contract, src => src.Ignore())
                   .ForMember(dest => dest.CustomerAssignmentReferenceType, src => src.MapFrom(x => x.AssignmentReferences))
                   .ForMember(dest => dest.CustomerCompanyAccountReference, src => src.MapFrom(x => x.AccountReferences))
                   .ForMember(dest => dest.CustomerNote, src => src.MapFrom(x => x.Notes))
                   .ForMember(dest => dest.CustomerAddress, src => src.MapFrom(x => x.Addresses));
            


            //Customer Mongo Document Search
            CreateMap<CustomerSearch, Document.Domain.Models.Document.EvolutionMongoDocumentSearch>()
                .ForMember(dest => dest.ModuleCode, src => src.ResolveUsing(x => Common.Enums.ModuleCodeType.CUST.ToString()))
                .ForMember(dest => dest.ReferenceCode, src => src.MapFrom(x => x.CustomerCode))
                .ForMember(dest => dest.Text, src => src.ResolveUsing<SearchTextFormatResolver,string>("DocumentSearchText"))
                .ForMember(dest => dest.DocumentTypes, src => src.ResolveUsing(x => string.IsNullOrEmpty(x.SearchDocumentType) ? null : new List<string>() { x.SearchDocumentType }))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }

    public class CityIdResolver : IValueResolver<DomainModel.CustomerAddress, DbModel.CustomerAddress, int?>
    {
        private readonly ICustomerRepository _customerRepository = null;

        public CityIdResolver(ICustomerRepository customerRepository)
        {
            this._customerRepository = customerRepository;
        }

        public int? Resolve(DomainModel.CustomerAddress source, DbModel.CustomerAddress destination, int? destMember, ResolutionContext context)
        {
            return this._customerRepository.GetCityIdForName(source.City,source.County);
        }
    }

    public class AssignmentReferenceTypeResolver : IValueResolver<DomainModel.CustomerAssignmentReference, DbModel.CustomerAssignmentReferenceType, int>
    {
        private readonly ICustomerRepository _customerRepository = null;

        public AssignmentReferenceTypeResolver(ICustomerRepository customerRepository)
        {
            this._customerRepository = customerRepository;
        }

        public int Resolve(DomainModel.CustomerAssignmentReference source, DbModel.CustomerAssignmentReferenceType destination, int destMember, ResolutionContext context)
        {
            return this._customerRepository.GetAssignmentReferenceIdForAssignmentRefferenceType(source.AssignmentRefType);
        }
    }

    public class CompanyIdResolver : IValueResolver<DomainModel.CustomerCompanyAccountReference, DbModel.CustomerCompanyAccountReference, int>
    {
        private readonly ICustomerRepository _customerRepository = null;

        public CompanyIdResolver(ICustomerRepository customerRepository)
        {
            this._customerRepository = customerRepository;
        }

        public int Resolve(DomainModel.CustomerCompanyAccountReference source, DbModel.CustomerCompanyAccountReference destination, int destMember, ResolutionContext context)
        {
            return this._customerRepository.GetCompanyIdForCompanyCode(source.CompanyCode);
        }
    }
}
