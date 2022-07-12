using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.Logging.Interfaces;
using Evolution.Supplier.Domain.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Supplier.Domain.Models.Supplier;
using AutoMapper.QueryableExtensions;

namespace Evolution.Contract.Infrastructure.Data
{
    public class SupplierRepository : GenericRepository<DbModel.Supplier>, ISupplierRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IAppLogger<SupplierRepository> _logger = null;
        private readonly IMapper _mapper = null;

        public SupplierRepository(EvolutionSqlDbContext dbContext, IMapper mapper, IAppLogger<SupplierRepository> logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._logger = logger;
        }

        // public IList<DbModel.Supplier> Search(DomainModel.SupplierSearch model, params Expression<Func<DbModel.Supplier, object>>[] includes)
        // {
        //     var supplierAsQueryable = this.PopulateSupplierAsQuerable(model);
        //     return this.Get(supplierAsQueryable, null, null, includes);
        // }
        public IList<DomainModel.Supplier> Search(DomainModel.SupplierSearch model, params Expression<Func<DbModel.Supplier, object>>[] includes)
        {
            var suppAsQuerable = this.PopulateSupplierAsQuerable(model); 
            
            if (includes.Any())
                suppAsQuerable = includes.Aggregate(suppAsQuerable, (current, include) => current.Include(include));

            return suppAsQuerable.ProjectTo<DomainModel.Supplier>().ToList();
        }

        public IList<DbModel.Supplier> Get(IList<int> ids, params Expression<Func<DbModel.Supplier, object>>[] includes)
        {
            return Get(null, ids, null, includes);
        }

        public IList<DbModel.Supplier> Get(IList<string> names, params Expression<Func<DbModel.Supplier, object>>[] includes)
        {
            return Get(null, null, names, includes);
        }

        private IList<DbModel.Supplier> Get(IQueryable<DbModel.Supplier> suppAsQuerable = null,
                                            IList<int> ids = null,
                                            IList<string> supplierNames = null,
                                            params Expression<Func<DbModel.Supplier, object>>[] includes)
        {
            if (suppAsQuerable == null)
                suppAsQuerable = _dbContext.Supplier;

            if (includes.Any())
                suppAsQuerable = includes.Aggregate(suppAsQuerable, (current, include) => current.Include(include));

            if (ids?.Count > 0)
                suppAsQuerable = suppAsQuerable.Where(x => ids.Contains(x.Id));
            if (supplierNames?.Count > 0)
                suppAsQuerable = suppAsQuerable.Where(x => supplierNames.Contains(x.SupplierName.ToString()));

            return suppAsQuerable.ToList();
        }

        private IQueryable<DbModel.Supplier> PopulateSupplierAsQuerable(DomainModel.SupplierSearch model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.Supplier>(model);
            IQueryable<DbModel.Supplier> supplierAsQueryable = _dbContext.Supplier;

            if (model.SupplierIds?.Count > 0)
                supplierAsQueryable = supplierAsQueryable.Where(x => model.SupplierIds.Contains(x.Id.ToString()));            

            #region Wildcard Search for Supplier
            if (model.SupplierName.HasEvoWildCardChar())
                supplierAsQueryable = supplierAsQueryable.WhereLike(x => x.SupplierName, model.SupplierName, '*');
            else if (!string.IsNullOrEmpty(model.SupplierName))
                supplierAsQueryable = supplierAsQueryable.Where(x => x.SupplierName == model.SupplierName);
            #endregion

            #region Wildcard Search for Supplier Address
            if (model.SupplierAddress.HasEvoWildCardChar())
                supplierAsQueryable = supplierAsQueryable.WhereLike(x => x.Address, model.SupplierAddress, '*');
            else if (!string.IsNullOrEmpty(model.SupplierAddress))
                supplierAsQueryable = supplierAsQueryable.Where(x => x.Address == model.SupplierAddress);
            #endregion

            #region Wildcard Search for County
            if (model.Country.HasEvoWildCardChar())
                supplierAsQueryable = supplierAsQueryable.WhereLike(x => x.Country.Name, model.Country, '*');
            else if(model.CountryId != null && model.CountryId.HasValue)
                supplierAsQueryable = supplierAsQueryable.Where(x => x.CountryId == model.CountryId); //Changes for D1536(IGO 983)
            else if (!string.IsNullOrEmpty(model.Country))
                supplierAsQueryable = supplierAsQueryable.Where(x => x.Country.Name == model.Country); 
            #endregion

            #region Wildcard Search for State
            if (model.State.HasEvoWildCardChar())
                supplierAsQueryable = supplierAsQueryable.WhereLike(x => x.County.Name, model.State, '*');
            else if (!string.IsNullOrEmpty(model.State))
                supplierAsQueryable = supplierAsQueryable.Where(x => x.CountyId == model.StateId);
            #endregion

            #region Wildcard Search for City
            if (model.City.HasEvoWildCardChar())
                supplierAsQueryable = supplierAsQueryable.WhereLike(x => x.City.Name, model.City, '*');
            else if (!string.IsNullOrEmpty(model.City))
                supplierAsQueryable = supplierAsQueryable.Where(x => x.CityId == model.CityId);
            #endregion

            var defWhereExpr = dbSearchModel.ToExpression();
            if (defWhereExpr != null)
                supplierAsQueryable = supplierAsQueryable.Where(defWhereExpr);

            return supplierAsQueryable;
        }

        public int DeleteSupplier(int supplierId)
        {
            int count = -1;
            try
            {
                var deleteStatement = Utility.GetSqlQuery(SQLModuleType.Supplier_Detail, SQLModuleActionType.Delete);
                count = _dbContext.Database.ExecuteSqlCommand(deleteStatement, supplierId);

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "SupplierId=" + supplierId);
            }

            return count;
        }
    }
}