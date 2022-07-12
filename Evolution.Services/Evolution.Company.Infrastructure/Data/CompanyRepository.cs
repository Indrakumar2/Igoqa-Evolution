using AutoMapper;
using Evolution.Common.Extensions;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Company.Domain.Models;

namespace Evolution.Company.Infrastructure.Data
{
    public class CompanyRepository : GenericRepository<DbModel.Company>, ICompanyRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public CompanyRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        //public IList<DomainModel.Companies.Company> Search(DomainModel.Companies.CompanySearch model)
        //{
        //    var dbSearchModel = this._mapper.Map<DbModel.Company>(model);
        //    IQueryable<DbModel.Company> whereClause = this._dbContext.Company;

        //    if (model?.CompanyCodes?.Count > 0)
        //        whereClause = whereClause.Where(x => model.CompanyCodes.Contains(x.Code));

        //    var expression = dbSearchModel.ToExpression();
        //    if (expression == null)
        //        return whereClause.ProjectTo<DomainModel.Companies.Company>().OrderBy(x1 => x1.CompanyName).ToList();
        //    else
        //        return whereClause.Where(expression).ProjectTo<DomainModel.Companies.Company>().OrderBy(x1 => x1.CompanyName).ToList();
        //}

        public IList<DomainModel.Companies.Company> Search(DomainModel.Companies.CompanySearch model)
        {
            IQueryable<DbModel.Company> whereClause = GetCompanyCompiled(model)?.AsQueryable();
            if (model?.CompanyCodes?.Count > 0)
                whereClause = whereClause.Where(x => model.CompanyCodes.Contains(x.Code));

            return whereClause.Select(x => new DomainModel.Companies.Company
            {
                Id = x.Id,
                CompanyCode = x.Code,
                CompanyName = x.Name,
                InvoiceName = x.InvoiceCompanyName,
                Currency = x.NativeCurrency,
                SalesTaxDescription = x.SalesTaxDescription,
                WithholdingTaxDescription = x.WithholdingTaxDescription,
                InterCompanyExpenseAccRef = x.InterCompanyExpenseAccRef,
                InterCompanyRateAccRef = x.InterCompanyRateAccRef,
                CompanyMiiwaid = x.CompanyMiiwaid,
                GfsBu = x.GfsBu,
                GfsCoa = x.GfsCoa,
                CompanyMiiwaref = x.CompanyMiiwaref,
                OperatingCountry =x.OperatingCountry, //IGO QC D-906
                OperatingCountryName = x.OperatingCountryNavigation.Name, //IGO QC D-906
                IsActive = x.IsActive,
                IsUseIctms = x.IsUseIctms,
                IsFullUse = x.IsFullUse,
                Region = x.Region,
                IsCOSEmailOverrideAllow = x.IsCostOfSalesEmailOverrideAllow,
                AvgTSHourlyCost = x.AverageTshourlycost,
                VatTaxRegNo = x.VatTaxRegistrationNo,
                EUVatPrefix = x.Euvatprefix,
                IARegion = x.Iaregion,
                CognosNumber = x.CognosNumber,
                UpdateCount = x.UpdateCount,
                ModifiedBy = x.ModifiedBy,
                LastModification = x.LastModification,
                LogoText = x.Logo != null ? x.Logo.Name : null,
                ResourceOutsideDistance = x.ResourceOutsideDistance,
                VATRegulationTextWithinEC = x.VatregTextWithinEc, // CR560
                VATRegulationTextOutsideEC = x.VatregTextOutsideEc // CR560
            })?.OrderBy(x1 => x1.CompanyName).ToList();
        }

        private static Func<EvolutionSqlDbContext, DomainModel.Companies.CompanySearch, IEnumerable<DbModel.Company>> _getCompanyCompiled;

        private static Func<EvolutionSqlDbContext, DomainModel.Companies.CompanySearch, IEnumerable<DbModel.Company>> _getFetchCompanyCompiled;

