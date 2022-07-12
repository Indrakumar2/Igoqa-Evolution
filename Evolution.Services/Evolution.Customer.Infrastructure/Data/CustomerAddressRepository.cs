using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.GenericDbRepository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Customer.Domain.Models;

namespace Evolution.Customer.Infrastructure.Data
{
    public class CustomerAddressRepository : GenericRepository<DbModel.CustomerAddress>, ICustomerAddressRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;

        public CustomerAddressRepository(DbModel.EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public IList<DomainModel.Customers.CustomerAddress> Search(DomainModel.Customers.CustomerAddress model)
        {

            if (this._dbContext == null)
                throw new System.InvalidOperationException("Datacontext is not intitialized.");

            return   _dbContext.CustomerAddress
                        .GroupJoin(_dbContext.Customer,
                                add => add.CustomerId,
                                cust => cust.Id,
                                (address, customer) => new { address, customer })
                                    .SelectMany(x => x.customer.DefaultIfEmpty(), (add, customer) => new { add.address, customer })
                        .GroupJoin(_dbContext.City,
                                left => left.address.CityId,
                                city => city.Id,
                                (left1, city) => new { left1.customer, left1.address, city })
                                   .SelectMany(x => x.city.DefaultIfEmpty(), (left, city) => new { left.address, left.customer, city })
                        .GroupJoin(_dbContext.County,
                                left => left.city.CountyId,
                                county => county.Id,
                                (left1, county) => new { left1.address, left1.customer, left1.city, county })
                                    .SelectMany(x => x.county.DefaultIfEmpty(), (left, county) => new { left.address, left.customer, left.city, county })
                        .GroupJoin(_dbContext.Country,
                                left => left.county.CountryId,
                                country => country.Id,
                                (left1, country) => new { left1.address, left1.customer, left1.city, left1.county, country })
                                    .SelectMany(x => x.country.DefaultIfEmpty(), (left, country) => new { left.address, left.customer, left.city, left.county, country })
                        .Where(x => (string.IsNullOrEmpty(model.Address) || x.address.Address == model.Address)
                                    && (string.IsNullOrEmpty(model.EUVatPrefix) || x.address.Euvatprefix == model.EUVatPrefix)
                                    && (string.IsNullOrEmpty(model.VatTaxRegNumber) || x.address.VatTaxRegistrationNo == model.VatTaxRegNumber)
                                    && (string.IsNullOrEmpty(model.PostalCode) || x.address.PostalCode == model.PostalCode)
                                    && (string.IsNullOrEmpty(model.City) || x.city.Name == model.City)
                                    && (string.IsNullOrEmpty(model.County) || x.county.Name == model.County)
                                    && (string.IsNullOrEmpty(model.Country) || x.country.Name == model.Country)
                                    && (string.IsNullOrEmpty(model.CustomerCode) || x.customer.Code == model.CustomerCode))
                         .Select(x => new DomainModel.Customers.CustomerAddress()
                        {
                            AddressId = x.address.Id,
                            Address = x.address.Address,
                            EUVatPrefix = x.address.Euvatprefix,
                            VatTaxRegNumber = x.address.VatTaxRegistrationNo,
                            PostalCode = x.address.PostalCode,
                            CustomerCode = x.customer.Code,
                            City = x.city.Name,
                            County = x.county.Name,
                            Country = x.country.Name,
                            CityId = x.city.Id,
                            StateId = x.county.Id,
                            CountryId = x.country.Id,
                            ModifiedBy = x.address.ModifiedBy,
                            UpdateCount = x.address.UpdateCount
                        }).ToList();

        }
        /* Added for Assignment Dropdown clean up*/
        public IList<DomainModel.Customers.CustomerAddress> SearchAddress(string customerCode)
        {
            return _dbContext.CustomerAddress.Where(x => x.Customer.Code == customerCode)?.Select(x =>
                 new DomainModel.Customers.CustomerAddress
                 {
                     AddressId = x.Id,
                     Address = x.Address,
                     CustomerCode = x.Customer.Code,
                     LastModifiedOn = x.LastModification,
                     ModifiedBy = x.ModifiedBy,
                     UpdateCount = x.UpdateCount

                 })?.ToList();
        }
    }
}