        public IEnumerable<DbModel.Company> GetCompanyCompiled(DomainModel.Companies.CompanySearch model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.Company>(model);
            var expression = dbSearchModel.ToExpression();

            if (expression != null)
            {
                _getCompanyCompiled = EF.CompileQuery<EvolutionSqlDbContext, DomainModel.Companies.CompanySearch, IEnumerable<DbModel.Company>>((dbContext, company) =>
                  dbContext.Company.Include(x => x.OperatingCountryNavigation).Include(x => x.Logo).Where(expression));
            }
            else
            {
                _getCompanyCompiled = EF.CompileQuery<EvolutionSqlDbContext, DomainModel.Companies.CompanySearch, IEnumerable<DbModel.Company>>((dbContext, company) =>
                    dbContext.Company.Include(x => x.OperatingCountryNavigation).Include(x => x.Logo));
            }
            return _getCompanyCompiled(_dbContext, model).Select(x => new DbModel.Company
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                InvoiceCompanyName = x.InvoiceCompanyName,
                NativeCurrency = x.NativeCurrency,
                SalesTaxDescription = x.SalesTaxDescription,
                WithholdingTaxDescription = x.WithholdingTaxDescription,
                InterCompanyExpenseAccRef = x.InterCompanyExpenseAccRef,
                InterCompanyRateAccRef = x.InterCompanyRateAccRef,
                CompanyMiiwaid = x.CompanyMiiwaid,
                GfsCoa = x.GfsCoa,
                GfsBu = x.GfsBu,
                CompanyMiiwaref = x.CompanyMiiwaref,
                OperatingCountry = x.OperatingCountry,
                OperatingCountryNavigation =x.OperatingCountryNavigation,
                IsActive = x.IsActive,
                IsUseIctms = x.IsUseIctms,
                IsFullUse = x.IsFullUse,
                Region = x.Region,
                IsCostOfSalesEmailOverrideAllow = x.IsCostOfSalesEmailOverrideAllow,
                AverageTshourlycost = x.AverageTshourlycost,
                VatTaxRegistrationNo = x.VatTaxRegistrationNo,
                Euvatprefix = x.Euvatprefix,
                Iaregion = x.Iaregion,
                CognosNumber = x.CognosNumber,
                UpdateCount = x.UpdateCount,
                ModifiedBy = x.ModifiedBy,
                LastModification = x.LastModification,
                Logo = x.Logo,
                LogoId = x.LogoId,
                ResourceOutsideDistance = x.ResourceOutsideDistance,
                VatregTextWithinEc = x.VatregTextWithinEc, // CR560
                VatregTextOutsideEc = x.VatregTextOutsideEc // CR560
            });
        }

        public IEnumerable<DbModel.Company> GetFetchCompanyCompiled(DomainModel.Companies.CompanySearch model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.Company>(model);
            var expression = dbSearchModel.ToExpression();
            if (expression != null)
            {
                _getFetchCompanyCompiled = EF.CompileQuery<EvolutionSqlDbContext, DomainModel.Companies.CompanySearch, IEnumerable<DbModel.Company>>((dbContext, company) =>
                  dbContext.Company.Where(expression));
            }
            else
            {
                _getFetchCompanyCompiled = EF.CompileQuery<EvolutionSqlDbContext, DomainModel.Companies.CompanySearch, IEnumerable<DbModel.Company>>((dbContext, company) =>
                    dbContext.Company);
            }
            return _getFetchCompanyCompiled(_dbContext, model).Select(x => new DbModel.Company
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                NativeCurrency = x.NativeCurrency,
                IsActive = x.IsActive
            });
        }

        public IList<DomainModel.Companies.Company> GetCompanyList(DomainModel.Companies.CompanySearch model)
        {
            IQueryable<DbModel.Company> whereClause = GetFetchCompanyCompiled(model)?.AsQueryable();
            return whereClause.Select(x => new DomainModel.Companies.Company
            {
                Id = x.Id,
                CompanyCode = x.Code,
                CompanyName = x.Name,
                Currency = x.NativeCurrency,
                IsActive = x.IsActive,
            })?.OrderBy(x1 => x1.CompanyName).ToList();
        }
    }
}
